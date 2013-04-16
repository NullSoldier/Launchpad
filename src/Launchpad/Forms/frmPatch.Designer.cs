namespace LaunchPad.Forms
{
	partial class frmPatch
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
			this.inNotes = new System.Windows.Forms.RichTextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btnInstall = new System.Windows.Forms.Button();
			this.lblInstruction = new System.Windows.Forms.Label();
			this.lnkNotes = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// inNotes
			// 
			this.inNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inNotes.BackColor = System.Drawing.SystemColors.Window;
			this.inNotes.Location = new System.Drawing.Point(14, 84);
			this.inNotes.Multiline = true;
			this.inNotes.Name = "inNotes";
			this.inNotes.ReadOnly = true;
			this.inNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.inNotes.Size = new System.Drawing.Size(427, 138);
			this.inNotes.TabIndex = 0;
			this.inNotes.Text = "Fixes:\r\n\t- Fixed some bugs in A2J\r\n\t- Fixed a crash in the simulator\r\nEnhancement" +
    "s:\r\n\t- Added new feature\r\n";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(14, 32);
			this.progressBar.MarqueeAnimationSpeed = 10;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(328, 27);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 1;
			// 
			// btnInstall
			// 
			this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInstall.Location = new System.Drawing.Point(348, 32);
			this.btnInstall.Name = "btnInstall";
			this.btnInstall.Size = new System.Drawing.Size(87, 27);
			this.btnInstall.TabIndex = 2;
			this.btnInstall.Text = "Install";
			this.btnInstall.UseVisualStyleBackColor = true;
			this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
			// 
			// lblInstruction
			// 
			this.lblInstruction.AutoSize = true;
			this.lblInstruction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInstruction.Location = new System.Drawing.Point(12, 8);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.Size = new System.Drawing.Size(240, 21);
			this.lblInstruction.TabIndex = 3;
			this.lblInstruction.Text = "Preparing to install patch v0.0.2.0";
			// 
			// lnkNotes
			// 
			this.lnkNotes.AutoSize = true;
			this.lnkNotes.Location = new System.Drawing.Point(14, 63);
			this.lnkNotes.Name = "lnkNotes";
			this.lnkNotes.Size = new System.Drawing.Size(113, 15);
			this.lnkNotes.TabIndex = 4;
			this.lnkNotes.TabStop = true;
			this.lnkNotes.Text = "View update notes...";
			this.lnkNotes.Visible = false;
			this.lnkNotes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNotes_LinkClicked);
			// 
			// frmPatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 234);
			this.Controls.Add(this.lnkNotes);
			this.Controls.Add(this.lblInstruction);
			this.Controls.Add(this.btnInstall);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.inNotes);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(469, 39);
			this.Name = "frmPatch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Patch";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_Closing);
			this.Shown += new System.EventHandler(this.form_Shown);
			this.Resize += new System.EventHandler(this.frmPatch_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox inNotes;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btnInstall;
		private System.Windows.Forms.Label lblInstruction;
		private System.Windows.Forms.LinkLabel lnkNotes;

	}
}