using System.Windows;
using System.Windows.Controls;
using CyberSecurityChatbot.Logic;

namespace CyberSecurityChatbotGUI.Views
{
    public partial class QuizGameScreen : UserControl
    {
        private readonly QuizManager _quizManager;

        public QuizGameScreen()
        {
            InitializeComponent();
            _quizManager = new QuizManager();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            FeedbackTextBlock.Text = "";
            NextButton.IsEnabled = false;

            if (_quizManager.IsFinished)
            {
                QuestionTextBlock.Text = $"Quiz Complete! Your Score: {_quizManager.Score} / {_quizManager.TotalQuestions}";
                OptionsPanel.Children.Clear();
                ScoreTextBlock.Text = _quizManager.Score >= _quizManager.TotalQuestions * 0.7
                    ? "Great job! You’re a cybersecurity pro!"
                    : "Keep learning to stay safe online!";
                return;
            }

            var question = _quizManager.CurrentQuestion;
            QuestionTextBlock.Text = question.QuestionText;
            ScoreTextBlock.Text = $"Question {_quizManager.TotalQuestions - (_quizManager.TotalQuestions - (_quizManager.TotalQuestions - _quizManager.TotalQuestions))} of {_quizManager.TotalQuestions}";

            OptionsPanel.Children.Clear();

            foreach (var option in question.Options)
            {
                var btn = new Button
                {
                    Content = option,
                    Margin = new Thickness(5),
                    Tag = option,
                    IsEnabled = true
                };
                btn.Click += OptionButton_Click;
                OptionsPanel.Children.Add(btn);
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string selectedAnswer = btn.Tag.ToString();
                var currentQuestion = _quizManager.CurrentQuestion;

                bool correct = string.Equals(selectedAnswer, currentQuestion.CorrectAnswer, System.StringComparison.OrdinalIgnoreCase);

                if (correct)
                {
                    FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                    FeedbackTextBlock.Text = "Correct! " + currentQuestion.Explanation;
                }
                else
                {
                    FeedbackTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                    FeedbackTextBlock.Text = $"Wrong! {currentQuestion.Explanation}";
                }

                // Disable all option buttons after answering
                foreach (Button b in OptionsPanel.Children)
                {
                    b.IsEnabled = false;
                }

                NextButton.IsEnabled = true;
                _quizManager.SubmitAnswer(selectedAnswer);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            LoadQuestion();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.Reset();
            LoadQuestion();
        }
    }
}
