using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Travel_Assistant.Services;
using Travel_Assistant.Models;
using Xamarin.Forms;
using Firebase.Auth;
using Firebase.Storage;
using System.Collections.ObjectModel;
using Firebase.Database;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Firebase.Firestore;
using Java.Util;
using System.Globalization;
using Android.Gms.Extensions;

[assembly: Dependency(typeof(Travel_Assistant.Droid.FirebaseUtils))]
namespace Travel_Assistant.Droid
{
    public class FirebaseUtils : IFirebase
    {
        public FirebaseUtils()
        {
            //Initialize();
        }
        public void AddBadgeDocument(Badge badge)
        {
            using (var storage = FirebaseStorage.Instance.GetReference("PozeLegitimatii/" + GetUserEmail() + "/fata"))
            {
                storage.PutBytes(badge.PozaFata);
            }

            using (var storage = FirebaseStorage.Instance.GetReference("PozeLegitimatii/" + GetUserEmail() + "/spate"))
            {
                storage.PutBytes(badge.PozaSpate);
            }

            HashMap map = new HashMap();
            map.Put("numar", badge.Numar);
            map.Put("universitate", badge.Universitate);

            var database = GetCloud();
            var docRef = database.Collection("legitimatii").Document(GetUserEmail());
            docRef.Set(map);
        }

        public void AddUserDocument(User user)
        {
            HashMap map = new HashMap();
            map.Put("nume", user.Nume);
            map.Put("prenume", user.Prenume);
            map.Put("telefon", user.Telefon);
            map.Put("cnp", user.CNP); 
            map.Put("creat_la", user.Creat.ToString());

            var database = GetCloud();

            DocumentReference docRef = database.Collection("users").Document(user.Email);
            docRef.Set(map);

        }

        public string GetUserEmail()
        {
            return FirebaseAuth.Instance.CurrentUser.Email;
        }

        private FirebaseFirestore GetCloud()
        {
            FirebaseFirestore database;

            database = FirebaseFirestore.Instance;

            return database;
        }

        public FirebaseDatabase GetDatabase()
        {
            FirebaseDatabase database;
            //FirebaseApp app = Initialize();
            database = FirebaseDatabase.Instance;

            return database;
        }

        private FirebaseApp Initialize()
        {
            FirebaseApp app = FirebaseApp.GetInstance(FirebaseApp.DefaultAppName);
            
            if(app == null)
            {
                FirebaseOptions options = new FirebaseOptions.Builder()
                  .SetProjectId("travel - assistant - d3dc7")
                  .SetApplicationId("travel - assistant - d3dc7")
                  .SetApiKey("AIzaSyAlndwzvJJjPE3_M1y0nFT1PrJqFUz-XzI")
                  .SetDatabaseUrl("https://travel-assistant-d3dc7-default-rtdb.europe-west1.firebasedatabase.app")
                  .SetStorageBucket("travel-assistant-d3dc7.appspot.com")
                  .Build();

                app = FirebaseApp.InitializeApp(Android.App.Application.Context, options);
            }
           

            return app;
        }

        public async void AddTicket(Ticket ticket)
        {
            
            HashMap map = new HashMap();
            map.Put("User Email", ticket.UserEmail);
            map.Put("Train ID", ticket.TrainID);
            map.Put("Departure Station", ticket.DepartureStation);
            map.Put("Arrival Station", ticket.ArrivalStation);
            map.Put("Departure Date", ticket.DepartureDate.ToString("g", CultureInfo.CreateSpecificCulture("de-DE")));
            map.Put("Arrival Date", ticket.ArrivalDate.ToString("g", CultureInfo.CreateSpecificCulture("de-DE")));
            map.Put("Train Class", ticket.TrainClass);
            map.Put("Discount", ticket.Discount);
            map.Put("Price", ticket.Price);
            map.Put("Distance", ticket.Distance);

            var database = GetCloud();
            var docRef = database.Collection("tickets").Document();
                
            await docRef.Set(map)
                    .AddOnSuccessListener(new OnSuccessListner())
                    .AddOnFailureListener(new OnFailureListener());
        }

        public async Task<Dictionary<string, Ticket>> GetUserTickets()
        {
            var database = GetCloud();
            Firebase.Firestore.Query query = database.Collection("tickets").WhereEqualTo("User Email", GetUserEmail());

            OnSuccessTicketsListner successListener = new OnSuccessTicketsListner();
            await query.Get().AddOnSuccessListener(successListener);

            return successListener.Tickets;
        }

        public async void RemoveTicket(string TicketID)
        {
            var database = GetCloud();
            var query = database.Collection("tickets").Document(TicketID);
            await query.Delete().AddOnSuccessListener(new OnSuccessRemoveTicketListener());
        }

        public class OnSuccessTicketsListner : Java.Lang.Object, IOnSuccessListener
        {
            public OnSuccessTicketsListner()
            {
                Tickets = new Dictionary<string, Ticket>();
            }
            public void OnSuccess(Java.Lang.Object result)
            {
                var snapshot = (QuerySnapshot)result;
                if (!snapshot.IsEmpty)
                {
                    foreach (DocumentSnapshot doc in snapshot.Documents)
                    {
                        Ticket ticket = new Ticket
                        {
                            ArrivalStation = doc.GetString("Arrival Station").ToString(),
                            UserEmail = doc.GetString("User Email").ToString(),
                            DepartureStation = doc.GetString("Departure Station"),
                            Discount = (bool)doc.GetBoolean("Discount"),
                            Price = (double)doc.GetDouble("Price"),
                            TrainClass = doc.GetString("Train Class"),
                            TrainID = doc.GetString("Train ID")
                        };

                        DateTime date;
                        DateTime.TryParse(doc.GetString("Arrival Date"), out date);
                        ticket.ArrivalDate = date;

                        DateTime.TryParse(doc.GetString("Departure Date"), out date);
                        ticket.DepartureDate = date;

                        Tickets.Add(doc.Id, ticket);
                    }
                }
            }

            public Dictionary<string, Ticket> Tickets { get; }
        }

        public class OnSuccessListner : Java.Lang.Object, IOnSuccessListener
        {
            public void OnSuccess(Java.Lang.Object result)
            {
                Display();
            }

            private async void Display()
            {
                await App.Current.MainPage.DisplayAlert("Success", "Ticket Bought", "Ok");
            }
        }

        public class OnFailureListener : Java.Lang.Object, IOnFailureListener
        {
            public void OnFailure(Java.Lang.Object result)
            {
                Display();
            }

            public void OnFailure(Java.Lang.Exception e)
            {
                Display();
            }

            private async void Display()
            {
                await App.Current.MainPage.DisplayAlert("Failure", "Failed! Please retry!", "Ok");
            }
        }

        public class OnSuccessRemoveTicketListener : Java.Lang.Object, IOnSuccessListener
        {
            public void OnSuccess(Java.Lang.Object result)
            {
                Display();
            }
            private async void Display()
            {
                await App.Current.MainPage.DisplayAlert("", "Bilet anulat cu succes!", "Ok");
            }
        }
    }
}