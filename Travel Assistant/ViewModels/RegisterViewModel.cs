using System;
using System.Collections.Generic;
using System.Text;
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
            try
            {
                User usr = new User { Nume = nume, Prenume = prenume, CNP = cnp, Email = email, Telefon = telefon, Creat = DateTime.Now };
                var user = ((App)Application.Current).Auth.Register(email, password);

                if (user != null)
                {
                    ((App)Application.Current).FirebaseUtils.AddUserDocument(usr);

                    await App.Current.MainPage.DisplayAlert("Success", "New User Created", "Ok");

                    if (((App)Application.Current).Auth.SignOut())
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                }
            }
            catch(Exception)
            {

            }
            
        }
    }
}
