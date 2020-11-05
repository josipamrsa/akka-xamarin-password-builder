using Akka;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    public class PickerActor : ReceiveActor
    {
        public PickerActor()
        {
            Receive<Word>((word) => OnReceiveWord(word));
        }

        private void OnReceiveWord(Word word)
        {
            string chunk = "";
            Random rnd = new Random();

            bool isVowel = "aeiouAEIOU".IndexOf(word.Content[0]) >= 0;
            if (isVowel) chunk = word.Content[0] + rnd.Next(0, 10).ToString();
            else chunk = word.Content[0].ToString();

            System.Diagnostics.Debug.WriteLine(Self.Path + " >> Received: " + chunk);            
            Sender.Tell(new PasswordChunk(chunk));
        }
    }
}
