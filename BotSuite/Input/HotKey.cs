// -----------------------------------------------------------------------
//  <copyright file="HotKey.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
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
	using System.Runtime.InteropServices;
	using System.Windows.Forms;

	using BotSuite.Logging;

	/// <summary>
	///     A class for setting HotKeys
	/// </summary>
	public class HotKey : IDisposable
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		/// <summary>
		///     Rises when the hotkey is pressed
		/// </summary>
		public event EventHandler HotKeyPressed;

		/// <summary>
		///     The tag
		/// </summary>
		public object Tag;

		private Keys keys;

		private int id;

		private bool disposed;

		/// <summary>
		///     Creates a new instance of the <see cref="HotKey" /> class
		/// </summary>
		protected HotKey()
		{
		}

		/// <summary>
		///     Registers the hotkey. You have to keep a reference to the returned object.
		/// </summary>
		/// <param name="keys"></param>
		/// <returns>The registered hotkey.</returns>
		public static HotKey Register(Keys keys)
		{
			HotKey hotKey = new HotKey { keys = keys };
			Wnd.HotKeyRegister(hotKey);
			return hotKey;
		}

		/// <summary>
		///     Calls Dispose: Unregisters the hotkey
		/// </summary>
		/// <param name="hotKey">The Hotkey</param>
		public static void UnRegister(HotKey hotKey)
		{
			hotKey.Dispose();
		}

		/// <summary>
		///     The keycombination
		/// </summary>
		public Keys Keys
		{
			get
			{
				return this.keys;
			}
		}

		/// <summary>
		///	Destructor for the <see cref="HotKey"/> class
		/// </summary>
		~HotKey()
		{
			this.Dispose();
		}

		/// <summary>
		///     Unregisters the Hotkey
		/// </summary>
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			Wnd.HotKeyUnRegister(this);
		}

		private class Wnd : Control
		{
			private static Wnd def;

			private const int ModAlt = 0x1;

			private const int ModControl = 0x2;

			private const int ModShift = 0x4;

			private const int WmHotkey = 0x312;

			private readonly List<IntPtr> hotkeys = new List<IntPtr>();

			private Wnd()
			{
				this.Visible = false;
			}

			private static Wnd Default
			{
				get
				{
					if (def == null)
					{
						def = new Wnd();
						def.CreateHandle();
					}

					return def;
				}
			}

			private int GetNewId(IntPtr item)
			{
				int i = 0;
				foreach (IntPtr r in this.hotkeys)
				{
					if ((long)r == 0)
					{
						this.hotkeys[i] = item;
						return i;
					}

					i++;
				}

				this.hotkeys.Add(item);
				return i;
			}

			private IntPtr GetObject(int id)
			{
				return this.hotkeys[id];
			}

			private void RemoveId(int id)
			{
				this.hotkeys[id] = (IntPtr)0;
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WmHotkey)
				{
					HotKey h = (HotKey)GCHandle.FromIntPtr(this.GetObject((int)m.WParam)).Target;
					if (h.HotKeyPressed != null)
					{
						h.HotKeyPressed(h, null);
					}
				}
				else
				{
					base.WndProc(ref m);
				}
			}

			internal static void HotKeyRegister(HotKey h)
			{
				h.id = Default.GetNewId(GCHandle.ToIntPtr(GCHandle.Alloc(h, GCHandleType.WeakTrackResurrection)));
				int modifiers = 0;
				if ((h.keys & Keys.Alt) == Keys.Alt)
				{
					modifiers = modifiers | ModAlt;
				}

				if ((h.keys & Keys.Control) == Keys.Control)
				{
					modifiers = modifiers | ModControl;
				}

				if ((h.keys & Keys.Shift) == Keys.Shift)
				{
					modifiers = modifiers | ModShift;
				}

				Keys k = h.keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
				RegisterHotKey(Default.Handle, h.id, modifiers, (int)k);
			}

			internal static void HotKeyUnRegister(HotKey h)
			{
				try
				{
					UnregisterHotKey(Default.Handle, h.id);
				}
				catch (Exception exception)
				{
					Logger.LogException(exception);
				}

				GCHandle.FromIntPtr(Default.GetObject(h.id)).Free();
				Default.RemoveId(h.id);
			}
		}
	}
}