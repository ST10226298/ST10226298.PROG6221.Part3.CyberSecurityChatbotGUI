using System.Windows;
using System.Windows.Controls;
using CyberSecurityChatbotGUI.Views;
using CyberSecurityChatbotGUI.Logic; 

namespace CyberSecurityChatbotGUI
{
    public partial class MainWindow : Window
    {
        private readonly ActivityLogManager _logManager;

        public MainWindow()
        {
            InitializeComponent();

            _logManager = new ActivityLogManager();

            // Pass _logManager here
            MainContentControl.Content = new ChatbotScreen(_logManager);
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string tag = button.Tag as string;

                switch (tag)
                {
                    case "Chat":
                        MainContentControl.Content = new ChatbotScreen(_logManager);
                        break;
                    case "Tasks":
                        MainContentControl.Content = new TaskAssistantScreen();
                        break;
                    case "Quiz":
                        MainContentControl.Content = new QuizGameScreen();
                        break;
                    case "Log":
                        MainContentControl.Content = new ActivityLogScreen(_logManager);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
