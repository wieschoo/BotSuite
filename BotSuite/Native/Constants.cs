// -----------------------------------------------------------------------
//  <copyright file="Constants.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Native
{
	using System;

	internal class Constants
	{
		/// <summary>
		///     defines the callback type for the hook
		/// </summary>
		internal const int WhKeyboardLl = 13;

		/// <summary>
		///     The wm keydown.
		/// </summary>
		internal const int WmKeydown = 0x100;

		/// <summary>
		///     The wm keyup.
		/// </summary>
		internal const int WmKeyup = 0x101;

		/// <summary>
		///     The wm syskeydown.
		/// </summary>
		internal const int WmSyskeydown = 0x104;

		/// <summary>
		///     The wm syskeyup.
		/// </summary>
		internal const int WmSyskeyup = 0x105;

		/// <summary>
		///     The key down event.
		/// </summary>
		internal const int KeyDownEvent = 0x1;

		/// <summary>
		///     The key up event.
		/// </summary>
		internal const int KeyUpEvent = 0x2;

		/// <summary>
		///     The mouse event flags.
		/// </summary>
		[Flags]
		internal enum MouseEventFlags : uint
		{
			/// <summary>
			///     The leftdown.
			/// </summary>
			Leftdown = 0x00000002,

			/// <summary>
			///     The leftup.
			/// </summary>
			Leftup = 0x00000004,

			/// <summary>
			///     The middledown.
			/// </summary>
			Middledown = 0x00000020,

			/// <summary>
			///     The middleup.
			/// </summary>
			Middleup = 0x00000040,

			/// <summary>
			///     The move.
			/// </summary>
			Move = 0x00000001,

			/// <summary>
			///     The absolute.
			/// </summary>
			Absolute = 0x00008000,

			/// <summary>
			///     The rightdown.
			/// </summary>
			Rightdown = 0x00000008,

			/// <summary>
			///     The rightup.
			/// </summary>
			Rightup = 0x00000010,

			/// <summary>
			///     The wheel.
			/// </summary>
			Wheel = 0x00000800,

			/// <summary>
			///     The xdown.
			/// </summary>
			Xdown = 0x00000080,

			/// <summary>
			///     The xup.
			/// </summary>
			Xup = 0x00000100
		}

		/// <summary>
		///     The process access type.
		/// </summary>
		[Flags]
		internal enum ProcessAccessType
		{
			/// <summary>
			///     The process terminate.
			/// </summary>
			ProcessTerminate = 0x0001,

			/// <summary>
			///     The process create thread.
			/// </summary>
			ProcessCreateThread = 0x0002,

			/// <summary>
			///     The process set sessionid.
			/// </summary>
			ProcessSetSessionid = 0x0004,

			/// <summary>
			///     The process vm operation.
			/// </summary>
			ProcessVmOperation = 0x0008,

			/// <summary>
			///     The process vm read.
			/// </summary>
			ProcessVmRead = 0x0010,

			/// <summary>
			///     The process vm write.
			/// </summary>
			ProcessVmWrite = 0x0020,

			/// <summary>
			///     The process dup handle.
			/// </summary>
			ProcessDupHandle = 0x0040,

			/// <summary>
			///     The process create process.
			/// </summary>
			ProcessCreateProcess = 0x0080,

			/// <summary>
			///     The process set quota.
			/// </summary>
			ProcessSetQuota = 0x0100,

			/// <summary>
			///     The process set information.
			/// </summary>
			ProcessSetInformation = 0x0200,

			/// <summary>
			///     The process query information.
			/// </summary>
			ProcessQueryInformation = 0x0400
		}
	}
}