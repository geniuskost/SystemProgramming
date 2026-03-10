using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Module2Task4
{
    public class LauncherForm : Form
    {
        public LauncherForm()
        {
            Text = "Запуск програм";
            Size = new Size(300, 300);
            StartPosition = FormStartPosition.CenterScreen;

            FlowLayoutPanel panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(20) };
            
            Button notepadBtn = CreateAppButton("Блокнот", "notepad.exe");
            Button calcBtn = CreateAppButton("Калькулятор", "calc.exe");
            Button paintBtn = CreateAppButton("Paint", "mspaint.exe");
            
            Button customAppBtn = new Button { Text = "Інший додаток...", Width = 200, Height = 40, Margin = new Padding(0, 0, 0, 10) };
            customAppBtn.Click += CustomAppBtn_Click;

            panel.Controls.Add(notepadBtn);
            panel.Controls.Add(calcBtn);
            panel.Controls.Add(paintBtn);
            panel.Controls.Add(customAppBtn);

            Controls.Add(panel);
        }

        private Button CreateAppButton(string text, string path)
        {
            Button btn = new Button { Text = text, Width = 200, Height = 40, Margin = new Padding(0, 0, 0, 10) };
            btn.Click += (s, e) => LaunchApp(path);
            return btn;
        }

        private void LaunchApp(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося запустити {path}:\n{ex.Message}", "Помилка");
            }
        }

        private void CustomAppBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Виконувані файли (*.exe)|*.exe|Всі файли (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LaunchApp(ofd.FileName);
                }
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new LauncherForm());
        }
    }
}
