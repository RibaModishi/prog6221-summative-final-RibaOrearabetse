using System;
using System.Windows;
using System.Windows.Controls;
using CybersecurityAwarenessBot;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityAwarenessBotGUI
{
    public partial class MainWindow : Window
    {
        private InputProcessor _processor;
        private QuizGame _quizGame;
        private int _currentQuizIndex = -1;
        private int _quizScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
        }

        private void InitializeChatbot()
        {
            var renderer = new GUIRenderer(ChatStack);
            var audioService = new AudioService();
            var knowledge = new KnowledgeBase("User");
            _processor = new InputProcessor(knowledge, renderer, audioService, "User");
            _quizGame = new QuizGame(renderer, knowledge.QuizQuestions);

            _processor.ProcessInput("hello");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
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

                if (_currentQuizIndex >= 0)
                {
                    ProcessQuizAnswer(input);
                }
                else
                {
                    bool exit = _processor.ProcessInput(input);
                    if (exit) Close();
                }
            }
        }

        private void ProcessQuizAnswer(string input)
        {
            if (int.TryParse(input, out int answer) && answer > 0 && answer <= 4)
            {
                _quizGame.ProcessAnswer(answer - 1, _currentQuizIndex);
                _currentQuizIndex++;

                if (_currentQuizIndex < _quizGame.Questions.Count)
                {
                    ShowQuizQuestion();
                }
                else
                {
                    _quizGame.EndQuiz();
                    _currentQuizIndex = -1;
                }
            }
            else
            {
                _processor._renderer.DisplayColoredMessage("Please enter a number between 1-4", ConsoleColor.Red);
            }
        }

        private void ShowQuizQuestion()
        {
            var question = _quizGame.Questions[_currentQuizIndex];
            _processor._renderer.DisplayColoredMessage($"\nQuestion {_currentQuizIndex + 1}: {question.Question}", ConsoleColor.Cyan);

            for (int i = 0; i < question.Options.Count; i++)
            {
                _processor._renderer.DisplayColoredMessage($"{i + 1}. {question.Options[i]}", ConsoleColor.White);
            }
        }
    }
}