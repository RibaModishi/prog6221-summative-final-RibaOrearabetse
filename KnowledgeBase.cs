using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    

    public class KnowledgeBase
    {
        public Dictionary<string, string> Topics { get; private set; }
        public Dictionary<string, string> SmallTalk { get; private set; }
        public List<QuizQuestion> QuizQuestions { get; } = new List<QuizQuestion>
        {
            // Question 1 - Phishing
            new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> {
                    "Reply with your password",
                    "Delete the email",
                    "Report the email as phishing",
                    "Ignore it"
                },
                2,
                "Correct! Reporting phishing emails helps prevent scams.",
                "Never share passwords via email - this is always a scam!"),

            // Question 2 - Password Security
            new QuizQuestion(
                "A strong password should:",
                new List<string> {
                    "Contain your pet's name",
                    "Be at least 12 characters with symbols",
                    "Be written on a sticky note",
                    "Use 'password123'"
                },
                1,
                "Great! Complex passwords are harder to crack.",
                "Short/simple passwords can be guessed in seconds by hackers."),

            // Question 3 - Public Wi-Fi
            new QuizQuestion(
                "Public Wi-Fi networks are always safe for online banking.",
                new List<string> { "True", "False" },
                1,
                "Right! Public Wi-Fi can be intercepted by hackers.",
                "Always use a VPN or mobile data for sensitive transactions."),

            // Question 4 - 2FA
            new QuizQuestion(
                "Two-factor authentication (2FA) is:",
                new List<string> {
                    "An unnecessary extra step",
                    "Only for important accounts",
                    "A second verification method for logins",
                    "The same as a password manager"
                },
                2,
                "Exactly! 2FA adds an extra security layer.",
                "2FA requires both something you know (password) and something you have (phone)."),

            // Question 5 - Social Engineering
            new QuizQuestion(
                "Someone calls claiming to be from Microsoft support asking to remote into your computer. You should:",
                new List<string> {
                    "Let them help fix your computer",
                    "Provide your credit card for verification",
                    "Hang up immediately",
                    "Give them your email password"
                },
                2,
                "Good job! This is a common tech support scam.",
                "Legitimate companies will never call unsolicited for remote access."),

            // Question 6 - Software Updates
            new QuizQuestion(
                "Software updates should be installed:",
                new List<string> {
                    "Only when you have time",
                    "Immediately when available",
                    "Never - they break things",
                    "Only for operating systems"
                },
                1,
                "Correct! Updates often contain critical security patches.",
                "Delaying updates leaves you vulnerable to known exploits."),

            // Question 7 - Link Safety
            new QuizQuestion(
                "How can you check if a link is safe before clicking?",
                new List<string> {
                    "Hover to see the real URL",
                    "Ask the sender if it's safe",
                    "Click it to test",
                    "All links are safe"
                },
                0,
                "Right! Hovering reveals the true destination.",
                "Even links from friends can be compromised."),

            // Question 8 - Backups
            new QuizQuestion(
                "How often should you backup important files?",
                new List<string> {
                    "Never - cloud storage is enough",
                    "Only after a ransomware attack",
                    "Regularly (at least monthly)",
                    "Once when you buy a computer"
                },
                2,
                "Perfect! Regular backups prevent data loss.",
                "The 3-2-1 rule: 3 copies, 2 media types, 1 offsite."),

            // Question 9 - SIM Swap
            new QuizQuestion(
                "What's the first sign of a SIM swap attack?",
                new List<string> {
                    "Your phone gets hotter",
                    "You lose cellular service suddenly",
                    "Your battery drains faster",
                    "You get more spam calls"
                },
                1,
                "Yes! Unexpected loss of service is the key indicator.",
                "Contact your carrier immediately if this happens."),

            // Question 10 - Password Managers
            new QuizQuestion(
                "Password managers are:",
                new List<string> {
                    "Less secure than reusing passwords",
                    "Only for tech experts",
                    "A secure way to store unique passwords",
                    "Too expensive to use"
                },
                2,
                "Correct! They generate and store strong passwords for you.",
                "Modern password managers use strong encryption.")
        };

        private readonly string _userName;

        public KnowledgeBase(string userName)
        {
            _userName = userName;
            InitializeTopics();
            InitializeSmallTalk();
        }

        private void InitializeTopics()
        {
            Topics = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "phishing", "Phishing is a cyber attack where attackers impersonate trustworthy entities to steal personal information like passwords and credit card numbers. Always check the sender's email address and avoid clicking suspicious links." },
                { "password safety", "Password safety means creating strong passwords that are at least 12 characters long, including uppercase, lowercase letters, numbers, and special characters. Avoid reusing passwords and enable two-factor authentication." },
                { "safe browsing", "Safe browsing involves using updated browsers, avoiding suspicious websites, not clicking unknown links or ads, and being cautious when downloading files. Using HTTPS sites and a reputable antivirus program helps protection." },
                { "fake sars", "Fake SARS scams involve fraudulent emails, SMS, or calls pretending to be from the South African Revenue Service. They often ask for personal details or payments. Always verify through official SARS channels." },
                { "sim swap scams", "SIM swap scams happen when criminals trick mobile operators into transferring your phone number to a new SIM. This lets them intercept SMS-based two-factor authentication codes. Be alert to sudden loss of phone signal." },
                { "whatsapp scams", "WhatsApp scams often involve messages from unknown contacts asking for money or personal info, sometimes pretending to be family members. Never send money or share sensitive data without independent verification." }
            };
        }

        private void InitializeSmallTalk()
        {
            SmallTalk = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "hello", $"Hello {_userName}! How can I assist you with cybersecurity today?" },
                { "hi", $"Hi {_userName}! What cybersecurity topic would you like to learn about?" },
                { "how are you", "I'm a bot, so I'm always ready to help you stay safe online!" },
                { "tell me a joke", "Why did the computer get cold? Because it forgot to close its Windows!" },
                { "thanks", "You're welcome! Stay safe out there." },
                { "thank you", "Happy to help! Let me know if you want to learn about anything else." }
            };
        }
    }
}