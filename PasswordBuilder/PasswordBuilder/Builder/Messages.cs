using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    //----MESSAGES----//
    public class Initialize
    {
        // Initializes SplitterActor
    }
 
    public class Sentence
    {
        // Receives sentence from user input
        public string Content { get; }
        public Sentence(string c) => Content = c;   
    }

    public class TrimmedSentence
    {
        // Trims sentence from unnecessary symbols 
        public string Content { get; }
        public int WordCount { get; }
        public TrimmedSentence(string c, int wc)
        {
            Content = c;
            WordCount = wc;
        }
    }
    public class Word {
        // Receives words from split sentence
        public string Content { get; }
        public int LetterCount { get; }
        public Word(string c, int lc)
        {
            Content = c;
            LetterCount = lc;
        }
    }
    public class PasswordChunk {
        // Receives generated password chunks
        public string Pick { get; }
        public PasswordChunk(string p) => Pick = p;
    
    }

    public class GeneratedPassword
    {
        public string Content { get; }
        public GeneratedPassword(string c) => Content = c;
    }
}
