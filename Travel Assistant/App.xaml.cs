using System;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travel_Assistant
{
    public partial class App : Application
    {
        public IAuth Auth { get; }
        public IFirebase FirebaseUtils { get; }
        private bool _validBadge;
        public bool ValidBadge { get { return _validBadge; } }
        public App()
        {
            InitializeComponent();
            
            FirebaseUtils = DependencyService.Get<IFirebase>();
            Auth = DependencyService.Get<IAuth>();
            _validBadge = false;

            MainPage = new AppShell();

        }

        protected override void OnStart()
        {
            CheckLogin();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async void CheckLogin()
        {
            if (Auth.IsSignedIn())
            {
                IsBadgeValid();
                await Shell.Current.GoToAsync("//Main");
            }
            else
            {
                //only open Login page when no valid login
                await Shell.Current.GoToAsync("//LoginPage");
            }

        }

        public async void IsBadgeValid()
        {
            Badge badge = null;
            UniversityBadge univBadge = null;
            await Task.Run(() =>
            {
                badge = FirebaseUtils.GetUserBadge().Result;
            });

            await Task.Run(() =>
            {
                univBadge = RDatabaseConsumer.GetBadge(badge.Numar, badge.Universitate).Result;
            });

            if (badge.Numar != null && badge.Universitate != null)
            {
                var viza = univBadge.Viza;
                var currYear = DateTime.Now.Year;
                var currMonth = DateTime.Now.Month;
                int[] tokens = new int[2];
                int idx = 0;
                
                foreach (var token in viza.Split('-'))
                {
                    tokens[idx++] = int.Parse(token);
                }

                if (currMonth < 10)
                {
                    if(tokens[1] >= currYear && tokens[0] >= currYear - 1)
                        _validBadge = true;
                    else
                        _validBadge = false;

                }
                else
                {
                    if (tokens[1] == currYear && tokens[0] == currYear - 1)
                        _validBadge = true;
                    else
                        _validBadge = false;
                }
            }
        }
    }
}
