/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Text;
using System.Diagnostics;

namespace BotSuite
{
    /// <summary>
    /// control extern application by reading values, writing values, (todo) click controls
    /// </summary>
    public class ApplicationTunnel
    {

        #region LOCAL VARIABLES
        protected int ProcessId;
        protected Process AttachedProcess;
        protected ProcessModule AttachedProcessModule;
        protected IntPtr ProcessHandle;
        protected int BaseAddress{get; private set;}
        #endregion

        #region CONSTRUCT AND DESTRUCT
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Id">id of process</param>
        public ApplicationTunnel(int Id)
        {
            ProcessId = Id;
            BaseAddress = 0;
            ProcessHandle = IntPtr.Zero;
            AttachProcess();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ProcessName">name of process</param>
        public ApplicationTunnel(string ProcessName)
        {

            Process[] myProgrammInstances;
            myProgrammInstances = System.Diagnostics.Process.GetProcessesByName(ProcessName);

            if (myProgrammInstances.Length == 0)
                throw new ArgumentNullException();
            ProcessId = myProgrammInstances[0].Id;
            BaseAddress = 0;
            ProcessHandle = IntPtr.Zero;
            AttachProcess();

        }

        /// <summary>
        /// destructor
        /// </summary>
        ~ApplicationTunnel()
        {
            DetachProcess();
        } 
        #endregion

        #region HELPERS
        /// <summary>
        /// get id of process by given name
        /// </summary>
        /// <example>
        /// <code>
        /// Process[] ListOfProcess = Memory.GetProcessIdByName("the name");
        /// </code>
        /// </example>
        /// <param name="name">name of process</param>
        /// <returns></returns>
        public static Process[] GetProcessIdByName(string name)
        {
            return Process.GetProcessesByName(name);
        }

        /// <summary>
        /// returns the ModulBase
        /// </summary>
        /// <param name="ProcName">name of process</param>
        /// <param name="ModuleName">name of modul</param>
        /// <returns></returns>
        public static Int32 GetModuleBase(string ProcName, string ModuleName)
        {
            Int32 BaseAddress = default(Int32);
            foreach (ProcessModule PM in Process.GetProcessesByName(ProcName)[0].Modules)
            {
                if (ModuleName.ToLower() == PM.ModuleName.ToLower())
                {
                    BaseAddress = BaseAddress = (int)PM.BaseAddress; ;
                }
            }
            return BaseAddress;
        }
        /// <summary>
        /// convert a hex string into int
        /// </summary>
        /// <example>
        /// <code>
        /// int result = Memory.Hex2Int"00B28498");
        /// </code>
        /// </example>
        /// <param name="hex">hex</param>
        /// <returns>result as integer </returns>
        public static int Hex2Int(string hex)
        {
            return Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        } 
        #endregion

        #region ProccessAttacher
        /// <summary>
        /// private function to attach a running process
        /// </summary>
        protected void AttachProcess()
        {
            AttachedProcess = Process.GetProcessById(ProcessId);
            NativeMethods.ProcessAccessType AccessFlags = NativeMethods.ProcessAccessType.PROCESS_VM_READ
                                                        | NativeMethods.ProcessAccessType.PROCESS_VM_WRITE
                                                        | NativeMethods.ProcessAccessType.PROCESS_VM_OPERATION;

            ProcessHandle = NativeMethods.OpenProcess((uint)AccessFlags, 1, (uint)ProcessId);
            AttachedProcessModule = AttachedProcess.MainModule;
            this.BaseAddress = (int)AttachedProcessModule.BaseAddress;
        }
        /// <summary>
        /// private function to detach a running process
        /// </summary>
        protected void DetachProcess()
        {
            int closeHandleReturn;
            closeHandleReturn = NativeMethods.CloseHandle(ProcessHandle);
            if (closeHandleReturn == 0)
            {
                //Code Zur Fehler Bearbeitung
            }
        } 
        #endregion




        #region READ MEMORY
        /// <summary>
        /// private function to read memory
        /// </summary>
        /// <param name="MemoryAddress">address</param>
        /// <param name="bytesToRead">bytes</param>
        /// <param name="bytesRead">result</param>
        /// <returns></returns>
        protected byte[] ReadMemoryAtAdress(IntPtr MemoryAddress, uint bytesToRead, out int bytesRead)
        {
            byte[] buffer = new byte[bytesToRead];
            IntPtr ptrBytesRead;
            NativeMethods.ReadProcessMemory(ProcessHandle, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);
            bytesRead = ptrBytesRead.ToInt32();
            return buffer;
        }
        /// <summary>
        /// read a value at a adress
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // direct access at 00B28498
        /// int MyValue1 = Trainer.Read<int>("00B28498");
        /// // direct access at "001AAAC4", 0x464
        /// int MyValue2 = Trainer.Read<int>("001AAAC4", 0x464);
        /// float MyValue1 = Trainer.Read<float>("00B28498");
        /// double MyValue1 = Trainer.Read<double>("00B28498");
        /// uint MyValue1 = Trainer.Read<uint>("00B28498");
        /// ]]>
        /// </code>
        /// </example>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="pAddress">address as string</param>
        /// <param name="relative">relative to baseaddress</param>
        /// <returns>value to read</returns>
        public T Read<T>(string pAddress, params int[] Offsets)
        {
            int off = Int32.Parse(pAddress, System.Globalization.NumberStyles.HexNumber);
            return Read<T>(off, Offsets);
        }

        /// <summary>
        /// Read a value, see other Read-method
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="pAddress">address as integer</param>
        /// <param name="relative">relative to baseaddress</param>
        /// <returns>value to read</returns>
        public T Read<T>(int pAddress, params int[] Offsets)
        {
            IntPtr ReadAdress =(IntPtr)pAddress;
            if (Offsets.Length>0)
                ReadAdress = (IntPtr)this.Pointer(pAddress,Offsets);


            uint pSize;
            byte[] buffer;
            int ptrBytesRead;

            if (typeof(T) == typeof(byte))
            {
                pSize = 1;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((byte)buffer[0]);
            }
            else if (typeof(T) == typeof(short))
            {
                pSize = 2;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((Int32)BitConverter.ToInt16(buffer, 0));
            }
            else if (typeof(T) == typeof(int))
            {
                pSize = 4;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((Int32)BitConverter.ToInt32(buffer, 0));
            }
            else if (typeof(T) == typeof(uint))
            {
                pSize = 4;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((Int32)BitConverter.ToUInt32(buffer, 0));
            }
            else if (typeof(T) == typeof(float))
            {
                pSize = 4;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((float)BitConverter.ToSingle(buffer, 0));
            }
            else if (typeof(T) == typeof(double))
            {
                pSize = 8;
                buffer = new byte[pSize];
                buffer = buffer = ReadMemoryAtAdress(ReadAdress, pSize, out ptrBytesRead);
                return (T)(object)((double)BitConverter.ToDouble(buffer, 0));
            }
            return default(T);
        } 
        #endregion

        #region WRITE AT MEMORY
        /// <summary>
        /// private function to write at memory
        /// </summary>
        /// <param name="MemoryAddress">address</param>
        /// <param name="bytesToWrite">bytes to write</param>
        /// <returns></returns>
        protected int WriteMemoryAtAdress(IntPtr MemoryAddress, byte[] bytesToWrite)
        {
            IntPtr ptrBytesWritten;
            NativeMethods.WriteProcessMemory(ProcessHandle, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);
            return ptrBytesWritten.ToInt32();
        }
        /// <summary>
        /// write a value at memory
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Memory Trainer = new Memory(...); 
        /// // direct access at 00B28498 
        /// Trainer.Write<int>("00B28498", an integer);
        /// // follow pointer access at "001AAAC4", 0x464
        /// Trainer.Write<int>("001AAAC4", an integer,0x464);
        /// Trainer.Write<float>("00B28498", a float var);
        /// Trainer.Write<double>("00B28498", a double var);
        /// Trainer.Write<uint>("00B28498", an unsigned integer);
        /// ]]>
        /// </code>
        /// </example>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="pAddress">address to write</param>
        /// <param name="WriteData">data to write</param>
        /// <param name="Offsets">offsets</param>
        public void Write<T>(string pAddress, T WriteData, params int[] Offsets)
        {
            int off = Int32.Parse(pAddress, System.Globalization.NumberStyles.HexNumber);
            Write<T>(off, WriteData, Offsets);
        }
        /// <summary>
        /// write a t memory
        /// </summary>
        /// <typeparam name="T">type of data to write</typeparam>
        /// <param name="pAddress">address to write</param>
        /// <param name="WriteData">data to write</param>
        /// <param name="Offsets">offsets</param>
        public void Write<T>(int pAddress, T WriteData, params int[] Offsets)
        {
            IntPtr WriteAdress = (IntPtr)pAddress;
            if (Offsets.Length > 0)
                WriteAdress = (IntPtr)this.Pointer(pAddress, Offsets);

            if (typeof(T) == typeof(byte))
            {
                WriteMemoryAtAdress(WriteAdress, BitConverter.GetBytes(Convert.ToInt16(WriteData)));
            }
            else if (typeof(T) == typeof(double))
            {
                WriteMemoryAtAdress(WriteAdress, BitConverter.GetBytes(Convert.ToDouble(WriteData)));
            }
            else if (typeof(T) == typeof(float))
            {
                WriteMemoryAtAdress(WriteAdress, BitConverter.GetBytes(Convert.ToSingle(WriteData)));
            }
            else if (typeof(T) == typeof(int))
            {
                WriteMemoryAtAdress(WriteAdress, BitConverter.GetBytes(Convert.ToInt32(WriteData)));
            }
        }

        /// <summary>
        /// Write a string of ASCII
        /// </summary>
        /// <param name="pAddress">address to write</param>
        /// <param name="StringToWrite">string to write</param>
        public void WriteAscii(int pAddress, string StringToWrite)
        {
            WriteMemoryAtAdress((IntPtr) pAddress, Encoding.ASCII.GetBytes(StringToWrite + "\0"));
        }
        /// <summary>
        /// Writes a unicode string
        /// </summary>
        /// <param name="pAddress">address to write</param>
        /// <param name="StringToWrite">string to write</param>
        public void WriteUnicode(int pAddress, string StringToWrite)
        {
            WriteMemoryAtAdress((IntPtr)pAddress, Encoding.Unicode.GetBytes(StringToWrite + "\0"));
        }
        #endregion

        #region FOLLOW POINTERS
        /// <summary>
        /// follow a pointer by start address
        /// </summary>
        /// <example>
        /// <code>
        /// // start in BaseAddress add follow the pointers by adding the offsets
        /// int MyPointer2 = Trainer.Pointer( 0x284, 0xE4, 0xE4, 0x30, 0x108);
        /// </code>
        /// </example>
        /// <param name="start">start address</param>
        /// <param name="Offsets">offsets</param>
        /// <returns></returns>
        public int Pointer(int start, params int[] Offsets)
        {
            if (Offsets.Length > 0)
            {
                //target = this.Read<int>(pAddress);
                for (int i = 0; i < Offsets.Length; i++)
                {
                    start = this.Read<int>(start);
                    start += Offsets[i];
                }
            }
            return start;
        } 
        
        /// <summary>
        /// follow a pointer by start address
        /// </summary>
        /// <example>
        /// <code>
        /// // start in 00B28498 add follow the pointers by adding the offsets
        /// int MyPointer2 = Trainer.Pointer("00B28498", 0x284, 0xE4, 0xE4, 0x30, 0x108);
        /// </code>
        /// </example>
        /// <param name="start">start address</param>
        /// <param name="Offsets">offsets</param>
        /// <returns></returns>
        protected int Pointer(string start, params int[] Offsets)
        {
            int target = Hex2Int(start);
            return this.Pointer(target, Offsets); 
        }
        #endregion
        /// <summary>
        /// returns handle of extern process
        /// </summary>
        /// <returns></returns>
        public IntPtr GetHandle()
        {
            return ProcessHandle;
        }
        /// <summary>
        /// tries to close the main window of process
        /// </summary>
        public void Close()
        {
            AttachedProcess.CloseMainWindow();
            AttachedProcess.WaitForExit(4000);
            if (!AttachedProcess.HasExited)
            {
                Kill();
            }
            else
            {
                AttachedProcess.Dispose();
            }
            
        }
        /// <summary>
        /// kills radical the process
        /// </summary>
        public void Kill()
        {
            try
            {
                AttachedProcess.Kill();
                AttachedProcess.WaitForExit();
            }
            catch { }
        }
    }
}


/*

// Trainer erstellen
Memory Trainer = new Memory("ck2");
// direkter Zugriff auf Speicherstelle 00B28498 + BaseAddress
int MyValue1 = Trainer.Read<int>("00B28498");
// direkter Zugriff auf Speicherstelle 00B28498
int MyValue2 = Trainer.Read<int>("00B28498", false);
// Zeiger ablaufen von der Base Adress
int MyPointer1 = Trainer.Pointer(0x284, 0xE4, 0xE4, 0x30, 0x108);
// Zeiger ablaufen von manuellen Startpunkt
int MyPointer2 = Trainer.Pointer("00B28498", 0x284, 0xE4, 0xE4, 0x30, 0x108);

int WriteValue = 123456789;
Trainer.Write(MyPointer2, WriteValue);

*/