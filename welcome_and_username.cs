using System;
using System.Threading;

namespace dynabot_cybersecurity
{//start of namespace
    public class welcome_and_username
    {//start of class

        //global variable to store username
        private string username = string.Empty;

        //helper method to simulate typing effect
        public static void TypeWrite(string message, ConsoleColor color, int delay = 30)
        {//start of method
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
            Console.ResetColor();
        }//end of method

        //void method to welcome the user
        public void welcome()
        {//start of void method
            Console.ForegroundColor = ConsoleColor.Cyan;
            TypeWrite("  ╔══════════════════════════════════════╗", ConsoleColor.Cyan);
            TypeWrite("  ║       DYNABOT CYBERSECURITY BOT      ║", ConsoleColor.Yellow);
            TypeWrite("  ║   Keeping You Safe in the Digital    ║", ConsoleColor.Yellow);
            TypeWrite("  ║              World                   ║", ConsoleColor.Yellow);
            TypeWrite("  ╚══════════════════════════════════════╝", ConsoleColor.Cyan);
            Console.WriteLine();
            Console.ResetColor();
        }//end of void method

        //method to ask for the username
        public void ask_user()
        {//start of method
            TypeWrite("Dynabot : Hello! Welcome to the Cybersecurity Awareness Bot.", ConsoleColor.Blue);
            TypeWrite("Dynabot : I'm here to help you stay safe online.", ConsoleColor.Blue);
            Console.WriteLine();
            TypeWrite("Dynabot : Enter your name...", ConsoleColor.Yellow);

            //do while to re-prompt if name is empty
            do
            {//start of do while
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("  USER  : ");
                Console.ForegroundColor = ConsoleColor.White;
                username = Console.ReadLine();
                Console.ResetColor();
            } while (!empty());//end of do while

        }//end of method

        //Boolean method to check if username is not empty
        private Boolean empty()
        {//start of boolean method
            if (username != "")
            {//start of if
                Console.WriteLine();
                TypeWrite("Dynabot : Hey " + username + "! Great to meet you. Let's get started!", ConsoleColor.Blue);
                Console.WriteLine();
                return true;
            }//end of if
            else
            {//start of else
                TypeWrite("Dynabot : Oops! You didn't enter a name. Please try again.", ConsoleColor.Red);
                return false;
            }//end of else
        }//end of boolean method

        //public getter for username so Program.cs can access it
        public string GetUsername()
        {
            return username;
        }

    }//end of class
}//end of namespace