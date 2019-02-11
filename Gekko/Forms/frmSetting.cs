using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Gekko.Libraries;
using IWshRuntimeLibrary;

namespace Gekko.Forms
{
    public partial class frmSetting : Form
    {
        Assembly asm;
        AssemblyName asmName;
		ConfigData s_config;

        // 定数
        readonly string SHORTCUT_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Gekko.lnk");


        public frmSetting()
        {
            InitializeComponent();
            asm = Assembly.GetExecutingAssembly();
            asmName = asm.GetName();

			// メニューの書式変更
			accountEditMenu.Font = new Font(accountEditMenu.Font, FontStyle.Bold);
        }

		private void formCloseAdmit()
		{
			// アカウント設定が0だったら、閉じれないようにしておく
			if (ReadSetting.Setting.Accounts.Count == 0)
			{
				this.ControlBox = false;
				cancelButton.Enabled = false;
			}
			else
			{
				this.ControlBox = true;
				cancelButton.Enabled = true;
			}
		}

        private void frmSetting_Load(object sender, EventArgs e)
        {
            // フォーム上に反映
			s_config = ReadSetting.Read();

            // アカウント
			foreach (Account a in s_config.Accounts)
            {
                accountListBox.Items.Add((string)a.SetName + " (" + a.AccountName + "@" + a.Domain + ")");
            }

            // メールチェック
            mailMailCheckNumeric.Value = (decimal)s_config.MailCheckInterval / 1000 / 60;
            mailPowerResumedDelayNumeric.Value = (decimal)s_config.PowerResumedDelayTime / 1000;

            // プロキシ
            proxyUseProxyCheck.Checked = s_config.UseProxy;
            proxyUseIeProxyCheck.Checked = s_config.UseIeProxy;
            proxyUseSelectProxyCheck.Checked = s_config.UseIeProxy ? false : true;
            proxySelectProxyBox.Text = s_config.ProxyHostName;
            proxySelectPortNumeric.Value = (decimal)s_config.ProxyPort;

            // アクセス
            accessNoSslCheck.Checked = s_config.NoUseSsl;

            // サウンド
            soundUseNewMailSoundCheck.Checked = s_config.PlayNewMailSound;
            soundUseDefaultNewMailSoundCheck.Checked = s_config.PlayDefaultNewMailSound;
            soundUseSelectNewMailSoundCheck.Checked = s_config.PlayDefaultNewMailSound ? false : true;
            soundSelectSoundFileBox.Text = s_config.PlayNewMailSoundPath;

            // 新着通知
            newNotifyBalloonCheck.Checked = s_config.NewMailBalloonOrPopupNotify;
            newRunApplicationCheck.Checked = s_config.NewMailBalloonOrPopupNotify ? false : true;
            newNotifySelectBox.SelectedIndex = (int)s_config.NewMailNotifyOperation;
			newNotifySelectBox.Enabled = Program.EnableBalloon;
            newNotifyClickBox.SelectedIndex = (int)s_config.BalloonClickOperation;
            newSelectRunApplicationFileBox.Text = s_config.RunApplicationPath;
            newSelectRunApplicationOptionBox.Text = s_config.RunApplicationOption;
			popupViewTimeBox.Value = s_config.PopupViewTime / 1000;
			notifyClickApplicationBox.Text = s_config.NotificationRunExecutiveFilePath;

			// 表示
			balloonInvisibleErrorCheck.Checked = s_config.BalloonInvisibleError;

            // ブラウザ
            browserUseDefaultBrowserCheck.Checked = s_config.UseDefaultBrowser;
            browserUseSelectBrowserCheck.Checked = !s_config.UseDefaultBrowser;
            browserSelectFileBox.Text = s_config.UseBrowserPath;

			// メッセージ一覧
			messageListFontSettingLabel.Text = string.Format(Properties.Resources.SettingMessageListFontInfo, s_config.MessageListFont.Name, s_config.MessageListFont.Size.ToString());
			messageListFontDialog.Font = s_config.MessageListFont;
			titleColorPick.BackColor = s_config.MessageListTitleColor;
			senderColorPick.BackColor = s_config.MessageListSenderColor;
			summaryColorPick.BackColor = s_config.MessageListSummaryColor;
			nonSelectColorPick.BackColor = s_config.MessageListNonSelectColor;
			nonSelectOddColorPick.BackColor = s_config.MessageListNonSelectOddColor;
			selectColorPick.BackColor = s_config.MessageListSelectColor;
			messageListSenderFontSizeBox.Value = (decimal)s_config.MessageListSenderFontSize;
			messageListRightCheck.Checked = s_config.MessageListRightList;

			// メインコンソール
			hideConsoleCheck.Checked = s_config.HideConsole;

			// 通知領域設定
			notifyDoubleClickBox.SelectedIndex = (int)s_config.NotifyDoubleClickOperation;
			disableAnimationCheck.Checked = s_config.DisableNotifyAnimation;

            // スタートアップ
            shortcutButtonSetting();
			
            // チェックボックス関連の強制実行
            proxyUseProxyCheck_CheckedChanged(null, null);
            proxyUseIeProxyCheck_CheckedChanged(null, null);
            accountListBox_SelectedIndexChanged(null, null);
			newNotifyClickBox_SelectedIndexChanged(null, null);

			soundUseDefaultNewMailSoundCheck_CheckedChanged(null, null);
			soundUseNewMailSoundCheck_CheckedChanged(null, null);

			// メッセージ一覧表示プレビューで、2項目目を選択した状態にしておく
			messageListPreviewBox.SelectedIndex = 1;

			// アカウント設定が0だったら、閉じれないようにしておく
			formCloseAdmit();

			// フォーカスをタブに
			tabControl1.Focus();
        }

