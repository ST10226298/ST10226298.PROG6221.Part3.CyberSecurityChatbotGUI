using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Logic
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;

        public QuizManager()
        {
            _questions = GenerateQuestions();
            _currentIndex = 0;
            _score = 0;
        }

        public QuizQuestion CurrentQuestion => _currentIndex < _questions.Count ? _questions[_currentIndex] : null;

        public int Score => _score;

        public int TotalQuestions => _questions.Count;

        public bool IsFinished => _currentIndex >= _questions.Count;

        public void SubmitAnswer(string answer)
        {
            if (IsFinished) return;

            if (string.Equals(CurrentQuestion.CorrectAnswer, answer, StringComparison.OrdinalIgnoreCase))
            {
                _score++;
            }
            _currentIndex++;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _score = 0;
        }

        private List<QuizQuestion> GenerateQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswer = "Report the email as phishing",
                    Explanation = "Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Using unique passwords for each account helps protect you if one account is compromised."
                },
                // Add 8 more questions similarly
                new QuizQuestion
                {
                    QuestionText = "Which of these is a strong password?",
                    Options = new List<string> { "123456", "password", "MyP@ssw0rd!", "qwerty" },
                    CorrectAnswer = "MyP@ssw0rd!",
                    Explanation = "Strong passwords combine letters, numbers, and symbols."
                },
                new QuizQuestion
                {
                    QuestionText = "What does VPN stand for?",
                    Options = new List<string> { "Virtual Private Network", "Very Personal Network", "Verified Public Network", "Virtual Personal Node" },
                    CorrectAnswer = "Virtual Private Network",
                    Explanation = "VPN encrypts your internet connection and provides privacy."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: Public Wi-Fi networks are always secure.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Public Wi-Fi can be insecure; using a VPN helps protect your data."
                },
                new QuizQuestion
                {
                    QuestionText = "What is phishing?",
                    Options = new List<string> { "A fishing technique", "A cyber attack to steal information", "A secure network protocol", "An antivirus software" },
                    CorrectAnswer = "A cyber attack to steal information",
                    Explanation = "Phishing tricks users into giving sensitive information."
                },
                new QuizQuestion
                {
                    QuestionText = "Why should you update software regularly?",
                    Options = new List<string> { "To get new features only", "To fix security vulnerabilities", "To make it slower", "It's not important" },
                    CorrectAnswer = "To fix security vulnerabilities",
                    Explanation = "Updates patch security holes hackers could exploit."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: Two-factor authentication adds an extra layer of security.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Two-factor authentication requires a second form of verification."
                },
                new QuizQuestion
                {
                    QuestionText = "What should you do if you suspect your account was hacked?",
                    Options = new List<string> { "Ignore it", "Change your passwords immediately", "Share your password with friends", "Delete your account" },
                    CorrectAnswer = "Change your passwords immediately",
                    Explanation = "Changing passwords limits damage and protects other accounts."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: It’s safe to download attachments from unknown senders.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Attachments from unknown sources can contain malware."
                }
            };
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }
}
