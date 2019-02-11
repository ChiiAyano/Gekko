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
		// �^�C�}�[�X���b�h
		System.Threading.Timer m_checkTimer;
		// �Ō�ɓǂ񂾃��[���̎�M����
		DateTime m_lastMailDate;
		// �ڑ����
		bool m_connecting = true;
		// �ʂɃ��[���`�F�b�N�����邽�߂́A�A�J�E���g�̃��j���[����
		List<ToolStripMenuItem> m_mailCheckAccountToolBar = new List<ToolStripMenuItem>();
		List<ToolStripMenuItem> m_mailCheckAccountNotify = new List<ToolStripMenuItem>();
		Gekko.MessageList.Forms.frmMessageViewer m_msgV;



		// �萔
		readonly string path = Path.Combine(Application.StartupPath, "config.xml");

		public const uint EM_LINESCROLL = 0x00B6;

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lparam);

		#region ��A�N�e�B�u�ŃE�B���h�E��\��������

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

		#region �����ݒ�

		public frmMain()
		{
			InitializeComponent();

			asm = Assembly.GetExecutingAssembly();
			asmName = asm.GetName();

			// �ꎞ�t�@�C���̍폜
			if (File.Exists(Path.Combine(Path.GetTempPath(), "Gekko.zip")))
				File.Delete(Path.Combine(Path.GetTempPath(), "Gekko.zip"));
			if (File.Exists(Path.Combine(Path.GetTempPath(), "GekkoUpdater.zip")))
				File.Delete(Path.Combine(Path.GetTempPath(), "GekkoUpdater.zip"));
			if (Directory.Exists(Path.Combine(Path.GetTempPath(), "GekkoUpdater")))
				Directory.Delete(Path.Combine(Path.GetTempPath(), "GekkoUpdater"), true);

			// �ݒ�t�@�C���̃p�X���w��
			ReadSetting.Path = path;

			// �A�C�R���ݒ�
			this.Icon = icons.gekko;
			// �ʒm�̈�̐ݒ�
			this.notify.Icon = icons.nomessage;
			this.notify.Text = Properties.Resources.SoftwareName;

			// ��O�L���b�`���[
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			// �d�����
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
			SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);

			// �Ō�ɓǂ񂾃��[���̎�M����
			m_lastMailDate = new DateTime();


			// �ݒ�t�@�C��������Γǂ�
			if (System.IO.File.Exists(path))
			{
				ReadSetting.Path = path;
				ReadSetting.Read();

				// ���[���`�F�b�N�^�C�}�[�̏�����
				m_checkTimer = new System.Threading.Timer(new TimerCallback(mailCheckTimer), null, Timeout.Infinite, ReadSetting.Setting.MailCheckInterval);
				// �A�J�E���g�ݒ肪�܂��Ȃ����
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
				// ���[���`�F�b�N�^�C�}�[�̏������i�^�C�}�[�͍쓮���Ȃ��j
				m_checkTimer = new System.Threading.Timer(new TimerCallback(mailCheckTimer), null, Timeout.Infinite, Timeout.Infinite);
				switch (MessageBox.Show(Properties.Resources.FirstRunDialog,
					Properties.Resources.SoftwareName,
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question))
				{
					case System.Windows.Forms.DialogResult.Yes:
						// �Ȃ���Ώ�����
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
						// �Ȃ���Ώ�����
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

			// �A�J�E���g�ݒ肪����΁A
			// �l�b�g���[�N�ڑ����m�����Ă���΃^�C�}�[���N������
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				// �N������
				m_checkTimer.Change(ReadSetting.Setting.PowerResumedDelayTime, ReadSetting.Setting.MailCheckInterval);
			}

			// ���b�Z�[�W�ꗗ����
			//frmMessageViewer.Instance.SetSettings();

			// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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

			// ���C���R���\�[���̃t�H���g�ύX
			logBox.Font = ReadSetting.Setting.MainConsoleFont;

			// �A�J�E���g���j���[�̍X�V
			createAccountMailCheckMenu();

			// ���b�Z�[�W�ꗗ�̏�����
			m_msgV = new Gekko.MessageList.Forms.frmMessageViewer();

			// �|�b�v�A�b�v�Œʒm
			//frmNewMessage fn = new frmNewMessage("0 �ʂ̐V�����b�Z�[�W������܂��B\nabcde\ncderfg");
			//fn.Show();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			this.Visible = ReadSetting.Setting.HideConsole ? false : true;

			//�E�B���h�E���\�����ꂽ���邱��
			WriteLog(Properties.Resources.SoftwareName + " Version " + Application.ProductVersion, Color.Blue);
		}

		#endregion

		#region ���O�o��

		/// <summary>
		/// ���C���R���\�[���̃��O���o�͂���
		/// </summary>
		/// <param name="message">�o�͂�����e</param>
		/// <param name="cl">������̐F</param>
		private void WriteLog(string message, Color cl)
		{
			// ���O���o�͂���
			this.Invoke((MethodInvoker)delegate()
			{
				// �����̏o��
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.SelectionColor = Color.FromArgb(220, 0, 73);
				this.logBox.SelectionFont = ReadSetting.Setting.MainConsoleFont;
				this.logBox.SelectedText = DateTime.Now.ToString("MM/dd HH:mm:ss: ");

				// ���e�̏o��
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.SelectionColor = cl;
				this.logBox.SelectionFont = ReadSetting.Setting.MainConsoleFont;
				this.logBox.SelectedText = message + "\r\n";

				// �L�����b�g����ԉ��ɂ���
				//this.logBox.Focus();
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.ScrollToCaret();
			});
		}
		#endregion

		#region ���[���`�F�b�N

		private struct mailCheckResult
		{
			/// <summary>
			/// �V�������[�������������ǂ���
			/// </summary>
			public bool NewMessage;
			/// <summary>
			/// ���ǃ��[����
			/// </summary>
			public int MessageCount;
			/// <summary>
			/// ��ԐV�������[���̎�M��
			/// </summary>
			public DateTime RecentMailDate;
			/// <summary>
			/// �G���[������������
			/// </summary>
			public bool IsError;
			/// <summary>
			/// ��M�󋵂̓��e
			/// </summary>
			public string Detail;
			/// <summary>
			/// ���[�����
			/// </summary>
			public MailFeed Feed;
		}

		private mailCheckResult mailCheck(Account account)
		{
			// �ڑ����ʂ̊i�[
			mailCheckResult mcr = new mailCheckResult { Detail = string.Empty, Feed = new MailFeed(), IsError = false, MessageCount = 0, NewMessage = false, RecentMailDate = new DateTime() };

			try
			{
				// �e�A�J�E���g�̐ݒ�����ɁA���[����ǂ�
				MailCheck mc = new MailCheck(account.AccountName, account.Password, account.Domain);
				MailFeed mf = mc.Check();
				mcr.Feed = mf;

#if DEBUG
				// �������g�p��
				Console.WriteLine("���[���`�F�b�N��: " + GC.GetTotalMemory(false));
#endif

				// ���[����1���ȏ゠���
				if (mf.Entry.Count > 0)
				{
					if (mf.Entry[0].Modified > m_lastMailDate)
					{
						// �V�������[���������
						mcr.NewMessage = true;
						// ��M�������X�V
						mcr.RecentMailDate = mf.Entry[0].Modified;
					}
					else
					{
						// �V�������[�����Ȃ�
					}
					// ���[�������J�E���g
					mcr.MessageCount = mf.FullCount;

					WriteLog(string.Format(Properties.Resources.MailCheckUnreadCount, mf.FullCount), Color.Green);

					// �ʒm�̈擙�ɕ\������ڍׂ�ǋL
					mcr.Detail = mcr.Detail.Insert(mcr.Detail.Length, string.Format(Properties.Resources.MailCheckMailCount + "\n", account.SetName, mf.FullCount));
				}
				else
				{
					WriteLog(Properties.Resources.MailCheckNoUnread, Color.DarkGray);
				}
			}
			catch (Exception wex)
			{
				// �G���[�ł���
				mcr.IsError = true;
				// �G���[���
				WriteLog(string.Format(Properties.Resources.MailCheckError, wex.Message), Color.Red);
				// �ʒm�̈擙�ɕ\������ڍׂ�ǋL
				mcr.Detail = mcr.Detail.Insert(mcr.Detail.Length, "! " + account.SetName + "\n");
			}

			// �Ō�ɒʐM���ʂ�Ԃ�
			return mcr;
		}

		private void mailCheckTimer(object o)
		{
			// ����
			List<MailFeed> mfl = new List<MailFeed>();	// ���[���t�B�[�h�̃��X�g
			bool newMessage = false;					// �V�������[������������
			string detail = string.Empty;				// ���ǃ��b�Z�[�W�̏ڍ�
			int totalMessageCount = 0;					// ���v���[����
			int errorCount = 0;							// �G���[�A�J�E���g��

			//frmMessageViewer.Instance.SetSettings();	// ���b�Z�[�W�ꗗ�̏���

			// �ڑ���Ԃ��m�����Ă����
			if (NetworkInterface.GetIsNetworkAvailable())
			{
				// �A�J�E���g����0�Ȃ甲����
				if (ReadSetting.Setting.Accounts.Count == 0)
					return;

				// ���[���`�F�b�N���͊֘A�R���g���[���𖳌���
				mailCheckControlEnabled(false);

				foreach (Account a in ReadSetting.Setting.Accounts)
				{
					// �A�J�E���g���g�p����ݒ�̎��Ƀ`�F�b�N
					if (a.Use)
					{
						// �i�s�󋵂����C���R���\�[���ɕ\��
						WriteLog(string.Format(Properties.Resources.MailChecking, a.SetName), Color.DodgerBlue);
						// �ʒm�̈�A�C�R���ύX
						if (!ReadSetting.Setting.DisableNotifyAnimation)
							this.notify.Icon = icons.checkmessage;

						// ���[���`�F�b�N�J�n
						mailCheckResult checkResult = mailCheck(a);
						// ����
						mfl.Add(checkResult.Feed);
						if (checkResult.IsError)
							errorCount++; // �G���[���������Ƃ��͉��Z
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
				// �������g�p��
				Console.WriteLine("���[���`�F�b�N������: " + GC.GetTotalMemory(false));
#endif
				// �G���[��1�ł������
				if (errorCount >= 1)
				{
					this.Invoke((MethodInvoker)delegate
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Error;
						BalloonManager.NotifyIcon = this.notify;

						// �o���[���ŃG���[��\��������ݒ�̏ꍇ
						if (!ReadSetting.Setting.BalloonInvisibleError)
						{
							if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckErrorCount, errorCount) + detail.Trim(),
								Properties.Resources.SoftwareName, 5000, ToolTipIcon.Error))
							{
								// �o���[�����g���Ȃ�
								if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
								{
									// �O���A�v���Ɉϑ�
								}
								// �|�b�v�A�b�v�ł͒ʒm���Ȃ�
							}
						}

						// �ʒm�̈�|�b�v�A�b�v���j���[�̑��������ύX
						notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);

						string notifyText = string.Format(Properties.Resources.MailCheckErrorCountNotify, errorCount) + Properties.Resources.SoftwareName;
						this.notify.Icon = icons.errormessage;

						// �c�[���`�b�v��64�����ȏ�\���ł��Ȃ��̂�
						if ((notifyText + "\n" + detail.Trim()).Length > 63)
							this.notify.Text = notifyText;
						else
							this.notify.Text = notifyText + "\n" + detail.Trim();
					});
				}
				else
				{
					// �V����1�ł������
					if (newMessage)
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
						BalloonManager.NotifyIcon = this.notify;
						if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckCountBalloons, totalMessageCount) + "\n" + detail.Trim(),
							Properties.Resources.SoftwareNameShort, 5000, ToolTipIcon.Info))
						{
							// �o���[�����g���Ȃ�
							if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
							{
								// �O���A�v���Ɉϑ�
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
									// �|�b�v�A�b�v�Œʒm
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

						// �V���Ȃ�T�E���h�Đ�
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

					// �g�[�^����1�ʈȏ゠���
					if (totalMessageCount > 0)
					{
						this.Invoke((MethodInvoker)delegate
						{
							string notifyText = string.Format(Properties.Resources.MailCheckCountNotify, totalMessageCount) + Properties.Resources.SoftwareName;

							// �ʒm�̈��ύX
							this.notify.Icon = icons.newmessage;

							// �c�[���`�b�v��64�����ȏ�\���ł��Ȃ��̂�
							if ((notifyText + "\n" + detail.Trim()).Length > 63)
								this.notify.Text = notifyText;
							else
								this.notify.Text = notifyText + "\n" + detail.Trim();

							// ���b�Z�[�W�r���[�A�̏���
							// frmMessageViewer.Instance.SetSettings(mfl);
							using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "mailFeed.xml")))
							{
								XmlSerializer mesWrite = new XmlSerializer(typeof(List<MailFeed>));
								mesWrite.Serialize(sw, mfl);
							}



							// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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
							// �ʒm�̈�A�C�R����ύX
							this.notify.Icon = icons.nomessage;
							this.notify.Text = Properties.Resources.SoftwareName;
							BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Information;

							// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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

						// ���[���t�B�[�h�̃t�@�C���폜
						if (File.Exists(Path.Combine(Application.StartupPath, "mailFeed.xml")))
							File.Delete(Path.Combine(Application.StartupPath, "mailFeed.xml"));
					}
				}
			}

			// �X�e�[�^�X�o�[�̓��e��ύX
			this.Invoke((MethodInvoker)delegate
			{
				this.latestCheckTimeStatus.Text = Properties.Resources.StatusLatestMailCheck + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			});

			// ���[���`�F�b�N�֘A�̃R���g���[���̗L����
			mailCheckControlEnabled(true);