        #region OK, キャンセル

        private void okButton_Click(object sender, EventArgs e)
        {
            // アカウント設定がなければ警告する
            if (accountListBox.Items.Count == 0)
            {
                MessageBox.Show(Properties.Resources.SettingNoAccountError, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 設定を格納
            // メールチェック
            s_config.MailCheckInterval = (int)mailMailCheckNumeric.Value * 1000 * 60;
            s_config.PowerResumedDelayTime = (int)mailPowerResumedDelayNumeric.Value * 1000;

            // プロキシ
            s_config.UseProxy = proxyUseProxyCheck.Checked;
            s_config.UseIeProxy = proxyUseIeProxyCheck.Checked;
            s_config.UseIeProxy = proxyUseSelectProxyCheck.Checked ? false : true;
            s_config.ProxyHostName = proxySelectProxyBox.Text;
            s_config.ProxyPort = (int)proxySelectPortNumeric.Value;

            // アクセス
            s_config.NoUseSsl = accessNoSslCheck.Checked;

            // サウンド
            s_config.PlayNewMailSound = soundUseNewMailSoundCheck.Checked;
            s_config.PlayDefaultNewMailSound = soundUseDefaultNewMailSoundCheck.Checked;
            s_config.PlayDefaultNewMailSound = soundUseSelectNewMailSoundCheck.Checked ? false : true;
            s_config.PlayNewMailSoundPath = soundSelectSoundFileBox.Text;

            // 新着通知
            s_config.NewMailBalloonOrPopupNotify = newNotifyBalloonCheck.Checked;
            s_config.NewMailBalloonOrPopupNotify = newRunApplicationCheck.Checked ? false : true;
            s_config.NewMailNotifyOperation = (ConfigData.NewMailNotification)newNotifySelectBox.SelectedIndex;
            s_config.BalloonClickOperation = (ConfigData.BalloonClick)newNotifyClickBox.SelectedIndex;
			s_config.NotificationRunExecutiveFilePath = notifyClickApplicationBox.Text;
            s_config.RunApplicationPath = newSelectRunApplicationFileBox.Text;
            s_config.RunApplicationOption = newSelectRunApplicationOptionBox.Text;
			s_config.PopupViewTime = (int)popupViewTimeBox.Value * 1000;
			s_config.NotificationRunExecutiveFilePath = notifyClickApplicationBox.Text;

			// 表示
			s_config.BalloonInvisibleError = balloonInvisibleErrorCheck.Checked;

            // ブラウザ
            s_config.UseDefaultBrowser = browserUseDefaultBrowserCheck.Checked;
            s_config.UseDefaultBrowser = browserUseSelectBrowserCheck.Checked ? false : true;
            s_config.UseBrowserPath = browserSelectFileBox.Text;

			// メッセージ一覧
			s_config.MessageListTitleColor = titleColorPick.BackColor;
			s_config.MessageListSenderColor = senderColorPick.BackColor;
			s_config.MessageListSummaryColor = summaryColorPick.BackColor;
			s_config.MessageListNonSelectColor = nonSelectColorPick.BackColor;
			s_config.MessageListNonSelectOddColor = nonSelectOddColorPick.BackColor;
			s_config.MessageListSelectColor = selectColorPick.BackColor;
			s_config.MessageListSenderFontSize = (float)messageListSenderFontSizeBox.Value;
			s_config.MessageListRightList = messageListRightCheck.Checked;

			// メインコンソール
			s_config.HideConsole = hideConsoleCheck.Checked;

			// 通知領域
			s_config.NotifyDoubleClickOperation = (ConfigData.NotifyDoubleClick)notifyDoubleClickBox.SelectedIndex;
			s_config.DisableNotifyAnimation = disableAnimationCheck.Checked;

			ReadSetting.Setting = s_config;

            // 設定を保存
			ReadSetting.Save();

			this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region 接続設定

		// 右クリックしたときの番号
		int accountlist_Selected = -1;
		/// <summary>
		/// アカウントの有効化・無効化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void accountUseMenu_Click(object sender, EventArgs e)
		{
			if (accountlist_Selected >= 0)
			{
				if (s_config.Accounts[accountlist_Selected].Use)
				{
					s_config.Accounts[accountlist_Selected].Use = false;
					accountUseMenu.Text = Properties.Resources.AccountListEnable;
				}
				else
				{
					s_config.Accounts[accountlist_Selected].Use = true;
					accountUseMenu.Text = Properties.Resources.AccountListDisable;
				}

				accountListBox.SelectedIndex = -1;
				accountListBox.Refresh();
			}
		}

		private void accountListBox_MouseUp(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				ListBox l = (ListBox)sender;
				accountlist_Selected = l.IndexFromPoint(e.Location);

				if (accountlist_Selected >= 0)
				{
					if (s_config.Accounts[accountlist_Selected].Use)
						accountUseMenu.Text = Properties.Resources.AccountListDisable;
					else
						accountUseMenu.Text = Properties.Resources.AccountListEnable;

					List<Account> disableA = s_config.Accounts.FindAll(delegate(Account a)
					{
						return !a.Use;
					});

					if ((disableA.Count == s_config.Accounts.Count - 1) && (s_config.Accounts[accountlist_Selected].Use))
						accountUseMenu.Enabled = false;
					else
						accountUseMenu.Enabled = true;

					l.SelectedIndex = accountlist_Selected;
					accountListContext.Show(l, e.Location);
				}
			}
		}

		private void accountListBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (accountListBox.IndexFromPoint(e.Location) == -1)
			{
				accountListBox.ClearSelected();
			}
		}

        /// <summary>
        /// アカウント設定を追加
        /// </summary>
        private void accountAddButton_Click(object sender, EventArgs e)
        {
            frmAccountSetting fas = new frmAccountSetting(null);
            if (fas.ShowDialog() == DialogResult.OK)
            {
				s_config.Accounts.Add(fas.Result);
                accountListBox.Items.Add(fas.Result.SetName + " (" + fas.Result.AccountName + "@" + fas.Result.Domain + ")");
                accountListBox.Refresh();

				// アカウント設定が0だったら、閉じれないようにしておく
				formCloseAdmit();
            }
        }

        /// <summary>
        /// アカウント設定を変更
        /// </summary>
        private void accountChangeButton_Click(object sender, EventArgs e)
        {
            if (accountListBox.SelectedIndex > -1)
            {
				frmAccountSetting fas = new frmAccountSetting(s_config.Accounts[accountListBox.SelectedIndex]);
                if (fas.ShowDialog() == DialogResult.OK)
                {
					s_config.Accounts[accountListBox.SelectedIndex] = fas.Result;
                    accountListBox.Items[accountListBox.SelectedIndex] = fas.Result.SetName + " (" + fas.Result.AccountName + "@" + fas.Result.Domain + ")";
                    accountListBox.Refresh();

					// アカウント設定が0だったら、閉じれないようにしておく
					formCloseAdmit();
                }
            }
        }

        /// <summary>
        /// アカウント設定を削除
        /// </summary>
        private void accountRemoveButton_Click(object sender, EventArgs e)
        {
            if (accountListBox.SelectedIndex > -1)
            {
                DialogResult dr = MessageBox.Show(Properties.Resources.SettingAccountDelete, Properties.Resources.SoftwareName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
					s_config.Accounts.RemoveAt(accountListBox.SelectedIndex);
                    accountListBox.Items.RemoveAt(accountListBox.SelectedIndex);

					// 使用しないアカウントだけになったときは強制的に使用するようにする
					List<Account> disableA = s_config.Accounts.FindAll(delegate(Account a)
					{
						return !a.Use;
					});

					if (disableA.Count == s_config.Accounts.Count)
						s_config.Accounts[0].Use = true;


                    accountListBox.Refresh();

					// アカウント設定が0だったら、閉じれないようにしておく
					formCloseAdmit();
                }
            }
        }


        /// <summary>
        /// アカウントリストの挙動にあわせてボタンの有効・無効化設定
        /// </summary>
        private void accountListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accountListBox.SelectedIndex < 0)
            {
                accountChangeButton.Enabled = false;
                accountRemoveButton.Enabled = false;
				accountListUpButton.Enabled = false;
				accountListDownButton.Enabled = false;
            }
            else
            {
                accountChangeButton.Enabled = true;
                accountRemoveButton.Enabled = true;
				if (accountListBox.Items.Count == 1)
				{
					accountListUpButton.Enabled = false;
					accountListDownButton.Enabled = false;
				}
				else if (accountListBox.SelectedIndex == 0)
				{
					accountListUpButton.Enabled = false;
					accountListDownButton.Enabled = true;
				}
				else if (accountListBox.SelectedIndex == accountListBox.Items.Count - 1)
				{
					accountListUpButton.Enabled = true;
					accountListDownButton.Enabled = false;
				}
				else
				{
					accountListUpButton.Enabled = true;
					accountListDownButton.Enabled = true;
				}
            }
        }

