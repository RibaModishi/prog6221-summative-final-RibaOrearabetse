using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityAwarenessBot
{
    public class InputProcessor
    {
        private readonly KnowledgeBase _knowledge;
        private readonly IResponseRenderer _renderer;
        private readonly AudioService _audioService;
        private readonly string _userName;


        private string _lastTopic;
        private string _favoriteTopic;

        private readonly CyberTaskManager _taskManager;
        public CyberTaskManager TaskManager => _taskManager;

        private readonly List<string> _activityLog = new List<string>();



        private readonly List<string> jokes = new List<string>
        {
            "Why did the hacker break up with the internet? Too many insecure connections.",
            "Why don’t hackers take vacations? Because they prefer to work on phishing trips.",
            "I changed my password to 'incorrect' so whenever I forget it, the computer says: 'Your password is incorrect.'",
            "Why was the computer cold? It left its Windows open.",
            "What do you call a hacker who uses Google? A search-and-destroy specialist.",
            "Knock knock. Who’s there? A phishing email. A phishing email who? The one asking for your bank details urgently!",
            "Why did the smartphone go to therapy? It couldn’t deal with all its app issues."
        };

        private readonly Dictionary<string, string> topicFollowUps = new Dictionary<string, string>()
        {
            { "password", "Use two-factor authentication (2FA) wherever possible. Avoid '123456' or 'password' as passwords – seriously!" },
            { "scam", "Watch out for poor grammar, urgent messages, or requests for money. Always double-check with the source." },
            { "privacy", "Be cautious with app permissions and browser extensions. Less is more when it comes to sharing data." },
            { "phishing", "Phishing happens via email, text, or phone. If something feels off, don’t click anything!" },
            { "malware", "Avoid downloading from shady websites. Keep antivirus software updated and don’t click suspicious attachments." },
            { "social engineering", "They may act friendly or urgent. Never share passwords or private info, even with 'colleagues' you just met." },
            { "2fa", "App-based 2FA (like Authy or Google Authenticator) is better than SMS. Use it for critical accounts." },
            { "firewall", "A firewall helps block unauthorized traffic. Think of it like a bouncer for your data." },
            { "vpn", "A VPN hides your online activity from hackers and ISPs. Very useful on public Wi-Fi!" },
            { "email", "Don't trust email links. When in doubt, go directly to the company website instead." },
        };

        private readonly Dictionary<string, string> keywordResponses = new Dictionary<string, string>()
        {
            { "malware", "Malware is malicious software like viruses, ransomware, or spyware. It can steal or destroy your data!" },
            { "password", "Strong passwords matter! Use at least 12 characters with symbols, numbers, and uppercase letters." },
            { "scam", "Online scams trick people into giving up info or money. Always double-check before clicking." },
            { "privacy", "Protect your personal data online. Avoid oversharing and use privacy settings on social media." },
            { "social engineering", "Social engineering tricks people into giving up confidential info by pretending to be someone trustworthy." },
            { "2fa", "Two-factor authentication adds a second step when logging in. It’s like a secret handshake for your account!" },
            { "firewall", "A firewall acts like a shield that filters out bad internet traffic trying to reach your system." },
            { "vpn", "A VPN encrypts your internet activity. Super helpful when using public Wi-Fi or accessing blocked content." },
            { "email", "Always check the sender’s address. Legit companies won’t ask for your password or personal info via email." },
        };

        private readonly Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
        {
            { "worried", "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe." },
            { "frustrated", "I'm sorry you're feeling frustrated. Cybersecurity can feel overwhelming, but I’m here to help simplify it." },
            { "anxious", "Cyber threats can be scary, but you're taking the right step by learning more. Let's go at your pace." },
            { "curious", "Curiosity is the first step to awareness! Ask me anything about staying safe online." },
            { "confused", "No problem! I'm here to explain anything you need in simple terms." },
        };

        private readonly List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Hover over links to see the real URL before clicking. If it looks suspicious, don’t touch it!",
            "Never download attachments from unknown senders. They could contain malware.",
            "Phishing can occur via email, SMS (smishing), or even voice calls (vishing).",
            "When in doubt, contact the organization directly using a verified phone number or website."
        };

        private readonly Dictionary<string, string> casualResponses = new Dictionary<string, string>
        {
            { "hello", "Hello there! How can I assist you with cybersecurity today?" },
            { "hi", "Hi! What would you like to learn about today?" },
            { "hey", "Hey! Ready to boost your cyber-awareness?" },
            { "how are you", "I'm just code, but I'm running great! Thanks for asking. How can I help you stay safe online?" },
            { "what's up", "Staying secure and helping folks like you! How can I assist you today?" },
        };

        public InputProcessor(KnowledgeBase knowledge, IResponseRenderer renderer, AudioService audioService, string userName)
        {
            _knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _userName = userName ?? string.Empty;
            _lastTopic = string.Empty;
            _favoriteTopic = string.Empty;

            _taskManager = new CyberTaskManager(renderer);
        }

        public bool ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _renderer.DisplayColoredMessage("Please enter something so I can help!", ConsoleColor.Yellow);
                return false;
            }

            input = input.ToLower().Trim();

            if (input == "help")
            {
                _renderer.DisplayColoredMessage("🛡️ Cybersecurity Awareness Bot – Full Command Guide 🛡️", ConsoleColor.Cyan);
                _renderer.DisplayColoredMessage("======================================================", ConsoleColor.White);

                // 🔒 Cybersecurity Topics
                _renderer.DisplayColoredMessage("\n🔒 CYBERSECURITY TOPICS:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage("Ask about topics like:", ConsoleColor.White);
                _renderer.DisplayColoredMessage(" - malware, phishing, password, scam, privacy, firewall, vpn, 2fa, email, social engineering", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage("Example: 'Tell me about phishing' or 'What is a firewall?'", ConsoleColor.DarkGray);

                // 🧠 Knowledge Base
                _renderer.DisplayColoredMessage("\n🧠 KNOWLEDGE FOLLOW-UPS:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage("Say things like:", ConsoleColor.White);
                _renderer.DisplayColoredMessage(" - 'More about phishing' or 'Explain malware in more detail'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - 'more', 'details', or 'don't understand' (for recent topic)", ConsoleColor.DarkGray);

                // 📅 Task Manager
                _renderer.DisplayColoredMessage("\n📅 TASK MANAGER:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage("Manage cybersecurity tasks:", ConsoleColor.White);
                _renderer.DisplayColoredMessage(" - Add a task: 'add task Title - Description - 7'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - Show all tasks: 'show tasks' or 'my tasks'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - Complete a task: 'complete task 1'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - Delete a task: 'delete task 2'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - Natural reminders: 'Remind me to update antivirus in 5 days'", ConsoleColor.Gray);

                // 🎯 Interests & Favorites
                _renderer.DisplayColoredMessage("\n🎯 TOPIC INTEREST:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(" - 'I'm interested in phishing'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - 'What's my favourite topic?'", ConsoleColor.Gray);

                // 😂 Jokes & Casual Chat
                _renderer.DisplayColoredMessage("\n😂 JOKES & CASUAL:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(" - 'Tell me a joke', 'That's funny', 'how are you?', etc.", ConsoleColor.Gray);

                // 📝 Phishing Tips
                _renderer.DisplayColoredMessage("\n📝 PHISHING TIPS:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(" - 'Give me a phishing tip'", ConsoleColor.Gray);

                // 🧪 Quiz Game
                _renderer.DisplayColoredMessage("\n🧪 QUIZ GAME:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(" - 'start quiz', 'test my knowledge'", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - Use number keys (1, 2, 3, 4) to answer", ConsoleColor.Gray);

                // 🛠️ System Commands
                _renderer.DisplayColoredMessage("\n🛠️ SYSTEM:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(" - 'help' – Show this help menu", ConsoleColor.Gray);
                _renderer.DisplayColoredMessage(" - 'exit' or 'quit' – Close the chatbot", ConsoleColor.Gray);

                _renderer.DisplayColoredMessage("\n💡 Tip: You can speak naturally. Try things like 'How do I stay safe on public WiFi?'", ConsoleColor.Magenta);
                return false;
            }


            if (input == "exit" || input == "quit")
            {
                _renderer.DisplayColoredMessage("Stay safe out there! Goodbye!", ConsoleColor.Cyan);
                return true;
            }

            if (input.Contains("joke") || input.Contains("funny"))
            {
                var random = new Random();
                int index = random.Next(jokes.Count);
                _renderer.DisplayColoredMessage("Here's a cybersecurity joke:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(jokes[index], ConsoleColor.White);
                return false;
            }

            if (input.StartsWith("add task"))
            {
                _taskManager.AddDefaultTask();
                return false;
            }

            if (input.StartsWith("show tasks"))
            {
                _taskManager.ShowTasks();
                return false;
            }

            foreach (var sentiment in sentimentResponses)
            {
                if (input.Contains(sentiment.Key))
                {
                    _renderer.DisplayColoredMessage(sentiment.Value, ConsoleColor.Magenta);
                    foreach (var keyword in keywordResponses.Keys)
                    {
                        if (input.Contains(keyword))
                        {
                            _lastTopic = keyword;
                            _renderer.DisplayColoredMessage(keywordResponses[keyword], ConsoleColor.White);
                            return false;
                        }
                    }
                    return false;
                }
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains("interested in") && input.Contains(keyword))
                {
                    _favoriteTopic = keyword;
                    _lastTopic = keyword;
                    _renderer.DisplayColoredMessage($"Great! I'll remember that you're interested in {keyword}.", ConsoleColor.Green);
                    return false;
                }
            }

            if (input.Contains("remind me") || input.Contains("favourite topic"))
            {
                if (!string.IsNullOrEmpty(_favoriteTopic))
                {
                    _renderer.DisplayColoredMessage($"You're interested in {_favoriteTopic}. Keep an eye on it regularly!", ConsoleColor.Green);
                }
                else
                {
                    _renderer.DisplayColoredMessage("Tell me what you're interested in by saying something like 'I'm interested in phishing.'", ConsoleColor.Yellow);
                }
                return false;
            }

            if (input == "cybersecurity")
            {
                _renderer.DisplayColoredMessage("Here are some cybersecurity topics you can ask me about:", ConsoleColor.Cyan);
                foreach (var topic in keywordResponses.Keys)
                {
                    _renderer.DisplayColoredMessage($"- {topic}", ConsoleColor.White);
                }
                return false;
            }

            if (input.Contains("more") || input.Contains("explain") || input.Contains("details") || input.Contains("don't understand"))
            {
                if (!string.IsNullOrEmpty(_lastTopic) && topicFollowUps.ContainsKey(_lastTopic))
                {
                    _renderer.DisplayColoredMessage($"Here's more on {_lastTopic}: {topicFollowUps[_lastTopic]}", ConsoleColor.White);
                }
                else
                {
                    _renderer.DisplayColoredMessage("What topic do you want more details on?", ConsoleColor.Yellow);
                }
                return false;
            }

            if (input.StartsWith("more about") || input.StartsWith("tell me more about"))
            {
                foreach (var topic in topicFollowUps.Keys)
                {
                    if (input.Contains(topic))
                    {
                        _lastTopic = topic;
                        _renderer.DisplayColoredMessage($"Here's more on {topic}: {topicFollowUps[topic]}", ConsoleColor.White);
                        return false;
                    }
                }

                _renderer.DisplayColoredMessage("Not sure what topic you meant. Try something like 'malware' or 'VPN'.", ConsoleColor.Yellow);
                return false;
            }

            if (input.Contains("phishing tip"))
            {
                var random = new Random();
                int index = random.Next(phishingTips.Count);
                _lastTopic = "phishing";
                _renderer.DisplayColoredMessage("Here's a phishing tip:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(phishingTips[index], ConsoleColor.White);
                return false;
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    _lastTopic = keyword;
                    _renderer.DisplayColoredMessage(keywordResponses[keyword], ConsoleColor.White);
                    return false;
                }
            }

            if (_knowledge.Topics.ContainsKey(input))
            {
                _lastTopic = input;
                _renderer.DisplayColoredMessage(_knowledge.Topics[input], ConsoleColor.White);
                return false;
            }

            foreach (var casual in casualResponses)
            {
                if (input.Contains(casual.Key))
                {
                    _renderer.DisplayColoredMessage(casual.Value, ConsoleColor.Cyan);
                    return false;
                }
            }

            // ==== 🧠 NLP SIMULATION START ====
            if ((input.Contains("remind me to") || input.Contains("set a reminder to") || input.Contains("add a task to")))
            {
                string taskTitle = ExtractTaskTitleFromNaturalInput(input);
                var task = new CyberTask
                {
                    Title = taskTitle,
                    Description = "Auto-created from natural language input",
                    ReminderDate = DateTime.Now.AddDays(1) // Default: tomorrow
                };
                _taskManager.AddTask(task);
                _renderer.DisplayColoredMessage($"Task added: {task.Title}", ConsoleColor.Green);
                return false;
            }

            if (input.Contains("start quiz") || input.Contains("quiz me") || input.Contains("take quiz"))
            {
                _renderer.DisplayColoredMessage("Launching cybersecurity quiz... Go to the 'Mini Game' tab and click 'Start Quiz'.", ConsoleColor.Cyan);
                return false;
            }
            // ==== 🧠 NLP SIMULATION END ====

            if (input.Contains("show activity log") || input.Contains("what have you done"))
            {
                _renderer.DisplayColoredMessage("📋 Here's a summary of recent actions:", ConsoleColor.Green);

                var recent = _activityLog.Skip(Math.Max(0, _activityLog.Count - 10)).Reverse();
                int i = 1;
                foreach (var entry in recent)
                {
                    _renderer.DisplayColoredMessage($"{i++}. {entry}", ConsoleColor.White);
                }

                return false;
            }


            _renderer.DisplayColoredMessage("Sorry, I didn’t quite get that. Try typing 'help' to see what you can ask me.", ConsoleColor.Yellow);
            return false;
        }
        public void DisplayMessage(string message, ConsoleColor color)
        {
            _renderer.DisplayColoredMessage(message, color);
        }
        private string ExtractTaskTitleFromNaturalInput(string input)
        {
            string[] phrasesToRemove = {
        "can you", "please", "remind me to", "set a reminder to", "add a task to",
        "i want to", "could you", "would you kindly", "i need to", "make a note to"
    };

            foreach (var phrase in phrasesToRemove)
            {
                input = input.Replace(phrase, "");
            }

            input = input.Trim();
            return input;

        }
        private void LogActivity(string message)
        {
            string entry = $"{DateTime.Now:HH:mm:ss} - {message}";
            _activityLog.Add(entry);

            // Optional: limit to last 50 entries for memory efficiency
            if (_activityLog.Count > 50)
                _activityLog.RemoveAt(0);
        }
        public void LogActionExternally(string message)
        {
            LogActivity(message);
        }

    }
}
