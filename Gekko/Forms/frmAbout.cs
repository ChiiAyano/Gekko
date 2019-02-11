using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Gekko.Forms
{
	public partial class frmAbout : Form
	{
		[DllImport("User32.dll")]
		private extern static long SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
		[DllImport("User32.dll")]
		private extern static void ReleaseCapture();
		const int WM_NClBUTTONDOWN = 0xa1;
		const int HTCAPTION = 2;

		string version = Properties.Resources.SoftwareName + "\n\t" +
			"Version " + Application.ProductVersion + "\n\n" +
			"Copyright(C) 2009- Chii Ayano";

		// ガラス効果用変数群
		bool enableGlass = false;
		private DwmApi.MARGINS m_glassMargins;
		private Region m_blurRegion;


		#region ガラス効果
		// This is a trick to get Windows to treat glass as part of the caption
		// area I learned from Daniel Moth.
		protected override void WndProc(ref Message msg)
		{
			base.WndProc(ref msg); // let the normal winproc process it

			const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
			const int WM_NCHITTEST = 0x84;
			const int HTCLIENT = 0x01;

			switch (msg.Msg)
			{
				case WM_NCHITTEST:

					if (HTCLIENT == msg.Result.ToInt32())
					{
						// it's inside the client area

						// Parse the WM_NCHITTEST message parameters
						// get the mouse pointer coordinates (in screen coordinates)
						Point p = new Point();
						p.X = (msg.LParam.ToInt32() & 0xFFFF);// low order word
						p.Y = (msg.LParam.ToInt32() >> 16); // hi order word

						// convert screen coordinates to client area coordinates
						p = PointToClient(p);

						// if it's on glass, then convert it from an HTCLIENT
						// message to an HTCAPTION message and let Windows handle it from then on
						if (PointIsOnGlass(p))
							msg.Result = new IntPtr(2);
					}
					break;

				case WM_DWMCOMPOSITIONCHANGED:
					OperatingSystem os = Environment.OSVersion;
					if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6))
					{
						if (DwmApi.DwmIsCompositionEnabled())
						{
							m_glassMargins = null;
							if (m_blurRegion != null)
							{
								m_blurRegion.Dispose();
								m_blurRegion = null;
							}
							// ガラス効果が使える
							enableGlass = true;
						}
						// ガラス効果は使えない
						enableGlass = false;
					}
					break;

				case 0xf:
					break;
			}
		}

		private bool PointIsOnGlass(Point p)
		{
			// test for region or entire client area
			// or if upper window glass and inside it.
			// not perfect, but you get the idea
			return m_glassMargins != null &&
				(m_glassMargins.cyTopHeight <= 0 ||
				 m_glassMargins.cyTopHeight > p.Y);
		}

		private void ResetDwmBlurBehind()
		{
			OperatingSystem os = Environment.OSVersion;
			if (enableGlass)
			{
				DwmApi.DWM_BLURBEHIND bbhOff = new DwmApi.DWM_BLURBEHIND();
				bbhOff.dwFlags = DwmApi.DWM_BLURBEHIND.DWM_BB_ENABLE | DwmApi.DWM_BLURBEHIND.DWM_BB_BLURREGION;
				bbhOff.fEnable = false;
				bbhOff.hRegionBlur = IntPtr.Zero;
				DwmApi.DwmEnableBlurBehindWindow(this.Handle, bbhOff);
			}
		}

		#endregion

		public frmAbout()
		{
			InitializeComponent();

			// ガラス効果が使えるか確認
			OperatingSystem os = Environment.OSVersion;
			if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6) &&
				(DwmApi.DwmIsCompositionEnabled()))
			{
				// ガラス効果が使える
				enableGlass = true;
			}
			else { enableGlass = false; }
		}

		private void frmAbout_Load(object sender, EventArgs e)
		{
			// AeroGlassをかける
			if (enableGlass)
			{
				this.BackColor = Color.Black;
				panel1.BackColor = Color.Black;
				okButton.Font = new Font(Properties.Resources.AboutFont, okButton.Font.Size, okButton.Font.Style);
				label1.Text = string.Empty;
				pictureBox1.Visible = false;
				panel1.DrawText = version;
				panel1.TextFont = new Font(Properties.Resources.AboutFont, 9, FontStyle.Regular);
				panel1.TextLocation = new Point(55, 35);
				panel1.TextSize = new Size(panel1.Width - panel1.TextLocation.X, panel1.Height - panel1.TextLocation.Y);
				panel1.GlowSize = 10;
				panel1.DrawImage = icons.icon48;
				panel1.DrawImageRectangle = new Rectangle(3, 37, 48, 48);

				ResetDwmBlurBehind();

				m_glassMargins = new DwmApi.MARGINS(-1, 0, 0, 0);

				if (DwmApi.DwmIsCompositionEnabled()) DwmApi.DwmExtendFrameIntoClientArea(this.Handle, m_glassMargins);
				this.FormBorderStyle = FormBorderStyle.FixedDialog;

				this.Invalidate();
			}
			else
			{
				label1.Text = version;
				pictureBox1.Image = icons.icon48;
				this.BackColor = SystemColors.Control;
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void frmAbout_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WM_NClBUTTONDOWN, HTCAPTION, 0);

				//this.Left += e.X - mousePoint.X;
				//this.Top += e.Y - mousePoint.Y;
			}
		}

		private void frmAbout_MouseDown(object sender, MouseEventArgs e)
		{
			//if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			//{
			//    mousePoint = new Point(e.X, e.Y);
			//}
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			if (enableGlass)
			{

				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

				e.Graphics.FillRectangle(new SolidBrush(Color.Black), e.ClipRectangle);

				//GraphicsPath gp = new GraphicsPath();
				//gp.AddString(version, new FontFamily("メイリオ"), (int)label1.Font.Style, 12, label1.Location, new StringFormat());

				e.Graphics.DrawImage(icons.icon48, new Rectangle(pictureBox1.Location, icons.icon48.Size));
				//e.Graphics.FillPath(new SolidBrush(Color.Black), gp);

			}
		}

		private void frmAbout_Move(object sender, EventArgs e)
		{
			//panel1.Invalidate();
		}
	}
}
