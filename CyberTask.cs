using System;

namespace CybersecurityAwarenessBot
{
    public class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Description} {(ReminderDate.HasValue ? $"(Remind on: {ReminderDate.Value.ToShortDateString()})" : "")}";
        }
    }
}