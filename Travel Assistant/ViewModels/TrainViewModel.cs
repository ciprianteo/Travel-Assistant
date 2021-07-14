using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    [QueryProperty(nameof(Arrive), "Arrive")]
    [QueryProperty(nameof(Depart), "Depart")]
    [QueryProperty(nameof(DepartDate), "DepartDate")]
    public class TrainViewModel
    {
        string _statiePlecare;
        string _statieSosire;
        DateTime _departureDate;
        public string Arrive
        {
            get => _statieSosire;
            set { _statieSosire = Uri.UnescapeDataString(value ?? string.Empty); }
        }
        public string Depart
        {
            get => _statiePlecare;
            set { _statiePlecare = Uri.UnescapeDataString(value ?? string.Empty); }
        }
        public string DepartDate
        {
            get => _departureDate.ToString();
            set { _departureDate = DateTime.Parse(Uri.UnescapeDataString(value ?? string.Empty)); }
        }
        public Command<TrainDisplayModel> ItemTapped { get; }
        public ObservableCollection<TrainDisplayModel> Trains { get; set; }

        public TrainViewModel()
        {
            Trains = new ObservableCollection<TrainDisplayModel>();
            ItemTapped = new Command<TrainDisplayModel>(OnItemSelected);
            SearchTrains();
        }

        private async void SearchTrains()
        {
            var trains = await RDatabaseConsumer.GetAllTrains();
            foreach( KeyValuePair<string,Train> t in trains)
            {
                var departureStation = t.Value.Statii[_statiePlecare];
                var arrivalStation = t.Value.Statii[_statieSosire];

                if(departureStation.Km < arrivalStation.Km)
                {
                    Trains.Add(new TrainDisplayModel
                    {
                        ID = t.Key,
                        DepartureCityDepartureTime = t.Value.Statii[_statiePlecare].Plecare,
                        ArrivalCityArrivalTime = t.Value.Statii[_statieSosire].Sosire
                    });
                }
            }
        }

        private async void OnItemSelected(TrainDisplayModel item)
        {
            if (item == null)
                return;

            Item itm = new Item { Id = "id", Text = "txt" };
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={itm.Id}");
        }
    }

    public class TrainDisplayModel
    {
        public string ID { get; set; }
        public string DepartureCityDepartureTime { get; set; }
        public string ArrivalCityArrivalTime { get; set; }
    }
}
