using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travel_Assistant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            this.BindingContext = new ProfileViewModel();
        }
    }
}