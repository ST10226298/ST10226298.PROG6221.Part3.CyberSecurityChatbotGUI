// File: Logic/ConversationManager.cs
using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Logic
{
    public class ConversationManager
    {
        private readonly string _userName;
        private readonly MemoryManager _memory;
        private readonly SentimentAnalyzer _sentiment;
        private readonly Dictionary<string, string> _keywordDescriptions;
        private readonly Dictionary<string, List<string>> _keywordDetails;
        private readonly Random _random = new();
        private string? _lastKeyword = null;  // Track last keyword for context

        public ConversationManager(string userName, MemoryManager memory, SentimentAnalyzer sentiment)
        {
            _userName = userName;
            _memory = memory;
            _sentiment = sentiment;

            _keywordDescriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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

            _keywordDetails = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new List<string>
                    {
                        "Use long passwords with a mix of characters, numbers, and symbols.",
                        "Avoid using easily guessable information like birthdays or names.",
                        "Consider using a password manager to store and generate passwords securely."
                    }
                },
                { "phishing", new List<string>
                    {
                        "Phishing emails often contain urgent messages or suspicious links.",
                        "Never enter personal information on suspicious websites.",
                        "Verify the sender’s email address before clicking links or downloading attachments."
                    }
                },
                { "privacy", new List<string>
                    {
                        "Adjust your social media privacy settings to limit who can see your info.",
                        "Be cautious about sharing your location or personal details publicly.",
                        "Regularly review app permissions on your devices."
                    }
                },
                { "vpn", new List<string>
                    {
                        "A VPN encrypts your internet traffic, hiding your IP address.",
                        "Choose reputable VPN providers that don’t log your activity.",
                        "Use VPNs especially when connected to public Wi-Fi networks."
                    }
                },
                { "malware", new List<string>
                    {
                        "Malware is malicious software that can harm your device or steal data.",
                        "Keep your antivirus software up-to-date and run regular scans.",
                        "Avoid downloading software or attachments from untrusted sources."
                    }
                },
                { "2fa", new List<string>
                    {
                        "2-Factor Authentication adds an extra layer of security beyond just passwords.",
                        "Use authenticator apps or SMS codes to verify your identity.",
                        "Enable 2FA on all your important online accounts whenever possible."
                    }
                },
                { "scam", new List<string>
                    {
                        "Scammers often use fake offers to trick you into giving money or info.",
                        "Be skeptical of unsolicited requests for money or personal information.",
                        "Report suspected scams to relevant authorities or platforms."
                    }
                },
                { "firewall", new List<string>
                    {
                        "A firewall helps protect your device by filtering incoming and outgoing traffic.",
                        "Enable the firewall on your computer and router for better security.",
                        "Firewalls can prevent unauthorized access and reduce malware risks."
                    }
                },
                { "breach", new List<string>
                    {
                        "Data breaches happen when hackers access private information.",
                        "Use services like 'Have I Been Pwned' to check if your email was compromised.",
                        "Change passwords immediately if you suspect your data was breached."
                    }
                },
                { "encryption", new List<string>
                    {
                        "Encryption converts your data into a coded format to prevent unauthorized access.",
                        "Use encrypted messaging apps for private communication.",
                        "Always check if websites use HTTPS to secure your data transmission."
                    }
                }
            };
        }

        public string GenerateResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please say something so I can help you!";

            input = input.ToLower();

            // Respond with detailed info if the user says 'explain' or similar
            if ((_lastKeyword != null) && (input.Contains("explain") || input.Contains("tell me more") || input.Contains("details")))
            {
                if (_keywordDetails.TryGetValue(_lastKeyword, out var details))
                {
                    return $"Here's more info on {_lastKeyword}:\n- {string.Join("\n- ", details)}";
                }
                return $"I don’t have more detailed information about {_lastKeyword}, but I can still help!";
            }

            // Check if the user input includes a new keyword
            foreach (var kvp in _keywordDescriptions)
            {
                if (input.Contains(kvp.Key))
                {
                    _lastKeyword = kvp.Key; // Track the topic
                    _memory.SetUserInterest(_userName, kvp.Key);
                    return kvp.Value + " (If you want, ask me to 'explain' it.)";
                }
            }

            // Sentiment-based empathy
            string sentiment = _sentiment.Analyze(input);
            string sentimentMsg = string.IsNullOrEmpty(sentiment) ? "" : _sentiment.GetMoodMessage(sentiment) + " ";

            // Store input in memory
            _memory.StoreConversation(_userName, input);

            // Fallback response
            var fallbackResponses = new List<string>
            {
                "Can you tell me more about that?",
                "That's interesting! Want to hear a tip about online safety?",
                "I'm not sure I understood, but I can give you a cybersecurity tip!"
            };

            string randomResponse = fallbackResponses[_random.Next(fallbackResponses.Count)];
            return sentimentMsg + randomResponse;
        }

        public string GetKeywordList()
        {
            return $"Here are some topics I can help you with:\n- {string.Join("\n- ", _keywordDescriptions.Keys)}";
        }
    }
}
