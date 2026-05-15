using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SecureScanUI
{
    public partial class Form1 : Form
    {
        private int totalFilesScanned = 0;
        private int threatsFound = 0;
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "virus_db.db");
        private string connString => $"Data Source={dbPath};Version=3;Journal Mode=WAL;Pooling=True;";

        
        private const string VT_API_KEY = "2bdf2549758b08c5c85f49a2b51726e2f40c5e7e3d6cd4eda45f9f761f441500";

        public Form1()
        {
            InitializeComponent();
            SetupPowerfulDatabase();
            InitializeCustomSettings();
        }

        private void InitializeCustomSettings()
        {
            pbScanProgress.Value = 0;
            pbScanProgress.Visible = false;
            lblPercentage.Text = "0%";
            lblPercentage.Visible = false;

            pnlHistory.Visible = false;
            pnlContent.Visible = true;
            pnlContent.Dock = DockStyle.Fill;
            pnlHistory.Dock = DockStyle.Fill;
            pnlContent.BringToFront();

            UpdateStatus("System Ready", Color.DarkGreen);
        }

        private void UpdateStatus(string message, Color color)
        {
            lblStatusText.Text = message;
            lblStatusText.ForeColor = color;
        }

        // --- DATABASE METHODS ---
        private void SetupPowerfulDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Signatures (id INTEGER PRIMARY KEY AUTOINCREMENT, md5_hash TEXT UNIQUE NOT NULL, virus_name TEXT NOT NULL, virus_type TEXT, severity_level TEXT);
                    CREATE INDEX IF NOT EXISTS idx_hash ON Signatures(md5_hash);
                    CREATE TABLE IF NOT EXISTS ScanHistory (id INTEGER PRIMARY KEY AUTOINCREMENT, file_name TEXT NOT NULL, file_path TEXT, scan_date DATETIME DEFAULT CURRENT_TIMESTAMP, status TEXT, detected_virus TEXT);";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn)) { cmd.ExecuteNonQuery(); }
            }
        }

        private void SaveToHistory(string fileName, string filePath, string result)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO ScanHistory (file_name, file_path, status) VALUES (@name, @path, @res)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", fileName);
                        cmd.Parameters.AddWithValue("@path", filePath);
                        cmd.Parameters.AddWithValue("@res", result);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private void LoadScanHistory()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT file_name AS 'File Name', status AS 'Status', scan_date AS 'Date' FROM ScanHistory ORDER BY id DESC";
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvHistory.DataSource = dt;
                        dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("History Load Error: " + ex.Message); }
        }

        // --- SCANNING CORE LOGIC ---
        private string GetMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private async Task<(bool IsMalicious, string Message)> ScanWithVirusTotal(string fileHash)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-apikey", VT_API_KEY);
                    string url = $"https://www.virustotal.com/api/v3/files/{fileHash}";
                    var response = await client.GetAsync(url);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return (false, "Cloud: Unknown (New File)");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                        {
                            var stats = doc.RootElement.GetProperty("data").GetProperty("attributes").GetProperty("last_analysis_stats");
                            int maliciousCount = stats.GetProperty("malicious").GetInt32();
                            if (maliciousCount > 0) return (true, $"Hybrid Alert: {maliciousCount} Engines detected threat!");
                        }
                    }
                }
            }
            catch (Exception ex) { return (false, "Cloud Scan Error: " + ex.Message); }
            return (false, "Cloud Verified: Clean");
        }

        private (bool Found, string VirusName, string VirusType) CheckForThreat(string hash)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT virus_name, virus_type FROM Signatures WHERE md5_hash = @hash LIMIT 1";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hash", hash);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) return (true, reader["virus_name"].ToString(), reader["virus_type"].ToString());
                        }
                    }
                }
            }
            catch { }
            return (false, "", "");
        }

        private async Task ScanSingleFile(string filePath)
        {
            try
            {
                pbScanProgress.Visible = true;
                pbScanProgress.Value = 50;
                string fileHash = GetMD5Hash(filePath);
                string fileName = Path.GetFileName(filePath);

                UpdateStatus("Checking Local & Cloud...", Color.Blue);

                var localThreat = CheckForThreat(fileHash);

                if (localThreat.Found)
                {
                   
                    UpdateStatus($"ALERT: {localThreat.VirusName}!", Color.Red);
                    SaveToHistory(fileName, filePath, $"Infected (Local)");
                    MessageBox.Show($"Threat Detected: {localThreat.VirusName}", "Security Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                   
                    var cloudResult = await ScanWithVirusTotal(fileHash);
                    UpdateStatus(cloudResult.Message, cloudResult.IsMalicious ? Color.Red : Color.DarkGreen);
                    SaveToHistory(fileName, filePath, cloudResult.Message);

                  
                    if (cloudResult.IsMalicious)
                    {
                        MessageBox.Show(cloudResult.Message, "Cloud Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Cloud Verified: File is Safe!", "Security Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Scan Error: " + ex.Message);
            }
            finally
            {
                pbScanProgress.Visible = false;
            }
        }

        private void ScanDirectoryRecursive(string path)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    totalFilesScanned++;
                    string hash = GetMD5Hash(file);
                    var result = CheckForThreat(hash);
                    if (result.Found)
                    {
                        threatsFound++;
                        SaveToHistory(Path.GetFileName(file), file, $"Infected: {result.VirusName}");
                    }
                    if (totalFilesScanned % 10 == 0)
                        this.Invoke(new Action(() =>
                        {
                            pbScanProgress.Value = (totalFilesScanned % 100);
                            lblPercentage.Text = $"Scanned: {totalFilesScanned}";
                        }));
                }
                foreach (string dir in Directory.GetDirectories(path)) ScanDirectoryRecursive(dir);
            }
            catch { }
        }

        
        private async void btnQuickScan_Click(object sender, EventArgs e)
        {
            totalFilesScanned = 0;
            threatsFound = 0;
            UpdateStatus("Quick Scan Started...", Color.Orange);
            pbScanProgress.Visible = true;
            lblPercentage.Visible = true;
            btnQuickScan.Enabled = false;

            
            string[] paths = {
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
    };

            await Task.Run(() =>
            {
                foreach (var p in paths)
                {
                    if (Directory.Exists(p))
                    {
                        this.Invoke(new Action(() => UpdateStatus($"Scanning: {Path.GetFileName(p)}", Color.Blue)));
                        ScanDirectoryRecursive(p);
                    }
                }
            });

            UpdateStatus("Quick Scan Finished!", Color.DarkGreen);
            btnQuickScan.Enabled = true;
            pbScanProgress.Visible = false;
            MessageBox.Show($"Quick Scan Complete!\n\nFiles Scanned: {totalFilesScanned}\nThreats Found: {threatsFound}");
        }
        private void btnHistory_Click(object sender, EventArgs e)
        {
            pnlContent.Visible = false;
            pnlHistory.Visible = true;
            pnlHistory.BringToFront();
            LoadScanHistory();
        }

        private void btnGlobalBack_Click(object sender, EventArgs e)
        {
           
            pnlHistory.Visible = false;

           
            pnlContent.Visible = true;
            pnlContent.BringToFront();

            UpdateStatus("System Ready", Color.DarkGreen);
        }

       
        private async void btnFullScan_Click(object sender, EventArgs e)
        {
            totalFilesScanned = 0; threatsFound = 0;
            UpdateStatus("Full System Scan Started...", Color.Orange);
            pbScanProgress.Visible = true;

            
            await Task.Run(() => ScanDirectoryRecursive("C:\\"));

            UpdateStatus("Full Scan Finished!", Color.DarkGreen);
            pbScanProgress.Visible = false;
            MessageBox.Show($"Full Scan Complete! Found: {threatsFound}");
        }

        
        private async void btnExternalScan_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    totalFilesScanned = 0; threatsFound = 0;
                    UpdateStatus("External Scan Started...", Color.Orange);
                    await Task.Run(() => ScanDirectoryRecursive(fbd.SelectedPath));
                    UpdateStatus("Scan Completed!", Color.DarkGreen);
                    MessageBox.Show($"Scan Finished! Found: {threatsFound}");
                }
            }
        }

        private async void btnSelectFile_Click(object sender, EventArgs e)

        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                
                ofd.Filter = "All Files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    await ScanSingleFile(ofd.FileName);

                }
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await ScanSingleFile(ofd.FileName);
                    pbScanProgress.Visible = true;
                    pbScanProgress.Style = ProgressBarStyle.Marquee; 
                }
            }
        }

    }
}
