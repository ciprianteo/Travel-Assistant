using System.ComponentModel;
using Travel_Assistant.ViewModels;
using Xamarin.Forms;

namespace Travel_Assistant.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}