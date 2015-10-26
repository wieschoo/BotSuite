using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace BotSuite.Imaging
{
    public static class DrawingHelper
    {
        //Find more Pinvoke Functions on: http://pinvoke.net/default.aspx/user32/InvalidateRect.html

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

    }
}
