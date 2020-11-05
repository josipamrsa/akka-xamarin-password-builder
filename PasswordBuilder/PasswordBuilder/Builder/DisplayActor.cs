using Akka.Actor;
using PasswordBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PasswordBuilder.Builder
{
    //----ACTOR TYPE----//
    public class DisplayActor : ReceiveActor
    {
        // Actor that transmits the generated passwords to the ViewModel
        public DisplayActor(PassMainViewModel pvm) =>
            Receive<GeneratedPassword>(pass => 
                Device.BeginInvokeOnMainThread(() => pvm.Password = pass.Content));
    }
}
