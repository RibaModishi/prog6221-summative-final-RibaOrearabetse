using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    public class QuizGame
    {
        private readonly IResponseRenderer _renderer;
        public List<QuizQuestion> Questions { get; }
        private int _score = 0;

        public QuizGame(IResponseRenderer renderer, List<QuizQuestion> questions)
        {
            _renderer = renderer;
            Questions = questions;
        }

        public void StartQuiz()
        {
            _score = 0;
            _renderer.DisplayColoredMessage("\n📝 Cybersecurity Quiz Started!", ConsoleColor.Magenta);
            _renderer.DisplayColoredMessage("Answer with numbers 1-4", ConsoleColor.Cyan);
        }

        public void ProcessAnswer(int selectedOption, int questionIndex)
        {
            var question = Questions[questionIndex];
            bool isCorrect = selectedOption == question.CorrectAnswerIndex;

            if (isCorrect)
            {
                _score++;
                _renderer.DisplayColoredMessage($"✓ {question.CorrectFeedback}", ConsoleColor.Green);
            }
            else
            {
                _renderer.DisplayColoredMessage($"✗ {question.IncorrectFeedback}", ConsoleColor.Red);
            }
        }

        public void EndQuiz()
        {
            double percentage = (_score * 100) / Questions.Count;
            _renderer.DisplayColoredMessage($"\nQuiz Complete! Score: {_score}/{Questions.Count}", ConsoleColor.Magenta);

            if (percentage >= 80)
                _renderer.DisplayColoredMessage("Cyber Security Expert! 🎉", ConsoleColor.Green);
            else if (percentage >= 50)
                _renderer.DisplayColoredMessage("Good start! Keep learning.", ConsoleColor.Yellow);
            else
                _renderer.DisplayColoredMessage("Review cybersecurity basics to stay safe.", ConsoleColor.Red);
        }
    }
}