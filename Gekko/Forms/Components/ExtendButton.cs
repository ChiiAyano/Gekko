using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gekko.Libraries;
using System.Drawing;

namespace Gekko.Forms.Components
{
	class ExtendButton:Control
	{
		private enum ButtonState
		{
			Enable = 1,
			Over = 2,
			Down = 3,
			Disable = 4
		}

		private ButtonState status = ButtonState.Enable;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int st = this.Enabled ? (int)status : 4;

			if (Application.RenderWithVisualStyles)
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Win32.RECT r = new Win32.RECT { left = 0, top = 0, bottom = 32, right = 32 };
				System.Windows.Forms.VisualStyles.VisualStyleRenderer renderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.CreateElement("NAVIGATION", 1, 1));

				Win32.DrawThemeBackground(renderer.Handle, hdc, 1, st, ref r, IntPtr.Zero);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			status = ButtonState.Over;
			this.Refresh();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			status = ButtonState.Enable;
			this.Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			status = ButtonState.Down;
			this.Refresh();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Point mScreenPos = Control.MousePosition;
			Point mClientPos = this.PointToClient(mScreenPos);
			
			base.OnMouseUp(e);

			if (this.ClientRectangle.Contains(mClientPos))
				status = ButtonState.Over;
			else
				status = ButtonState.Enable;

			this.Refresh();
		}
	}
}
