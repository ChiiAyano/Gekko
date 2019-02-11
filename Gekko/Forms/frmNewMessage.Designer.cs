namespace Gekko.Forms
{
	partial class frmNewMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewMessage));
            this.closeTimer = new System.Windows.Forms.Timer(this.components);
            this.extendedPanel1 = new Gekko.Forms.Components.ExtendedPanel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // closeTimer
            // 
            this.closeTimer.Interval = 10000;
            this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
            // 
            // extendedPanel1
            // 
            this.extendedPanel1.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.extendedPanel1, "extendedPanel1");
            this.extendedPanel1.DrawImage = null;
            this.extendedPanel1.DrawImageRectangle = new System.Drawing.Rectangle(3, 37, 48, 48);
            this.extendedPanel1.DrawText = "";
            this.extendedPanel1.GlowSize = 12;
            this.extendedPanel1.Name = "extendedPanel1";
            this.extendedPanel1.TextFont = new System.Drawing.Font("メイリオ", 9F);
            this.extendedPanel1.TextLocation = new System.Drawing.Point(57, 37);
            this.extendedPanel1.TextSize = new System.Drawing.Size(100, 100);
            this.extendedPanel1.Click += new System.EventHandler(this.extendedPanel1_Click);
            // 
            // messageLabel
            // 
            resources.ApplyResources(this.messageLabel, "messageLabel");
            this.messageLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Click += new System.EventHandler(this.extendedPanel1_Click);
            // 
            // frmNewMessage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.extendedPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmNewMessage_Load);
            this.Move += new System.EventHandler(this.frmNewMessage_Move);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Gekko.Forms.Components.ExtendedPanel extendedPanel1;
		private System.Windows.Forms.Timer closeTimer;
		private System.Windows.Forms.Label messageLabel;
	}
}