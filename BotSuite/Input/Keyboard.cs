// -----------------------------------------------------------------------
//  <copyright file="Keyboard.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BotSuite.Input
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Windows.Forms;
	using Win32;
	using Win32.Methods;

	/// <summary>
	///     A class that manages a global low level keyboard hook
	/// </summary>
	public class Keyboard
	{
		/// <summary>
		///     The instance.
		/// </summary>
		private static readonly Keyboard _instance;

		/// <summary>
		///     The collections of keys to watch for
		/// </summary>
		private static readonly List<Keys> _hookedKeys = new List<Keys>();

		/// <summary>
		///     Handle to the hook, need this to unhook and call the next hook
		/// </summary>
		private IntPtr _hhook = IntPtr.Zero;

		/// <summary>
		///     Occurs when one of the hooked keys is pressed
		/// </summary>
		public static event KeyEventHandler KeyDown;

		/// <summary>
		///     Occurs when one of the hooked keys is released
		/// </summary>
		public static event KeyEventHandler KeyUp;

		/// <summary>
		///     Initializes static members of the <see cref="Keyboard" /> class.
		/// </summary>
		static Keyboard()
		{
			_instance = new Keyboard();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Keyboard" /> class and installs the keyboard hook.
		/// </summary>
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
		private static readonly Delegates.KeyboardHookProc _safeDelegateCallback = HookProc;

		/// <summary>
		///		Adds a key to the hooked keys list
		/// </summary>
		/// <param name="key">The key to be added</param>
		public static void HookKey(Keys key)
		{
			if(!_hookedKeys.Contains(key))
			{
				_hookedKeys.Add(key);
			}
		}

		/// <summary>
		///		Removes a key from the hooked keys list
		/// </summary>
		/// <param name="key">The key to be removed</param>
		public static void UnhookKey(Keys key)
		{
			if(_hookedKeys.Contains(key))
			{
				_hookedKeys.Remove(key);
			}
		}

		/// <summary>
		///     The hook.
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Hook()
		{
			using(Process curProcess = Process.GetCurrentProcess())
			{
				using(ProcessModule curModule = curProcess.MainModule)
				{
					this._hhook = User32.SetWindowsHookEx(Constants.WH_KEYBOARD_LL, _safeDelegateCallback, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
				}
			}
		}

		/// <summary>
		///     removes the global hook
		/// </summary>
		/// <remarks>
		///     this is a private method. You cannot us it! See the constructor #ctor for an example.
		/// </remarks>
		private void Unhook()
		{
			User32.UnhookWindowsHookEx(this._hhook);
		}

		/// <summary>
		///     callback for the keyboard hook
		/// </summary>
		/// <param name="code">
		///     hook code, do sth iff &gt;=0
		/// </param>
		/// <param name="wParam">
		///     event type
		/// </param>
		/// <param name="lParam">
		///     keyhook event information
		/// </param>
		/// <remarks>
		///     this is a private method. You cannot use it! See the constructor #ctor for an example.
		/// </remarks>
		/// <returns>
		///     the int
		/// </returns>
		private static IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
		{
			if(code < 0)
			{
				return User32.CallNextHookEx(_instance._hhook, code, wParam, lParam);
			}

			Keys key = (Keys)Marshal.ReadInt32(lParam);
			if(_hookedKeys == null)
			{
				return User32.CallNextHookEx(_instance._hhook, code, wParam, lParam);
			}

			if(!_hookedKeys.Contains(key))
			{
				return User32.CallNextHookEx(_instance._hhook, code, wParam, lParam);
			}

			KeyEventArgs kea = new KeyEventArgs(key);
			if((wParam.ToInt32() == Constants.WM_KEYDOWN || wParam.ToInt32() == Constants.WM_SYSKEYDOWN) && (KeyDown != null))
			{
				KeyDown(_instance, kea);
			}
			else if((wParam.ToInt32() == Constants.WM_KEYUP || wParam.ToInt32() == Constants.WM_SYSKEYUP) && (KeyUp != null))
			{
				KeyUp(_instance, kea);
			}

			return kea.Handled ? (IntPtr)1 : User32.CallNextHookEx(_instance._hhook, code, wParam, lParam);
		}

		/// <summary>
		///     types a key or a sequence of keys
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
			const int KEY_DOWN_EVENT = 0x0100;
			const int KEY_UP_EVENT = 0x0101;
			const int CHAR_EVENT = 0x0102;

			// send key down event
			if(User32.SendMessage(hwnd, KEY_DOWN_EVENT, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send character event
			if(User32.SendMessage(hwnd, CHAR_EVENT, (uint)key, GetLParam(1, key, 0, 0, 0, 0)))
			{
				return;
			}

			Utility.Delay(20);

			// send key up event
			if(User32.SendMessage(hwnd, KEY_UP_EVENT, (uint)key, GetLParam(1, key, 0, 0, 1, 1)))
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
			User32.keybd_event(key, 0x45, Constants.KEY_DOWN_EVENT, 0);
			Thread.Sleep(duration);
			User32.keybd_event(key, 0x45, Constants.KEY_DOWN_EVENT | Constants.KEY_UP_EVENT, 0);
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
			short state = User32.GetAsyncKeyState(key);
			return state == short.MinValue || state == 1;
		}
	}
}