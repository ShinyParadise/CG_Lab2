using System.Globalization;

namespace Main.Image
{
    public class ImageFile : IBitmap
    {
        public ImageFile() { }

        public void ReadFromFile(string filename)
        {
            string hexString = "";
            int curHexIdx;

            FileStream fs = new FileStream(filename, FileMode.Open);

            while ((curHexIdx = fs.ReadByte()) != -1)
            {
                hexString += string.Format("{0:X2}", curHexIdx);
            }

            fs.Close();

            ParseData(hexString);
        }

        public void WriteToFile(string filename)
        {
            string hexString = "";

            FileStream fs = new FileStream(filename, FileMode.Create);

            hexString += _width.ToString("X4");
            hexString += _height.ToString("X4");
            hexString += _bitsOnPixel.ToString("X2");
            hexString += _paletteColorNumber.ToString("X4");

            hexString += _hexPalette;
            hexString += _hexPicture;

            fs.Write(Convert.FromHexString(hexString));
            fs.Close();
        }

        private void ParseData(string hexString)
        {
            string widthHexStr = hexString.Substring(0, 4);
            string heightHexStr = hexString.Substring(4, 4);
            string bitOnPixedHexStr = hexString.Substring(8, 2);
            string paletteColorNumberHexStr = hexString.Substring(10, 4);


            _width = int.Parse(widthHexStr, NumberStyles.HexNumber);
            _height = int.Parse(heightHexStr, NumberStyles.HexNumber);
            _bitsOnPixel = int.Parse(bitOnPixedHexStr, NumberStyles.HexNumber);
            _paletteColorNumber = int.Parse(paletteColorNumberHexStr, NumberStyles.HexNumber);
            _paletteSide = (int)Math.Sqrt(_paletteColorNumber);
            _imageResolution = _width * _height;

            _paletteSize = _paletteColorNumber * 4; // упрощение вместо colorNumber * 32 / 8

            _hexPalette = hexString.Substring(_headerLength, _paletteSize * 2);
            GetPaletteArray();

            int pictureOffset = _headerLength + _paletteSize * 2;
            _hexPicture = hexString.Substring(pictureOffset, hexString.Length - pictureOffset);
        }

        private void GetPaletteArray()
        {
            Color[,] palette = new Color[_paletteSide, _paletteSide];

            for (int y = 0; y < _paletteSide; y++)
            {
                for (int x = 0; x < _paletteSide; x++)
                {
                    string stringArgb = _hexPalette.Substring((y * _paletteSide + x) * 8, 8);
                    int argb = int.Parse(stringArgb, NumberStyles.HexNumber);
                    Color color = Color.FromArgb(argb);

                    palette[y, x] = color;
                }
            }

            _palette = palette;
        }

        public int Width { get => _width; }
        public int Height { get => _height; }
        public int BitsOnPixel { get => _bitsOnPixel; }
        public int PaletteColorNumber { get => _paletteColorNumber; }
        public int PaletteSide { get => _paletteSide; }
        public int HeaderLength { get => _headerLength; }
        public int ImageSquare { get => _imageResolution; }
        public int paletteSize { get => _paletteSize; }
        public string HexPicture { get => _hexPicture; }
        public Color[,] Palette { get => _palette; }

        private int _width;
        private int _height;
        private int _imageResolution;

        private int _bitsOnPixel;
        private readonly int _headerLength = 14;

        private int _paletteSize;
        private int _paletteColorNumber;
        private int _paletteSide;
        private Color[,] _palette = { };
        private string _hexPalette = "";

        private string _hexPicture = "";
    }
}
