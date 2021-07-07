using System;
using System.Collections.Generic;
using System.Text;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    public class LoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public Command LoginCommand { get; }
        public Command ShowRegisterPage { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            ShowRegisterPage = new Command(OnRegisterLabelClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            var user = ((App)Application.Current).Auth.LogIn(email, password);

            if (user != null)
            {
                await Shell.Current.GoToAsync("//Main");
            }
        }

        private async void OnRegisterLabelClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
