using System;
using System.Collections.ObjectModel;

namespace CyberSecurityChatbotGUI.Logic
{
    public class ActivityLogManager
    {
        // ObservableCollection allows UI to auto-update when entries are added
        public ObservableCollection<string> LogEntries { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Adds a new log entry with a timestamp.
        /// </summary>
        /// <param name="entry">The log message (e.g., user input or bot response)</param>
        public void AddEntry(string entry)
        {
            if (string.IsNullOrWhiteSpace(entry)) return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            LogEntries.Add($"[{timestamp}] {entry}");
        }

        /// <summary>
        /// Clears all current log entries.
        /// </summary>
        public void Clear()
        {
            LogEntries.Clear();
        }
    }
}
