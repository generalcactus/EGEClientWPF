using EgeClient.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        Variant variant;

        private DispatcherTimer timer;
        private int timeRemaining = 3 * 60 * 60 + 55 * 60; // 3 часа 55 минут в секундах
        private int currentTask = 1;
        private int totalTasks = 27;

        private Dictionary<int, string> taskAnswers = new Dictionary<int, string>();

        // Объявляем элементы как поля класса, если они не найдены в XAML
        //private TextBlock txtTaskPoint;
        //private TextBlock txtTaskTexts;
        //private TextBlock txtTaskNumbers;
        //private TextBlock txtTimers;
        //private TextBlock txtProgress;
        //private ProgressBar progressBar;
        //private Button btnPrevious;
        //private Button btnNext;
        private TextBox txtAnswers;

        public ExamWindow(Variant var)
        {
            variant = var;
            InitializeComponent();
            InitializeElements();
            InitializeTimer();
            UpdateTaskDisplay();
        }

        private void InitializeElements()
        {
            // Инициализируем элементы, если они не найдены в XAML
            // Это защита от случаев, когда элементы не созданы в разметке

            // Ищем элементы по имени, если не найдены - создаем заглушки
            txtTaskNumber = FindName("txtTaskNumber") as TextBlock ?? new TextBlock();
            txtTaskPoints = FindName("txtTaskPoints") as TextBlock ?? new TextBlock();
            //txtTaskText = FindName("txtTaskText") as TextBlock ?? new TextBlock();
            txtTimer = FindName("txtTimer") as TextBlock ?? new TextBlock();
            txtProgress = FindName("txtProgress") as TextBlock ?? new TextBlock();
            progressBar = FindName("progressBar") as ProgressBar ?? new ProgressBar();
            btnPrevious = FindName("btnPrevious") as Button ?? new Button();
            btnNext = FindName("btnNext") as Button ?? new Button();
            txtAnswer = FindName("txtAnswer") as TextBox ?? new TextBox();

            // Устанавливаем начальные значения для безопасности
            txtTimer.Text = "03:55:00";
            txtProgress.Text = "1/27 заданий";
            progressBar.Value = 1;
            progressBar.Maximum = 27;

            txtAnswer.TextChanged += txtAnswer_TextChanged;
        }

        private void txtAnswer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (taskAnswers.ContainsKey(currentTask)){
                taskAnswers[currentTask] = txtAnswer.Text;
            }
            else
            {
                taskAnswers.Add(currentTask, txtAnswer.Text);
            }
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

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && int.TryParse(button.Content?.ToString(), out int taskNumber))
                {
                    if (taskAnswers.ContainsKey(currentTask))
                    {
                        taskAnswers[currentTask] = txtAnswer.Text;
                    }
                    else
                    {
                        taskAnswers.Add(currentTask, txtAnswer.Text);
                    }
                    currentTask = taskNumber;
                    UpdateTaskDisplay();

                    // Обновляем стиль кнопки на "отвеченная"
                    if (txtAnswer.Text != "")
                    {
                        var answeredStyle = (Style)this.FindResource("AnsweredTaskButtonStyle");
                        if (answeredStyle != null)
                        {
                            button.Style = answeredStyle;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переключении задания: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateTaskDisplay()
        {
            try
            {
                txtTaskNumber.Text = $"Задание {currentTask}";

                // Обновляем прогресс
                progressBar.Value = currentTask;
                txtProgress.Text = $"{currentTask}/{totalTasks} заданий";

                if (taskAnswers.ContainsKey(currentTask))
                {
                    txtAnswer.Text = taskAnswers[currentTask];
                }
                else
                {
                    txtAnswer.Text = string.Empty;
                }

                // Обновляем текст задания в зависимости от номера
                UpdateTaskContent();

                // Обновляем состояние кнопок навигации
                btnPrevious.IsEnabled = currentTask > 1;
                btnNext.IsEnabled = currentTask < totalTasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления задания: {ex.Message}");
            }
        }

        //private void UpdateTaskContent()
        //{
        //    try
        //    {
        //        if (MainWindow.variant != null)
        //            // Обновляем баллы и текст задания
        //            /*switch (currentTask)
        //            {
        //                case 1:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    TaskImage.Source = new BitmapImage(new Uri("D:\\EgeWPF\\EgeClient\\EgeClient\\bin\\Debug\\net8.0-windows7.0\\variant\\1.jpg"));
        //                    break;
        //                case 2:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    break;
        //                case 3:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    break;
        //                case 4:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    break;
        //                case 5:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    break;
        //                default:
        //                    txtTaskPoints.Text = "(1 балл)";
        //                    break;
        //            }*/
        //        foreach (TaskBase task in variant.Tasks)
        //            {
        //                txtTaskPoints.Text = "(1 балл)";
        //                if (task.question != "")
        //                {
        //                    TaskImage.Source = new BitmapImage(new Uri($"D:\\EgeWPF\\EgeClient\\EgeClient\\bin\\Debug\\net8.0-windows7.0\\variant\\{task.question}.jpg"));
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Ошибка обновления контента задания: {ex.Message}");
        //    }
        //}

        private void UpdateTaskContent()
        {
            try
            {
                if (variant?.Tasks != null)
                {
                    // Находим задание по текущему номеру
                    var currentTaskObj = variant.Tasks.FirstOrDefault(t => t.task_number == currentTask);

                    if (currentTaskObj != null)
                    {
                        txtTaskPoints.Text = "(1 балл)";

                        // Используем жесткий путь, но для текущего задания
                        if (!string.IsNullOrEmpty(currentTaskObj.question))
                        {
                            string imagePath = $"C:\\-VS-PROGS-\\EGEClientWPF-master\\EGEClientWPF-master\\EGEClientWPF\\EgeClient\\EgeClient\\bin\\Debug\\net8.0-windows7.0\\variant\\{currentTaskObj.question}.jpg";
                            if (System.IO.File.Exists(imagePath))
                            {
                                TaskImage.Source = new BitmapImage(new Uri(imagePath));
                            }
                            else
                            {
                                // Если файл не найден
                                TaskImage.Source = null;
                                MessageBox.Show($"Файл не найден: {imagePath}");
                                Console.WriteLine($"Файл не найден: {imagePath}");
                            }
                        }
                        else
                        {
                            TaskImage.Source = null;
                        }
                    }
                    else
                    {
                        // Если задание не найдено в варианте
                        txtTaskPoints.Text = "(1 балл)";
                        TaskImage.Source = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления контента задания: {ex.Message}");
                TaskImage.Source = null;
            }
        }



        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (taskAnswers.ContainsKey(currentTask))
                {
                    taskAnswers[currentTask] = txtAnswer.Text;
                }
                else
                {
                    taskAnswers.Add(currentTask, txtAnswer.Text);
                }
                if (currentTask > 1)
                {
                    currentTask--;
                    UpdateTaskDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе к предыдущему заданию: {ex.Message}", "Ошибка");
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (taskAnswers.ContainsKey(currentTask))
                {
                    taskAnswers[currentTask] = txtAnswer.Text;
                }
                else
                {
                    taskAnswers.Add(currentTask, txtAnswer.Text);
                }
                if (currentTask < totalTasks)
                {
                    currentTask++;
                    UpdateTaskDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе к следующему заданию: {ex.Message}", "Ошибка");
            }
        }

        private void btnFinishExam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (taskAnswers.ContainsKey(currentTask))
                {
                    taskAnswers[currentTask] = txtAnswer.Text;
                }
                else
                {
                    taskAnswers.Add(currentTask, txtAnswer.Text);
                }
                var result = MessageBox.Show("Вы уверены, что хотите завершить экзамен?",
                                           "Завершение экзамена",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    FinishExam();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при завершении экзамена: {ex.Message}", "Ошибка");
            }
        }

        private void FinishExam()
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                }

                MessageBox.Show("Экзамен завершен! Ваши ответы сохранены.", "Завершено",
                              MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (timer != null && timer.IsEnabled)
                {
                    var result = MessageBox.Show("Экзамен будет завершен. Продолжить?",
                                               "Выход",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        timer.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при закрытии окна: {ex.Message}");
            }
        }


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