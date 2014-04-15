/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BotSuite
{
    /// <summary>
    ///     Class for simulating mouse actions
    /// </summary>
    public class Mouse
    {
        /// <summary>
        ///     Performs a left click
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.LeftClick();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void LeftClick()
        {
            LeftDown();
            Utility.Delay(10, 30);
            LeftUp();
        }

        /// <summary>
        ///     Simulates a left click at a specific point through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void LeftClick(IntPtr handle, Point point)
        {
            LeftDown(handle, point);
            Utility.Delay(10, 30);
            LeftUp(handle, point);
        }

        /// <summary>
        ///     Performs a left button down
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.LeftDown();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void LeftDown()
        {
            NativeMethods.mouse_event((Int32) (NativeMethods.MouseEventFlags.LEFTDOWN), 0, 0, 0, new IntPtr(0));
        }

        private static IntPtr GetLParam(Point point)
        {
            return (IntPtr) ((point.Y << 16) | point.X);
        }

        /// <summary>
        ///     Simulates a left button down through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void LeftDown(IntPtr handle, Point point)
        {
            IntPtr wParam = IntPtr.Zero;
            NativeMethods.SendMessage(handle, (uint) MouseEvents.WM_LBUTTONDOWN, wParam, GetLParam(point));
        }

        /// <summary>
        ///     Performs a left button up
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.LeftUp();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void LeftUp()
        {
            NativeMethods.mouse_event((Int32) (NativeMethods.MouseEventFlags.LEFTUP), 0, 0, 0, new IntPtr(0));
        }

        /// <summary>
        ///     Simulates a left button up through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void LeftUp(IntPtr handle, Point point)
        {
            IntPtr wParam = IntPtr.Zero;
            NativeMethods.SendMessage(handle, (uint) MouseEvents.WM_LBUTTONUP, wParam, GetLParam(point));
        }

        /// <summary>
        ///     Performs a right click
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.RightClick();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void RightClick()
        {
            RightDown();
            Utility.Delay(10, 30);
            RightUp();
        }

        /// <summary>
        ///     Simulates a right click at a specific point through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void RightClick(IntPtr handle, Point point)
        {
            RightUp(handle, point);
            Utility.Delay(10, 30);
            RightDown(handle, point);
        }

        /// <summary>
        ///     Performs a right button down
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.RightDown();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void RightDown()
        {
            NativeMethods.mouse_event((Int32) (NativeMethods.MouseEventFlags.RIGHTDOWN), 0, 0, 0, new IntPtr(0));
        }

        /// <summary>
        ///     Simulates a right button down through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void RightDown(IntPtr handle, Point point)
        {
            IntPtr wParam = IntPtr.Zero;
            NativeMethods.SendMessage(handle, (uint) MouseEvents.WM_RBUTTONDOWN, wParam, GetLParam(point));
        }

        /// <summary>
        ///     Performs a right button up
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.RightUp();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void RightUp()
        {
            NativeMethods.mouse_event((Int32) (NativeMethods.MouseEventFlags.RIGHTUP), 0, 0, 0, new IntPtr(0));
        }

        /// <summary>
        ///     Simulates a right button up through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void RightUp(IntPtr handle, Point point)
        {
            IntPtr wParam = IntPtr.Zero;
            NativeMethods.SendMessage(handle, (uint) MouseEvents.WM_RBUTTONUP, wParam, GetLParam(point));
        }

        /// <summary>
        ///     Performs a double-click (left button)
        /// </summary>
        public static void DoubleClick()
        {
            LeftClick();
            Utility.Delay(150);
            LeftClick();
        }

        /// <summary>
        ///     Simulates a double-click (left button) through the native methods
        /// </summary>
        /// <param name="handle">The handle of the window to click</param>
        /// <param name="point">The point to click</param>
        public static void DoubleClick(IntPtr handle, Point point)
        {
            LeftClick(handle, point);
            Utility.Delay(150);
            LeftClick(handle, point);
        }

        /// <summary>
        ///     Moves the mouse to a specific point
        /// </summary>
        /// <example>
        ///     <code>
        /// Point target = new Point(10,10);
        /// Mouse.Move(target,true,10);
        /// </code>
        /// </example>
        /// <param name="targetPosition">The target position</param>
        /// <param name="human">Simulate human-like jumps</param>
        /// <param name="steps">The points of pathpolygons</param>
        /// <returns></returns>
        public static Boolean Move(Point targetPosition, Boolean human = true, Int32 steps = 100)
        {
            if (!human)
            {
                Cursor.Position = targetPosition;
                return true;
            }
            Point start = GetPosition();
            PointF iterPoint = start;

            var slope = new PointF(targetPosition.X - start.X, targetPosition.Y - start.Y);

            slope.X = slope.X/steps;
            slope.Y = slope.Y/steps;

            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                Cursor.Position = Point.Round(iterPoint);
                Utility.Delay(5, 10);
            }

            // test if it works
            if ((Cursor.Position.X == targetPosition.X) && (Cursor.Position.Y == targetPosition.Y))
                return true;
            return false;
        }

        /// <summary>
        ///     Moves the mouse to the middle of a rectangle
        /// </summary>
        /// <example>
        ///     <code>
        /// Rectangle r = new Rectangle(50, 50, 100, 100);
        /// Mouse.Move(r,true,10);
        /// </code>
        /// </example>
        /// <param name="R">The rectangle to move to</param>
        /// <param name="human">Simulate human-like jumps</param>
        /// <param name="steps">The points of pathpolygons</param>
        /// <returns></returns>
        public static Boolean Move(Rectangle R, Boolean human = true, Int32 steps = 100)
        {
            return Move(new Point(Convert.ToInt32(R.Left + (R.Width/2)), Convert.ToInt32(R.Top + (R.Height/2))), human,
                steps);
        }

        /// <summary>
        ///     Performs a drag-and-drop action
        /// </summary>
        /// <param name="source">The drag point</param>
        /// <param name="target">The drop point</param>
        /// <param name="human">Simulate human-like jumps</param>
        /// <param name="steps">The points of pathpolygons</param>
        /// <returns></returns>
        public static void DragAndDrop(Point source, Point target, Boolean human = true, Int32 steps = 100)
        {
            Move(source, human, steps);
            LeftClick();
            if (human)
                Jiggle();
            Utility.Delay(10, 30);
            Move(target, human, steps);
            LeftClick();
        }

        /// <summary>
        ///     Returns the current position of the mouse
        /// </summary>
        /// <example>
        ///     <code>
        /// Point CurPos = Point.Empty;
        /// CurPos = Mouse.GetPosition();
        /// </code>
        /// </example>
        /// <returns>The mouse position</returns>
        public static Point GetPosition()
        {
            return Cursor.Position;
        }

        /// <summary>
        ///     Simulates a mouse jiggle
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.Jiggle();
        /// </code>
        /// </example>
        /// <returns></returns>
        public static void Jiggle()
        {
            Int32 xChange = Utility.Random(-10, 10);
            Int32 yChange = Utility.Random(-10, 10);

            Move(new Point(Cursor.Position.X + xChange, Cursor.Position.Y + yChange), true, 5);
            Utility.Delay(20, 60);
            Move(new Point(Cursor.Position.X - xChange, Cursor.Position.Y - yChange), true, 5);
        }

        /// <summary>
        ///     Moves the mouse relatively to a window
        /// </summary>
        /// <example>
        ///     <code>
        /// IntPtr hwnd = ... ;
        /// bool res = MoveRelativeToWindow(hwnd, 20, 35, true, 10);
        /// </code>
        /// </example>
        /// <param name="windowHandle">The handle of the window</param>
        /// <param name="point">The target point</param>
        /// <param name="human">Simulate human-like jumps</param>
        /// <param name="steps">The points of pathpolygons</param>
        /// <returns>true/false</returns>
        public static Boolean MoveRelativeToWindow(IntPtr windowHandle, Point point, Boolean human = true,
            Int32 steps = 100)
        {
            var WINDOW = new NativeMethods.RECT();

            if (!NativeMethods.GetWindowRect(windowHandle, out WINDOW))
                return false;

            if (!Move(new Point(WINDOW.Left + point.X, WINDOW.Top + point.Y), human, steps))
                return false;

            return true;
        }

        /// <summary>
        ///     Returns the current position of the mouse, relative to a window
        /// </summary>
        /// <example>
        ///     <code>
        /// Point CurPos = Point.Empty;
        /// IntPtr hwnd = ... ;
        /// CurPos = Mouse.GetPositionRelativeToControl(hwnd);
        /// </code>
        /// </example>
        /// <param name="controlHandle">The handle of the window</param>
        /// <returns>Point position</returns>
        public static Point GetPositionRelativeToControl(IntPtr controlHandle)
        {
            Point position = Cursor.Position;
            var WINDOW = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(controlHandle, out WINDOW);
            return new Point(position.X - WINDOW.Left, position.Y - WINDOW.Top);
        }

        /// <summary>
        ///     Tests if the mouse is hovering a window
        /// </summary>
        /// <example>
        ///     <code>
        /// bool OverTextbox = Mouse.HoverControl(Textbox1.Handle);
        /// </code>
        /// </example>
        /// <param name="controlHandle">The handle of the window</param>
        /// <returns>true/false</returns>
        public static bool HoverControl(IntPtr controlHandle)
        {
            Point mousePosition = GetPositionRelativeToControl(controlHandle);
            var WINDOW = new NativeMethods.RECT();
            NativeMethods.GetWindowRect(controlHandle, out WINDOW);
            return InRectangle(mousePosition, WINDOW.Top, WINDOW.Left, WINDOW.Bottom, WINDOW.Right);
        }

        /// <summary>
        ///     Returns whether the mouse is inside a rectangle or not
        /// </summary>
        /// <example>
        ///     <code>
        /// Point Pos = GetPosition(); // get position of the mouse
        /// bool InRectangle = Mouse.InRectangle(Pos,50, 10, 20, 70);
        /// </code>
        /// </example>
        /// <param name="point">The point to test</param>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="left">The left of the rectangle</param>
        /// <param name="bottom">The bottom of the rectangle</param>
        /// <param name="right">The right of the rectangle</param>
        /// <returns>inside=true / outside=false</returns>
        public static bool InRectangle(Point point, int top, int left, int bottom, int right)
        {
            return (
                (left < point.X) && (point.X < right) && (top < point.Y) && (point.Y < bottom)
                );
        }

        /// <summary>
        ///     Returns whether the mouse is inside a rectangle or not
        /// </summary>
        /// <example>
        ///     <code>
        /// bool InRectangle = Mouse.InRectangle(50, 10, 20, 70);
        /// </code>
        /// </example>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="left">The left of the rectangle</param>
        /// <param name="bottom">The bottom of the rectangle</param>
        /// <param name="right">The right of the rectangle</param>
        /// <returns>inside=true / outside=false</returns>
        public static bool InRectangle(int top, int left, int bottom, int right)
        {
            return InRectangle(GetPosition(), top, left, bottom, right);
        }

        /// <summary>
        ///     Performs a mouse scroll
        /// </summary>
        /// <example>
        ///     <code>
        /// Mouse.Scroll(-50);
        /// </code>
        /// </example>
        /// <param name="wheeldelta">Positive scrolls down, negative scrolls up</param>
        public static void Scroll(int wheeldelta)
        {
            NativeMethods.mouse_event((uint) NativeMethods.MouseEventFlags.WHEEL, 0, 0, -wheeldelta, IntPtr.Zero);
        }

        private enum MouseEvents : uint
        {
            WM_MOUSEFIRST = 0x200,
            WM_MOUSEMOVE = 0x200,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_LBUTTONDBLCLK = 0x203,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RBUTTONDBLCLK = 0x206,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MOUSEWHEEL = 0x20A,
            WM_MOUSEHWHEEL = 0x20E
        }
    }
}
