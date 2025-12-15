using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using EgeClient.Classes;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentTask > 1)
                {
                    currentTask--;
                    UpdateTaskDisplay(currentTask);
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
                if (currentTask < totalTasks)
                {
                    currentTask++;
                    UpdateTaskDisplay(currentTask);
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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (timer != null && timer.IsEnabled)
                {
                    var result = MessageBox.Show("Экзамен будет завершен. Уверены что хотите закончить?",
                                               "Выход",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        OutputTxtJsonAnswers.SaveAnswersToJsonSimple(taskAnswers, variant);
                        OutputTxtJsonAnswers.SaveAnswersToTXT(taskAnswers, variant);
                        timer.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при закрытии окна: {ex.Message}");
            }
        }


        // обработчик сохранения ответа
        private void btnSaveAnswer_Click(object sender, RoutedEventArgs e)
        {
            // если ответ дан, сохраняем и обновляем кнопки в панели слева
            if (txtAnswer.Text != "")
            {
                if (taskAnswers.ContainsKey(currentTask))
                {
                    taskAnswers[currentTask] = txtAnswer.Text;
                }
                else
                {
                    taskAnswers.Add(currentTask, txtAnswer.Text);
                }
                foreach (Button btn in ListOfButtons.Children)
                {
                    if (btn.Content.ToString() == currentTask.ToString())
                    {
                        var answeredStyle = (Style)this.FindResource("AnsweredTaskButtonStyle");
                        if (answeredStyle != null)
                        {
                            btn.Style = answeredStyle;
                        }
                    }
                }
            }

            // если ответ пуст и сохранен в taskAnswers, удаляем и меняем стиль кнопок
            else
            {
                if (taskAnswers.ContainsKey(currentTask))
                {
                    taskAnswers.Remove(currentTask);
                    foreach (Button btn in ListOfButtons.Children)
                    {
                        if (btn.Content.ToString() == currentTask.ToString())
                        {
                            var answeredStyle = (Style)this.FindResource("TaskButtonStyle");
                            if (answeredStyle != null)
                            {
                                btn.Style = answeredStyle;
                            }
                        }
                    }
                }
            }

            // Обновляем прогресс
            progressBar.Value = taskAnswers.Count;
            txtProgress.Text = $"{taskAnswers.Count}/{totalTasks} заданий";

            

        }

        private void btnSaveTable_Click(object sender, RoutedEventArgs e)
        {
            taskAnswers[currentTask] = SerializeGridAnswers(AnswerTableGrid, tableTaskConfigs[currentTask]);
        }

        private void btnClearTable_Click(object sender, RoutedEventArgs e)
        {
            taskAnswers[currentTask] = "";
            UpdateTaskDisplay(currentTask);
        }



        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && int.TryParse(button.Content?.ToString(), out int taskNumber))
                {
                    currentTask = taskNumber;
                    UpdateTaskDisplay(taskNumber);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переключении задания: {ex.Message}", "Ошибка");
            }
        }
    }
}
