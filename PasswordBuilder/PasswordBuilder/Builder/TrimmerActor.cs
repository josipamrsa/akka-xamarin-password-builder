using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    public class TrimmerActor : ReceiveActor
    {
        public TrimmerActor()
        {
            Receive<Sentence>((s) => CleanUpSentence(s.Content));
        }

        private void CleanUpSentence(string content)
        {
            char[] symbolsToTrim = new char[] { '.', ',', ';', '?', '!' };
            string cleanSentence = content.Trim(symbolsToTrim);
            TrimmedSentence trimmed = new TrimmedSentence(cleanSentence, cleanSentence.Split(' ').Length);

            Sender.Tell(trimmed);           
        }
    }
}
