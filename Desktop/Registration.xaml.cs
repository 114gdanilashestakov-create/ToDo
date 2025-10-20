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
using System.Windows.Shapes;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox1.Text;
            string password1 = PasswordBox2.Text;
            string name = NameTextBox.Text;

            if (ValidateEmail(email) && ValidatePassword(password) && ValidateName(name) && password == password1)
            {
                MessageBox.Show("Регистрация успешно проведена!");
                Main_empty MW = new Main_empty();
                MW.Show();
                this.Hide();
            }
            else
            {

                string errorMessage = "";

                if (!ValidateEmail(email))
                {
                    errorMessage += "Неверный формат почты\n";
                }

                if (!ValidatePassword(password))
                {
                    errorMessage += "Пароль меньше 6 симв.\n";
                }

                if (!ValidateName(name))
                {
                    errorMessage += "Имя короче 3 симв.\n";
                }

                MessageBox.Show(errorMessage, "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void FormClosing()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }

        private bool ValidatePassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 6;
        }

        private bool ValidateName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.Length >= 3;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow regW = new MainWindow();
            regW.Show();
            this.Hide();
        }

    }
}
