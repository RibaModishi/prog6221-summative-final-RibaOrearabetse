using System;
using System.Windows;
using System.Windows.Controls;
using CybersecurityAwarenessBot;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace CybersecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private InputProcessor _processor;
        private DispatcherTimer _reminderTimer;
        private List<QuizQuestion> _quizQuestions;
        private int _currentQuizIndex = 0;
        private int _quizScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
            InitializeReminderSystem();
            InitializeQuizGame();
        }

        private void InitializeChatbot()
        {
            var renderer = new GUIRenderer(ChatStack);
            var audioService = new AudioService();
            var knowledge = new KnowledgeBase("User");
            _processor = new InputProcessor(knowledge, renderer, audioService, "User");

            // Play welcome message
            _processor.ProcessInput("hello");
        }

        private void InitializeReminderSystem()
        {
            _reminderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1) // Check every minute
            };
            _reminderTimer.Tick += (s, e) => CheckReminders();
            _reminderTimer.Start();
        }

        private void InitializeQuizGame()
        {
            _quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What is the most secure password practice?",
                    new List<string> { "Using the same password everywhere", "Password with 8 characters", "Password with 12+ characters and symbols", "Writing passwords on paper" },
                    2),
                new QuizQuestion(
                    "What should you do if you receive a suspicious email?",
                    new List<string> { "Click links to check", "Forward to IT department", "Reply asking if it's real", "Download attachments" },
                    1),
                // Add more questions
            };
            ShowNextQuizQuestion();
        }

        private void ShowNextQuizQuestion()
        {
            if (_currentQuizIndex >= _quizQuestions.Count)
            {
                QuizQuestion.Text = "Quiz completed!";
                NextQuizButton.IsEnabled = false;
                return;
            }

            var question = _quizQuestions[_currentQuizIndex];
            QuizQuestion.Text = question.QuestionText;
            QuizOptions.Children.Clear();

            for (int i = 0; i < question.Options.Count; i++)
            {
                var radio = new RadioButton
                {
                    Content = question.Options[i],
                    Margin = new Thickness(5),
                    Tag = i
                };
                QuizOptions.Children.Add(radio);
            }
        }

        private void CheckReminders()
        {
            var dueTasks = _processor.Tasks
                .Where(t => t.ReminderDate.HasValue &&
                           t.ReminderDate.Value.Date == DateTime.Now.Date &&
                           !t.IsCompleted)
                .ToList();

            foreach (var task in dueTasks)
            {
                MessageBox.Show($"Reminder: {task.Title}\n{task.Description}", "Task Reminder");
                task.ReminderDate = null; // Clear reminder after showing
            }
        }

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
            string input = UserInputBox.Text;
            UserInputBox.Text = "";

            if (!string.IsNullOrWhiteSpace(input))
            {
                var userText = new TextBlock
                {
                    Text = $"You: {input}",
                    Margin = new Thickness(5),
                    FontWeight = FontWeights.Bold
                };
                ChatStack.Children.Add(userText);

                bool exit = _processor.ProcessInput(input);
                if (exit) Close();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var task = new CyberTask
            {
                Title = TaskTitleBox.Text,
                Description = TaskDescBox.Text,
                ReminderDate = TaskReminderDate.SelectedDate
            };

            _processor.Tasks.Add(task);
            RefreshTaskList();

            TaskTitleBox.Text = "";
            TaskDescBox.Text = "";
            TaskReminderDate.SelectedDate = null;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is CyberTask task)
            {
                _processor.Tasks.Remove(task);
                RefreshTaskList();
            }
        }

        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = _processor.Tasks;
        }

        private void NextQuizQuestion_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = QuizOptions.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked == true);

            if (selectedOption != null)
            {
                int selectedIndex = (int)selectedOption.Tag;
                if (selectedIndex == _quizQuestions[_currentQuizIndex].CorrectAnswerIndex)
                {
                    _quizScore++;
                    QuizResult.Text = "Correct!";
                    QuizResult.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    QuizResult.Text = "Incorrect!";
                    QuizResult.Foreground = System.Windows.Media.Brushes.Red;
                }

                _currentQuizIndex++;
                ShowNextQuizQuestion();
            }
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; }
        public List<string> Options { get; }
        public int CorrectAnswerIndex { get; }

        public QuizQuestion(string question, List<string> options, int correctIndex)
        {
            QuestionText = question;
            Options = options;
            CorrectAnswerIndex = correctIndex;
        }
    }
}