using PasswordBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PasswordBuilder
{
    public partial class MainPage : ContentPage
    {
        public PassMainViewModel ViewModel
        {
            get { return BindingContext as PassMainViewModel; }
            set { BindingContext = value; }
        }
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new PassMainViewModel();

            MessagingCenter.Subscribe<PassMainViewModel, string>(this, 
                "textCopiedToClipboard", 
                (sender, info) => {
                    DisplayAlert("Info", "Copied to clipboard", "OK");
                });
        }

        protected override void OnAppearing()
        {
            ViewModel.InitializeSystemCommand.Execute(null);
        }
    }
}
