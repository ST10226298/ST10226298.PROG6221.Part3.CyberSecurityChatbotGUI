using System;
using System.Collections.ObjectModel;

namespace CyberSecurityChatbot.Logic
{
    public class TaskManager
    {
        public ObservableCollection<TaskItem> Tasks { get; private set; }

        public TaskManager()
        {
            Tasks = new ObservableCollection<TaskItem>();
        }

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            Tasks.Add(new TaskItem
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate,
                IsCompleted = false
            });
        }

        public void RemoveTask(TaskItem task)
        {
            if (Tasks.Contains(task))
                Tasks.Remove(task);
        }

        public void MarkTaskCompleted(TaskItem task, bool isCompleted)
        {
            var t = Tasks.Contains(task) ? task : null;
            if (t != null)
            {
                t.IsCompleted = isCompleted;
            }
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
