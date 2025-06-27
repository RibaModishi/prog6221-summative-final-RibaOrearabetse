
 Cybersecurity Awareness Chatbot – Project Overview

 📌 Project Title

Cybersecurity Awareness Chatbot (WPF GUI)
Final Summative Project – PROG6221 - ST10446648

🎯 What’s the purpose of this project?

The aim of this project is to build a simple but smart chatbot that helps South African users become more aware of online threats. The chatbot answers questions about topics like phishing, passwords, malware, VPNs, and more. It also includes fun features like a cybersecurity quiz, task reminders, and a system that understands natural language (even if you don’t type in perfect commands).

The goal was to make learning about cybersecurity **interactive and engaging** while also giving users tools to stay safe online.


 💡 Features I’ve Implemented

| Feature                      | What it does                                                                                             |
| ---------------------------- | -------------------------------------------------------------------------------------------------------- |
| **Chatbot Chat**             | You can type in questions or commands and the bot responds with helpful cybersecurity info.              |
| **Task Assistant**           | Lets you add and manage personal cybersecurity tasks or reminders (like “Update password”).              |
| **Natural Language Support** | You don’t have to use perfect commands—typing things like “Remind me to change my password” still works. |
| **Quiz Game**                | Tests your cybersecurity knowledge with multiple choice questions and feedback.                          |
| **Activity Log**             | Keeps a short history of what the chatbot has helped you with (like tasks or quiz attempts).             |
| **Jokes & Small Talk**       | Just for fun! The bot can tell jokes and respond to greetings like “hi” or “what’s up”.                  |



 🧰 What I Used to Build It

* C# and WPF (Windows Presentation Foundation) – Used for both the backend logic and the graphical user interface.
* OOP (Object-Oriented Programming) – Everything is broken up into classes like `InputProcessor`, `QuizGame`, and `CyberTaskManager`.
* Basic NLP Techniques – I used string manipulation (like `Contains`) to detect what users are saying even if they type differently.
* No third-party libraries – I built everything using only what’s available in .NET and Visual Studio, to show I understand the basics.



 🔎 How It’s All Structured

Here’s a simplified view of how the program works:


+--------------------+
|   MainWindow.xaml  | ← The interface (what the user sees)
+--------------------+
          ↓
+--------------------+
|   InputProcessor   | ← Handles all logic and detects what users want
+--------------------+
  ↓         ↓         ↓
QuizGame   CyberTaskManager   KnowledgeBase
  ↓              ↓               ↓
Shows quiz   Manages tasks   Gives cybersecurity info




 📝 Example: What the Chatbot Can Do

Let’s say a user types:

* "Remind me to update my password" → It will create a task.
* "Start quiz" → It launches the cybersecurity quiz.
* "What have you done for me?" → It shows the activity log like this:

> ```
> Here's what I’ve done recently:
> 1. Task added: 'Update my password'
> 2. Quiz started - 10 questions loaded
> 3. Reminder set for: Update privacy settings (in 3 days)
> ```

---

 ✅ What I’ve Learned

This project helped me improve my understanding of:

* How to use C# and WPF to create desktop applications.
* Handling natural language input using basic string logic.
* Structuring code using OOP (classes, interfaces, constructors).
* Building interactive UIs with buttons, tabs, and event handlers.
* Making software more "human-friendly" and not just strict commands.


