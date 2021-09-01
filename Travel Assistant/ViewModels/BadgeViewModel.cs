using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    public class BadgeViewModel: INotifyPropertyChanged
    {
        public ImageSource FaceImage { get; set; }
        public ImageSource BackImage { get; set; }
        public string NrLegitimatie { get; set; }
        public string Universitate { get; set; }
        public List<string> UniversitiesList { get; set; }
        public Command LoadFacePhotoCommand { get; }
        public Command LoadBackPhotoCommand { get; }
        public Command UploadBadgeCommand { get; }
        public int SelIdx { get; set; }
        public bool IsUploading { get; set; }
        private IImageConverter ImageConverter;
        public BadgeViewModel()
        {
            LoadFacePhotoCommand = new Command(OnLoadFacePhotoClicked);
            LoadBackPhotoCommand = new Command(OnLoadBackPhotoClicked);
            UploadBadgeCommand = new Command(OnUploadBadgeClicked);

            FaceImage = ImageSource.FromFile("plus.png");
            BackImage = ImageSource.FromFile("plus.png");

            ImageConverter = DependencyService.Get<IImageConverter>();

            IsUploading = false;
            SelIdx = -1;
            UniversitiesList = new List<string>();

            UniversitiesList.Add("UNIVERSITATEA POLITEHNICA DIN BUCURESTI");
            UniversitiesList.Add("UNIVERSITATEA DIN BUCURESTI");
            UniversitiesList.Add("ACADEMIA DE STUDII ECONOMICE DIN BUCURESTI");
            UniversitiesList.Add("ACADEMIA TEHNICA MILITARA");
        }

        private async void OnUploadBadgeClicked()
        {
            IsUploading = true;
            Badge badge = new Badge
            {
                Numar = NrLegitimatie,
                Universitate = UniversitiesList[SelIdx],
                Email = ((App)Application.Current).FirebaseUtils.GetUserEmail(),
            };

            await Task.Run(() => { 
                var pozafata = ImageConverter.ImageSourceToByteArray(FaceImage);
                badge.PozaFata = pozafata;

                var pozaspate = ImageConverter.ImageSourceToByteArray(BackImage);
                badge.PozaSpate = pozaspate;
                }
            );
 
            if (await IsBadgeValid(badge))
            {
                ((App)Application.Current).FirebaseUtils.AddBadgeDocument(badge);
                ((App)Application.Current).IsBadgeValid();
                await App.Current.MainPage.DisplayAlert("Success", "Legitimatia a fost inregistrata cu succes!", "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failure", "Legitimatia nu a fost gasita in baza de date a facultatii sau datele nu coincid!", "Ok");
            }

            IsUploading = false;

            await Shell.Current.GoToAsync($"//Main/{nameof(ProfilePage)}");
        }

        private async void OnLoadFacePhotoClicked()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                FaceImage = ImageSource.FromStream(() => stream);
                OnPropertyChanged(nameof(FaceImage));
            }
        }

        private async void OnLoadBackPhotoClicked()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                BackImage = ImageSource.FromStream(() => stream);
                OnPropertyChanged(nameof(BackImage));
            }
        }

        private async Task<bool> IsBadgeValid(Badge badge)
        {
            UniversityBadge uniBadge;
            uniBadge = await RDatabaseConsumer.GetBadge(badge.Numar, badge.Universitate);
            if (uniBadge == null)
                return false;

            var userDetails = await ((App)Application.Current).FirebaseUtils.GetUserDetails();

            if (uniBadge.Nume == userDetails.Nume && uniBadge.Prenume == userDetails.Prenume && uniBadge.CNP == userDetails.CNP)
                return true;

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
