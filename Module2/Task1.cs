using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Module2Task1
{
    public class ProcessListForm : Form
    {
        private ListBox processListBox;
        private NumericUpDown intervalNumericUpDown;
        private Timer updateTimer;

        public ProcessListForm()
        {
            Text = "Список процесів";
            Size = new Size(500, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            Label intervalLabel = new Label { Text = "Інтервал оновлення (мс):", Left = 10, Top = 15, AutoSize = true };
            intervalNumericUpDown = new NumericUpDown { Left = 200, Top = 12, Minimum = 500, Maximum = 60000, Value = 2000 };
            Button applyButton = new Button { Text = "Застосувати", Left = 350, Top = 10, Width = 100 };
            
            applyButton.Click += (s, e) => updateTimer.Interval = (int)intervalNumericUpDown.Value;

            topPanel.Controls.Add(intervalLabel);
            topPanel.Controls.Add(intervalNumericUpDown);
            topPanel.Controls.Add(applyButton);
            Controls.Add(topPanel);

            processListBox = new ListBox { Dock = DockStyle.Fill };
            Controls.Add(processListBox);

            updateTimer = new Timer { Interval = 2000 };
            updateTimer.Tick += (s, e) => UpdateProcessList();
            updateTimer.Start();

            UpdateProcessList();
        }

        private void UpdateProcessList()
        {
            processListBox.Items.Clear();
            foreach (var process in Process.GetProcesses())
            {
                processListBox.Items.Add($"[{process.Id}] {process.ProcessName}");
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ProcessListForm());
        }
    }
}
