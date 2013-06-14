/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/


using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Linq;
using System.Runtime.InteropServices;





// [ USAGE ] ------------------------------------------------------------------------------
//
// Examples:
//
//      Pattern _sigScan = new Pattern();
//      _sigScan.Process = someProc;
//      _sigScan.Address = new IntPtr(0x123456);
//      _sigScan.Size = 0x1000;
//      IntPtr pAddr = _sigScan.FindPattern(new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF, 0x51, 0x55, 0xFC, 0x11 }, "xxxx?xx?", 12);
//
//      SigScan _sigScan = new SigScan(someProc, new IntPtr(0x123456), 0x1000);
//      IntPtr pAddr = _sigScan.FindPattern(new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF, 0x51, 0x55, 0xFC, 0x11 }, "xxxx?xx?", 12);
//
// ----------------------------------------------------------------------------------------
namespace BotSuite
{
    public class Pattern
    {
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out()] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead
            );

        
        private byte[] m_vDumpedRegion;
        private Process m_vProcess;
        private IntPtr m_vAddress;
        private Int32 m_vSize;
        #region "sigScan Class Construction"
        

        public Pattern()
        {
            this.m_vProcess = null;
            this.m_vAddress = IntPtr.Zero;
            this.m_vSize = 0;
            this.m_vDumpedRegion = null;
        }
        
        public Pattern(Process proc, IntPtr addr, int size)
        {
            this.m_vProcess = proc;
            this.m_vAddress = addr;
            this.m_vSize = size;
        }
        #endregion

        #region "Pattern Private Methods"
        
        private bool DumpMemory()
        {
            try
            {
                // Checks to ensure we have valid data.
                if (this.m_vProcess == null)
                    return false;
                if (this.m_vProcess.HasExited == true)
                    return false;
                if (this.m_vAddress == IntPtr.Zero)
                    return false;
                if (this.m_vSize == 0)
                    return false;

                // Create the region space to dump into.
                this.m_vDumpedRegion = new byte[this.m_vSize];

                bool bReturn = false;
                int nBytesRead = 0;

                // Dump the memory.
                bReturn = ReadProcessMemory(
                    this.m_vProcess.Handle, this.m_vAddress, this.m_vDumpedRegion, this.m_vSize, out nBytesRead
                    );

                // Validation checks.
                if (bReturn == false || nBytesRead != this.m_vSize)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

       
        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            // Loop the pattern and compare to the mask and dump.
            for (int x = 0; x < btPattern.Length; x++)
            {
                // If the mask char is a wildcard, just continue.
                if (strMask[x] == '?')
                    continue;

                // If the mask char is not a wildcard, ensure a match is made in the pattern.
                if ((strMask[x] == 'x') && (btPattern[x] != this.m_vDumpedRegion[nOffset + x]))
                    return false;
            }

            // The loop was successful so we found the pattern.
            return true;
        }
        #endregion

        #region "Pattern Public Methods"
        

        public IntPtr FindPattern(byte[] btPattern, string strMask, int nOffset)
        {
            try
            {
                // Dump the memory region if we have not dumped it yet.
                if (this.m_vDumpedRegion == null || this.m_vDumpedRegion.Length == 0)
                {
                    if (!this.DumpMemory())
                        return IntPtr.Zero;
                }

                // Ensure the mask and pattern lengths match.
                if (strMask.Length != btPattern.Length)
                    return IntPtr.Zero;

                // Loop the region and look for the pattern.
                for (int x = 0; x < this.m_vDumpedRegion.Length; x++)
                {
                    if (this.MaskCheck(x, btPattern, strMask))
                    {
                        // The pattern was found, return it.
                        return new IntPtr((int)this.m_vAddress + (x + nOffset));
                    }
                }

                // Pattern was not found.
                return IntPtr.Zero;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        
        
        public void ResetRegion()
        {
            this.m_vDumpedRegion = null;
        }
        #endregion

        #region "sigScan Class Properties"
        public Process Process
        {
            get { return this.m_vProcess; }
            set { this.m_vProcess = value; }
        }
        public IntPtr Address
        {
            get { return this.m_vAddress; }
            set { this.m_vAddress = value; }
        }
        public Int32 Size
        {
            get { return this.m_vSize; }
            set { this.m_vSize = value; }
        }
        #endregion

    }
}