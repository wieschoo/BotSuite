// -----------------------------------------------------------------------
//  <copyright file="NativeMethods.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;

	/// <summary>
	///     P/Invoke Methods
	/// </summary>
	public class NativeMethods
	{
		/// <summary>
		///     The mouse_event.
		/// </summary>
		/// <param name="dwFlags">
		///     The dw flags.
		/// </param>
		/// <param name="dx">
		///     The dx.
		/// </param>
		/// <param name="dy">
		///     The dy.
		/// </param>
		/// <param name="dwData">
		///     The dw data.
		/// </param>
		/// <param name="dwExtraInfo">
		///     The dw extra info.
		/// </param>
		[DllImport("user32")]
		internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, IntPtr dwExtraInfo);

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

		/// <summary>
		///     The set foreground window.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32", ExactSpelling = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		///     The get window rect.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <param name="lpRect">
		///     The lp rect.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

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

		/// <summary>
		///     The get async key state.
		/// </summary>
		/// <param name="vKey">
		///     The v key.
		/// </param>
		/// <returns>
		///     The <see cref="short" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern short GetAsyncKeyState(Keys vKey);

		/// <summary>
		///     The show window.
		/// </summary>
		/// <param name="hwnd">
		///     The hwnd.
		/// </param>
		/// <param name="nCmdShow">
		///     The n cmd show.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("User32")]
		internal static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		/// <summary>
		///     The keyboard hook struct.
		/// </summary>
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
		///     The set windows hook ex.
		/// </summary>
		/// <param name="idHook">
		///     The id hook.
		/// </param>
		/// <param name="callback">
		///     The callback.
		/// </param>
		/// <param name="hInstance">
		///     The h instance.
		/// </param>
		/// <param name="threadId">
		///     The thread id.
		/// </param>
		/// <returns>
		///     The <see cref="IntPtr" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

		/// <summary>
		///     The unhook windows hook ex.
		/// </summary>
		/// <param name="hInstance">
		///     The h instance.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		/// <summary>
		///     The call next hook ex.
		/// </summary>
		/// <param name="idHook">
		///     The id hook.
		/// </param>
		/// <param name="nCode">
		///     The n code.
		/// </param>
		/// <param name="wParam">
		///     The w param.
		/// </param>
		/// <param name="lParam">
		///     The l param.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

		/// <summary>
		///     The load library.
		/// </summary>
		/// <param name="lpFileName">
		///     The lp file name.
		/// </param>
		/// <returns>
		///     The <see cref="IntPtr" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern IntPtr LoadLibrary(string lpFileName);

		/// <summary>
		///     The open process.
		/// </summary>
		/// <param name="dwDesiredAccess">
		///     The dw desired access.
		/// </param>
		/// <param name="bInheritHandle">
		///     The b inherit handle.
		/// </param>
		/// <param name="dwProcessId">
		///     The dw process id.
		/// </param>
		/// <returns>
		///     The <see cref="IntPtr" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

		/// <summary>
		///     The close handle.
		/// </summary>
		/// <param name="hObject">
		///     The h object.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern int CloseHandle(IntPtr hObject);

		/// <summary>
		///     The read process memory.
		/// </summary>
		/// <param name="hProcess">
		///     The h process.
		/// </param>
		/// <param name="lpBaseAddress">
		///     The lp base address.
		/// </param>
		/// <param name="buffer">
		///     The buffer.
		/// </param>
		/// <param name="size">
		///     The size.
		/// </param>
		/// <param name="lpNumberOfBytesRead">
		///     The lp number of bytes read.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern int ReadProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			[In] [Out] byte[] buffer,
			uint size,
			out IntPtr lpNumberOfBytesRead);

		/// <summary>
		///     The write process memory.
		/// </summary>
		/// <param name="hProcess">
		///     The h process.
		/// </param>
		/// <param name="lpBaseAddress">
		///     The lp base address.
		/// </param>
		/// <param name="buffer">
		///     The buffer.
		/// </param>
		/// <param name="size">
		///     The size.
		/// </param>
		/// <param name="lpNumberOfBytesWritten">
		///     The lp number of bytes written.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern int WriteProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			[In] [Out] byte[] buffer,
			uint size,
			out IntPtr lpNumberOfBytesWritten);

		/// <summary>
		///     The write private profile string.
		/// </summary>
		/// <param name="section">
		///     The section.
		/// </param>
		/// <param name="key">
		///     The key.
		/// </param>
		/// <param name="value">
		///     The value.
		/// </param>
		/// <param name="filePath">
		///     The file path.
		/// </param>
		/// <returns>
		///     The <see cref="long" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

		/// <summary>
		///     The get private profile string.
		/// </summary>
		/// <param name="section">
		///     The section.
		/// </param>
		/// <param name="key">
		///     The key.
		/// </param>
		/// <param name="Default">
		///     The default.
		/// </param>
		/// <param name="retVal">
		///     The ret val.
		/// </param>
		/// <param name="size">
		///     The size.
		/// </param>
		/// <param name="filePath">
		///     The file path.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		[DllImport("kernel32")]
		internal static extern int GetPrivateProfileString(
			string section,
			string key,
			string Default,
			StringBuilder retVal,
			int size,
			string filePath);

		/// <summary>
		///     The print window.
		/// </summary>
		/// <param name="hwnd">
		///     The hwnd.
		/// </param>
		/// <param name="hdcBlt">
		///     The hdc blt.
		/// </param>
		/// <param name="nFlags">
		///     The n flags.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

		/// <summary>
		///     The get window dc.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <returns>
		///     The <see cref="IntPtr" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern IntPtr GetWindowDC(IntPtr hWnd);

		/// <summary>
		///     The vk key scan.
		/// </summary>
		/// <param name="ch">
		///     The ch.
		/// </param>
		/// <returns>
		///     The <see cref="short" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern short VkKeyScan(char ch);

		/// <summary>
		///     The post message.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <param name="msg">
		///     The msg.
		/// </param>
		/// <param name="wParam">
		///     The w param.
		/// </param>
		/// <param name="lParam">
		///     The l param.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		/// <summary>
		///     The send message.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <param name="msg">
		///     The msg.
		/// </param>
		/// <param name="wParam">
		///     The w param.
		/// </param>
		/// <param name="lParam">
		///     The l param.
		/// </param>
		/// <returns>
		///     The <see cref="IntPtr" />.
		/// </returns>
		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		///     The send message.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <param name="wMsg">
		///     The w msg.
		/// </param>
		/// <param name="wParam">
		///     The w param.
		/// </param>
		/// <param name="lParam">
		///     The l param.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern bool SendMessage(IntPtr hWnd, int wMsg, uint wParam, uint lParam);

		/// <summary>
		///     The keybd_event.
		/// </summary>
		/// <param name="bVk">
		///     The b vk.
		/// </param>
		/// <param name="bScan">
		///     The b scan.
		/// </param>
		/// <param name="dwFlags">
		///     The dw flags.
		/// </param>
		/// <param name="dwExtraInfo">
		///     The dw extra info.
		/// </param>
		/// <returns>
		///     The <see cref="uint" />.
		/// </returns>
		[DllImport("user32")]
		internal static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
	}
}