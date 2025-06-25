using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    public class QuizQuestion
    {
        public string Question { get; }
        public List<string> Options { get; }
        public int CorrectAnswerIndex { get; }
        public string CorrectFeedback { get; }
        public string IncorrectFeedback { get; }

        public QuizQuestion(string question, List<string> options, int correctIndex,
                          string correctFeedback, string incorrectFeedback)
        {
            Question = question;
            Options = options;
            CorrectAnswerIndex = correctIndex;
            CorrectFeedback = correctFeedback;
            IncorrectFeedback = incorrectFeedback;
        }
    }
}