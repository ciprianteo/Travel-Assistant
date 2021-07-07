using System.IO;
using System.Threading.Tasks;
using Travel_Assistant.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: Dependency(typeof(Travel_Assistant.Droid.AndroidImageHelper))]
namespace Travel_Assistant.Droid
{
    public class AndroidImageHelper : IImageConverter
    {
        public byte[] ImageSourceToByteArray(ImageSource source)
        {
            var image = GetNativeImage(source).Result;

            using var stream = new MemoryStream();
            image.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 50, stream);
            return stream.ToArray();

        }

        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler handler = null;
            if (source is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                handler = new StreamImagesourceHandler();
            }
            else if (source is FontImageSource)
            {
                handler = new FontImageSourceHandler();
            }

            return handler;
        }

        private static async Task<Android.Graphics.Bitmap> GetNativeImage(ImageSource source)
        {
            var nativeImageHandler = GetHandler(source);
            Android.Graphics.Bitmap result = await nativeImageHandler.LoadImageAsync(source, Application.Context);

            return result;
        }
    }
}
