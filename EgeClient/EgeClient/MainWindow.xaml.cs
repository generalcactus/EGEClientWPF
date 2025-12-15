using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using EgeClient.Classes;
using Microsoft.Win32;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace EgeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Variant variant;
        public static Student student;
        public string ZipFilePath;
        public string ExtractPath = "variant";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Owner = this;
            loginWindow.ShowDialog();
            if (loginWindow.IsDataSaved)
            {
                student = new Student() { FIO = loginWindow.username,
                    StudentGroup = (loginWindow.group != ""? loginWindow.group : null),
                    StudentCard = (!loginWindow.student_card.Contains("_") ? loginWindow.student_card : null)
                };
            }
        }

        private void btnStartExam_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                if (!File.Exists("D:\\EgeWPF\\exam\\1.json"))
                {
                    throw new FileNotFoundException("File not found!");
                }
                string jsonString = File.ReadAllText("D:\\EgeWPF\\exam\\1.json");
                variant = JsonSerializer.Deserialize<Variant>(jsonString);
                MessageBox.Show(variant.Tasks[0].question);
                string newstrJson = JsonSerializer.Serialize<Variant>(variant);
                File.WriteAllText("D:\\EgeWPF\\exam\\2.json", newstrJson);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
            if (student != null)
            {
                if (student.FIO != null && File.Exists($"I:\\Temp\\{student.FIO}"))
                {
                    ZipFilePath = $"I:\\Temp\\{variant.Student.FIO}";
                }
                else
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = false;
                    openFileDialog.DefaultExt = ".zip";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        ZipFilePath = openFileDialog.FileName;
                    }
                }
                if (ZipFilePath != "" && ZipFilePath != null)
                {
                    if (Directory.Exists(ExtractPath))
                    {
                        Directory.Delete(ExtractPath, true);
                    }
                    Directory.CreateDirectory(ExtractPath);
                    ZipFile.ExtractToDirectory(ZipFilePath, ExtractPath);
                    string jsonFileName = Directory.GetFiles(ExtractPath, "*.json")[0];
                    string jsonFileText = File.ReadAllText(jsonFileName);
                    variant = JsonSerializer.Deserialize<Variant>(jsonFileText);
                    variant.Student = student;
                    ExamWindow examWindow = new ExamWindow(variant);
                    examWindow.Owner = this;
                    examWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали архив с вариантом", "Не выбран файл");
                }
            }
            else
            {
                MessageBox.Show("Сначала введите ваши данные", "Ошибка авторизации");
            }
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Заглушка для кнопок меню
            if (sender is System.Windows.Controls.Button button)
            {
                MessageBox.Show($"Нажата кнопка: {button.Content}", "Меню");
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?",
                                       "Выход",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}