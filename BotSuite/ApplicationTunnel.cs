// -----------------------------------------------------------------------
//  <copyright file="ApplicationTunnel.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Logging;
	using Win32;
	using Win32.Methods;

	/// <summary>
	///     control external applications by reading/writing values
	/// </summary>
	public class ApplicationTunnel
	{
		/// <summary>
		///     intern id of process
		/// </summary>
		private readonly int _processId;

		/// <summary>
		///     process data
		/// </summary>
		private Process _attachedProcess;

		/// <summary>
		///     handle of modul
		/// </summary>
		private ProcessModule _attachedProcessModule;

		/// <summary>
		///     handle of process
		/// </summary>
		private IntPtr _processHandle;

		/// <summary>
		///     Gets the base address.
		/// </summary>
		protected int BaseAddress { get; private set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="ApplicationTunnel" /> class.
		/// </summary>
		/// <param name="id">
		///     id of process
		/// </param>
		public ApplicationTunnel(int id)
		{
			this._processId = id;
			this.BaseAddress = 0;
			this._processHandle = IntPtr.Zero;
			this.AttachProcess();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ApplicationTunnel" /> class.
		/// </summary>
		/// <param name="processName">
		///     name of process
		/// </param>
		public ApplicationTunnel(string processName)
		{
			Process[] programmInstances = Process.GetProcessesByName(processName);

			if(programmInstances.Length == 0)
			{
				throw new Exception(String.Format("No process found with given name \"{0}\"", processName));
			}

			this._processId = programmInstances.First().Id;
			this.BaseAddress = 0;
			this._processHandle = IntPtr.Zero;
			this.AttachProcess();
		}

		/// <summary>
		///     Finalizes an instance of the <see cref="ApplicationTunnel" /> class.
		/// </summary>
		~ApplicationTunnel()
		{
			this.DetachProcess();
		}

		/// <summary>
		///     get id of process by given name
		/// </summary>
		/// <param name="name">
		///     name of process
		/// </param>
		/// <returns>
		///     an array of processes
		/// </returns>
		public static Process[] GetProcessIdByName(string name)
		{
			return Process.GetProcessesByName(name);
		}

		/// <summary>
		///     Gets the module base.
		/// </summary>
		/// <param name="procName">
		///     Name of the proc.
		/// </param>
		/// <param name="moduleName">
		///     Name of the module.
		/// </param>
		/// <returns>
		///     the module base
		/// </returns>
		public static int GetModuleBase(string procName, string moduleName)
		{
			return (from ProcessModule pm in Process.GetProcessesByName(procName)[0].Modules
					where string.Equals(moduleName, pm.ModuleName, StringComparison.CurrentCultureIgnoreCase)
					select (int)pm.BaseAddress).FirstOrDefault();
		}

		/// <summary>
		///     convert a hex string into int
		/// </summary>
		/// <param name="hex">
		///     the hex string
		/// </param>
		/// <returns>
		///     result as integer
		/// </returns>
		public static int Hex2Int(string hex)
		{
			return int.Parse(hex, NumberStyles.HexNumber);
		}

		/// <summary>
		///     private function to attach a running process
		/// </summary>
		protected void AttachProcess()
		{
			this._attachedProcess = Process.GetProcessById(this._processId);
			const Constants.ProcessAccessType ACCESS_FLAGS =
				Constants.ProcessAccessType.ProcessVmRead
				| Constants.ProcessAccessType.ProcessVmWrite
				| Constants.ProcessAccessType.ProcessVmOperation;

			this._processHandle = Kernel32.OpenProcess((uint)ACCESS_FLAGS, 1, (uint)this._processId);
			this._attachedProcessModule = this._attachedProcess.MainModule;
			this.BaseAddress = (int)this._attachedProcessModule.BaseAddress;
		}

		/// <summary>
		///     private function to detach a running process
		/// </summary>
		protected void DetachProcess()
		{
			int closeHandleReturn = Kernel32.CloseHandle(this._processHandle);
			if(closeHandleReturn == 0)
			{
				// TODO: Code Zur Fehler Bearbeitung
			}
		}

		/// <summary>
		///     private function to read memory
		/// </summary>
		/// <param name="memoryAddress">
		///     the address
		/// </param>
		/// <param name="bytesToRead">
		///     the byte count to read
		/// </param>
		/// <param name="bytesRead">
		///     the byte count read
		/// </param>
		/// <returns>
		///     the value at the given address
		/// </returns>
		protected byte[] ReadMemoryAtAdress(IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
		{
			byte[] buffer = new byte[bytesToRead];
			IntPtr ptrBytesRead;
			Kernel32.ReadProcessMemory(this._processHandle, memoryAddress, buffer, bytesToRead, out ptrBytesRead);
			bytesRead = ptrBytesRead.ToInt32();
			return buffer;
		}

		/// <summary>
		///     read a value at a adress
		/// </summary>
		/// <typeparam name="T">
		///     type of value
		/// </typeparam>
		/// <param name="address">
		///     address as string
		/// </param>
		/// <param name="offsets">
		///     offsets to follow to get value
		/// </param>
		/// <returns>
		///     value to read
		/// </returns>
		public T Read<T>(string address, params int[] offsets)
		{
			int off = int.Parse(address, NumberStyles.HexNumber);
			return this.Read<T>(off, offsets);
		}

		/// <summary>
		///     Read a value, see other Read-method
		/// </summary>
		/// <typeparam name="T">
		///     type of value
		/// </typeparam>
		/// <param name="address">
		///     address as integer
		/// </param>
		/// <param name="offsets">
		///     offsets to follow to get value
		/// </param>
		/// <returns>
		///     value to read
		/// </returns>
		public T Read<T>(int address, params int[] offsets)
		{
			IntPtr readAdress = (IntPtr)address;
			if(offsets.Length > 0)
			{
				readAdress = (IntPtr)this.Pointer(address, offsets);
			}

			uint size;
			byte[] buffer;
			int ptrBytesRead;

			if(typeof(T) == typeof(byte))
			{
				size = 1;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)buffer[0];
			}

			if(typeof(T) == typeof(short))
			{
				size = 2;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)((int)BitConverter.ToInt16(buffer, 0));
			}

			if(typeof(T) == typeof(int))
			{
				size = 4;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)BitConverter.ToInt32(buffer, 0);
			}

			if(typeof(T) == typeof(uint))
			{
				size = 4;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)((int)BitConverter.ToUInt32(buffer, 0));
			}

			if(typeof(T) == typeof(float))
			{
				size = 4;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)BitConverter.ToSingle(buffer, 0);
			}

			if(typeof(T) == typeof(double))
			{
				size = 8;
				buffer = this.ReadMemoryAtAdress(readAdress, size, out ptrBytesRead);
				return (T)(object)BitConverter.ToDouble(buffer, 0);
			}

			return default(T);
		}

		/// <summary>
		///     private function to write at memory
		/// </summary>
		/// <param name="memoryAddress">
		///     the address
		/// </param>
		/// <param name="bytesToWrite">
		///     bytes to write
		/// </param>
		/// <returns>
		///     count of bytes written
		/// </returns>
		protected int WriteMemoryAtAdress(IntPtr memoryAddress, byte[] bytesToWrite)
		{
			IntPtr ptrBytesWritten;
			Kernel32.WriteProcessMemory(
				this._processHandle,
				memoryAddress,
				bytesToWrite,
				(uint)bytesToWrite.Length,
				out ptrBytesWritten);
			return ptrBytesWritten.ToInt32();
		}

		/// <summary>
		///     write a value at memory
		/// </summary>
		/// <typeparam name="T">
		///     type of value
		/// </typeparam>
		/// <param name="address">
		///     address to write
		/// </param>
		/// <param name="writeData">
		///     data to write
		/// </param>
		/// <param name="offsets">
		///     the offsets
		/// </param>
		public void Write<T>(string address, T writeData, params int[] offsets)
		{
			int off = int.Parse(address, NumberStyles.HexNumber);
			this.Write(off, writeData, offsets);
		}

		/// <summary>
		///     write at memory
		/// </summary>
		/// <typeparam name="T">
		///     type of data to write
		/// </typeparam>
		/// <param name="address">
		///     address to write
		/// </param>
		/// <param name="writeData">
		///     data to write
		/// </param>
		/// <param name="offsets">
		///     the offsets
		/// </param>
		public void Write<T>(int address, T writeData, params int[] offsets)
		{
			IntPtr writeAdress = (IntPtr)address;
			if(offsets.Length > 0)
			{
				writeAdress = (IntPtr)this.Pointer(address, offsets);
			}

			if(typeof(T) == typeof(byte))
			{
				this.WriteMemoryAtAdress(writeAdress, BitConverter.GetBytes(Convert.ToInt16(writeData)));
			}
			else if(typeof(T) == typeof(double))
			{
				this.WriteMemoryAtAdress(writeAdress, BitConverter.GetBytes(Convert.ToDouble(writeData)));
			}
			else if(typeof(T) == typeof(float))
			{
				this.WriteMemoryAtAdress(writeAdress, BitConverter.GetBytes(Convert.ToSingle(writeData)));
			}
			else if(typeof(T) == typeof(int))
			{
				this.WriteMemoryAtAdress(writeAdress, BitConverter.GetBytes(Convert.ToInt32(writeData)));
			}
		}

		/// <summary>
		///     Write a string of ASCII
		/// </summary>
		/// <param name="address">
		///     address to write
		/// </param>
		/// <param name="stringToWrite">
		///     string to write
		/// </param>
		public void WriteAscii(int address, string stringToWrite)
		{
			this.WriteMemoryAtAdress((IntPtr)address, Encoding.ASCII.GetBytes(stringToWrite + "\0"));
		}

		/// <summary>
		///     Writes a unicode string
		/// </summary>
		/// <param name="address">
		///     address to write
		/// </param>
		/// <param name="stringToWrite">
		///     string to write
		/// </param>
		public void WriteUnicode(int address, string stringToWrite)
		{
			this.WriteMemoryAtAdress((IntPtr)address, Encoding.Unicode.GetBytes(stringToWrite + "\0"));
		}

		/// <summary>
		///     follow a pointer by start address
		/// </summary>
		/// <param name="start">
		///     start address
		/// </param>
		/// <param name="offsets">
		///     the offsets
		/// </param>
		/// <returns>
		///     a pointer
		/// </returns>
		public int Pointer(int start, params int[] offsets)
		{
			if(offsets.Length <= 0)
			{
				return start;
			}

			// target = this.Read<int>(pAddress);
			foreach(int offset in offsets)
			{
				start = this.Read<int>(start);
				start += offset;
			}

			return start;
		}

		/// <summary>
		///     follow a pointer by start address
		/// </summary>
		/// <param name="start">
		///     start address
		/// </param>
		/// <param name="offsets">
		///     the offsets
		/// </param>
		/// <returns>
		///     a pointer
		/// </returns>
		protected int Pointer(string start, params int[] offsets)
		{
			int target = Hex2Int(start);
			return this.Pointer(target, offsets);
		}

		/// <summary>
		///     returns handle of extern process
		/// </summary>
		/// <returns>a handle</returns>
		public IntPtr GetHandle()
		{
			return this._processHandle;
		}

		/// <summary>
		///     tries to close the main window of process
		/// </summary>
		public void Close()
		{
			this._attachedProcess.CloseMainWindow();
			this._attachedProcess.WaitForExit(4000);

			if(!this._attachedProcess.HasExited)
			{
				this.Kill();
			}
			else
			{
				this._attachedProcess.Dispose();
			}
		}

		/// <summary>
		///     kills radical the process
		/// </summary>
		public void Kill()
		{
			try
			{
				this._attachedProcess.Kill();
				this._attachedProcess.WaitForExit();
			}
			catch(Exception exception)
			{
				Logger.LogException(exception);
			}
		}
	}
}
