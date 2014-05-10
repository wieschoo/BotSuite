// -----------------------------------------------------------------------
//  <copyright file="Keyboard.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Input
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Windows.Forms;

	using BotSuite.Native;
	using BotSuite.Native.Methods;
	using BotSuite.Native.Structs;

	/// <summary>
	///     A class that manages a global low level keyboard hook
	/// </summary>
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
		private static readonly List<Keys> HookedKeys = new List<Keys>();

		/// <summary>
		///     Handle to the hook, need this to unhook and call the next hook
		/// </summary>
		private IntPtr hhook = IntPtr.Zero;

		/// <summary>
		///     Occurs when one of the hooked keys is pressed
		/// </summary>
		public static event KeyEventHandler KeyDown;

		/// <summary>
		///     Occurs when one of the hooked keys is released
		/// </summary>
		public static event KeyEventHandler KeyUp;

		/// <summary>
		///     Initializes a new instance of the <see cref="Keyboard" /> class and installs the keyboard hook.
		/// </summary>
		/// <remarks>
		///     use the whole class by the code from the example!
		/// </remarks>
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
		private static readonly Delegates.KeyboardHookProc SafeDelegateCallback = HookProc;

		/// <summary>
		///     The hook.
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Hook()
		{
			IntPtr instance = Kernel32.LoadLibrary("User32");
			this.hhook = User32.SetWindowsHookEx(Constants.WhKeyboardLl, SafeDelegateCallback, instance, 0);
		}

		/// <summary>
		///     removes the global hook
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Unhook()
		{
			User32.UnhookWindowsHookEx(this.hhook);
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
		///     this is a private method. You cannot use it! See the constructor #ctor for an example.
		/// </remarks>
		/// <returns>
		///     the int
		/// </returns>
		private static int HookProc(int code, int wparam, ref KeyboardHookStruct lparam)
		{
			if (code < 0)
			{
				return User32.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			Keys key = (Keys)lparam.VkCode;
			if (HookedKeys == null)
			{
				return User32.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			if (!HookedKeys.Contains(key))
			{
				return User32.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
			}

			KeyEventArgs kea = new KeyEventArgs(key);
			if ((wparam == Constants.WmKeydown || wparam == Constants.WmSyskeydown) && (KeyDown != null))
			{
				KeyDown(Instance, kea);
			}
			else if ((wparam == Constants.WmKeyup || wparam == Constants.WmSyskeyup) && (KeyUp != null))
			{
				KeyUp(Instance, kea);
			}

			return kea.Handled ? 1 : User32.CallNextHookEx(Instance.hhook, code, wparam, ref lparam);
		}

		/// <summary>
		///     types a key or o sequence of keys
		/// </summary>
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
		/// <param name="sequence">
		///     Sequence to type
		/// </param>
		/// <param name="hwnd">
		///     target application
		/// </param>
		public static void Type(string sequence, IntPtr hwnd)
		{
			User32.SetForegroundWindow(hwnd);
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
			if (User32.SendMessage(hwnd, KeyDownEvent, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send character event
			if (User32.SendMessage(hwnd, CharEvent, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send key up event
			if (User32.SendMessage(hwnd, KeyUpEvent, (uint)key, GetLParam(1, key, 0, 0, 1, 1)))
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
		///     Press a key
		/// </summary>
		/// <param name="key">
		///     key to press
		/// </param>
		public static void PressKey(Keys key)
		{
			PressKey((byte)key);
		}

		/// <summary>
		///     Press a key
		/// </summary>
		/// <param name="key">
		///     key to press
		/// </param>
		public static void PressKey(byte key)
		{
			HoldKey(key, 50);
		}

		/// <summary>
		///     Hold down a key for a specific time
		/// </summary>
		/// <param name="key">
		///     key to hold
		/// </param>
		/// <param name="duration">
		///     the duration
		/// </param>
		public static void HoldKey(Keys key, int duration = 500)
		{
			HoldKey((byte)key, duration);
		}

		/// <summary>
		///     Hold down a key for a specific time
		/// </summary>
		/// <param name="key">
		///     key to hold
		/// </param>
		/// <param name="duration">
		///     the duration
		/// </param>
		public static void HoldKey(byte key, int duration = 500)
		{
			User32.keybd_event(key, 0x45, Constants.KeyDownEvent, 0);
			Thread.Sleep(duration);
			User32.keybd_event(key, 0x45, Constants.KeyDownEvent | Constants.KeyUpEvent, 0);
		}

		/// <summary>
		///     test if a key is pressed
		/// </summary>
		/// <param name="key">
		///     the key(s)
		/// </param>
		/// <returns>
		///     true or false
		/// </returns>
		public static bool IsKeyDown(Keys key)
		{
			return User32.GetAsyncKeyState(key) == short.MinValue;
		}
	}
}