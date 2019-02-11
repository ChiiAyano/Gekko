namespace Gekko.Forms
{
	partial class frmAccountSetting
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountSetting));
			this.label1 = new System.Windows.Forms.Label();
			this.accountSettingNameBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.accountNameBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.accountDomainBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.accountPasswordBox = new System.Windows.Forms.TextBox();
			this.accountPreviewLabel = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.accountConnectTestButton = new System.Windows.Forms.Button();
			this.accountConnectTestLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.formNullErrorPrivider = new System.Windows.Forms.ErrorProvider(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.formNullErrorPrivider)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// accountSettingNameBox
			// 
			resources.ApplyResources(this.accountSettingNameBox, "accountSettingNameBox");
			this.accountSettingNameBox.Name = "accountSettingNameBox";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// accountNameBox
			// 
			resources.ApplyResources(this.accountNameBox, "accountNameBox");
			this.accountNameBox.Name = "accountNameBox";
			this.accountNameBox.TextChanged += new System.EventHandler(this.accountNameBox_TextChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// accountDomainBox
			// 
			this.accountDomainBox.FormattingEnabled = true;
			this.accountDomainBox.Items.AddRange(new object[] {
            resources.GetString("accountDomainBox.Items"),
            resources.GetString("accountDomainBox.Items1"),
            resources.GetString("accountDomainBox.Items2")});
			resources.ApplyResources(this.accountDomainBox, "accountDomainBox");
			this.accountDomainBox.Name = "accountDomainBox";
			this.accountDomainBox.SelectedIndexChanged += new System.EventHandler(this.accountDomainBox_SelectedIndexChanged);
			this.accountDomainBox.TextChanged += new System.EventHandler(this.accountDomainBox_SelectedIndexChanged);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// accountPasswordBox
			// 
			resources.ApplyResources(this.accountPasswordBox, "accountPasswordBox");
			this.accountPasswordBox.Name = "accountPasswordBox";
			this.accountPasswordBox.TextChanged += new System.EventHandler(this.accountPasswordBox_TextChanged);
			// 
			// accountPreviewLabel
			// 
			resources.ApplyResources(this.accountPreviewLabel, "accountPreviewLabel");
			this.accountPreviewLabel.Name = "accountPreviewLabel";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.accountPreviewLabel);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// accountConnectTestButton
			// 
			resources.ApplyResources(this.accountConnectTestButton, "accountConnectTestButton");
			this.accountConnectTestButton.Name = "accountConnectTestButton";
			this.accountConnectTestButton.UseVisualStyleBackColor = true;
			this.accountConnectTestButton.Click += new System.EventHandler(this.accountConnectTestButton_Click);
			// 
			// accountConnectTestLabel
			// 
			resources.ApplyResources(this.accountConnectTestLabel, "accountConnectTestLabel");
			this.accountConnectTestLabel.Name = "accountConnectTestLabel";
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// formNullErrorPrivider
			// 
			this.formNullErrorPrivider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.formNullErrorPrivider.ContainerControl = this;
			// 
			// frmAccountSetting
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.accountConnectTestLabel);
			this.Controls.Add(this.accountConnectTestButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.accountPasswordBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.accountDomainBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.accountNameBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.accountSettingNameBox);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAccountSetting";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.formNullErrorPrivider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox accountSettingNameBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox accountNameBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox accountDomainBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox accountPasswordBox;
		private System.Windows.Forms.Label accountPreviewLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button accountConnectTestButton;
		private System.Windows.Forms.Label accountConnectTestLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ErrorProvider formNullErrorPrivider;
	}
}