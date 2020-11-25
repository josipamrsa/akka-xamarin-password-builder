using Akka;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    //----ACTOR TYPE----//
    public class PickerActor : ReceiveActor
    {
        // Actor that generates random password chunk from received word
        public PickerActor()
        {
            // On a newly received word, process it
            Receive<Word>((word) => OnReceiveWord(word));
        }

        //----HANDLERS----//

        // Word processing
        private void OnReceiveWord(Word word)
        {
            string chunk = "";
            Random rnd = new Random();

            // If a vowel is detected, add a random number to the chunk
            // Number is generated in a [0, 9] range
            // Chunk consists of the first letter in received word and 
            // randomly generated number
            bool isVowel = "aeiouAEIOU".IndexOf(word.Content[0]) >= 0;
            if (isVowel) chunk = word.Content[0] + rnd.Next(0, 10).ToString();
            
            // Otherwise just return the first letter of the word
            else chunk = word.Content[0].ToString();

            // Send the generated chunk to coordinator
            System.Diagnostics.Debug.WriteLine(Self.Path + " >> Received: " + chunk + ", id: " + word.Id);            
            Sender.Tell(new PasswordChunk(chunk, word.Id));
        }
    }
}
