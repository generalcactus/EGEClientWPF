using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EgeClient.Classes
{
    public class TaskLoader
    {
        // Папка, куда будут скопированы все ресурсы внутри рабочего каталога приложения
        private const string TargetFolderName = "ExamResources";

        // Список задач, который мы будем заполнять
        public List<TaskBase> TaskList { get; private set; } = new List<TaskBase>();

        // Конструктор: принимает путь к корневой папке с заданиями (например, C:\MyExamData)
        public TaskLoader(string sourceRootPath)
        {
            // 1. Копируем исходную папку в рабочий каталог
            string targetRootPath = CopyResourcesToWorkingDirectory(sourceRootPath);

            // 2. Создаем список задач на основе скопированных файлов
            CreateTaskListFromResources(targetRootPath);
        }

        private string CopyResourcesToWorkingDirectory(string sourceRootPath)
        {
            // Получаем путь к исполняемому файлу (.exe)
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetPath = Path.Combine(appDirectory, TargetFolderName);

            // Если целевая папка уже существует, удаляем ее, чтобы обеспечить чистоту данных
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
            }

            // Если исходная папка не существует, вызываем исключение
            if (!Directory.Exists(sourceRootPath))
            {
                throw new DirectoryNotFoundException($"Исходная папка не найдена: {sourceRootPath}");
            }

            // Копирование папки и всего ее содержимого
            CopyDirectory(new DirectoryInfo(sourceRootPath), new DirectoryInfo(targetPath));

            return targetPath;
        }

        private void CreateTaskListFromResources(string targetRootPath)
        {
            // Получаем все подпапки (задания) в корневой директории
            var taskFolders = Directory.GetDirectories(targetRootPath)
                                     .OrderBy(p => p); // Сортируем по имени для порядка

            foreach (var folderPath in taskFolders)
            {
                // Пытаемся извлечь номер задания из имени папки (например, 'Task_01' -> 1)
                int taskNumber = ExtractTaskNumber(folderPath);

                // Если номер задания извлечь не удалось, пропускаем папку
                if (taskNumber == 0) continue;

                // Находим все файлы в текущей папке задания
                var files = Directory.GetFiles(folderPath);

                string? questionPath = null;
                List<string>? filePath = new List<string>();

                // Определяем, какой файл является картинкой, а какой — дополнительным файлом
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (extension == ".png")
                    {
                        // Предполагаем, что картинка — это 'question'
                        questionPath = file;
                    }
                    else
                    {
                        filePath.Add(file);
                    }
                }

                // Если найдена картинка, создаем задачу
                if (!string.IsNullOrEmpty(questionPath))
                {
                    TaskList.Add(new TaskBase
                    {
                        task_number = taskNumber,
                        question = questionPath,
                        file = filePath // Может быть null, если дополнительного файла нет
                    });
                }
            }
        }

        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Копирование файлов
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true); // true = перезапись
            }

            // Рекурсивное копирование подпапок
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }

        private static int ExtractTaskNumber(string folderPath)
        {
            string folderName = Path.GetFileName(folderPath);

            // 1. Проверяем, является ли имя папки целым числом
            if (int.TryParse(folderName, out int taskNumber))
            {
                // 2. Дополнительная проверка на диапазон (от 1 до 27)
                if (taskNumber >= 1 && taskNumber <= 27)
                {
                    return taskNumber;
                }
            }

            // Если это не число или оно вне диапазона 1-27, возвращаем 0
            return 0;
        }
    }
}
