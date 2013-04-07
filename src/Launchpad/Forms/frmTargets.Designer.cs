namespace Launchpad
{
	partial class frmTargets
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
			this.listTargets = new System.Windows.Forms.ListView();
			this.colDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colOS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnOk = new System.Windows.Forms.Button();
			this.lblCloseNote = new System.Windows.Forms.Label();
			this.lblError = new System.Windows.Forms.Label();
			this.lnkError = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// listTargets
			// 
			this.listTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listTargets.CheckBoxes = true;
			this.listTargets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDeviceName,
            this.colOS});
			this.listTargets.FullRowSelect = true;
			this.listTargets.HideSelection = false;
			this.listTargets.Location = new System.Drawing.Point(14, 12);
			this.listTargets.Name = "listTargets";
			this.listTargets.Size = new System.Drawing.Size(444, 319);
			this.listTargets.TabIndex = 0;
			this.listTargets.UseCompatibleStateImageBehavior = false;
			this.listTargets.View = System.Windows.Forms.View.Details;
			// 
			// colDeviceName
			// 
			this.colDeviceName.Text = "Device";
			this.colDeviceName.Width = 242;
			// 
			// colOS
			// 
			this.colOS.Text = "DevicePlatform";
			this.colOS.Width = 170;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(383, 337);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblCloseNote
			// 
			this.lblCloseNote.AutoSize = true;
			this.lblCloseNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCloseNote.Location = new System.Drawing.Point(15, 341);
			this.lblCloseNote.Name = "lblCloseNote";
			this.lblCloseNote.Size = new System.Drawing.Size(327, 15);
			this.lblCloseNote.TabIndex = 3;
			this.lblCloseNote.Text = "This list updates automatically. There is no need to reopen it.";
			// 
			// lblError
			// 
			this.lblError.AutoSize = true;
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(14, 341);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(261, 15);
			this.lblError.TabIndex = 4;
			this.lblError.Text = "Failed to get devices. List will refresh when fixed,";
			this.lblError.Visible = false;
			// 
			// lnkError
			// 
			this.lnkError.AutoSize = true;
			this.lnkError.Location = new System.Drawing.Point(275, 341);
			this.lnkError.Name = "lnkError";
			this.lnkError.Size = new System.Drawing.Size(101, 15);
			this.lnkError.TabIndex = 5;
			this.lnkError.TabStop = true;
			this.lnkError.Text = "more information";
			this.lnkError.Visible = false;
			this.lnkError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onErrorClicked);
			// 
			// frmTargets
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(473, 372);
			this.Controls.Add(this.lnkError);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.lblCloseNote);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.listTargets);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmTargets";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select devices to run on";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
			this.Load += new System.EventHandler(this.formLoaded);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listTargets;
		private System.Windows.Forms.ColumnHeader colDeviceName;
		private System.Windows.Forms.ColumnHeader colOS;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label lblCloseNote;
		private System.Windows.Forms.Label lblError;
		private System.Windows.Forms.LinkLabel lnkError;
	}
}