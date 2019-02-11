using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using MessageList.Properties;

namespace Gekko.MessageList.Libraries
{
	/// <summary>
	/// 設定データ
	/// </summary>
	[XmlType(Namespace = "Gekko", TypeName = "Config")]
	public class ConfigData
	{
		// アカウント
		private List<Account> c_accounts = new List<Account>();
		// 動作設定
		private int c_mailCheckInterval = 120000;
		private int c_powerResumedDelayTime = 20000;
		private bool c_useDefaultBrowser = true;
		private string c_useBrowserPath = string.Empty;
		private bool c_newMailBalloonOrPopupNotify = true;
		private int c_newMailNotifyOperation = Program.EnableBalloon ? 0 : 1;
		private int c_balloonClickOperation = 0;
		private string c_runApplicationPath = string.Empty;
		private string c_runApplicationOption = string.Empty;
		private bool c_useOldTypeMailList = false;
		private int c_maxMailList = 20;
		private int c_mailListClickOperation = 0;
		private bool c_playNewMailSound = true;
		private bool c_playDefaultNewMailSound = true;
		private string c_playNewMailSoundPath = string.Empty;
		private int c_notifyDoubleClickOperation = 0;
		private bool c_versionCheck = true;
		private bool c_useProxy = false;
		private bool c_useIeProxy = true;
		private string c_proxyHostName = string.Empty;
		private int c_proxyPort = 8080;
		private bool c_noSsl = false;
		private bool c_hideConsole = false;
		// 表示設定
		private string c_messageListFontName = Resources.SettingsMessageListDefaultFont;
		private float c_messageListFontSize = 12;
		private bool c_messageListAntialias = true;
		private int c_messageTitleColor = Color.Black.ToArgb();
		private int c_messageSenderColor = Color.Black.ToArgb();
		private int c_messageSummaryColor = Color.Black.ToArgb();
		private int c_messageNonSelectOddColor = Color.FromArgb(234, 234, 234).ToArgb();
		private int c_messageNonSelectColor = SystemColors.Window.ToArgb();
		private int c_messageSelectColor = SystemColors.Highlight.ToArgb();
		private float c_messageListSenderFontSize = 2;
		private int c_messageListLeftWidth = 200;
		private int c_popupViewTime = 10000;
		private string c_notificationRunExecutiveFilePath = string.Empty;
		// その他設定
		private bool c_messageListTopMost = true;
		private Rectangle c_messageListRectangle
			= new Rectangle(Screen.PrimaryScreen.WorkingArea.Width - 786,
				Screen.PrimaryScreen.WorkingArea.Height - 326,
				786, 326);
		private bool c_messageListInvisibleViewer = false;
		private bool c_messageListRightList = false;
		// メインコンソール
		private string c_mainConsoleFontName = SystemFonts.DefaultFont.Name;
		private float c_mainConsoleFontSize = SystemFonts.DefaultFont.Size;

		// プロパティ
		/// <summary>
		/// アカウント設定のリスト
		/// </summary>
		public List<Account> Accounts
		{
			get { return c_accounts; }
			set { c_accounts = value; }
		}

		/// <summary>
		/// メールチェック間隔（単位: ミリ秒）
		/// </summary>
		public int MailCheckInterval
		{
			get { return c_mailCheckInterval; }
			set { c_mailCheckInterval = value; }
		}

		/// <summary>
		/// 起動時・電源状態復帰後のメールチェック開始時間（単位: ミリ秒）
		/// </summary>
		public int PowerResumedDelayTime
		{
			get { return c_powerResumedDelayTime; }
			set { c_powerResumedDelayTime = value; }
		}

		/// <summary>
		/// デフォルトのブラウザを利用するか
		/// </summary>
		public bool UseDefaultBrowser
		{
			get { return c_useDefaultBrowser; }
			set { c_useDefaultBrowser = value; }
		}

		/// <summary>
		/// 使用するブラウザのパス
		/// </summary>
		public string UseBrowserPath
		{
			get { return c_useBrowserPath; }
			set { c_useBrowserPath = value; }
		}

		/// <summary>
		/// 通知にバルーンかポップアップを使用する
		/// </summary>
		public bool NewMailBalloonOrPopupNotify
		{
			get { return c_newMailBalloonOrPopupNotify; }
			set { c_newMailBalloonOrPopupNotify = value; }
		}

		/// <summary>
		/// 通知にバルーンを使用するか
		/// </summary>
		public NewMailNotification NewMailNotifyOperation
		{
			get { return (NewMailNotification)c_newMailNotifyOperation; }
			set { c_newMailNotifyOperation = (int)value; }
		}

