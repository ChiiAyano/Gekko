using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Drawing;

namespace Gekko.Libraries
{
	internal class DwmApi
	{
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmEnableBlurBehindWindow(
			IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(
			IntPtr hWnd, MARGINS pMargins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmEnableComposition(bool bEnable);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmGetColorizationColor(
			out int pcrColorization,
			[MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern IntPtr DwmRegisterThumbnail(
			IntPtr dest, IntPtr source);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmUpdateThumbnailProperties(
			IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmQueryThumbnailSourceSize(
			IntPtr hThumbnail, out Size size);

		[DllImport("UxTheme.dll", PreserveSig = false)]
		public static extern IntPtr BeginBufferedPaint(IntPtr hdc, ref RECT rect, int dwFormat, ref BP_PAINTPARAMS pPaintParams, ref IntPtr hdc2);

		[DllImport("UxTheme.dll", PreserveSig = false)]
		public static extern int EndBufferedPaint(IntPtr hBafferedPrint, bool fUpdateTarget);

		[DllImport("UxTheme.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DTTOPTS pOptions);


		[StructLayout(LayoutKind.Sequential)]
		public class DWM_THUMBNAIL_PROPERTIES
		{
			public uint dwFlags;
			public RECT rcDestination;
			public RECT rcSource;
			public byte opacity;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fVisible;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fSourceClientAreaOnly;
			public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
			public const uint DWM_TNP_RECTSOURCE = 0x00000002;
			public const uint DWM_TNP_OPACITY = 0x00000004;
			public const uint DWM_TNP_VISIBLE = 0x00000008;
			public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MARGINS
		{
			public int cxLeftWidth, cxRightWidth,
					   cyTopHeight, cyBottomHeight;

			public MARGINS(int left, int top, int right, int bottom)
			{
				cxLeftWidth = left; cyTopHeight = top;
				cxRightWidth = right; cyBottomHeight = bottom;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class DWM_BLURBEHIND
		{
			public uint dwFlags;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fEnable;
			public IntPtr hRegionBlur;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fTransitionOnMaximized;

			public const uint DWM_BB_ENABLE = 0x00000001;
			public const uint DWM_BB_BLURREGION = 0x00000002;
			public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left, top, right, bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left; this.top = top;
				this.right = right; this.bottom = bottom;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BP_PAINTPARAMS
		{
			public uint cbSize;
			public uint dwFlags;
			public RECT prcExclude;
			public BLENDFUNCTION pBlendFunction;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BLENDFUNCTION
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}

		[StructLayout(LayoutKind.Sequential)]
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
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTAPI
		{
			public int x;
			public int y;
		}
	}

}

