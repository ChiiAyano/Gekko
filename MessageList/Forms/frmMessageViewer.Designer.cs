namespace Gekko.MessageList.Forms
{
	partial class frmMessageViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessageViewer));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.messageListTab = new System.Windows.Forms.TabControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.topMostButton = new System.Windows.Forms.ToolStripButton();
			this.browserViewButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.checkSpamButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.invisibleViewerButton = new System.Windows.Forms.ToolStripButton();
			this.windowOpacityMenu = new System.Windows.Forms.ToolStripDropDownButton();
			this.windowOpacity100 = new System.Windows.Forms.ToolStripMenuItem();
			this.windowOpacity80 = new System.Windows.Forms.ToolStripMenuItem();
			this.windowOpacity50 = new System.Windows.Forms.ToolStripMenuItem();
			this.windowOpacity20 = new System.Windows.Forms.ToolStripMenuItem();
			this.detailToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.progressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.progressStatusProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.messageViewerBox = new Gekko.MessageList.Forms.Components.WebBrowserEx();
			this.dummyBrowser = new AxSHDocVw.AxWebBrowser();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dummyBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.messageListTab);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.messageViewerBox);
			this.splitContainer1.Panel2.Controls.Add(this.dummyBrowser);
			// 
			// messageListTab
			// 
			resources.ApplyResources(this.messageListTab, "messageListTab");
			this.messageListTab.Multiline = true;
			this.messageListTab.Name = "messageListTab";
			this.messageListTab.SelectedIndex = 0;
			this.messageListTab.SelectedIndexChanged += new System.EventHandler(this.messageListTab_SelectedIndexChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topMostButton,
            this.browserViewButton,
            this.toolStripSeparator1,
            this.checkSpamButton,
            this.toolStripSeparator2,
            this.invisibleViewerButton,
            this.windowOpacityMenu});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// topMostButton
			// 
			this.topMostButton.Image = global::MessageList.icons.lightbulb_off;
			resources.ApplyResources(this.topMostButton, "topMostButton");
			this.topMostButton.Name = "topMostButton";
			this.topMostButton.CheckedChanged += new System.EventHandler(this.topMostButton_CheckedChanged);
			this.topMostButton.Click += new System.EventHandler(this.topMostButton_Click);
			// 
			// browserViewButton
			// 
			this.browserViewButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			resources.ApplyResources(this.browserViewButton, "browserViewButton");
			this.browserViewButton.Image = global::MessageList.icons.world_go;
			this.browserViewButton.Name = "browserViewButton";
			this.browserViewButton.Click += new System.EventHandler(this.browserViewButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// checkSpamButton
			// 
			this.checkSpamButton.Image = global::MessageList.icons.bug;
			resources.ApplyResources(this.checkSpamButton, "checkSpamButton");
			this.checkSpamButton.Name = "checkSpamButton";
			this.checkSpamButton.Click += new System.EventHandler(this.checkSpamButton_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// invisibleViewerButton
			// 
			this.invisibleViewerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.invisibleViewerButton.Image = global::MessageList.icons.application_side_expand;
			resources.ApplyResources(this.invisibleViewerButton, "invisibleViewerButton");
			this.invisibleViewerButton.Name = "invisibleViewerButton";
			this.invisibleViewerButton.CheckedChanged += new System.EventHandler(this.invisibleViewerButton_CheckedChanged);
			this.invisibleViewerButton.Click += new System.EventHandler(this.invisibleViewerButton_Click);
			// 
			// windowOpacityMenu
			// 
			this.windowOpacityMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.windowOpacityMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowOpacity100,
            this.windowOpacity80,
            this.windowOpacity50,
            this.windowOpacity20});
			this.windowOpacityMenu.Image = global::MessageList.icons.contrast_high;
			resources.ApplyResources(this.windowOpacityMenu, "windowOpacityMenu");
			this.windowOpacityMenu.Name = "windowOpacityMenu";
			// 
			// windowOpacity100
			// 
			this.windowOpacity100.Checked = true;
			this.windowOpacity100.CheckState = System.Windows.Forms.CheckState.Checked;
			this.windowOpacity100.Name = "windowOpacity100";
			resources.ApplyResources(this.windowOpacity100, "windowOpacity100");
			this.windowOpacity100.Click += new System.EventHandler(this.windowOpacity_Click);
			// 
			// windowOpacity80
			// 
			this.windowOpacity80.Name = "windowOpacity80";
			resources.ApplyResources(this.windowOpacity80, "windowOpacity80");
			this.windowOpacity80.Click += new System.EventHandler(this.windowOpacity_Click);
			// 
			// windowOpacity50
			// 
			this.windowOpacity50.Name = "windowOpacity50";
			resources.ApplyResources(this.windowOpacity50, "windowOpacity50");
			this.windowOpacity50.Click += new System.EventHandler(this.windowOpacity_Click);
			// 
			// windowOpacity20
			// 
			this.windowOpacity20.Name = "windowOpacity20";
			resources.ApplyResources(this.windowOpacity20, "windowOpacity20");
			this.windowOpacity20.Click += new System.EventHandler(this.windowOpacity_Click);
			// 
			// detailToolTip
			// 
			this.detailToolTip.AutoPopDelay = 5000;
			this.detailToolTip.InitialDelay = 500;
			this.detailToolTip.ReshowDelay = 0;
			this.detailToolTip.ShowAlways = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressStatusLabel,
            this.progressStatusProgress});
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Name = "statusStrip1";
			// 
			// progressStatusLabel
			// 
			resources.ApplyResources(this.progressStatusLabel, "progressStatusLabel");
			this.progressStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.progressStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.progressStatusLabel.Name = "progressStatusLabel";
			this.progressStatusLabel.Spring = true;
			// 
			// progressStatusProgress
			// 
			this.progressStatusProgress.Name = "progressStatusProgress";
			resources.ApplyResources(this.progressStatusProgress, "progressStatusProgress");
			// 
			// messageViewerBox
			// 
			resources.ApplyResources(this.messageViewerBox, "messageViewerBox");
			this.messageViewerBox.MinimumSize = new System.Drawing.Size(20, 20);
			this.messageViewerBox.Name = "messageViewerBox";
			this.messageViewerBox.ScriptErrorsSuppressed = true;
			this.messageViewerBox.NewWindow2 += new Gekko.MessageList.Forms.Components.WebBrowserNewWindow2EventHandler(this.messageViewerBox_NewWindow2);
			this.messageViewerBox.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.messageViewerBox_DocumentCompleted);
			this.messageViewerBox.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.messageViewerBox_Navigating);
			this.messageViewerBox.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.messageViewerBox_ProgressChanged);
			// 
			// dummyBrowser
			// 
			resources.ApplyResources(this.dummyBrowser, "dummyBrowser");
			this.dummyBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("dummyBrowser.OcxState")));
			this.dummyBrowser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.dummyBrowser_BeforeNavigate2);
			// 
			// frmMessageViewer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMessageViewer";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMessageViewer_FormClosing);
			this.Load += new System.EventHandler(this.frmMessageViewer_Load);
			this.VisibleChanged += new System.EventHandler(this.frmMessageViewer_VisibleChanged);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMessageViewer_MouseUp);
			this.Move += new System.EventHandler(this.frmMessageViewer_Move);
			this.Resize += new System.EventHandler(this.frmMessageViewer_Move);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dummyBrowser)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabControl messageListTab;
		private AxSHDocVw.AxWebBrowser dummyBrowser;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton topMostButton;
		private System.Windows.Forms.ToolStripButton browserViewButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton checkSpamButton;
		private Gekko.MessageList.Forms.Components.WebBrowserEx messageViewerBox;
		private System.Windows.Forms.ToolTip detailToolTip;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel progressStatusLabel;
		private System.Windows.Forms.ToolStripProgressBar progressStatusProgress;
		private System.Windows.Forms.ToolStripButton invisibleViewerButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripDropDownButton windowOpacityMenu;
		private System.Windows.Forms.ToolStripMenuItem windowOpacity100;
		private System.Windows.Forms.ToolStripMenuItem windowOpacity80;
		private System.Windows.Forms.ToolStripMenuItem windowOpacity50;
		private System.Windows.Forms.ToolStripMenuItem windowOpacity20;
	}
}