namespace Launchpad.Forms
{
	partial class frmProject
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.inDisplayName = new System.Windows.Forms.TextBox();
			this.inLoading = new System.Windows.Forms.TextBox();
			this.inAuthKey = new System.Windows.Forms.TextBox();
			this.cmbOrientation = new System.Windows.Forms.ComboBox();
			this.inURLSchemes = new System.Windows.Forms.TextBox();
			this.inVersion = new System.Windows.Forms.TextBox();
			this.inID = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.fileIdentity = new Launchpad.Forms.FileSelector();
			this.fileProvision = new Launchpad.Forms.FileSelector();
			this.fileResourcesiOS = new Launchpad.Forms.FileSelector();
			this.label11 = new System.Windows.Forms.Label();
			this.inBackground = new System.Windows.Forms.CheckBox();
			this.inGlassEffect = new System.Windows.Forms.CheckBox();
			this.cmbFamiliesiOS = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.fileResourcesAndroid = new Launchpad.Forms.FileSelector();
			this.fileKeyPassword = new Launchpad.Forms.FileSelector();
			this.fileKeystorePassword = new Launchpad.Forms.FileSelector();
			this.fileKeystore = new Launchpad.Forms.FileSelector();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.inVersionAndroid = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(485, 321);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.inDisplayName);
			this.tabPage1.Controls.Add(this.inLoading);
			this.tabPage1.Controls.Add(this.inAuthKey);
			this.tabPage1.Controls.Add(this.cmbOrientation);
			this.tabPage1.Controls.Add(this.inURLSchemes);
			this.tabPage1.Controls.Add(this.inVersion);
			this.tabPage1.Controls.Add(this.inID);
			this.tabPage1.Location = new System.Drawing.Point(4, 26);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage1.Size = new System.Drawing.Size(477, 291);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Both";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 211);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 17);
			this.label7.TabIndex = 13;
			this.label7.Text = "URLSchemes";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 174);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(98, 17);
			this.label6.TabIndex = 12;
			this.label6.Text = "Loading Screen";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 17);
			this.label5.TabIndex = 11;
			this.label5.Text = "Orientations";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 248);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(110, 17);
			this.label4.TabIndex = 10;
			this.label4.Text = "Authorization Key";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 17);
			this.label3.TabIndex = 9;
			this.label3.Text = "Version";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 17);
			this.label2.TabIndex = 8;
			this.label2.Text = "Display Name";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 17);
			this.label1.TabIndex = 7;
			this.label1.Text = "ID";
			// 
			// inDisplayName
			// 
			this.inDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inDisplayName.Location = new System.Drawing.Point(142, 60);
			this.inDisplayName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inDisplayName.Name = "inDisplayName";
			this.inDisplayName.Size = new System.Drawing.Size(316, 25);
			this.inDisplayName.TabIndex = 6;
			// 
			// inLoading
			// 
			this.inLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inLoading.Location = new System.Drawing.Point(142, 171);
			this.inLoading.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inLoading.Name = "inLoading";
			this.inLoading.Size = new System.Drawing.Size(316, 25);
			this.inLoading.TabIndex = 5;
			// 
			// inAuthKey
			// 
			this.inAuthKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inAuthKey.Location = new System.Drawing.Point(142, 245);
			this.inAuthKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inAuthKey.Name = "inAuthKey";
			this.inAuthKey.Size = new System.Drawing.Size(316, 25);
			this.inAuthKey.TabIndex = 4;
			// 
			// cmbOrientation
			// 
			this.cmbOrientation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOrientation.FormattingEnabled = true;
			this.cmbOrientation.Location = new System.Drawing.Point(142, 133);
			this.cmbOrientation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.cmbOrientation.Name = "cmbOrientation";
			this.cmbOrientation.Size = new System.Drawing.Size(316, 25);
			this.cmbOrientation.TabIndex = 3;
			// 
			// inURLSchemes
			// 
			this.inURLSchemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inURLSchemes.Enabled = false;
			this.inURLSchemes.Location = new System.Drawing.Point(142, 208);
			this.inURLSchemes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inURLSchemes.Name = "inURLSchemes";
			this.inURLSchemes.Size = new System.Drawing.Size(316, 25);
			this.inURLSchemes.TabIndex = 2;
			// 
			// inVersion
			// 
			this.inVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inVersion.Location = new System.Drawing.Point(142, 97);
			this.inVersion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inVersion.Name = "inVersion";
			this.inVersion.Size = new System.Drawing.Size(316, 25);
			this.inVersion.TabIndex = 1;
			// 
			// inID
			// 
			this.inID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inID.Location = new System.Drawing.Point(142, 24);
			this.inID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inID.Name = "inID";
			this.inID.Size = new System.Drawing.Size(316, 25);
			this.inID.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.fileIdentity);
			this.tabPage2.Controls.Add(this.fileProvision);
			this.tabPage2.Controls.Add(this.fileResourcesiOS);
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.inBackground);
			this.tabPage2.Controls.Add(this.inGlassEffect);
			this.tabPage2.Controls.Add(this.cmbFamiliesiOS);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.label10);
			this.tabPage2.Location = new System.Drawing.Point(4, 26);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage2.Size = new System.Drawing.Size(477, 291);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "iOS";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// fileIdentity
			// 
			this.fileIdentity.BackColor = System.Drawing.Color.Transparent;
			this.fileIdentity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileIdentity.Location = new System.Drawing.Point(155, 13);
			this.fileIdentity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fileIdentity.MinimumSize = new System.Drawing.Size(0, 25);
			this.fileIdentity.Name = "fileIdentity";
			this.fileIdentity.Path = "";
			this.fileIdentity.Size = new System.Drawing.Size(313, 32);
			this.fileIdentity.TabIndex = 39;
			// 
			// fileProvision
			// 
			this.fileProvision.BackColor = System.Drawing.Color.Transparent;
			this.fileProvision.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileProvision.Location = new System.Drawing.Point(155, 47);
			this.fileProvision.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fileProvision.MinimumSize = new System.Drawing.Size(0, 25);
			this.fileProvision.Name = "fileProvision";
			this.fileProvision.Path = "";
			this.fileProvision.Size = new System.Drawing.Size(313, 32);
			this.fileProvision.TabIndex = 38;
			// 
			// fileResourcesiOS
			// 
			this.fileResourcesiOS.BackColor = System.Drawing.Color.Transparent;
			this.fileResourcesiOS.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileResourcesiOS.Location = new System.Drawing.Point(155, 79);
			this.fileResourcesiOS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.fileResourcesiOS.MinimumSize = new System.Drawing.Size(0, 25);
			this.fileResourcesiOS.Name = "fileResourcesiOS";
			this.fileResourcesiOS.Path = "";
			this.fileResourcesiOS.Size = new System.Drawing.Size(313, 32);
			this.fileResourcesiOS.TabIndex = 37;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(15, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(114, 17);
			this.label11.TabIndex = 27;
			this.label11.Text = "Developer Identity";
			// 
			// inBackground
			// 
			this.inBackground.AutoSize = true;
			this.inBackground.Location = new System.Drawing.Point(20, 193);
			this.inBackground.Name = "inBackground";
			this.inBackground.Size = new System.Drawing.Size(234, 21);
			this.inBackground.TabIndex = 32;
			this.inBackground.Text = "Run in background when minimized";
			this.inBackground.UseVisualStyleBackColor = true;
			// 
			// inGlassEffect
			// 
			this.inGlassEffect.AutoSize = true;
			this.inGlassEffect.Location = new System.Drawing.Point(20, 163);
			this.inGlassEffect.Name = "inGlassEffect";
			this.inGlassEffect.Size = new System.Drawing.Size(165, 21);
			this.inGlassEffect.TabIndex = 31;
			this.inGlassEffect.Text = "Add glass effect to icon";
			this.inGlassEffect.UseVisualStyleBackColor = true;
			// 
			// cmbFamiliesiOS
			// 
			this.cmbFamiliesiOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFamiliesiOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFamiliesiOS.FormattingEnabled = true;
			this.cmbFamiliesiOS.Location = new System.Drawing.Point(155, 120);
			this.cmbFamiliesiOS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.cmbFamiliesiOS.Name = "cmbFamiliesiOS";
			this.cmbFamiliesiOS.Size = new System.Drawing.Size(313, 25);
			this.cmbFamiliesiOS.TabIndex = 25;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(15, 123);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 17);
			this.label8.TabIndex = 30;
			this.label8.Text = "iOS Device Families";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(15, 87);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(109, 17);
			this.label9.TabIndex = 29;
			this.label9.Text = "Resources Folder";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(15, 50);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 17);
			this.label10.TabIndex = 28;
			this.label10.Text = "Provisioning Profile";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.fileResourcesAndroid);
			this.tabPage3.Controls.Add(this.fileKeyPassword);
			this.tabPage3.Controls.Add(this.fileKeystorePassword);
			this.tabPage3.Controls.Add(this.fileKeystore);
			this.tabPage3.Controls.Add(this.label17);
			this.tabPage3.Controls.Add(this.label16);
			this.tabPage3.Controls.Add(this.label12);
			this.tabPage3.Controls.Add(this.inVersionAndroid);
			this.tabPage3.Controls.Add(this.label13);
			this.tabPage3.Controls.Add(this.label15);
			this.tabPage3.Location = new System.Drawing.Point(4, 26);
			this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage3.Size = new System.Drawing.Size(477, 291);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Android";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// fileResourcesAndroid
			// 
			this.fileResourcesAndroid.BackColor = System.Drawing.Color.Transparent;
			this.fileResourcesAndroid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileResourcesAndroid.Location = new System.Drawing.Point(167, 148);
			this.fileResourcesAndroid.Margin = new System.Windows.Forms.Padding(0);
			this.fileResourcesAndroid.MinimumSize = new System.Drawing.Size(0, 29);
			this.fileResourcesAndroid.Name = "fileResourcesAndroid";
			this.fileResourcesAndroid.Path = "";
			this.fileResourcesAndroid.Size = new System.Drawing.Size(292, 29);
			this.fileResourcesAndroid.TabIndex = 58;
			// 
			// fileKeyPassword
			// 
			this.fileKeyPassword.BackColor = System.Drawing.Color.Transparent;
			this.fileKeyPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileKeyPassword.Location = new System.Drawing.Point(169, 113);
			this.fileKeyPassword.Margin = new System.Windows.Forms.Padding(0);
			this.fileKeyPassword.MinimumSize = new System.Drawing.Size(0, 29);
			this.fileKeyPassword.Name = "fileKeyPassword";
			this.fileKeyPassword.Path = "";
			this.fileKeyPassword.Size = new System.Drawing.Size(292, 29);
			this.fileKeyPassword.TabIndex = 57;
			// 
			// fileKeystorePassword
			// 
			this.fileKeystorePassword.BackColor = System.Drawing.Color.Transparent;
			this.fileKeystorePassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileKeystorePassword.Location = new System.Drawing.Point(167, 79);
			this.fileKeystorePassword.Margin = new System.Windows.Forms.Padding(0);
			this.fileKeystorePassword.MinimumSize = new System.Drawing.Size(0, 29);
			this.fileKeystorePassword.Name = "fileKeystorePassword";
			this.fileKeystorePassword.Path = "";
			this.fileKeystorePassword.Size = new System.Drawing.Size(292, 29);
			this.fileKeystorePassword.TabIndex = 56;
			// 
			// fileKeystore
			// 
			this.fileKeystore.BackColor = System.Drawing.Color.Transparent;
			this.fileKeystore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fileKeystore.Location = new System.Drawing.Point(167, 45);
			this.fileKeystore.Margin = new System.Windows.Forms.Padding(0);
			this.fileKeystore.MinimumSize = new System.Drawing.Size(0, 29);
			this.fileKeystore.Name = "fileKeystore";
			this.fileKeystore.Path = "";
			this.fileKeystore.Size = new System.Drawing.Size(292, 29);
			this.fileKeystore.TabIndex = 55;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(17, 153);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(109, 17);
			this.label17.TabIndex = 53;
			this.label17.Text = "Resources Folder";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(17, 119);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(112, 17);
			this.label16.TabIndex = 50;
			this.label16.Text = "Key Password File";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(17, 17);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(87, 17);
			this.label12.TabIndex = 40;
			this.label12.Text = "Version Code";
			// 
			// inVersionAndroid
			// 
			this.inVersionAndroid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.inVersionAndroid.Location = new System.Drawing.Point(167, 13);
			this.inVersionAndroid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.inVersionAndroid.Name = "inVersionAndroid";
			this.inVersionAndroid.Size = new System.Drawing.Size(295, 25);
			this.inVersionAndroid.TabIndex = 36;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(17, 85);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(142, 17);
			this.label13.TabIndex = 43;
			this.label13.Text = "Keystore Password File";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(17, 51);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(82, 17);
			this.label15.TabIndex = 41;
			this.label15.Text = "Keystore File";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(301, 328);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 30);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(398, 328);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 30);
			this.btnSave.TabIndex = 14;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// frmProject
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(484, 367);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(500, 405);
			this.Name = "frmProject";
			this.Text = "Spaceport App Properties";
			this.Load += new System.EventHandler(this.onLoaded);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox inDisplayName;
		private System.Windows.Forms.TextBox inLoading;
		private System.Windows.Forms.TextBox inAuthKey;
		private System.Windows.Forms.ComboBox cmbOrientation;
		private System.Windows.Forms.TextBox inURLSchemes;
		private System.Windows.Forms.TextBox inVersion;
		private System.Windows.Forms.TextBox inID;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox inBackground;
		private System.Windows.Forms.CheckBox inGlassEffect;
		private System.Windows.Forms.ComboBox cmbFamiliesiOS;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox inVersionAndroid;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private FileSelector fileResourcesiOS;
		private FileSelector fileIdentity;
		private FileSelector fileProvision;
		private FileSelector fileResourcesAndroid;
		private FileSelector fileKeyPassword;
		private FileSelector fileKeystorePassword;
		private FileSelector fileKeystore;
	}
}