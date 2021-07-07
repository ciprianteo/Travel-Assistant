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
}
