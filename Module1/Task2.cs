using System;
using System.Runtime.InteropServices;

namespace Module1Task2
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        static void Main(string[] args)
        {
            MessageBox(IntPtr.Zero, "Загадайте число від 0 до 100. Комп'ютер спробує його вгадати.", "Гра: Вгадай число", 0);

            bool playAgain = true;
            while (playAgain)
            {
                int min = 0;
                int max = 100;
                bool guessed = false;

                while (min <= max)
                {
                    int mid = min + (max - min) / 2;
                    int result = MessageBox(IntPtr.Zero, $"Ваше число {mid}?", "Гра: Вгадай число", 4); 
                    
                    if (result == 6) 
                    {
                        MessageBox(IntPtr.Zero, "Ура! Я вгадав ваше число!", "Перемога", 0);
                        guessed = true;
                        break;
                    }
                    else
                    {
                        if (min == max) break;

                        int greater = MessageBox(IntPtr.Zero, $"Ваше число більше за {mid}?", "Гра: Вгадай число", 4);
                        if (greater == 6)
                        {
                            min = mid + 1;
                        }
                        else
                        {
                            max = mid - 1;
                        }
                    }
                }

                if (!guessed)
                {
                    MessageBox(IntPtr.Zero, "Здається, десь була помилка у відповідях...", "Помилка", 0);
                }

                int replay = MessageBox(IntPtr.Zero, "Бажаєте зіграти ще раз?", "Повтор гри", 4);
                if (replay != 6)
                {
                    playAgain = false;
                }
            }
        }
    }
}
