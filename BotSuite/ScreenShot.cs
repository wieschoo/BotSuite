/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BotSuite
{
	/// <summary>
	/// This class provide functions to create screenshots
	/// </summary>
	public class ScreenShot
	{
		/// <summary>
		/// create a complete screenshot
		/// </summary>
		/// <example>
		/// <code>
		/// Bitmap capture = ScreenShot.Create();
		/// </code>
		/// </example>
		/// <returns>bitmap of captured screen</returns>
		public static Bitmap Create()
		{
			return Create(0, 0, Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height);
		}

		/// <summary>
		/// creates a screenshot from a hidden window
		/// </summary>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// IntPtr hwnd = ... ;
		/// Bitmap capture = ScreenShot.CreateFromHidden(hwnd);
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="WindowHandle">handle of window</param>
		/// <returns></returns>
		public static Bitmap CreateFromHidden(IntPtr WindowHandle)
		{
			Bitmap bmpScreen = null;
			try
			{
				Rectangle R = Rectangle.Empty;
				using(Graphics WindowGraphic = System.Drawing.Graphics.FromHdc(NativeMethods.GetWindowDC(WindowHandle)))
					R = Rectangle.Round(WindowGraphic.VisibleClipBounds);
				bmpScreen = new Bitmap(R.Width, R.Height);
				using(Graphics g = Graphics.FromImage(bmpScreen))
				{
					IntPtr Hdc = g.GetHdc();
					try
					{
						NativeMethods.PrintWindow(WindowHandle, Hdc, (uint)0);
					}
					finally
					{
						g.ReleaseHdc(Hdc);
					}
				}
			}
			catch
			{
				if(bmpScreen != null)
					bmpScreen.Dispose();
			}
			return bmpScreen;
		}

		/// <summary>
		/// create a complete screenshot by using a rectangle 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// // capture upper left 10 x 10 px rectangle
		/// Bitmap capture = ScreenShot.Create(0,0,10,10);
		/// ]]>
		/// </code>
		/// </example>
		/// <returns>bitmap of captured screen</returns>
		public static Bitmap Create(Int32 left, Int32 top, Int32 width, Int32 height)
		{
			Bitmap bmpScreen = null;
			Graphics g = null;
			try
			{
				bmpScreen = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				g = Graphics.FromImage(bmpScreen);
				g.CopyFromScreen(left, top, 0, 0, new Size(width, height));
			}
			catch(Exception)
			{
				if(bmpScreen != null)
					bmpScreen.Dispose();
			}
			finally
			{
				if(g != null)
					g.Dispose();
			}
			return bmpScreen;
		}

		/// <summary>
		/// create a complete screenshot by using a handle
		/// </summary>
		/// <param name="windowHandle">handle of window</param>
		/// <example>
		/// <code>
		/// <![CDATA[
		/// IntPtr hwnd = ... ;
		/// Bitmap capture = ScreenShot.Create(hwnd);
		/// ]]>
		/// </code>
		/// </example>
		/// <param name="windowHandle">handle of window</param>
		/// <returns>captured screen</returns>
		public static Bitmap Create(IntPtr windowHandle)
		{
			NativeMethods.RECT WINDOW = new NativeMethods.RECT();
			NativeMethods.GetWindowRect(windowHandle, out WINDOW);
			Int32 winWidth = WINDOW.Right - WINDOW.Left;
			Int32 winHeight = WINDOW.Bottom - WINDOW.Top;
			return Create(WINDOW.Left, WINDOW.Top, winWidth, winHeight);
		}

		/// <summary>
		/// create a screenshot relativ to control C in a rectangle Focus
		/// </summary>
		/// <param name="ctrl">relativ to this control</param>
		/// <param name="screenshotArea">screenshot area</param>
		/// <returns></returns>
		public static Bitmap CreateRelativeToControl(Control ctrl, Rectangle screenshotArea)
		{
			Bitmap bmpScreen = null;
			Rectangle r = ctrl.ClientRectangle;
			Point LeftTopP = ctrl.PointToScreen(new Point(screenshotArea.Left, screenshotArea.Top));
			bmpScreen = Create(LeftTopP.X + 1, LeftTopP.Y + 1, screenshotArea.Width - 1, screenshotArea.Height - 1);
			return bmpScreen;
		}
	}
}