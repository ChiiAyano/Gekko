namespace Gekko.Forms
{
	partial class frmAccountSelect
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountSelect));
			this.label1 = new System.Windows.Forms.Label();
			this.openButton = new System.Windows.Forms.Button();
			this.accountSelectBox = new System.Windows.Forms.ListBox();
			this.closeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// openButton
			// 
			resources.ApplyResources(this.openButton, "openButton");
			this.openButton.Name = "openButton";
			this.openButton.UseVisualStyleBackColor = true;
			this.openButton.Click += new System.EventHandler(this.openButton_Click);
			// 
			// accountSelectBox
			// 
			this.accountSelectBox.FormattingEnabled = true;
			resources.ApplyResources(this.accountSelectBox, "accountSelectBox");
			this.accountSelectBox.Name = "accountSelectBox";
			this.accountSelectBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.accountSelectBox_MouseDoubleClick);
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.closeButton, "closeButton");
			this.closeButton.Name = "closeButton";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// frmAccountSelect
			// 
			this.AcceptButton = this.openButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.closeButton;
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.accountSelectBox);
			this.Controls.Add(this.openButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAccountSelect";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmAccountSelect_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.ListBox accountSelectBox;
		private System.Windows.Forms.Button closeButton;
	}
}