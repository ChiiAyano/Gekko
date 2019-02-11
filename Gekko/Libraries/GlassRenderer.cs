using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using BITMAPINFO = Gekko.Libraries.Win32.BITMAPINFO;
using BITMAPINFOHEADER = Gekko.Libraries.Win32.BITMAPINFOHEADER;
using DTTOPTS = Gekko.Libraries.Win32.DTTOPTS;
using RECT = Gekko.Libraries.Win32.RECT;

namespace Gekko.Libraries
{
    public class GlassRenderer
    {
        public static void DrawText(IntPtr hwnd, String text, Font font, Rectangle ctlrct, int iglowSize)
        {
            RECT rc = new RECT();
            RECT rc2 = new RECT();

            //make it larger to contain the glow effect
            rc.left = ctlrct.Left;
            rc.right = ctlrct.Right + 2 * iglowSize;
            rc.top = ctlrct.Top;
            rc.bottom = ctlrct.Bottom + 2 * iglowSize;

            //Just the same rect with rc,but (0,0) at the lefttop
			rc2.left = iglowSize;
			rc2.top = iglowSize;
            rc2.right = rc.right - rc.left;
            rc2.bottom = rc.bottom - rc.top;

            //hwnd must be the handle of form,not control
            IntPtr destdc = Win32.GetDC(hwnd);

            // Set up a memory DC where we'll draw the text.
            IntPtr Memdc = Win32.CreateCompatibleDC(destdc);
            IntPtr bitmap;
            IntPtr bitmapOld = IntPtr.Zero;
            IntPtr logfnotOld;

            int uFormat =  Win32.DT_VCENTER | Win32.DT_NOPREFIX;   //text format

            BITMAPINFO dib = new BITMAPINFO();
            dib.bmiHeader.biHeight = -(rc.bottom - rc.top);         // negative because DrawThemeTextEx() uses a top-down DIB
            dib.bmiHeader.biWidth = rc.right - rc.left;
            dib.bmiHeader.biPlanes = 1;
            dib.bmiHeader.biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            dib.bmiHeader.biBitCount = 32;
            dib.bmiHeader.biCompression = Win32.BI_RGB;
			dib.bmiColors = new Win32.RGBQUAD() { rgbRed = 100, rgbGreen = 100, rgbBlue = 100 };

			if (!(Win32.SaveDC(Memdc) == 0))
            {
                bitmap = Win32.CreateDIBSection(Memdc, ref dib, Win32.DIB_RGB_COLORS, 0, IntPtr.Zero, 0);   // Create a 32-bit bmp for use in offscreen drawing when glass is on
                if (!(bitmap == IntPtr.Zero))
                {
                    bitmapOld = Win32.SelectObject(Memdc, bitmap);
                    IntPtr hFont = font.ToHfont();
                    logfnotOld = Win32.SelectObject(Memdc, hFont);
                    try
                    {

                        System.Windows.Forms.VisualStyles.VisualStyleRenderer renderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);

                        DTTOPTS dttOpts = new DTTOPTS();

                        dttOpts.dwSize = (uint)Marshal.SizeOf(typeof(DTTOPTS));
						dttOpts.dwFlags = Win32.DTT_COMPOSITED | Win32.DTT_GLOWSIZE;
                        dttOpts.iGlowSize = iglowSize;

                        Win32.DrawThemeTextEx(renderer.Handle, Memdc, 0, 0, text, -1, uFormat, ref rc2, ref dttOpts);
                        Win32.BitBlt(destdc, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, Memdc, 0, 0, Win32.SRCCOPY);

                    }
                    catch (Exception)
                    {
                        
                    }

                    //Remember to clean up
                    Win32.SelectObject(Memdc, bitmapOld);
                    Win32.SelectObject(Memdc, logfnotOld);
                    Win32.DeleteObject(bitmap);
                    Win32.DeleteObject(hFont);

                    Win32.ReleaseDC(Memdc, -1);
                    Win32.DeleteDC(Memdc);

                }
            }
        }
    }


}

