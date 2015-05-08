// -----------------------------------------------------------------------
//  <copyright file="KeyboardHookStruct.cs" company="Binary Overdrive">
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