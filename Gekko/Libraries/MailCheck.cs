using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Text;

namespace Gekko.Libraries
{
	/// <summary>
	/// Gmail のメールをチェックする
	/// </summary>
	public class MailCheck
	{
		string _user;
		string _mailAddress;
		string _pass;
		string _atom;

		/// <summary>
		/// Gmail のメールをチェックする
		/// </summary>
		/// <param name="user">ユーザ名</param>
		/// <param name="pass">パスワード（暗号化したまま）</param>
		/// <param name="domain">ドメイン名</param>
		public MailCheck(string user, string pass, string domain)
		{
			_user = user;
			_mailAddress = user + "@" + domain;
			_pass = pass;
			_atom = GetAtomUri(domain);
		}

		/// <summary>
		/// メールを取得する
		/// </summary>
		/// <returns></returns>
		public MailFeed Check()
		{
			return Check(_atom);
		}

		/// <summary>
		/// メールを取得する
		/// </summary>
		/// <param name="checkUri">チェックするATOMフィードのURI</param>
		/// <returns></returns>
        public MailFeed Check(string checkUri)
        {
            // メール取得準備
            try
            {
                HttpWebRequest hwreq = (HttpWebRequest)HttpWebRequest.Create(checkUri);
                // パスワードはこの時点で復号化
				hwreq.Credentials = new NetworkCredential(_mailAddress, Password.Decrypt(_pass, _user));
				// プロキシ
				if (ReadSetting.Setting.UseProxy)
				{
					if (ReadSetting.Setting.UseIeProxy)
						hwreq.Proxy = WebRequest.GetSystemWebProxy();
					else
						hwreq.Proxy = new WebProxy(ReadSetting.Setting.ProxyHostName, ReadSetting.Setting.ProxyPort);
				}
				else
				{
					hwreq.Proxy = null;
				}
                hwreq.Method = "GET";

                // 取得開始
                using (HttpWebResponse hwres = (HttpWebResponse)hwreq.GetResponse())
                {
                    return feedDeserialize(hwres.GetResponseStream());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// ドメインからATOMのURLを割り出す
		/// </summary>
		/// <param name="domain">ドメイン名</param>
		/// <returns></returns>
		public string GetAtomUri(string domain)
		{
			switch (domain)
			{
				case "gmail.com":
					return "https://mail.google.com/mail/feed/atom";
				case "livedoor.com":
					return "https://mail.google.com/a/livedoor.com/feed/atom";
				case "auone.jp":
					return "https://mail.google.com/a/auone.jp/feed/atom";
				default:
					return string.Format("https://mail.google.com/a/{0}/feed/atom", domain);
			}
		}

		/// <summary>
		/// メールフィードのデシリアライズ
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private MailFeed feedDeserialize(Stream str)
		{
			StringBuilder buff = new StringBuilder();
			try
			{
				using (StreamReader sr = new StreamReader(str))
				{
					// ストリームを読む
					//while (sr.Peek() > -1)
					//{
					//   // buff = buff.Insert(buff.Length, sr.ReadLine());
					//    buff.Append(sr.ReadLine());
					//}
					buff.Append(sr.ReadToEnd());
				}

				// XmlSerializerでも読めるようにヘッダを変更
				//if (buff.Contains("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">"))
				//{
				buff = buff.Replace(
					"<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">",
					"<feed xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
				//}

				// 読んだデータを処理する
				using (StringReader sr = new StringReader(buff.ToString()))
				{
					XmlSerializer xs = new XmlSerializer(typeof(MailFeed));
					return (MailFeed)xs.Deserialize(sr);

				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


	}
}
