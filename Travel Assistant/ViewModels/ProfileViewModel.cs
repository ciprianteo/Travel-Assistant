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
        public Command AddBadgeCommand { get; set; }
        public ProfileViewModel()
        {
            LogOutCommand = new Command(OnLogOutClicked);
            AddBadgeCommand = new Command(OnDisplayBadgeMenuClicked);
        }

        private async void OnLogOutClicked()
        {
            if(((App)Application.Current).Auth.IsSignedIn())
            {
                ((App)Application.Current).Auth.SignOut();
            }

            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnDisplayBadgeMenuClicked()
        {
            if (((App)Application.Current).ValidBadge)
            {
                var action = await App.Current.MainPage.DisplayAlert("", "Aveti deja introdusa o legitimatie valida! Continuati?", "Yes", "No");
                if (!action)
                {
                    return;
                }
            }

            await Shell.Current.GoToAsync("BadgePage");
        }

    }
}
