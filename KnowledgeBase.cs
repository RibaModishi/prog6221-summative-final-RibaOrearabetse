using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{
    public class KnowledgeBase
    {
        // Stores cybersecurity topics and their explanations
        public Dictionary<string, string> Topics { get; private set; }

        // Stores small talk responses to make the bot more engaging
        public Dictionary<string, string> SmallTalk { get; private set; }

        // User's name for personalized responses
        private readonly string _userName;

        // Constructor initializes the knowledge base with the user's name
        public KnowledgeBase(string userName)
        {
            _userName = userName;
            InitializeTopics();     // Load cybersecurity topic responses
            InitializeSmallTalk();  // Load small talk conversation options
        }

        /// <summary>
        /// Populates the Topics dictionary with educational content about common cyber threats,
        /// including local South African examples like fake SARS and SIM swap scams.
        /// </summary>
        private void InitializeTopics()
        {
            Topics = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "phishing",
                  "Phishing is a cyber attack where attackers impersonate trustworthy entities, " +
                  "like banks or government services, to steal your personal information such as passwords and credit card numbers. " +
                  "Always check the sender's email address and avoid clicking suspicious links."
                },
                { "password safety",
                  "Password safety means creating strong passwords that are at least 12 characters long, " +
                  "including uppercase, lowercase letters, numbers, and special characters. " +
                  "Avoid reusing passwords and enable two-factor authentication whenever possible."
                },
                { "safe browsing",
                  "Safe browsing involves using updated browsers, avoiding suspicious websites, " +
                  "not clicking unknown links or ads, and being cautious when downloading files. " +
                  "Using HTTPS sites and a reputable antivirus program can help protect you online."
                },
                { "fake sars",
                  "Fake SARS scams involve fraudulent emails, SMS, or calls pretending to be from the South African Revenue Service. " +
                  "They often ask for personal details or payments. Always verify any SARS communication by contacting SARS directly through official channels."
                },
                { "sim swap scams",
                  "SIM swap scams happen when criminals trick mobile operators into transferring your phone number to a new SIM. " +
                  "This can allow them to intercept SMS-based two-factor authentication codes and access your accounts. " +
                  "Be alert to sudden loss of phone signal and notify your provider immediately if this happens."
                },
                { "whatsapp scams",
                  "WhatsApp scams often involve messages from unknown contacts asking for money or personal info, sometimes pretending to be family members. " +
                  "Never send money or share sensitive data without verifying the request independently."
                }
            };
        }

        /// <summary>
        /// Populates the SmallTalk dictionary with casual conversation responses
        /// to make the chatbot more friendly and interactive.
        /// </summary>
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
