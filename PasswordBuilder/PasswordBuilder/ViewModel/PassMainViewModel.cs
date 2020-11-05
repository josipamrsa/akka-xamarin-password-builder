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
    public class PassMainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand InitializeSystemCommand { get; private set; }
        public ICommand CopyToClipboardCommand { get; private set; }
        public ICommand StartPasswordGeneratorCommand { get; private set; }

        public PassMainViewModel()
        {
            InitializeSystemCommand = new Command(() => 
                ThreadPool.QueueUserWorkItem(obj => PasswordSystem.InitializeSplitter()));
            
            CopyToClipboardCommand = new Command<string>(async (pass) => 
                await CopyToClipboard(pass));
        
            StartPasswordGeneratorCommand = new Command(() =>
                ThreadPool.QueueUserWorkItem(obj =>
                    PasswordSystem.StartSplitting(_sentence, this))
            );
        }   

        private async Task CopyToClipboard(string pass)
        {
            await Clipboard.SetTextAsync(pass);
            if (Clipboard.HasText)
            {
                var copied = await Clipboard.GetTextAsync();
                MessagingCenter.Send(this, "textCopiedToClipboard", copied);
            }
               
        }
    }
}
