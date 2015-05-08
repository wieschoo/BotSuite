// -----------------------------------------------------------------------
//  <copyright file="Mouse.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Input
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using Win32;
	using Win32.Methods;
	using Win32.Structs;

	/// <summary>
	///     class for simulate mouse actions like moving or clicking
	/// </summary>
	public class Mouse
	{
		/// <summary>
		///     causes a left-click (press and release)
		/// </summary>
		public static void LeftClick()
		{
			LeftDown();
			Utility.RandomDelay();
			LeftUp();
		}

		/// <summary>
		///     press down the left mouse button
		/// </summary>
		public static void LeftDown()
		{
			User32.mouse_event((int)Constants.MouseEventFlags.Leftdown, 0, 0, 0, 0);
		}

		/// <summary>
		///     release the left mouse button
		/// </summary>
		public static void LeftUp()
		{
			User32.mouse_event((int)Constants.MouseEventFlags.Leftup, 0, 0, 0, 0);
		}

		/// <summary>
		///     causes a right-click (press and release)
		/// </summary>
		public static void RightClick()
		{
			RightDown();
			Utility.RandomDelay();
			RightUp();
		}

		/// <summary>
		///     press down the right mouse button
		/// </summary>
		public static void RightDown()
		{
			User32.mouse_event((int)Constants.MouseEventFlags.Rightdown, 0, 0, 0, 0);
		}

		/// <summary>
		///     release the right mouse button
		/// </summary>
		public static void RightUp()
		{
			User32.mouse_event((int)Constants.MouseEventFlags.Rightup, 0, 0, 0, 0);
		}

		/// <summary>
		///     performs a double click
		/// </summary>
		public static void DoubleClick()
		{
			LeftClick();
			Utility.Delay(150);
			LeftClick();
		}

		/// <summary>
		///     causes a mouse movement to a given point
		/// </summary>
		/// <param name="targetPosition">
		///     target coordinate
		/// </param>
		/// <param name="human">
		///     prevent mouse jumps
		/// </param>
		/// <param name="steps">
		///     points of pathpolygons
		/// </param>
		/// <returns>
		///     the bool
		/// </returns>
		public static bool Move(Point targetPosition, bool human = true, int steps = 100)
		{
			if(!human)
			{
				Cursor.Position = targetPosition;
				return true;
			}

			Point start = GetPosition();
			PointF iterPoint = start;

			PointF slope = new PointF(targetPosition.X - start.X, targetPosition.Y - start.Y);

			slope.X = slope.X / steps;
			slope.Y = slope.Y / steps;

			for(int i = 0; i < steps; i++)
			{
				iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
				Cursor.Position = Point.Round(iterPoint);
				Utility.RandomDelay(5, 10);
			}

			// test if it works
			return (Cursor.Position.X == targetPosition.X) && (Cursor.Position.Y == targetPosition.Y);
		}

		/// <summary>
		///     causes a mouse movement to given coordinates
		/// </summary>
		/// <param name="targetX">
		///     x coordinate of target
		/// </param>
		/// <param name="targetY">
		///     y coordinate of target
		/// </param>
		/// <param name="human">
		///     prevent mouse jumps
		/// </param>
		/// <param name="steps">
		///     points of pathpolygons
		/// </param>
		/// <returns>
		///     the bool
		/// </returns>
		public static bool Move(int targetX, int targetY, bool human = true, int steps = 100)
		{
			return Move(new Point(targetX, targetY), human, steps);
		}

		/// <summary>
		///     causes a mouse movement into the middle of a rectangle
		/// </summary>
		/// <param name="r">
		///     the rectangle to move to
		/// </param>
		/// <param name="human">
		///     prevent mouse jumps
		/// </param>
		/// <param name="steps">
		///     points of pathpolygons
		/// </param>
		/// <returns>
		///     the bool
		/// </returns>
		public static bool Move(Rectangle r, bool human = true, int steps = 100)
		{
			return Move(
				new Point(Convert.ToInt32(r.Left + (r.Width / 2)), Convert.ToInt32(r.Top + (r.Height / 2))),
				human,
				steps);
		}

		/// <summary>
		///     causes a drag and drop
		/// </summary>
		/// <param name="source">
		///     drag point
		/// </param>
		/// <param name="target">
		///     drop point
		/// </param>
		/// <param name="human">
		///     prevent mouse jumps
		/// </param>
		/// <param name="steps">
		///     points of pathpolygons
		/// </param>
		public static void DragAndDrop(Point source, Point target, bool human = true, int steps = 100)
		{
			Move(source, human, steps);
			LeftClick();
			if(human)
			{
				Jiggle();
			}

			Utility.RandomDelay();
			Move(target, human, steps);
			LeftClick();
		}

		/// <summary>
		///     get the current position of the mouse pointer
		/// </summary>
		/// <returns>Point position</returns>
		public static Point GetPosition()
		{
			return Cursor.Position;
		}

		/// <summary>
		///     causes a slightly mouse jiggle +-10 pixel
		/// </summary>
		public static void Jiggle()
		{
			int xchange = Utility.RandomInt(-10, 10);
			int ychange = Utility.RandomInt(-10, 10);

			Move(Cursor.Position.X + xchange, Cursor.Position.Y + ychange, true, 5);
			Utility.RandomDelay(20, 60);
			Move(Cursor.Position.X - xchange, Cursor.Position.Y - ychange, true, 5);
		}

		/// <summary>
		///     move the mouse relative to the window
		/// </summary>
		/// <param name="windowHandle">
		///     handle of window
		/// </param>
		/// <param name="targetX">
		///     x coordinate
		/// </param>
		/// <param name="targetY">
		///     y coordinate
		/// </param>
		/// <param name="human">
		///     prevent mouse jumps
		/// </param>
		/// <param name="steps">
		///     points of pathpolygons
		/// </param>
		/// <returns>
		///     true or false
		/// </returns>
		public static bool MoveRelativeToWindow(
			IntPtr windowHandle,
			int targetX,
			int targetY,
			bool human = true,
			int steps = 100)
		{
			Rect window;

			return User32.GetWindowRect(windowHandle, out window)
					&& Move(window.Left + targetX, window.Top + targetY, human, steps);
		}

		/// <summary>
		///     get the current position of the mouse pointer relative to a window
		/// </summary>
		/// <param name="windowHandle">
		///     handle of window
		/// </param>
		/// <returns>
		///     Point position
		/// </returns>
		public static Point GetPositionRelativeToWindow(IntPtr windowHandle)
		{
			Point position = Cursor.Position;
			Rect window;
			User32.GetWindowRect(windowHandle, out window);
			return new Point(position.X - window.Left, position.Y - window.Top);
		}

		/// <summary>
		///     returns whether the cursor is inside a rectangle or outside
		/// </summary>
		/// <param name="t">
		///     top of rectangle
		/// </param>
		/// <param name="l">
		///     left of rectangle
		/// </param>
		/// <param name="b">
		///     bottom of rectangle
		/// </param>
		/// <param name="r">
		///     right of rectangle
		/// </param>
		/// <returns>
		///     inside=true, outside=false
		/// </returns>
		public static bool InRectangle(int t, int l, int b, int r)
		{
			Point p = GetPosition();
			return (l < p.X) && (p.X < r) && (t < p.Y) && (p.Y < b);
		}

		/// <summary>
		///     simulates mouse scroll wheel actions
		/// </summary>
		/// <param name="wheeldelta">
		///     if positive, scrolls down, if negative, scrolls up
		/// </param>
		public static void Scroll(int wheeldelta)
		{
			User32.mouse_event((uint)Constants.MouseEventFlags.Wheel, 0, 0, -wheeldelta, 0);
		}
	}
}