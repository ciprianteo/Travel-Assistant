using System;
using System.Collections.Generic;
using System.Text;

namespace Travel_Assistant.Models
{
    public class User
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string CNP { get; set; }
        public DateTime Creat { get; set; }
    }
}
