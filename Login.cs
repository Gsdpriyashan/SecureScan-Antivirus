using System;
using System.Data.SQLite;
using System.Drawing; // Placeholder වල වර්ණ සඳහා අවශ්‍යයි
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;

namespace SecureScanUI
{
    public partial class Login : Form
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "virus_db.db");
        private string connString => $"Data Source={dbPath};Version=3;";

        private string generatedCode;
        private string tempUser, tempPass, tempEmail;

        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;

       
       
        public Login()
        {
            InitializeComponent();
            CreateUserTable();

           
            SetupPlaceholders();

            
            if (txtVerifyCode != null) txtVerifyCode.Visible = false;
            if (btnConfirmVerify != null) btnConfirmVerify.Visible = false;
        }

        
        private void SetupPlaceholders()
        {
            SetPlaceholder(txtUsername, "Enter Username");
            SetPlaceholder(txtPassword, "Enter Password");
            SetPlaceholder(txtEmail, "Enter Email (Register)");
            SetPlaceholder(txtVerifyCode, "Enter 6-Digit Code");
        }

        private void SetPlaceholder(TextBox textBox, string placeholderText)
        {
            if (textBox == null) return;

            textBox.Text = placeholderText;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (sender, e) =>
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (textBox == txtPassword) textBox.UseSystemPasswordChar = true;
                }
            };

            textBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox == txtPassword) textBox.UseSystemPasswordChar = false;
                    textBox.Text = placeholderText;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void CreateUserTable()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Users (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    username TEXT UNIQUE, 
                                    password TEXT, 
                                    email TEXT, 
                                    is_verified INTEGER DEFAULT 0);";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn)) { cmd.ExecuteNonQuery(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void SendVerificationEmail(string userEmail)
        {
            Random rnd = new Random();
            generatedCode = rnd.Next(100000, 999999).ToString();

            try
            {
                var fromAddress = new MailAddress("dularasudarshana987@gmail.com", "SecureScan Admin");
                var toAddress = new MailAddress(userEmail);
                string fromPassword = "junpoecxnebfutkh"; 

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "SecureScan Registration Code",
                    Body = $"Your registration code has been generated. Use the following code to complete your setup: {generatedCode}"
                })
                {
                    smtp.Send(message);
                }
                MessageBox.Show("Verification Sent: A security code has been dispatched to your registered email address. Please enter it to proceed.");

                txtVerifyCode.Visible = true;
                btnConfirmVerify.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Email Error: " + ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            
            if (txtEmail.Visible == false)
            {
                
                txtEmail.Visible = true;
                btnRegister.Text = "Confirm Registration"; 
                btnLogin.Visible = false;
                txtEmail.Focus();
                return; 
            }

           
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                txtUsername.Text == "Enter Username" ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Missing Details: Please fill in all required information.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            tempUser = txtUsername.Text;
            tempPass = txtPassword.Text;
            tempEmail = txtEmail.Text;

            
            SendVerificationEmail(tempEmail);

            MessageBox.Show("Verification Code Sent to your Email!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

           
        {

                
                if (picCamera.Image != null)
                {
                    try
                    {
                        string folderPath = Path.Combine(Application.StartupPath, "UserFaces");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                       
                        using (Bitmap bmp = new Bitmap((Bitmap)picCamera.Image.Clone()))
                        {
                            string fileName = txtUsername.Text + ".jpg";
                            string fullPath = Path.Combine(folderPath, fileName);

                            if (File.Exists(fullPath))
                            {
                               
                                try { File.Delete(fullPath); } catch { }
                            }

                            bmp.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        MessageBox.Show("Face Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("කරුණාකර කැමරාව ක්‍රියාත්මක කර පින්තූරයක් පෙනෙන්නට සලස්වන්න.");
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
           
        
            
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count > 0)
            {
                
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);

               
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

                
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("කැමරාව සොයාගත නොහැක!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public float CompareImages(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1.Size != bmp2.Size)
            {
                
                bmp2 = new Bitmap(bmp2, bmp1.Size);
            }

            int equalPixels = 0;
            int totalPixels = bmp1.Width * bmp1.Height;

            for (int x = 0; x < bmp1.Width; x++)
            {
                for (int y = 0; y < bmp1.Height; y++)
                {
                    if (bmp1.GetPixel(x, y) == bmp2.GetPixel(x, y))
                        equalPixels++;
                }
            }
            return (float)equalPixels / totalPixels * 100;
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                picCamera.Image = bitmap;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void btnFaceUnlock_Click(object sender, EventArgs e)
        {
            picCamera.Visible = true;

            
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username first!", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string savedPath = Path.Combine(Application.StartupPath, "UserFaces", txtUsername.Text + ".jpg");

            if (File.Exists(savedPath) && picCamera.Image != null)
            {
                Bitmap currentFrame = new Bitmap(picCamera.Image);
                Bitmap savedFrame = new Bitmap(savedPath);

                float similarity = CompareImages(currentFrame, savedFrame);

                
                if (similarity > 80)
                {
                    MessageBox.Show("Face Verified! Access Granted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Face Not Recognized! Access Denied.", "Security Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please register your face first or enter username.");
            }
        }
        

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
          
            Login loginForm = new Login();
            loginForm.Show();

            
            this.Hide();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        
        {
           
            if (e.KeyCode == Keys.Enter)
            {
                
                txtPassword.Focus();

               
                e.SuppressKeyPress = true;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                
                e.SuppressKeyPress = true;

                
                btnLogin.PerformClick();
            }
        }
        

        private void btnConfirmVerify_Click(object sender, EventArgs e)
        {
            if (txtVerifyCode.Text == generatedCode)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        string sql = "INSERT INTO Users (username, password, email, is_verified) VALUES (@user, @pass, @email, 1)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@user", tempUser);
                            cmd.Parameters.AddWithValue("@pass", tempPass);
                            cmd.Parameters.AddWithValue("@email", tempEmail);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Registration Successful! Your account has been created. You may now proceed to login.");
                    txtVerifyCode.Visible = false;
                    btnConfirmVerify.Visible = false;
                }
                catch { MessageBox.Show("Username Unavailable: This username is already in use. Please choose a different one"); }
            }
            else
            {
                MessageBox.Show("Invalid Authentication Code: The code you entered does not match. Please check and try again.");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Enter Username" || txtPassword.Text == "Enter Password")
            {
                MessageBox.Show("Entry Required: Please provide your login details.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Users WHERE username=@user AND password=@pass";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Authentication Successful! Welcome to SecureScan. Your dashboard is now ready.");
                        this.Hide();
                        Form1 mainForm = new Form1();
                        mainForm.Show();
                    }
                    else { MessageBox.Show("Authentication Failed: The credentials provided do not match our records."); }
                }
            }
        }
    }
}