using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Module4Task2
{
    class CoffeeCup { }
    class Egg { }
    class Bacon { }
    class Toast { }
    class Juice { }

    public class BreakfastForm : Form
    {
        Label lblCoffee, lblEggs, lblBacon, lblToast, lblJuice, lblStatus;
        ProgressBar progressBar;
        Button btnStart;

        int completedTasks = 0;
        int totalTasks = 5;

        public BreakfastForm()
        {
            Text = "Асинхронний сніданок";
            Size = new Size(450, 350);
            StartPosition = FormStartPosition.CenterScreen;

            int y = 20;
            lblCoffee = CreateLabel("Кава: Очікування...", y); y += 30;
            lblEggs = CreateLabel("Яйця: Очікування...", y); y += 30;
            lblBacon = CreateLabel("Бекон: Очікування...", y); y += 30;
            lblToast = CreateLabel("Тости: Очікування...", y); y += 30;
            lblJuice = CreateLabel("Сік: Очікування...", y); y += 30;

            progressBar = new ProgressBar { Left = 20, Top = y, Width = 390, Maximum = 100 }; y += 40;
            lblStatus = new Label { Text = "Натисніть 'Готувати', щоб почати", Left = 20, Top = y, Width = 390 }; y += 30;

            btnStart = new Button { Text = "Готувати", Left = 150, Top = y, Width = 150, Height = 40 };
            btnStart.Click += BtnStart_Click;

            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            Controls.Add(btnStart);
        }

        private Label CreateLabel(string text, int top)
        {
            Label lbl = new Label { Text = text, Left = 20, Top = top, Width = 390, AutoSize = false };
            Controls.Add(lbl);
            return lbl;
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            progressBar.Value = 0;
            completedTasks = 0;
            lblStatus.Text = "Статус: Готуємо сніданок...";

            lblCoffee.Text = "Кава: Починаємо наливати...";
            lblEggs.Text = "Яйця: Починаємо смажити...";
            lblBacon.Text = "Бекон: Починаємо смажити...";
            lblToast.Text = "Тости: Починаємо готувати...";
            lblJuice.Text = "Сік: Починаємо наливати...";

            var coffeeTask = PourCoffeeAsync();
            var eggsTask = FryEggsAsync();
            var baconTask = FryBaconAsync();
            var toastTask = MakeToastWithJamAsync();
            var juiceTask = PourJuiceAsync();

            await Task.WhenAll(coffeeTask, eggsTask, baconTask, toastTask, juiceTask);

            lblStatus.Text = "Статус: Сніданок готовий!";
            btnStart.Enabled = true;
        }

        private void UpdateProgress()
        {
            completedTasks++;
            progressBar.Value = (completedTasks * 100) / totalTasks;
        }

        private async Task<CoffeeCup> PourCoffeeAsync()
        {
            await Task.Delay(1000);
            lblCoffee.Text = "Кава: Налито!";
            UpdateProgress();
            return new CoffeeCup();
        }

        private async Task<Egg> FryEggsAsync()
        {
            await Task.Delay(3000);
            lblEggs.Text = "Яйця: Готово!";
            UpdateProgress();
            return new Egg();
        }

        private async Task<Bacon> FryBaconAsync()
        {
            await Task.Delay(4000);
            lblBacon.Text = "Бекон: Готово!";
            UpdateProgress();
            return new Bacon();
        }

        private async Task<Toast> MakeToastWithJamAsync()
        {
            lblToast.Text = "Тости: Смажимо хліб...";
            await Task.Delay(2000);
            lblToast.Text = "Тости: Хліб підсмажено, наносимо варення...";
            await Task.Delay(1000);
            lblToast.Text = "Тости: Варення нанесено, тости готові!";
            UpdateProgress();
            return new Toast();
        }

        private async Task<Juice> PourJuiceAsync()
        {
            await Task.Delay(1500);
            lblJuice.Text = "Сік: Налито!";
            UpdateProgress();
            return new Juice();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new BreakfastForm());
        }
    }
}
