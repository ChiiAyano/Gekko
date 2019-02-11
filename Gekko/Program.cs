using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gekko.Forms;
using System.Threading;
using Microsoft.Win32;
using Gekko.Libraries;
using System.IO;

namespace Gekko
{
	static class Program
	{
		// 終了モード
		static bool shutdownMode = false;
		static object syncobj = new object();
		static DateTime releaseDate = new DateTime(2010, 7, 25);
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);


			//Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			//Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
			

			// テスト用
#if DEBUG
			if (args.Length == 0)
			{
#endif
				// 引数が何もなければ通常起動
				frmMain f = new frmMain();

				if (SystemShutdown)
				{
					Application.Exit();
					return;
				}

				// コンソールを表示するかどうか
				if (ReadSetting.Setting.HideConsole)
				{
					f.WindowState = FormWindowState.Minimized;
					f.Visible = false;
				}

				Application.Run(f);
#if DEBUG
			}
			else if (args[0] == "/maillist")
			{

			}
#endif
		}

		/// <summary>
		/// シャットダウンが通知されたかどうか
		/// </summary>
		public static bool SystemShutdown
		{
			get
			{
				return shutdownMode;
			}
			set
			{
				lock (syncobj)
				{
					shutdownMode = value;
				}
			}
		}

		/// <summary>
		/// このソフトウェアのリリース日付
		/// </summary>
		public static DateTime ReleaseDate
		{
			get
			{
				return releaseDate;
			}
		}

		/// <summary>
		/// バルーンを使うことができるか
		/// </summary>
		public static bool EnableBalloon
		{
			get
			{
				OperatingSystem os = Environment.OSVersion;
				if (os.Platform == PlatformID.Win32NT)
				{
					if (os.Version.Major >= 5)
					{
						// システムがバルーンを使うことができるか確認する
						RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
						int balloon = (int)reg.GetValue("EnableBalloonTips", (int)1);

						return (balloon == 1);
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}

			}
		}

		/// <summary>
		/// 使用しているOSがWindowsVista以上か
		/// </summary>
		public static bool OperatingSystemIsMoreThanWindowsVista
		{
			get
			{
				OperatingSystem os = Environment.OSVersion;
				if (os.Platform == PlatformID.Win32NT)
				{
					if (os.Version.Major >= 6)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}

		}

		/// <summary>
		/// ログを追記（デバッグモードのみ）
		/// </summary>
		/// <param name="msg"></param>
		public static void WriteLog(string msg)
		{
#if DEBUG
			// ログの書き出し
			using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, "log.txt"), true))
			{
				sw.WriteLine("{0}: {1}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), msg);
			}
#endif
		}
	}
}