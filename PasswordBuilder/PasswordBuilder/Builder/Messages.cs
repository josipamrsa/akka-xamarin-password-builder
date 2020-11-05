using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    public class Initialize
    {

    }

    public class Sentence
    {
        public string Content { get; }
        public Sentence(string c)
        {
            Content = c;            
        }
    }

    public class TrimmedSentence
    {
        public string Content { get; }
        public int WordCount { get; }
        public TrimmedSentence(string c, int wc)
        {
            Content = c;
            WordCount = wc;
        }
    }
    public class Word {
        public string Content { get; }
        public int LetterCount { get; }
        public Word(string c, int lc)
        {
            Content = c;
            LetterCount = lc;
        }
    }
    public class PasswordChunk {
        public string Pick { get; }
        public PasswordChunk(string p) => Pick = p;
    
    }

    public class GeneratedPassword
    {
        public string Content { get; }
        public GeneratedPassword(string c) => Content = c;
    }
}
