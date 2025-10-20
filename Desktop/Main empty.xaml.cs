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

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для Main_empty.xaml
    /// </summary>
    public partial class Main_empty : Window
    {
        private bool isMenuOpen = false;
        public Main_empty()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход из профиля",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Выход выполнен успешно!");
                MainWindow MainW = new MainWindow();
                MainW.Show();
                this.Hide();
            }
        }

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите новое фото профиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
            
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"Выбрано новое фото: {System.IO.Path.GetFileName(selectedFilePath)}");
                avatarImage.Source = new BitmapImage(new Uri(selectedFilePath));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
