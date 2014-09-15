/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BotSuite
{
    /// <summary>
    /// Class with functions for window handling
    /// </summary>
	public class Window
	{
		/// <summary>
		/// Sets a window as a foreground window
		/// </summary>
		/// <param name="windowName"></param>
		/// <returns></returns>
		public static Boolean SetFrontWindow(String windowName)
		{
            return NativeMethods.SetForegroundWindow(FindWindowByWindowTitle(windowName));
		}

		/// <summary>
		/// Shows a window
		/// </summary>
		/// <param name="windowHandle"></param>
		/// <returns></returns>
		public static void ShowWindow(IntPtr windowHandle)
		{
			NativeMethods.ShowWindow(windowHandle, 9);
		}
        /// <summary>
        /// Collects MainWindows
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// List<IntPtr> hWnds = new List<IntPtr>();
        /// hWnds = Window.GetAllMainWindows();
        /// ]]>
        /// </code>
        /// </example>
        /// <returns>An array of the main windows' handles</returns>
        public static IntPtr[] GetAllMainWindows()
        {
            var hWnds = new List<IntPtr>();
            Process[] processes = Process.GetProcesses();
            if (processes.Length <= 0)
            {
                return hWnds.ToArray();
            }

            hWnds.AddRange(processes.Select(process => process.MainWindowHandle));

            return hWnds.ToArray();
        }

        /// <summary>
        /// Tries to find a window by its name
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// IntPtr hwnd = (IntPtr)0;
        /// hWnd = Window.FindWindowByProcessName("notepad");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="processName">Process name of window</param>
        /// <returns>Handle of window</returns>
        public static IntPtr FindWindowByProcessName(string processName)
        {
            if (processName.EndsWith(".exe"))
                processName = processName.Remove(processName.Length - 4, 4);

            Process[] process = Process.GetProcessesByName(processName);

            if (process.Length == 0)
                return IntPtr.Zero;

            return process[0].MainWindowHandle;
        }

        /// <summary>
        /// Gets a handle of a window by name
        /// </summary>
        /// <example>
        /// <code>
        /// IntPtr hwnd = Window.GetHandleByWindowTitle("notepad");
        /// </code>
        /// </example>
        /// <param name="windowTitle">Name of window</param>
        /// <returns>Handle of window</returns>
        public static IntPtr FindWindowByWindowTitle(String windowTitle)
        {
            var hWnd = IntPtr.Zero;
            Process[] processes = Process.GetProcesses();
            if (processes.Length <= 0)
            {
                return hWnd;
            }

            foreach (Process process in processes)
            {
                if (process.MainWindowTitle == windowTitle)
                {
                    hWnd = process.MainWindowHandle;
                    break;
                }
            }

            return hWnd;
        }

        /// <summary>
        /// Gets a handle of a window by id of process
        /// </summary>
        /// <param name="id">The process id</param>
        /// <returns>Handle of window</returns>
        public static IntPtr FindWindowByProcessId(int id)
        {
            Process process = Process.GetProcessById(id);
            return process.MainWindowHandle;
        }

        /// <summary>
        /// Hides the maximize button of a window
        /// </summary>
        /// <param name="handle">Window handle</param>
        public static void HideMaximizeButton(IntPtr handle)
        {
            NativeMethods.HideMaximizeButton(handle);
        }

        /// <summary>
        /// Hides the minimize button of a window
        /// </summary>
        /// <param name="handle">Window handle</param>
        public static void HideMinimizeButton(IntPtr handle)
        {
            NativeMethods.HideMinimizeButton(handle);
        }

        /// <summary>
        /// Shows the maximize button of a window
        /// </summary>
        /// <param name="handle">Window handle</param>
        public static void ShowMaximizeButton(IntPtr handle)
        {
            NativeMethods.ShowMaximizeButton(handle);
        }

        /// <summary>
        /// Shows the minimize button of a window
        /// </summary>
        /// <param name="handle">Window handle</param>
        public static void ShowMinimizeButton(IntPtr handle)
        {
            NativeMethods.ShowMinimizeButton(handle);
        }
      
	}
}
