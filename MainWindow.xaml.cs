using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CybersecurityAwarenessBot;

namespace CybersecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private InputProcessor _processor;
        private QuizGame _quizGame;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
        }

        // ===== CHAT FUNCTIONALITY =====
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string input = UserInputBox.Text.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                _processor.ProcessInput(input);
                UserInputBox.Clear();
            }
        }

        // ===== TASK MANAGEMENT =====
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskTitleBox.Text))
            {
                var task = new CyberTask
                {
                    Title = TaskTitleBox.Text,
                    Description = TaskDescBox.Text,
                    ReminderDate = TaskReminderDate.SelectedDate
                };

                _processor.TaskManager.AddTask(task);
                RefreshTaskList();

                TaskTitleBox.Text = "";
                TaskDescBox.Text = "";
                TaskReminderDate.SelectedDate = null;

                _processor.DisplayMessage("Task added successfully!", ConsoleColor.Green);

                _processor.LogActionExternally($"Task added: '{task.Title}' with reminder {task.ReminderDate?.ToShortDateString() ?? "none"}");
            }
            else
            {
                _processor.DisplayMessage("Task title cannot be empty", ConsoleColor.Red);
            }
        }


        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = _processor.TaskManager.GetTasks();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is CyberTask selectedTask)
            {
                _processor.TaskManager.RemoveTask(selectedTask);
                RefreshTaskList();
                _processor.DisplayMessage("Task deleted", ConsoleColor.Green);
            }
            else
            {
                _processor.DisplayMessage("Please select a task to delete", ConsoleColor.Red);
            }
        }

        // ===== QUIZ FUNCTIONALITY =====
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int selectedIndex)
            {
                foreach (var child in QuizOptionsPanel.Children)
                {
                    if (child is Button btn) btn.IsEnabled = false;
                }

                _quizGame.CheckAnswer(selectedIndex);
            }
        }

        private void NextQuizQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_quizGame == null) return;

            if (!_quizGame.IsActive)
            {
                _quizGame.StartQuiz();
                NextQuizButton.Content = "Next Question";
            }
            else
            {
                _quizGame.NextQuestion();
            }
        }

        private void InitializeChatbot()
        {
            var renderer = new GUIRenderer(ChatStack);
            var audioService = new AudioService();
            var knowledge = new KnowledgeBase("User");
            _processor = new InputProcessor(knowledge, renderer, audioService, "User");

            var quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion("What should you do if you receive an email asking for your password?",
                    new List<string> { "Reply with it", "Delete the email", "Report as phishing", "Forward to coworkers" }, 2,
                    "Legitimate organizations will never ask for passwords via email.", "Phishing"),

                new QuizQuestion("What makes a strong password?",
                    new List<string> { "Short and simple", "Your pet's name", "Random characters with numbers/symbols", "Your birthday" }, 2,
                    "Strong passwords should be long, random, and unique.", "Passwords"),

                new QuizQuestion("Is it safe to use public WiFi for banking?",
                    new List<string> { "Yes, always", "Only if network has a password", "Never", "Only with VPN" }, 3,
                    "Use a VPN on public WiFi to stay safe.", "Network Security"),

                new QuizQuestion("A caller says they're IT support and needs remote access. What do you do?",
                    new List<string> { "Give access", "Verify through official contacts", "Give employee ID", "Hang up" }, 1,
                    "Verify all unexpected IT requests through official methods.", "Social Engineering"),

                new QuizQuestion("What's safest way to open email attachments?",
                    new List<string> { "Open all", "Only from known senders", "Scan with antivirus first", "Never open" }, 2,
                    "Scan attachments even from trusted sources.", "Malware"),

                new QuizQuestion("Why install updates promptly?",
                    new List<string> { "They're annoying", "Fix security bugs", "Slow PC", "Change settings" }, 1,
                    "Updates fix critical security flaws.", "System Security"),

                new QuizQuestion("2FA means:",
                    new List<string> { "Two passwords", "Two verification steps", "Slower login", "More work" }, 1,
                    "2FA adds a second verification layer for better security.", "Authentication"),

                new QuizQuestion("How often should you back up files?",
                    new List<string> { "Never", "Once a year", "When you remember", "Regularly on a schedule" }, 3,
                    "Backups prevent data loss from malware or hardware failure.", "Data Protection"),

                new QuizQuestion("You find a USB in the parking lot. What do you do?",
                    new List<string> { "Plug into PC", "Give to security", "Take it home", "Format it" }, 1,
                    "Unknown USBs may contain malware.", "Physical Security"),

                new QuizQuestion("What shouldn’t you share on social media?",
                    new List<string> { "Favorite movie", "Mom’s maiden name", "Lunch pic", "All of the above" }, 1,
                    "Personal info like maiden names can be used to guess passwords.", "Privacy")
            };

            _quizGame = new QuizGame(
                renderer,
                quizQuestions,
                OnQuizEnd,
                DisplayCurrentQuestion,
                feedback => QuizFeedbackLabel.Content = feedback,
                result => QuizResultText.Text = result
            );

            _processor.DisplayMessage("Hello! Ready to test your cybersecurity knowledge?", ConsoleColor.Cyan);
        }

        private void DisplayCurrentQuestion(QuizQuestion question)
        {
            QuizQuestionText.Text = $"Question {_quizGame.CurrentQuestionNumber}: {question.Question}";
            CreateAnswerButtons(question);
            QuizFeedbackLabel.Content = ""; // Clear previous feedback
        }

        private void OnQuizEnd()
        {
            QuizOptionsPanel.Children.Clear();
            QuizQuestionText.Text = "🎓 Quiz completed! Check your final score above.";
            NextQuizButton.IsEnabled = false;
        }

        private void CreateAnswerButtons(QuizQuestion question)
        {
            QuizOptionsPanel.Children.Clear();

            for (int i = 0; i < question.Options.Count; i++)
            {
                var button = new Button
                {
                    Content = $"{i + 1}. {question.Options[i]}",
                    Tag = i,
                    Margin = new Thickness(5),
                    IsEnabled = true
                };
                button.Click += AnswerButton_Click;
                QuizOptionsPanel.Children.Add(button);
            }
        }
    }
}
