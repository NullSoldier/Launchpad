namespace PluginSpaceport
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
			// frmTargets
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(473, 372);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.listTargets);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "frmTargets";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select devices to run on";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
			this.Load += new System.EventHandler(this.LoadingForm);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listTargets;
		private System.Windows.Forms.ColumnHeader colDeviceName;
		private System.Windows.Forms.ColumnHeader colOS;
		private System.Windows.Forms.Button btnOk;
	}
}