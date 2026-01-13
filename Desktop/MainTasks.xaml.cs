using Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop
{
    public partial class MainTasks : Window
    {
        private int _currentUserId;
        private string _userName;
        private List<Task> _tasks;
        private Task _selectedTask;

        public MainTasks(int userId, string userName)
        {
            InitializeComponent();
            _currentUserId = userId;
            _userName = userName;
            LoadUserData();
            LoadTasks();
        }

        private void LoadUserData()
        {
            string firstLetter = _userName.Length > 0 ? _userName[0].ToString().ToUpper() : "U";

            UserNameTextBlock.Text = _userName;

            var stackPanel = (StackPanel)LeftPanel.Child;
            if (stackPanel.Children[1] is Border avatarBorder && avatarBorder.Child is TextBlock avatarText)
            {
                avatarText.Text = firstLetter;
            }
        }

        private void LoadTasks()
        {
            try
            {
                _tasks = TaskRepository.GetTasksByUser(_currentUserId);
                DisplayCategories();
                DisplayTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayCategories()
        {
            CategoriesPanel.Children.Clear();

            var allButton = CreateCategoryButton("Все");
            allButton.Click += (s, e) => DisplayTasks();
            CategoriesPanel.Children.Add(allButton);

            var categories = TaskRepository.GetAllCategories(_currentUserId);
            if (categories.Count == 0)
            {
                categories = new List<string> { "Дом", "Работа", "Учеба", "Отдых" };
            }

            foreach (var category in categories)
            {
                var button = CreateCategoryButton(category);
                button.Click += (s, e) => DisplayTasks(category);
                CategoriesPanel.Children.Add(button);
            }
        }

        private Button CreateCategoryButton(string text)
        {
            return new Button
            {
                Content = text,
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                BorderBrush = Brushes.Transparent,
                FontSize = 14,
                Margin = new Thickness(0, 0, 20, 0),
                Padding = new Thickness(10, 5, 10, 5),
                Cursor = System.Windows.Input.Cursors.Hand
            };
        }

        private void DisplayTasks(string category = null)
        {
            TasksPanel.Children.Clear();

            var filteredTasks = string.IsNullOrEmpty(category) || category == "Все"
                ? _tasks
                : _tasks.Where(t => t.Category == category).ToList();

            foreach (var task in filteredTasks)
            {
                TasksPanel.Children.Add(CreateTaskControl(task));
            }
        }

        private Border CreateTaskControl(Task task)
        {
            var taskBorder = new Border
            {
                Background = Brushes.Transparent,
                BorderBrush = new SolidColorBrush(Color.FromRgb(85, 85, 85)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(10),
                Tag = task.Id,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var checkBox = new CheckBox
            {
                IsChecked = task.IsCompleted,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            checkBox.Checked += (s, e) => UpdateTaskStatus(task.Id, true);
            checkBox.Unchecked += (s, e) => UpdateTaskStatus(task.Id, false);

            var titleText = new TextBlock
            {
                Text = task.Title,
                Foreground = task.IsCompleted ?
                    new SolidColorBrush(Color.FromRgb(128, 128, 128)) :
                    Brushes.White,
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                TextDecorations = task.IsCompleted ? TextDecorations.Strikethrough : null
            };

            var dateText = new TextBlock
            {
                Text = task.DueDate.HasValue ?
                    task.DueDate.Value.ToString("HH:mm dd MMMM yyyy") :
                    "Нет даты",
                Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128)),
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(20, 0, 0, 0)
            };

            Grid.SetColumn(checkBox, 0);
            Grid.SetColumn(titleText, 1);
            Grid.SetColumn(dateText, 2);

            grid.Children.Add(checkBox);
            grid.Children.Add(titleText);
            grid.Children.Add(dateText);

            taskBorder.Child = grid;
            taskBorder.MouseLeftButtonDown += (s, e) => SelectTask(task);

            return taskBorder;
        }

        private void SelectTask(Task task)
        {
            _selectedTask = task;
            SelectedTaskTitle.Text = task.Title;
            SelectedTaskDate.Text = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("HH:mm dd MMMM yyyy")
                : "Нет даты";
            SelectedTaskDescription.Text = string.IsNullOrEmpty(task.Description)
                ? "Нет описания"
                : task.Description;

            CompleteButton.IsEnabled = !task.IsCompleted;
            DeleteButton.IsEnabled = true;
        }

        private void UpdateTaskStatus(int taskId, bool isCompleted)
        {
            try
            {
                if (TaskRepository.UpdateTaskStatus(taskId, isCompleted))
                {
                    var task = _tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task != null)
                    {
                        task.IsCompleted = isCompleted;
                        if (_selectedTask?.Id == taskId)
                        {
                            SelectTask(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var createTaskWindow = new CreateTask(_currentUserId);
            if (createTaskWindow.ShowDialog() == true)
            {
                LoadTasks();
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask != null && !_selectedTask.IsCompleted)
            {
                UpdateTaskStatus(_selectedTask.Id, true);
                CompleteButton.IsEnabled = false;
                LoadTasks();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту задачу?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (TaskRepository.DeleteTask(_selectedTask.Id))
                        {
                            _tasks.Remove(_selectedTask);
                            _selectedTask = null;

                            SelectedTaskTitle.Text = "Заголовок";
                            SelectedTaskDate.Text = "Нет даты";
                            SelectedTaskDescription.Text = "Выберите задачу для просмотра деталей";

                            CompleteButton.IsEnabled = false;
                            DeleteButton.IsEnabled = false;

                            LoadTasks();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}