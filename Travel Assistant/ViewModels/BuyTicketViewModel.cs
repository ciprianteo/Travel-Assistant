using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    [QueryProperty(nameof(TrainID), nameof(TrainID))]
    [QueryProperty(nameof(DepartureStation), nameof(DepartureStation))]
    [QueryProperty(nameof(ArrivalStation), nameof(ArrivalStation))]
    [QueryProperty(nameof(DepartureDate), nameof(DepartureDate))]
    [QueryProperty(nameof(ArrivalDate), nameof(ArrivalDate))]
    [QueryProperty(nameof(Distance), nameof(Distance))]
    public class BuyTicketViewModel : INotifyPropertyChanged
    {
        int _distance;
        int _catIdx;
        int _classIdx;
        double _price;
        string _trainID;
        string _departStation;
        string _arrivalStation;
        DateTime _departureDate;
        DateTime _arrivalDate;
        Dictionary<string, Models.Price> _clasesPrice;
        public int ClassIdx 
        {
            get => _classIdx;
            set
            {
                _classIdx = value;
                ChangePrice();
            }
        }
        public int CatIdx
        {
            get => _catIdx;
            set
            {
                _catIdx = value;
                ChangePrice();
            }
        }
        public string Journey { get; set; }
        public string TrainID
        {
            get => _trainID;
            set { _trainID = Uri.UnescapeDataString(value ?? string.Empty); OnPropertyChanged(nameof(TrainID)); }
        }
        public string DepartureStation
        {
            get => _departStation;
            set { _departStation = Uri.UnescapeDataString(value ?? string.Empty); Journey += _departStation; }
        }
        public string ArrivalStation
        {
            get => _arrivalStation;
            set { _arrivalStation = Uri.UnescapeDataString(value ?? string.Empty); Journey += " - " + _arrivalStation;}
        }
        public string DepartureDate
        {
            get => _departureDate.ToString(CultureInfo.CreateSpecificCulture("de-DE"));
            set { _departureDate = DateTime.Parse(Uri.UnescapeDataString(value ?? string.Empty)); OnPropertyChanged(nameof(DepartureDate)); }
        }
        public string ArrivalDate
        {
            get => _arrivalDate.ToString(CultureInfo.CreateSpecificCulture("de-DE"));
            set { _arrivalDate = DateTime.Parse(Uri.UnescapeDataString(value ?? string.Empty)); OnPropertyChanged(nameof(ArrivalDate)); }
        }
        public string Distance
        {
            get => _distance.ToString();
            set {  int.TryParse(value ?? "0", out _distance); Journey += " " + (value ?? "0") + " Km"; OnPropertyChanged(nameof(Journey)); }
        }
        public string Price
        {
            get => (_price.ToString("#.##") + " RON");
            set { double.TryParse(value ?? "0", out _price); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> ClasesList { get; set; }
        public ObservableCollection<string> CategoryList { get; set; }
        public Command BuyTicketCommand { get; set; }
        public BuyTicketViewModel()
        {
            _price = 0;
            CatIdx = -1;
            ClassIdx = -1;
            BuyTicketCommand = new Command(OnBuyTicketClicked);
            ClasesList = new ObservableCollection<string> { "I", "II" };
            CategoryList = new ObservableCollection<string> { "Adult"};

            if (((App)Application.Current).ValidBadge)
                CategoryList.Add("Student");

            SearchPrices();
        }

        private async void SearchPrices()
        {
            _clasesPrice = await RDatabaseConsumer.GetPrices();
        }

        private void ChangePrice()
        {
            if (_clasesPrice != null && _classIdx != -1 && _catIdx != -1)
            {
                if (CategoryList[_catIdx].Equals("Adult"))
                    _price = _distance * _clasesPrice[ClasesList[ClassIdx]].Adult;
                else
                    _price = _distance * _clasesPrice[ClasesList[ClassIdx]].Student;

                _price = Math.Round(_price, 2);

                OnPropertyChanged(nameof(Price));
            }
        }

        private async void OnBuyTicketClicked()
        {
            Ticket ticket = new Ticket
            {
                TrainID = this.TrainID,
                DepartureStation = this.DepartureStation,
                DepartureDate = this._departureDate,
                ArrivalStation = this.ArrivalStation,
                ArrivalDate = this._arrivalDate,
                Price = this._price,
                Discount = CategoryList[_catIdx].Equals("Student"),
                TrainClass = ClasesList[ClassIdx],
                UserEmail = ((App)Application.Current).FirebaseUtils.GetUserEmail(),
                Distance = this._distance
            };

            ((App)Application.Current).FirebaseUtils.AddTicket(ticket);
            await Shell.Current.GoToAsync($"//Main/{nameof(SearchTrainPage)}");
        }
    }
}
