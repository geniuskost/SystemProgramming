using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace Module2Task3
{
    public class ProcessManagerForm : Form
    {
        private ListBox processListBox;
        private TextBox detailsTextBox;
        private Button terminateButton;
        private NumericUpDown intervalNumericUpDown;
        private Timer updateTimer;

        public ProcessManagerForm()
        {
            Text = "Менеджер процесів з можливістю завершення";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label intervalLabel = new Label { Text = "Інтервал оновлення (мс):", Left = 10, Top = 15, AutoSize = true };
            intervalNumericUpDown = new NumericUpDown { Left = 200, Top = 12, Minimum = 500, Maximum = 60000, Value = 2000 };
            
            topPanel.Controls.Add(intervalLabel);
            topPanel.Controls.Add(intervalNumericUpDown);
            Controls.Add(topPanel);

            SplitContainer splitContainer = new SplitContainer { Dock = DockStyle.Fill };
            Controls.Add(splitContainer);
            splitContainer.BringToFront();

            processListBox = new ListBox { Dock = DockStyle.Fill };
            processListBox.SelectedIndexChanged += ProcessListBox_SelectedIndexChanged;
            splitContainer.Panel1.Controls.Add(processListBox);

            Panel rightPanel = new Panel { Dock = DockStyle.Fill };
            detailsTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, Font = new Font("Consolas", 10) };
            
            terminateButton = new Button { Text = "Завершити процес", Dock = DockStyle.Bottom, Height = 40, Enabled = false };
            terminateButton.Click += TerminateButton_Click;

            rightPanel.Controls.Add(detailsTextBox);
            rightPanel.Controls.Add(terminateButton);
            
            splitContainer.Panel2.Controls.Add(rightPanel);

            updateTimer = new Timer { Interval = 2000 };
            updateTimer.Tick += (s, e) => 
            {
                updateTimer.Interval = (int)intervalNumericUpDown.Value;
                UpdateProcessList();
            };
            updateTimer.Start();

            UpdateProcessList();
        }

        private void UpdateProcessList()
        {
            int selectedIndex = processListBox.SelectedIndex;
            int topIndex = processListBox.TopIndex;

            processListBox.Items.Clear();
            foreach (var process in Process.GetProcesses().OrderBy(p => p.ProcessName))
            {
                processListBox.Items.Add(process.Id + " - " + process.ProcessName);
            }

            if (selectedIndex < processListBox.Items.Count && selectedIndex >= 0)
            {
                processListBox.SelectedIndex = selectedIndex;
                processListBox.TopIndex = topIndex;
            }
        }

        private void ProcessListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processListBox.SelectedItem == null)
            {
                terminateButton.Enabled = false;
                return;
            }

            terminateButton.Enabled = true;
            string selectedText = processListBox.SelectedItem.ToString();
            int id = int.Parse(selectedText.Split('-')[0].Trim());

            try
            {
                Process p = Process.GetProcessById(id);
                Process[] copies = Process.GetProcessesByName(p.ProcessName);

                string startTime = "Немає доступу";
                string cpuTime = "Немає доступу";
                try
                {
                    startTime = p.StartTime.ToString();
                    cpuTime = p.TotalProcessorTime.ToString();
                }
                catch { }

                detailsTextBox.Text = $"Ідентифікатор: {p.Id}\r\n" +
                                      $"Ім'я процесу: {p.ProcessName}\r\n" +
                                      $"Час старту: {startTime}\r\n" +
                                      $"Процесорний час: {cpuTime}\r\n" +
                                      $"Кількість потоків: {p.Threads.Count}\r\n" +
                                      $"Кількість копій: {copies.Length}";
            }
            catch (Exception ex)
            {
                detailsTextBox.Text = $"Не вдалося отримати інформацію: {ex.Message}";
                terminateButton.Enabled = false;
            }
        }

        private void TerminateButton_Click(object sender, EventArgs e)
        {
            if (processListBox.SelectedItem == null) return;
            string selectedText = processListBox.SelectedItem.ToString();
            int id = int.Parse(selectedText.Split('-')[0].Trim());

            try
            {
                Process p = Process.GetProcessById(id);
                p.Kill();
                MessageBox.Show("Процес успішно завершено", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateProcessList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося завершити процес: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new ProcessManagerForm());
        }
    }
}
