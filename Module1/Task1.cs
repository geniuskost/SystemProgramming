using System;
using System.Runtime.InteropServices;

namespace Module1Task1
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        static void Main(string[] args)
        {
            MessageBox(IntPtr.Zero, "Hello, World!", "Повідомлення", 0);
        }
    }
}
