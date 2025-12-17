using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace EgeClient.Classes
{
    public class OutputTxtJsonAnswers
    {
        public static void SaveAnswersToJsonSimple(Dictionary<int, string> taskAnswers, Variant variant)
        {
            try
            {
                // Заполняем ответы
                foreach (var task in variant.Tasks)
                {
                    if (taskAnswers.ContainsKey(task.task_number))
                    {
                        task.student_answer = taskAnswers[task.task_number];
                    }
                }

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


        public static void SaveAnswersToTXT(Dictionary<int, string> taskAnswers, Variant variant)
        {
            try
            {

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FileName = $"{variant.Student?.FIO?.Replace(" ", "") ?? "Unknown"}_ответы.txt";
                saveFileDialog.Title = "Сохранить ответы";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 1; i <= variant.Tasks.Count; i++)
                        {
                            if (taskAnswers.ContainsKey(i))
                            {
                                //if (i == 17 || i == 18 || i == 20 || i == 25 || i == 26)
                                //{
                                //    string[] s = taskAnswers[i].Split(';');
                                //    string[] stolb1 = s[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                //    string[] stolb2 = s[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                //    string answer = "";
                                //    for (int j = 0; j < stolb1.Length - 1; j++)
                                //    {
                                //        answer += $"{stolb1[j]} {stolb2[j]}, ";
                                //    }
                                //    answer += $"{stolb1[stolb1.Length - 1]} {stolb2[stolb1.Length - 1]}";
                                //    for (int k = 0; k < answer.Length; k++)
                                //    {
                                //        if (char.IsLetter(answer[k]))
                                //        {
                                //            if (k >= 2)
                                //            {
                                //                answer = answer.Substring(0, k - 2);
                                //            }
                                //            else
                                //            {
                                //                answer = "%noanswer%";
                                //                break;
                                //            }
                                //        }
                                //    }
                                //    sw.WriteLine($"{i} - {answer}");
                                //}
                                //else sw.WriteLine($"{i} - {taskAnswers[i]}");

                                if (i == 17 || i == 18 || i == 20 || i == 25 || i == 26 || i == 27)
                                {
                                    string[] s = taskAnswers[i].Split(';');
                                    string[] stolb1 = s[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                    string[] stolb2 = s[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                    int cutindex = stolb1.Length;
                                    for (int j = stolb1.Length - 1; j >= 0; j--)
                                    {
                                        if (stolb1[j] == "missed" && stolb2[j] == "missed")
                                        {
                                            cutindex--;
                                        }
                                        else break;
                                        
                                    }
                                    string answer = "";
                                    for (int k = 0; k < cutindex - 1; k++)
                                    {
                                        answer += $"{stolb1[k]} {stolb2[k]}, ";
                                    }
                                    answer += $"{stolb1[cutindex - 1]} {stolb2[cutindex - 1]}";
                                    sw.WriteLine($"{i} - {answer}");
                                }
                                else sw.WriteLine($"{i} - {taskAnswers[i]}");

                                //sw.WriteLine($"{i} - {taskAnswers[i]}");
                            }
                            else
                            {
                                sw.WriteLine($"{i} - %noanswer%");
                            }

                        }
                    }
                    MessageBox.Show($"Файл успешно сохранен!", "Сохранение завершено",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Application.Current.MainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nСтек вызовов:\n{ex.StackTrace}",
        "Ошибка",
        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