		/// <summary>
		/// アカウントリストの項目を上にもっていく
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void accountListUpButton_Click(object sender, EventArgs e)
		{
			if (accountListBox.SelectedIndex > 0)
			{
				int afterSelectedIndex = accountListBox.SelectedIndex - 1;

				s_config.Accounts.Move(accountListBox.SelectedIndex, accountListBox.SelectedIndex - 1);
				// リストの置き換え
				accountListBox.BeginUpdate();
				object item = accountListBox.Items[accountListBox.SelectedIndex - 1];
				accountListBox.Items[accountListBox.SelectedIndex - 1] = accountListBox.Items[accountListBox.SelectedIndex];
				accountListBox.Items[accountListBox.SelectedIndex] = item;
				accountListBox.EndUpdate();
				accountListBox.SelectedIndex = afterSelectedIndex;
			}
		}
		/// <summary>
		/// アカウントリストの項目を下にもっていく
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void accountListDownButton_Click(object sender, EventArgs e)
		{
			if (accountListBox.SelectedIndex < accountListBox.Items.Count - 1)
			{
				int afterSelectedIndex = accountListBox.SelectedIndex + 1;

				s_config.Accounts.Move(accountListBox.SelectedIndex, accountListBox.SelectedIndex + 1);
				// リストの置き換え
				accountListBox.BeginUpdate();
				object item = accountListBox.Items[accountListBox.SelectedIndex + 1];
				accountListBox.Items[accountListBox.SelectedIndex + 1] = accountListBox.Items[accountListBox.SelectedIndex];
				accountListBox.Items[accountListBox.SelectedIndex] = item;
				accountListBox.EndUpdate();
				accountListBox.SelectedIndex = afterSelectedIndex;
			}
		}

