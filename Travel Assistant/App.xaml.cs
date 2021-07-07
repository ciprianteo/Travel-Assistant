using System;
using System.Threading.Tasks;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travel_Assistant
{
    public partial class App : Application
    {
        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public IAuth Auth { get; }
        public IFirebase FirebaseUtils { get; }
        public App()
        {
            InitializeComponent();

            Auth = DependencyService.Get<IAuth>();
            FirebaseUtils = DependencyService.Get<IFirebase>();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

        }

        protected override void OnStart()
        {
            _ = CheckLogin();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task CheckLogin()
        {
            if (Auth.IsSignedIn())
            {
                await Shell.Current.GoToAsync("//Main");
            }
            else
            {
                //only open Login page when no valid login
                await Shell.Current.GoToAsync("//LoginPage");
            }

        }
    }
}
