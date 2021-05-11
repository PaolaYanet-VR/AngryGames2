using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppGas.Services
{
    public class ImageService
    {
        public ImageSource ConvertImageFromBase64ToImageSource(string imageBase64)
        {
            // Convierte una imagen en formato base 64 a un formato binario una imagen para poder ser visualizada
            if (!string.IsNullOrEmpty(imageBase64))
            {
                return ImageSource.FromStream(() =>
                    new MemoryStream(System.Convert.FromBase64String(imageBase64))
                    );
            }
            else
            {
                return null; // TODO: enviar imagen de not_found
            }
        }

        public async Task<string> ConvertImageFilePathToBase64(string filepath)
        {

            if (!string.IsNullOrEmpty(filepath))
            {
                FileStream stream = File.Open(filepath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return System.Convert.ToBase64String(bytes);
            }
            else
            {
                return string.Empty;
            }
        }

        public string SaveImageFromBase64(string imageBase64, int id)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), id + ".tmp");
                byte[] data = Convert.FromBase64String(imageBase64);
                System.IO.File.WriteAllBytes(filePath, data);
                return filePath;
            }
            return string.Empty;
        }
    }
}
