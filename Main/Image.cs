﻿using System.Globalization;

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
            _palleteSideLength = (int)Math.Sqrt(_palleteColorNumber);
            _imageResolution = _width * _height;

            _palleteSize = (_imageResolution * _bitsOnPixel) / 8;


            string hexPallete = hexString.Substring(_headerLength, hexString.Length - _headerLength - _palleteSize * 2);
            GetPalleteArray(_palleteSideLength, hexPallete);

            _hexPicture = hexString.Substring(142, hexString.Length - 142);
        }

        private void GetPalleteArray(int palleteSide, string hexPallete)
        {
            Color[,] pallete = new Color[palleteSide, palleteSide];

            for (int y = 0; y < palleteSide; y++)
            {
                for (int x = 0; x < palleteSide; x++)
                {

                    string stringArgb = hexPallete.Substring((y * palleteSide + x) * 8, 8);
                    int argb = Int32.Parse(stringArgb, NumberStyles.HexNumber);
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
        public int PalleteSideLength { get => _palleteSideLength; }
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
        private int _palleteSideLength;
        private Color[,] _pallete = {};

        private string _hexPicture = "";
    }
}