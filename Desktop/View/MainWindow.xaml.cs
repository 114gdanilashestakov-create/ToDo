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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Regist());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorMessageLabel.Content = "Пожалуйста, заполните все поля.";
                return;
            }

            var user = UserRepository.AuthenticateUser(login, password);
            if (user != null)
            {
                ErrorMessageLabel.Content = "";
                MainFrame.Navigate(new MainTasks(user.Id, user.Name));
            }
            else
            {
                ErrorMessageLabel.Content = "Неверный логин или пароль.";
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
