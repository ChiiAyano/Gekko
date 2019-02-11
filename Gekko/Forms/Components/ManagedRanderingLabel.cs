using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Gekko.Libraries;
using System.ComponentModel;

namespace Gekko.Forms.Components
{
	public class ExtendedPanel : Panel
	{
		string _text = string.Empty;
		Size _size = new Size(100, 100);
		Point _location = new Point(57, 37);
		int _glow = 12;
		Image _image = null;
		Rectangle _rect = new Rectangle(3, 37, 48, 48);
		Font _font = new Font("メイリオ", 9, FontStyle.Regular);

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			switch (m.Msg)
			{
				case 0xf:
					OperatingSystem os = Environment.OSVersion;
					if (os.Platform == PlatformID.Win32NT)
					{
						if (os.Version.Major >= 6)
						{
							if (DwmApi.DwmIsCompositionEnabled())
							{
								if (_image != null)
									this.CreateGraphics().DrawImage(_image, _rect);

								GlassRenderer.DrawText(base.Handle, _text, _font,
								    new Rectangle(_location, _size), _glow);
							}
						}
					}
					break;
				case 0x14:
					m.Result = IntPtr.Zero;
					break;
			}
		}

		/// <summary>
		/// Aero Glass が有効の場合の、表示するテキストを取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効の場合の、表示するテキストです。")]
		public string DrawText
		{
			get { return _text; }
			set { _text = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、表示するテキストの左上からの座標を取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、表示するテキストの左上からの座標です。")]
		public Point TextLocation
		{
			get { return _location; }
			set { _location = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、表示するテキストの描画範囲を取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、表示するテキストの描画範囲です。")]
		public Size TextSize
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、表示するテキストの周りに描画されるグローの大きさを取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、表示するテキストの周りに描画されるグローの大きさです。")]
		public int GlowSize
		{
			get { return _glow; }
			set { _glow = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、描画する画像を取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、描画する画像です。")]
		public Image DrawImage
		{
			get { return _image; }
			set { _image = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、描画する画像の位置と大きさを取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、描画する画像の位置と大きさです。")]
		public Rectangle DrawImageRectangle
		{
			get { return _rect; }
			set { _rect = value; }
		}

		/// <summary>
		/// Aero Glass が有効な場合の、表示するテキストのフォントを取得または設定します。
		/// </summary>
		[Category("Appearance")]
		[Description("Aero Glass が有効な場合の、表示するテキストのフォントです。")]
		public Font TextFont
		{
			get { return _font; }
			set { _font = value; }
		}
	}
}
