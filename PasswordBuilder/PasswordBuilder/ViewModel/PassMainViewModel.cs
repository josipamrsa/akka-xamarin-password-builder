using Akka.Actor;
using PasswordBuilder.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordBuilder.ViewModel
{
    //----MAIN VIEWMODEL----//
    public class PassMainViewModel : INotifyPropertyChanged
    {
        // OnPropertyChanged event to fetch and return data
        public event PropertyChangedEventHandler PropertyChanged;

        //----PROPERTIES----//

        // User generated sentence
        private string _sentence;
        public string Sentence
        {
            get => _sentence;
            set
            {
                _sentence = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sentence)));
            }
        }

        // System generated password
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        //----COMMAND PROPERTIES----//

        public ICommand InitializeSystemCommand { get; private set; }
        public ICommand CopyToClipboardCommand { get; private set; }
        public ICommand StartPasswordGeneratorCommand { get; private set; }

        public PassMainViewModel()
        {
            // Command handlers

            // Prepare the system and the SplitterActor
            InitializeSystemCommand = new Command(() => 
                // ThreadPool is a class used to work with threads
                // QueueUserWorkItem - queue a method for execution, method executes
                // once a thread pool becomes available
                ThreadPool.QueueUserWorkItem(obj => PasswordSystem.InitializeSplitter()));
            
            // Copy the newly generated password to Clipboard
            CopyToClipboardCommand = new Command<string>(async (pass) => 
                await CopyToClipboard(pass));
        
            // Start the password generator process
            StartPasswordGeneratorCommand = new Command(() =>
                ThreadPool.QueueUserWorkItem(obj =>
                    PasswordSystem.StartSplitting(_sentence, this))
            );
        }   

        //----METHODS----//

        // Copy generated item to Clipboard
        private async Task CopyToClipboard(string pass)
        {
            // Clipboard is a feature that is a part of Xamarin.Essentials lib

            // Set the password in Clipboard
            await Clipboard.SetTextAsync(pass);
            if (Clipboard.HasText)
            {
                // If it has text, notify user
                var copied = await Clipboard.GetTextAsync();
                MessagingCenter.Send(this, "textCopiedToClipboard", copied);
            }
               
        }
    }
}
