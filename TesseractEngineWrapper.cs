using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Tesseract;

namespace Bigdata.OrcTesseract
{
    public class TesseractEngineWrapper
    {
        public void Handle(string fileOrFolder)
        {
            var arr = fileOrFolder.Split(' ');
            var lang = "vie";
            if (arr.Length > 1)
            {
                lang = arr[0];
                fileOrFolder = arr[1];
            }

            List<string> files = new List<string>();
            if (File.Exists(fileOrFolder))
            {
                files.Add(fileOrFolder);
            }
            else
            {
                if (Directory.Exists(fileOrFolder))
                {
                    files = Directory.GetFiles(fileOrFolder).ToList();
                }
                else
                {
                    Console.WriteLine($"Error: not found any file or folder {fileOrFolder}");
                }
            }

            foreach (var fileImage in files)
            {
                var ocrtext = TryFindText(fileImage, lang);
                Console.WriteLine(fileImage);
                TrySaveToFile(fileImage, ocrtext);
                Console.WriteLine($"Found text in lang: {lang}: {ocrtext}");
            }

        }

        public void TrySaveToFile(string fileImage, string content)
        {
            string path = fileImage + ".txt";

            try
            {
                using (var sw = new StreamWriter(path))
                {
                    sw.WriteLine(content);
                    sw.Flush();
                }
                Console.WriteLine($"Text found store in {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not write to file {path} Error: {ex.Message}");
            }
        }
        public string TryFindText(string fileImage, string lang = "eng")
        {
            var folderData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            string ocrtext = string.Empty;
           // using(Bitmap bmp = new Bitmap(fileImage))
            {                
               
                using (TesseractEngine _tesseractEngine = new TesseractEngine(folderData, lang))
                {
                    using (Pix pix = Pix.LoadFromFile(fileImage))
                    //using (Pix pix = ConvertFromBitmap(bmp))
                    {
                        using (var page = _tesseractEngine.Process(pix))
                        {
                            ocrtext = page.GetText();
                        }
                        //pix.Save(fileImage + ".png", ImageFormat.Png);
                    }
                }
            }
            

            return ocrtext;
        }

        Pix ConvertFromBitmap(Bitmap bmp)
        {
            MemoryStream byteStream = new MemoryStream();
            bmp.Save(byteStream, System.Drawing.Imaging.ImageFormat.Tiff);
            var pix = Pix.LoadTiffFromMemory(byteStream.ToArray());
            return pix;
        }

        Bitmap GrayScale(Bitmap bmp)
        {
            //get image dimension
            int width = bmp.Width;
            int height = bmp.Height;

            //color of pixel
            Color p;

            //grayscale
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get pixel value
                    p = bmp.GetPixel(x, y);

                    //extract pixel component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find average
                    int avg = (r + g + b) / 3;

                    //set new pixel value
                    bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            }

            return bmp;
        }
    }
}
