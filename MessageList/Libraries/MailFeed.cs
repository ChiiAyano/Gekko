using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;


// TODO: 各プロパティにset入れる！
namespace Gekko.MessageList.Libraries
{
	/// <summary>
	/// Gmail ATOM フィード
	/// </summary>
	[XmlRoot(ElementName = "feed")]
	public class MailFeed
	{
		private string title = string.Empty;
		private string tagline = string.Empty;
		private LinkTag link = new LinkTag();
		[XmlElement("modified")]
		public string modified = string.Empty;
		private int fullcount = 0;
		[XmlElement]
		private List<MailEntry> entry = new List<MailEntry>();

		// プロパティ
		/// <summary>
		/// フィードのタイトル
		/// </summary>
		[XmlElement("title")]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		/// <summary>
		/// フィードの概要
		/// </summary>
		[XmlElement("tagline")]
		public string Tagline
		{
			get { return tagline; }
			set { tagline = value; }
		}

		/// <summary>
		/// Gmail へのリンク
		/// </summary>
		[XmlElement("link")]
		public LinkTag Link
		{
			get { return link; }
			set { link = value; }
		}

		/// <summary>
		/// フィードの更新日
		/// </summary>
		public DateTime Modified
		{
			get
			{
				DateTime dt = new DateTime();
				Match mt = Regex.Match(modified, @"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})Z");
				if (mt.Success)
				{
					// 時が24なら00に変更
					if(mt.Groups[4].Value == "24")
					{
						modified = mt.Groups[1].Value + "-" + mt.Groups[2].Value + "-" + mt.Groups[3].Value + "T" +
							"00:" + mt.Groups[5].Value + ":" + mt.Groups[6].Value + "Z";
					}
				}
				if (DateTime.TryParse(modified, out dt))
				{
					return dt;
				}
				else { return dt; }
			}
			set { modified = value.ToString("yyyy-MM-ddTHH:mm:ssZ"); }
		}

		/// <summary>
		/// 日付をATOMから拾うためのものです
		/// </summary>
		public string Modified_inner
		{
			get { return modified; }
			set { modified = value; }
		}

		/// <summary>
		/// 新着メッセージ数
		/// </summary>
		[XmlElement("fullcount")]
		public int FullCount
		{
			get { return fullcount; }
			set { fullcount = value; }
		}

		/// <summary>
		/// メッセージ一覧
		/// </summary>
		[XmlElement("entry")]
		public List<MailEntry> Entry
		{
			get { return entry; }
			set { entry = value; }
		}
	}


	/// <summary>
	/// リンク情報
	/// </summary>
	public class LinkTag
	{
		private string rel = string.Empty;
		private string href = string.Empty;
		private string type = string.Empty;

		// プロパティ
		/// <summary>
		/// リンクの関係
		/// </summary>
		[XmlAttribute("rel")]
		public string Relationship
		{
			get { return rel; }
			set { rel = value; }
		}

		/// <summary>
		/// メッセージの個別URI
		/// </summary>
		[XmlAttribute("href")]
		public string Uri
		{
			get { return href; }
			set { href = value; }
		}

		/// <summary>
		/// リンク先のコンテンツの種類
		/// </summary>
		[XmlAttribute("type")]
		public string LinkType
		{
			get { return type; }
			set { type = value; }
		}
	}
	/// <summary>
	/// 送信者情報
	/// </summary>
	public class Author
	{
		private string name = string.Empty;
		private string email = string.Empty;

		// プロパティ
		/// <summary>
		/// 送信者名
		/// </summary>
		[XmlElement("name")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// メールアドレス
		/// </summary>
		[XmlElement("email")]
		public string Email
		{
			get { return email; }
			set { email = value; }
		}
	}

	/// <summary>
	/// メールエントリ
	/// </summary>
	[XmlRoot(ElementName = "")]
	public class MailEntry
	{
		private string title = string.Empty;
		private string summary = string.Empty;
		private LinkTag link = new LinkTag();
		[XmlElement("modified")]
		public string modified = string.Empty;
		[XmlElement("issued")]
		public string issued = string.Empty;
		private string id = string.Empty;
		private Author author = new Author();
		private bool isOpened = false;

		// プロパティ
		/// <summary>
		/// タイトル
		/// </summary>
		[XmlElement("title")]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		/// <summary>
		/// メッセージの概要
		/// </summary>
		[XmlElement("summary")]
		public string Summary
		{
			get { return summary; }
			set { summary = value; }
		}
		/// <summary>
		/// メッセージの個別アドレス
		/// </summary>
		[XmlElement("link")]
		public LinkTag Link
		{
			get { return link; }
			set { link = value; }
		}

		/// <summary>
		/// メッセージの受信日
		/// </summary>
		public DateTime Modified
		{
			get
			{
				DateTime dt = new DateTime();
				Match mt = Regex.Match(modified, @"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})Z");
				if (mt.Success)
				{
					// 時が24なら00に変更
					if (mt.Groups[4].Value == "24")
					{
						modified = mt.Groups[1].Value + "-" + mt.Groups[2].Value + "-" + mt.Groups[3].Value + "T" +
							"00:" + mt.Groups[5].Value + ":" + mt.Groups[6].Value + "Z";
					}
				}
				if (DateTime.TryParse(modified, out dt))
				{
					return dt;
				}
				else { return dt; }
			}
			set { modified = value.ToString("yyyy-MM-ddTHH:mm:ssZ"); }
		}

		/// <summary>
		/// 日付をATOMから拾うためのものです
		/// </summary>
		public string Modified_inner
		{
			get { return modified; }
			set { modified = value; }
		}

		/// <summary>
		/// メッセージの送信日時
		/// </summary>
		public DateTime Issued
		{
			get
			{
				DateTime dt = new DateTime();
				Match mt = Regex.Match(issued, @"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})Z");
				if (mt.Success)
				{
					// 時が24なら日付繰り上げて00に変更
					if (mt.Groups[4].Value == "24")
					{
						issued = mt.Groups[1].Value + "-" + mt.Groups[2].Value + "-" + (int.Parse(mt.Groups[3].Value) + 1).ToString() + "T" +
							"00:" + mt.Groups[5].Value + ":" + mt.Groups[6].Value + "Z";
					}
				}
				if (DateTime.TryParse(modified, out dt))
				{
					return dt;
				}
				else { return dt; }
			}
			set { modified = value.ToString("yyyy-MM-ddTHH:mm:ssZ"); }
		}

		/// <summary>
		/// 日付をATOMから拾うためのものです
		/// </summary>
		public string Issued_inner
		{
			get { return issued; }
			set { issued = value; }
		}

		/// <summary>
		/// メッセージの送信者情報
		/// </summary>
		[XmlElement("author")]
		public Author Author
		{
			get { return author; }
			set { author = value; }
		}

		/// <summary>
		/// メールを開封したか
		/// </summary>
		[XmlIgnore()]
		public bool IsOpened
		{
			get { return isOpened; }
			set { isOpened = value; }
		}
	}
}
