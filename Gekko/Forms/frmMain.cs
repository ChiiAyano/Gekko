using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Gekko.Libraries;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Gekko.Forms
{
	public partial class frmMain : Form
	{
		Assembly asm;
		AssemblyName asmName;
		// タイマースレッド
		System.Threading.Timer m_checkTimer;
		// 最後に読んだメールの受信日時
		DateTime m_lastMailDate;
		// 接続状態
		bool m_connecting = true;
		// 個別にメールチェックをするための、アカウントのメニュー項目
		List<ToolStripMenuItem> m_mailCheckAccountToolBar = new List<ToolStripMenuItem>();
		List<ToolStripMenuItem> m_mailCheckAccountNotify = new List<ToolStripMenuItem>();
		Gekko.MessageList.Forms.frmMessageViewer m_msgV;



		// 定数
		readonly string path = Path.Combine(Application.StartupPath, "config.xml");

		public const uint EM_LINESCROLL = 0x00B6;

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lparam);

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

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x16) // WM_ENDSESSION
			{
				Program.WriteLog("WM_ENDSESSION: " + m.Msg);
				Program.SystemShutdown = true;
			}
			base.WndProc(ref m);
		}

		#region 初期設定

		public frmMain()
		{
			InitializeComponent();

			asm = Assembly.GetExecutingAssembly();
			asmName = asm.GetName();

			// 一時ファイルの削除
			if (File.Exists(Path.Combine(Path.GetTempPath(), "Gekko.zip")))
				File.Delete(Path.Combine(Path.GetTempPath(), "Gekko.zip"));
			if (File.Exists(Path.Combine(Path.GetTempPath(), "GekkoUpdater.zip")))
				File.Delete(Path.Combine(Path.GetTempPath(), "GekkoUpdater.zip"));
			if (Directory.Exists(Path.Combine(Path.GetTempPath(), "GekkoUpdater")))
				Directory.Delete(Path.Combine(Path.GetTempPath(), "GekkoUpdater"), true);

			// 設定ファイルのパスを指定
			ReadSetting.Path = path;

			// アイコン設定
			this.Icon = icons.gekko;
			// 通知領域の設定
			this.notify.Icon = icons.nomessage;
			this.notify.Text = Properties.Resources.SoftwareName;

			// 例外キャッチャー
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			// 電源状態
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
			SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);

			// 最後に読んだメールの受信日時
			m_lastMailDate = new DateTime();


			// 設定ファイルがあれば読む
			if (System.IO.File.Exists(path))
			{
				ReadSetting.Path = path;
				ReadSetting.Read();

				// メールチェックタイマーの初期化
				m_checkTimer = new System.Threading.Timer(new TimerCallback(mailCheckTimer), null, Timeout.Infinite, ReadSetting.Setting.MailCheckInterval);
				// アカウント設定がまだなければ
				if (ReadSetting.Setting.Accounts.Count == 0)
				{
					MessageBox.Show(Properties.Resources.RunNoAccountDialog, Properties.Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					mainSettingMenu_Click(null, null);
				}
				else
				{
				}
			}
			else
			{
				// メールチェックタイマーの初期化（タイマーは作動しない）
				m_checkTimer = new System.Threading.Timer(new TimerCallback(mailCheckTimer), null, Timeout.Infinite, Timeout.Infinite);
				switch (MessageBox.Show(Properties.Resources.FirstRunDialog,
					Properties.Resources.SoftwareName,
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question))
				{
					case System.Windows.Forms.DialogResult.Yes:
						// なければ初期化
						ReadSetting.Read();
						ReadSetting.Save();
						notifySettingMenu.Enabled = false;

						if (Program.OperatingSystemIsMoreThanWindowsVista)
						{
							if (Application.RenderWithVisualStyles)
							{
								Wizards.frmSetupWizVista wiv = new Gekko.Forms.Wizards.frmSetupWizVista();
								wiv.ShowDialog();
							}
							else
							{
								Wizards.frmSetupWizXp wiz = new Gekko.Forms.Wizards.frmSetupWizXp();
								wiz.ShowDialog();
							}
						}
						else
						{
							Wizards.frmSetupWizXp wiz = new Gekko.Forms.Wizards.frmSetupWizXp();
							wiz.ShowDialog();
						}
						notifySettingMenu.Enabled = true;
						break;

					case System.Windows.Forms.DialogResult.No:
						// なければ初期化
						ReadSetting.Read();
						ReadSetting.Save();
						mainSettingMenu_Click(null, null);
						break;

					case System.Windows.Forms.DialogResult.Cancel:
						Program.SystemShutdown = true;
						Application.Exit();
						break;

					default:
						mainSettingMenu_Click(null, null);
						break;
				}
			}

			// アカウント設定があれば、
			// ネットワーク接続が確立していればタイマーを起動する
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				// 起動する
				m_checkTimer.Change(ReadSetting.Setting.PowerResumedDelayTime, ReadSetting.Setting.MailCheckInterval);
			}

			// メッセージ一覧準備
			//frmMessageViewer.Instance.SetSettings();

			// 通知領域ポップアップの太字部分の変更
			switch (ReadSetting.Setting.NotifyDoubleClickOperation)
			{
				case ConfigData.NotifyDoubleClick.ShowMainWindow:
					notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					break;
				case ConfigData.NotifyDoubleClick.ShowInbox:
					notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					break;
				case ConfigData.NotifyDoubleClick.MailCheck:
					notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					break;
				default:
					break;
			}

			// メインコンソールのフォント変更
			logBox.Font = ReadSetting.Setting.MainConsoleFont;

			// アカウントメニューの更新
			createAccountMailCheckMenu();

			// メッセージ一覧の初期化
			m_msgV = new Gekko.MessageList.Forms.frmMessageViewer();

			// ポップアップで通知
			//frmNewMessage fn = new frmNewMessage("0 通の新着メッセージがあります。\nabcde\ncderfg");
			//fn.Show();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			this.Visible = ReadSetting.Setting.HideConsole ? false : true;

			//ウィンドウが表示されたらやること
			WriteLog(Properties.Resources.SoftwareName + " Version " + Application.ProductVersion, Color.Blue);
		}

		#endregion

		#region ログ出力

		/// <summary>
		/// メインコンソールのログを出力する
		/// </summary>
		/// <param name="message">出力する内容</param>
		/// <param name="cl">文字列の色</param>
		private void WriteLog(string message, Color cl)
		{
			// ログを出力する
			this.Invoke((MethodInvoker)delegate()
			{
				// 日時の出力
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.SelectionColor = Color.FromArgb(220, 0, 73);
				this.logBox.SelectionFont = ReadSetting.Setting.MainConsoleFont;
				this.logBox.SelectedText = DateTime.Now.ToString("MM/dd HH:mm:ss: ");

				// 内容の出力
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.SelectionColor = cl;
				this.logBox.SelectionFont = ReadSetting.Setting.MainConsoleFont;
				this.logBox.SelectedText = message + "\r\n";

				// キャレットを一番下にする
				//this.logBox.Focus();
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.ScrollToCaret();
			});
		}
		#endregion

		#region メールチェック

		private struct mailCheckResult
		{
			/// <summary>
			/// 新しいメールがあったかどうか
			/// </summary>
			public bool NewMessage;
			/// <summary>
			/// 未読メール数
			/// </summary>
			public int MessageCount;
			/// <summary>
			/// 一番新しいメールの受信日
			/// </summary>
			public DateTime RecentMailDate;
			/// <summary>
			/// エラーが発生したか
			/// </summary>
			public bool IsError;
			/// <summary>
			/// 受信状況の内容
			/// </summary>
			public string Detail;
			/// <summary>
			/// メール情報
			/// </summary>
			public MailFeed Feed;
		}

		private mailCheckResult mailCheck(Account account)
		{
			// 接続結果の格納
			mailCheckResult mcr = new mailCheckResult { Detail = string.Empty, Feed = new MailFeed(), IsError = false, MessageCount = 0, NewMessage = false, RecentMailDate = new DateTime() };

			try
			{
				// 各アカウントの設定を元に、メールを読む
				MailCheck mc = new MailCheck(account.AccountName, account.Password, account.Domain);
				MailFeed mf = mc.Check();
				mcr.Feed = mf;

#if DEBUG
				// メモリ使用量
				Console.WriteLine("メールチェック時: " + GC.GetTotalMemory(false));
#endif

				// メールが1件以上あれば
				if (mf.Entry.Count > 0)
				{
					if (mf.Entry[0].Modified > m_lastMailDate)
					{
						// 新しいメールがあれば
						mcr.NewMessage = true;
						// 受信日時を更新
						mcr.RecentMailDate = mf.Entry[0].Modified;
					}
					else
					{
						// 新しいメールがない
					}
					// メール数をカウント
					mcr.MessageCount = mf.FullCount;

					WriteLog(string.Format(Properties.Resources.MailCheckUnreadCount, mf.FullCount), Color.Green);

					// 通知領域等に表示する詳細を追記
					mcr.Detail = mcr.Detail.Insert(mcr.Detail.Length, string.Format(Properties.Resources.MailCheckMailCount + "\n", account.SetName, mf.FullCount));
				}
				else
				{
					WriteLog(Properties.Resources.MailCheckNoUnread, Color.DarkGray);
				}
			}
			catch (Exception wex)
			{
				// エラーでした
				mcr.IsError = true;
				// エラーを報告
				WriteLog(string.Format(Properties.Resources.MailCheckError, wex.Message), Color.Red);
				// 通知領域等に表示する詳細を追記
				mcr.Detail = mcr.Detail.Insert(mcr.Detail.Length, "! " + account.SetName + "\n");
			}

			// 最後に通信結果を返す
			return mcr;
		}

		private void mailCheckTimer(object o)
		{
			// 準備
			List<MailFeed> mfl = new List<MailFeed>();	// メールフィードのリスト
			bool newMessage = false;					// 新しいメールがあったか
			string detail = string.Empty;				// 未読メッセージの詳細
			int totalMessageCount = 0;					// 合計メール数
			int errorCount = 0;							// エラーアカウント数

			//frmMessageViewer.Instance.SetSettings();	// メッセージ一覧の準備

			// 接続状態が確立していれば
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				// アカウント数が0なら抜ける
				if (ReadSetting.Setting.Accounts.Count == 0)
					return;

				// メールチェック中は関連コントロールを無効化
				mailCheckControlEnabled(false);

				foreach (Account a in ReadSetting.Setting.Accounts)
				{
					// アカウントを使用する設定の時にチェック
					if (a.Use)
					{
						// 進行状況をメインコンソールに表示
						WriteLog(string.Format(Properties.Resources.MailChecking, a.SetName), Color.DodgerBlue);
						// 通知領域アイコン変更
						if (!ReadSetting.Setting.DisableNotifyAnimation)
							this.notify.Icon = icons.checkmessage;

						// メールチェック開始
						mailCheckResult checkResult = mailCheck(a);
						// 結果
						mfl.Add(checkResult.Feed);
						if (checkResult.IsError)
							errorCount++; // エラーがあったときは加算
						if (checkResult.RecentMailDate > m_lastMailDate)
						{
							newMessage = true;
							m_lastMailDate = checkResult.RecentMailDate;
						}
						detail = detail.Insert(detail.Length, checkResult.Detail);
						totalMessageCount += checkResult.MessageCount;
					}
					else
					{
						mfl.Add(new MailFeed());
					}
				}


#if DEBUG
				// メモリ使用量
				Console.WriteLine("メールチェック完了時: " + GC.GetTotalMemory(false));
#endif
				// エラーが1つでもあれば
				if (errorCount >= 1)
				{
					this.Invoke((MethodInvoker)delegate
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Error;
						BalloonManager.NotifyIcon = this.notify;

						// バルーンでエラーを表示させる設定の場合
						if (!ReadSetting.Setting.BalloonInvisibleError)
						{
							if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckErrorCount, errorCount) + detail.Trim(),
								Properties.Resources.SoftwareName, 5000, ToolTipIcon.Error))
							{
								// バルーンが使えない
								if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
								{
									// 外部アプリに委託
								}
								// ポップアップでは通知しない
							}
						}

						// 通知領域ポップアップメニューの太字部分変更
						notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);

						string notifyText = string.Format(Properties.Resources.MailCheckErrorCountNotify, errorCount) + Properties.Resources.SoftwareName;
						this.notify.Icon = icons.errormessage;

						// ツールチップは64文字以上表示できないので
						if ((notifyText + "\n" + detail.Trim()).Length > 63)
							this.notify.Text = notifyText;
						else
							this.notify.Text = notifyText + "\n" + detail.Trim();
					});
				}
				else
				{
					// 新着が1つでもあれば
					if (newMessage)
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
						BalloonManager.NotifyIcon = this.notify;
						if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckCountBalloons, totalMessageCount) + "\n" + detail.Trim(),
							Properties.Resources.SoftwareNameShort, 5000, ToolTipIcon.Info))
						{
							// バルーンが使えない
							if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
							{
								// 外部アプリに委託
								try
								{
									if (string.IsNullOrEmpty(ReadSetting.Setting.RunApplicationOption))
									{
										Process.Start(ReadSetting.Setting.RunApplicationPath,
											"\"" + string.Format(Properties.Resources.MailCheckCountBalloons, totalMessageCount) + "\"");
									}
									else
									{
										Process.Start(ReadSetting.Setting.RunApplicationPath,
											"\"" + string.Format(Properties.Resources.MailCheckCountBalloons, totalMessageCount) + "\\n" + string.Format(ReadSetting.Setting.RunApplicationOption, detail) + "\"");
									}
								}
								catch
								{

								}
							}
							else if ((ReadSetting.Setting.NewMailBalloonOrPopupNotify &&
								ReadSetting.Setting.NewMailNotifyOperation == ConfigData.NewMailNotification.Popup) ||
								!Program.EnableBalloon)
							{
								this.Invoke((MethodInvoker)delegate
								{
									// ポップアップで通知
									frmNewMessage fn = new frmNewMessage(string.Format(Properties.Resources.MailCheckCountBalloons, totalMessageCount) + "\n" + detail.Trim());
									//fn.Show();

									ShowWindow(fn.Handle, 4);
									//#if DEBUG
									//                                    SetWindowPos(fn.Handle, -1,
									//                                        100,
									//                                        100,
									//                                        fn.Width, fn.Height, SWP_NOACTIVE | 0x0040);
									//#else
									SetWindowPos(fn.Handle, -1,
										Screen.PrimaryScreen.WorkingArea.Width - fn.Width,
										Screen.PrimaryScreen.WorkingArea.Height - fn.Height,
										fn.Width, fn.Height, SWP_NOACTIVE | 0x0040);
									//#endif
									fn.TimerEnabled = true;
								});
							}

						}

						// 新着ならサウンド再生
						if (ReadSetting.Setting.PlayNewMailSound)
						{
							if (BalloonManager.ClickAction == BalloonManager.BalloonClickAction.NewMail)
							{
								if (ReadSetting.Setting.PlayDefaultNewMailSound ||
									string.IsNullOrEmpty(ReadSetting.Setting.PlayNewMailSoundPath))
									PlaySounds.Play("MailBeep");
								else
									PlaySounds.Play(ReadSetting.Setting.PlayNewMailSoundPath);
							}
						}
					}

					// トータルで1通以上あれば
					if (totalMessageCount > 0)
					{
						this.Invoke((MethodInvoker)delegate
						{
							string notifyText = string.Format(Properties.Resources.MailCheckCountNotify, totalMessageCount) + Properties.Resources.SoftwareName;

							// 通知領域を変更
							this.notify.Icon = icons.newmessage;

							// ツールチップは64文字以上表示できないので
							if ((notifyText + "\n" + detail.Trim()).Length > 63)
								this.notify.Text = notifyText;
							else
								this.notify.Text = notifyText + "\n" + detail.Trim();

							// メッセージビューアの準備
							// frmMessageViewer.Instance.SetSettings(mfl);
							using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "mailFeed.xml")))
							{
								XmlSerializer mesWrite = new XmlSerializer(typeof(List<MailFeed>));
								mesWrite.Serialize(sw, mfl);
							}



							// 通知領域ポップアップの太字部分の変更
							notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
							notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
							notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
							notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);

							BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
						});
					}
					else
					{
						this.Invoke((MethodInvoker)delegate
						{
							// 通知領域アイコンを変更
							this.notify.Icon = icons.nomessage;
							this.notify.Text = Properties.Resources.SoftwareName;
							BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Information;

							// 通知領域ポップアップの太字部分の変更
							switch (ReadSetting.Setting.NotifyDoubleClickOperation)
							{
								case ConfigData.NotifyDoubleClick.ShowMainWindow:
									notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
									notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);
									break;
								case ConfigData.NotifyDoubleClick.ShowInbox:
									notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
									notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);
									break;
								case ConfigData.NotifyDoubleClick.MailCheck:
									notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
									notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
									notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Bold);
									break;
								default:
									break;
							}
						});

						// メールフィードのファイル削除
						if (File.Exists(Path.Combine(Application.StartupPath, "mailFeed.xml")))
							File.Delete(Path.Combine(Application.StartupPath, "mailFeed.xml"));
					}
				}
			}

			// ステータスバーの内容を変更
			this.Invoke((MethodInvoker)delegate
			{
				this.latestCheckTimeStatus.Text = Properties.Resources.StatusLatestMailCheck + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			});

			// メールチェック関連のコントロールの有効化
			mailCheckControlEnabled(true);

