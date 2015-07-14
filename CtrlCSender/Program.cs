using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CtrlCSender
{
    class Program
    {
        // Enumerated type for the control messages sent to the handler routine
        public enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [DllImport("kernel32.dll")]
        public static extern bool GenerateConsoleCtrlEvent(
            uint dwCtrlEvent,
            int dwProcessGroupId);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);

        [STAThread]
        static int Main(string[] args)
        {
            int pid;
            if (!int.TryParse(args[0], out pid))
                return -1;

            FreeConsole();
            if (AttachConsole(pid))
            {
                SetConsoleCtrlHandler(null, true);
                try
                {
                    if (!GenerateConsoleCtrlEvent((uint)CtrlTypes.CTRL_C_EVENT, 0))
                        return -1;
                }
                finally
                {
                }
                return 0;
            }
            string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
            return -1;
        }
    }
}
