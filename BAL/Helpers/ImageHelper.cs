using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BAL.Helpers
{
    public class ImageHelper
    {
        public static string Base64ToImage(byte[] image)
        {
            using (var ms = new MemoryStream())
            {
                string base64String = Convert.ToBase64String(image);
                var imgSrc = string.Format($"data:image/gif;base64,{base64String}");
                return imgSrc;
            }
            

        }
        public static async Task<byte[]> ImageToBase64(IFormFile image)
        {
            using(var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }
    }
}