        /// <summary>
        /// プロキシの利用をするかのチェックボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proxyUseProxyCheck_CheckedChanged(object sender, EventArgs e)
        {
            // これがチェック入っているならその下の項目を有効化
            if ((proxyUseProxyCheck.Checked))
            {
                proxyUseIeProxyCheck.Enabled = true;
                proxyUseSelectProxyCheck.Enabled = true;
            }
            else
            {
                // 入っていなければ無効化
                proxyUseIeProxyCheck.Enabled = false;
                proxyUseSelectProxyCheck.Enabled = false;
            }
        }

        /// <summary>
        /// プロキシはIEの設定を使用するかのチェックボックス
        /// </summary>
        private void proxyUseIeProxyCheck_CheckedChanged(object sender, EventArgs e)
        {
            if ((proxyUseIeProxyCheck.Checked) || (!proxyUseIeProxyCheck.Enabled))
            {
                // チェックがある、もしくはチェックが無効なら任意設定の部分を無効化
                proxySelectPanel.Enabled = false;
            }
            else
            {
                // チェックがなければ任意設定の部分を有効化
                proxySelectPanel.Enabled = true;
            }
        }

        #endregion

        #region 表示・動作設定

        private void soundUseNewMailSoundCheck_CheckedChanged(object sender, EventArgs e)
        {
            // 新着受信時にサウンドを再生するかの設定
            soundPlayPanel.Enabled = soundUseNewMailSoundCheck.Checked;
        }

