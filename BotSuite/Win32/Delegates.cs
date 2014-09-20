// -----------------------------------------------------------------------
//  <copyright file="Delegates.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

using System;

namespace BotSuite.Win32
{
	internal class Delegates
	{
		/// <summary>
		///     The keyboard hook proc.
		/// </summary>
		/// <param name="code">
		///     The code.
		/// </param>
		/// <param name="wParam">
		///     The w param.
		/// </param>
		/// <param name="lParam">
		///     The l param.
		/// </param>
		/// <returns>
		///     some proc
		/// </returns>
		public delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);
	}
}