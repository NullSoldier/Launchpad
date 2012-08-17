namespace PluginInstaller
{
	partial class frmPerformAction
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
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblInstruction = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(16, 20);
			this.progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(421, 35);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 0;
			// 
			// lblInstruction
			// 
			this.lblInstruction.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblInstruction.AutoSize = true;
			this.lblInstruction.Location = new System.Drawing.Point(295, 62);
			this.lblInstruction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.Size = new System.Drawing.Size(146, 20);
			this.lblInstruction.TabIndex = 1;
			this.lblInstruction.Text = "Reticulating splines...";
			// 
			// frmPerformAction
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 90);
			this.Controls.Add(this.lblInstruction);
			this.Controls.Add(this.progressBar);
			this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "frmPerformAction";
			this.Text = "Updating";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblInstruction;
	}
}