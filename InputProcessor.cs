using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CybersecurityAwarenessBot
{
    public class InputProcessor
    {
        private readonly KnowledgeBase _knowledge;
        private readonly IResponseRenderer _renderer;
        private readonly AudioService _audioService;
        private readonly string _userName;

        private string _lastTopic = string.Empty;
        private string _favoriteTopic = string.Empty;
        private readonly List<CyberTask> _tasks = new List<CyberTask>();
        public QuizGame QuizGame { get; }

        // Jokes collection
        private readonly List<string> _jokes = new List<string>
        {
            "Why did the hacker break up with the internet? Too many insecure connections.",
            "Why don't hackers take vacations? Because they prefer to work on phishing trips.",
            "I changed my password to 'incorrect' so whenever I forget it, the computer says: 'Your password is incorrect.'",
            "Why was the computer cold? It left its Windows open."
        };

        // Keyword responses
        private readonly Dictionary<string, string> _keywordResponses = new Dictionary<string, string>
        {
            { "malware", "Malware is malicious software like viruses, ransomware, or spyware. It can steal or destroy your data!" },
            { "password", "Strong passwords matter! Use at least 12 characters with symbols, numbers, and uppercase letters." },
            { "phishing", "Phishing attempts often pretend to be from trusted sources. Never click suspicious links!" }
        };

        // Sentiment responses
        private readonly Dictionary<string, string> _sentimentResponses = new Dictionary<string, string>
        {
            { "worried", "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe." },
            { "frustrated", "I'm sorry you're feeling frustrated. Cybersecurity can feel overwhelming, but I'm here to help simplify it." }
        };

        // Casual responses
        private readonly Dictionary<string, string> _casualResponses = new Dictionary<string, string>
        {
            { "hello", "Hello there! How can I assist you with cybersecurity today?" },
            { "hi", "Hi! What would you like to learn about today?" },
            { "how are you", "I'm just code, but I'm running great! How can I help you stay safe online?" }
        };

        public InputProcessor(KnowledgeBase knowledge, IResponseRenderer renderer, AudioService audioService, string userName)
        {
            _knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _userName = userName ?? string.Empty;
            QuizGame = new QuizGame(renderer, knowledge.QuizQuestions);
        }

        public bool ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _renderer.DisplayColoredMessage("Please enter something so I can help!", ConsoleColor.Yellow);
                return false;
            }

            input = input.ToLower().Trim();

            // Check for commands first
            if (TryProcessCommand(input)) return false;

            // Check for task-related inputs
            if (TryProcessTaskInput(input)) return false;

            // Check for quiz commands
            if (input.Contains("quiz") || input.Contains("game") || input.Contains("test"))
            {
                QuizGame.StartQuiz();
                return false;
            }

            // Process natural language inputs
            return ProcessNaturalLanguage(input);
        }

        private bool TryProcessCommand(string input)
        {
            switch (input)
            {
                case "help":
                    ShowHelp();
                    return true;
                case "exit":
                case "quit":
                    _renderer.DisplayColoredMessage("Stay safe out there! Goodbye!", ConsoleColor.Cyan);
                    return true;
                case "cybersecurity":
                    ShowTopics();
                    return true;
                default:
                    return false;
            }
        }

        private bool TryProcessTaskInput(string input)
        {
            if (input.Contains("task") || input.Contains("remind"))
            {
                if (input.StartsWith("add task") || input.Contains("create task"))
                {
                    ProcessAddTask(input);
                    return true;
                }
                if (input.StartsWith("show tasks") || input.Contains("my tasks"))
                {
                    ShowTasks();
                    return true;
                }
                if (input.StartsWith("complete task") || input.Contains("mark as done"))
                {
                    ProcessCompleteTask(input);
                    return true;
                }
                if (input.StartsWith("delete task"))
                {
                    ProcessDeleteTask(input);
                    return true;
                }
                if (input.Contains("remind me to"))
                {
                    ProcessReminderRequest(input);
                    return true;
                }
            }
            return false;
        }

        private void ProcessAddTask(string input)
        {
            try
            {
                var task = new CyberTask();
                if (TryParseTaskCommand(input, out task))
                {
                    _tasks.Add(task);
                    _renderer.DisplayColoredMessage($"Task added: {task.Title}", ConsoleColor.Green);
                    if (task.ReminderDate.HasValue)
                    {
                        _renderer.DisplayColoredMessage($"I'll remind you on {task.ReminderDate.Value.ToShortDateString()}", ConsoleColor.Cyan);
                    }
                }
                else
                {
                    _renderer.DisplayColoredMessage("Please provide task details in format: 'Add task [title] - [description] - [days]'", ConsoleColor.Yellow);
                }
            }
            catch
            {
                _renderer.DisplayColoredMessage("Couldn't add task. Please check the format.", ConsoleColor.Red);
            }
        }

        private bool TryParseTaskCommand(string input, out CyberTask task)
        {
            task = new CyberTask();
            var addTaskMatch = Regex.Match(input, @"add task (.+?) - (.+?)(?: - (\d+))?$");
            if (addTaskMatch.Success)
            {
                task.Title = addTaskMatch.Groups[1].Value.Trim();
                task.Description = addTaskMatch.Groups[2].Value.Trim();
                if (addTaskMatch.Groups[3].Success && int.TryParse(addTaskMatch.Groups[3].Value, out int days))
                {
                    task.ReminderDate = DateTime.Now.AddDays(days);
                }
                return true;
            }
            return false;
        }

        private void ShowTasks()
        {
            if (!_tasks.Any())
            {
                _renderer.DisplayColoredMessage("You don't have any tasks yet.", ConsoleColor.Yellow);
                return;
            }

            _renderer.DisplayColoredMessage("Your cybersecurity tasks:", ConsoleColor.Cyan);
            for (int i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];
                var status = task.IsCompleted ? "[✓]" : "[ ]";
                _renderer.DisplayColoredMessage($"{i + 1}. {status} {task.Title}: {task.Description}", ConsoleColor.White);
                if (task.ReminderDate.HasValue)
                {
                    _renderer.DisplayColoredMessage($"   ⏰ Reminder: {task.ReminderDate.Value.ToShortDateString()}", ConsoleColor.Cyan);
                }
            }
        }

        private void ProcessCompleteTask(string input)
        {
            if (int.TryParse(Regex.Match(input, @"\d+").Value, out int taskId) && taskId > 0 && taskId <= _tasks.Count)
            {
                _tasks[taskId - 1].IsCompleted = true;
                _renderer.DisplayColoredMessage($"Marked task '{_tasks[taskId - 1].Title}' as completed!", ConsoleColor.Green);
            }
            else
            {
                _renderer.DisplayColoredMessage("Please specify a valid task number.", ConsoleColor.Yellow);
            }
        }

        private void ProcessDeleteTask(string input)
        {
            if (int.TryParse(Regex.Match(input, @"\d+").Value, out int taskId) && taskId > 0 && taskId <= _tasks.Count)
            {
                var title = _tasks[taskId - 1].Title;
                _tasks.RemoveAt(taskId - 1);
                _renderer.DisplayColoredMessage($"Deleted task '{title}'", ConsoleColor.Green);
            }
            else
            {
                _renderer.DisplayColoredMessage("Please specify a valid task number.", ConsoleColor.Yellow);
            }
        }

        private void ProcessReminderRequest(string input)
        {
            var match = Regex.Match(input, @"remind me to (.+?)(?: in (\d+) days)?(?: about (.+))?$");
            if (match.Success)
            {
                var task = new CyberTask
                {
                    Title = match.Groups[1].Value.Trim(),
                    Description = match.Groups[3].Success ? match.Groups[3].Value.Trim() : "Reminder set through chat",
                    ReminderDate = match.Groups[2].Success ? DateTime.Now.AddDays(int.Parse(match.Groups[2].Value)) : (DateTime?)null
                };

                _tasks.Add(task);
                _renderer.DisplayColoredMessage($"I'll remind you to {task.Title}", ConsoleColor.Green);
                if (task.ReminderDate.HasValue)
                {
                    _renderer.DisplayColoredMessage($"on {task.ReminderDate.Value.ToShortDateString()}", ConsoleColor.Cyan);
                }
            }
        }

        private bool ProcessNaturalLanguage(string input)
        {
            // Check for jokes
            if (input.Contains("joke") || input.Contains("funny"))
            {
                var random = new Random();
                _renderer.DisplayColoredMessage("Here's a cybersecurity joke:", ConsoleColor.Green);
                _renderer.DisplayColoredMessage(_jokes[random.Next(_jokes.Count)], ConsoleColor.White);
                return false;
            }

            // Check for sentiment
            foreach (var sentiment in _sentimentResponses)
            {
                if (input.Contains(sentiment.Key))
                {
                    _renderer.DisplayColoredMessage(sentiment.Value, ConsoleColor.Magenta);
                    return false;
                }
            }

            // Check for topic keywords
            foreach (var keyword in _keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    _lastTopic = keyword;
                    _renderer.DisplayColoredMessage(_keywordResponses[keyword], ConsoleColor.White);
                    return false;
                }
            }

            // Check knowledge base
            foreach (var topic in _knowledge.Topics.Keys)
            {
                if (input.Contains(topic))
                {
                    _lastTopic = topic;
                    _renderer.DisplayColoredMessage(_knowledge.Topics[topic], ConsoleColor.White);
                    return false;
                }
            }

            // Check casual conversation
            foreach (var casual in _casualResponses)
            {
                if (input.Contains(casual.Key))
                {
                    _renderer.DisplayColoredMessage(casual.Value, ConsoleColor.Cyan);
                    return false;
                }
            }

            // Default response
            _renderer.DisplayColoredMessage("I'm not sure I understand. Try typing 'help' to see what I can do.", ConsoleColor.Yellow);
            return false;
        }

        private void ShowHelp()
        {
            _renderer.DisplayColoredMessage("🚀 Cybersecurity Awareness Bot - Command Guide 🚀", ConsoleColor.Cyan);
            _renderer.DisplayColoredMessage("========================================", ConsoleColor.White);

            // Cybersecurity Features
            _renderer.DisplayColoredMessage("\n🔒 CYBERSECURITY TOPICS:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("Ask about: malware, phishing, passwords, scams, privacy, 2fa, vpn, firewall", ConsoleColor.White);
            _renderer.DisplayColoredMessage("Example: \"Tell me about phishing\" or \"How do passwords work?\"", ConsoleColor.Gray);

            // Task Assistant
            _renderer.DisplayColoredMessage("\n📝 TASK ASSISTANT:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("- Add task: \"add task Enable 2FA - Turn on two-factor authentication - 7\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- View tasks: \"show tasks\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Complete task: \"complete task 1\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Delete task: \"delete task 1\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Natural language: \"Remind me to update passwords every 30 days\"", ConsoleColor.Gray);

            // Sentiment Analysis
            _renderer.DisplayColoredMessage("\n😊 SENTIMENT ANALYSIS:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("Try phrases like: \"I'm worried about hackers\" or \"This is frustrating\"", ConsoleColor.White);

            // Quiz Game
            _renderer.DisplayColoredMessage("\n🎮 QUIZ GAME:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("- Start quiz: \"start quiz\" or \"test my knowledge\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Answer by typing numbers (1, 2, etc.)", ConsoleColor.White);

            // Additional Features
            _renderer.DisplayColoredMessage("\n✨ OTHER FEATURES:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("- Jokes: \"Tell me a joke\" or \"That's funny\"", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Follow-up: \"Tell me more\" (after a topic)", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- Phishing tips: \"Give me a phishing tip\"", ConsoleColor.White);

            // Navigation
            _renderer.DisplayColoredMessage("\n🔧 SYSTEM COMMANDS:", ConsoleColor.Green);
            _renderer.DisplayColoredMessage("- \"help\" - Show this message", ConsoleColor.White);
            _renderer.DisplayColoredMessage("- \"exit\" or \"quit\" - Close the chatbot", ConsoleColor.White);

            _renderer.DisplayColoredMessage("\n💡 PRO TIP: You can use natural language for most commands!", ConsoleColor.Magenta);
        }

        private void ShowTopics()
        {
            _renderer.DisplayColoredMessage("Cybersecurity topics you can ask about:", ConsoleColor.Cyan);
            foreach (var topic in _keywordResponses.Keys)
            {
                _renderer.DisplayColoredMessage($"- {topic}", ConsoleColor.White);
            }
        }
    }
}