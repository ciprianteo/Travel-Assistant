using System;
using System.Collections.Generic;
using System.Text;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    class ProfileViewModel
    {
        public Command LogOutCommand { get; set; }
        public ProfileViewModel()
        {
            LogOutCommand = new Command(OnLogOutClicked);
        }

        private async void OnLogOutClicked(object obj)
        {
            if(((App)Application.Current).Auth.IsSignedIn())
            {
                ((App)Application.Current).Auth.SignOut();
            }

            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