#if DEBUG
			// メモリ使用量
			Console.WriteLine("メールチェック処理完了時: " + GC.GetTotalMemory(false));
#endif
			// ガベージコレクト
			GC.Collect();

#if DEBUG
			// メモリ使用量
			Console.WriteLine("メールチェック処理GC時: " + GC.GetTotalMemory(false));
#endif
		}

		private void mailCheckControlEnabled(bool b)
		{
			this.Invoke((MethodInvoker)delegate
			{
				this.notifyMailCheckMenu.Enabled = b;
				this.messageCheck.Enabled = b;
				this.toolMailCheckButton.Enabled = b;
			});
		}

		#endregion

		#region メニュー

		private void mainSettingMenu_Click(object sender, EventArgs e)
		{
			// 一時的にタイマーを止める
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// 準備して設定画面を表示
			frmSetting fs = new frmSetting();

			// メインコンソールが表示されているかどうかでタスクトレイ表示の有無を決める
			if (this.Visible)
			{
				fs.StartPosition = FormStartPosition.CenterParent;
				fs.ShowInTaskbar = false;
			}
			else
			{
				fs.StartPosition = FormStartPosition.CenterScreen;
				fs.ShowInTaskbar = true;
			}
			// 表示
			fs.ShowDialog();
			// 読み込みし直す
			ReadSetting.Read();

			// 通知領域ポップアップの太字部分の変更
			switch (ReadSetting.Setting.NotifyDoubleClickOperation)
			{
				case ConfigData.NotifyDoubleClick.ShowMainWindow:
					notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					break;
				case ConfigData.NotifyDoubleClick.ShowInbox:
					notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					break;
				case ConfigData.NotifyDoubleClick.MailCheck:
					notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
					break;
				default:
					break;
			}

			// 個別メールチェック用のメニューの生成
			createAccountMailCheckMenu();

			// タイマーを設定値に戻す
			m_checkTimer.Change(ReadSetting.Setting.MailCheckInterval, ReadSetting.Setting.MailCheckInterval);
		}

		private void createAccountMailCheckMenu()
		{
			// リストの初期化
			m_mailCheckAccountToolBar.Clear();
			m_mailCheckAccountNotify.Clear();

			// ポップアップメニュー用に「すべてメールチェック」と区切り線を入れる
			ToolStripMenuItem allCheck = new ToolStripMenuItem(Properties.Resources.PopupAllCheckMenu);
			if (ReadSetting.Setting.NotifyDoubleClickOperation == ConfigData.NotifyDoubleClick.MailCheck)
			{
				allCheck.Font = new Font(menuStrip1.Font, FontStyle.Bold);
			}
			ToolStripSeparator popupSeparator = new ToolStripSeparator();
			allCheck.Click += new EventHandler(messageCheck_Click);

			// 個別メールチェック用のメニューを作る
			foreach (Account a in ReadSetting.Setting.Accounts)
			{
				ToolStripMenuItem tsmi =
					new ToolStripMenuItem("&" + (ReadSetting.Setting.Accounts.IndexOf(a) + 1) + ". " + a.SetName);
				ToolStripMenuItem tsminotify =
					new ToolStripMenuItem("&" + (ReadSetting.Setting.Accounts.IndexOf(a) + 1) + ". " + a.SetName);
				tsmi.Click += new EventHandler(tsmi_Click);
				tsminotify.Click += new EventHandler(tsmi_Click);

				m_mailCheckAccountToolBar.Add(tsmi);
				m_mailCheckAccountNotify.Add(tsminotify);
			}
			// ツールバーと通知領域のアカウントメニューを初期化
			toolMailCheckButton.DropDown.Items.Clear();
			notifyMailCheckMenu.DropDown.Items.Clear();
			// 追加
			foreach (ToolStripMenuItem t in m_mailCheckAccountToolBar)
			{
				toolMailCheckButton.DropDownItems.Add(t);
			}
			notifyMailCheckMenu.DropDown.Items.Add(allCheck);
			notifyMailCheckMenu.DropDown.Items.Add(popupSeparator);
			notifyMailCheckMenu.DropDown.Items.AddRange(m_mailCheckAccountNotify.ToArray());
		}

		private void tsmi_Click(object sender, EventArgs e)
		{
			// アカウント毎にメールチェックをする
			ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
			mailCheckResult mcr = new mailCheckResult();


			int menuIndex;
			if (m_mailCheckAccountNotify.Contains(tsmi))
				menuIndex = m_mailCheckAccountNotify.IndexOf(tsmi);
			else
				menuIndex = m_mailCheckAccountToolBar.IndexOf(tsmi);


			// 進行状況をメインコンソールに表示
			WriteLog(string.Format(Properties.Resources.MailChecking, ReadSetting.Setting.Accounts[menuIndex].SetName), Color.DodgerBlue);

			// 通知領域アイコン変更
			this.notify.Icon = icons.checkmessage;

			// メールチェック開始
			this.Invoke((MethodInvoker)delegate
			{
				mcr = mailCheck(ReadSetting.Setting.Accounts[menuIndex]);
			});

			#region メールチェック処理

			// エラーの場合
			if (mcr.IsError)
			{
				this.Invoke((MethodInvoker)delegate
				{
					BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Error;
					BalloonManager.NotifyIcon = this.notify;
					if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckErrorCount, 1) + mcr.Detail.Trim(),
						Properties.Resources.SoftwareName, 5000, ToolTipIcon.Error))
					{
						// バルーンが使えない
						if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
						{
							// 外部アプリに委託
						}
						// ポップアップでは通知しない

						// 通知領域ポップアップメニューの太字部分変更
						notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					}
					string notifyText = string.Format(Properties.Resources.MailCheckErrorCountNotify, 1) + Properties.Resources.SoftwareName;
					this.notify.Icon = icons.errormessage;

					// ツールチップは64文字以上表示できないので
					if ((notifyText + "\n" + mcr.Detail.Trim()).Length > 63)
						this.notify.Text = notifyText;
					else
						this.notify.Text = notifyText + "\n" + mcr.Detail.Trim();
				});
			}
			else
			{
				// 新着が1つでもあれば
				if (mcr.NewMessage)
				{
					BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
					BalloonManager.NotifyIcon = this.notify;
					if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckCountBalloons, mcr.MessageCount) + "\n" + mcr.Detail.Trim(),
						Properties.Resources.SoftwareNameShort, 5000, ToolTipIcon.Info))
					{
						// バルーンが使えない
						if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
						{
							// 外部アプリに委託
							try
							{
								if (string.IsNullOrEmpty(ReadSetting.Setting.RunApplicationOption))
								{
									Process.Start(ReadSetting.Setting.RunApplicationPath,
										"\"" + string.Format(Properties.Resources.MailCheckCountBalloons, mcr.MessageCount) + "\"");
								}
								else
								{
									Process.Start(ReadSetting.Setting.RunApplicationPath,
										"\"" + string.Format(Properties.Resources.MailCheckCountBalloons, mcr.MessageCount) + "\\n" + string.Format(ReadSetting.Setting.RunApplicationOption, mcr.Detail) + "\"");
								}
							}
							catch
							{

							}
						}
						else if ((ReadSetting.Setting.NewMailBalloonOrPopupNotify &&
							ReadSetting.Setting.NewMailNotifyOperation == ConfigData.NewMailNotification.Popup) ||
							!Program.EnableBalloon)
						{
							this.Invoke((MethodInvoker)delegate
							{
								// ポップアップで通知
								frmNewMessage fn = new frmNewMessage(string.Format(Properties.Resources.MailCheckCountBalloons, mcr.MessageCount) + "\n" + mcr.Detail.Trim());
								//fn.Show();

								ShowWindow(fn.Handle, 4);
								SetWindowPos(fn.Handle, -1,
									Screen.PrimaryScreen.WorkingArea.Width - fn.Width,
									Screen.PrimaryScreen.WorkingArea.Height - fn.Height,
									fn.Width, fn.Height, SWP_NOACTIVE | 0x0040);
								fn.TimerEnabled = true;
							});
						}

					}

					// 新着ならサウンド再生
					if (ReadSetting.Setting.PlayNewMailSound)
					{
						if (BalloonManager.ClickAction == BalloonManager.BalloonClickAction.NewMail)
						{
							if (ReadSetting.Setting.PlayDefaultNewMailSound ||
								string.IsNullOrEmpty(ReadSetting.Setting.PlayNewMailSoundPath))
								PlaySounds.Play("MailBeep");
							else
								PlaySounds.Play(ReadSetting.Setting.PlayNewMailSoundPath);
						}
					}
				}

				// トータルで1通以上あれば
				if (mcr.MessageCount > 0)
				{
					this.Invoke((MethodInvoker)delegate
					{
						string notifyText = string.Format(Properties.Resources.MailCheckCountNotify, mcr.MessageCount) + Properties.Resources.SoftwareName;

						// 通知領域を変更
						this.notify.Icon = icons.newmessage;

						// ツールチップは64文字以上表示できないので
						if ((notifyText + "\n" + mcr.Detail.Trim()).Length > 63)
							this.notify.Text = notifyText;
						else
							this.notify.Text = notifyText + "\n" + mcr.Detail.Trim();

						// メッセージビューアの準備
						List<MailFeed> mfl = new List<MailFeed>();
						for (int i = 0; i < ReadSetting.Setting.Accounts.Count; i++)
						{
							mfl.Add(new MailFeed());
						}
						mfl[menuIndex] = mcr.Feed;
						// メッセージビューアの準備
						// frmMessageViewer.Instance.SetSettings(mfl);
						using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "mailFeed.xml")))
						{
							XmlSerializer mesWrite = new XmlSerializer(typeof(List<MailFeed>));
							mesWrite.Serialize(sw, mfl);
						}

						// 通知領域ポップアップの太字部分の変更
						notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
						notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
						notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
						notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);

						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
					});
				}
				else
				{
					this.Invoke((MethodInvoker)delegate
					{
						// 通知領域アイコンを変更
						this.notify.Icon = icons.nomessage;
						this.notify.Text = Properties.Resources.SoftwareName;
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Information;

						// 通知領域ポップアップの太字部分の変更
						switch (ReadSetting.Setting.NotifyDoubleClickOperation)
						{
							case ConfigData.NotifyDoubleClick.ShowMainWindow:
								notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
								notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);
								break;
							case ConfigData.NotifyDoubleClick.ShowInbox:
								notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
								notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Regular);
								break;
							case ConfigData.NotifyDoubleClick.MailCheck:
								notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyOpenInboxMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
								notifyMailCheckMenu.Font = new Font(menuStrip1.Font, FontStyle.Bold);
								notifyMailCheckMenu.DropDownItems[1].Font = new Font(menuStrip1.Font, FontStyle.Bold);
								break;
							default:
								break;
						}
					});

				}
			}

			// ステータスバーの内容を変更
			this.Invoke((MethodInvoker)delegate
			{
				this.latestCheckTimeStatus.Text = Properties.Resources.StatusLatestMailCheck + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			});

			// メールチェック関連のコントロールの有効化
			mailCheckControlEnabled(true);


			#endregion
		}

		private void mainExitMenu_Click(object sender, EventArgs e)
		{
			// 終了処理
			Program.SystemShutdown = true;
			this.Close();
		}

		private void mainConnectStartMenu_Click(object sender, EventArgs e)
		{
			// 接続処理
			WriteLog(Properties.Resources.WriteLogConnectionResumed, Color.Blue);
			// タイマー開始するのと同時にメールチェック
			m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);

			mainConnectStartMenu.Enabled = false;
			mainConnectPauseMenu.Enabled = true;
			m_connecting = true;
			notifyConnectMenu.Text = Properties.Resources.NotifyConnectMenuDisconnect;
			notifyConnectMenu.Image = icons.disconnect;
		}

		private void mainConnectPauseMenu_Click(object sender, EventArgs e)
		{
			// 切断処理
			WriteLog(Properties.Resources.WriteLogConnectionStopped, Color.Blue);
			// タイマーを中止
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			mainConnectStartMenu.Enabled = true;
			mainConnectPauseMenu.Enabled = false;
			notify.Icon = icons.pausemessage;
			this.notify.Text = string.Format(Properties.Resources.NotifyPause, Properties.Resources.SoftwareName);
			BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Other;
			m_connecting = false;
			notifyConnectMenu.Text = Properties.Resources.NotifyConnectMenuConnect;
			notifyConnectMenu.Image = icons.connect;
		}


		private void mainHideMenu_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		#endregion

		#region ヘルプ
		private void helpAboutMenu_Click(object sender, EventArgs e)
		{
			frmAbout fa = new frmAbout();
			if (this.Visible)
				fa.StartPosition = FormStartPosition.CenterParent;
			else
				fa.StartPosition = FormStartPosition.CenterScreen;
			fa.ShowDialog(this);

			//throw new ApplicationException("It's test.");
		}
		#endregion

		#region 例外処理

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			// タイマーを中止
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// ログの書き出し
			using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "log.txt"), true))
			{
				this.Invoke((MethodInvoker)delegate
				{
					string[] str = logBox.Text.Split('\n');
					foreach (string s in str)
					{
						sw.WriteLine(s);
					}
				});
			}

			// 他スレッドでの例外の補足
			frmExceptionManager em = new frmExceptionManager((Exception)e.ExceptionObject);
			em.Show();
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			// タイマーを中止
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// ログの書き出し
			using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "log.txt"), true))
			{
				this.Invoke((MethodInvoker)delegate
				{
					string[] str = logBox.Text.Split('\n');
					foreach (string s in str)
					{
						sw.WriteLine(str);
					}
				});
			}

			// メインスレッドでの例外の補足
			frmExceptionManager em = new frmExceptionManager(e.Exception);
			em.Show();
		}


		#endregion

		#region 通知領域ポップアップ


		private void notifyMainWindowMenu_Click(object sender, EventArgs e)
		{
			this.Visible = true;
			this.WindowState = FormWindowState.Normal;
		}

		private void notifyOpenInboxMenu_Click(object sender, EventArgs e)
		{
			openInbox();
		}

		/// <summary>
		/// 受信トレイの表示
		/// </summary>
		private void openInbox()
		{
			// 受信トレイ表示
			// アカウントが一つだけしか登録されてなければ直開き
			if (ReadSetting.Setting.Accounts.Count == 1)
			{
				string uri = ReadSetting.Setting.Accounts[0].GetInboxUri;
				if (ReadSetting.Setting.NoUseSsl)
					uri = uri.Insert(0, "http://");
				else
					uri = uri.Insert(0, "https://");

				BrowserShow.browserOpen(uri);
			}
			else
			{
				// 複数あれば
				frmAccountSelect ac = new frmAccountSelect(ReadSetting.Setting.Accounts);
				if (ac.ShowDialog() == DialogResult.OK)
				{
					string uri = ac.Account.GetInboxUri;
					if (ReadSetting.Setting.NoUseSsl)
						uri = uri.Insert(0, "http://");
					else
						uri = uri.Insert(0, "https://");

					BrowserShow.browserOpen(uri);
				}
			}
		}

		private void notifyConnectMenu_Click(object sender, EventArgs e)
		{
			// 接続/切断処理
			switch (m_connecting)
			{
				case true:
					mainConnectPauseMenu_Click(null, null);
					break;
				case false:
					mainConnectStartMenu_Click(null, null);
					break;
				default:
					break;
			}
		}

        private void notifyOfficialSiteMenu_Click(object sender, EventArgs e)
		{
			// 公式サイト表示
			if (MessageBox.Show(Properties.Resources.NotifyOfficialSiteDialog, Properties.Resources.SoftwareName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				== DialogResult.Yes)
			{
				BrowserShow.browserOpen("http://soft.udonge.net/");
			}
		}
		#endregion

		#region ログボックスポップアップ

		private void logCopyMenu_Click(object sender, EventArgs e)
		{
			logBox.Copy();
		}

		private void logAllSelectMenu_Click(object sender, EventArgs e)
		{
			logBox.SelectAll();
		}

		private void logDeleteMenu_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Properties.Resources.LogDeleteDialog, Properties.Resources.SoftwareName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
				== DialogResult.Yes)
			{
				logBox.Clear();
				WriteLog(Properties.Resources.SoftwareName + " Version " + Application.ProductVersion, Color.Blue);
			}
		}

		private void logFontMenu_Click(object sender, EventArgs e)
		{
			logFontDialog.Font = ReadSetting.Setting.MainConsoleFont;

			if (logFontDialog.ShowDialog() == DialogResult.OK)
			{
				ReadSetting.Setting.MainConsoleFont = logFontDialog.Font;
				logBox.SelectAll();
				logBox.SelectionFont = logFontDialog.Font;
				logBox.SelectionStart = logBox.TextLength;

				ReadSetting.Save();
			}

		}

		private void logFontDialog_Apply(object sender, EventArgs e)
		{
			logBox.SelectAll();
			logBox.SelectionFont = logFontDialog.Font;
			logBox.SelectionStart = logBox.TextLength;

			ReadSetting.Setting.MainConsoleFont = logFontDialog.Font;
			ReadSetting.Save();
		}

		#endregion

		#region バルーン制御

		private void notify_BalloonTipClicked(object sender, EventArgs e)
		{
			BalloonClickAction();
		}

		private void notify_Click(object sender, EventArgs e)
		{

		}

		private void notify_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				BalloonClickAction();
			}
			else if (e.Button == MouseButtons.Middle)
			{
				if (BalloonManager.ClickAction == BalloonManager.BalloonClickAction.NewMail)
				{
					// 設定によって動作を変える
					switch (ReadSetting.Setting.NotifyDoubleClickOperation)
					{
						case ConfigData.NotifyDoubleClick.ShowMainWindow:
							this.Visible = true;
							this.WindowState = FormWindowState.Normal;
							break;
						case ConfigData.NotifyDoubleClick.ShowInbox:
							frmAccountSelect f = new frmAccountSelect(ReadSetting.Setting.Accounts);
							if (f.ShowDialog() == DialogResult.OK)
								BrowserShow.browserOpen(f.Account.GetInboxUri);
							break;
						case ConfigData.NotifyDoubleClick.MailCheck:
							m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);
							break;
						case ConfigData.NotifyDoubleClick.ShowMessageList:
							messageListMenu_Click(null, null);
							break;
						default:
							break;
					}
				}
			}
		}

		private void notify_DoubleClick(object sender, EventArgs e)
		{
			// 設定によって動作を変える
			switch (ReadSetting.Setting.NotifyDoubleClickOperation)
			{
				case ConfigData.NotifyDoubleClick.ShowMainWindow:
					this.Visible = true;
					this.WindowState = FormWindowState.Normal;
					break;
				case ConfigData.NotifyDoubleClick.ShowInbox:
					openInbox();
					//frmAccountSelect f = new frmAccountSelect(ReadSetting.Setting.Accounts);
					//if (f.ShowDialog() == DialogResult.OK)
					//    BrowserShow.browserOpen(f.Account.GetInboxUri);
					break;
				case ConfigData.NotifyDoubleClick.MailCheck:
					m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);
					break;
				case ConfigData.NotifyDoubleClick.ShowMessageList:
					messageListMenu_Click(null, null);
					break;
				default:
					break;
			}
		}

		private void BalloonClickAction()
		{
			switch (BalloonManager.ClickAction)
			{
				case BalloonManager.BalloonClickAction.NewMail:
					// 新着メールであれば
					// 設定で決めた動作をおこなう
					switch (ReadSetting.Setting.BalloonClickOperation)
					{
						case ConfigData.BalloonClick.MessageList:
							messageListMenu_Click(null, null);
							break;
						case ConfigData.BalloonClick.RunBrowser:
							openInbox();
							//frmAccountSelect fas = new frmAccountSelect(ReadSetting.Setting.Accounts);
							//if (fas.ShowDialog() == DialogResult.OK)
							//{
							//    if (ReadSetting.Setting.NoUseSsl)
							//        BrowserShow.browserOpen("http://" + fas.Account.GetInboxUri);
							//    else
							//        BrowserShow.browserOpen("https://" + fas.Account.GetInboxUri);
							//}
							break;
						case ConfigData.BalloonClick.RunExecutive:
							if ((!string.IsNullOrEmpty(ReadSetting.Setting.NotificationRunExecutiveFilePath)) ||
								(File.Exists(ReadSetting.Setting.NotificationRunExecutiveFilePath)))
							{
								runApplication();
							}
							break;
						default:
							break;
					}
					break;
				case BalloonManager.BalloonClickAction.Information:
					break;
				case BalloonManager.BalloonClickAction.Error:
					m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);
					break;
				default:
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

		#endregion

		#region 電源状態

		void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
				case PowerModes.Resume:
					// 中断状態から復帰
					if (m_connecting)
					{
						BalloonManager.NotifyIcon = this.notify;
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Information;
						BalloonManager.Show(Properties.Resources.BalloonPowerResumed, Properties.Resources.SoftwareName, 5000, ToolTipIcon.Info);
						WriteLog(Properties.Resources.WriteLogPowerResumed, Color.Blue);

						this.notify.Icon = icons.nomessage;
						this.notify.Text = Properties.Resources.SoftwareName;

						m_checkTimer.Change(ReadSetting.Setting.PowerResumedDelayTime, ReadSetting.Setting.MailCheckInterval);
					}
					break;
				case PowerModes.StatusChange:
					break;
				case PowerModes.Suspend:
					// 中断
					if (m_connecting)
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Other;

						WriteLog(Properties.Resources.WriteLogPowerSuspention, Color.Blue);

						this.notify.Icon = icons.pausemessage;
						//this.notify.Text = "中断中 - " + Properties.Resources.SoftwareName;
						m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);
					}
					break;
				default:
					break;
			}
		}

		#endregion

		#region 終了処理

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.WriteLog("Program.SystemShutdown == " + Program.SystemShutdown.ToString());
			Program.WriteLog("frmMain_e.CloseReason == " + e.CloseReason.ToString());

			//if (!(shutdownMode) || (e.CloseReason == CloseReason.UserClosing) || (e.CloseReason == CloseReason.FormOwnerClosing)
			//    || (e.CloseReason == CloseReason.ApplicationExitCall) || (e.CloseReason == CloseReason.TaskManagerClosing))
			//{
			//    e.Cancel = true;

			//    this.WindowState = FormWindowState.Minimized;
			//    this.Visible = false;
			//}
			//else
			//{
			//    e.Cancel = false;
			//}

			// 終了処理であれば閉じる。でなければ閉じずに縮小
			if ((Program.SystemShutdown) || ((e.CloseReason | CloseReason.FormOwnerClosing) == CloseReason.FormOwnerClosing))
			{

				Program.WriteLog("終了するよ！");

				e.Cancel = false;
				Program.SystemShutdown = true;

				// メールフィードのファイル削除
				if (File.Exists(Path.Combine(Application.StartupPath, "mailFeed.xml")))
					File.Delete(Path.Combine(Application.StartupPath, "mailFeed.xml"));
			}
			else if (!(Program.SystemShutdown) ||
				((e.CloseReason | CloseReason.UserClosing) == CloseReason.UserClosing) ||
				((e.CloseReason | CloseReason.None) == CloseReason.None))
			{
				Program.WriteLog("終了しないよ！");
				e.Cancel = true;

				this.WindowState = FormWindowState.Minimized;
				this.Visible = false;
			}
		}

		// システムがシャットダウンされるとき
		private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			Program.WriteLog("SystemEvents_SessionEnding: " + e.Reason.ToString());

			Program.SystemShutdown = true;
		}

		#endregion

		private void frmMain_KeyUp(object sender, KeyEventArgs e)
		{
			//if (e.KeyData == Keys.Alt)
			//{
			//    menuStrip1.Visible = menuStrip1.Visible ? false : true;
			//}
		}

		private void messageCheck_Click(object sender, EventArgs e)
		{
			// メールチェック開始
			m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);
		}

		private void messageListMenu_Click(object sender, EventArgs e)
		{
			// メッセージリスト表示
			//frmMessageViewer.Instance.Show();
			//frmMessageViewer.Instance.Activate();
			//MessageList.Forms.frmMessageViewer f = new Gekko.MessageList.Forms.frmMessageViewer();
			//f.Show();

			Process.Start(Path.Combine(Application.StartupPath, "MessageList.exe"));
		}

		private void logBox_MouseDown(object sender, MouseEventArgs e)
		{
			// ログボックスでマウスを押した
			if (e.Button == MouseButtons.Right)
			{
				if (logBox.SelectionLength < 1)
				{
					logCopyMenu.Enabled = false;
				}
				else
				{
					logCopyMenu.Enabled = true;
				}

				if (logBox.TextLength > 0)
				{
					logDeleteMenu.Enabled = true;
				}
				else
				{
					logDeleteMenu.Enabled = false;
				}
			}
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Visible = false;
			}
			else if (this.WindowState == FormWindowState.Normal)
			{
				// キャレットを一番下にする
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.ScrollToCaret();
			}
		}
	}
}