namespace SecureScanUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnDashboard = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblKasperskyStatus = new System.Windows.Forms.Label();
            this.lblCloudConfidence = new System.Windows.Forms.Label();
            this.btnFullScan = new System.Windows.Forms.Button();
            this.lblAvastStatus = new System.Windows.Forms.Label();
            this.btnQuickScan = new System.Windows.Forms.Button();
            this.lblBitdefenderStatus = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHistory = new System.Windows.Forms.Panel();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnGlobalBack = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.btnHistory = new System.Windows.Forms.Button();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.pbScanProgress = new System.Windows.Forms.ProgressBar();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.lblStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExternalScan = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.pnlHistory.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDashboard
            // 
            this.btnDashboard.Location = new System.Drawing.Point(0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(75, 23);
            this.btnDashboard.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 0;
            // 
            // lblKasperskyStatus
            // 
            this.lblKasperskyStatus.AutoSize = true;
            this.lblKasperskyStatus.Location = new System.Drawing.Point(540, 296);
            this.lblKasperskyStatus.Name = "lblKasperskyStatus";
            this.lblKasperskyStatus.Size = new System.Drawing.Size(88, 16);
            this.lblKasperskyStatus.TabIndex = 4;
            this.lblKasperskyStatus.Text = "Avast: Ready";
            // 
            // lblCloudConfidence
            // 
            this.lblCloudConfidence.AutoSize = true;
            this.lblCloudConfidence.Location = new System.Drawing.Point(518, 309);
            this.lblCloudConfidence.Name = "lblCloudConfidence";
            this.lblCloudConfidence.Size = new System.Drawing.Size(118, 16);
            this.lblCloudConfidence.TabIndex = 5;
            this.lblCloudConfidence.Text = "Kaspersky: Ready";
            // 
            // btnFullScan
            // 
            this.btnFullScan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnFullScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFullScan.Location = new System.Drawing.Point(152, 224);
            this.btnFullScan.Name = "btnFullScan";
            this.btnFullScan.Size = new System.Drawing.Size(89, 60);
            this.btnFullScan.TabIndex = 1;
            this.btnFullScan.Text = "Full System Scan";
            this.btnFullScan.UseVisualStyleBackColor = false;
            this.btnFullScan.Click += new System.EventHandler(this.btnFullScan_Click);
            // 
            // lblAvastStatus
            // 
            this.lblAvastStatus.AutoSize = true;
            this.lblAvastStatus.Location = new System.Drawing.Point(515, 322);
            this.lblAvastStatus.Name = "lblAvastStatus";
            this.lblAvastStatus.Size = new System.Drawing.Size(123, 16);
            this.lblAvastStatus.TabIndex = 6;
            this.lblAvastStatus.Text = "Bitdefender: Ready";
            // 
            // btnQuickScan
            // 
            this.btnQuickScan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnQuickScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuickScan.Location = new System.Drawing.Point(338, 224);
            this.btnQuickScan.Name = "btnQuickScan";
            this.btnQuickScan.Size = new System.Drawing.Size(95, 60);
            this.btnQuickScan.TabIndex = 0;
            this.btnQuickScan.Text = "Quick Scan";
            this.btnQuickScan.UseVisualStyleBackColor = false;
            this.btnQuickScan.Click += new System.EventHandler(this.btnQuickScan_Click);
            // 
            // lblBitdefenderStatus
            // 
            this.lblBitdefenderStatus.AutoSize = true;
            this.lblBitdefenderStatus.Location = new System.Drawing.Point(486, 335);
            this.lblBitdefenderStatus.Name = "lblBitdefenderStatus";
            this.lblBitdefenderStatus.Size = new System.Drawing.Size(158, 16);
            this.lblBitdefenderStatus.TabIndex = 7;
            this.lblBitdefenderStatus.Text = "System Health: Protected";
            // 
            // pnlContent
            // 
            this.pnlContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlContent.Controls.Add(this.pnlHistory);
            this.pnlContent.Controls.Add(this.btnHistory);
            this.pnlContent.Controls.Add(this.lblPercentage);
            this.pnlContent.Controls.Add(this.pbScanProgress);
            this.pnlContent.Controls.Add(this.statusStrip2);
            this.pnlContent.Controls.Add(this.btnExternalScan);
            this.pnlContent.Controls.Add(this.btnSelectFile);
            this.pnlContent.Controls.Add(this.lblBitdefenderStatus);
            this.pnlContent.Controls.Add(this.btnQuickScan);
            this.pnlContent.Controls.Add(this.lblAvastStatus);
            this.pnlContent.Controls.Add(this.btnFullScan);
            this.pnlContent.Controls.Add(this.lblCloudConfidence);
            this.pnlContent.Controls.Add(this.lblKasperskyStatus);
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(677, 441);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlHistory
            // 
            this.pnlHistory.Controls.Add(this.pnlTopBar);
            this.pnlHistory.Controls.Add(this.dgvHistory);
            this.pnlHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHistory.Location = new System.Drawing.Point(0, 0);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(677, 415);
            this.pnlHistory.TabIndex = 15;
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.Controls.Add(this.btnGlobalBack);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(677, 45);
            this.pnlTopBar.TabIndex = 2;
            // 
            // btnGlobalBack
            // 
            this.btnGlobalBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnGlobalBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGlobalBack.Location = new System.Drawing.Point(3, 3);
            this.btnGlobalBack.Name = "btnGlobalBack";
            this.btnGlobalBack.Size = new System.Drawing.Size(75, 37);
            this.btnGlobalBack.TabIndex = 0;
            this.btnGlobalBack.Text = "🔙";
            this.btnGlobalBack.UseVisualStyleBackColor = false;
            this.btnGlobalBack.Click += new System.EventHandler(this.BtnGlobalBack_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(0, 0);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersWidth = 51;
            this.dgvHistory.RowTemplate.Height = 24;
            this.dgvHistory.Size = new System.Drawing.Size(677, 415);
            this.dgvHistory.TabIndex = 1;
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistory.Location = new System.Drawing.Point(43, 225);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(103, 59);
            this.btnHistory.TabIndex = 14;
            this.btnHistory.Text = "Scan History";
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Location = new System.Drawing.Point(453, 422);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(26, 16);
            this.lblPercentage.TabIndex = 13;
            this.lblPercentage.Text = "0%";
            // 
            // pbScanProgress
            // 
            this.pbScanProgress.Location = new System.Drawing.Point(347, 422);
            this.pbScanProgress.Name = "pbScanProgress";
            this.pbScanProgress.Size = new System.Drawing.Size(100, 23);
            this.pbScanProgress.TabIndex = 12;
            // 
            // statusStrip2
            // 
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusText});
            this.statusStrip2.Location = new System.Drawing.Point(0, 415);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(677, 26);
            this.statusStrip2.TabIndex = 11;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // lblStatusText
            // 
            this.lblStatusText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(101, 20);
            this.lblStatusText.Text = "System Ready";
            // 
            // btnExternalScan
            // 
            this.btnExternalScan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExternalScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExternalScan.Location = new System.Drawing.Point(247, 225);
            this.btnExternalScan.Name = "btnExternalScan";
            this.btnExternalScan.Size = new System.Drawing.Size(85, 59);
            this.btnExternalScan.TabIndex = 9;
            this.btnExternalScan.Text = "Scan External Drive";
            this.btnExternalScan.UseVisualStyleBackColor = false;
            this.btnExternalScan.Click += new System.EventHandler(this.btnExternalScan_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFile.Location = new System.Drawing.Point(152, 291);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(281, 52);
            this.btnSelectFile.TabIndex = 8;
            this.btnSelectFile.Text = "select file to scan";
            this.btnSelectFile.UseVisualStyleBackColor = false;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(748, 442);
            this.Controls.Add(this.pnlContent);
            this.Name = "Form1";
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlHistory.ResumeLayout(false);
            this.pnlTopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblKasperskyStatus;
        private System.Windows.Forms.Label lblCloudConfidence;
        private System.Windows.Forms.Button btnFullScan;
        private System.Windows.Forms.Label lblAvastStatus;
        private System.Windows.Forms.Button btnQuickScan;
        private System.Windows.Forms.Label lblBitdefenderStatus;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnExternalScan;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusText;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.ProgressBar pbScanProgress;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Button btnGlobalBack;


        private void BtnGlobalBack_Click(object sender, System.EventArgs e)
        {

        }
    }
}