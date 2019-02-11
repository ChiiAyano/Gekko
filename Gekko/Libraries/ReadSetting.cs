using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace Gekko.Libraries
{
	public static class ReadSetting
	{
		static XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
		static ConfigData cd = new ConfigData();
		static string _path= System.IO.Path.Combine(Application.StartupPath, "config.xml");

		/// <summary>
		/// 設定ファイルのパスを取得または設定
		/// </summary>
		public static string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		/// <summary>
		/// 設定ファイルを取得または設定
		/// </summary>
		public static ConfigData Setting
		{
			get
			{
				if (cd == null)
				{
					return Read();
				}
				else { return cd; }
			}
			set
			{
				cd = value;
			}
		}

		/// <summary>
		/// 設定ファイルを読み込む
		/// </summary>
		/// <returns></returns>
		public static ConfigData Read()
		{
			if (!File.Exists(_path))
			{
				cd = new ConfigData();
				return cd;	// 設定データがなければ初期化しておく
			}

			try
			{
				using (StreamReader sr = new StreamReader(_path))
				{
					cd = (ConfigData)xs.Deserialize(sr);
					return cd;
				}
			}
			catch 
			{
#if DEBUG
				Console.WriteLine("設定ファイルデータにミスがあります。");
#endif
				return new ConfigData();
			}
		}

		/// <summary>
		/// 設定データをセーブ
		/// </summary>
		public static void Save()
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(_path))
				{
					xs.Serialize(sw, (object)cd);
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				Console.WriteLine("書き込み中にエラー: {0}", ex.Message);
#endif
			}
		}

	}
}
