using System;
using System.Runtime.InteropServices;

namespace Module1Task3
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        static void Main(string[] args)
        {
            IntPtr hWnd = FindWindow("Notepad", null);

            if (hWnd != IntPtr.Zero)
            {
                SendMessage(hWnd, 0x0010, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
