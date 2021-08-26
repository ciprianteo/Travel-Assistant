using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Services;
using Firebase.Auth;
using Xamarin.Forms;
using Firebase;

[assembly : Dependency(typeof(Travel_Assistant.Droid.DroidAuth))]
namespace Travel_Assistant.Droid
{
    public class DroidAuth : IAuth
    {
        private FirebaseAuth _auth;
        public DroidAuth()
        {
            _auth = FirebaseAuth.Instance;
        }
        public bool IsSignedIn()
        {
            var user = _auth.CurrentUser;
            return user != null;
        }

        public async Task<string> LogIn(string email, string password)
        {
            try
            {
                var user = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                var token = user.User.GetIdToken(false);

                return token.ToString();
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return "%INVALID%USER%";
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return "%INVALID%PASSWORD%";
            }
        }

        public async Task<string> Register(string email, string password)
        {
            try
            {
                var newUser = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                var token =  newUser.User.GetIdToken(false);

                return token.ToString();
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                await App.Current.MainPage.DisplayAlert("Failed", e.Message, "Ok");
                return string.Empty;
            }
            catch(FirebaseAuthInvalidCredentialsException e)
            {
                await App.Current.MainPage.DisplayAlert("Failed", e.Message, "Ok");
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                _auth.SignOut();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}