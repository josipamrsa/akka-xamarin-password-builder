﻿using Akka.Actor;
using Akka.Routing;
using Akka.Util.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PasswordBuilder.Builder
{
    //----ACTOR TYPE----//
    public class SplitterActor : ReceiveActor
    {
        // Worker actors
        private IActorRef _pickers;     // Chunk generators
        private IActorRef _trimmer;     // Sentence cleaner

        // List that saves received password chunks
        private string[] _passwordChunks;

        // Actor that coordinates worker actors and assigns work
        public SplitterActor()
        {
            // To start the SplitterActor on app start-up,
            // an Initialize message is utilized to define
            // a new actor behaviour

            Receive<Initialize>(/* British devs be like */ innit =>
                Become(() => InitializeHandler(innit)));
        }

        //----HANDLERS----//

        // Actor behaviour handler
        private void InitializeHandler(Initialize init)
        {
            // On new received sentence, process it with TrimmerActor
            Receive<Sentence>(s => OnReceiveSentence(s));

            // On trimmed sentence, pass the object to PickerActor,
            // clean previous content of the list and kill the TrimmerActor
            Receive<TrimmedSentence>(t =>
            {
                OnReceiveTrimmedSentence(t);
                _passwordChunks = new string[t.WordCount];
                _trimmer.Tell(PoisonPill.Instance);
            });

            // On received password chunk, save it in the list
            Receive<PasswordChunk>(p =>
            {
                System.Diagnostics.Debug.WriteLine(p.Id + ">> " + p.Pick);
                _passwordChunks[p.Id] = p.Pick;               
            });

            // After all actors have finished, join the newly created
            // password, and publish it into the system stream
            Receive<Terminated>(term =>
            {
                var join = _passwordChunks.Join("");
                Context.System.EventStream.Publish(new GeneratedPassword(join));
            });
        }

        // TrimmerActor handler
        private void OnReceiveSentence(Sentence s)
        {
            // Initialize TrimmerActor
            _trimmer = Context.ActorOf(Props.Create(() => new TrimmerActor()), "trimmer");
            System.Diagnostics.Debug.WriteLine(_trimmer.Path);

            // Send the sentence object
            _trimmer.Tell(s);

            // Watch the actor
            Context.Watch(_trimmer);
        }

        // PickerActor handler
        // TODO - have the chunks returned in order
        private void OnReceiveTrimmedSentence(TrimmedSentence t)
        {
            // Initialize PickerActors with a RoundRobin router           
            _pickers = Context.ActorOf(Props.Create<PickerActor>()
                .WithRouter(new RoundRobinPool(t.WordCount)), "picker");

            // Split the trimmed sentence and assign each word to one worker
            var splitTrimmed = t.Content.Split(' ');

            splitTrimmed.ForEach(word =>
            {
                // For each word in array, send it to a picker actor to process (with index)
                _pickers.Tell(new Word(word, word.Length, splitTrimmed.ToList().IndexOf(word)));
            });
                
            // After all workers have finished, kill them
            _pickers.Tell(new Broadcast(PoisonPill.Instance));

            // Watch router and workers
            Context.Watch(_pickers);
        }
    }
}
