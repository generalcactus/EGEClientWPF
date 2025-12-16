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
    public partial class MainWindow : Window
    {
        public static Variant variant = new Variant();
        public static Student student;
        //public string ZipFilePath;
        public string VariantFolder;
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
            if (student != null)
            {
                if (student.FIO != null && File.Exists($"I:\\Temp\\{student.FIO}"))
                {
                    //ZipFilePath = $"I:\\Temp\\{variant.Student.FIO}";
                    VariantFolder = $"I:\\Temp\\{variant.Student.FIO}_задания";
                }
                else
                {
                    /*OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = false;
                    openFileDialog.DefaultExt = ".zip";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        ZipFilePath = openFileDialog.FileName;
                    }*/
                    OpenFolderDialog openFolderDialog = new OpenFolderDialog();
                    if(openFolderDialog.ShowDialog() == true)
                    {
                        VariantFolder = openFolderDialog.FolderName;
                    }
                }
                /*if (ZipFilePath != "" && ZipFilePath != null)
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
                }*/
                if (VariantFolder != "" && VariantFolder != null && System.IO.Path.GetFileNameWithoutExtension(VariantFolder)==student.FIO)
                {
                    TaskLoader taskLoader = new TaskLoader(VariantFolder);
                    variant.Tasks = taskLoader.TaskList;
                    variant.Student = student;
                    ExamWindow examWindow = new ExamWindow(variant);
                    examWindow.Owner = this;
                    examWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали папку с вариантом или выбрали не свою");
                }
            }
            else
            {
                MessageBox.Show("Сначала введите ваши данные", "Ошибка авторизации");
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