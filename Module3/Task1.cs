using System;
using System.Threading;

namespace Module3Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            
            using (Timer timer = new Timer(TimerCallback, null, 0, 1000))
            {
                Console.SetCursorPosition(0, 2);
                Console.WriteLine("Натисніть Enter для виходу...");
                Console.ReadLine();
            }
        }

        static void TimerCallback(object state)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, 0);
            
            Console.Write($"Поточний час: {DateTime.Now.ToString("HH:mm:ss")}");
            
            Console.SetCursorPosition(0, 2);
        }
    }
}