		/// <summary>
		/// バルーンクリック時の動作
		/// </summary>
		public BalloonClick BalloonClickOperation
		{
			get { return (BalloonClick)c_balloonClickOperation; }
			set { c_balloonClickOperation = (int)value; }
		}

		/// <summary>
		/// ポップアップ通知の表示時間
		/// </summary>
		public int PopupViewTime
		{
			get { return c_popupViewTime; }
			set { c_popupViewTime = value; }
		}

		/// <summary>
		/// バルーンでエラー内容を表示しない
		/// </summary>
		public bool BalloonInvisibleError
		{
			get;
			set;
		}

		/// <summary>
		/// 通知をクリックした時に実行するアプリケーションのパス
		/// </summary>
		public string NotificationRunExecutiveFilePath
		{
			get { return c_notificationRunExecutiveFilePath; }
			set { c_notificationRunExecutiveFilePath = value; }
		}

		/// <summary>
		/// 実行するアプリケーションのパス
		/// </summary>
		public string RunApplicationPath
		{
			get { return c_runApplicationPath; }
			set { c_runApplicationPath = value; }
		}

		/// <summary>
		/// 実行するアプリケーションのオプション
		/// </summary>
		public string RunApplicationOption
		{
			get { return c_runApplicationOption; }
			set { c_runApplicationOption = value; }
		}

		/// <summary>
		/// 旧来のメッセージ一覧で表示するか
		/// </summary>
		public bool UseOldMessageList
		{
			get { return c_useOldTypeMailList; }
			set { c_useOldTypeMailList = value; }
		}

		/// <summary>
		/// メッセージ一覧の最大項目数
		/// </summary>
		public int MessageListMaxCount
		{
			get { return c_maxMailList; }
			set { c_maxMailList = value; }
		}

		/// <summary>
		/// メッセージ一覧の項目をクリックしたときの動作
		/// </summary>
		public MessageListClick MessageListClickOperation
		{
			get { return (MessageListClick)c_mailListClickOperation; }
			set { c_mailListClickOperation = (int)value; }
		}

		/// <summary>
		/// 新着受信時にサウンドを再生する
		/// </summary>
		public bool PlayNewMailSound
		{
			get { return c_playNewMailSound; }
			set { c_playNewMailSound = value; }
		}

		/// <summary>
		/// 新着受信時の再生サウンドを既定のサウンドにする
		/// </summary>
		public bool PlayDefaultNewMailSound
		{
			get { return c_playDefaultNewMailSound; }
			set { c_playDefaultNewMailSound = value; }
		}

		/// <summary>
		/// 新着受信時サウンドのパス
		/// </summary>
		public string PlayNewMailSoundPath
		{
			get { return c_playNewMailSoundPath; }
			set { c_playNewMailSoundPath = value; }
		}

		/// <summary>
		/// 通知領域をダブルクリックしたときの動作
		/// </summary>
		public NotifyDoubleClick NotifyDoubleClickOperation
		{
			get { return (NotifyDoubleClick)c_notifyDoubleClickOperation; }
			set { c_notifyDoubleClickOperation = (int)value; }
		}

		/// <summary>
		/// 起動時にバージョンチェックをおこなう
		/// </summary>
		public bool VersionCheckAtRun
		{
			get { return c_versionCheck; }
			set { c_versionCheck = value; }
		}

		/// <summary>
		/// プロキシを使用するか
		/// </summary>
		public bool UseProxy
		{
			get { return c_useProxy; }
			set { c_useProxy = value; }
		}

		/// <summary>
		/// IEのプロキシ設定を使用するか
		/// </summary>
		public bool UseIeProxy
		{
			get { return c_useIeProxy; }
			set { c_useIeProxy = value; }
		}

		/// <summary>
		/// プロキシのホスト名
		/// </summary>
		public string ProxyHostName
		{
			get { return c_proxyHostName; }
			set { c_proxyHostName = value; }
		}

		/// <summary>
		/// プロキシのポート番号
		/// </summary>
		public int ProxyPort
		{
			get { return c_proxyPort; }
			set { c_proxyPort = value; }
		}

		/// <summary>
		/// 接続にSSLを使用しない
		/// </summary>
		public bool NoUseSsl
		{
			get { return c_noSsl; }
			set { c_noSsl = value; }
		}

		/// <summary>
		/// メインコンソールで使用するフォント
		/// </summary>
		[XmlIgnore()]
		public Font MainConsoleFont
		{
			get { return new Font(c_mainConsoleFontName, c_mainConsoleFontSize); }
			set
			{
				c_mainConsoleFontName = value.Name;
				c_mainConsoleFontSize = value.Size;
			}
		}

