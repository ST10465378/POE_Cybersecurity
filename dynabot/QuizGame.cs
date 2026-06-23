using System;
using System.Collections.Generic;

namespace dynabot
{// start of namespace

    // ── simple data structure to represent one quiz question ───────────
    public class QuizQuestion
    {// start of QuizQuestion class
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }   // null for true/false questions
        public string CorrectAnswer { get; set; }    // e.g. "C" or "True"
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }
    }// end of QuizQuestion class

    internal class QuizGame
    {// start of QuizGame class

        // ── quiz state ─────────────────────────────────────────────────
        private List<QuizQuestion> questions;
        private int currentIndex = 0;
        private int score = 0;
        public bool IsActive { get; private set; } = false;

        // ── constructor: build the 12 questions ──────────────────────────
        public QuizGame()
        {// start of constructor
            questions = BuildQuestions();
        }// end of constructor

        // ── build the question bank (12 mixed MCQ / true-false) ────────
        private List<QuizQuestion> BuildQuestions()
        {// start of method

            List<QuizQuestion> q = new List<QuizQuestion>();

            q.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Correct! Reporting phishing emails helps prevent scams.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "A strong password should include uppercase, lowercase, numbers, and symbols.",
                Options = null,
                CorrectAnswer = "True",
                Explanation = "Correct! Mixing character types makes passwords much harder to crack.",
                IsTrueFalse = true
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "What is the safest action when connecting to public Wi-Fi?",
                Options = new List<string> { "A) Do online banking freely", "B) Use a VPN", "C) Share your password", "D) Disable your firewall" },
                CorrectAnswer = "B",
                Explanation = "Correct! A VPN encrypts your traffic on untrusted networks.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "Two-factor authentication (2FA) adds an extra layer of security to your accounts.",
                Options = null,
                CorrectAnswer = "True",
                Explanation = "Correct! 2FA makes it much harder for attackers to gain access.",
                IsTrueFalse = true
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "Which of these is a common sign of a phishing email?",
                Options = new List<string> { "A) Urgent demands for personal info", "B) A company logo", "C) Correct grammar", "D) A known sender" },
                CorrectAnswer = "A",
                Explanation = "Correct! Urgency and pressure tactics are classic phishing red flags.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "It's safe to reuse the same password across multiple accounts.",
                Options = null,
                CorrectAnswer = "False",
                Explanation = "Incorrect logic avoided! Reusing passwords means one breach compromises all your accounts.",
                IsTrueFalse = true
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "What does malware typically aim to do?",
                Options = new List<string> { "A) Improve your device speed", "B) Harm or exploit your device", "C) Update your software", "D) Back up your files" },
                CorrectAnswer = "B",
                Explanation = "Correct! Malware is software designed to damage or exploit systems.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "HTTPS in a website URL means the connection is encrypted.",
                Options = null,
                CorrectAnswer = "True",
                Explanation = "Correct! HTTPS encrypts data between your browser and the website.",
                IsTrueFalse = true
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "What should you do immediately if your account is hacked?",
                Options = new List<string> { "A) Ignore it", "B) Change your password and enable 2FA", "C) Wait a week", "D) Share it with friends" },
                CorrectAnswer = "B",
                Explanation = "Correct! Securing the account immediately limits further damage.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "A firewall helps block unauthorised access to your network.",
                Options = null,
                CorrectAnswer = "True",
                Explanation = "Correct! Firewalls filter traffic based on security rules.",
                IsTrueFalse = true
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "Which practice helps protect your online privacy?",
                Options = new List<string> { "A) Posting your address publicly", "B) Reviewing privacy settings regularly", "C) Using the same password everywhere", "D) Disabling antivirus" },
                CorrectAnswer = "B",
                Explanation = "Correct! Regularly reviewing privacy settings limits exposure of personal data.",
                IsTrueFalse = false
            });

            q.Add(new QuizQuestion
            {
                QuestionText = "Scammers never impersonate trusted organisations.",
                Options = null,
                CorrectAnswer = "False",
                Explanation = "Incorrect logic avoided! Scammers frequently impersonate banks, companies, and government bodies.",
                IsTrueFalse = true
            });

            return q;

        }// end of method

        // ── start a new quiz attempt ───────────────────────────────────
        public void StartQuiz()
        {// start of method
            currentIndex = 0;
            score        = 0;
            IsActive     = true;
        }// end of method

        // ── get the current question formatted for display ─────────────
        public string GetCurrentQuestionText()
        {// start of method

            if (currentIndex >= questions.Count)
            {// start of if
                return null;
            }// end of if

            QuizQuestion current = questions[currentIndex];
            string text = "Question " + (currentIndex + 1) + " of " + questions.Count + ":\n" + current.QuestionText;

            if (!current.IsTrueFalse)
            {// start of if
                foreach (string option in current.Options)
                {// start of foreach
                    text += "\n" + option;
                }// end of foreach
            }// end of if
            else
            {// start of else
                text += "\n(Answer True or False)";
            }// end of else

            return text;

        }// end of method

        // ── submit an answer and return feedback ────────────────────────
        public string SubmitAnswer(string userAnswer)
        {// start of method

            if (currentIndex >= questions.Count)
            {// start of if
                return null;
            }// end of if

            QuizQuestion current = questions[currentIndex];

            // normalise the user's answer (accepts "a", "A)", "true", etc.)
            string cleaned = userAnswer.Trim().ToUpper().Replace(")", "");

            bool isCorrect;

            if (current.IsTrueFalse)
            {// start of if
                isCorrect = cleaned.StartsWith("T") && current.CorrectAnswer == "True"
                         || cleaned.StartsWith("F") && current.CorrectAnswer == "False";
            }// end of if
            else
            {// start of else
                // accept just the letter, e.g. "C" or "C)" or "c) report..."
                isCorrect = cleaned.Length > 0 && cleaned[0].ToString() == current.CorrectAnswer;
            }// end of else

            string feedback;

            if (isCorrect)
            {// start of if
                score++;
                feedback = "✅ " + current.Explanation;
            }// end of if
            else
            {// start of else
                feedback = "❌ Not quite. The correct answer was " + current.CorrectAnswer + ". " + current.Explanation;
            }// end of else

            currentIndex++;

            return feedback;

        }// end of method

        // ── check if the quiz has finished ─────────────────────────────
        public bool IsFinished()
        {// start of method
            return currentIndex >= questions.Count;
        }// end of method

        // ── get the final score message ────────────────────────────────
        public string GetFinalScoreMessage()
        {// start of method

            IsActive = false;

            string feedback;
            double percentage = (double)score / questions.Count * 100;

            if (percentage >= 80)
            {// start of if
                feedback = "Great job! You're a cybersecurity pro! 🏆";
            }// end of if
            else if (percentage >= 50)
            {// start of else if
                feedback = "Good effort! Keep learning to stay even safer online. 💪";
            }// end of else if
            else
            {// start of else
                feedback = "Keep learning to stay safe online! Practice makes perfect. 📚";
            }// end of else

            return "Quiz complete! You scored " + score + " out of " + questions.Count + ".\n" + feedback;

        }// end of method

        // ── reset the quiz state ───────────────────────────────────────
        public void EndQuiz()
        {// start of method
            IsActive = false;
        }// end of method

    }// end of QuizGame class

}// end of namespace
