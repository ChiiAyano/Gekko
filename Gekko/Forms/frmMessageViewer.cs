using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;
using Gekko.Forms.Components;
using System.IO;

namespace Gekko.Forms
{
	public partial class frmMessageViewer : Form
	{
		List<MailFeed> mailFeed = new List<MailFeed>();
		bool doubleCountCheck = false;
		// スパム表示しているアカウントの表示
		bool[] spamCheckFlags;
		// 通常受信のメールを保持
		MailFeed[] inboxMail;
		// 項目をクリックした時点でのアカウント番号
		int accountNo;

		// このウィンドウのインスタンス保持
		static frmMessageViewer me = new frmMessageViewer();

		delegate ReceiveState receiveSpamDelegate(ListBox lb, MailCheck mc, string uri, int tabIndex);
		receiveSpamDelegate rsd;

		private frmMessageViewer()
		{
			InitializeComponent();

			this.Size = ReadSetting.Setting.MessageBoxPositionSize.Size;
			this.Location = ReadSetting.Setting.MessageBoxPositionSize.Location;
			this.invisibleViewerButton.Checked = ReadSetting.Setting.MessageListInvisibleViewer;
			this.splitContainer1.SplitterDistance = ReadSetting.Setting.MessageListLeftWidth;
			this.Opacity = ReadSetting.Setting.MessageListWindowOpacity;
			this.windowOpacityMenues(ReadSetting.Setting.MessageListWindowOpacity);

			messageViewerBox.StatusTextChanged += new EventHandler(messageViewerBox_StatusTextChanged);
		}

		/// <summary>
		/// このクラスのインスタンスを返します
		/// </summary>
		public static frmMessageViewer Instance
		{
			get
			{
				if ((me == null) || (me.IsDisposed))
					me = new frmMessageViewer();
				return me;
			}
		}

		private void frmMessageViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.WriteLog("frmMessageViewer_e.CloseReason == " + e.CloseReason.ToString());
			Program.WriteLog("Program.SystemShutown == " + Program.SystemShutdown.ToString());

			if ((Program.SystemShutdown) ||
				((e.CloseReason | CloseReason.FormOwnerClosing) == CloseReason.FormOwnerClosing) ||
				((e.CloseReason | CloseReason.WindowsShutDown) == CloseReason.WindowsShutDown))
			{

				Program.WriteLog("メッセージ一覧閉じるよ！");

				e.Cancel = false;
			}
			else if (!(Program.SystemShutdown) ||
				((e.CloseReason | CloseReason.UserClosing) == CloseReason.UserClosing) ||
				((e.CloseReason | CloseReason.None) == CloseReason.None))
			{

				Program.WriteLog("メッセージ一覧閉じないよ！");

				// ブラウザオブジェクトを空にしとく
				this.messageViewerBox.Navigate("about:blank");

				// 設定保存
				ConfigData cd = ReadSetting.Setting;
				cd.MessageListTopMost = topMostButton.Checked;
				cd.MessageBoxPositionSize = new Rectangle(this.Location, this.Size);
				cd.MessageListLeftWidth = splitContainer1.SplitterDistance;
				cd.MessageListWindowOpacity = this.Opacity;
				ReadSetting.Setting = cd;
				ReadSetting.Save();

				checkSpamButton.Checked = false;

				e.Cancel = true;
				this.Hide();

				// ガベージコレクション強制実行
				GC.Collect();
#if DEBUG
				Console.WriteLine("メッセージ一覧クローズ時: " + GC.GetTotalMemory(false));
#endif
			}
		}

		/// <summary>
		/// メッセージビューアの準備
		/// </summary>
		/// <param name="mf">メールフィードのリスト</param>
		public void SetSettings(List<MailFeed> mf)
		{
			this.mailFeed = mf;
		}

		/// <summary>
		/// メッセージビューアの準備
		/// </summary>
		public void SetSettings()
		{
			this.mailFeed = null;
		}

		private void frmMessageViewer_Load(object sender, EventArgs e)
		{
			SetList();
			this.TopMost = ReadSetting.Setting.MessageListTopMost;
			this.ShowInTaskbar = !ReadSetting.Setting.MessageListTopMost;
			topMostButton.Image = ReadSetting.Setting.MessageListTopMost ? icons.lightbulb : icons.lightbulb_off;
			topMostButton.Checked = ReadSetting.Setting.MessageListTopMost;

			this.Size = ReadSetting.Setting.MessageBoxPositionSize.Size;
			this.Location = ReadSetting.Setting.MessageBoxPositionSize.Location;
		}


