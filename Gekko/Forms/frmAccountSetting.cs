using System;
using System.Drawing;
using System.Windows.Forms;
using Gekko.Libraries;

namespace Gekko.Forms
{
	public partial class frmAccountSetting : Form
	{
		Gekko.Libraries.Account a_account;
		delegate Exception mailTestDelegate(string account, string password, string domain);
		mailTestDelegate mtd;


		/// <summary>
		/// アカウント設定を表示させる
		/// </summary>
		/// <param name="account">アカウント設定クラス</param>
		public frmAccountSetting(Gekko.Libraries.Account account)
		{
			InitializeComponent();

			if (account == null)
			{
				this.a_account = new Gekko.Libraries.Account();
			}
			else
			{
				this.a_account = account;
			}

			// アカウント設定の反映
			this.accountSettingNameBox.Text = a_account.SetName;
			this.accountNameBox.Text = a_account.AccountName;
			this.accountDomainBox.Text = a_account.Domain;
			this.accountPasswordBox.Text = Password.Decrypt(a_account.Password, a_account.AccountName);
			// エラーを最初から表示するためにTextChengedを強制実行
			//accountNameBox_TextChanged(null, null);
			//accountDomainBox_SelectedIndexChanged(null, null);
			//accountPasswordBox_TextChanged(null, null);
		}

		private void accountNameBox_TextChanged(object sender, EventArgs e)
		{
			// メールアドレスの復元と無記入チェック

			// 無記入ならエラー表示
			if (string.IsNullOrEmpty(accountNameBox.Text))
			{
				accountNameBox.BackColor = Color.AntiqueWhite;
				formNullErrorPrivider.SetError(accountNameBox, Properties.Resources.AccountSettingVoidAccount);
			}
			else
			{
				accountNameBox.BackColor = SystemColors.Window;
				formNullErrorPrivider.SetError(accountNameBox, string.Empty);
			}

			accountPreviewLabel.Text = this.accountNameBox.Text + "@" + this.accountDomainBox.Text;
		}

		private void accountDomainBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// メールアドレスの復元と無記入チェック
			// 無記入ならエラー表示
			if (string.IsNullOrEmpty(accountDomainBox.Text))
			{
				accountDomainBox.BackColor = Color.AntiqueWhite;
				formNullErrorPrivider.SetError(accountDomainBox, Properties.Resources.AccountSettingVoidDomain);
			}
			else
			{
				accountDomainBox.BackColor = SystemColors.Window;
				formNullErrorPrivider.SetError(accountDomainBox, string.Empty);
			}

			accountPreviewLabel.Text = this.accountNameBox.Text + "@" + this.accountDomainBox.Text;
		}

		private void accountPasswordBox_TextChanged(object sender, EventArgs e)
		{
			// パスワードボックスの無記入チェック
			if (string.IsNullOrEmpty(accountPasswordBox.Text))
			{
				accountPasswordBox.BackColor = Color.AntiqueWhite;
				formNullErrorPrivider.SetError(accountPasswordBox, Properties.Resources.AcoountSettingVoidPassword);
			}
			else
			{
				accountPasswordBox.BackColor = SystemColors.Window;
				formNullErrorPrivider.SetError(accountPasswordBox, string.Empty);
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			// エラーみつけたら何もしない
			if (string.IsNullOrEmpty(accountNameBox.Text) ||
				string.IsNullOrEmpty(accountDomainBox.Text) ||
				string.IsNullOrEmpty(accountPasswordBox.Text))
			{
				accountNameBox_TextChanged(null, null);
				accountDomainBox_SelectedIndexChanged(null, null);
				accountPasswordBox_TextChanged(null, null);
				return;
			}

			// アカウント設定名がないなら（なし）をつける
			if (string.IsNullOrEmpty(accountSettingNameBox.Text))
			{
				a_account.SetName = Properties.Resources.AccountSettingAltAName;
			}
			else
			{
				a_account.SetName = accountSettingNameBox.Text;
			}
			a_account.AccountName = accountNameBox.Text;
			a_account.Domain = accountDomainBox.Text;
			a_account.Password = Password.Encrypt(accountPasswordBox.Text, accountNameBox.Text);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void accountConnectTestButton_Click(object sender, EventArgs e)
		{
			// 接続テスト
			mtd = new mailTestDelegate(mailTest);
			IAsyncResult ar = mtd.BeginInvoke(accountNameBox.Text, Password.Encrypt(accountPasswordBox.Text, accountNameBox.Text), accountDomainBox.Text,
				new AsyncCallback(accountConnectTestInvokeMethod), null);

			// 接続中の旨を伝える
			accountConnectTestLabel.Text = Properties.Resources.AccountSettingTestConnecting;
			accountConnectTestButton.Enabled = false;
			okButton.Enabled = false;
			cancelButton.Enabled = false;
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
					accountConnectTestLabel.Text = Properties.Resources.AccountSettingTestSucceeded;
				});
			}
			else
			{
				this.Invoke((MethodInvoker)delegate()
				{
					accountConnectTestLabel.Text = "Error: " + ex.Message;
				});
			}

			this.Invoke((MethodInvoker)delegate()
			{
				accountConnectTestButton.Enabled = true;
				okButton.Enabled = true;
				cancelButton.Enabled = true;
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

		/// <summary>
		/// 設定した状態を取得
		/// </summary>
		public Account Result
		{
			get { return a_account; }
		}
	}
}
