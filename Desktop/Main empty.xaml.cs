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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Desktop
{
    public partial class Main_empty : Window
    {
        public Main_empty()
        {
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            MainTasks mainTasksWindow = new MainTasks(1,"");
            mainTasksWindow.Show();
            this.Hide();
        }


        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
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
        private void LogOut_Click(object sender, RoutedEventArgs e)
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
    }
}
