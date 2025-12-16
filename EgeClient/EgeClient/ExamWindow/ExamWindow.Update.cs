using System;
using System.Collections.Generic;
using System.IO;
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
                    // режим таблицы

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
                    // режим текстового поля

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
                else
                {
                    DownloadLinkContainer.Children.Clear();
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
                        // достаем картинку задания
                        if (!string.IsNullOrEmpty(currentTaskObj.question))
                        {
                            //string imagePath = $".\\variant\\{currentTaskObj.question}.jpg";
                            //imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
                            string imagePath = currentTaskObj.question;
                            if (System.IO.File.Exists(imagePath))
                            {
                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);

                                // прочитывание всего файла в память и закрие файлового дескриптора.
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                TaskImage.Source = bitmap;
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
