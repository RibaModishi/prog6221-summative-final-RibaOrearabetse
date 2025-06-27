using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    public class CyberTaskManager
    {
        private readonly List<CyberTask> _tasks = new List<CyberTask>();
        private readonly IResponseRenderer _renderer;

        public CyberTaskManager(IResponseRenderer renderer)
        {
            _renderer = renderer;
        }

        public void AddTask(CyberTask task)
        {
            if (task != null)
            {
                _tasks.Add(task);
            }
        }

        public void RemoveTask(CyberTask task)
        {
            if (task != null && _tasks.Contains(task))
            {
                _tasks.Remove(task);
            }
        }

        public List<CyberTask> GetTasks()
        {
            return new List<CyberTask>(_tasks);
        }

        public void AddDefaultTask()
        {
            _renderer.DisplayColoredMessage("Please provide a title:", ConsoleColor.Green);

            _tasks.Add(new CyberTask
            {
                Title = "Enable 2FA",
                Description = "Turn on two-factor authentication on email.",
                ReminderDate = DateTime.Now.AddDays(7)
            });

            _renderer.DisplayColoredMessage("Task added with 7-day reminder!", ConsoleColor.White);
        }

        public void ShowTasks()
        {
            if (_tasks.Count == 0)
            {
                _renderer.DisplayColoredMessage("You don't have any tasks yet.", ConsoleColor.Yellow);
                return;
            }

            _renderer.DisplayColoredMessage("Your cybersecurity tasks:", ConsoleColor.Cyan);
            foreach (var task in _tasks)
            {
                string reminder = task.ReminderDate.HasValue ? task.ReminderDate.Value.ToShortDateString() : "No reminder set";
                _renderer.DisplayColoredMessage($"- {task.Title}: {task.Description} (Reminder: {reminder})", ConsoleColor.White);
            }
        }
    }
}
