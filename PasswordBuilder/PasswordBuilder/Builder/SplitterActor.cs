using Akka.Actor;
using Akka.Routing;
using Akka.Util.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    public class SplitterActor : ReceiveActor
    {
        private IActorRef _pickers;
        private IActorRef _trimmer;
        private List<string> _passwordChunks;

        public SplitterActor()
        {
            Receive<Initialize>(/* British devs be like */ innit => 
                Become(() => InitializeHandler(innit)));
            _passwordChunks = new List<string>();                      
        }

        private void InitializeHandler(Initialize init)
        {
            Receive<Sentence>(s => OnReceiveSentence(s));
            Receive<TrimmedSentence>(t => {
                OnReceiveTrimmedSentence(t);
                _passwordChunks.Clear();
                _trimmer.Tell(PoisonPill.Instance);
            });

            Receive<PasswordChunk>(p => _passwordChunks.Add(p.Pick));
            Receive<Terminated>(term => {
                var join = _passwordChunks.ToArray().Join("");
                Context.System.EventStream.Publish(new GeneratedPassword(join));
            });
        }

        private void OnReceiveSentence(Sentence s)
        {
            _trimmer = Context.ActorOf(Props.Create(() => new TrimmerActor()), "trimmer");
            System.Diagnostics.Debug.WriteLine(_trimmer.Path);
            _trimmer.Tell(s);
            Context.Watch(_trimmer);
        }

        private void OnReceiveTrimmedSentence(TrimmedSentence t)
        {
            _pickers = Context.ActorOf(Props.Create<PickerActor>()
                .WithRouter(new RoundRobinPool(t.WordCount)), "picker");

            var splitTrimmed = t.Content.Split(' ');
            splitTrimmed.ForEach(word => _pickers.Tell(new Word(word, word.Length)));
            _pickers.Tell(new Broadcast(PoisonPill.Instance));

            Context.Watch(_pickers);
        }
    }
}
