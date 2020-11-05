using Akka.Actor;
using PasswordBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    public static class PasswordSystem 
    {
        private static readonly ActorSystem _system;
        private static IActorRef _splitter;

        static PasswordSystem()
        {
            System.Diagnostics.Debug.WriteLine("Pokretanje sustava...");
            _system = ActorSystem.Create("password-build-system");
            System.Diagnostics.Debug.WriteLine("Pokretanje SplitterActora...");
            _splitter = _system.ActorOf(Props.Create<SplitterActor>(), "splitter");
        }

        public static void InitializeSplitter()
        {          
            _splitter.Tell(new Initialize());
        }

        public static void StartSplitting(string sentence, PassMainViewModel pvm)
        {
            var props = Props.Create(() => new DisplayActor(pvm));
            var dispatcher = _system.ActorOf(props);

            _system.EventStream.Subscribe(dispatcher, typeof(GeneratedPassword));
            _splitter.Tell(new Sentence(sentence));         
        }
    }
}
