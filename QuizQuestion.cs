using System.Collections.Generic;

public class QuizQuestion
{
    public string Question { get; }
    public List<string> Options { get; }
    public int CorrectAnswerIndex { get; }
    public string Explanation { get; }
    public string Topic { get; }

    public QuizQuestion(string question, List<string> options, int correctAnswerIndex, string explanation, string topic)
    {
        Question = question;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
        Explanation = explanation;
        Topic = topic;
    }
}
