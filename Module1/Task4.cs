using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Module1Task4
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        static void Main(string[] args)
        {
            while (true)
            {
                IntPtr hWnd = FindWindow("Notepad", null);

                if (hWnd != IntPtr.Zero)
                {
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    SendMessage(hWnd, 0x000C, IntPtr.Zero, "Блокнот - " + currentTime);
                }

                Thread.Sleep(1000); 
            }
        }
    }
}
