// -----------------------------------------------------------------------
//  <copyright file="ScreenShot.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Windows.Forms;

	using BotSuite.Native.Methods;
	using BotSuite.Native.Structs;

	/// <summary>
	///     This class provide functions to create screenshots
	/// </summary>
	public class ScreenShot
	{
		/// <summary>
		///     create a screenshot of the primary screen
		/// </summary>
		/// <returns>bitmap of captured screen</returns>
		public static Bitmap Create()
		{
			return Create(0, 0, Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height);
		}

		/// <summary>
		///     creates a screenshot from a hidden window
		/// </summary>
		/// <param name="windowHandle">
		///     handle of window
		/// </param>
		/// <returns>
		///     The <see cref="Bitmap" />.
		/// </returns>
		public static Bitmap CreateFromHidden(IntPtr windowHandle)
		{
			Bitmap bmpScreen = null;
			try
			{
				Rectangle r;
				using (Graphics windowGraphic = Graphics.FromHdc(User32.GetWindowDC(windowHandle)))
				{
					r = Rectangle.Round(windowGraphic.VisibleClipBounds);
				}

				bmpScreen = new Bitmap(r.Width, r.Height);
				using (Graphics g = Graphics.FromImage(bmpScreen))
				{
					IntPtr hdc = g.GetHdc();
					try
					{
						User32.PrintWindow(windowHandle, hdc, 0);
					}
					finally
					{
						g.ReleaseHdc(hdc);
					}
				}
			}
			catch
			{
				if (bmpScreen != null)
				{
					bmpScreen.Dispose();
				}
			}

			return bmpScreen;
		}

		/// <summary>
		///     create a complete screenshot by using a rectangle
		/// </summary>
		/// <param name="left">
		///     The left.
		/// </param>
		/// <param name="top">
		///     The top.
		/// </param>
		/// <param name="width">
		///     The width.
		/// </param>
		/// <param name="height">
		///     The height.
		/// </param>
		/// <returns>
		///     bitmap of captured screen
		/// </returns>
		public static Bitmap Create(int left, int top, int width, int height)
		{
			Bitmap bmpScreen = null;
			Graphics g = null;
			try
			{
				bmpScreen = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				g = Graphics.FromImage(bmpScreen);
				g.CopyFromScreen(left, top, 0, 0, new Size(width, height));
			}
			catch (Exception)
			{
				if (bmpScreen != null)
				{
					bmpScreen.Dispose();
				}
			}
			finally
			{
				if (g != null)
				{
					g.Dispose();
				}
			}

			return bmpScreen;
		}

		/// <summary>
		///     create a complete screenshot by using a handle
		/// </summary>
		/// <param name="windowHandle">
		///     handle of window
		/// </param>
		/// <returns>
		///     captured screen
		/// </returns>
		public static Bitmap Create(IntPtr windowHandle)
		{
			Rect window;
			User32.GetWindowRect(windowHandle, out window);
			int winWidth = window.Right - window.Left;
			int winHeight = window.Bottom - window.Top;
			return Create(window.Left, window.Top, winWidth, winHeight);
		}

		/// <summary>
		///     create a screenshot relativ to control C in a rectangle Focus
		/// </summary>
		/// <param name="ctrl">
		///     relativ to this control
		/// </param>
		/// <param name="screenshotArea">
		///     screenshot area
		/// </param>
		/// <returns>
		///     The <see cref="Bitmap" />.
		/// </returns>
		public static Bitmap CreateRelativeToControl(Control ctrl, Rectangle screenshotArea)
		{
			Point leftTopP = ctrl.PointToScreen(new Point(screenshotArea.Left, screenshotArea.Top));
			Bitmap bmpScreen = Create(leftTopP.X + 1, leftTopP.Y + 1, screenshotArea.Width - 1, screenshotArea.Height - 1);
			return bmpScreen;
		}
	}
}