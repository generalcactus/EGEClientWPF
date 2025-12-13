using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        private void UpdateTaskDisplay(int taskNumber)
        {
            try
            {
                // Проверяем, нужен ли табличный ввод для текущего задания
                bool isTableTask = System.Array.Exists(tableTaskNumbers, element => element == taskNumber);

                if (isTableTask)
                {
                    // --- РЕЖИМ ТАБЛИЦЫ ---

                    if (tableTaskConfigs.ContainsKey(taskNumber))
                    {
                        int requiredRows = tableTaskConfigs[taskNumber];
                        GenerateTableCells(requiredRows);
                    }

                    // если ответ в таблице был сохранен, возвращаем его в таблицу
                    if (taskAnswers.TryGetValue(currentTask, out string? value))
                    {
                        DeserializeGridAnswers(AnswerTableGrid, tableTaskConfigs[currentTask], value);
                    }

                    Col0.Width = new GridLength(2, GridUnitType.Star);
                    Col1.Width = new GridLength(1, GridUnitType.Star);

                    // 2. Скрываем текстовое поле ввода
                    txtAnswer.Visibility = Visibility.Hidden;

                    // 3. Показываем табличное поле ввода
                    TableAnswerPanel.Visibility = Visibility.Visible;
                    labelAnswer.Visibility = Visibility.Hidden;
                    btnSaveAnswer.Visibility = Visibility.Hidden;
                }
                else
                {
                    // --- РЕЖИМ ТЕКСТОВОГО ПОЛЯ ---

                    Col0.Width = new GridLength(1, GridUnitType.Star);
                    Col1.Width = new GridLength(0);
                    ClearTable();
                    // 2. Показываем текстовое поле ввода
                    txtAnswer.Visibility = Visibility.Visible;

                    // 3. Скрываем табличное поле ввода
                    TableAnswerPanel.Visibility = Visibility.Collapsed;

                    labelAnswer.Visibility = Visibility.Visible;
                    btnSaveAnswer.Visibility = Visibility.Visible;
                }
                txtTaskNumber.Text = $"Задание {currentTask}";

                

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

                //ссылка на скачивание
                string fileName = null;
                //fileDownloadConfigs.TryGetValue(taskNumber, out fileName);
                var currentTaskObj = variant.Tasks.FirstOrDefault(t => t.task_number == currentTask);
                if (currentTaskObj.file != null)
                {
                    UpdateDownloadLink(currentTaskObj.file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления задания: {ex.Message}");
            }
        }

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
                            //string imagePath = $"C:\\-VS-PROGS-\\EGEClientWPF-master\\EGEClientWPF-master\\EGEClientWPF\\EgeClient\\EgeClient\\bin\\Debug\\net8.0-windows7.0\\variant\\{currentTaskObj.question}.jpg";
                            string imagePath = $"D:\\allProjects\\приложение_C#_Core\\ForGit\\EgeClient\\EgeClient\\bin\\Debug\\net10.0-windows7.0\\variant\\{currentTaskObj.question}.jpg";
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
    }
}
