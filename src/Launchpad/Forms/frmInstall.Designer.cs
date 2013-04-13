namespace LaunchPad.Forms
{
	partial class frmInstall
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
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.inLog = new System.Windows.Forms.RichTextBox();
			this.btnInstalliOS = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnInstallAndroid = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// inLog
			// 
			this.inLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inLog.BackColor = System.Drawing.SystemColors.Window;
			this.inLog.Location = new System.Drawing.Point(14, 14);
			this.inLog.Name = "inLog";
			this.inLog.ReadOnly = true;
			this.inLog.Size = new System.Drawing.Size(461, 308);
			this.inLog.TabIndex = 0;
			this.inLog.Text = "";
			// 
			// btnInstalliOS
			// 
			this.btnInstalliOS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInstalliOS.Location = new System.Drawing.Point(362, 328);
			this.btnInstalliOS.Name = "btnInstalliOS";
			this.btnInstalliOS.Size = new System.Drawing.Size(113, 27);
			this.btnInstalliOS.TabIndex = 1;
			this.btnInstalliOS.Text = "Install to iOS";
			this.btnInstalliOS.UseVisualStyleBackColor = true;
			this.btnInstalliOS.Click += new System.EventHandler(this.btnInstalliOS_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(150, 328);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(87, 27);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnInstallAndroid
			// 
			this.btnInstallAndroid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInstallAndroid.Location = new System.Drawing.Point(243, 328);
			this.btnInstallAndroid.Name = "btnInstallAndroid";
			this.btnInstallAndroid.Size = new System.Drawing.Size(113, 27);
			this.btnInstallAndroid.TabIndex = 4;
			this.btnInstallAndroid.Text = "Install to Android";
			this.btnInstallAndroid.UseVisualStyleBackColor = true;
			this.btnInstallAndroid.Click += new System.EventHandler(this.btnInstallAndroid_Click);
			// 
			// frmInstall
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(490, 367);
			this.Controls.Add(this.btnInstallAndroid);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnInstalliOS);
			this.Controls.Add(this.inLog);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmInstall";
			this.Text = "Install to device";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInstall_FormClosing);
			this.Load += new System.EventHandler(this.frmInstall_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox inLog;
		private System.Windows.Forms.Button btnInstalliOS;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnInstallAndroid;
	}
}