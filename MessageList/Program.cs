using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using Gekko.MessageList.Forms;
using System.Threading;

namespace Gekko.MessageList
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// 多重起動できないようにする
			bool created;
			Mutex mu = new Mutex(true, "MessageList", out created);
			if (!created)
				return;

			Application.Run(new frmMessageViewer());
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

						return (balloon == 1) ? true : false;
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
