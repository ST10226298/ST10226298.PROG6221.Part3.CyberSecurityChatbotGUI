using System.Collections.Generic;

namespace CyberSecurityChatbot.Logic
{
    public class MemoryManager
    {
        // Stores conversation history for each user
        private readonly Dictionary<string, List<string>> _userConversations = new();

        // Stores user-specific interests or other info
        private readonly Dictionary<string, string> _userInterests = new();

        public void StoreConversation(string userName, string input)
        {
            if (!_userConversations.ContainsKey(userName))
            {
                _userConversations[userName] = new List<string>();
            }
            _userConversations[userName].Add(input);
        }

        public List<string> GetConversationHistory(string userName)
        {
            return _userConversations.ContainsKey(userName) ? _userConversations[userName] : new List<string>();
        }

        public void SetUserInterest(string userName, string interest)
        {
            _userInterests[userName] = interest;
        }

        public string GetUserInterest(string userName)
        {
            return _userInterests.ContainsKey(userName) ? _userInterests[userName] : string.Empty;
        }
    }
}
