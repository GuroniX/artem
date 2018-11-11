using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Atrem.Animation.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var currentDirectory = Directory.GetCurrentDirectory();
            //var smilesDirectory = Path.Combine(currentDirectory, "Smiles");
            //var smilePaths = Directory.GetFiles(smilesDirectory);
            //foreach (var smilePath in smilePaths)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Smile from {0}", smilePath);
            //    var smile = File.ReadAllText(smilePath);
            //    Console.WriteLine(smile);
            //}

            foreach (var remoteImageUrl in _remoteImageUrls)
            {
                var image = GetBitmapFromUrl(remoteImageUrl);
                var ascii = ConvertImageToAsciiArt(image);

                Console.WriteLine("From {0}", remoteImageUrl);
                Console.Write(ascii);
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        private static string[] _remoteImageUrls = new []
        {
            "https://www.sunhome.ru/i/wallpapers/61/pozitivnie-kartinki.orig.jpg",
            "https://bipbap.ru/wp-content/uploads/2017/10/0_8eb56_842bba74_XL-640x400.jpg",
            "https://bipbap.ru/wp-content/uploads/2017/10/10-640x400.jpg"
        };

        private const int AsciiWidth = 150;

        private static readonly string[] AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

        private static Bitmap GetBitmapFromUrl(string remoteImageUrl)
        {
            WebRequest request = WebRequest.Create(remoteImageUrl);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bitmap = new Bitmap(responseStream);
            return bitmap;
        }

        private static string ConvertImageToAsciiArt(Bitmap image)
        {
            image = GetReSizedImage(image, AsciiWidth);

            //Convert the resized image into ASCII
            string ascii = ConvertToAscii(image);
            return ascii;
        }

        private static Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth)
        {
            int asciiHeight = 0;
            //Calculate the new Height of the image from its width
            asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);

            //Create a new Bitmap and define its resolution
            Bitmap result = new Bitmap(asciiWidth, asciiHeight);
            Graphics g = Graphics.FromImage(result);
            //The interpolation mode produces high quality images 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }


        private static string ConvertToAscii(Bitmap image)
        {
            Boolean toggle = false;
            StringBuilder sb = new StringBuilder();

            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color pixelColor = image.GetPixel(w, h);
                    //Average out the RGB components to find the Gray Color
                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayColor = Color.FromArgb(red, green, blue);

                    //Use the toggle flag to minimize height-wise stretch
                    if (!toggle)
                    {
                        int index = (grayColor.R * 10) / 255;
                        sb.Append(AsciiChars[index]);
                    }
                }

                if (!toggle)
                {
                    sb.Append(Environment.NewLine);
                    toggle = true;
                }
                else
                {
                    toggle = false;
                }
            }

            return sb.ToString();
        }
    }
}
