using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    public class BadgeViewModel
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

            UniversitiesList.Add("UNIVERSITATEA POLITEHNICA DIN BUCUREŞTI");
            UniversitiesList.Add("UNIVERSITATEA TEHNICĂ DE CONSTRUCŢII DIN BUCUREŞTI ");
            UniversitiesList.Add("UNIVERSITATEA DIN BUCUREŞTI");
            UniversitiesList.Add("ACADEMIA DE STUDII ECONOMICE DIN BUCUREŞTI");
            UniversitiesList.Add("UNIVERSITATEA DE MEDICINĂ ŞI FARMACIE CAROL DAVILA DIN BUCUREŞTI");
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

            IsUploading = false;
            ((App)Application.Current).FirebaseUtils.AddBadgeDocument(badge);
        }

        private async void OnLoadFacePhotoClicked()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                FaceImage = ImageSource.FromStream(() => stream);
            }
        }

        private async void OnLoadBackPhotoClicked()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                BackImage = ImageSource.FromStream(() => stream);
            }
        }
    }
}