#if DEBUG
			// �������g�p��
			Console.WriteLine("���[���`�F�b�N����������: " + GC.GetTotalMemory(false));
#endif
			// �K�x�[�W�R���N�g
			GC.Collect();

#if DEBUG
			// �������g�p��
			Console.WriteLine("���[���`�F�b�N����GC��: " + GC.GetTotalMemory(false));
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

		#region ���j���[

		private void mainSettingMenu_Click(object sender, EventArgs e)
		{
			// �ꎞ�I�Ƀ^�C�}�[���~�߂�
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// �������Đݒ��ʂ�\��
			frmSetting fs = new frmSetting();

			// ���C���R���\�[�����\������Ă��邩�ǂ����Ń^�X�N�g���C�\���̗L�������߂�
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
			// �\��
			fs.ShowDialog();
			// �ǂݍ��݂�����
			ReadSetting.Read();

			// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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

			// �ʃ��[���`�F�b�N�p�̃��j���[�̐���
			createAccountMailCheckMenu();

			// �^�C�}�[��ݒ�l�ɖ߂�
			m_checkTimer.Change(ReadSetting.Setting.MailCheckInterval, ReadSetting.Setting.MailCheckInterval);
		}

		private void createAccountMailCheckMenu()
		{
			// ���X�g�̏�����
			m_mailCheckAccountToolBar.Clear();
			m_mailCheckAccountNotify.Clear();

			// �|�b�v�A�b�v���j���[�p�Ɂu���ׂă��[���`�F�b�N�v�Ƌ�؂��������
			ToolStripMenuItem allCheck = new ToolStripMenuItem(Properties.Resources.PopupAllCheckMenu);
			if (ReadSetting.Setting.NotifyDoubleClickOperation == ConfigData.NotifyDoubleClick.MailCheck)
			{
				allCheck.Font = new Font(menuStrip1.Font, FontStyle.Bold);
			}
			ToolStripSeparator popupSeparator = new ToolStripSeparator();
			allCheck.Click += new EventHandler(messageCheck_Click);

			// �ʃ��[���`�F�b�N�p�̃��j���[�����
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
			// �c�[���o�[�ƒʒm�̈�̃A�J�E���g���j���[��������
			toolMailCheckButton.DropDown.Items.Clear();
			notifyMailCheckMenu.DropDown.Items.Clear();
			// �ǉ�
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
			// �A�J�E���g���Ƀ��[���`�F�b�N������
			ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
			mailCheckResult mcr = new mailCheckResult();


			int menuIndex;
			if (m_mailCheckAccountNotify.Contains(tsmi))
				menuIndex = m_mailCheckAccountNotify.IndexOf(tsmi);
			else
				menuIndex = m_mailCheckAccountToolBar.IndexOf(tsmi);


			// �i�s�󋵂����C���R���\�[���ɕ\��
			WriteLog(string.Format(Properties.Resources.MailChecking, ReadSetting.Setting.Accounts[menuIndex].SetName), Color.DodgerBlue);

			// �ʒm�̈�A�C�R���ύX
			this.notify.Icon = icons.checkmessage;

			// ���[���`�F�b�N�J�n
			this.Invoke((MethodInvoker)delegate
			{
				mcr = mailCheck(ReadSetting.Setting.Accounts[menuIndex]);
			});

			#region ���[���`�F�b�N����

			// �G���[�̏ꍇ
			if (mcr.IsError)
			{
				this.Invoke((MethodInvoker)delegate
				{
					BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Error;
					BalloonManager.NotifyIcon = this.notify;
					if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckErrorCount, 1) + mcr.Detail.Trim(),
						Properties.Resources.SoftwareName, 5000, ToolTipIcon.Error))
					{
						// �o���[�����g���Ȃ�
						if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
						{
							// �O���A�v���Ɉϑ�
						}
						// �|�b�v�A�b�v�ł͒ʒm���Ȃ�

						// �ʒm�̈�|�b�v�A�b�v���j���[�̑��������ύX
						notifyMainWindowMenu.Font = new Font(menuStrip1.Font, FontStyle.Regular);
					}
					string notifyText = string.Format(Properties.Resources.MailCheckErrorCountNotify, 1) + Properties.Resources.SoftwareName;
					this.notify.Icon = icons.errormessage;

					// �c�[���`�b�v��64�����ȏ�\���ł��Ȃ��̂�
					if ((notifyText + "\n" + mcr.Detail.Trim()).Length > 63)
						this.notify.Text = notifyText;
					else
						this.notify.Text = notifyText + "\n" + mcr.Detail.Trim();
				});
			}
			else
			{
				// �V����1�ł������
				if (mcr.NewMessage)
				{
					BalloonManager.ClickAction = BalloonManager.BalloonClickAction.NewMail;
					BalloonManager.NotifyIcon = this.notify;
					if (!BalloonManager.Show(string.Format(Properties.Resources.MailCheckCountBalloons, mcr.MessageCount) + "\n" + mcr.Detail.Trim(),
						Properties.Resources.SoftwareNameShort, 5000, ToolTipIcon.Info))
					{
						// �o���[�����g���Ȃ�
						if (!ReadSetting.Setting.NewMailBalloonOrPopupNotify)
						{
							// �O���A�v���Ɉϑ�
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
								// �|�b�v�A�b�v�Œʒm
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

					// �V���Ȃ�T�E���h�Đ�
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

				// �g�[�^����1�ʈȏ゠���
				if (mcr.MessageCount > 0)
				{
					this.Invoke((MethodInvoker)delegate
					{
						string notifyText = string.Format(Properties.Resources.MailCheckCountNotify, mcr.MessageCount) + Properties.Resources.SoftwareName;

						// �ʒm�̈��ύX
						this.notify.Icon = icons.newmessage;

						// �c�[���`�b�v��64�����ȏ�\���ł��Ȃ��̂�
						if ((notifyText + "\n" + mcr.Detail.Trim()).Length > 63)
							this.notify.Text = notifyText;
						else
							this.notify.Text = notifyText + "\n" + mcr.Detail.Trim();

						// ���b�Z�[�W�r���[�A�̏���
						List<MailFeed> mfl = new List<MailFeed>();
						for (int i = 0; i < ReadSetting.Setting.Accounts.Count; i++)
						{
							mfl.Add(new MailFeed());
						}
						mfl[menuIndex] = mcr.Feed;
						// ���b�Z�[�W�r���[�A�̏���
						// frmMessageViewer.Instance.SetSettings(mfl);
						using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "mailFeed.xml")))
						{
							XmlSerializer mesWrite = new XmlSerializer(typeof(List<MailFeed>));
							mesWrite.Serialize(sw, mfl);
						}

						// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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
						// �ʒm�̈�A�C�R����ύX
						this.notify.Icon = icons.nomessage;
						this.notify.Text = Properties.Resources.SoftwareName;
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Information;

						// �ʒm�̈�|�b�v�A�b�v�̑��������̕ύX
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

			// �X�e�[�^�X�o�[�̓��e��ύX
			this.Invoke((MethodInvoker)delegate
			{
				this.latestCheckTimeStatus.Text = Properties.Resources.StatusLatestMailCheck + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			});

			// ���[���`�F�b�N�֘A�̃R���g���[���̗L����
			mailCheckControlEnabled(true);


			#endregion
		}

		private void mainExitMenu_Click(object sender, EventArgs e)
		{
			// �I������
			Program.SystemShutdown = true;
			this.Close();
		}

		private void mainConnectStartMenu_Click(object sender, EventArgs e)
		{
			// �ڑ�����
			WriteLog(Properties.Resources.WriteLogConnectionResumed, Color.Blue);
			// �^�C�}�[�J�n����̂Ɠ����Ƀ��[���`�F�b�N
			m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);

			mainConnectStartMenu.Enabled = false;
			mainConnectPauseMenu.Enabled = true;
			m_connecting = true;
			notifyConnectMenu.Text = Properties.Resources.NotifyConnectMenuDisconnect;
			notifyConnectMenu.Image = icons.disconnect;
		}

		private void mainConnectPauseMenu_Click(object sender, EventArgs e)
		{
			// �ؒf����
			WriteLog(Properties.Resources.WriteLogConnectionStopped, Color.Blue);
			// �^�C�}�[�𒆎~
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

		#region �w���v
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

		#region ��O����

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			// �^�C�}�[�𒆎~
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// ���O�̏����o��
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

			// ���X���b�h�ł̗�O�̕⑫
			frmExceptionManager em = new frmExceptionManager((Exception)e.ExceptionObject);
			em.Show();
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			// �^�C�}�[�𒆎~
			m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);

			// ���O�̏����o��
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

			// ���C���X���b�h�ł̗�O�̕⑫
			frmExceptionManager em = new frmExceptionManager(e.Exception);
			em.Show();
		}


		#endregion

		#region �ʒm�̈�|�b�v�A�b�v


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
		/// ��M�g���C�̕\��
		/// </summary>
		private void openInbox()
		{
			// ��M�g���C�\��
			// �A�J�E���g������������o�^����ĂȂ���Β��J��
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
				// ���������
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
			// �ڑ�/�ؒf����
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
			// �����T�C�g�\��
			if (MessageBox.Show(Properties.Resources.NotifyOfficialSiteDialog, Properties.Resources.SoftwareName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				== DialogResult.Yes)
			{
				BrowserShow.browserOpen("http://soft.udonge.net/");
			}
		}
		#endregion

		#region ���O�{�b�N�X�|�b�v�A�b�v

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

		#region �o���[������

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
					// �ݒ�ɂ���ē����ς���
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
			// �ݒ�ɂ���ē����ς���
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
					// �V�����[���ł����
					// �ݒ�Ō��߂�����������Ȃ�
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

		#region �d�����

		void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
				case PowerModes.Resume:
					// ���f��Ԃ��畜�A
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
					// ���f
					if (m_connecting)
					{
						BalloonManager.ClickAction = BalloonManager.BalloonClickAction.Other;

						WriteLog(Properties.Resources.WriteLogPowerSuspention, Color.Blue);

						this.notify.Icon = icons.pausemessage;
						//this.notify.Text = "���f�� - " + Properties.Resources.SoftwareName;
						m_checkTimer.Change(Timeout.Infinite, Timeout.Infinite);
					}
					break;
				default:
					break;
			}
		}

		#endregion

		#region �I������

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

			// �I�������ł���Ε���B�łȂ���Ε����ɏk��
			if ((Program.SystemShutdown) || ((e.CloseReason | CloseReason.FormOwnerClosing) == CloseReason.FormOwnerClosing))
			{

				Program.WriteLog("�I�������I");

				e.Cancel = false;
				Program.SystemShutdown = true;

				// ���[���t�B�[�h�̃t�@�C���폜
				if (File.Exists(Path.Combine(Application.StartupPath, "mailFeed.xml")))
					File.Delete(Path.Combine(Application.StartupPath, "mailFeed.xml"));
			}
			else if (!(Program.SystemShutdown) ||
				((e.CloseReason | CloseReason.UserClosing) == CloseReason.UserClosing) ||
				((e.CloseReason | CloseReason.None) == CloseReason.None))
			{
				Program.WriteLog("�I�����Ȃ���I");
				e.Cancel = true;

				this.WindowState = FormWindowState.Minimized;
				this.Visible = false;
			}
		}

		// �V�X�e�����V���b�g�_�E�������Ƃ�
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
			// ���[���`�F�b�N�J�n
			m_checkTimer.Change(0, ReadSetting.Setting.MailCheckInterval);
		}

		private void messageListMenu_Click(object sender, EventArgs e)
		{
			// ���b�Z�[�W���X�g�\��
			//frmMessageViewer.Instance.Show();
			//frmMessageViewer.Instance.Activate();
			//MessageList.Forms.frmMessageViewer f = new Gekko.MessageList.Forms.frmMessageViewer();
			//f.Show();

			Process.Start(Path.Combine(Application.StartupPath, "MessageList.exe"));
		}

		private void logBox_MouseDown(object sender, MouseEventArgs e)
		{
			// ���O�{�b�N�X�Ń}�E�X��������
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
				// �L�����b�g����ԉ��ɂ���
				this.logBox.SelectionStart = this.logBox.TextLength;
				this.logBox.ScrollToCaret();
			}
		}
	}
}