using System;
using System.Collections.Generic;
using System.ComponentModel;
using Travel_Assistant.Models;
using Travel_Assistant.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travel_Assistant.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}