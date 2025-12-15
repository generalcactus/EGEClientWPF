using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EgeClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string username;
        public string group;
        public string student_card;
        private bool isShkolnik = false;
        public bool IsDataSaved;
        public LoginWindow()
        {
            InitializeComponent();
            FillGroupList();
            SetGroupName();
            
        }

        private void SetGroupName()
        {
            if (!isShkolnik) {
                GroupName.Text = "Группа";
            }
            else
            {
                GroupName.Text = "Школа";
            }
        }

        private void FillGroupList()
        {
            if (!isShkolnik)
            {
                txtUserGroup.Items.Clear();
                txtUsername.Focus();
                {
                    txtUserGroup.Items.Add("КБ-11");
                    txtUserGroup.Items.Add("КБ-21");
                    txtUserGroup.Items.Add("ИБ-11");
                    txtUserGroup.Items.Add("ИБ-21");
                    txtUserGroup.Items.Add("ИВТ-11");
                    txtUserGroup.Items.Add("ИВТ-21");
                }
            }
            else
            {
                txtUserGroup.Items.Clear();
                txtUsername.Focus();
                {
                    txtUserGroup.Items.Add("Школа №33");
                    txtUserGroup.Items.Add("Школа №52");
                    txtUserGroup.Items.Add("Школа №50");
                    txtUserGroup.Items.Add("Школа №44");
                    txtUserGroup.Items.Add("Школа №83");
                    txtUserGroup.Items.Add("Школа №72");
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            IsDataSaved = false;
            username = txtUsername.Text;
            group = txtUserGroup.Text;
            student_card = txtUserStudCard.Text;
            // Простая демонстрационная проверка
            if (username !="")
            {
                // Успешная авторизация
                MessageBox.Show("Данные сохранены!", "Авторизация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                IsDataSaved = true;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                // Анимация ошибки
                txtUsername.BorderBrush = System.Windows.Media.Brushes.Red;
            }
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Сбрасываем стиль ошибки при изменении текста
            txtUsername.BorderBrush = System.Windows.Media.Brushes.Gray;      
            errorMessage.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            isShkolnik = true;
            //MainContainment.InvalidateVisual();
            labelStudCard.Visibility = Visibility.Collapsed;
            txtUserStudCard.Visibility = Visibility.Collapsed;

            txtUserStudCard.Clear();
            //this.UpdateLayout();
            //this.InvalidateVisual();
            FillGroupList();
            SetGroupName();

        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            isShkolnik = false;
            labelStudCard.Visibility = Visibility.Visible;
            txtUserStudCard.Visibility = Visibility.Visible;
            //MainContainment.InvalidateVisual();
            //this.UpdateLayout();
            //this.InvalidateVisual();
            FillGroupList();
            SetGroupName();
        }
        //кириешки
    }
}
