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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        /*private readonly Dictionary<int, string> fileDownloadConfigs = new Dictionary<int, string>
        {
            { 18, "18.xlsx" },
            { 26, "file_q26.zip" },
            { 27, "image_a.jpg" }
            // ... другие задания с файлами ...
        };*/
        private readonly Dictionary<int, int> tableTaskConfigs = new Dictionary<int, int>
        {
            { 17, 1 },
            { 18, 1 },
            { 20, 1 },
            { 25, 10 },
            { 26, 1 },
            { 27, 2}
        };

        Variant variant;
        TestingOption testingoption;
        private readonly int[] tableTaskNumbers = { 17, 18, 20, 25, 26, 27 };

        private DispatcherTimer timer;
        private int timeRemaining = 3 * 60 * 60 + 55 * 60; // 3 часа 55 минут в секундах
        private int currentTask = 1;
        private int totalTasks = 27;

        private Dictionary<int, string> taskAnswers = new Dictionary<int, string>();
        private TextBox txtAnswers;

        public ExamWindow(Variant var, TestingOption to)
        {
            variant = var;
            testingoption = to;
            InitializeComponent();
            InitializeElements();

            InitializeTimer();
            UpdateTaskDisplay(1);

        }

        private void InitializeElements()
        {

            // Устанавливаем начальные значения для безопасности
            txtTimer.Text = "03:55:00";

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
                Console.WriteLine($"Ошибка обновления таймера: {ex.Message}");
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
                //OutputTxtJsonAnswers.SaveAnswersToTXT(taskAnswers, variant);
                OutputTxtJsonAnswers.SaveAnswersToJson(taskAnswers, variant, testingoption);


                //OutputTxtJsonAnswers.SaveAnswersToJsonSimple(taskAnswers, variant);



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

    }
}