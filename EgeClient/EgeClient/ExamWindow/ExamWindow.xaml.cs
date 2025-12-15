using EgeClient.Classes;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.IO;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        private readonly Dictionary<int, string> fileDownloadConfigs = new Dictionary<int, string>
        {
            { 18, "18.xlsx" },
            { 26, "file_q26.zip" },
            { 27, "image_a.jpg" }
            // ... другие задания с файлами ...
        };
        private readonly Dictionary<int, int> tableTaskConfigs = new Dictionary<int, int>
        {
            { 17, 5 },  // Задание 17 требует 5 строк
            { 18, 10 }, // Задание 18 требует 10 строк
            { 19, 3 },  // Задание 19 требует 3 строки
            { 20, 8 },  // Задание 20 требует 8 строк
            // ... добавьте остальные задания с их количеством строк ...
            { 25, 6 },
            { 26, 4 },
        };

        Variant variant;
        private readonly int[] tableTaskNumbers = { 17, 18, 19, 20, 25, 26 };

        private DispatcherTimer timer;
        private int timeRemaining = 3 * 60 * 60 + 55 * 60; // 3 часа 55 минут в секундах
        private int currentTask = 1;
        private int totalTasks = 27;

        private Dictionary<int, string> taskAnswers = new Dictionary<int, string>();
        private TextBox txtAnswers;

        public ExamWindow(Variant var)
        {
            variant = var;
            InitializeComponent();
            InitializeElements();
            // **Важно:** Сгенерируем ячейки таблицы при инициализации
            //GenerateTableCells();

            // Инициализируем вид для первого задания
            //UpdateTaskView(currentTask);

            InitializeTimer();
            UpdateTaskDisplay(1);
        }

        private void InitializeElements()
        {

            // Устанавливаем начальные значения для безопасности
            txtTimer.Text = "03:55:00";
            txtProgress.Text = "1/27 заданий";
            progressBar.Value = 1;
            progressBar.Maximum = 27;

            txtAnswer.TextChanged += txtAnswer_TextChanged;
        }
       


        private void txtAnswer_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (taskAnswers.ContainsKey(currentTask))
            //{
            //    taskAnswers[currentTask] = txtAnswer.Text;
            //}
            //else
            //{
            //    taskAnswers.Add(currentTask, txtAnswer.Text);
            //}
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateTimerDisplay();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            UpdateTimerDisplay();

            if (timeRemaining <= 0)
            {
                timer.Stop();
                MessageBox.Show("Время вышло! Экзамен завершен.", "Время",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                FinishExam();
            }
        }

        private void UpdateTimerDisplay()
        {
            try
            {
                int hours = timeRemaining / 3600;
                int minutes = (timeRemaining % 3600) / 60;
                int seconds = timeRemaining % 60;
                txtTimer.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            catch (Exception ex)
            {
                // Заглушка на случай ошибок
                Console.WriteLine($"Ошибка обновления таймера: {ex.Message}");
            }
        }


        
        private void HandleDownloadClick(string fileName)
        {
            MessageBox.Show($"Начато скачивание файла: {fileName}");

            // Здесь должна быть реальная логика скачивания:
            // 1. Определить полный путь к файлу.
            // 2. Определить место для сохранения (например, папка "Загрузки").
            // 3. Запустить процесс скачивания (например, с помощью System.Diagnostics.Process.Start для открытия файла/ссылки).
        }


        private void FinishExam()
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                }

<<<<<<< HEAD
                MessageBox.Show("Экзамен завершен! Ваши ответы сохранены.", "Завершено",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                OutputTxtJsonAnswers.SaveAnswersToTXT(taskAnswers, variant);
                OutputTxtJsonAnswers.SaveAnswersToJsonSimple(taskAnswers, variant);
                
=======
/*                MessageBox.Show("Экзамен завершен! Ваши ответы сохранены.", "Завершено",
                              MessageBoxButton.OK, MessageBoxImage.Information);*/
                SaveAnswersToJsonSimple();
>>>>>>> 46eafea3b1098d3cfec9a8af178a19a9d58db162

                if (Owner != null)
                {
                    Owner.Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при завершении: {ex.Message}", "Ошибка");
                this.Close();
            }
        }



        

<<<<<<< HEAD
=======
                // Диалог сохранения файла
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                    FileName = $"{variant.Student?.FIO?.Replace(" ", "_") ?? "Unknown"}_Вариант_{variant.ExamInfo?.variant_number ?? 0}_{DateTime.Now:yyyy-MM-dd_HH-mm}",
                    DefaultExt = ".json",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Сериализуем весь variant
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    // Сохраняем файл
                    string json = JsonSerializer.Serialize(variant, options);
                    File.WriteAllText(saveFileDialog.FileName, json);

                    MessageBox.Show($"Файл сохранен:\n{saveFileDialog.FileName}", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Application.Current.MainWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
>>>>>>> 46eafea3b1098d3cfec9a8af178a19a9d58db162



        private void AnswerTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Заглушка для обработки клавиш в текстовом поле
        }

        private void TaskNavigation_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Заглушка для навигации по заданиям с клавиатуры
        }

        public void Window_Closing()
        {
            
        }
    }
}