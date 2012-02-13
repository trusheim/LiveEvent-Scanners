namespace SU_MT2000_SUIDScanner
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.PleaseScanLabel = new System.Windows.Forms.Label();
            this.GoLabel = new System.Windows.Forms.Label();
            this.MoreInfo = new System.Windows.Forms.Label();
            this.StopLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PleaseScanLabel
            // 
            this.PleaseScanLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.PleaseScanLabel.Location = new System.Drawing.Point(29, 104);
            this.PleaseScanLabel.Name = "PleaseScanLabel";
            this.PleaseScanLabel.Size = new System.Drawing.Size(263, 32);
            this.PleaseScanLabel.Text = "Scan a SUID";
            this.PleaseScanLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GoLabel
            // 
            this.GoLabel.Font = new System.Drawing.Font("Arial", 100F, System.Drawing.FontStyle.Bold);
            this.GoLabel.Location = new System.Drawing.Point(0, 14);
            this.GoLabel.Name = "GoLabel";
            this.GoLabel.Size = new System.Drawing.Size(320, 136);
            this.GoLabel.Text = "GO";
            this.GoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.GoLabel.Visible = false;
            // 
            // MoreInfo
            // 
            this.MoreInfo.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular);
            this.MoreInfo.Location = new System.Drawing.Point(0, 150);
            this.MoreInfo.Name = "MoreInfo";
            this.MoreInfo.Size = new System.Drawing.Size(320, 53);
            this.MoreInfo.Text = "More information";
            this.MoreInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MoreInfo.Visible = false;
            // 
            // StopLabel
            // 
            this.StopLabel.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Bold);
            this.StopLabel.Location = new System.Drawing.Point(3, 14);
            this.StopLabel.Name = "StopLabel";
            this.StopLabel.Size = new System.Drawing.Size(317, 136);
            this.StopLabel.Text = "STOP";
            this.StopLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StopLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.Controls.Add(this.StopLabel);
            this.Controls.Add(this.MoreInfo);
            this.Controls.Add(this.GoLabel);
            this.Controls.Add(this.PleaseScanLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label PleaseScanLabel;
        private System.Windows.Forms.Label GoLabel;
        private System.Windows.Forms.Label MoreInfo;
        private System.Windows.Forms.Label StopLabel;

    }
}
