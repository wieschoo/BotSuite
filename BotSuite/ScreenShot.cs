// -----------------------------------------------------------------------
//  <copyright file="ScreenShot.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Windows.Forms;
	using Win32.Methods;
	using Win32.Structs;

	/// <summary>
	///     This class provides functions to create screenshots
	/// </summary>
	public class ScreenShot
	{
		/// <summary>
		///     Create a screenshot of the primary screen.
		/// </summary>
		/// <returns>
		///		<see cref="Bitmap"/> of captured screen.
		///	</returns>
		public static Bitmap Create()
		{
			return ScreenShot.Create(Screen.PrimaryScreen.Bounds);
		}

		/// <summary>
		///     Creates a screenshot from a hidden window.
		/// </summary>
		/// <param name="windowHandle">
		///     The handle of the window.
		/// </param>
		/// <returns>
		///     A <see cref="Bitmap"/> of the window.
		/// </returns>
		public static Bitmap CreateFromHidden(IntPtr windowHandle)
		{
			Bitmap bmpScreen = null;
			try
			{
				Rectangle r;
				using(Graphics windowGraphic = Graphics.FromHdc(User32.GetWindowDC(windowHandle)))
				{
					r = Rectangle.Round(windowGraphic.VisibleClipBounds);
				}

				bmpScreen = new Bitmap(r.Width, r.Height);
				using(Graphics g = Graphics.FromImage(bmpScreen))
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
				if(bmpScreen != null)
				{
					bmpScreen.Dispose();
				}
			}

			return bmpScreen;
		}

		/// <summary>
		///     Create a complete screenshot by using a <see cref="Rectangle"/>
		/// </summary>
		/// <param name="screenshotArea">
		///		The <see cref="Rectangle"/> that defines the screenshot area.
		/// </param>
		/// <returns>
		///     A <see cref="Bitmap"/> of the captured area.
		/// </returns>
		public static Bitmap Create(Rectangle screenshotArea)
		{
			Bitmap bmpScreen = null;
			Graphics g = null;
			try
			{
				bmpScreen = new Bitmap(screenshotArea.Width, screenshotArea.Height, PixelFormat.Format24bppRgb);
				g = Graphics.FromImage(bmpScreen);
				g.CopyFromScreen(screenshotArea.Left, screenshotArea.Top, 0, 0, new Size(screenshotArea.Width, screenshotArea.Height));
			}
			catch(Exception)
			{
				if(bmpScreen != null)
				{
					bmpScreen.Dispose();
				}
			}
			finally
			{
				if(g != null)
				{
					g.Dispose();
				}
			}

			return bmpScreen;
		}

		/// <summary>
		///     Create a complete screenshot of a window by using a handle.
		/// </summary>
		/// <param name="windowHandle">
		///     Handle of the window.
		/// </param>
		/// <returns>
		///     A <see cref="Bitmap"/> of the window.
		/// </returns>
		public static Bitmap Create(IntPtr windowHandle)
		{
			Rect window;
			User32.GetWindowRect(windowHandle, out window);
			int winWidth = window.Right - window.Left;
			int winHeight = window.Bottom - window.Top;
			Rectangle windowRect = new Rectangle(window.Left, window.Top, winWidth, winHeight);
			Bitmap bmpScreen = ScreenShot.Create(windowRect);
			return bmpScreen;
		}

		/// <summary>
		///     Create a screenshot relativ to a <see cref="Control"/> in a rectangle focus.
		/// </summary>
		/// <param name="ctrl">
		///     Relativ to this <see cref="Control"/>.
		/// </param>
		/// <param name="screenshotArea">
		///     Screenshot area.
		/// </param>
		/// <returns>
		///     A <see cref="Bitmap"/> of the captured area.
		/// </returns>
		public static Bitmap CreateRelativeToControl(Control ctrl, Rectangle screenshotArea)
		{
			Point leftTopP = ctrl.PointToScreen(new Point(screenshotArea.Left, screenshotArea.Top));
			Rectangle r = new Rectangle(leftTopP.X + 1, leftTopP.Y + 1, screenshotArea.Width - 1, screenshotArea.Height - 1);
			Bitmap bmpScreen = ScreenShot.Create(r);
			return bmpScreen;
		}
	}
}