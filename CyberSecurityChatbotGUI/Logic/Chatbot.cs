using System;
using System.Collections.Generic;
using CyberSecurityChatbot.Logic;

namespace CyberSecurityChatbotGUI.Logic
{
    public class Chatbot
    {
        private readonly string _userName;
        private readonly MemoryManager _memory;
        private readonly SentimentAnalyzer _sentiment;
        private readonly ActivityLogManager _activityLog;

        private readonly Dictionary<string, string> _keywordShortDescriptions;
        private readonly Dictionary<string, List<string>> _keywordDetailedResponses;
        private readonly Random _random;

        public Chatbot(string userName, MemoryManager memory, SentimentAnalyzer sentiment, ActivityLogManager activityLog)
        {
            _userName = userName;
            _memory = memory;
            _sentiment = sentiment;
            _activityLog = activityLog;

            _random = new Random();

            _keywordShortDescriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", "Use a unique password for each account and avoid using personal info." },
                { "phishing", "Be cautious of emails that ask for personal details — they may be phishing scams." },
                { "privacy", "Avoid oversharing personal information on social media." },
                { "malware", "Keep your antivirus software updated to protect against malware." },
                { "2fa", "Enable 2-factor authentication for extra account security." },
                { "scam", "If something seems too good to be true, it probably is a scam." },
                { "vpn", "Using a VPN can secure your connection, especially on public Wi-Fi." },
                { "firewall", "A firewall helps protect your device from unauthorized access." },
                { "breach", "Check if your email was part of a data breach and update your passwords regularly." },
                { "encryption", "Encryption helps secure your data from unauthorized access." },
            };

            _keywordDetailedResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new List<string> {
                    "Use long passwords with a mix of characters, numbers, and symbols.",
                    "Avoid using easily guessable information like birthdays or names.",
                    "Consider using a password manager to store and generate passwords securely."
                }},
                { "phishing", new List<string> {
                    "Phishing emails often contain urgent messages or suspicious links.",
                    "Never enter personal information on suspicious websites.",
                    "Verify the sender’s email address before clicking links or downloading attachments."
                }},
                { "privacy", new List<string> {
                    "Adjust your social media privacy settings to limit who can see your info.",
                    "Be cautious about sharing your location or personal details publicly.",
                    "Regularly review app permissions on your devices."
                }},
                { "vpn", new List<string> {
                    "A VPN encrypts your internet traffic, hiding your IP address.",
                    "Choose reputable VPN providers that don’t log your activity.",
                    "Use VPNs especially when connected to public Wi-Fi networks."
                }},
                { "malware", new List<string> {
                    "Malware is malicious software that can harm your device or steal data.",
                    "Keep your antivirus software up-to-date and run regular scans.",
                    "Avoid downloading software or attachments from untrusted sources."
                }},
                { "2fa", new List<string> {
                    "2-Factor Authentication adds an extra layer of security beyond just passwords.",
                    "Use authenticator apps or SMS codes to verify your identity.",
                    "Enable 2FA on all your important online accounts whenever possible."
                }},
                { "scam", new List<string> {
                    "Scammers often use fake offers to trick you into giving money or info.",
                    "Be skeptical of unsolicited requests for money or personal information.",
                    "Report suspected scams to relevant authorities or platforms."
                }},
                { "firewall", new List<string> {
                    "A firewall helps protect your device by filtering incoming and outgoing traffic.",
                    "Enable the firewall on your computer and router for better security.",
                    "Firewalls can prevent unauthorized access and reduce malware risks."
                }},
                { "breach", new List<string> {
                    "Data breaches happen when hackers access private information.",
                    "Use services like 'Have I Been Pwned' to check if your email was compromised.",
                    "Change passwords immediately if you suspect your data was breached."
                }},
                { "encryption", new List<string> {
                    "Encryption converts your data into a coded format to prevent unauthorized access.",
                    "Use encrypted messaging apps for private communication.",
                    "Always check if websites use HTTPS to secure your data transmission."
                }},
            };
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please say something so I can help you!";

            string lowerInput = input.ToLower();

            // Check if input includes a keyword AND a request for more details
            foreach (var keyword in _keywordDetailedResponses.Keys)
            {
                if (lowerInput.Contains(keyword) &&
                    (lowerInput.Contains("explain") || lowerInput.Contains("tell me more") || lowerInput.Contains("details")))
                {
                    var details = _keywordDetailedResponses[keyword];
                    string detailedResponse = $"Here's more info on {keyword}:\n- {string.Join("\n- ", details)}";
                    _activityLog.AddEntry($"User: {input}");
                    _activityLog.AddEntry($"Bot: {detailedResponse}");
                    return detailedResponse;
                }
            }

            // Check for short description if keyword exists (without "explain" request)
            foreach (var kvp in _keywordShortDescriptions)
            {
                if (lowerInput.Contains(kvp.Key))
                {
                    _memory.SetUserInterest(_userName, kvp.Key);
                    string shortResponse = kvp.Value + " (If you want, ask me to 'tell me more' about it.)";
                    _activityLog.AddEntry($"User: {input}");
                    _activityLog.AddEntry($"Bot: {shortResponse}");
                    return shortResponse;
                }
            }

            // Sentiment checks  for empathy
            string sentiment = _sentiment.Analyze(input);
            string sentimentMsg = string.IsNullOrEmpty(sentiment) ? "" : _sentiment.GetMoodMessage(sentiment) + " ";

            // Store conversation for later recall
            _memory.StoreConversation(_userName, input);

            // Fallback random responses
            var fallbackResponses = new List<string>
            {
                "Can you tell me more about that?",
                "That's interesting! Want to hear a tip about online safety?",
                "I'm not sure I understood, but I can give you a cybersecurity tip!"
            };

            var randomResponse = fallbackResponses[_random.Next(fallbackResponses.Count)];
            _activityLog.AddEntry($"User: {input}");
            _activityLog.AddEntry($"Bot: {sentimentMsg + randomResponse}");
            return sentimentMsg + randomResponse;
        }
    }
}
