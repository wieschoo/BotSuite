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
	using Win32.Methods;

	/// <summary>
	///     Class with function for windows handling
	/// </summary>
	public class Window
	{
		/// <summary>
		///     Set a window to be in the front.
		/// </summary>
		/// <param name="windowHandle">
		///     The window handle.
		/// </param>
		/// <returns>
		///     True, if the action was successfull, else false.
		/// </returns>
		public static bool SetFrontWindow(IntPtr windowHandle)
		{
			return User32.SetForegroundWindow(windowHandle);
		}

		/// <summary>
		///     Show a window.
		/// </summary>
		/// <param name="windowHandle">
		///     The window handle.
		/// </param>
		public static void ShowWindow(IntPtr windowHandle)
		{
			User32.ShowWindow(windowHandle, 9);
		}

		/// <summary>
		///     Get all main windows of running processes.
		/// </summary>
		/// <returns>
		///		An array of window handles.
		///	</returns>
		public static IntPtr[] GetAllMainWindows()
		{
			List<IntPtr> hwnds = new List<IntPtr>();
			Process[] processes = Process.GetProcesses();
			if(processes.Length <= 0)
			{
				return hwnds.ToArray();
			}

			hwnds.AddRange(processes.Select(process => process.MainWindowHandle));

			return hwnds.ToArray();
		}

		/// <summary>
		///     Try to find a window by using the name of the corresponding process.
		/// </summary>
		/// <param name="processName">
		///     Name of the process.
		/// </param>
		/// <returns>
		///     The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByProcessName(string processName)
		{
			if(processName.EndsWith(".exe"))
			{
				processName = processName.Remove(processName.Length - 4, 4);
			}

			Process[] process = Process.GetProcessesByName(processName);
			return process.Length == 0 ? IntPtr.Zero : process[0].MainWindowHandle;
		}

		/// <summary>
		///     Get a handle of a window by name.
		///     The title has to match the <paramref name="windowTitle"/> exactly.
		/// </summary>
		/// <param name="windowTitle">
		///     Exact name of the window.
		/// </param>
		/// <returns>
		///     The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByExactWindowTitle(string windowTitle)
		{
			IntPtr hwnd = (IntPtr)0;
			Process[] processes = Process.GetProcesses();

			if(processes.Length <= 0)
			{
				return hwnd;
			}

			Process process = processes.FirstOrDefault(p => p.MainWindowTitle == windowTitle);

			if(process != null)
			{
				hwnd = process.MainWindowHandle;
			}

			return hwnd;
		}

		/// <summary>
		///     Get a handle of a window by name.
		///     The title only has to contain the <paramref name="windowTitle"/> and does not need to match it exactly.
		/// </summary>
		/// <param name="windowTitle">
		///     Title of the window.
		/// </param>
		/// <returns>
		///     The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByWindowTitle(string windowTitle)
		{
			IntPtr hwnd = (IntPtr)0;
			Process[] processes = Process.GetProcesses();

			if(processes.Length <= 0)
			{
				return hwnd;
			}

			Process process = processes.FirstOrDefault(p => p.MainWindowTitle == windowTitle);

			if(process != null)
			{
				hwnd = process.MainWindowHandle;
			}

			return hwnd;
		}

		/// <summary>
		///		Get a handle of a window by the id of the process.
		/// </summary>
		/// <param name="processId">
		///		The id of the process in question.
		/// </param>
		/// <returns>
		///		The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByProcessId(int processId)
		{
			Process process = Process.GetProcessById(processId);

			if(process != null)
			{
				return process.MainWindowHandle;
			}
			else
			{
				return (IntPtr)0;
			}
		}
	}
}