        private void soundUseDefaultNewMailSoundCheck_CheckedChanged(object sender, EventArgs e)
        {
            // サウンドは既定か選択か
            if (soundUseDefaultNewMailSoundCheck.Checked)
            {
                soundListenDefaultNewMailButton.Enabled = true;
                soundSelectPanel.Enabled = false;
            }
            else
            {
                soundListenDefaultNewMailButton.Enabled = false;
                soundSelectPanel.Enabled = true;

                if (string.IsNullOrEmpty(soundSelectSoundFileBox.Text))
                { soundListenSelectSoundButton.Enabled = false; }
                else
                { soundListenSelectSoundButton.Enabled = true; }
            }
        }

        private void newNotifyBalloonCheck_CheckedChanged(object sender, EventArgs e)
        {
            // 新着受信時の通知方法
            if (newNotifyBalloonCheck.Checked)
            {
                newBalloonPanel.Enabled = true;
                newRunApplicationPanel.Enabled = false;
            }
            else
            {
                newBalloonPanel.Enabled = false;
                newRunApplicationPanel.Enabled = true;
            }
        }

        private void browserUseDefaultBrowserCheck_CheckedChanged(object sender, EventArgs e)
        {
            // ブラウザは既定か選択か
            if (browserUseDefaultBrowserCheck.Checked)
            {
                browserSelectPanel.Enabled = false;
            }
            else
            {
                browserSelectPanel.Enabled = true;
            }
        }

        private void soundListenDefaultNewMailButton_Click(object sender, EventArgs e)
        {
            // 試聴
            PlaySounds.Play("MailBeep");
        }

        private void soundSelectSoundButton_Click(object sender, EventArgs e)
        {
            // 新着サウンドを選択する
			if (string.IsNullOrEmpty(soundSelectSoundFileBox.Text))
				soundSelectDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
			else
				soundSelectDialog.InitialDirectory = Path.GetDirectoryName(soundSelectSoundFileBox.Text);

            if (soundSelectDialog.ShowDialog() == DialogResult.OK)
            {
                soundSelectSoundFileBox.Text = soundSelectDialog.FileName;
                if (!string.IsNullOrEmpty(soundSelectDialog.FileName))
                {
                    soundListenSelectSoundButton.Enabled = true;
                }
            }
        }

        private void soundListenSelectSoundButton_Click(object sender, EventArgs e)
        {
            // 試聴
            PlaySounds.Play(soundSelectSoundFileBox.Text);
        }

