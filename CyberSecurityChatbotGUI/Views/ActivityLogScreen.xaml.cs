using System.Collections.ObjectModel;
using System.Windows.Controls;
using CyberSecurityChatbotGUI.Logic;

namespace CyberSecurityChatbotGUI.Views
{
    public partial class ActivityLogScreen : UserControl
    {
        private readonly ActivityLogManager _activityManager;

        // Expose the log entries for data binding
        public ObservableCollection<string> ActivityLog => _activityManager.LogEntries;

        public ActivityLogScreen(ActivityLogManager activityManager)
        {
            InitializeComponent();
            _activityManager = activityManager;
            DataContext = this;
        }
    }
}
