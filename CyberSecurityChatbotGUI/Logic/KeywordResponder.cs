using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot.Logic
{
    public class KeywordResponder
    {
        private readonly Dictionary<string, string[]> _keywordResponses;

        public KeywordResponder()
        {
            _keywordResponses = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new[] { "Use a unique password for each account and avoid using personal info." } },
                { "phishing", new[] { "Be cautious of emails that ask for personal details — they may be phishing scams." } },
                { "privacy", new[] { "Avoid oversharing personal information on social media." } },
                { "malware", new[] { "Keep your antivirus software updated to protect against malware." } },
                { "2fa", new[] { "Enable 2-factor authentication for extra account security." } },
                { "scam", new[] { "If something seems too good to be true, it probably is a scam." } },
                { "vpn", new[] { "Using a VPN can secure your connection, especially on public Wi-Fi." } },
                { "firewall", new[] { "A firewall helps protect your device from unauthorized access." } },
                { "breach", new[] { "Check if your email was part of a data breach and update your passwords regularly." } },
                { "encryption", new[] { "Encryption helps secure your data from unauthorized access." } },
            };
        }

        // Returns the short description for the first matching keyword found in input, or null if none found
        public string? GetResponse(string input)
        {
            foreach (var kvp in _keywordResponses)
            {
                if (input.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return kvp.Value[0];
                }
            }
            return null;
        }

        // Returns all keywords for topic listing
        public IEnumerable<string> GetKeywords()
        {
            return _keywordResponses.Keys;
        }
    }
}
