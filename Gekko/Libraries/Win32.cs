using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Gekko.Libraries
{
    //class to manage all our p/invoke
    public class Win32
    {
        public enum BP_BUFFERFORMAT
        {
            BPBF_COMPATIBLEBITMAP,
            BPBF_DIB,
            BPBF_TOPDOWNDIB,
            BPBF_TOPDOWNMONODIB
        }

		public enum WINDOWTHEMEATTRIBUTETYPE
		{
			WTA_NONCLIENT = 1
		}

        public const int DTT_COMPOSITED = (int)(1UL << 13);
        public const int DTT_GLOWSIZE = (int)(1UL << 11);

        //Text format consts
        public const int DT_SINGLELINE = 0x00000020;
        public const int DT_CENTER = 0x00000001;
        public const int DT_VCENTER = 0x00000004;
        public const int DT_NOPREFIX = 0x00000800;

        //Const for BitBlt
        public const int SRCCOPY = 0x00CC0020;

        //Consts for CreateDIBSection
        public const int BI_RGB = 0;

        //color table in RGBs
        public const int DIB_RGB_COLORS = 0;

        //consts for the textbox
        public const int WM_PRINTCLIENT = 0x0318;
        public const int PRF_CLIENT = 4;
        public const int WM_PAINT = 0xf;

		public const uint WTNCA_NODRAWCAPTION =		0x00000001;
		public const uint WTNCA_NODRAWICON =		0x00000002;
		public const uint WTNCA_NOSYSMENU =			0x00000004;
		public const uint WTNCA_NOMIRRORHELP =		0x00000008;

        public struct MARGINS
        {
            public int m_Left;
            public int m_Right;
            public int m_Top;
            public int m_Bottom;
        };

        public struct POINTAPI
        {
            public int x;
            public int y;
        };
        

        public struct DTTOPTS
        {
            public uint dwSize;
            public uint dwFlags;
            public uint crText;
            public uint crBorder;
            public uint crShadow;
            public int iTextShadowType;
            public POINTAPI ptShadowOffset;
            public int iBorderSize;
            public int iFontPropId;
            public int iColorPropId;
            public int iStateId;
            public int fApplyOverlay;
            public int iGlowSize;
            public IntPtr pfnDrawTextCallback;
            public int lParam;
        };

        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        };

        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        };

        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public RGBQUAD bmiColors;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(System.Drawing.Rectangle rect)
            {
                left = rect.Left;
                top = rect.Top;
                right = rect.Right;
                bottom = rect.Bottom;
            }

            //for ease with casting from a rectangle
            public static implicit operator RECT(System.Drawing.Rectangle rc)
            {
                return new RECT(rc);
            }
        }

		[StructLayout(LayoutKind.Sequential)]
		public struct WTA_OPTIONS
		{
			public uint dwFlags;
			public uint dwMask;
		}

        //required p/invokes

        //UXTHEME
        [DllImport("uxtheme.dll")]
        public static extern IntPtr BeginBufferedPaint(IntPtr hdc, ref RECT prcTarget, BP_BUFFERFORMAT dwFormat, IntPtr pPaintParams, out IntPtr phdc);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr EndBufferedPaint(IntPtr hBufferedPaint, bool fUpdateTarget);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr BufferedPaintSetAlpha(IntPtr targetDC, IntPtr prcTarget, byte Alpha);

        [DllImport("uxtheme.dll")]
        public static extern void BufferedPaintInit();

        [DllImport("uxtheme.dll")]
        public static extern void BufferedPaintUnInit();

        [DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DTTOPTS pOptions);
        
        [DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DrawThemeText(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags1, int dwFlags2, ref RECT pRect);

		[DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPardId, int iStateId, ref RECT pRect, IntPtr pClipRect);

		[DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);

		[DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int SetWindowThemeAttribute(IntPtr hwnd, WINDOWTHEMEATTRIBUTETYPE eAttribute, ref WTA_OPTIONS pvAttribute, uint cbAttribute);

		[DllImport("uxtheme.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int SetWindowThemeNonClientAttributes(IntPtr hwnd, uint dwMask, uint dwAttributes);

        //USER32
        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, RECT lpRect, bool bErase);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hdc);
        
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int SaveDC(IntPtr hdc);
                
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hdc, int state);
        
        //GDI
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);
        
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, int ppvBits, IntPtr hSection, uint dwOffset);

        //DWM
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

    }
}
