using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Travel_Assistant.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travel_Assistant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainsPage : ContentPage
    { 
        public TrainsPage()
        {
            InitializeComponent();

            this.BindingContext = new TrainViewModel();
        }
    }
}
