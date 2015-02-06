// -----------------------------------------------------------------------
//  <copyright file="HotKey.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Input
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using Logging;

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

		private Keys _keys;

		private int _id;

		private bool _disposed;

		/// <summary>
		///     Creates a new instance of the <see cref="HotKey" /> class
		/// </summary>
		protected HotKey()
		{
		}

		/// <summary>
		///     Registers the hotkey. You have to keep a reference to the returned object.
		/// </summary>
		/// <param name="keys">The keys that will work as the global hotkey.</param>
		/// <returns>The registered hotkey.</returns>
		public static HotKey Register(Keys keys)
		{
			HotKey hotKey = new HotKey { _keys = keys };
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
				return this._keys;
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
			if(this._disposed)
			{
				return;
			}
			this._disposed = true;
			Wnd.HotKeyUnRegister(this);
		}

		private class Wnd : Control
		{
			private const int MOD_ALT = 0x1;
			private const int MOD_CONTROL = 0x2;
			private const int MOD_SHIFT = 0x4;
			private const int WM_HOTKEY = 0x312;

			private static Wnd _def;
			private readonly List<IntPtr> _hotkeys = new List<IntPtr>();

			private Wnd()
			{
				this.Visible = false;
			}

			private static Wnd Default
			{
				get
				{
					if(_def == null)
					{
						_def = new Wnd();
						_def.CreateHandle();
					}

					return _def;
				}
			}

			private int GetNewId(IntPtr item)
			{
				int i = 0;
				foreach(IntPtr r in this._hotkeys)
				{
					if((long)r == 0)
					{
						this._hotkeys[i] = item;
						return i;
					}

					i++;
				}

				this._hotkeys.Add(item);
				return i;
			}

			private IntPtr GetObject(int id)
			{
				return this._hotkeys[id];
			}

			private void RemoveId(int id)
			{
				this._hotkeys[id] = (IntPtr)0;
			}

			protected override void WndProc(ref Message m)
			{
				if(m.Msg == WM_HOTKEY)
				{
					HotKey h = (HotKey)GCHandle.FromIntPtr(this.GetObject((int)m.WParam)).Target;
					if(h.HotKeyPressed != null)
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
				h._id = Default.GetNewId(GCHandle.ToIntPtr(GCHandle.Alloc(h, GCHandleType.WeakTrackResurrection)));
				int modifiers = 0;
				if((h._keys & Keys.Alt) == Keys.Alt)
				{
					modifiers = modifiers | MOD_ALT;
				}

				if((h._keys & Keys.Control) == Keys.Control)
				{
					modifiers = modifiers | MOD_CONTROL;
				}

				if((h._keys & Keys.Shift) == Keys.Shift)
				{
					modifiers = modifiers | MOD_SHIFT;
				}

				Keys k = h._keys & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
				RegisterHotKey(Default.Handle, h._id, modifiers, (int)k);
			}

			internal static void HotKeyUnRegister(HotKey h)
			{
				try
				{
					UnregisterHotKey(Default.Handle, h._id);
				}
				catch(Exception exception)
				{
					Logger.LogException(exception);
				}

				GCHandle.FromIntPtr(Default.GetObject(h._id)).Free();
				Default.RemoveId(h._id);
			}
		}
	}
}