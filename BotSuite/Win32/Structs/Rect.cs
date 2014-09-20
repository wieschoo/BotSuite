// -----------------------------------------------------------------------
//  <copyright file="Rect.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Win32.Structs
{
	using System.Runtime.InteropServices;

	/// <summary>
	///     The rect.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct Rect
	{
		/// <summary>
		///     The left.
		/// </summary>
		public int Left;

		/// <summary>
		///     The top.
		/// </summary>
		public int Top;

		/// <summary>
		///     The right.
		/// </summary>
		public int Right;

		/// <summary>
		///     The bottom.
		/// </summary>
		public int Bottom;
	}
}