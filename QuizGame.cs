using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CybersecurityAwarenessBot;

public class QuizGame
{
    private readonly IResponseRenderer _renderer;
    public List<QuizQuestion> Questions { get; }
    private int _currentQuestionIndex = -1;
    private int _score;
    private bool _isQuizActive = false;

    private readonly Action _onQuizEnd;
    private readonly Action<QuizQuestion> _onQuestionDisplay;
    private readonly Action<string> _onFeedbackDisplay;
    private readonly Action<string> _onFinalResultDisplay;

    public int CurrentQuestionNumber => _currentQuestionIndex + 1;
    public bool IsActive => _isQuizActive;

    public QuizGame(
        IResponseRenderer renderer,
        List<QuizQuestion> questions,
        Action onQuizEnd,
        Action<QuizQuestion> onQuestionDisplay,
        Action<string> onFeedbackDisplay,
        Action<string> onFinalResultDisplay)
    {
        _renderer = renderer;
        Questions = questions;
        _onQuizEnd = onQuizEnd;
        _onQuestionDisplay = onQuestionDisplay;
        _onFeedbackDisplay = onFeedbackDisplay;
        _onFinalResultDisplay = onFinalResultDisplay;
    }

    public void StartQuiz()
    {
        _currentQuestionIndex = 0;
        _score = 0;
        _isQuizActive = true;
        _onQuestionDisplay?.Invoke(Questions[_currentQuestionIndex]);
    }

    public void NextQuestion()
    {
        _currentQuestionIndex++;

        if (_currentQuestionIndex < Questions.Count)
        {
            _onQuestionDisplay?.Invoke(Questions[_currentQuestionIndex]);
        }
        else
        {
            EndQuiz();
        }
    }

    public void CheckAnswer(int selectedOptionIndex)
    {
        if (!_isQuizActive) return;

        var question = Questions[_currentQuestionIndex];
        string feedback;

        if (selectedOptionIndex == question.CorrectAnswerIndex)
        {
            _score++;
            feedback = $"✅ Correct! {question.Explanation}";
            _renderer.DisplayColoredMessage(feedback, ConsoleColor.Green);
        }
        else
        {
            feedback = $"❌ Incorrect. The correct answer was: {question.Options[question.CorrectAnswerIndex]}.\n{question.Explanation}";
            _renderer.DisplayColoredMessage(feedback, ConsoleColor.Red);
        }

        _onFeedbackDisplay?.Invoke(feedback);

        // Delay and move to next question
        Task.Delay(2000).ContinueWith(_ =>
        {
            Application.Current.Dispatcher.Invoke(() => NextQuestion());
        });
    }

    public void EndQuiz()
    {
        _isQuizActive = false;
        double percentage = (_score * 100.0) / Questions.Count;

        string feedback;
        if (percentage >= 80)
            feedback = "🎉 Excellent! You're a cybersecurity pro!";
        else if (percentage >= 60)
            feedback = "👍 Good job! You're getting there!";
        else
            feedback = "📚 Keep practicing to improve your security knowledge!";

        string result = $"🏁 Quiz complete!\nScore: {_score}/{Questions.Count} ({percentage:0}%)\n{feedback}";

        _renderer.DisplayColoredMessage(result, ConsoleColor.Yellow);
        _onFeedbackDisplay?.Invoke(""); // Clear question feedback
        _onFinalResultDisplay?.Invoke(result);
        _onQuizEnd?.Invoke();
    }
}
