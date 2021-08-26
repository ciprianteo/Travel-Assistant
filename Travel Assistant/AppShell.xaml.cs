using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel_Assistant.ViewModels;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(BadgePage), typeof(BadgePage));
            Routing.RegisterRoute(nameof(TrainsPage), typeof(TrainsPage));
            Routing.RegisterRoute(nameof(BuyTicketPage), typeof(BuyTicketPage));
            Routing.RegisterRoute(nameof(AccountInfoPage), typeof(AccountInfoPage));
        }

    }
}
