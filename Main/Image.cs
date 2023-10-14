using System.Globalization;
using System.Text;

namespace Main
{
    public class Image
    {
        public Image() {}

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
            hexString += _palleteColorNumber.ToString("X4");

            hexString += _hexPallete;
            hexString += _hexPicture;

            fs.Write(Convert.FromHexString(hexString));
            fs.Close();
        }

        private void ParseData(string hexString)
        {
            string widthHexStr = hexString.Substring(0, 4);
            string heightHexStr = hexString.Substring(4, 4);
            string bitOnPixedHexStr = hexString.Substring(8, 2);
            string palleteColorNumberHexStr = hexString.Substring(10, 4);


            _width = int.Parse(widthHexStr, NumberStyles.HexNumber);
            _height = int.Parse(heightHexStr, NumberStyles.HexNumber);
            _bitsOnPixel = int.Parse(bitOnPixedHexStr, NumberStyles.HexNumber);
            _palleteColorNumber = int.Parse(palleteColorNumberHexStr, NumberStyles.HexNumber);
            _palleteSide = (int)Math.Sqrt(_palleteColorNumber);
            _imageResolution = _width * _height;

            _palleteSize = _palleteColorNumber * 4; // упрощение вместо colorNumber * 32 / 8

            _hexPallete = hexString.Substring(_headerLength, _palleteSize * 2);
            GetPalleteArray(_hexPallete);

            int pictureOffset = _headerLength + _palleteSize * 2;
            _hexPicture = hexString.Substring(pictureOffset, hexString.Length - pictureOffset);
        }

        private void GetPalleteArray(string hexPallete)
        {
            Color[,] pallete = new Color[_palleteSide, _palleteSide];

            for (int y = 0; y < _palleteSide; y++)
            {
                for (int x = 0; x < _palleteSide; x++)
                {
                    string stringArgb = hexPallete.Substring((y * _palleteSide + x) * 8, 8);
                    int argb = int.Parse(stringArgb, NumberStyles.HexNumber);
                    Color color = Color.FromArgb(argb);

                    pallete[y, x] = color;
                }
            }

            _pallete = pallete;
        }

        public int Width { get => _width; }
        public int Height { get => _height; }
        public int BitsOnPixel { get => _bitsOnPixel; }
        public int PalleteColorNumber { get => _palleteColorNumber; }
        public int PalleteSide { get => _palleteSide; }
        public int HeaderLength { get => _headerLength; }
        public int ImageSquare { get => _imageResolution; }
        public int PalleteSize { get => _palleteSize; }
        public string HexPicture { get => _hexPicture; }
        public Color[,] Pallete { get => _pallete; }

        private int _width;
        private int _height;
        private int _imageResolution;

        private int _bitsOnPixel;
        private readonly int _headerLength = 14;

        private int _palleteSize;
        private int _palleteColorNumber;
        private int _palleteSide;
        private Color[,] _pallete = {};
        private string _hexPallete;

        private string _hexPicture = "";
    }
}
