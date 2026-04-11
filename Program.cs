using System;

namespace dynabot_cybersecurity
{//start of namespace
    public class Program
    {//start of class
        static void Main(string[] args)
        {//start of method

            // create voice greeting class
            new voice_greeting();

            // create ascii logo class
            new ascii_logo();

            // create classes to dispaly welcome screen and ask user for name
            welcome_and_username welcome_and_collect = new welcome_and_username();
            welcome_and_collect.welcome();
            welcome_and_collect.ask_user();

            //get the username
            string collected_name = welcome_and_collect.GetUsername();

            // show the colour demo for the chat before the chat begins
            chats show_message = new chats();
            show_message.show_chats();

            // running the cybersecurity responses
            conversation chats_response = new conversation();
            chats_response.ResponseSystem(collected_name);

            // prompt and search class to obtain info on the cybersecurity topics
            prompt_and_search search = new prompt_and_search();
            search.aibot(collected_name);

        }//end of method
    }//end of class
}//end of namespace