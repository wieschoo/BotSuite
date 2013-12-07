// -----------------------------------------------------------------------
//  <copyright file="Keyboard.cs" company="HoovesWare">
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
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Windows.Forms;

	/// <summary>
	///     A class that manages a global low level keyboard hook
	/// </summary>
	/// <remarks>
	///     Singleton Pattern
	/// </remarks>
	public class Keyboard
	{
		/// <summary>
		///     The instance.
		/// </summary>
		private static readonly Keyboard Instance;

		/// <summary>
		///     Initializes static members of the <see cref="Keyboard" /> class.
		/// </summary>
		static Keyboard()
		{
			Instance = new Keyboard();
		}

		/// <summary>
		///     The collections of keys to watch for
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// Keyboard.HookedKeys.Add(Keys.F5);
		/// ]]>
		/// </code>
		/// </example>
		private static readonly List<Keys> HookedKeys = new List<Keys>();

		/// <summary>
		///     Handle to the hook, need this to unhook and call the next hook
		/// </summary>
		private IntPtr hhook = IntPtr.Zero;

		/// <summary>
		///     Occurs when one of the hooked keys is pressed
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// Keyboard.KeyDown += new KeyEventHandler(MyKeyDownRoutine);
		/// ]]>
		/// </code>
		/// </example>
		public static event KeyEventHandler KeyDown;

		/// <summary>
		///     Occurs when one of the hooked keys is released
		/// </summary>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// Keyboard.KeyUp += new KeyEventHandler(MyKeyUpRoutine);
		/// ]]>
		/// </code>
		/// </example>
		public static event KeyEventHandler KeyUp;

		/// <summary>
		///     Initializes a new instance of the <see cref="Keyboard" /> class and installs the keyboard hook.
		/// </summary>
		/// <remarks>
		///     use the whole class by the code from the example!
		/// </remarks>
		/// <example>
		///     <code>
		/// <![CDATA[
		/// Keyboard.HookedKeys.Add(Keys.F5);
		/// Keyboard.KeyDown += new KeyEventHandler(MyKeyDownRoutine);
		/// Keyboard.KeyUp += new KeyEventHandler(MyKeyUpRoutine);
		/// ]]>
		/// </code>
		/// </example>
		public Keyboard()
		{
			this.Hook();
		}

		/// <summary>
		///     Finalizes an instance of the <see cref="Keyboard" /> class.
		/// </summary>
		~Keyboard()
		{
			this.Unhook();
		}

		/// <summary>
		///     The safe delegate callback.
		/// </summary>
		private static readonly NativeMethods.KeyboardHookProc SafeDelegateCallback = HookProc;

		/// <summary>
		///     The hook.
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Hook()
		{
			IntPtr instance = NativeMethods.LoadLibrary("User32");
			this.hhook = NativeMethods.SetWindowsHookEx(NativeMethods.WhKeyboardLl, SafeDelegateCallback, instance, 0);
		}

		/// <summary>
		///     removes the global hook
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Unhook()
		{
			NativeMethods.UnhookWindowsHookEx(this.hhook);
		}

		/// <summary>
		///     callback for the keyboard hook
		/// </summary>
		/// <param name="code">
		///     hook code, do sth iff &gt;=0
		/// </param>
		/// <param name="wparam">
		///     event type
		/// </param>
		/// <param name="lparam">
		///     keyhook event information
		/// </param>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		/// <returns>
		///     the int
		/// </returns>
		private static int HookProc(int code, int wparam, ref NativeMethods.KeyboardHookStruct lparam)
		{
			if (code < 0)
			{
				return NativeMethods.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			Keys key = (Keys)lparam.VkCode;
			if (HookedKeys == null)
			{
				return NativeMethods.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			if (!HookedKeys.Contains(key))
			{
				return NativeMethods.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			KeyEventArgs kea = new KeyEventArgs(key);
			if ((wparam == NativeMethods.WmKeydown || wparam == NativeMethods.WmSyskeydown) && (KeyDown != null))
			{
				KeyDown(Instance, kea);
			}
			else if ((wparam == NativeMethods.WmKeyup || wparam == NativeMethods.WmSyskeyup) && (KeyUp != null))
			{
				KeyUp(Instance, kea);
			}

			return kea.Handled ? 1 : NativeMethods.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
		}

		/// <summary>
		///     types a key or o sequence of keys
		/// </summary>
		/// <example>
		///     <code>
		/// Keyboard.Type("{ENTER}"); // click the enter button
		/// Keyboard.Type("ENTER");   // types "E","N","T","E","R"
		/// </code>
		/// </example>
		/// <param name="sequence">
		///     Sequence to type
		/// </param>
		public static void Type(string sequence)
		{
			SendKeys.Send(sequence.Trim());
		}

		/// <summary>
		///     types a key or o sequence of keys to an application (by set the application to foreground)
		/// </summary>
		/// <example>
		///     <code>
		/// IntPtr hWnd = something;
		/// Keyboard.Type("{ENTER}",hWnd); // click the enter button
		/// Keyboard.Type("ENTER",hWnd);   // types "E","N","T","E","R"
		/// // or send it to a specific window 
		/// </code>
		/// </example>
		/// <param name="sequence">
		///     Sequence to type
		/// </param>
		/// <param name="hwnd">
		///     target application
		/// </param>
		public static void Type(string sequence, IntPtr hwnd)
		{
			NativeMethods.SetForegroundWindow(hwnd);
			SendKeys.SendWait(sequence.Trim());
			SendKeys.Flush();
		}

		/// <summary>
		///     send keys as an array to a hidden window
		/// </summary>
		/// <param name="key">
		///     array of keys to send
		/// </param>
		/// <param name="hwnd">
		///     handle of window
		/// </param>
		public static void TypeToHiddenWindow(Keys[] key, IntPtr hwnd)
		{
			key.ToList().ForEach(k => TypeToHiddenWindow(k, hwnd));
		}

		/// <summary>
		///     send a key to a hidden window
		/// </summary>
		/// <param name="key">
		///     key to send
		/// </param>
		/// <param name="hwnd">
		///     handle of window
		/// </param>
		public static void TypeToHiddenWindow(Keys key, IntPtr hwnd)
		{
			const int KeyDownEvent = 0x0100;
			const int KeyUpEvent = 0x0101;
			const int CharEvent = 0x0102;

			// send key down event
			if (NativeMethods.SendMessage(hwnd, KeyDownEvent, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send character event
			if (NativeMethods.SendMessage(hwnd, CharEvent, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send key up event
			if (NativeMethods.SendMessage(hwnd, KeyUpEvent, (uint)key, GetLParam(1, key, 0, 0, 1, 1)))
			{
				return;
			}

			Utility.Delay(20);
		}

		/// <summary>
		///     The get l param.
		/// </summary>
		/// <param name="numberOfRepetitions">
		///     The number of repetitions.
		/// </param>
		/// <param name="key">
		///     The key.
		/// </param>
		/// <param name="extended">
		///     The extended.
		/// </param>
		/// <param name="contextCode">
		///     The context code.
		/// </param>
		/// <param name="previousState">
		///     The previous state.
		/// </param>
		/// <param name="transitionState">
		///     The transition state.
		/// </param>
		/// <returns>
		///     The <see cref="uint" />.
		/// </returns>
		private static uint GetLParam(
			short numberOfRepetitions,
			Keys key,
			byte extended,
			byte contextCode,
			byte previousState,
			byte transitionState)
		{
			uint lparam = (uint)numberOfRepetitions;
			uint scanCode = (uint)key;
			lparam += scanCode * 0x10000;
			lparam += (uint)(extended * 0x1000000);
			lparam += (uint)((contextCode * 2) * 0x10000000);
			lparam += (uint)((previousState * 4) * 0x10000000);
			lparam += (uint)((transitionState * 8) * 0x10000000);
			return lparam;
		}

		/// <summary>
		///     Hold down a key for a specific time
		/// </summary>
		/// <example>
		///     <code>
		/// Keyboard.HoldKey(Keys.A, 250); // Holds down the "A" key for 250ms
		/// Keyboard.HoldKey(Keys.Left, 1000); // Holds down "Left" key for 1 second
		/// </code>
		/// </example>
		/// <param name="key">
		///     key to hold
		/// </param>
		/// <param name="duration">
		///     the duration
		/// </param>
		public static void HoldKey(Keys key, int duration = 500)
		{
			const int KeyDownEvent = 0x0001;
			const int KeyUpEvent = 0x0002;

			NativeMethods.keybd_event((byte)key, 0, KeyDownEvent, 0);
			Thread.Sleep(duration);
			NativeMethods.keybd_event((byte)key, 0, KeyUpEvent, 0);
		}

		/// <summary>
		///     test if a key is pressed
		/// </summary>
		/// <example>
		///     <code>
		/// // test if F1 is currently pressed
		/// bool pressed = Keyboard.IsKeyDown(Keys.F1);
		/// </code>
		/// </example>
		/// <param name="key">
		///     the key(s)
		/// </param>
		/// <returns>
		///     true or false
		/// </returns>
		public static bool IsKeyDown(Keys key)
		{
			return NativeMethods.GetAsyncKeyState(key) == short.MinValue;
		}
	}
}