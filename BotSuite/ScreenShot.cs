/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BotSuite
{
    /// <summary>
    ///     This class provides functions to create screenshots
    /// </summary>
    public class ScreenShot
    {
        /// <summary>
        ///     Creates a complete screenshot
        /// </summary>
        /// <example>
        ///     <code>
        /// Bitmap capture = ScreenShot.Create();
        /// </code>
        /// </example>
        /// <returns>Bitmap of captured screen</returns>
        public static Bitmap Create()
        {
            return Create(0, 0, Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height);
        }

        /// <summary>
        ///     Creates a screenshot from a hidden window
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// IntPtr hwnd = ... ;
        /// Bitmap capture = ScreenShot.CreateFromHidden(hwnd);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="windowHandle">Handle of window</param>
        /// <returns></returns>
        public static Bitmap CreateFromHidden(IntPtr windowHandle)
        {
            Bitmap bitmap = null;
            try
            {
                NativeMethods.RECT rectangle;
                NativeMethods.GetWindowRect(windowHandle, out rectangle);

                bitmap = new Bitmap(rectangle.Width, rectangle.Height);

                using (Graphics gfx = Graphics.FromImage(bitmap))
                {
                    IntPtr hdc = gfx.GetHdc();
                    NativeMethods.PrintWindow(windowHandle, hdc, 0);

                    gfx.ReleaseHdc(hdc);
                    gfx.Dispose();
                }
            }
            catch
            {
                if (bitmap != null)
                    bitmap.Dispose();
            }

            return bitmap;
        }

        /// <summary>
        ///     Creates a screenshot from a hidden window using a different method
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// IntPtr hwnd = ... ;
        /// Bitmap capture = ScreenShot.CreateFromHidden(hwnd);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="windowHandle">Handle of window</param>
        /// <returns></returns>
        public static Bitmap CreateFromHidden2(IntPtr windowHandle)
        {
            Bitmap bmpScreen = null;
            try
            {
                Rectangle r;
                using (Graphics windowGraphic = Graphics.FromHdc(NativeMethods.GetWindowDC(windowHandle)))
                    r = Rectangle.Round(windowGraphic.VisibleClipBounds);
                bmpScreen = new Bitmap(r.Width, r.Height);
                using (Graphics g = Graphics.FromImage(bmpScreen))
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        NativeMethods.PrintWindow(windowHandle, hdc, 0);
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
                    bmpScreen.Dispose();
            }
            return bmpScreen;
        }

        /// <summary>
        ///     Creates a complete screenshot using a rectangle
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// // capture upper left 10 x 10 px rectangle
        /// Bitmap capture = ScreenShot.Create(0,0,10,10);
        /// ]]>
        /// </code>
        /// </example>
        /// <returns>Bitmap of captured screen</returns>
        public static Bitmap Create(Int32 left, Int32 top, Int32 width, Int32 height)
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
                    bmpScreen.Dispose();
            }
            finally
            {
                if (g != null)
                    g.Dispose();
            }
            return bmpScreen;
        }

        /// <summary>
        ///     Creates a complete screenshot using a handle
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// IntPtr hwnd = ... ;
        /// Bitmap capture = ScreenShot.Create(hwnd);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="windowHandle">Handle of window</param>
        /// <returns>Captured screen</returns>
        public static Bitmap Create(IntPtr windowHandle)
        {
            NativeMethods.RECT window;
            NativeMethods.GetWindowRect(windowHandle, out window);
            Int32 winWidth = window.Right - window.Left;
            Int32 winHeight = window.Bottom - window.Top;
            return Create(window.Left, window.Top, winWidth, winHeight);
        }

        /// <summary>
        ///     Creates a screenshot relative to a control in a rectangle
        /// </summary>
        /// <param name="control">Relative control</param>
        /// <param name="screenshotArea">Screenshot area</param>
        /// <returns></returns>
        public static Bitmap CreateRelativeToControl(Control control, Rectangle screenshotArea)
        {
            Point leftTopP = control.PointToScreen(new Point(screenshotArea.Left, screenshotArea.Top));
            Bitmap bmpScreen = Create(leftTopP.X + 1, leftTopP.Y + 1, screenshotArea.Width - 1, screenshotArea.Height - 1);
            return bmpScreen;
        }
    }
}