		private void frmMessageViewer_VisibleChanged(object sender, EventArgs e)
		{
			// リスト等の再編
			if (this.Visible == true)
			{
				SetList();
			}
		}


		private void SetList()
		{
#if DEBUG
			Console.WriteLine("メッセージ一覧準備前: " + GC.GetTotalMemory(false));
#endif

			// タブページを全部消しておく
			if (messageListTab.TabPages.Count > 0)
			{
				messageListTab.TabPages.Clear();
			}

			// タブの準備
			foreach (Account a in ReadSetting.Setting.Accounts)
			{
				// 使うアカウントだけ表示する
				if (a.Use)
				{
					if (mailFeed != null)
					{
						if (mailFeed[ReadSetting.Setting.Accounts.IndexOf(a)].FullCount > 0)
						{
							// 未読あるアカウントだけ表示
							TabPage tp = new TabPage(a.SetName);
							ListBox lb = new ListBox();

							lb.DrawMode = DrawMode.OwnerDrawVariable;
							lb.BackColor = ReadSetting.Setting.MessageListNonSelectColor;
							lb.BorderStyle = BorderStyle.None;
							lb.DrawItem += new DrawItemEventHandler(lb_DrawItem);
							lb.MeasureItem += new MeasureItemEventHandler(lb_MeasureItem);
							lb.MouseDown += new MouseEventHandler(lb_MouseDown);
							lb.MouseMove += new MouseEventHandler(lb_MouseMove);
							lb.Click += new EventHandler(lb_Click);
							lb.Dock = DockStyle.Fill;

							foreach (MailEntry me in mailFeed[ReadSetting.Setting.Accounts.IndexOf(a)].Entry)
							{
								lb.Items.Add(me);
							}

							//　追加
							tp.Controls.Add(lb);
							messageListTab.TabPages.Add(tp);
						}
					}
					else
					{
						// 全アカウント追加
						TabPage tp = new TabPage(a.SetName);
						ListBox lb = new ListBox();

						lb.DrawMode = DrawMode.OwnerDrawVariable;
						lb.BackColor = ReadSetting.Setting.MessageListNonSelectColor;
						lb.BorderStyle = BorderStyle.None;
						lb.DrawItem += new DrawItemEventHandler(lb_DrawItem);
						lb.MeasureItem += new MeasureItemEventHandler(lb_MeasureItem);
						lb.MouseDown += new MouseEventHandler(lb_MouseDown);
						lb.MouseMove += new MouseEventHandler(lb_MouseMove);
						lb.Click += new EventHandler(lb_Click);
						lb.Dock = DockStyle.Fill;

						// 空のまま追加
						tp.Controls.Add(lb);
						messageListTab.TabPages.Add(tp);
					}
				}

				// タブの数だけspamCheckFlagsとinboxMailの配列数を決める
				spamCheckFlags = new bool[messageListTab.TabCount];
				inboxMail = new MailFeed[messageListTab.TabCount];

				// リストとビューアの配置を決める
				splitContainer1.Panel1.Controls.Clear();
				splitContainer1.Panel2.Controls.Clear();
				if (ReadSetting.Setting.MessageListRightList)
				{
					splitContainer1.Panel1.Controls.Add(messageViewerBox);
					splitContainer1.Panel1.Controls.Add(dummyBrowser);
					splitContainer1.Panel2.Controls.Add(messageListTab);
				}
				else
				{
					splitContainer1.Panel2.Controls.Add(messageViewerBox);
					splitContainer1.Panel2.Controls.Add(dummyBrowser);
					splitContainer1.Panel1.Controls.Add(messageListTab);
				}

				invisibleViewer(!ReadSetting.Setting.MessageListInvisibleViewer);
			}
#if DEBUG
			Console.WriteLine("メッセージ一覧準備完了時: " + GC.GetTotalMemory(false));
#endif
		}

