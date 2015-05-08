using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BotSuite
{
    /// <summary>
    ///  P/Invokes
    /// </summary>
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, IntPtr dwExtraInfo);

        internal enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        internal enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400)
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }
            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }
        }

        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("User32.dll")]
        internal static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }



        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        internal const int WH_KEYBOARD_LL = 13;
        internal const int WM_KEYDOWN = 0x100;
        internal const int WM_KEYUP = 0x101;
        internal const int WM_SYSKEYDOWN = 0x104;
        internal const int WM_SYSKEYUP = 0x105;
        #endregion

        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
        [DllImport("kernel32.dll")]
        internal static extern Int32 CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        internal static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        internal static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);


        [DllImport("kernel32.dll")]
        internal static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);
        [DllImport("kernel32.dll")]
        internal static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SendMessage(IntPtr hWnd, int wMsg, uint wParam, uint lParam);
        [DllImport("user32.dll")]
        internal static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        internal extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        internal extern static int GetWindowLong(IntPtr hwnd, int index);

        internal static void HideMaximizeButton(IntPtr hwnd)
        {
            const int GWL_STYLE = -16;
            const long WS_MAXIMIZEBOX = 0x00010000L;

            long value = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MAXIMIZEBOX));
        }

        internal static void HideMinimizeButton(IntPtr hwnd)
        {
            const int GWL_STYLE = -16;
            const long WS_MINIMIZEBOX = 0x00020000L;

            long value = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MINIMIZEBOX));
        }

        internal static void ShowMaximizeButton(IntPtr hwnd)
        {
            const int GWL_STYLE = -16;
            const long WS_MAXIMIZEBOX = 0x00010000L;

            long value = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (int)(value | WS_MAXIMIZEBOX));
        }

        internal static void ShowMinimizeButton(IntPtr hwnd)
        {
            const int GWL_STYLE = -16;
            const long WS_MINIMIZEBOX = 0x00020000L;

            long value = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (int)(value | WS_MINIMIZEBOX));
        }
    }
}
