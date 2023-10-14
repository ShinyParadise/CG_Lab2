using Main.Image;
using System.Drawing.Imaging;

namespace Main
{
    public class BmpFile : IBitmap
    {
        public void ReadFromFile(string filename)
        {
            _bitmap = (Bitmap) Bitmap.FromFile(filename);
        }

        public void WriteToFile(string filename)
        {
            _bitmap.Save(filename, ImageFormat.Bmp);
        }

        public Bitmap Bitmap { get { return _bitmap; } }

        private Bitmap _bitmap;
    }
}
