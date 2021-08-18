using System;
using System.Collections.Generic;
using System.Text;

namespace Travel_Assistant.Models
{
    public class Badge
    {
        public string Email { get; set; }
        public string Numar { get; set; }
        public string Universitate { get; set; }
        public byte[] PozaFata { get; set; }
        public byte[] PozaSpate { get; set; }

    }

    public class UniversityBadge
    {
        public string CNP { get; set; }
        public string Initiala { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Viza { get; set; }
    }
}
