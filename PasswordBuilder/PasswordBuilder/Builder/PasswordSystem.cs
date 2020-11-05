using Akka.Actor;
using PasswordBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBuilder.Builder
{
    //----ACTOR SYSTEM----//
    public static class PasswordSystem 
    {
        // System that manages password generation

        private static readonly ActorSystem _system;
        private static IActorRef _splitter;

        static PasswordSystem()
        {
            // Initializers for the system and the coordinator (SplitterActor)
            System.Diagnostics.Debug.WriteLine("Pokretanje sustava...");
            _system = ActorSystem.Create("password-build-system");
            System.Diagnostics.Debug.WriteLine("Pokretanje SplitterActora...");
            _splitter = _system.ActorOf(Props.Create<SplitterActor>(), "splitter");
        }

        //----METHODS----//
        public static void InitializeSplitter()
        {          
            // Start SplitterActor 
            _splitter.Tell(new Initialize());
        }

        public static void StartSplitting(string sentence, PassMainViewModel pvm)
        {
            // Start the dispatcher to main UI (DisplayActor)
            var props = Props.Create(() => new DisplayActor(pvm));
            var dispatcher = _system.ActorOf(props);

            // Subscribe to events that carry a newly generated password
            _system.EventStream.Subscribe(dispatcher, typeof(GeneratedPassword));
            // Tell SplitterActor to begin operations on user sentence
            _splitter.Tell(new Sentence(sentence));         
        }
    }
}
