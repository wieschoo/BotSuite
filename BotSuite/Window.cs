// -----------------------------------------------------------------------
//  <copyright file="Window.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Win32.Methods;

	/// <summary>
	///     Class with functions for handling windows.
	/// </summary>
	public class Window
	{
		private const string EXT_EXE = ".exe";

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
			List<IntPtr> windowHandles = new List<IntPtr>();
			Process[] processes = Process.GetProcesses();

			if(processes.Length > 0)
			{
				windowHandles.AddRange(processes.Select(process => process.MainWindowHandle));
			}

			return windowHandles.ToArray();
		}

		/// <summary>
		///     Try to find a window by using the name of the corresponding process.
		///     If no process with the given name is found, IntPtr.Zero is returned.
		///     If multiple processes have the same name, only the main window handle of the first one found will be returned.
		/// </summary>
		/// <param name="processName">
		///     Name of the process.
		/// </param>
		/// <returns>
		///     The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByProcessName(string processName)
		{
			if(processName.EndsWith(EXT_EXE))
			{
				processName = processName.Remove(processName.Length - EXT_EXE.Length, EXT_EXE.Length);
			}

			Process[] processes = Process.GetProcessesByName(processName);
			IntPtr windowHandle = IntPtr.Zero;

			if(processes.Length > 0)
			{
				windowHandle = processes.First().MainWindowHandle;
			}

			return windowHandle;
		}

		/// <summary>
		///     Get a handle of a window by name.
		///     If no window with the given title is found, IntPtr.Zero is returned.
		///     If multiple Windows have the same title, only the first one found will be returned.
		/// </summary>
		/// <param name="windowTitle">
		///     Title of the window.
		/// </param>
		/// <param name="exactMatch">
		///		True, if the title has to match exactly, else false. (Default: True)
		/// </param>
		/// <returns>
		///     The handle of the window.
		/// </returns>
		public static IntPtr FindWindowByWindowTitle(string windowTitle, bool exactMatch = true)
		{
			IntPtr windowHandle = IntPtr.Zero;
			Process[] processes = Process.GetProcesses();

			if(processes.Length > 0)
			{
				Process process = null;

				if(exactMatch)
				{
					process = processes.FirstOrDefault(p => p.MainWindowTitle == windowTitle);
				}
				else
				{
					process = processes.FirstOrDefault(p => p.MainWindowTitle.Contains(windowTitle));
				}

				if(process != null)
				{
					windowHandle = process.MainWindowHandle;
				}
			}

			return windowHandle;
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
			IntPtr windowHandle = IntPtr.Zero;
			Process process = Process.GetProcessById(processId);

			if(process != null)
			{
				windowHandle = process.MainWindowHandle;
			}

			return windowHandle;
		}
	}
}
