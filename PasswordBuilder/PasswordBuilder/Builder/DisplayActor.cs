using Akka.Actor;
using PasswordBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PasswordBuilder.Builder
{
    public class DisplayActor : ReceiveActor
    {
        public DisplayActor(PassMainViewModel pvm) =>
            Receive<GeneratedPassword>(pass => 
                Device.BeginInvokeOnMainThread(() => pvm.Password = pass.Content));
    }
}
