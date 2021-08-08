using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Travel_Assistant.ViewModels
{
    public class MyTicketsViewModel : INotifyPropertyChanged
    {
        private bool isBusy;
        private Dictionary<string, Ticket> _tickets;
        public ObservableCollection<TicketDisplayModel> Tickets { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Command ItemTapped { get; }
        public Command GetTicketsCommand { get; }
        public bool IsBusy { get { return isBusy; } }

        TicketDisplayModel selectedTicket;
        public TicketDisplayModel SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                if(selectedTicket != value)
                {
                    selectedTicket = value;
                }
            }
        }


        public MyTicketsViewModel()
        {
            isBusy = false;
            GetTicketsCommand = new Command(GetTickets);
            ItemTapped = new Command(RemoveTicket);
            Tickets = new ObservableCollection<TicketDisplayModel>();
            GetTickets();
        }

        private async void GetTickets()
        {
            isBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            
            await Task.Run( () =>{ _tickets = ((App)Application.Current).FirebaseUtils.GetUserTickets().Result; });
            UpdateDisplayedTickets();

            isBusy = false;
            OnPropertyChanged(nameof(IsBusy));
        }

        private void UpdateDisplayedTickets()
        {
            Tickets.Clear();
            foreach (var ticket in _tickets)
            {
                TicketDisplayModel tdm = new TicketDisplayModel
                {
                    TicketID = ticket.Key,
                    TrainID = ticket.Value.TrainID,
                    DepartureCityDepartureTime = ticket.Value.DepartureStation + " - " + ticket.Value.DepartureDate.ToString(),
                    ArrivalCityArrivalTime = ticket.Value.ArrivalStation + " - " + ticket.Value.ArrivalDate.ToString()
                };

                Tickets.Add(tdm);
            }
        }
        private async void RemoveTicket()
        {
            var action = await App.Current.MainPage.DisplayAlert("", "Anulati biletul?", "Yes", "No");
            if(action)
            {
                ((App)Application.Current).FirebaseUtils.RemoveTicket(selectedTicket.TicketID);
                Tickets.Remove(selectedTicket);
                selectedTicket = null;
            }
        }
    }

    public class TicketDisplayModel
    {
        public string TicketID { get; set; }
        public string TrainID { get; set; }
        public string DepartureCityDepartureTime { get; set; }
        public string ArrivalCityArrivalTime { get; set; }
    }
}
