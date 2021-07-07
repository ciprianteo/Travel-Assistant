using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Travel_Assistant.Services;
using Travel_Assistant.Models;
using Java.Util;
using Xamarin.Forms;
using Firebase.Auth;
using Firebase.Storage;
using System.Collections.ObjectModel;
using Firebase.Database;
using System.Threading.Tasks;

[assembly: Dependency(typeof(Travel_Assistant.Droid.FirebaseUtils))]
namespace Travel_Assistant.Droid
{
    public class FirebaseUtils : IFirebase
    {
        public FirebaseUtils()
        {
            Initialize();
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

        public static FirebaseFirestore GetCloud()
        {
            FirebaseFirestore database;
            FirebaseApp app = FirebaseFirestore.Instance.App;

            database = FirebaseFirestore.GetInstance(app);

            return database;
        }

        public static FirebaseDatabase GetDatabase()
        {
            FirebaseDatabase database;
            var app = FirebaseDatabase.Instance.App;
            database = FirebaseDatabase.GetInstance(app);

            return database;
        }

        private void Initialize()
        {
            FirebaseApp app = FirebaseApp.InitializeApp(Android.App.Application.Context);
            if (app == null)
            {
                FirebaseOptions options = new FirebaseOptions.Builder()
                   .SetProjectId("travel - assistant - d3dc7")
                   .SetApplicationId("travel - assistant - d3dc7")
                   .SetApiKey("AIzaSyAlndwzvJJjPE3_M1y0nFT1PrJqFUz-XzI")
                   .SetDatabaseUrl("https://travel-assistant-d3dc7-default-rtdb.europe-west1.firebasedatabase.app")
                   .SetStorageBucket("travel-assistant-d3dc7.appspot.com")
                   .Build();

                FirebaseApp.InitializeApp(Android.App.Application.Context, options);
            }
        }
    }
}