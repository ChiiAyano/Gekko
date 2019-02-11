using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;
using System.Runtime.InteropServices;

namespace Gekko.Forms.Wizards
{
	public partial class frmSetupWizVista : Form
	{
		[DllImport("User32.dll")]
		private extern static long SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
		[DllImport("User32.dll")]
		private extern static void ReleaseCapture();
		const int WM_NClBUTTONDOWN = 0xa1;
		const int HTCAPTION = 2;

		ConfigData cd = new ConfigData();
		int currentPanelNo = 0;

		public frmSetupWizVista()
		{
			InitializeComponent();

			Color titleCl = Color.RoyalBlue;
			fsTitleLabel.ForeColor = titleCl;
			fsTitleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f);
			scTitleLabel.ForeColor = titleCl;
			scTitleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f);
			thTitleLabel.ForeColor = titleCl;
			thTitleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f);
			foTitleLabel.ForeColor = titleCl;
			foTitleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f);
			fiTitleLabel.ForeColor = titleCl;
			fiTitleLabel.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f);
			extendedPanel1.Font = SystemFonts.CaptionFont;
			firstPanel.Font = SystemFonts.MessageBoxFont;
			secondPanel.Font = SystemFonts.MessageBoxFont;
			thirdPanel.Font = SystemFonts.MessageBoxFont;
			forthPanel.Font = SystemFonts.MessageBoxFont;
			fifthPanel.Font = SystemFonts.MessageBoxFont;
			nextButton.Font = SystemFonts.MessageBoxFont;

			// 2番目
			scDomainBox.SelectedIndex = 0;
			// 3番目
			thIntervalBox.Value = cd.MailCheckInterval / 1000 / 60;
			// 4番目
			foBalloonCheck.Enabled = Program.EnableBalloon;
			label13.Enabled = Program.EnableBalloon;
			pictureBox3.Enabled = Program.EnableBalloon;
			foPopupCheck.Checked = !Program.EnableBalloon;
			foBalloonCheck.Checked = (cd.NewMailNotifyOperation == ConfigData.NewMailNotification.Balloon) ? true : false;
			foPopupCheck.Checked = !foBalloonCheck.Checked;
			// 5番目
			fiSoundUseCheck.Checked = cd.PlayNewMailSound;
			fiDefaultSoundCheck.Checked = cd.PlayDefaultNewMailSound;
			fiSelectSoundCheck.Checked = !cd.PlayDefaultNewMailSound;

			// 1番目を表示
			ChangePanel(0);

			Win32.WTA_OPTIONS wtaopt = new Win32.WTA_OPTIONS();
			wtaopt.dwFlags = 3;
			wtaopt.dwMask = 3;
			Win32.SetWindowThemeAttribute(this.Handle, Win32.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref wtaopt, (uint)Marshal.SizeOf(typeof(Win32.WTA_OPTIONS)));

		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (Win32.DwmIsCompositionEnabled())
			{
				this.BackColor = Color.Black;
				this.dwmDisTitleLabel.Visible = false;

				Win32.MARGINS m = new Win32.MARGINS { m_Top = 41, m_Left = 0, m_Right = 0, m_Bottom = 0 };
				Win32.DwmExtendFrameIntoClientArea(this.Handle, ref m);

				Win32.WTA_OPTIONS wtaopt = new Win32.WTA_OPTIONS();
				wtaopt.dwFlags = 3;
				wtaopt.dwMask = 3;

				extendedPanel1.TextSize = extendedPanel1.Size;
				Win32.SetWindowThemeAttribute(this.Handle, Win32.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref wtaopt, (uint)Marshal.SizeOf(typeof(Win32.WTA_OPTIONS)));
				//Win32.SetWindowThemeNonClientAttributes(this.Handle, 3, 3);
			}
			else
			{
				this.dwmDisTitleLabel.Font = new Font(SystemFonts.CaptionFont.FontFamily, this.dwmDisTitleLabel.Font.Size, FontStyle.Regular);
			}
		}

		private void frmSetupWizVista_Load(object sender, EventArgs e)
		{

		}

		// 閉じさせない
		protected override CreateParams CreateParams
		{
			get
			{
				const int CS_NOCLOSE = 0x200;
				CreateParams cp = base.CreateParams;
				cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;

				return cp;
			}
		}

		#region パネル変遷

		private void ChangePanel(int panelNo)
		{
			switch (panelNo)
			{
				case 0: // 1番目
					firstPanel.Visible = true;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					nextButton.Enabled = true;
					break;
				case 1: // 2番目
					firstPanel.Visible = false;
					secondPanel.Visible = true;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					scAccountBox_TextChanged(null, null);
					break;
				case 2: // 3番目
					firstPanel.Visible = false;
					secondPanel.Visible = false;
					thirdPanel.Visible = true;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					break;
				case 3: // 4番目
					firstPanel.Visible = false;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = true;
					fifthPanel.Visible = false;
					break;
				case 4: // 5番目
					firstPanel.Visible = false;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = true;
					break;
			}

			currentPanelNo = panelNo;
		}

		#endregion

		#region 2番目

		delegate Exception mailTestDelegate(string account, string password, string domain);
		mailTestDelegate mtd;


		private void scTestButton_Click(object sender, EventArgs e)
		{
			mtd = new mailTestDelegate(mailTest);
			IAsyncResult ar = mtd.BeginInvoke(scAccountBox.Text, Password.Encrypt(scPassBox.Text, scAccountBox.Text), scDomainBox.Text,
				new AsyncCallback(accountConnectTestInvokeMethod), null);

			// 接続中の旨を伝える
			scTestResultLabel.Text = Properties.Resources.AccountSettingTestConnecting;
			nextButton.Enabled = false;
			previousButton.Enabled = false;
			scTestButton.Enabled = false;
		}

		private void accountConnectTestInvokeMethod(IAsyncResult ar)
		{
			// 応答を待つ
			Exception ex = mtd.EndInvoke(ar);
			//mf = mailTest(accountNameBox.Text, Password.Encrypt(accountPasswordBox.Text, accountNameBox.Text), accountDomainBox.Text);

			if (ex == null)
			{
				this.Invoke((MethodInvoker)delegate()
				{
					scTestResultLabel.Text = Properties.Resources.AccountSettingTestSucceeded;
				});
			}
			else
			{
				this.Invoke((MethodInvoker)delegate()
				{
					scTestResultLabel.Text = "Error: " + ex.Message;
				});
			}

			this.Invoke((MethodInvoker)delegate()
			{
				nextButton.Enabled = true;
				previousButton.Enabled = true;
				scTestButton.Enabled = true;
			});
		}

		private Exception mailTest(string account, string password, string domain)
		{
			try
			{
				MailCheck mc = new MailCheck(account, password, domain);
				mc.Check();
			}
			catch (Exception ex)
			{
				return ex;
			}
			return null;
		}

		private void scAccountBox_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(scAccountBox.Text) ||
				string.IsNullOrEmpty(scPassBox.Text) ||
				string.IsNullOrEmpty(scDomainBox.Text))
			{
				scTestButton.Enabled = false;
				nextButton.Enabled = false;
			}
			else
			{
				scTestButton.Enabled = true;
				nextButton.Enabled = true;
			}
		}

		#endregion


		#region 5番目

		private void fiSoundUseCheck_CheckedChanged(object sender, EventArgs e)
		{
			fiSoundPanel.Enabled = fiSoundUseCheck.Checked;
		}

		private void fiDefaultSoundCheck_CheckedChanged(object sender, EventArgs e)
		{
			fiSelectSoundPanel.Enabled = !fiDefaultSoundCheck.Checked;
			fiDefaultSoundListenButton.Enabled = fiDefaultSoundCheck.Checked;
		}

		private void fiDefaultSoundListenButton_Click(object sender, EventArgs e)
		{
			// 試聴
			PlaySounds.Play("MailBeep");
		}

		private void fiSelectSoundButton_Click(object sender, EventArgs e)
		{
			if (fiSelectSoundDialog.ShowDialog() == DialogResult.OK)
			{
				fiSelectSoundPathBox.Text = fiSelectSoundDialog.FileName;
				fiSelectSoundListenButton.Enabled = true;
			}
		}

		private void fiSelectSoundListenButton_Click(object sender, EventArgs e)
		{
			// 試聴
			PlaySounds.Play(fiSelectSoundPathBox.Text);
		}

		#endregion

		private void previousButton_Click(object sender, EventArgs e)
		{
			ChangePanel(--currentPanelNo);
			if (currentPanelNo == 0)
				previousButton.Enabled = false;
			else
				previousButton.Enabled = true;

			nextButton.Text = Properties.Resources.WizardNextButton;
		}

		private void nextButton_Click(object sender, EventArgs e)
		{
			if (currentPanelNo == 4)
			{
				Save();
				this.Close();
			}
			else
			{
				ChangePanel(++currentPanelNo);
				if (currentPanelNo == 1)
					previousButton.Enabled = true;
				if (currentPanelNo == 4)
					nextButton.Text = Properties.Resources.WizardCompleteButton;
				else
					nextButton.Text = Properties.Resources.WizardNextButton;
			}
		}

		private void Save()
		{
			// 2番目
			Account a = new Account { SetName = "既定", AccountName = scAccountBox.Text, Domain = scDomainBox.Text, Password = Password.Encrypt(scPassBox.Text, scAccountBox.Text) };
			cd.Accounts.Add(a);
			// 3番目
			cd.MailCheckInterval = (int)thIntervalBox.Value * 60 * 1000;
			// 4番目
			cd.NewMailBalloonOrPopupNotify = true;
			cd.NewMailNotifyOperation = foBalloonCheck.Checked ? ConfigData.NewMailNotification.Balloon : ConfigData.NewMailNotification.Popup;
			// 5番目
			cd.PlayNewMailSound = fiSoundUseCheck.Checked;
			cd.PlayDefaultNewMailSound = fiDefaultSoundCheck.Checked;
			cd.PlayNewMailSoundPath = fiSelectSoundPathBox.Text;

			ReadSetting.Setting = cd;
			ReadSetting.Save();
		}

		private void frmSetupWizVista_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WM_NClBUTTONDOWN, HTCAPTION, 0);
			}
		}
	}
}
