using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Travel_Assistant.Services
{
    public interface IImageConverter
    {
        byte[] ImageSourceToByteArray(ImageSource source);
    }
}
