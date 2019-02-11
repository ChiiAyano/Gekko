using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Gekko.Forms
{
	public partial class frmNewMessage : Form
	{
		string labelText = string.Empty;

		#region 非アクティブでウィンドウを表示させる

		const int HWND_TOPMOST = -1;
		const int SWP_NOACTIVE = 0x0010;
		const int SW_SHOWNOACTIVE = 4;

		[DllImport("user32.dll")]
		private extern static bool ShowWindow(IntPtr hWnd, int flags);
		[DllImport("user32.dll")]
		private extern static bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
			int x, int y, int cx, int cy, uint uFlags);

		#endregion

		public frmNewMessage(string text)
		{
			InitializeComponent();

			labelText = text;
//#if DEBUG
//            this.Location = new Point(100, 100);
//#else
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
				Screen.PrimaryScreen.WorkingArea.Height - this.Height);
//#endif
		}

		protected override bool ShowWithoutActivation
		{
			get
			{
				return true;
			}
		}

		public bool TimerEnabled
		{
			get
			{
				return closeTimer.Enabled;
			}
			set
			{
				closeTimer.Enabled = value;
			}
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case Win32.WM_PAINT:
					// ガラス効果が使えるか確認
					OperatingSystem os = Environment.OSVersion;
					if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6) &&
						(DwmApi.DwmIsCompositionEnabled()))
					{
						this.BackColor = Color.Black;
						extendedPanel1.Visible = true;
						messageLabel.Visible = false;

						Win32.MARGINS mar = new Win32.MARGINS();
						mar.m_Left = -1;
						mar.m_Right = 0;
						mar.m_Bottom = 0;
						mar.m_Top = 0;
						Win32.DwmExtendFrameIntoClientArea(this.Handle, ref mar);

						// 文字描画
						SizeF textsize = extendedPanel1.CreateGraphics().MeasureString(labelText, new Font(Properties.Resources.AboutFont, 12));
						extendedPanel1.DrawText = labelText;
						extendedPanel1.TextLocation = new Point(0, 0);
						extendedPanel1.TextSize = textsize.ToSize();
						extendedPanel1.GlowSize = 7;
						this.Size = new
							Size((int)textsize.Width + extendedPanel1.GlowSize + 10, (int)textsize.Height + extendedPanel1.GlowSize + 15);
						this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
							Screen.PrimaryScreen.WorkingArea.Height - this.Height - 2);

					}
					else
					{
						this.BackColor = SystemColors.Control;
						this.Opacity = 0.9;
						this.Padding = new Padding(10);
						messageLabel.Text = labelText;
						messageLabel.Visible = true;

						this.ClientSize = new Size(messageLabel.Size.Width + 50, messageLabel.Height + 10);
						this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
							Screen.PrimaryScreen.WorkingArea.Height - this.Height);

					}
					break;
				default:
					int wm_syscommand = 0x0112;
					int sc_move = 0xF010;
					if ((m.Msg == wm_syscommand) && ((m.WParam.ToInt32() & 0xFFF0) == sc_move))
					{
						m.Result = IntPtr.Zero;
						return;
					}
					break;

			}

			base.WndProc(ref m);

		}

		private void frmNewMessage_Load(object sender, EventArgs e)
		{

//#if DEBUG
//            SetWindowPos(this.Handle, 0, 100, 100, this.Width, this.Height, SWP_NOACTIVE);
//#else
			SetWindowPos(this.Handle, -1,
			    Screen.PrimaryScreen.WorkingArea.Width - this.Width,
			    Screen.PrimaryScreen.WorkingArea.Height - this.Height,
			    this.Width, this.Height, SWP_NOACTIVE);
//#endif
			closeTimer.Interval = ReadSetting.Setting.PopupViewTime;
			closeTimer.Start();
		}

		private void frmNewMessage_Move(object sender, EventArgs e)
		{
			
		}

		private void closeTimer_Tick(object sender, EventArgs e)
		{
			this.Close();
		}

		private void extendedPanel1_Click(object sender, EventArgs e)
		{
			// タイマー停止
			closeTimer.Stop();

			// 設定で決めた動作をおこなう
			switch (ReadSetting.Setting.BalloonClickOperation)
			{
				case ConfigData.BalloonClick.MessageList:
					//frmMessageViewer.Instance.Show();
					//frmMessageViewer.Instance.Activate();
					Process.Start(Path.Combine(Application.StartupPath, "MessageList.exe"));
					this.Close();
					break;
				case ConfigData.BalloonClick.RunBrowser:
					frmAccountSelect fas = new frmAccountSelect(ReadSetting.Setting.Accounts);
					if (fas.ShowDialog() == DialogResult.OK)
					{
						if (ReadSetting.Setting.NoUseSsl)
							BrowserShow.browserOpen("http://" + fas.Account.GetInboxUri);
						else
							BrowserShow.browserOpen("https://" + fas.Account.GetInboxUri);
					}
					this.Close();
					break;
				case ConfigData.BalloonClick.RunExecutive:
					runApplication();
					this.Close();
					break;
				default:
					// 何もせず閉じる
					this.Close();
					break;
			}
		}

		private void runApplication()
		{
			try
			{
				System.Diagnostics.Process.Start(ReadSetting.Setting.NotificationRunExecutiveFilePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(Properties.Resources.BrowserRunError + "\n" + ex.Message, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
