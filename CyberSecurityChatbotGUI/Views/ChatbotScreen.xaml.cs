using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CyberSecurityChatbot.Logic;
using CyberSecurityChatbotGUI.Logic;

namespace CyberSecurityChatbotGUI.Views
{
    public partial class ChatbotScreen : UserControl
    {
        private Chatbot chatbot;
        private string userName = null;
        private bool isAwaitingName = true;
        private readonly KeywordResponder keywordResponder = new();

        // Add these fields here:
        private readonly MemoryManager memoryManager;
        private readonly SentimentAnalyzer sentimentAnalyzer;
        private readonly ActivityLogManager activityLogManager;

        public ChatbotScreen(ActivityLogManager logManager)
        {
            InitializeComponent();

            memoryManager = new MemoryManager();
            sentimentAnalyzer = new SentimentAnalyzer();
            activityLogManager = logManager;

            AddChatBubble("Welcome! Please enter your name to get started.", isUser: false);

            UserInputBox.KeyDown += UserInputBox_KeyDown;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(userMessage))
                return;

            AddChatBubble(userMessage, isUser: true);
            UserInputBox.Clear();

            // Log user input immediately
            activityLogManager.AddEntry($"User: {userMessage}");

            if (isAwaitingName)
            {
                userName = userMessage;
                // Pass activityLogManager to Chatbot so it can also log internally if needed
                chatbot = new Chatbot(userName, memoryManager, sentimentAnalyzer, activityLogManager);
                isAwaitingName = false;

                string welcomeMsg = $"Hi {userName}! I'm your Cybersecurity Bot. You can ask me about phishing, passwords, scams, and more.";
                AddChatBubble(welcomeMsg, isUser: false);
                activityLogManager.AddEntry($"Bot: {welcomeMsg}");

                string topicsMsg = "If you want to see topics I can help with, just type 'topics'.";
                AddChatBubble(topicsMsg, isUser: false);
                activityLogManager.AddEntry($"Bot: {topicsMsg}");
                return;
            }

            if (userMessage.ToLower().Contains("topic"))
            {
                var topics = keywordResponder.GetKeywords();
                string topicsList = string.Join(", ", topics);
                string topicsMessage = $"Here are some topics you can ask me about: {topicsList}";
                AddChatBubble(topicsMessage, isUser: false);
                activityLogManager.AddEntry($"Bot: {topicsMessage}");
                return;
            }

            // Use the chatbot to generate a response
            string botResponse = chatbot.GetResponse(userMessage);
            AddChatBubble(botResponse, isUser: false);

            // Log the bot response
            activityLogManager.AddEntry($"Bot: {botResponse}");
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void AddChatBubble(string message, bool isUser)
        {
            TextBlock chatBubble = new TextBlock
            {
                Text = message,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                MaxWidth = 400,
                TextWrapping = TextWrapping.Wrap,
                Background = isUser ? Brushes.LightBlue : Brushes.LightGray,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            ChatStackPanel.Children.Add(chatBubble);

            if (VisualTreeHelper.GetParent(ChatStackPanel) is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToEnd();
            }
        }
    }
}
