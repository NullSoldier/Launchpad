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
			this.btnAgree = new System.Windows.Forms.CheckBox();
			this.inLicense = new System.Windows.Forms.RichTextBox();
			this.inAssemblyPath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// inConsole
			// 
			this.inConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inConsole.Location = new System.Drawing.Point(18, 49);
			this.inConsole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.inConsole.Multiline = true;
			this.inConsole.Name = "inConsole";
			this.inConsole.ReadOnly = true;
			this.inConsole.Size = new System.Drawing.Size(945, 486);
			this.inConsole.TabIndex = 0;
			this.inConsole.Visible = false;
			// 
			// btnInstall
			// 
			this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInstall.Enabled = false;
			this.btnInstall.Location = new System.Drawing.Point(18, 543);
			this.btnInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnInstall.Name = "btnInstall";
			this.btnInstall.Size = new System.Drawing.Size(947, 57);
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
			this.btnFinish.Location = new System.Drawing.Point(18, 543);
			this.btnFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(947, 57);
			this.btnFinish.TabIndex = 2;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Visible = false;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// btnAgree
			// 
			this.btnAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAgree.AutoSize = true;
			this.btnAgree.Location = new System.Drawing.Point(861, 510);
			this.btnAgree.Name = "btnAgree";
			this.btnAgree.Size = new System.Drawing.Size(78, 25);
			this.btnAgree.TabIndex = 3;
			this.btnAgree.Text = "I Agree";
			this.btnAgree.UseVisualStyleBackColor = true;
			this.btnAgree.CheckedChanged += new System.EventHandler(this.btnAgree_CheckedChanged);
			// 
			// inLicense
			// 
			this.inLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inLicense.Location = new System.Drawing.Point(20, 47);
			this.inLicense.Name = "inLicense";
			this.inLicense.ReadOnly = true;
			this.inLicense.Size = new System.Drawing.Size(945, 457);
			this.inLicense.TabIndex = 5;
			this.inLicense.Text = "My penis is small.";
			// 
			// inAssemblyPath
			// 
			this.inAssemblyPath.Location = new System.Drawing.Point(20, 12);
			this.inAssemblyPath.Name = "inAssemblyPath";
			this.inAssemblyPath.Size = new System.Drawing.Size(835, 29);
			this.inAssemblyPath.TabIndex = 6;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(861, 12);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(102, 29);
			this.btnBrowse.TabIndex = 7;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(983, 604);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.inAssemblyPath);
			this.Controls.Add(this.btnAgree);
			this.Controls.Add(this.inConsole);
			this.Controls.Add(this.btnInstall);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.inLicense);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Spaceport Plugin Installer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inConsole;
		private System.Windows.Forms.Button btnInstall;
		private System.Windows.Forms.Button btnFinish;
		private System.Windows.Forms.CheckBox btnAgree;
		private System.Windows.Forms.RichTextBox inLicense;
		private System.Windows.Forms.TextBox inAssemblyPath;
		private System.Windows.Forms.Button btnBrowse;
	}
}

