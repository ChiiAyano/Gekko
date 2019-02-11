using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MessageList.Properties;

namespace Gekko.MessageList.Libraries
{
	public static class BrowserShow
	{
		/// <summary>
		/// ブラウザで指定されたURIを開く
		/// </summary>
		/// <param name="uri">ブラウザで開くURI</param>
		public static void browserOpen(string uri)
		{
			try
			{
				if (ReadSetting.Setting.UseDefaultBrowser)
				{
					System.Diagnostics.Process.Start(uri);
				}
				else
				{
					if (string.IsNullOrEmpty(ReadSetting.Setting.UseBrowserPath))
					{
						System.Diagnostics.Process.Start(uri);
					}
					else
					{
						System.Diagnostics.Process.Start(ReadSetting.Setting.UseBrowserPath, uri);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Resources.BrowserRunError + "\n" + ex.Message, Resources.SoftwareName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
