using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Tibb.Util;
using System.Windows.Forms;

namespace Tibb.Clients
{
    class ClientMC
    {
        public static Boolean isInTibiaFolder()
        {
            string myPath = getCurrentDirectory();
            string tibiaPath = Path.Combine(myPath, "Tibia.exe");
            if (File.Exists(tibiaPath))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public static String getCurrentDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }

        public ClientMC(Process p)
        {
            process = p;
            process.WaitForInputIdle();

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                process.Refresh();
                System.Threading.Thread.Sleep(5);
            }
            processHandle = WinApi.OpenProcess(WinApi.PROCESS_ALL_ACCESS, 0, (uint)process.Id);
        }

        public static ClientMC OpenMC()
        {
            string myPath = getCurrentDirectory();
            string tibiaPath = Path.Combine(myPath, "Tibia.exe");
            return OpenMC(tibiaPath, ""); 
            //return OpenMC(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Tibia\tibia.exe"), "");
        }

        public static ClientMC OpenMC(string path, string arguments)
        {
            WinApi.PROCESS_INFORMATION pi = new WinApi.PROCESS_INFORMATION();
            WinApi.STARTUPINFO si = new WinApi.STARTUPINFO();

            if (arguments == null)
                arguments = "";

            if (!WinApi.CreateProcess(path, " " + arguments, IntPtr.Zero, IntPtr.Zero,
                false, WinApi.CREATE_SUSPENDED, IntPtr.Zero, System.IO.Path.GetDirectoryName(path), ref si, out pi))
                return null;


            uint baseAddress = 0;
            IntPtr hThread = WinApi.CreateRemoteThread(pi.hProcess, IntPtr.Zero, 0,
                                    WinApi.GetProcAddress(WinApi.GetModuleHandle("Kernel32"), "GetModuleHandleA"), IntPtr.Zero, 0, IntPtr.Zero);
            if (hThread == IntPtr.Zero)
            {
                WinApi.CloseHandle(pi.hProcess);
                WinApi.CloseHandle(pi.hThread);
                return null;
            }

            WinApi.WaitForSingleObject(hThread, 0xFFFFFFFF);
            WinApi.GetExitCodeThread(hThread, out baseAddress);
            WinApi.CloseHandle(hThread);

            if (baseAddress == 0)
            {
                WinApi.CloseHandle(pi.hProcess);
                WinApi.CloseHandle(pi.hThread);
                return null;
            }

            IntPtr handle = WinApi.OpenProcess(WinApi.PROCESS_ALL_ACCESS, 0, pi.dwProcessId);
            if (handle == IntPtr.Zero)
            {
                WinApi.CloseHandle(pi.hProcess);
                WinApi.CloseHandle(pi.hThread);
                return null;
            }

            var process = Process.GetProcessById(Convert.ToInt32(pi.dwProcessId));
            Util.Memory.WriteByte(handle, ClientMC.MultiClient, ClientMC.MultiClientJMP);
            WinApi.ResumeThread(pi.hThread);
            process.WaitForInputIdle();
            Util.Memory.WriteByte(handle, ClientMC.MultiClient, ClientMC.MultiClientJNZ);


            WinApi.CloseHandle(pi.hProcess);
            WinApi.CloseHandle(pi.hThread);
            WinApi.CloseHandle(handle);


            return new ClientMC(process);
        }

        private Process process;
        private IntPtr processHandle;
        private static long MultiClient = 0x534610;
        private static byte MultiClientJMP = 0xEB;
        private static byte MultiClientJNZ = 0x75;
    }
}