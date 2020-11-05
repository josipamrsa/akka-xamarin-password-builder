using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    //----ACTOR TYPE----//
    public class TrimmerActor : ReceiveActor
    {
        // Actor that cleans up user input
        public TrimmerActor()
        {
            // On a newly received sentence, clean it up
            Receive<Sentence>((s) => CleanUpSentence(s.Content));
        }

        //----HANDLERS----//

        // Sentence cleanup
        private void CleanUpSentence(string content)
        {
            // Trim by typical sentence symbols
            char[] symbolsToTrim = new char[] { '.', ',', ';', '?', '!' };
            string cleanSentence = content.Trim(symbolsToTrim);

            // Send the newly cleaned sentence back
            TrimmedSentence trimmed = new TrimmedSentence(cleanSentence, cleanSentence.Split(' ').Length);
            Sender.Tell(trimmed);           
        }
    }
}
