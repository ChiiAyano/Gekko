namespace Gekko.Forms
{
	partial class frmExceptionManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExceptionManager));
			this.panel1 = new System.Windows.Forms.Panel();
			this.exceptionLabel = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.hintLabel = new System.Windows.Forms.Label();
			this.resumeCheck = new System.Windows.Forms.CheckBox();
			this.exitButton = new System.Windows.Forms.Button();
			this.clipBoardButton = new System.Windows.Forms.Button();
			this.stackTraceBox = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.exceptionLabel);
			this.panel1.Controls.Add(this.pictureBox1);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// exceptionLabel
			// 
			resources.ApplyResources(this.exceptionLabel, "exceptionLabel");
			this.exceptionLabel.Name = "exceptionLabel";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Gekko.icons.myicon279_32x32x32;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.hintLabel);
			this.panel2.Controls.Add(this.resumeCheck);
			this.panel2.Controls.Add(this.exitButton);
			this.panel2.Controls.Add(this.clipBoardButton);
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			// 
			// hintLabel
			// 
			resources.ApplyResources(this.hintLabel, "hintLabel");
			this.hintLabel.Name = "hintLabel";
			// 
			// resumeCheck
			// 
			resources.ApplyResources(this.resumeCheck, "resumeCheck");
			this.resumeCheck.Name = "resumeCheck";
			this.resumeCheck.UseVisualStyleBackColor = true;
			// 
			// exitButton
			// 
			resources.ApplyResources(this.exitButton, "exitButton");
			this.exitButton.Name = "exitButton";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
			// 
			// clipBoardButton
			// 
			resources.ApplyResources(this.clipBoardButton, "clipBoardButton");
			this.clipBoardButton.Name = "clipBoardButton";
			this.clipBoardButton.UseVisualStyleBackColor = true;
			this.clipBoardButton.Click += new System.EventHandler(this.clipBoardButton_Click);
			// 
			// stackTraceBox
			// 
			resources.ApplyResources(this.stackTraceBox, "stackTraceBox");
			this.stackTraceBox.Name = "stackTraceBox";
			// 
			// frmExceptionManager
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add(this.stackTraceBox);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmExceptionManager";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmExceptionManager_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label exceptionLabel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox resumeCheck;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Button clipBoardButton;
		private System.Windows.Forms.TextBox stackTraceBox;
		private System.Windows.Forms.Label hintLabel;
	}
}