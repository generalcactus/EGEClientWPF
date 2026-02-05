using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EgeClient.Classes
{
    public class FileManager
    {
        public string CreateTaskDirectoryStructure(TestingOption testingOption)
        {
            try
            {
                // Путь к директории в bin/Debug
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string mainFolderName = "testfolder";

                // Создаем главную папку
                string mainFolderPath = System.IO.Path.Combine(basePath, mainFolderName);

                if (Directory.Exists(mainFolderPath))
                {
                    // Если папка уже существует, можно удалить или спросить пользователя
                    // Для примера удаляем и создаем заново
                    Directory.Delete(mainFolderPath, true);
                }

                Directory.CreateDirectory(mainFolderPath);

                // Сортируем задания по номеру (на случай если они не в порядке)
                var sortedTasks = testingOption.TaskList
                    .OrderBy(t => int.Parse(t.TaskNumber))
                    .ToList();

                // Создаем подпапки от 1 до 27
                for (int i = 1; i <= 27; i++)
                {
                    string taskFolderName = i.ToString();
                    string taskFolderPath = System.IO.Path.Combine(mainFolderPath, taskFolderName);
                    Directory.CreateDirectory(taskFolderPath);

                    // Находим соответствующее задание
                    var taskData = sortedTasks.FirstOrDefault(t => t.TaskNumber == taskFolderName);

                    if (taskData != null)
                    {
                        // Сохраняем основное изображение задания
                        if (taskData.Image != null && taskData.Image.Length > 0)
                        {
                            string imagePath = System.IO.Path.Combine(taskFolderPath, $"task_{taskFolderName}.png");
                            SaveImageFromByteArray(taskData.Image, imagePath);
                        }

                        // Сохраняем дополнительные файлы
                        if (taskData.Files != null && taskData.Files.Count > 0)
                        {
                            SaveAdditionalFiles(taskData.Files, taskFolderPath);
                        }
                    }
                }

                MessageBox.Show($"Директория успешно создана: {mainFolderPath}", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return mainFolderPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании директории: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void SaveImageFromByteArray(byte[] imageData, string filePath)
        {
            try
            {
                File.WriteAllBytes(filePath, imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения {filePath}: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveAdditionalFiles(List<FileData> files, string folderPath)
        {
            foreach (var fileData in files)
            {
                if (fileData.Data != null && fileData.Data.Length > 0 &&
                    !string.IsNullOrEmpty(fileData.FileName))
                {
                    try
                    {
                        string filePath = System.IO.Path.Combine(folderPath, fileData.FileName);
                        File.WriteAllBytes(filePath, fileData.Data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла {fileData.FileName}: {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
    }
}
