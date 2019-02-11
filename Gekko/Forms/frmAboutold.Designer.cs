namespace Gekko.Forms
{
	partial class frmAbout
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
			this.okButton = new System.Windows.Forms.Button();
			this.panel1 = new Gekko.Forms.Components.ExtendedPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.ForeColor = System.Drawing.Color.Black;
			this.okButton.Location = new System.Drawing.Point(203, 134);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(115, 28);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseCompatibleTextRendering = true;
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.DrawImage = null;
			this.panel1.DrawImageRectangle = new System.Drawing.Rectangle(3, 37, 48, 48);
			this.panel1.DrawText = "ぬるぽ";
			this.panel1.GlowSize = 12;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
			this.panel1.Size = new System.Drawing.Size(326, 170);
			this.panel1.TabIndex = 2;
			this.panel1.TextFont = new System.Drawing.Font("メイリオ", 9F);
			this.panel1.TextLocation = new System.Drawing.Point(57, 37);
			this.panel1.TextSize = new System.Drawing.Size(100, 100);
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseMove);
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseDown);
			this.panel1.Move += new System.EventHandler(this.frmAbout_Move);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(57, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseMove);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::Gekko.icons.icon48;
			this.pictureBox1.Location = new System.Drawing.Point(3, 51);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseMove);
			// 
			// frmAbout
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(332, 176);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "バージョン情報";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseDown);
			this.Move += new System.EventHandler(this.frmAbout_Move);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAbout_MouseMove);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private Gekko.Forms.Components.ExtendedPanel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}