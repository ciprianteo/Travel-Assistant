using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    [QueryProperty(nameof(Email), nameof(Email))]
    [QueryProperty(nameof(DataInreg), nameof(DataInreg))]
    [QueryProperty(nameof(Legitimatie), nameof(Legitimatie))]
    [QueryProperty(nameof(Nume), nameof(Nume))]
    [QueryProperty(nameof(Prenume), nameof(Prenume))]
    [QueryProperty(nameof(Telefon), nameof(Telefon))]
    [QueryProperty(nameof(CNP), nameof(CNP))]
    public class AccountInfoViewModel : INotifyPropertyChanged
    {
        private string _email;
        public string Email 
        {   get
            {
                return _email;
            }
            set 
            {
                _email = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(Email));
            } 
        }
        private string _dataInreg;
        public string DataInreg 
        { 
            get
            {
                return _dataInreg;
            }
            set
            {
                _dataInreg = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(DataInreg));
            }
        }
        private bool _legitimatie;
        public string Legitimatie 
        { 
            get
            {
                if (_legitimatie)
                    return "Valid";
                else
                    return "Invalid";
            }
            set
            {
                _legitimatie = bool.Parse(Uri.UnescapeDataString(value ?? "false"));
                OnPropertyChanged(nameof(Legitimatie));
            }
        }
        private string _nume;
        public string Nume 
        { 
            get
            {
                return _nume;
            }
            set
            {
                _nume = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(Nume));
            }
        }
        private string _prenume;
        public string Prenume 
        { 
            get
            {
                return _prenume;
            }
            set
            {
                _prenume = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(Prenume));
            }
        }
        private string _telefon;
        public string Telefon 
        { 
            get
            {
                return _telefon;
            }
            set
            {
                _telefon = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(Telefon));
            }
        }
        private string _cnp;
        public string CNP 
        { 
            get
            {
                return _cnp;
            }
            set
            {
                _cnp = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(CNP));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
