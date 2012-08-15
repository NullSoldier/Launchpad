namespace PluginInstaller
{
	partial class frmMain
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
			this.inConsole = new System.Windows.Forms.TextBox();
			this.btnInstall = new System.Windows.Forms.Button();
			this.btnFinish = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// inConsole
			// 
			this.inConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inConsole.Location = new System.Drawing.Point(18, 19);
			this.inConsole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.inConsole.Multiline = true;
			this.inConsole.Name = "inConsole";
			this.inConsole.ReadOnly = true;
			this.inConsole.Size = new System.Drawing.Size(817, 309);
			this.inConsole.TabIndex = 0;
			// 
			// btnInstall
			// 
			this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInstall.Location = new System.Drawing.Point(18, 341);
			this.btnInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnInstall.Name = "btnInstall";
			this.btnInstall.Size = new System.Drawing.Size(819, 57);
			this.btnInstall.TabIndex = 1;
			this.btnInstall.Text = "Install";
			this.btnInstall.UseVisualStyleBackColor = true;
			this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.Enabled = false;
			this.btnFinish.Location = new System.Drawing.Point(18, 407);
			this.btnFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(819, 57);
			this.btnFinish.TabIndex = 2;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(855, 468);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.btnInstall);
			this.Controls.Add(this.inConsole);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "frmMain";
			this.Text = "Spaceport Plugin Installer";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inConsole;
		private System.Windows.Forms.Button btnInstall;
		private System.Windows.Forms.Button btnFinish;
	}
}

