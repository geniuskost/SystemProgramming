using System;
using System.IO;
using System.Threading;

namespace Module3Task2
{
    class Program
    {
        static bool cancelEncryption = false;

        static void Main(string[] args)
        {
            Console.Write("Введіть шлях до файлу для шифрування: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не знайдено.");
                return;
            }

            Thread encryptThread = new Thread(() => EncryptFile(filePath));
            encryptThread.Start();

            Console.WriteLine("\n[ Шифрування розпочато (Шифр Цезаря) ]");
            Console.WriteLine(">>> Натисніть 'C' для скасування...\n");
            
            while (encryptThread.IsAlive)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.C)
                    {
                        cancelEncryption = true;
                        Console.WriteLine("\n[ Отримано сигнал скасування. Зупинка... ]");
                        break;
                    }
                }
                Thread.Sleep(100);
            }

            encryptThread.Join();
            Console.WriteLine("\nПрограма завершила роботу. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void EncryptFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                char[] buffer = content.ToCharArray();
                int shift = 3;

                int delayChunk = Math.Max(1, buffer.Length / 100);
                
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (cancelEncryption)
                    {
                        Console.WriteLine("\n--> Шифрування скасовано користувачем.");
                        return;
                    }

                    buffer[i] = (char)(buffer[i] + shift);

                    if (buffer.Length > 0 && i % delayChunk == 0)
                    {
                        Thread.Sleep(20);
                    }
                }

                string newPath = filePath + ".encrypted";
                File.WriteAllText(newPath, new string(buffer));
                Console.WriteLine($"\n--> Шифрування завершено успішно!");
                Console.WriteLine($"    Збережено у файл: {newPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПомилка: {ex.Message}");
            }
        }
    }
}
