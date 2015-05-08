// -----------------------------------------------------------------------
//  <copyright file="Delegates.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
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