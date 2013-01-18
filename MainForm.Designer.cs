namespace SU_MT2000_SUIDScanner
{
    partial class MainForm : Symbol.MT2000.UserInterface.BaseForm
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
            this.SmallInfo = new System.Windows.Forms.Label();
            this.BigLabel = new System.Windows.Forms.Label();
            this.numIn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PleaseScanLabel
            // 
            this.PleaseScanLabel.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
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
            // SmallInfo
            // 
            this.SmallInfo.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular);
            this.SmallInfo.Location = new System.Drawing.Point(0, 150);
            this.SmallInfo.Name = "SmallInfo";
            this.SmallInfo.Size = new System.Drawing.Size(320, 53);
            this.SmallInfo.Text = "More information";
            this.SmallInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SmallInfo.Visible = false;
            // 
            // BigLabel
            // 
            this.BigLabel.Font = new System.Drawing.Font("Arial", 54F, System.Drawing.FontStyle.Bold);
            this.BigLabel.Location = new System.Drawing.Point(0, 33);
            this.BigLabel.Name = "BigLabel";
            this.BigLabel.Size = new System.Drawing.Size(320, 117);
            this.BigLabel.Text = "BigInfo";
            this.BigLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BigLabel.Visible = false;
            // 
            // numIn
            // 
            this.numIn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular);
            this.numIn.Location = new System.Drawing.Point(222, 157);
            this.numIn.Name = "numIn";
            this.numIn.Size = new System.Drawing.Size(98, 53);
            this.numIn.Text = "0";
            this.numIn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.Controls.Add(this.numIn);
            this.Controls.Add(this.BigLabel);
            this.Controls.Add(this.SmallInfo);
            this.Controls.Add(this.GoLabel);
            this.Controls.Add(this.PleaseScanLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label PleaseScanLabel;
        private System.Windows.Forms.Label GoLabel;
        private System.Windows.Forms.Label SmallInfo;
        private System.Windows.Forms.Label BigLabel;
        private System.Windows.Forms.Label numIn;

    }
}