		private void lb_Click(object sender, EventArgs e)
		{
			// 呼び出し元のリストボックスを取得
			ListBox lb = ((ListBox)sender);

			if (lb.SelectedIndex > -1)
			{
				MailEntry me = ((MailEntry)lb.Items[lb.SelectedIndex]);
				me.IsOpened = true;

				lb.Refresh();

				// ブラウザにメール表示
				Account ac = ReadSetting.Setting.Accounts.Find(delegate(Account find)
				{
					return (find.SetName == messageListTab.SelectedTab.Text);
				});
				accountNo = ReadSetting.Setting.Accounts.IndexOf(ac);
				if (spamCheckFlags[messageListTab.SelectedIndex])
				{
					string url = ReadSetting.Setting.NoUseSsl ? "http://" : "https://" + ac.GetInboxUri;
					url = url.TrimEnd('/');
					if (ac.Domain == "gmail.com")
						url = url.Insert(url.Length, "/mail/#spam");
					else
						url = url.Insert(url.Length, "/#spam");

					// ビューアが非表示ならブラウザで表示する
					if (ReadSetting.Setting.MessageListInvisibleViewer)
						BrowserShow.browserOpen(url);
					else
						messageViewerBox.Navigate(url);
					
				}
				else
				{
					// ビューアが非表示ならブラウザで表示する
					if (ReadSetting.Setting.MessageListInvisibleViewer)
						BrowserShow.browserOpen(me.Link.Uri);
					else
						messageViewerBox.Navigate(me.Link.Uri);
				}
			}
		}

		private void lb_MouseDown(object sender, MouseEventArgs e)
		{
			ListBox lb = (ListBox)sender;
			if (lb.IndexFromPoint(e.Location) == -1)
			{
				lb.ClearSelected();
			}
		}

		MailEntry lastMailEntry = null;
		private void lb_MouseMove(object sender, MouseEventArgs e)
		{
			// 一覧をポイントして日付をポップアップさせる
			ListBox lb = (ListBox)sender;

			if (lb.Items.Count > 0)
			{
				if (lb.IndexFromPoint(e.Location) > -1)
				{
					MailEntry me = (MailEntry)lb.Items[lb.IndexFromPoint(e.Location)];
					if (me != lastMailEntry)
					{
						detailToolTip.RemoveAll();
						detailToolTip.Active = false;

						if (me != null)
						{
							detailToolTip.SetToolTip(lb, string.Format("{0}{1}", Properties.Resources.MessageListToolTip, me.Modified.ToString(Properties.Resources.MessageListToolTipTime)));
							detailToolTip.ShowAlways = true;
							detailToolTip.Show(string.Format("{0}{1}", Properties.Resources.MessageListToolTip, me.Modified.ToString(Properties.Resources.MessageListToolTipTime)), lb);
							detailToolTip.Active = true;
						}

						lastMailEntry = me;
					}
				}
			}
		}


