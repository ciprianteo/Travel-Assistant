using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            if(!IsValidEmail())
            {
                await App.Current.MainPage.DisplayAlert("", "Adresa de email nu este corespunzatoare!", "Ok");
                return;
            }
            if(password == string.Empty || password == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Password nu poate fi gol!", "Ok");
                return;
            }

            string user = null;

            await Task.Run(() => { user = ((App)Application.Current).Auth.LogIn(email, password).Result; });

            if (user == "%INVALID%USER%")
            {
                await App.Current.MainPage.DisplayAlert("", "Contul asociat acestui email nu exista!", "Ok");
            }
            else if (user == "%INVALID%PASSWORD%")
            {
                await App.Current.MainPage.DisplayAlert("", "Parola introdusa este incorecta!", "Ok");
            }else
            {
                ((App)Application.Current).IsBadgeValid();
                await Shell.Current.GoToAsync("//Main");
            }
        }

        private async void OnRegisterLabelClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        private bool IsValidEmail()
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
