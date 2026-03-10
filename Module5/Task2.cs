using System;
using System.Threading;

namespace Module5Task2
{
    class Program
    {
        static Semaphore semaphore = new Semaphore(3, 3);
        
        static void Main()
        {
            Console.WriteLine("Запуск програми. Створення 10 потоків (не більше 3 одночасно).\n");

            Thread[] threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(Worker);
                threads[i].Name = $"ID-{i + 1}";
                threads[i].Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            Console.WriteLine("\nВсі потоки завершили роботу успішно.");
        }

        static void Worker()
        {
            string threadName = Thread.CurrentThread.Name;

            Console.WriteLine($"[ПОТІК {threadName}] Став у чергу...");
            
            semaphore.WaitOne();

            Console.WriteLine($"---> [ПОТІК {threadName}] ОТРИМАВ ДОСТУП і розпочав роботу!");
            
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string numbers = "";
            for (int i = 0; i < 5; i++)
            {
                numbers += rnd.Next(1, 100) + " ";
                Thread.Sleep(200);
            }
            
            Console.WriteLine($"     [ПОТІК {threadName}] Згенерував числа: {numbers}");
            Console.WriteLine($"<--- [ПОТІК {threadName}] ЗАВЕРШИВ РОБОТУ і звільнив місце.");
            
            semaphore.Release();
        }
    }
}