		#region メッセージリストのオーナードロー
		private void lb_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			float height = 0;
			// 件名の高さ
			height += e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Bold)).Height;
			// 日付、内容プレビューの高さ
			height += e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - ReadSetting.Setting.MessageListSenderFontSize, FontStyle.Regular)).Height * 2;
			// 余分
			height += 15;

			e.ItemHeight = (int)Math.Round((double)height, MidpointRounding.ToEven);
		}

		private void lb_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1) return;

			MailEntry mes = ((MailEntry)((ListBox)sender).Items[e.Index]);
			Brush bt, bd, bm, bsel, bunder;
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			int y = 0;

			
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
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			else
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

			e.Graphics.FillRectangle(bsel, e.Bounds);

			// 高さ測定
			int titleheight = (int)e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont, FontStyle.Bold)).Height;
			int colmnheight = (int)e.Graphics.MeasureString("ABCDEFGabcdefg", new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - ReadSetting.Setting.MessageListSenderFontSize, FontStyle.Regular)).Height;

			y += e.Bounds.Top + 5;
			// メールを開封してある（項目を選択した）場合はタイトルの太字解除
			if (mes.IsOpened)
			{
				e.Graphics.DrawString(mes.Title, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Regular), bt, new RectangleF(5, y, e.Bounds.Width, titleheight), sf);
			}
			else
			{
				e.Graphics.DrawString(mes.Title, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size, FontStyle.Bold), bt, new RectangleF(5, y, e.Bounds.Width, titleheight), sf);

			}
			y += titleheight + 5;
			e.Graphics.DrawString(Properties.Resources.MessageListMailAuthor + mes.Author.Name, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - ReadSetting.Setting.MessageListSenderFontSize, FontStyle.Regular), bd, new RectangleF(7, y, e.Bounds.Width, colmnheight), sf);
			y += colmnheight + 2;
			e.Graphics.DrawString(mes.Summary, new Font(ReadSetting.Setting.MessageListFont.FontFamily, ReadSetting.Setting.MessageListFont.Size - ReadSetting.Setting.MessageListSenderFontSize, FontStyle.Regular), bm, new RectangleF(7, y, e.Bounds.Width, colmnheight), sf);

			// 下線
			e.Graphics.DrawLine(new Pen(bunder), new Point(e.Bounds.Left, e.Bounds.Bottom - 1), new Point(e.Bounds.Right, e.Bounds.Bottom - 1));

			bt.Dispose();
			bd.Dispose();
			bm.Dispose();
			bsel.Dispose();
			bunder.Dispose();

			e.DrawFocusRectangle();
		}
		#endregion

		private void frmMessageViewer_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void frmMessageViewer_Move(object sender, EventArgs e)
		{
			//if (this.Top < 0)
			//{
			//    this.Top = 0;
			//}
			//if (this.Left < 0)
			//{
			//    this.Left = 0;
			//}
			//if (this.Bottom > Screen.GetWorkingArea(this).Bottom)
			//{
			//    this.Top = Screen.GetWorkingArea(this).Bottom - this.Height;
			//}
			//if (this.Right > Screen.GetWorkingArea(this).Right)
			//{
			//    this.Left = Screen.GetWorkingArea(this).Right - this.Width;
			//}

		}
		private void messageViewerBox_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if ((string)e.Url.ToString() == "about:blank")
				browserViewButton.Enabled = false;
			else
				browserViewButton.Enabled = true;
		}

		private void messageViewerBox_NewWindow2(object sender, WebBrowserNewWindow2EventArgs e)
		{
			// ブラウザオープンを抑制していたら解除
			if (doubleCountCheck)
				doubleCountCheck = false;

			this.dummyBrowser.RegisterAsBrowser = true;
			e.ppDisp = this.dummyBrowser.Application;
		}

		private void messageViewerBox_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
		{
			progressStatusProgress.Maximum = (int)e.MaximumProgress;
			progressStatusProgress.Value = (int)e.CurrentProgress;
		}


		private void messageViewerBox_StatusTextChanged(object sender, EventArgs e)
		{
			if (messageViewerBox.Url.ToString() == "about:blank")
				browserViewButton.Enabled = false;
			else
				browserViewButton.Enabled = true;

			progressStatusLabel.Text = messageViewerBox.StatusText;
#if DEBUG
			Console.WriteLine("メッセージビューアのステータス変更時: " + GC.GetTotalMemory(false));
#endif
		}


		private void dummyBrowser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			if (!doubleCountCheck)
			{
				BrowserShow.browserOpen((string)e.uRL);
				// ここでブラウザオープン抑制
				doubleCountCheck = true;
			}
			else
			{
				doubleCountCheck = false;
			}
		}

		private void dummyBrowser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			e.cancel = true;

			if (!doubleCountCheck)
			{
				BrowserShow.browserOpen((string)e.uRL);
				// ここでブラウザオープン抑制
				doubleCountCheck = true;
			}
			else
			{
				doubleCountCheck = false;
			}

		}

		private void topMostButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void topMostButton_Click(object sender, EventArgs e)
		{
			// 最前面表示切り替え
			topMostButton.Checked = !topMostButton.Checked;
			this.TopMost = topMostButton.Checked;
			this.ShowInTaskbar = !topMostButton.Checked;
			topMostButton.Image = topMostButton.Checked ? icons.lightbulb : icons.lightbulb_off;
		}

		private void browserViewButton_Click(object sender, EventArgs e)
		{
			// ブラウザで表示
			BrowserShow.browserOpen(messageViewerBox.Url.ToString());
		}

		private void checkSpamButton_Click(object sender, EventArgs e)
		{
			ListBox lb = (ListBox)messageListTab.SelectedTab.Controls[0];

			// 今受信トレイの情報を開いていれば
			if (!checkSpamButton.Checked)
			{
				if (inboxMail[messageListTab.SelectedIndex] != null)
					inboxMail[messageListTab.SelectedIndex].Entry.Clear();
				else
					inboxMail[messageListTab.SelectedIndex] = new MailFeed();

				foreach (MailEntry m in lb.Items)
				{
					inboxMail[messageListTab.SelectedIndex].Entry.Add(m);
				}
			}

			// 初期化
			lb.Items.Clear();

			Account ac = ReadSetting.Setting.Accounts.Find(delegate(Account find)
			{
				return (find.SetName == messageListTab.SelectedTab.Text);
			});

			MailCheck mc = new MailCheck(ac.AccountName, ac.Password, ac.Domain);

			// スパムチェック開始
			if (!checkSpamButton.Checked)
			{
				progressStatusLabel.Text = Properties.Resources.MessageListSpamChecking;
				checkSpamButton.Enabled = false;

				rsd = new receiveSpamDelegate(reveiveSpamMail);
				IAsyncResult ar = rsd.BeginInvoke(lb, mc, mc.GetAtomUri(ac.Domain) + "/spam", 
					messageListTab.SelectedIndex, new AsyncCallback(receiveSpamMailInvokeMethod), null);
			}
			else
			{
				// 受信トレイの情報復帰
				foreach (MailEntry me in inboxMail[messageListTab.SelectedIndex].Entry)
				{
					lb.Items.Add(me);
				}
				lb.Refresh();

				spamCheckFlags[messageListTab.SelectedIndex] = false;
				checkSpamButton.Checked = false;
				progressStatusLabel.Text = Properties.Resources.MessageListInboxRestore;

#if DEBUG
				Console.WriteLine("受信トレイ情報復帰時: " + GC.GetTotalMemory(false));
#endif
			}
		}

		private void invisibleViewerButton_Click(object sender, EventArgs e)
		{
			invisibleViewerButton.Checked = invisibleViewerButton.Checked ? false : true;
		}

		private void invisibleViewerButton_CheckedChanged(object sender, EventArgs e)
		{
			if (invisibleViewerButton.Checked)
			{
				// 非表示にする
				if (ReadSetting.Setting.MessageListRightList)
					splitContainer1.Panel1Collapsed = true;
				else
					splitContainer1.Panel2Collapsed = true;
				invisibleViewerButton.Image = icons.application_side_contract;
				browserViewButton.Visible = false;
			}
			else
			{
				// 表示する
				if (ReadSetting.Setting.MessageListRightList)
					splitContainer1.Panel1Collapsed = false;
				else
					splitContainer1.Panel2Collapsed = false; 
				invisibleViewerButton.Image = icons.application_side_expand;
				browserViewButton.Visible = true;
			}

			ReadSetting.Setting.MessageListInvisibleViewer = invisibleViewerButton.Checked;
			ReadSetting.Save();
		}

		private void invisibleViewer(bool invisible)
		{
			if (invisible)
			{
				// 表示する
				if (ReadSetting.Setting.MessageListRightList)
					splitContainer1.Panel1Collapsed = false;
				else
					splitContainer1.Panel2Collapsed = false;
				invisibleViewerButton.Image = icons.application_side_expand;
				browserViewButton.Visible = true;
			}
			else
			{

				// 非表示にする
				if (ReadSetting.Setting.MessageListRightList)
					splitContainer1.Panel1Collapsed = true;
				else
					splitContainer1.Panel2Collapsed = true;
				invisibleViewerButton.Image = icons.application_side_contract;
				browserViewButton.Visible = false;
			}
		}


		private void receiveSpamMailInvokeMethod(IAsyncResult ar)
		{
			// 応答を待つ
			ReceiveState rs = rsd.EndInvoke(ar);

			// 例外が入っていなければ
			if (rs.ex == null)
			{
				this.Invoke((MethodInvoker)delegate
				{
					// ブラウザ部のステータスを書き換え
					if (rs.fullcount > rs.feedcount)
						progressStatusLabel.Text = string.Format(Properties.Resources.MessageListSpamReceiveOver, rs.fullcount, rs.feedcount);
					else
						progressStatusLabel.Text = string.Format(Properties.Resources.MessageListSpamReceive, rs.fullcount);

					// スパム表示ボタンの状態変化
					checkSpamButton.Enabled = true;
					spamCheckFlags[rs.tabIndex] = true;
					if ((messageListTab.SelectedIndex == rs.tabIndex) && (spamCheckFlags[rs.tabIndex]))
						checkSpamButton.Checked = true;

#if DEBUG
					Console.WriteLine("迷惑メール受信処理完了時: " + GC.GetTotalMemory(false));
#endif
				});
			}
			else
			{
				this.Invoke((MethodInvoker)delegate
				{
					progressStatusLabel.Text = Properties.Resources.MessageListSpamError + rs.ex.Message;
					checkSpamButton.Enabled = true;
				});
			}
		}


		private ReceiveState reveiveSpamMail(ListBox lb, MailCheck mc, string uri, int tabIndex)
		{
			// スパムメールのチェック、受信トレイのメール再チェック
			try
			{
				MailFeed mf = mc.Check(uri);

				this.Invoke((MethodInvoker)delegate
				{
					// リストボックスに反映
					foreach (MailEntry me in mf.Entry)
					{
						lb.Items.Add(me);
					}
					lb.Refresh();
				});

				ReceiveState rs = new ReceiveState();
				rs.ex = null;
				rs.fullcount = mf.FullCount;
				rs.feedcount = mf.Entry.Count;
				rs.tabIndex = tabIndex;

				return rs;
			}
			catch (Exception ex)
			{
				ReceiveState rs = new ReceiveState();
				rs.ex = ex;
				rs.fullcount = 0;
				rs.feedcount = 0;
				rs.tabIndex = tabIndex;

				return rs;
			}
		}

		// スパム取得用構造体
		private struct ReceiveState
		{
			public int fullcount;
			public int feedcount;
			public Exception ex;
			public int tabIndex;
		}

		private void messageListTab_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (messageListTab.SelectedIndex > -1)
			{
				checkSpamButton.Checked = spamCheckFlags[messageListTab.SelectedIndex];
			}
		}

		private void messageViewerBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

			if (e.Url.GetLeftPart(UriPartial.Path).Contains("https://www.google.com/accounts/ServiceLogin"))
			{
				messageViewerBox.Document.All.GetElementsByName("Email")[0].InnerText = ReadSetting.Setting.Accounts[accountNo].Address;
				messageViewerBox.Document.All.GetElementsByName("Passwd")[0].InnerText = Password.Decrypt(ReadSetting.Setting.Accounts[accountNo].Password, ReadSetting.Setting.Accounts[accountNo].AccountName);
				messageViewerBox.Document.Forms[0].InvokeMember("submit");
			}
			else if (e.Url.GetLeftPart(UriPartial.Path).Contains(string.Format("https://{0}/ServiceLogin",
				ReadSetting.Setting.Accounts[accountNo].GetInboxUri.Replace
				("mail", "www"))))
			{
				messageViewerBox.Document.All.GetElementsByName("Email")[0].InnerText = ReadSetting.Setting.Accounts[accountNo].AccountName;
				messageViewerBox.Document.All.GetElementsByName("Passwd")[0].InnerText = Password.Decrypt(ReadSetting.Setting.Accounts[accountNo].Password, ReadSetting.Setting.Accounts[accountNo].AccountName);
				messageViewerBox.Document.Forms[0].InvokeMember("submit");
			}
			else if (e.Url.GetLeftPart(UriPartial.Path).Contains("https://member.livedoor.com/login"))
			{
				messageViewerBox.Document.All.GetElementsByName("livedoor_id")[0].InnerText = ReadSetting.Setting.Accounts[accountNo].AccountName;
				messageViewerBox.Document.All.GetElementsByName("password")[0].InnerText = Password.Decrypt(ReadSetting.Setting.Accounts[accountNo].Password, ReadSetting.Setting.Accounts[accountNo].AccountName);
				messageViewerBox.Document.Forms[0].InvokeMember("submit");
			}
			else if (e.Url.GetLeftPart(UriPartial.Path).Contains("https://integration.auone.jp/login/"))
			{
				messageViewerBox.Document.All.GetElementsByName("auoneid")[0].InnerText = ReadSetting.Setting.Accounts[accountNo].AccountName;
				messageViewerBox.Document.All.GetElementsByName("password")[0].InnerText = Password.Decrypt(ReadSetting.Setting.Accounts[accountNo].Password, ReadSetting.Setting.Accounts[accountNo].AccountName);
				messageViewerBox.Document.All.GetElementsByName("password")[0].Focus();
				SendKeys.SendWait("{ENTER}");
			}
		}

		private void windowOpacity_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;

			switch (tsmi.Text)
			{
				case "100%":
					this.Opacity = 1f;
					windowOpacity100.Checked = true;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = false;
					break;
				case "80%":
					this.Opacity = 0.8f;
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = true;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = false;
					break;
				case "50%":
					this.Opacity = 0.5f;
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = true;
					windowOpacity20.Checked = false;
					break;
				case "20%":
					this.Opacity = 0.2f;
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = true;
					break;
			}
		}

		private void windowOpacityMenues(double opacity)
		{
			switch ((int)(opacity * 10))
			{
				case 8:
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = true;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = false;
					break;
				case 5:
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = true;
					windowOpacity20.Checked = false;
					break;
				case 2:
					windowOpacity100.Checked = false;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = true;
					break;
				default:
					this.Opacity = 1;
					windowOpacity100.Checked = true;
					windowOpacity80.Checked = false;
					windowOpacity50.Checked = false;
					windowOpacity20.Checked = false;
					break;
			}
		}

	}
}
