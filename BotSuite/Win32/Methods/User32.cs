// -----------------------------------------------------------------------
//  <copyright file="User32.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Win32.Methods
{
	using System;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using Structs;

	internal class User32
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
		internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

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

		/// <summary>
		///     The set foreground window.
		/// </summary>
		/// <param name="hWnd">
		///     The h wnd.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		[DllImport("user32")]
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
		internal static extern IntPtr SetWindowsHookEx(
			int idHook,
			Delegates.KeyboardHookProc callback,
			IntPtr hInstance,
			uint threadId);

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
		internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

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
		///     The get message extra info
		/// </summary>
		/// <returns>
		///     The <see cref="IntPtr" />
		/// </returns>
		[DllImport("user32")]
		internal static extern IntPtr GetMessageExtraInfo();
	}
}