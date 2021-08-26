using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    class ProfileViewModel
    {
        public Command LogOutCommand { get; set; }
        public Command AddBadgeCommand { get; set; }
        public Command GoToInfoPageCommand { get; set; }
        public ProfileViewModel()
        {
            LogOutCommand = new Command(OnLogOutClicked);
            AddBadgeCommand = new Command(OnDisplayBadgeMenuClicked);
            GoToInfoPageCommand = new Command(OnAccountInfoClicked);
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

        private async void OnAccountInfoClicked()
        {
            User userDetails = null;
            await Task.Run(() => { userDetails = ((App)Application.Current).FirebaseUtils.GetUserDetails().Result; });

            string validBadge = ((App)Application.Current).ValidBadge.ToString();

            await Shell.Current.GoToAsync($"{ nameof(AccountInfoPage)}?Email={((App)Application.Current).FirebaseUtils.GetUserEmail()}&DataInreg={ userDetails.Creat.ToString(CultureInfo.CreateSpecificCulture("de-DE")) }" +
                $"&Legitimatie={ validBadge }&Nume={ userDetails.Nume }&Prenume={ userDetails.Prenume }&Telefon={ userDetails.Telefon }&CNP={ userDetails.CNP } ");
        }
    }
}
