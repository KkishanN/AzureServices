using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBrokeredMessaging.Sender
{
    internal class Bot
    {
        public static Bot Instance { get; } = new Bot();
        string lastName = "Kumar";
        public SaveTranscript saveTranscript;
        public void Display1(string name)
        {
            saveTranscript = new SaveTranscript();
            saveTranscript.DisplayFromSaveTranscript($"{name} - {lastName}");
        }

        public void Display2(string name)
        {
            saveTranscript.DisplayFromSaveTranscript($"{name} - {lastName}");
        }
    }
}
