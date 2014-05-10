// -----------------------------------------------------------------------
//  <copyright file="Window.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	using BotSuite.Native.Methods;

	/// <summary>
	///     Class with function for windows handling
	/// </summary>
	public class Window
	{
		/// <summary>
		///     set a window into the front
		/// </summary>
		/// <param name="windowName">
		///     the window name
		/// </param>
		/// <returns>
		///     true, if action worked
		/// </returns>
		public static bool SetFrontWindow(string windowName)
		{
			return User32.SetForegroundWindow(FindWindowByWindowTitle(windowName));
		}

		/// <summary>
		///     show a window
		/// </summary>
		/// <param name="windowHandle">
		///     the window handle
		/// </param>
		public static void ShowWindow(IntPtr windowHandle)
		{
			User32.ShowWindow(windowHandle, 9);
		}

		/// <summary>
		///     collect MainWindows
		/// </summary>
		/// <returns>Returns an array of window handles.</returns>
		public static IntPtr[] GetAllMainWindows()
		{
			List<IntPtr> hwnds = new List<IntPtr>();
			Process[] processes = Process.GetProcesses();
			if (processes.Length <= 0)
			{
				return hwnds.ToArray();
			}

			hwnds.AddRange(processes.Select(process => process.MainWindowHandle));

			return hwnds.ToArray();
		}

		/// <summary>
		///     try to find a window by using the name of the corresponding process
		/// </summary>
		/// <param name="processName">
		///     handle of window
		/// </param>
		/// <returns>
		///     the handle of the window
		/// </returns>
		public static IntPtr FindWindowByProcessName(string processName)
		{
			if (processName.EndsWith(".exe"))
			{
				processName = processName.Remove(processName.Length - 4, 4);
			}

			Process[] process = Process.GetProcessesByName(processName);
			return process.Length == 0 ? IntPtr.Zero : process[0].MainWindowHandle;
		}

		/// <summary>
		///     get a handle of a window by name
		/// </summary>
		/// <param name="windowTitle">
		///     name of window
		/// </param>
		/// <returns>
		///     handle of window
		/// </returns>
		public static IntPtr FindWindowByWindowTitle(string windowTitle)
		{
			IntPtr hwnd = (IntPtr)0;
			Process[] processes = Process.GetProcesses();
			if (processes.Length <= 0)
			{
				return hwnd;
			}

			foreach (Process process in processes.Where(process => process.MainWindowTitle == windowTitle))
			{
				hwnd = process.MainWindowHandle;
				break;
			}

			return hwnd;
		}

		/// <summary>
		///     get a handle of a window by the id of the process
		/// </summary>
		/// <param name="id">
		///     The process Id of the process in question.
		/// </param>
		/// <returns>
		///     handle of window
		/// </returns>
		public static IntPtr FindWindowByProcessId(int id)
		{
			Process process = Process.GetProcessById(id);
			return process.MainWindowHandle;
		}
	}
}