		public string MainConsoleFontName
		{
			get { return c_mainConsoleFontName; }
			set { c_mainConsoleFontName = value; }
		}

		public float MainConsoleFontSize
		{
			get { return c_mainConsoleFontSize; }
			set { c_mainConsoleFontSize = value; }
		}

		/// <summary>
		/// メッセージリストで使用するフォント
		/// </summary>
		[XmlIgnore()]
		public Font MessageListFont
		{
			get { return new Font(c_messageListFontName, c_messageListFontSize); }
			set
			{
				c_messageListFontName = value.Name;
				c_messageListFontSize = value.Size;
			}
		}

		public string MessageFontName
		{
			get { return c_messageListFontName; }
			set
			{ c_messageListFontName = value; }
		}

		public float MessageFontSize
		{
			get { return c_messageListFontSize; }
			set
			{ c_messageListFontSize = value; }
		}

		/// <summary>
		/// メッセージリストでアンチエイリアスをかけるか
		/// </summary>
		public bool MessageListAntialias
		{
			get { return c_messageListAntialias; }
			set { c_messageListAntialias = value; }
		}

		/// <summary>
		/// メッセージリストのタイトル文字色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListTitleColor
		{
			get { return Color.FromArgb(MessageListTitleColorString); }
			set { MessageListTitleColorString = value.ToArgb(); }
		}

		[XmlElement("MessageListTitleColor")]
		public int MessageListTitleColorString
		{
			get { return c_messageTitleColor; }
			set { c_messageTitleColor = value; }
		}

		/// <summary>
		/// メッセージリストの送信者文字色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListSenderColor
		{
			get { return Color.FromArgb(c_messageSenderColor); }
			set { c_messageSenderColor = value.ToArgb(); }
		}

		[XmlElement("MessageListSenderColor")]
		public int MessageListSenderColorString
		{
			get { return c_messageSenderColor; }
			set { c_messageSenderColor = value; }
		}

		/// <summary>
		/// メッセージリストの概要文字色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListSummaryColor
		{
			get { return Color.FromArgb(c_messageSummaryColor); }
			set { c_messageSummaryColor = value.ToArgb(); }
		}

		[XmlElement("MessageListSummaryColor")]
		public int MessageListSummaryColorString
		{
			get { return c_messageSummaryColor; }
			set { c_messageSummaryColor = value; }
		}

		/// <summary>
		/// メッセージリストの非選択偶数背景色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListNonSelectColor
		{
			get { return Color.FromArgb(c_messageNonSelectColor); }
			set { c_messageNonSelectColor = value.ToArgb(); }
		}

		[XmlElement("MessageListNonSelectColor")]
		public int MessageListNonSelectColorString
		{
			get { return c_messageNonSelectColor; }
			set { c_messageNonSelectColor = value; }
		}

		/// <summary>
		/// メッセージリストの非選択奇数背景色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListNonSelectOddColor
		{
			get { return Color.FromArgb(c_messageNonSelectOddColor); }
			set { c_messageNonSelectOddColor = value.ToArgb(); }
		}

		[XmlElement("MessageListNonSelectOddColor")]
		public int MessageListNonSelectOddColorString
		{
			get { return c_messageNonSelectOddColor; }
			set { c_messageNonSelectOddColor = value; }
		}

		/// <summary>
		/// メッセージリストの選択背景色
		/// </summary>
		[XmlIgnore()]
		public Color MessageListSelectColor
		{
			get { return Color.FromArgb(c_messageSelectColor); }
			set { c_messageSelectColor = value.ToArgb(); }
		}

		[XmlElement("MessageListSelectColor")]
		public int MessageListSelectColorString
		{
			get { return c_messageSelectColor; }
			set { c_messageSelectColor = value; }
		}

		/// <summary>
		/// メッセージ一覧を手前表示にするか
		/// </summary>
		public bool MessageListTopMost
		{
			get { return c_messageListTopMost; }
			set { c_messageListTopMost = value; }
		}

		/// <summary>
		/// メッセージ一覧の表示位置と大きさ
		/// </summary>
		public Rectangle MessageBoxPositionSize
		{
			get { return c_messageListRectangle; }
			set { c_messageListRectangle = value; }
		}

		/// <summary>
		/// メッセージ一覧のビューアを非表示にするか
		/// </summary>
		public bool MessageListInvisibleViewer
		{
			get { return c_messageListInvisibleViewer; }
			set { c_messageListInvisibleViewer = value; }
		}