        private void newSelectRunApplicationButton_Click(object sender, EventArgs e)
        {
            // 開くアプリを選択する
			if (string.IsNullOrEmpty(newSelectRunApplicationFileBox.Text))
				newRunApplicationSelectDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			else
				newRunApplicationSelectDialog.InitialDirectory = Path.GetDirectoryName(newSelectRunApplicationFileBox.Text);
			
			if (newRunApplicationSelectDialog.ShowDialog() == DialogResult.OK)
            {
                newSelectRunApplicationFileBox.Text = newRunApplicationSelectDialog.FileName;
            }
        }

        private void browserSelectButton_Click(object sender, EventArgs e)
        {
            // ブラウザを選択する
			if (string.IsNullOrEmpty(browserSelectFileBox.Text))
				browserSelectDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			else
				browserSelectDialog.InitialDirectory = Path.GetDirectoryName(browserSelectFileBox.Text);

            if (browserSelectDialog.ShowDialog() == DialogResult.OK)
            {
                browserSelectFileBox.Text = browserSelectDialog.FileName;
            }
        }

		private void newNotifyClickBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 通知をクリックした時
			if (newNotifyClickBox.SelectedIndex == 3)
				runAppPanel.Enabled = true;
			else
				runAppPanel.Enabled = false;
		}

		private void notifyClickApplicationButton_Click(object sender, EventArgs e)
		{
			// 通知クリックしたらアプリケーションを実行するときの実行するファイルを指定
			if (notifyClickApplicationSelectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				notifyClickApplicationBox.Text = notifyClickApplicationSelectDialog.FileName;
			}
		}

        #endregion

		#region メッセージ一覧設定

		private void titleColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListTitleColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListTitleColor = messageListColorDialog.Color;
				titleColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}

		private void senderColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListSenderColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListSenderColor = messageListColorDialog.Color;
				senderColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}

		private void summaryColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListSummaryColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListSummaryColor = messageListColorDialog.Color;
				summaryColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}

		private void nonSelectColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListNonSelectColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListNonSelectColor = messageListColorDialog.Color;
				nonSelectColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}

		private void nonSelectOddColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListNonSelectOddColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListNonSelectOddColor = messageListColorDialog.Color;
				nonSelectOddColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}


		private void selectColorButton_Click(object sender, EventArgs e)
		{
			messageListColorDialog.Color = ReadSetting.Setting.MessageListSelectColor;
			if (messageListColorDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MessageListSelectColor = messageListColorDialog.Color;
				selectColorPick.BackColor = messageListColorDialog.Color;

				messageListPreviewBox.Refresh();
			}
		}

		private void messageListFontSettingButton_Click(object sender, EventArgs e)
		{
			messageListFontDialog.Font = ReadSetting.Setting.MessageListFont;
			if (messageListFontDialog.ShowDialog() == DialogResult.OK)
			{
				if (!messageListFontDialog.Font.FontFamily.IsStyleAvailable(FontStyle.Regular))
				{
					MessageBox.Show(Properties.Resources.SettingFontNotAvailableRegular, Properties.Resources.SoftwareName,
						MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
				}
				else
				{
					ReadSetting.Setting.MessageListFont = messageListFontDialog.Font;
					messageListFontSettingLabel.Text = string.Format(Properties.Resources.SettingMessageListFontInfo, ReadSetting.Setting.MessageListFont.Name, ReadSetting.Setting.MessageListFont.Size.ToString());

					messageListPreviewBox.Refresh();
					messageListPreviewBox.Invalidate();
				}
			}
		}


		private void messageListSenderFontSizeBox_ValueChanged(object sender, EventArgs e)
		{
			messageListPreviewBox.Refresh();
		}

		#endregion

		#region 起動設定

		private void startupRegistButton_Click(object sender, EventArgs e)
        {
            // スタートアップに設定
            if (System.IO.File.Exists(SHORTCUT_PATH))
            {
                MessageBox.Show(Properties.Resources.SettingStartupAlreadyRegisted, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
				// GmailChecker のショートカットがあった場合
				if (System.IO.File.Exists(Path.Combine(Path.GetDirectoryName(SHORTCUT_PATH), "GmailChecker.lnk")))
				{
					switch (MessageBox.Show(Properties.Resources.SettingStartupGmailChecker,
						Properties.Resources.SoftwareName,
						MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1))
					{
						case DialogResult.Yes:
							System.IO.File.Delete(Path.Combine(Path.GetDirectoryName(SHORTCUT_PATH), "GmailChecker.lnk"));
							RegistStartup();
							break;
						case DialogResult.No:
							RegistStartup();
							break;
						case DialogResult.Cancel:
							break;
					}
				}
                else if (MessageBox.Show(Properties.Resources.SettingStartupRegistConfirm, Properties.Resources.SoftwareName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
					RegistStartup();
				}
            }
        }

		private void RegistStartup()
		{
			try
			{
				IWshShell iwshell = new WshShellClass();
				IWshShortcut iwshort = (IWshShortcut)iwshell.CreateShortcut(SHORTCUT_PATH);
				iwshort.TargetPath = Assembly.GetEntryAssembly().Location;
				iwshort.Description = Properties.Resources.SoftwareName;

				// ショートカット作成
				iwshort.Save();
				MessageBox.Show(Properties.Resources.SettingStartupRegested, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Information);

				shortcutButtonSetting();
			}
			catch (Exception ex)
			{
				MessageBox.Show(Properties.Resources.SettingStartupRegistError + ex.Message, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void startupRestoreButton_Click(object sender, EventArgs e)
        {
            // スタートアップから解除
            if (System.IO.File.Exists(SHORTCUT_PATH))
            {
                if (MessageBox.Show(Properties.Resources.SettingStartupRestoreConfirm, Properties.Resources.SoftwareName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    == DialogResult.Yes)
                {
                    System.IO.File.Delete(SHORTCUT_PATH);
                    MessageBox.Show(Properties.Resources.SettingStartupRestored, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    shortcutButtonSetting();
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.SettingStartupRestoreError, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void shortcutButtonSetting()
        {
            if (System.IO.File.Exists(SHORTCUT_PATH))
            {
                startupRegistButton.Enabled = false;
                startupRestoreButton.Enabled = true;
            }
            else
            {
                startupRegistButton.Enabled = true;
                startupRestoreButton.Enabled = false;
            }
        }

        #endregion


        #region リストボックスのオーナードロー
        private void accountListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
			// アカウント設定がゼロなら抜ける
			if (accountListBox.Items.Count == 0)
				return;

            Brush br = null;
            Brush b2 = null;

            // 色の準備
			// 使わないアカウントがある場合
			if (((e.State & DrawItemState.Selected) == DrawItemState.Selected) || s_config.Accounts[e.Index].Use)
			{
				br = new SolidBrush(e.ForeColor);	// 文字色
				b2 = new SolidBrush(e.BackColor);	// 背景色
			}
			else
			{
				br = new SolidBrush(Color.Gray);
				b2 = new SolidBrush(Color.Gainsboro);
			}


            // 文字列の取得
            string text = ((ListBox)sender).Items[e.Index].ToString();
            // 重複するアカウントを入れておくリスト
            List<Account> findAcc = new List<Account>();
            
            // 一致するかを調べるための手続き
            Predicate<Account> preFindAcc = new Predicate<Account>(delegate(Account b)
                {
                    // 調べ元のアカウント設定のドメインが、調べ先のアカウント設定のドメインと一致したら
					if (ReadSetting.Setting.Accounts[e.Index].Domain == b.Domain)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            // 上の手続きを利用して、リスト中に一致するアカウント設定を割り出しているところ
			findAcc = ReadSetting.Setting.Accounts.FindAll(preFindAcc);

            // 割り出した数が 1以下 = 自分以外一致しなかった場合
			// アカウントが使用する状態になっていない場合
			if (!s_config.Accounts[e.Index].Use)
			{
				// 通常の描画
				e.Graphics.FillRectangle(b2, e.Bounds);
				e.Graphics.DrawString(text, e.Font, br, e.Bounds);
				errorToolTip.RemoveAll();
			}
			else
			{
				// アカウントが選択されている状態の場合
				if ((findAcc.Count <= 1 || (e.State & DrawItemState.Selected) == DrawItemState.Selected))
				{
					// 通常の描画
					e.Graphics.FillRectangle(b2, e.Bounds);
					e.Graphics.DrawString(text, e.Font, br, e.Bounds);
					errorToolTip.RemoveAll();
				}
				else
				{
					// 2つ以上あれば
					// とりあえず自分をエラー描画しておく
					e.Graphics.FillRectangle(new SolidBrush(Color.AntiqueWhite), e.Bounds);
					e.Graphics.DrawString(text, e.Font, new SolidBrush(Color.Red), e.Bounds);
					errorToolTip.SetToolTip(accountListBox, "ドメインが重複するアカウントがあります。");
				}
			}

            // 描画処理終了
            br.Dispose();
            b2.Dispose();

        }

#endregion

		#region メッセージ一覧プレビューのオーナードロー

		private void messageListPreviewBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			// 高さ調整
			ListBox lb = (ListBox)sender;
			float height = 0;
			// 件名の高さ
			height += e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Bold)).Height;
			// 日付、内容プレビューの高さ
			height += e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - (float)messageListSenderFontSizeBox.Value, FontStyle.Regular)).Height * 2;
			// 余分
			height += 15;

			lb.ItemHeight = (int)Math.Round((double)height, MidpointRounding.ToEven);

			if (e.Index == -1) return;
			Brush bt, bd, bm, bsel, bunder;
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			int y = 0;
			messageListPreviewBox.BackColor = ReadSetting.Setting.MessageListNonSelectColor;

			if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
			{
				bt = new SolidBrush(ReadSetting.Setting.MessageListTitleColor);
				bd = new SolidBrush(ReadSetting.Setting.MessageListSenderColor);
				bm = new SolidBrush(ReadSetting.Setting.MessageListSummaryColor);
				if (e.Index % 2 == 1)
					bsel = new SolidBrush(ReadSetting.Setting.MessageListNonSelectOddColor);
				else
					bsel = new SolidBrush(ReadSetting.Setting.MessageListNonSelectColor);
			}
			else
			{
				bt = new SolidBrush(e.ForeColor);
				bd = new SolidBrush(e.ForeColor);
				bm = new SolidBrush(e.ForeColor);
				bsel = new SolidBrush(ReadSetting.Setting.MessageListSelectColor);
			}
			bunder = new SolidBrush(Color.FromArgb(160, 160, 160));


			if (ReadSetting.Setting.MessageListAntialias)
			{
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			}
			else
			{
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			}

			e.Graphics.FillRectangle(bsel, e.Bounds);

			// 高さ測定
			int titleheight = (int)e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont, FontStyle.Bold)).Height;
			int colmnheight = (int)e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - (float)messageListSenderFontSizeBox.Value, FontStyle.Regular)).Height;

			y += e.Bounds.Top + 5;
			if (e.Index == 0)
				e.Graphics.DrawString(Properties.Resources.SettingMessageListPreviewTitle, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Bold), bt, new RectangleF(5, y, e.Bounds.Width, titleheight), sf);
			else
				e.Graphics.DrawString(Properties.Resources.SettingMessageListPreviewTitle, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Regular), bt, new RectangleF(5, y, e.Bounds.Width, titleheight), sf);

			y += titleheight + 5;
			e.Graphics.DrawString(Properties.Resources.SettingMessageListPreviewSender, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - (float)messageListSenderFontSizeBox.Value, FontStyle.Regular), bd, new RectangleF(7, y, e.Bounds.Width, colmnheight), sf);

			y += colmnheight + 2;
			e.Graphics.DrawString(Properties.Resources.SettingMessageListPreviewDetail,
				new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - (float)messageListSenderFontSizeBox.Value, FontStyle.Regular), bm, new RectangleF(7, y, e.Bounds.Width, colmnheight), sf);

			// 下線
			e.Graphics.DrawLine(new Pen(bunder), new Point(e.Bounds.Left, e.Bounds.Bottom), new Point(e.Bounds.Right, e.Bounds.Bottom));

			bt.Dispose();
			bd.Dispose();
			bm.Dispose();
			bsel.Dispose();
			bunder.Dispose();
		}

		private void messageListPreviewBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{

		}

		private void frmSetting_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.ControlBox)
			{
				e.Cancel = true;
			}
			else
			{
				e.Cancel = false;
			}
		}
        #endregion
	}
}
