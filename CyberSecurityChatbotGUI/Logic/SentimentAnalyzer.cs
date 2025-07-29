using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Logic
{
    public class SentimentAnalyzer
    {
        private readonly Dictionary<string, string> _sentimentKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            { "worried", "worried" },
            { "anxious", "worried" },
            { "scared", "worried" },
            { "curious", "curious" },
            { "interested", "curious" },
            { "frustrated", "frustrated" },
            { "angry", "frustrated" },
            { "confused", "frustrated" }
        };

        public string Analyze(string input)
        {
            foreach (var keyword in _sentimentKeywords.Keys)
            {
                if (input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return _sentimentKeywords[keyword];
                }
            }

            return string.Empty; // No sentiment detected
        }

        public string GetMoodMessage(string sentiment)
        {
            return sentiment.ToLower() switch
            {
                "worried" => "I understand it can be worrying. Let's see how I can help.",
                "curious" => "Great! Curiosity is the first step to staying safe online.",
                "frustrated" => "I get it, this stuff can be confusing. I'll do my best to clarify.",
                _ => ""
            };
        }
    }
}
