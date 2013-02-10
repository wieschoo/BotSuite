/* **************************************************************
 * Name:      BotSuite.NET
 * Version:   v2.1.0beta3
 * Purpose:   Framework for creating bots
 * Author:    wieschoo
 * Created:   16.09.2012
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2012 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BotSuite
{
    /// <summary>
    /// Class with function for windows handling
    /// </summary>
	public class Window
	{
		/// <summary>
		/// set a window into the front
		/// </summary>
		/// <param name="WindowName"></param>
		/// <returns></returns>
		public static Boolean SetFrontWindow(String WindowName)
		{
            return NativeMethods.SetForegroundWindow(FindWindowByWindowTitle(WindowName));
		}

		/// <summary>
		/// show a window
		/// </summary>
		/// <param name="WindowHandle"></param>
		/// <returns></returns>
		public static void ShowWindow(IntPtr WindowHandle)
		{
			NativeMethods.ShowWindow(WindowHandle, 9);
		}
        /// <summary>
        /// collect MainWindows
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// List<IntPtr> hWnds = new List<IntPtr>();
        /// hWnds = Window.GetAllMainWindows();
        /// ]]>
        /// </code>
        /// </example>
        /// <returns>Returns an array of window handles.</returns>
        public static IntPtr[] GetAllMainWindows()
        {
            List<IntPtr> hWnds = new List<IntPtr>();
            Process[] processes = Process.GetProcesses();
            if (processes.Length <= 0)
            {
                return hWnds.ToArray();
            }

            foreach (Process process in processes)
            {
                hWnds.Add(process.MainWindowHandle);
            }

            return hWnds.ToArray();
        }

        /// <summary>
        /// try to find a window by using the name of the corresponding process
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// IntPtr hwnd = (IntPtr)0;
        /// hWnd = Window.FindWindowByProcessName("notepad");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="ProcessName">handle of window</param>
        /// <returns></returns>
        public static IntPtr FindWindowByProcessName(string ProcessName)
        {
            if (ProcessName.EndsWith(".exe"))
                ProcessName = ProcessName.Remove(ProcessName.Length - 4, 4);

            Process[] process = Process.GetProcessesByName(ProcessName);
            if (process == null || process.Length == 0)
                return IntPtr.Zero;

            return process[0].MainWindowHandle;
        }

        /// <summary>
        /// get a handle of a window by name
        /// </summary>
        /// <example>
        /// <code>
        /// IntPtr hwnd = Window.GetHandleByWindowTitle("notepad");
        /// </code>
        /// </example>
        /// <param name="WindowTitle">name of window</param>
        /// <returns>handle of window</returns>
        public static IntPtr FindWindowByWindowTitle(String WindowTitle)
        {
            IntPtr hWnd = (IntPtr)0;
            Process[] processes = Process.GetProcesses();
            if (processes.Length <= 0)
            {
                return hWnd;
            }

            foreach (Process process in processes)
            {
                if (process.MainWindowTitle == WindowTitle)
                {
                    hWnd = process.MainWindowHandle;
                    break;
                }
            }

            return hWnd;
        }

        /// <summary>
        /// get a handle of a window by the id of the process
        /// </summary>
        /// <param name="id">The process Id of the process in question.</param>
        /// <returns>handle of window</returns>
        public static IntPtr FindWindowByProcessId(int id)
        {
            Process process = Process.GetProcessById(id);
            return process.MainWindowHandle;
        }
      
	}
}
