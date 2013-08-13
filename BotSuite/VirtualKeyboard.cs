using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace BotSuite
{
    class VirtualKeyboard
    {
        
               

        /// <summary>
        /// Imports the Win32 API to post a message to a process
        /// This uses PostMessage to directy sending keys to a process
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// 
        /// 
        /// ////////////////////METHODE 1///////////////////////////////////////
        /// 
        /// Process[] processes = Process.GetProcessesByName("yourprocessname");
        /// 
        /// 
        /// //you can definate your consts before inserting into the function
        /// 
        /// const UInt32 WM_KEYDOWN = 0x0100; // variable (pushes a button)
        /// const UInt32 WM_KEYUP = 0x0101; // variable (key up)
        /// const int W_Key = 0x57; // variable (W Key)
        /// const int VK_RETURN = 0x0D; // variable (sends Return button)
        /// 
        /// foreach (Process proc in processes)
        /// VirtualKeyboard.PostMessage(proc.MainWindowHandle, WM_KEYDOWN, W_Key, 0); // sends a pushed "W Key" to process
        /// 
        /// foreach (Process proc in processes)
        /// VirtualKeyboard.PostMessage(proc.MainWindowHandle, WM_KEYUP, W_Key, 0); // sends a leaved "W Key" to process
        /// 
        /// foreach (Process proc in processes)
        /// VirtualKeyboard.PostMessage(proc.MainWindowHandle, WM_KEYDOWN, VK_RETURN, 0); // sends a pushed "RETURN" button to process
        /// //////////////////////////METHODE END/////////////////////////////////////////
        /// 
        /// 
        /// /////////////////////////METHODE 2 ///////////////////////////////////////////
        /// 
        /// Process[] processes = Process.GetProcessesByName("yourprocessname");
        /// 
        /// foreach (Process proc in processes)
        /// VirtualKeyboard.PostMessage(proc.MainWindowHandle, 0x100, 0x57, 0); // // sends a pushed "W Key" to process
        /// 
        /// ////////////////////////METHODE END ///////////////////////////////////////////
        /// 
        /// 
        /// // Here are the Virtual-Key-Codes "http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731%28v=vs.85%29.aspx" 
        /// </code>
        /// </example>
        /// <param name="hWnd">Integer for the process</param>
        /// <param name="Msg">KeyEvent</param>
        /// <param name="wParam">Parameter of the button</param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        

        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;


        /// <summary>
        /// Imports the Win32 API to set a process window to the foreground
        /// </summary>
        /// <example>
        /// <code>
        /// 
        /// VirtualKeyboard.ActivateProcess("yourprocessname");
        /// </code>
        /// </example>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ActivateProcess(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);

            // Activate the first process we find with this name and sets its window to the foreground
            if (p.Count() > 0)
                SetForegroundWindow(p[0].MainWindowHandle);
        }



        
        /// <summary>
        /// Imports the Win32 API to set keybd_event event + KeyDown + KeyUp
        /// works like SendInput/SendKeys just for Windows
        /// </summary>
        /// <example>
        /// <code>
        /// // Sends keys to a window
        /// // Activate Process required for this methode (look example) 
        /// 
        /// VirtualKeyboard.ActivateProcess("yourprocessname"); // Is watching out for a process by this name and sets its window to the foreground
        /// 
        /// VirtualKeyboard.KeyDown(System.Windows.Forms.Keys.A);
        /// Thread.Sleep(200); // change the value inside for pauseing the process between the events
        /// VirtualKeyboard.KeyUp(System.Windows.Forms.Keys.A);
        /// </code>
        /// </example>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public static void KeyDown(System.Windows.Forms.Keys key)
        {
            keybd_event((byte)key, 0, 0, 0);
        }

        public static void KeyUp(System.Windows.Forms.Keys key)
        {
            keybd_event((byte)key, 0, 0x0002, 0);
        }

        /// <summary>
        /// Hold down a key for a specific time
        /// </summary>
        /// <example>
        /// <code>
        /// // Sends keys to a window for a specific time period
        /// // Activate Process required for this methode (look example) 
        /// 
        /// VirtualKeyboard.ActivateProcess("yourprocessname"); // Is watching out for a process by this name and sets its window to the foreground
        /// 
        /// VirtualKeyboard.HoldKey((byte)Keys.A, 250); // Holds down the "A" key for 250ms
        /// VirtualKeyboard.HoldKey((byte)Keys.Left, 1000); // Holds down "Left" key for 1 second
        /// </code>
        /// </example>
        /// <param name="key"></param>
        /// <param name="duration"></param>

        public static void HoldKey(byte key, int duration)
        {
            const int KEY_DOWN_EVENT = 0x0001;
            const int KEY_UP_EVENT = 0x0002;
            int totalDuration = 0;
            const int PauseFor = 30;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
                System.Threading.Thread.Sleep(PauseFor);
                totalDuration += PauseFor;
            }
        }
    }
}
