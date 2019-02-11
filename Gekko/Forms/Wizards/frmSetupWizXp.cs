using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;

namespace Gekko.Forms.Wizards
{
	public partial class frmSetupWizXp : Form
	{
		ConfigData cd = new ConfigData();
		int currentPanelNo = 0;

		public frmSetupWizXp()
		{
			InitializeComponent();
			
			// デザイナでは設定できない部分
			titleLabel.Font = new Font(titleLabel.Font, FontStyle.Bold);
			label2.Font = new Font(Control.DefaultFont.FontFamily, 14F, FontStyle.Bold);
			label19.Font = new Font(Control.DefaultFont.FontFamily, 14F, FontStyle.Bold);

			// 2番目
			scDomainBox.SelectedIndex = 0;
			// 3番目
			thIntervalBox.Value = cd.MailCheckInterval / 1000 / 60;
			// 4番目
			foBalloonCheck.Enabled = Program.EnableBalloon;
			label13.Enabled = Program.EnableBalloon;
			foPopupCheck.Checked = !Program.EnableBalloon;
			foBalloonCheck.Checked = (cd.NewMailNotifyOperation == ConfigData.NewMailNotification.Balloon) ? true : false;
			foPopupCheck.Checked = !foBalloonCheck.Checked;
			// 5番目
			fiSoundUseCheck.Checked = cd.PlayNewMailSound;
			fiDefaultSoundCheck.Checked = cd.PlayDefaultNewMailSound;
			fiSelectSoundCheck.Checked = !cd.PlayDefaultNewMailSound;

			// 1番目を表示
			ChangePanel(0);
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


		private void frmSetupWizXp_Load(object sender, EventArgs e)
		{

		}

		#region パネル変遷

		private void ChangePanel(int panelNo)
		{
			switch (panelNo)
			{
				case 0: // 1番目
					firstPanel.Visible = true;
					setPanel.Visible = false;
					lastPanel.Visible = false;
					nextButton.Enabled = true;
					break;
				case 1: // 2番目
					firstPanel.Visible = false;
					setPanel.Visible = true;
					secondPanel.Visible = true;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					lastPanel.Visible = false;
					scAccountBox_TextChanged(null, null);
					titleLabel.Text = Properties.Resources.WizardSecondTitle;
					break;
				case 2: // 3番目
					firstPanel.Visible = false;
					setPanel.Visible = true;
					secondPanel.Visible = false;
					thirdPanel.Visible = true;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					lastPanel.Visible = false;
					titleLabel.Text = Properties.Resources.WizardThirdTitle;
					break;
				case 3: // 4番目
					firstPanel.Visible = false;
					setPanel.Visible = true;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = true;
					fifthPanel.Visible = false;
					lastPanel.Visible = false;
					titleLabel.Text = Properties.Resources.WizardForthTitle;
					break;
				case 4: // 5番目
					firstPanel.Visible = false;
					setPanel.Visible = true;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = true;
					lastPanel.Visible = false;
					titleLabel.Text = Properties.Resources.WizardFifthTitle;
					break;
				case 5: // 6番目
					firstPanel.Visible = false;
					setPanel.Visible = false;
					secondPanel.Visible = false;
					thirdPanel.Visible = false;
					forthPanel.Visible = false;
					fifthPanel.Visible = false;
					lastPanel.Visible = true;
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
			if (currentPanelNo == 5)
			{
				Save();
				this.Close();
			}
			else
			{
				ChangePanel(++currentPanelNo);
				if (currentPanelNo == 1)
					previousButton.Enabled = true;
				if (currentPanelNo == 5)
					nextButton.Text = Properties.Resources.WizardCompleteButton;
				else
					nextButton.Text = Properties.Resources.WizardNextButton;
			}
		}

		private void Save()
		{
			// 2番目
			Account a = new Account { SetName="既定", AccountName = scAccountBox.Text, Domain = scDomainBox.Text, Password = Password.Encrypt(scPassBox.Text, scAccountBox.Text) };
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

	}
}
