// -----------------------------------------------------------------------
//  <copyright file="Delegates.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Native
{
	using BotSuite.Native.Structs;

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
		public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);
	}
}