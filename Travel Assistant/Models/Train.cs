using System;
using System.Collections.Generic;
using System.Text;

namespace Travel_Assistant.Models
{
    public class Train
    {
        //public string ID { get; set; }
        public Dictionary<string, Statie> Statii { get; set; }

        public Train()
        {
            Statii = new Dictionary<string, Statie>();
            //ID = string.Empty;
        }

    }

    public class Statie
    {
        public string Sosire { get; set; }
        public string Plecare { get; set; }
        public int Km { get; set; }
    }

    public class Price
    {
        public float Adult { get; set; }
        public float Student { get; set; }
    }

    public class Ticket
    {
        public string UserEmail { get; set; }
        public string TrainID { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public double Price { get; set; }
        public string TrainClass { get; set; }
        public bool Discount { get; set; }
        public int Distance { get; set; }

    }
}
