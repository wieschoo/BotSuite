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

/// <summary>
/// Namespace for BotSuite
/// </summary>
namespace BotSuite
{
    /// <summary>
    ///  This class provide functions to create screenshots
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
			Bitmap bmpScreen = null;
			Graphics g = null;
			try
			{
                bmpScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				g = Graphics.FromImage(bmpScreen);
				g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
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
        public System.Drawing.Bitmap CreateFromHidden(IntPtr WindowHandle)
        {
            Rectangle R = Rectangle.Empty;
            Graphics WindowGraphic = System.Drawing.Graphics.FromHdc(NativeMethods.GetWindowDC(WindowHandle));
            R = Rectangle.Round(WindowGraphic.VisibleClipBounds);
            Bitmap bmpScreen = new Bitmap(R.Width, R.Height);
            Graphics g = Graphics.FromImage(bmpScreen);
            IntPtr Hdc = g.GetHdc();
            try
            {
                NativeMethods.PrintWindow(WindowHandle, Hdc, (uint)0);
            }
            finally
            {
                g.ReleaseHdc(Hdc);
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
        /// <param name="WindowHandle">handle of window</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// IntPtr hwnd = ... ;
        /// Bitmap capture = ScreenShot.Create(hwnd);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="WindowHandle">handle of window</param>
        /// <returns>captured screen</returns>
        
		public static Bitmap Create(IntPtr WindowHandle)
		{
            NativeMethods.RECT WINDOW = new NativeMethods.RECT();
			NativeMethods.GetWindowRect(WindowHandle, out WINDOW);
			Int32 winWidth = WINDOW.Right - WINDOW.Left;
			Int32 winHeight = WINDOW.Bottom - WINDOW.Top;
			Size winSize = new Size(winWidth, winHeight);
			Bitmap bmpScreen = new Bitmap(winWidth, winHeight);
			Graphics graphic = Graphics.FromImage(bmpScreen);
			graphic.CopyFromScreen(WINDOW.Left, WINDOW.Top, 0, 0, winSize, CopyPixelOperation.SourceCopy);
			graphic.Dispose();
			return bmpScreen;
		}
        /// <summary>
        /// create a screenshot relativ to control C in a rectangle Focus
        /// </summary>
        /// <param name="C">relativ to this control</param>
        /// <param name="Focus">screenshot area</param>
        /// <returns></returns>
        public static Bitmap CreateRelativeToControl(Control C,Rectangle Focus)
        {
            Rectangle r = C.ClientRectangle;
            Point LeftTopP = C.PointToScreen(new Point(Focus.Left, Focus.Top));

            Bitmap B = Create(LeftTopP.X+1, LeftTopP.Y+1, Focus.Width-1, Focus.Height-1);
            return B;

        }
	}
}
