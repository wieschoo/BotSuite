using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace BotSuite
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    /// <remarks>
    /// Singleton-Pattern
    /// </remarks>
    public class Keyboard
    {
        static Keyboard _instance;
        static Keyboard()
        {
            _instance = new Keyboard();
        }

        public static Keyboard Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Keyboard.HookedKeys.Add(Keys.F5);
        /// ]]>
        /// </code>
        /// </example>
        public static List<Keys> HookedKeys = new List<Keys>();
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;

        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Keyboard.KeyDown += new KeyEventHandler(MyKeyDownRoutine);
        /// ]]>
        /// </code>
        /// </example>
        public static event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Keyboard.KeyUp += new KeyEventHandler(MyKeyUpRoutine);
        /// ]]>
        /// </code>
        /// </example>
        public static event KeyEventHandler KeyUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
        /// </summary>
        /// <remarks>
        /// use the whole class by the code from the example!
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Keyboard.HookedKeys.Add(Keys.F5);
        /// Keyboard.KeyDown += new KeyEventHandler(MyKeyDownRoutine);
        /// Keyboard.KeyUp += new KeyEventHandler(MyKeyUpRoutine);
        /// ]]>
        /// </code>
        /// </example>
        public Keyboard()
        {
            hook();
        }

        /// <summary>
        /// unmanaged resources were released and other cleanup operations were performed before the
        /// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        /// <remarks>
        /// destructor
        /// </remarks>
        ~Keyboard()
        {
            unhook();
        }

        public static NativeMethods.keyboardHookProc SAFE_delegate_callback = new NativeMethods.keyboardHookProc(hookProc);

        /// <remarks>
        /// this is a private method. You cannot us it! See the constructor #ctor for an example.
        /// </remarks>
        private void hook()
        {
            IntPtr hInstance = NativeMethods.LoadLibrary("User32");
            hhook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, SAFE_delegate_callback, hInstance, 0);
        }

        /// <summary>
        /// removes the global hook
        /// </summary>
        /// <remarks>
        /// this is a private method. You cannot us it! See the constructor #ctor for an example.
        /// </remarks>
        private void unhook()
        {
            NativeMethods.UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        /// callback for the keyboard hook
        /// </summary>
        /// <param name="code">hook code, do sth iff >=0</param>
        /// <param name="wParam">event type</param>
        /// <param name="lParam">keyhook event information</param>
        /// <remarks>
        /// this is a private method. You cannot us it! See the constructor #ctor for an example.
        /// </remarks>
        /// <returns></returns>
        private static int hookProc(int code, int wParam, ref NativeMethods.keyboardHookStruct lParam)
        {

            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (HookedKeys != null)
                {
                    if (HookedKeys.Contains(key))
                    {
                        KeyEventArgs kea = new KeyEventArgs(key);
                        if ((wParam == NativeMethods.WM_KEYDOWN || wParam == NativeMethods.WM_SYSKEYDOWN) && (KeyDown != null))
                        {
                            KeyDown(Instance, kea);
                        }
                        else if ((wParam == NativeMethods.WM_KEYUP || wParam == NativeMethods.WM_SYSKEYUP) && (KeyUp != null))
                        {
                            KeyUp(Instance, kea);
                        }
                        if (kea.Handled)
                            return 1;
                    }
                }
            }
            return NativeMethods.CallNextHookEx(Instance.hhook, code, wParam, ref lParam);
        }

        /// <summary>
        /// types a key or o sequence of keys
        /// </summary>
        /// <example>
        /// <code>
        /// Keyboard.Type("{ENTER}"); // click the enter button
        /// Keyboard.Type("ENTER");   // types "E","N","T","E","R"
        /// </code>
        /// </example>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.aspx
        /// </remarks>
        /// <param name="Sequence">Sequence to type</param>
        /// <returns></returns>
        public static void Type(String Sequence)
        {
            SendKeys.Send(Sequence.Trim());
        }

        /// <summary>
        /// types a key or o sequence of keys to an application (by set the application to foreground)
        /// </summary>
        /// <example>
        /// <code>
        /// IntPtr hWnd = something;
        /// Keyboard.Type("{ENTER}",hWnd); // click the enter button
        /// Keyboard.Type("ENTER",hWnd);   // types "E","N","T","E","R"
        /// // or send it to a specific window 
        /// </code>
        /// </example>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.aspx
        /// </remarks>
        /// <param name="Sequence">Sequence to type</param>
        /// <param name="hWnd">target application</param>
        /// <returns></returns>
        public static void Type(String Sequence, IntPtr hWnd)
        {
            NativeMethods.SetForegroundWindow((IntPtr)hWnd);
            SendKeys.SendWait(Sequence.Trim());
            SendKeys.Flush();
        }

        /// <summary>
        /// send keys as an array to a hidden window
        /// </summary>
        /// <param name="Key">array of keys to send</param>
        /// <param name="hWnd">handle of window</param>
        public static void TypeToHiddenWindow(Keys[] Key, IntPtr hWnd)
        {
            Key.ToList().ForEach(k => { TypeToHiddenWindow(k, hWnd); });
        }

        /// <summary>
        /// send a key to a hidden window
        /// </summary>
        /// <param name="Key">key to send</param>
        /// <param name="hWnd">handle of window</param>
        public static void TypeToHiddenWindow(Keys Key, IntPtr hWnd)
        {

            const int KEY_DOWN_EVENT = 0x0100;
            const int KEY_UP_EVENT = 0x0101;
            const int CHAR_EVENT = 0x0102;

            // send key down event
            if (NativeMethods.SendMessage(hWnd, KEY_DOWN_EVENT, (uint)Key, GetLParam(1, Key, 0, 0, 0, 0)))
                return;
            Utility.Delay(20);
            // send character event
            if (NativeMethods.SendMessage(hWnd, CHAR_EVENT, (uint)Key, GetLParam(1, Key, 0, 0, 0, 0)))
                return;
            Utility.Delay(20);
            // send key up event
            if (NativeMethods.SendMessage(hWnd, KEY_UP_EVENT, (uint)Key, GetLParam(1, Key, 0, 0, 1, 1)))
                return;
            Utility.Delay(20);

        }


        private static uint GetLParam(Int16 NumberOfRepetitions, Keys key, byte extended, byte contextCode, byte previousState, byte transitionState)
        {
            uint lParam = (uint)NumberOfRepetitions;
            uint scanCode = (uint)key;
            lParam += (uint)(scanCode * 0x10000);
            lParam += (uint)((extended) * 0x1000000);
            lParam += (uint)((contextCode * 2) * 0x10000000);
            lParam += (uint)((previousState * 4) * 0x10000000);
            lParam += (uint)((transitionState * 8) * 0x10000000);
            return lParam;
        }

        /// <summary>
        /// Hold down a key for a specific time
        /// </summary>
        /// <example>
        /// <code>
        /// Keyboard.HoldKey(Keys.A, 250); // Holds down the "A" key for 250ms
        /// Keyboard.HoldKey(Keys.Left, 1000); // Holds down "Left" key for 1 second
        /// </code>
        /// </example>
        /// <param name="key">key to hold</param>
        /// <param name="duration">duration </param>
        public static void HoldKey(Keys key, int duration = 500)
        {
            const int KEY_DOWN_EVENT = 0x0001;
            const int KEY_UP_EVENT = 0x0002;

            NativeMethods.keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
            System.Threading.Thread.Sleep(duration);
            NativeMethods.keybd_event((byte)key, 0, KEY_UP_EVENT, 0);

        }

        /// <summary>
        /// test if a key is pressed
        /// </summary>
        /// <example>
        /// <code>
        /// // test if F1 is currently pressed
        /// bool pressed = Keyboard.IsKeyDown(Keys.F1);
        /// </code>
        /// </example>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/system.windows.forms.keys
        /// </remarks>
        /// <param name="key">key</param>
        /// <returns>true/false</returns>
        public static Boolean IsKeyDown(Keys key)
        {
            return (NativeMethods.GetAsyncKeyState(key) == Int16.MinValue);
        }
    }
}
