using System;

namespace dynabot_cybersecurity
{//start of namespace
    public class chats
    {//start of class

        //void method to show a demo chat with colors
        public void show_chats()
        {//start of void method

            //section header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ──────────────────────────────────────");
            Console.ResetColor();

            //chatbot name in blue
            welcome_and_username.TypeWrite("  Dynabot  : ", ConsoleColor.Blue, 20);
            //chatbot message in red
            welcome_and_username.TypeWrite("  Welcome! What is your name, user?", ConsoleColor.Red, 30);

            //user name in gray
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  USER     : ");
            //user message in yellow
            welcome_and_username.TypeWrite("Hi, my name is Don Ladino", ConsoleColor.Yellow, 30);

            //section divider
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ──────────────────────────────────────");
            Console.ResetColor();
            Console.WriteLine();

        }//end of void method

    }//end of class
}//end of namespace