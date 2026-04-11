using System;
using System.Media;

namespace dynabot_cybersecurity
{//Start of namespace
    public class voice_greeting
    {//Start of class
        public voice_greeting()
        {//start of constructor
            //call the greet method
            greet();
        }//end of constructor

        //method to play the voice
        private void greet()
        {//start of void method
            //auto get the path of the voice recording
            string paths = AppDomain.CurrentDomain.BaseDirectory;
            //build the full path to the wav file
            string fullpath = paths.Replace(@"bin\Debug\", "greeting.wav");
            //load the audio then play it
            SoundPlayer voice_play = new SoundPlayer(fullpath);
            //load the audio
            voice_play.Load();
            //then play it
            voice_play.Play();
        }//end of void method
    }//end of class
}//End of namespace