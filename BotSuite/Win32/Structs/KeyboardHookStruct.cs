// -----------------------------------------------------------------------
//  <copyright file="KeyboardHookStruct.cs" company="Wieschoo &amp; Binary Overdrive">
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
	///     The keyboard hook struct.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct KeyboardHookStruct
	{
		/// <summary>
		///     The vk code.
		/// </summary>
		public int VkCode;

		/// <summary>
		///     The scan code.
		/// </summary>
		public int ScanCode;

		/// <summary>
		///     The flags.
		/// </summary>
		public int Flags;

		/// <summary>
		///     The time.
		/// </summary>
		public int Time;

		/// <summary>
		///     The dw extra info.
		/// </summary>
		public int DwExtraInfo;
	}
}