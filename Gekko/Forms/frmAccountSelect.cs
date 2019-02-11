using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;

namespace Gekko.Forms
{
	public partial class frmAccountSelect : Form
	{
		List<Account> ac = new List<Account>();

		public frmAccountSelect(List<Account> accounts)
		{
			InitializeComponent();

			this.ac = accounts;
			this.DialogResult = DialogResult.Cancel; // デフォルトでキャンセル
		}

		private void frmAccountSelect_Load(object sender, EventArgs e)
		{
			foreach (Account a in ac)
			{
				accountSelectBox.Items.Add((string)a.SetName + " (" + a.AccountName + "@" + a.Domain + ")");
			}

			accountSelectBox.SelectedIndex = 0;
		}

		private void openButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// ブラウザで受信トレイを開くアカウント情報
		/// </summary>
		public Account Account
		{
			get
			{
				return ac[accountSelectBox.SelectedIndex];
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void accountSelectBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListBox lb = (ListBox)sender;

			if ((e.Button & System.Windows.Forms.MouseButtons.Left) != System.Windows.Forms.MouseButtons.Left)
				return;

			// リストをダブルクリックしたらその時選択されていたアカウントを開く
			if (lb.IndexFromPoint(e.Location) > -1)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}
	}
}
