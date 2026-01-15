using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginT.Text;
            string password = passwordT.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorMessageLabel.Content = "Пожалуйста, заполните все поля.";
                return;
            }

            var user = UserRepository.AuthenticateUser(login, password);
            if (user != null)
            {
                ErrorMessageLabel.Content = "";

                MainTasks mainWindow = new MainTasks(user.Id, user.Name);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorMessageLabel.Content = "Неверный логин или пароль.";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Registration mainW = new Registration();
            mainW.Show();
            this.Hide();
        }
    }
}