		/// <summary>
		/// メッセージ一覧の、件名フォントサイズと送信者、概要のフォントサイズとの差
		/// </summary>
		public float MessageListSenderFontSize
		{
			get { return c_messageListSenderFontSize; }
			set { c_messageListSenderFontSize = value; }
		}

		/// <summary>
		/// メッセージ一覧のリストを右側に表示するか
		/// </summary>
		public bool MessageListRightList
		{
			get { return c_messageListRightList; }
			set { c_messageListRightList = value; }
		}

		/// <summary>
		/// メッセージ一覧の左側の幅
		/// </summary>
		public int MessageListLeftWidth
		{
			get { return c_messageListLeftWidth; }
			set { c_messageListLeftWidth = value; }
		}

		/// <summary>
		/// メッセージ一覧の不透明度
		/// </summary>
		public double MessageListWindowOpacity
		{
			get;
			set;
		}

		/// <summary>
		/// 起動時にメインコンソールを非表示にしておくか
		/// </summary>
		public bool HideConsole
		{
			get { return c_hideConsole; }
			set { c_hideConsole = value; }
		}

		// 列挙型

		/// <summary>
		/// 新着の通知方法
		/// </summary>
		public enum NewMailNotification
		{
			/// <summary>
			/// バルーン
			/// </summary>
			Balloon,
			/// <summary>
			/// ポップアップ
			/// </summary>
			Popup
		}

		/// <summary>
		/// バルーンをクリックした時の動作
		/// </summary>
		public enum BalloonClick
		{
			/// <summary>
			/// メッセージ一覧を表示する
			/// </summary>
			MessageList,
			/// <summary>
			/// 受信トレイをブラウザで表示する
			/// </summary>
			RunBrowser,
			/// <summary>
			/// なにもしない
			/// </summary>
			None,
			/// <summary>
			/// アプリケーションを実行する
			/// </summary>
			RunExecutive
		}

		/// <summary>
		/// メッセージ一覧の項目をクリックした時の動作
		/// </summary>
		public enum MessageListClick
		{
			/// <summary>
			/// ビューアを立ち上げる
			/// </summary>
			Viewer,
			/// <summary>
			/// ブラウザを立ち上げる
			/// </summary>
			RunBrowser
		}

		/// <summary>
		/// 通知領域をダブルクリックしたときの動作
		/// </summary>
		public enum NotifyDoubleClick
		{
			/// <summary>
			/// メインウィンドウを表示
			/// </summary>
			ShowMainWindow,
			/// <summary>
			/// 受信トレイを開く
			/// </summary>
			ShowInbox,
			/// <summary>
			/// メールチェック
			/// </summary>
			MailCheck,
			/// <summary>
			/// メッセージ一覧の表示
			/// </summary>
			ShowMessageList
		}
	}

	/// <summary>
	/// アカウントの設定
	/// </summary>
	public class Account
	{
		private string a_accountSetName = string.Empty;
		private string a_account = string.Empty;
		private string a_domain = "gmail.com";
		private string a_password = string.Empty;
		private bool a_use = true;

		// プロパティ
		/// <summary>
		/// アカウントの設定名
		/// </summary>
		public string SetName
		{
			get { return a_accountSetName; }
			set { a_accountSetName = value; }
		}

		/// <summary>
		/// アカウント名（「@」の左側）
		/// </summary>
		public string AccountName
		{
			get { return a_account; }
			set { a_account = value; }
		}

		/// <summary>
		/// ドメイン名（「@」の右側）
		/// </summary>
		public string Domain
		{
			get { return a_domain; }
			set { a_domain = value; }
		}

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password
		{
			get { return a_password; }
			set { a_password = value; }
		}

		/// <summary>
		/// このアカウントを使うか
		/// </summary>
		public bool Use
		{
			get { return a_use; }
			set { a_use = value; }
		}

		/// <summary>
		/// アカウントの受信トレイのURIを取得
		/// </summary>
		[XmlIgnore()]
		public string GetInboxUri
		{
			get
			{
				string baseUri = "mail.google.com/";
				if (Domain != "gmail.com")
					baseUri = baseUri.Insert(baseUri.Length, "a/" + Domain);
				return baseUri;
			}
		}

		/// <summary>
		/// アカウントとドメインを組み合わせたアドレスを取得
		/// </summary>
		[XmlIgnore()]
		public string Address
		{
			get
			{
				return a_account + "@" + a_domain;
			}
		}
	}
}
