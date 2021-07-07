using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Travel_Assistant.Models;

namespace Travel_Assistant.Droid
{
    public class TrainListeners : Java.Lang.Object, IValueEventListener
    {
        List<Train> trainsList = new List<Train>();
        public List<Train> TrainList { get { return trainsList; } }
        public bool IsListAvailable { get; set; }

        //public event EventHandler<TrainDataEventArgs> TrainRetrived;
        //public class TrainDataEventArgs : EventArgs
        //{
        //    public List<Train> Trains { get; set; }
        //}
        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                trainsList.Clear();
                foreach (DataSnapshot trainData in child)
                {
                    Train train = new Train();
                    //train.ID = trainData.Key;
                    var statii = trainData.Child("statii");

                    trainsList.Add(train);
                }

                IsListAvailable = true;
            }
        }

        public void Create()
        {
            IsListAvailable = false;
            DatabaseReference trainRef = FirebaseUtils.GetDatabase().GetReference("trenuri");
            trainRef.AddValueEventListener(this);
        }
    }
}