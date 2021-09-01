using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    
    class RegisterViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string passwordConf { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string telefon { get; set; }
        public string cnp { get; set; }

        public Command RegisterCommand { get; }
        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            bool valid = await CheckFileds();
            if (!valid)
                return;

            try
            {
                User usr = new User { Nume = nume, Prenume = prenume, CNP = cnp, Email = email, Telefon = telefon, Creat = DateTime.Now };
                var user = await ((App)Application.Current).Auth.Register(email, password);

                if (user != null && user != string.Empty)
                {
                    ((App)Application.Current).FirebaseUtils.AddUserDocument(usr);

                    await App.Current.MainPage.DisplayAlert("Success", "Contul de utilizator a fost creat!", "Ok");

                    if (((App)Application.Current).Auth.SignOut())
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                }
            }
            catch(Exception)
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Contul nu a putut fi creat!", "Ok");
            }
            
        }

        private async Task<bool> CheckFileds()
        {
            if(nume == string.Empty || nume == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Nume nu poate fi gol!", "Ok");
                return false;
            }
            if (prenume == string.Empty || prenume == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Prenume nu poate fi gol!", "Ok");
                return false;
            }
            if (cnp == string.Empty || cnp == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul CNP nu poate fi gol!", "Ok");
                return false;
            }
            else if (cnp.Length != 13)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul CNP trebuie sa aiba exact 13 caractere!", "Ok");
                return false;

            }
            else if(!OnlyDigits(cnp))
            {
                await App.Current.MainPage.DisplayAlert("", "Campul CNP trebuie sa contina doar cifre!", "Ok");
                return false;

            }else if(cnp[0] == '0' || cnp[1] == '9')
            {
                await App.Current.MainPage.DisplayAlert("", "Un CNP valid trebuie sa aiba prima cifra in intervalul [1, 8]!", "Ok");
                return false;
            }

            if (email == string.Empty || email == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Email nu poate fi gol!", "Ok");
                return false;
            }
            if (telefon == string.Empty || telefon == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Telefon nu poate fi gol!", "Ok");
                return false;
            }
            if (password == string.Empty || password == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Parola nu poate fi gol!", "Ok");
                return false;
            }
            if (passwordConf == string.Empty || passwordConf == null)
            {
                await App.Current.MainPage.DisplayAlert("", "Campul Confirmare Parola nu poate fi gol!", "Ok");
                return false;
            }
            if(passwordConf != password)
            {
                await App.Current.MainPage.DisplayAlert("", "Parolele introduse nu se potrivesc!", "Ok");
                return false;
            }

            return true;
        }

        private bool OnlyDigits(string str)
        {
            foreach (char c in str)
                if (c < '0' || c > '9')
                    return false;

            return true;
        }

    }
}
