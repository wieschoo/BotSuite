// -----------------------------------------------------------------------
//  <copyright file="Rect.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
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