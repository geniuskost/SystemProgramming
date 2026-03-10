using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Module3Task3
{
    public class EncryptForm : Form
    {
        TextBox pathTextBox;
        Button browseBtn, encryptBtn, cancelBtn;
        Label statusLabel;
        ProgressBar progressBar;
        
        Thread encryptThread;
        bool cancelEncryption = false;

        public EncryptForm()
        {
            Text = "Шифрування файлів";
            Size = new Size(500, 200);
            StartPosition = FormStartPosition.CenterScreen;

            Label pathLabel = new Label { Text = "Шлях до файлу:", Left = 20, Top = 20, Width = 100 };
            
            pathTextBox = new TextBox { Left = 120, Top = 18, Width = 230 };
            browseBtn = new Button { Text = "Огляд...", Left = 360, Top = 16, Width = 100 };
            browseBtn.Click += BrowseBtn_Click;

            encryptBtn = new Button { Text = "Шифрувати", Left = 20, Top = 60, Width = 150 };
            encryptBtn.Click += EncryptBtn_Click;

            cancelBtn = new Button { Text = "Скасувати", Left = 180, Top = 60, Width = 150, Enabled = false };
            cancelBtn.Click += CancelBtn_Click;

            statusLabel = new Label { Left = 20, Top = 100, Width = 440, Text = "Очікування...", AutoSize = false };
            
            progressBar = new ProgressBar { Left = 20, Top = 130, Width = 440, Maximum = 100 };

            Controls.Add(pathLabel);
            Controls.Add(pathTextBox);
            Controls.Add(browseBtn);
            Controls.Add(encryptBtn);
            Controls.Add(cancelBtn);
            Controls.Add(statusLabel);
            Controls.Add(progressBar);
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = ofd.FileName;
                }
            }
        }

        private void EncryptBtn_Click(object sender, EventArgs e)
        {
            string path = pathTextBox.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("Вказаний файл не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cancelEncryption = false;
            encryptBtn.Enabled = false;
            cancelBtn.Enabled = true;
            statusLabel.Text = "Виконується шифрування...";
            progressBar.Value = 0;

            encryptThread = new Thread(() => EncryptFile(path));
            encryptThread.Start();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            cancelEncryption = true;
            cancelBtn.Enabled = false;
            statusLabel.Text = "Скасування... зачекайте";
        }

        private void EncryptFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                char[] buffer = content.ToCharArray();
                int shift = 3;
                int length = buffer.Length;

                for (int i = 0; i < length; i++)
                {
                    if (cancelEncryption)
                    {
                        Invoke((Action)(() => 
                        {
                            statusLabel.Text = "Шифрування скасовано користувачем.";
                            ResetInterface();
                        }));
                        return;
                    }

                    buffer[i] = (char)(buffer[i] + shift);

                    if (length > 0 && (i % (Math.Max(1, length / 100)) == 0 || i == length - 1))
                    {
                        int progress = (int)((i + 1) * 100L / length);
                        
                        Invoke((Action)(() => 
                        {
                            progressBar.Value = Math.Min(100, progress);
                        }));
                        
                        Thread.Sleep(20);
                    }
                }

                string newPath = filePath + ".encrypted";
                File.WriteAllText(newPath, new string(buffer));
                
                Invoke((Action)(() => 
                {
                    statusLabel.Text = $"Успішно! Збережено: {newPath}";
                    progressBar.Value = 100;
                    MessageBox.Show($"Шифрування завершено успішно.\nФайл збережено як:\n{newPath}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInterface();
                }));
            }
            catch (Exception ex)
            {
                Invoke((Action)(() => 
                {
                    statusLabel.Text = "Сталася помилка!";
                    MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetInterface();
                }));
            }
        }

        private void ResetInterface()
        {
            encryptBtn.Enabled = true;
            cancelBtn.Enabled = false;
            progressBar.Value = 0;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EncryptForm());
        }
    }
}
