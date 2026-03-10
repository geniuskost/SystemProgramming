using System;
using System.Threading;

namespace Module5Task1
{
    class BankAccount
    {
        private decimal _balance;
        private readonly object _lockObject = new object();

        public BankAccount(decimal initialBalance)
        {
            _balance = initialBalance;
            Console.WriteLine($"[Ініціалізація] Рахунок створено. Початковий баланс: {_balance:C}");
        }

        public void Deposit(decimal amount, int threadId)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(_lockObject, ref lockTaken);
                _balance += amount;
                Console.WriteLine($"[Потік {threadId}] Додано коштів: {amount:C}. Новий баланс: {_balance:C}");
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lockObject);
                }
            }
        }

        public void Withdraw(decimal amount, int threadId)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(_lockObject, ref lockTaken);
                if (_balance >= amount)
                {
                    _balance -= amount;
                    Console.WriteLine($"[Потік {threadId}] Знято коштів: {amount:C}. Новий баланс: {_balance:C}");
                }
                else
                {
                    Console.WriteLine($"[Потік {threadId}] Спроба зняти {amount:C}. Відмова: недостатньо коштів! Поточний баланс: {_balance:C}");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lockObject);
                }
            }
        }
    }

    class Program
    {
        static BankAccount account;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            account = new BankAccount(1000m);

            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(SimulateBankOperations);
                threads[i].Name = (i + 1).ToString();
                threads[i].Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            Console.WriteLine("\n[Завершення] Всі операції виконано.");
        }

        static void SimulateBankOperations()
        {
            int threadId = int.Parse(Thread.CurrentThread.Name);
            
            for (int i = 0; i < 3; i++)
            {
                bool isDeposit = rnd.Next(0, 2) == 1;
                decimal amount = rnd.Next(50, 500);

                if (isDeposit)
                {
                    account.Deposit(amount, threadId);
                }
                else
                {
                    account.Withdraw(amount, threadId);
                }

                Thread.Sleep(rnd.Next(100, 500));
            }
        }
    }
}
