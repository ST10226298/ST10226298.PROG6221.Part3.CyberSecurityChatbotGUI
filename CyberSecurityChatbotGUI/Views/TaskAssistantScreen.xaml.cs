using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CyberSecurityChatbot.Logic; // Make sure your TaskManager namespace is correct

namespace CyberSecurityChatbotGUI.Views
{
    public partial class TaskAssistantScreen : UserControl
    {
        private readonly TaskManager _taskManager;
        public ObservableCollection<TaskItem> Tasks => _taskManager.Tasks;

        public TaskAssistantScreen()
        {
            InitializeComponent();

            _taskManager = new TaskManager();

            TasksListBox.ItemsSource = Tasks;

            // Initialize placeholders on load (in case user clicks out without typing anything)
            TitleTextBox.Foreground = Brushes.Gray;
            DescriptionTextBox.Foreground = Brushes.Gray;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Validate that Title is not empty or placeholder
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || TitleTextBox.Text == "Task Title")
            {
                MessageBox.Show("Please enter a task title.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Replace placeholder description with empty if not changed
            var description = DescriptionTextBox.Text == "Task Description" ? "" : DescriptionTextBox.Text;

            _taskManager.AddTask(TitleTextBox.Text, description, ReminderDatePicker.SelectedDate);

            // Clear inputs and reset placeholders
            TitleTextBox.Text = "Task Title";
            TitleTextBox.Foreground = Brushes.Gray;

            DescriptionTextBox.Text = "Task Description";
            DescriptionTextBox.Foreground = Brushes.Gray;

            ReminderDatePicker.SelectedDate = null;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TaskItem task)
            {
                _taskManager.RemoveTask(task);
            }
        }

        private void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is TaskItem task)
            {
                _taskManager.MarkTaskCompleted(task, true);
            }
        }

        private void TaskCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is TaskItem task)
            {
                _taskManager.MarkTaskCompleted(task, false);
            }
        }

        // Remove placeholder text on focus
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if ((tb.Name == "TitleTextBox" && tb.Text == "Task Title") ||
                    (tb.Name == "DescriptionTextBox" && tb.Text == "Task Description"))
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black;
                }
            }
        }

        // Restore placeholder text on lost focus if empty
        private void AddText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    if (tb.Name == "TitleTextBox")
                    {
                        tb.Text = "Task Title";
                        tb.Foreground = Brushes.Gray;
                    }
                    else if (tb.Name == "DescriptionTextBox")
                    {
                        tb.Text = "Task Description";
                        tb.Foreground = Brushes.Gray;
                    }
                }
            }
        }
    }
}
