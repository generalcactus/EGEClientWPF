using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace EgeClient
{
    public partial class ExamWindow : Window
    {
        private void UpdateDownloadLink(string fileName)
        {
            DownloadLinkContainer.Children.Clear();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var linkStack = new StackPanel { Orientation = Orientation.Horizontal, Cursor = Cursors.Hand, Margin = new Thickness(25, 0, 0, 0) };
            var icon = new TextBlock
            {
                Text = "📖",
                FontSize = 16,
                Foreground = new BrushConverter().ConvertFromString("#28a745") as SolidColorBrush,
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            // создаем текстовый блок с ссылкой
            var textBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.AliceBlue
            };

            var hyperlink = new Hyperlink
            {
                NavigateUri = new Uri($"D:\\allProjects\\приложение_C#_Core\\ForGit\\EgeClient\\EgeClient\\bin\\Debug\\net10.0-windows7.0\\variant\\{fileName}", UriKind.RelativeOrAbsolute),
                FontSize = 16,

            };

            hyperlink.Inlines.Add(fileName);
            // добавляем обработчик нажатия на ссылку
            //hyperlink.Click += (s, e) => HandleDownloadClick(fileName);
            hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

            textBlock.Inlines.Add(hyperlink);
            // собираем элементы и добавляем их в контейнер
            linkStack.Children.Add(icon);
            linkStack.Children.Add(textBlock);

            DownloadLinkContainer.Children.Add(linkStack);
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                // 1. Запускаем внешний процесс для открытия URI
                // Используем UseShellExecute = true, чтобы система сама выбрала, чем открыть файл (Excel, Проводник и т.д.)
                Process.Start(new ProcessStartInfo(e.Uri.LocalPath) { UseShellExecute = true });

                // 2. Устанавливаем e.Handled = true, чтобы WPF не пытался открыть URI снова
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл. Ошибка: {ex.Message}", "Ошибка открытия файла", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true; // Важно установить true даже при ошибке
            }
        }
    }
}
