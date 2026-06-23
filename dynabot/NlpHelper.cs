using System;
using System.Collections.Generic;

namespace dynabot
{// start of namespace

    internal class NlpHelper
    {// start of NlpHelper class

        // ── phrases that imply "add a task" ─────────────────────────────
        private static List<string> taskPhrases = new List<string>
        {
            "add a task", "add task", "create a task", "new task",
            "remind me to", "set a reminder", "i need to remember",
            "add reminder", "schedule"
        };

        // ── phrases that imply "view tasks" ───────────────────────────
        private static List<string> viewTaskPhrases = new List<string>
        {
            "show my tasks", "view tasks", "list tasks", "my tasks",
            "what tasks do i have", "show tasks"
        };

        // ── phrases that imply "start quiz" ─────────────────────────────
        private static List<string> quizPhrases = new List<string>
        {
            "start quiz", "take a quiz", "quiz me", "test my knowledge",
            "play quiz", "start the quiz", "begin quiz"
        };

        // ── phrases that imply "show activity log" ──────────────────────
        private static List<string> logPhrases = new List<string>
        {
            "show activity log", "what have you done for me", "activity log",
            "show log", "show my activity", "what have you done", "show history"
        };

        // ── detect intent from free-form user input ─────────────────────
        public static string DetectIntent(string input)
        {// start of method

            string lower = input.ToLower();

            foreach (string phrase in logPhrases)
            {// start of foreach
                if (lower.Contains(phrase)) return "activity_log";
            }// end of foreach

            foreach (string phrase in quizPhrases)
            {// start of foreach
                if (lower.Contains(phrase)) return "start_quiz";
            }// end of foreach

            foreach (string phrase in viewTaskPhrases)
            {// start of foreach
                if (lower.Contains(phrase)) return "view_tasks";
            }// end of foreach

            foreach (string phrase in taskPhrases)
            {// start of foreach
                if (lower.Contains(phrase)) return "add_task";
            }// end of foreach

            return "none";

        }// end of method

        // ── extract a usable task title from free-form input ─────────────
        // e.g. "remind me to update my password" -> "update my password"
        public static string ExtractTaskTitle(string input)
        {// start of method

            string lower = input.ToLower();

            foreach (string phrase in taskPhrases)
            {// start of foreach
                int index = lower.IndexOf(phrase);
                if (index >= 0)
                {// start of if
                    string remainder = input.Substring(index + phrase.Length).Trim();

                    if (remainder.Length > 0)
                    {// start of inner if
                        return char.ToUpper(remainder[0]) + remainder.Substring(1);
                    }// end of inner if
                }// end of if
            }// end of foreach

            // fallback: return the whole input capitalised
            return char.ToUpper(input[0]) + input.Substring(1);

        }// end of method

    }// end of NlpHelper class

}// end of namespace
