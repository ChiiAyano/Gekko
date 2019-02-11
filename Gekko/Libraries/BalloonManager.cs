using System.Windows.Forms;

namespace Gekko.Libraries
{
	/// <summary>
	/// バルーンの表示、挙動を制御します。
	/// </summary>
	public static class BalloonManager
	{
		static NotifyIcon ni = new NotifyIcon();
		static BalloonClickAction bca = BalloonClickAction.Information;
		
		/// <summary>
		/// バルーン通知の種類
		/// </summary>
		public enum BalloonClickAction
		{
			/// <summary>
			/// 新着がある
			/// </summary>
			NewMail,
			/// <summary>
			/// お知らせがある
			/// </summary>
			Information,
			/// <summary>
			/// エラーがある
			/// </summary>
			Error,
			/// <summary>
			/// その他の場合
			/// </summary>
			Other
		}

		/// <summary>
		/// 表示する対象の System.Windows.Forms.NotifyIcon を取得または設定します。
		/// </summary>
		public static NotifyIcon NotifyIcon
		{
			get { return ni; }
			set { ni = value; }

		}

		/// <summary>
		/// バルーンクリック時の動作を取得または設定します。
		/// </summary>
		public static BalloonClickAction ClickAction
		{
			get { return bca; }
			set { bca = value; }
		}

		/// <summary>
		/// バルーンを表示します。
		/// </summary>
		/// <param name="message">表示する内容</param>
		/// <param name="title">バルーンのタイトル</param>
		/// <param name="showTime">表示時間（ミリ秒）</param>
		/// <param name="icon">アイコン表示</param>
		/// <returns>バルーンが使えるならばtrueを、使えないならばfalseを返します。</returns>
		public static bool Show(string message, string title, int showTime, ToolTipIcon icon)
		{
			if (ReadSetting.Setting.NewMailBalloonOrPopupNotify && (ReadSetting.Setting.NewMailNotifyOperation == ConfigData.NewMailNotification.Balloon)
				&& Program.EnableBalloon)
			{
				if (ni != null)
					ni.ShowBalloonTip(showTime, title, message, icon);

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
