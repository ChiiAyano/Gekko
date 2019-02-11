namespace Gekko.Forms
{
	partial class frmMain
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.logMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.logCopyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.logAllSelectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.logDeleteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.logFontMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.latestCheckTimeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSettingMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mainConnectStartMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mainConnectPauseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainHideMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mainExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.messageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.messageCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.messageListMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.messageViewInbox = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpAboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyMainWindowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.notifySettingMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyOpenInboxMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyMailCheckMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyMessageListMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyAboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyOfficialSiteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSettingButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMailCheckButton = new System.Windows.Forms.ToolStripSplitButton();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nullpoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMessageListButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.logFontDialog = new System.Windows.Forms.FontDialog();
            this.logMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.notifyMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logMenu
            // 
            this.logMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logCopyMenu,
            this.logAllSelectMenu,
            this.toolStripSeparator7,
            this.logDeleteMenu,
            this.toolStripSeparator13,
            this.logFontMenu});
            this.logMenu.Name = "logMenu";
            resources.ApplyResources(this.logMenu, "logMenu");
            // 
            // logCopyMenu
            // 
            resources.ApplyResources(this.logCopyMenu, "logCopyMenu");
            this.logCopyMenu.Name = "logCopyMenu";
            this.logCopyMenu.Click += new System.EventHandler(this.logCopyMenu_Click);
            // 
            // logAllSelectMenu
            // 
            this.logAllSelectMenu.Name = "logAllSelectMenu";
            resources.ApplyResources(this.logAllSelectMenu, "logAllSelectMenu");
            this.logAllSelectMenu.Click += new System.EventHandler(this.logAllSelectMenu_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // logDeleteMenu
            // 
            resources.ApplyResources(this.logDeleteMenu, "logDeleteMenu");
            this.logDeleteMenu.Name = "logDeleteMenu";
            this.logDeleteMenu.Click += new System.EventHandler(this.logDeleteMenu_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // logFontMenu
            // 
            resources.ApplyResources(this.logFontMenu, "logFontMenu");
            this.logFontMenu.Name = "logFontMenu";
            this.logFontMenu.Click += new System.EventHandler(this.logFontMenu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.latestCheckTimeStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            // 
            // latestCheckTimeStatus
            // 
            this.latestCheckTimeStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.latestCheckTimeStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.latestCheckTimeStatus.Name = "latestCheckTimeStatus";
            resources.ApplyResources(this.latestCheckTimeStatus, "latestCheckTimeStatus");
            this.latestCheckTimeStatus.Spring = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu,
            this.messageMenu,
            this.helpMenu});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // mainMenu
            // 
            this.mainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainSettingMenu,
            this.toolStripSeparator1,
            this.mainConnectMenu,
            this.toolStripSeparator2,
            this.mainHideMenu,
            this.toolStripSeparator8,
            this.mainExitMenu});
            this.mainMenu.Name = "mainMenu";
            resources.ApplyResources(this.mainMenu, "mainMenu");
            // 
            // mainSettingMenu
            // 
            resources.ApplyResources(this.mainSettingMenu, "mainSettingMenu");
            this.mainSettingMenu.Name = "mainSettingMenu";
            this.mainSettingMenu.Click += new System.EventHandler(this.mainSettingMenu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // mainConnectMenu
            // 
            this.mainConnectMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainConnectStartMenu,
            this.mainConnectPauseMenu});
            this.mainConnectMenu.Name = "mainConnectMenu";
            resources.ApplyResources(this.mainConnectMenu, "mainConnectMenu");
            // 
            // mainConnectStartMenu
            // 
            resources.ApplyResources(this.mainConnectStartMenu, "mainConnectStartMenu");
            this.mainConnectStartMenu.Name = "mainConnectStartMenu";
            this.mainConnectStartMenu.Click += new System.EventHandler(this.mainConnectStartMenu_Click);
            // 
            // mainConnectPauseMenu
            // 
            resources.ApplyResources(this.mainConnectPauseMenu, "mainConnectPauseMenu");
            this.mainConnectPauseMenu.Name = "mainConnectPauseMenu";
            this.mainConnectPauseMenu.Click += new System.EventHandler(this.mainConnectPauseMenu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // mainHideMenu
            // 
            resources.ApplyResources(this.mainHideMenu, "mainHideMenu");
            this.mainHideMenu.Name = "mainHideMenu";
            this.mainHideMenu.Click += new System.EventHandler(this.mainHideMenu_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // mainExitMenu
            // 
            resources.ApplyResources(this.mainExitMenu, "mainExitMenu");
            this.mainExitMenu.Name = "mainExitMenu";
            this.mainExitMenu.Click += new System.EventHandler(this.mainExitMenu_Click);
            // 
            // messageMenu
            // 
            this.messageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageCheck,
            this.messageListMenu,
            this.toolStripSeparator9,
            this.messageViewInbox});
            this.messageMenu.Name = "messageMenu";
            resources.ApplyResources(this.messageMenu, "messageMenu");
            // 
            // messageCheck
            // 
            resources.ApplyResources(this.messageCheck, "messageCheck");
            this.messageCheck.Name = "messageCheck";
            this.messageCheck.Click += new System.EventHandler(this.messageCheck_Click);
            // 
            // messageListMenu
            // 
            resources.ApplyResources(this.messageListMenu, "messageListMenu");
            this.messageListMenu.Name = "messageListMenu";
            this.messageListMenu.Click += new System.EventHandler(this.messageListMenu_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // messageViewInbox
            // 
            resources.ApplyResources(this.messageViewInbox, "messageViewInbox");
            this.messageViewInbox.Name = "messageViewInbox";
            this.messageViewInbox.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.messageViewInbox.Click += new System.EventHandler(this.notifyOpenInboxMenu_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpAboutMenu});
            this.helpMenu.Name = "helpMenu";
            resources.ApplyResources(this.helpMenu, "helpMenu");
            // 
            // helpAboutMenu
            // 
            resources.ApplyResources(this.helpAboutMenu, "helpAboutMenu");
            this.helpAboutMenu.Name = "helpAboutMenu";
            this.helpAboutMenu.Click += new System.EventHandler(this.helpAboutMenu_Click);
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyMainWindowMenu,
            this.notifySettingMenu,
            this.toolStripSeparator3,
            this.notifyOpenInboxMenu,
            this.notifyMailCheckMenu,
            this.notifyMessageListMenu,
            this.toolStripSeparator4,
            this.notifyConnectMenu,
            this.toolStripSeparator5,
            this.notifyAboutMenu,
            this.notifyOfficialSiteMenu,
            this.toolStripSeparator6,
            this.notifyExitMenu});
            this.notifyMenu.Name = "notifyMenu";
            resources.ApplyResources(this.notifyMenu, "notifyMenu");
            // 
            // notifyMainWindowMenu
            // 
            resources.ApplyResources(this.notifyMainWindowMenu, "notifyMainWindowMenu");
            this.notifyMainWindowMenu.Name = "notifyMainWindowMenu";
            this.notifyMainWindowMenu.Click += new System.EventHandler(this.notifyMainWindowMenu_Click);
            // 
            // notifySettingMenu
            // 
            resources.ApplyResources(this.notifySettingMenu, "notifySettingMenu");
            this.notifySettingMenu.Name = "notifySettingMenu";
            this.notifySettingMenu.Click += new System.EventHandler(this.mainSettingMenu_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // notifyOpenInboxMenu
            // 
            resources.ApplyResources(this.notifyOpenInboxMenu, "notifyOpenInboxMenu");
            this.notifyOpenInboxMenu.Name = "notifyOpenInboxMenu";
            this.notifyOpenInboxMenu.Click += new System.EventHandler(this.notifyOpenInboxMenu_Click);
            // 
            // notifyMailCheckMenu
            // 
            resources.ApplyResources(this.notifyMailCheckMenu, "notifyMailCheckMenu");
            this.notifyMailCheckMenu.Name = "notifyMailCheckMenu";
            // 
            // notifyMessageListMenu
            // 
            resources.ApplyResources(this.notifyMessageListMenu, "notifyMessageListMenu");
            this.notifyMessageListMenu.Name = "notifyMessageListMenu";
            this.notifyMessageListMenu.Click += new System.EventHandler(this.messageListMenu_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // notifyConnectMenu
            // 
            resources.ApplyResources(this.notifyConnectMenu, "notifyConnectMenu");
            this.notifyConnectMenu.Name = "notifyConnectMenu";
            this.notifyConnectMenu.Click += new System.EventHandler(this.notifyConnectMenu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // notifyAboutMenu
            // 
            resources.ApplyResources(this.notifyAboutMenu, "notifyAboutMenu");
            this.notifyAboutMenu.Name = "notifyAboutMenu";
            this.notifyAboutMenu.Click += new System.EventHandler(this.helpAboutMenu_Click);
            // 
            // notifyOfficialSiteMenu
            // 
            resources.ApplyResources(this.notifyOfficialSiteMenu, "notifyOfficialSiteMenu");
            this.notifyOfficialSiteMenu.Name = "notifyOfficialSiteMenu";
            this.notifyOfficialSiteMenu.Click += new System.EventHandler(this.notifyOfficialSiteMenu_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // notifyExitMenu
            // 
            resources.ApplyResources(this.notifyExitMenu, "notifyExitMenu");
            this.notifyExitMenu.Name = "notifyExitMenu";
            this.notifyExitMenu.Click += new System.EventHandler(this.mainExitMenu_Click);
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.White;
            this.logBox.ContextMenuStrip = this.logMenu;
            resources.ApplyResources(this.logBox, "logBox");
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.notifyMenu;
            resources.ApplyResources(this.notify, "notify");
            this.notify.BalloonTipClicked += new System.EventHandler(this.notify_BalloonTipClicked);
            this.notify.Click += new System.EventHandler(this.notify_Click);
            this.notify.DoubleClick += new System.EventHandler(this.notify_DoubleClick);
            this.notify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notify_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator12,
            this.toolSettingButton,
            this.toolStripSeparator11,
            this.toolMailCheckButton,
            this.toolMessageListButton,
            this.toolStripButton1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // toolSettingButton
            // 
            this.toolSettingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolSettingButton, "toolSettingButton");
            this.toolSettingButton.Name = "toolSettingButton";
            this.toolSettingButton.Click += new System.EventHandler(this.mainSettingMenu_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // toolMailCheckButton
            // 
            this.toolMailCheckButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.nullpoToolStripMenuItem});
            resources.ApplyResources(this.toolMailCheckButton, "toolMailCheckButton");
            this.toolMailCheckButton.Name = "toolMailCheckButton";
            this.toolMailCheckButton.ButtonClick += new System.EventHandler(this.messageCheck_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            resources.ApplyResources(this.testToolStripMenuItem, "testToolStripMenuItem");
            // 
            // nullpoToolStripMenuItem
            // 
            this.nullpoToolStripMenuItem.Name = "nullpoToolStripMenuItem";
            resources.ApplyResources(this.nullpoToolStripMenuItem, "nullpoToolStripMenuItem");
            // 
            // toolMessageListButton
            // 
            this.toolMessageListButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolMessageListButton, "toolMessageListButton");
            this.toolMessageListButton.Name = "toolMessageListButton";
            this.toolMessageListButton.Click += new System.EventHandler(this.messageListMenu_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.notifyOpenInboxMenu_Click);
            // 
            // logFontDialog
            // 
            this.logFontDialog.AllowVerticalFonts = false;
            this.logFontDialog.ShowApply = true;
            this.logFontDialog.ShowEffects = false;
            this.logFontDialog.Apply += new System.EventHandler(this.logFontDialog_Apply);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.logMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.notifyMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripStatusLabel latestCheckTimeStatus;
		private System.Windows.Forms.ToolStripMenuItem mainMenu;
		private System.Windows.Forms.ToolStripMenuItem mainSettingMenu;
		private System.Windows.Forms.ToolStripMenuItem helpMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mainConnectMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem mainExitMenu;
		private System.Windows.Forms.ToolStripMenuItem mainConnectStartMenu;
		private System.Windows.Forms.ToolStripMenuItem mainConnectPauseMenu;
		private System.Windows.Forms.ToolStripMenuItem helpAboutMenu;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;
		private System.Windows.Forms.ToolStripMenuItem notifySettingMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem notifyOpenInboxMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyMailCheckMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyMessageListMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem notifyConnectMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem notifyAboutMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyOfficialSiteMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem notifyExitMenu;
		private System.Windows.Forms.ContextMenuStrip logMenu;
		private System.Windows.Forms.ToolStripMenuItem logCopyMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem logDeleteMenu;
		private System.Windows.Forms.ToolStripMenuItem logAllSelectMenu;
		private System.Windows.Forms.RichTextBox logBox;
		private System.Windows.Forms.NotifyIcon notify;
		private System.Windows.Forms.ToolStripMenuItem messageMenu;
		private System.Windows.Forms.ToolStripMenuItem messageCheck;
		private System.Windows.Forms.ToolStripMenuItem messageListMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyMainWindowMenu;
		private System.Windows.Forms.ToolStripMenuItem mainHideMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem messageViewInbox;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolSettingButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripButton toolMessageListButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem logFontMenu;
		private System.Windows.Forms.FontDialog logFontDialog;
		private System.Windows.Forms.ToolStripSplitButton toolMailCheckButton;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem nullpoToolStripMenuItem;
	}
}