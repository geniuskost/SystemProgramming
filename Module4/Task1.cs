using System;
using System.Threading.Tasks;

namespace Module4Task1
{
    class CoffeeCup { }
    class Egg { }
    class Bacon { }
    class Toast { }
    class Juice { }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Починаємо готувати сніданок...\n");

            Task<CoffeeCup> coffeeTask = PourCoffeeAsync();
            Task<Egg> eggsTask = FryEggsAsync(2);
            Task<Bacon> baconTask = FryBaconAsync(3);
            Task<Toast> toastTask = MakeToastWithJamAsync(2);
            Task<Juice> juiceTask = PourJuiceAsync();

            await Task.WhenAll(coffeeTask, eggsTask, baconTask, toastTask, juiceTask);

            Console.WriteLine("\nСніданок готовий!");
        }

        static async Task<CoffeeCup> PourCoffeeAsync()
        {
            Console.WriteLine("Починаємо наливати каву...");
            await Task.Delay(1000);
            Console.WriteLine("Каву налито!");
            return new CoffeeCup();
        }

        static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine($"Починаємо смажити {howMany} яйця...");
            await Task.Delay(3000);
            Console.WriteLine("Яйця готові!");
            return new Egg();
        }

        static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"Починаємо смажити бекон ({slices} шматочків)...");
            await Task.Delay(4000);
            Console.WriteLine("Бекон готовий!");
            return new Bacon();
        }

        static async Task<Toast> MakeToastWithJamAsync(int slices)
        {
            Console.WriteLine($"Починаємо готувати тости ({slices} шт.)...");
            var toast = await ToastBreadAsync(slices);
            await ApplyJamAsync(toast);
            return toast;
        }

        static async Task<Toast> ToastBreadAsync(int slices)
        {
            Console.WriteLine("Смажимо хліб у тостері...");
            await Task.Delay(2000);
            Console.WriteLine("Хліб підсмажено!");
            return new Toast();
        }

        static async Task ApplyJamAsync(Toast toast)
        {
            Console.WriteLine("Наносимо варення на тост...");
            await Task.Delay(1000);
            Console.WriteLine("Варення нанесено, тости готові!");
        }

        static async Task<Juice> PourJuiceAsync()
        {
            Console.WriteLine("Починаємо наливати сік...");
            await Task.Delay(1500);
            Console.WriteLine("Сік налито!");
            return new Juice();
        }
    }
}
