using System.Globalization;

namespace Main.Image
{
    public class ImageFile : ISaveable, ICloneable
    {
        public ImageFile() { }

        public ImageFile(int width,
            int height,
            int imageResolution,
            int bitsPerPixel,
            int paletteSize,
            int paletteColorNumber,
            int paletteSide,
            Color[,] palette,
            string hexPalette,
            string hexPicture)
        {
            _width = width;
            _height = height;
            _imageResolution = imageResolution;
            _bitsPerPixel = bitsPerPixel;
            _paletteSize = paletteSize;
            _paletteColorNumber = paletteColorNumber;
            _paletteSide = paletteSide;
            _palette = palette;
            _hexPalette = hexPalette;
            _hexPicture = hexPicture;
        }

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
            hexString += _bitsPerPixel.ToString("X2");
            hexString += _paletteColorNumber.ToString("X4");

            hexString += _hexPalette;
            hexString += _hexPicture;

            fs.Write(Convert.FromHexString(hexString));
            fs.Close();
        }

        public void Scale()
        {
            int scaleSize = 2;
            int scaledWidth = _width * scaleSize;
            int scaledHeight = _height * scaleSize;
            int offset = 8 - _bitsPerPixel;

            string newHexImage = "";

            if (_bitsPerPixel == 4)
            {
                for (int i = 0; i < _height; i++)
                {
                    string newRow = "";
                    for (int j = 0; j < _width; j++)
                    {
                        char c = _hexPicture[i * _height + j];
                        // ок
                        newRow += c;
                        newRow += c;
                    }
                    newHexImage += newRow + newRow;
                }
            }
            else
            {
                byte[] newByteImage = new byte[scaledWidth * scaledHeight];
                byte[] byteImage = Convert.FromHexString(_hexPicture);
                int pixel;
                int pixelOffset;
                int mask = 255;

                // вычленяются и дублируются 5-битовые пиксели (в теории любое кол-во меньше 8, не равное 4)
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        pixelOffset = (i * _height + j) * _bitsPerPixel;
                        offset = pixelOffset % 8;                        
                        pixel = byteImage[pixelOffset / 8] << offset; // почистили впереди
                        pixel = pixel & mask;
                        pixel = pixel >> 8 - _bitsPerPixel;
                        if (offset > 8 - _bitsPerPixel) // не весь пиксель в байте
                        {
                            pixel = pixel >> (_bitsPerPixel - 8 + offset);
                            pixel = pixel << (_bitsPerPixel - 8 + offset);
                            pixel += byteImage[pixelOffset / 8 + 1] >> (16 - _bitsPerPixel - offset);
                        }

                        for (int k = 0; k < scaleSize; k++)
                        {
                            for (int n = 0; n < scaleSize; n++)
                            {
                                newByteImage[scaleSize * (scaleSize * i + k) * _width + scaleSize * j + n] = (byte)pixel;
                            }
                        }
                    }
                }


                // упаковка, не работает
                offset = 8 - _bitsPerPixel;
                int bitRemain = 8;
                pixel = 0;
                for (int j = 0; j < scaledHeight * scaledWidth - 1; )
                {
                    while (bitRemain >= _bitsPerPixel)
                    {                        
                        pixel += (byte)(newByteImage[j] << offset);
                        j++;
                        bitRemain = bitRemain - _bitsPerPixel;
                        offset -= _bitsPerPixel;
                    }

                    if (bitRemain != 0)
                    {
                        pixel += (byte)(newByteImage[j] >> (_bitsPerPixel - bitRemain));
                        newHexImage += ((byte)pixel).ToString("X2"); // записать пиксель
                        pixel = (byte)(newByteImage[j] << (8 - _bitsPerPixel + bitRemain));
                        j++;
                        offset = 8 - _bitsPerPixel - bitRemain + 1;
                        bitRemain = 8 - (_bitsPerPixel - bitRemain);
                    }
                    else
                    {
                        offset = 8 - _bitsPerPixel;
                        bitRemain = 8;
                        newHexImage += ((byte)pixel).ToString("X2");
                        pixel = 0;
                    }
                }
            }

            _hexPicture = newHexImage;
            _width = scaledWidth;
            _height = scaledHeight;
        }

        private void ParseData(string hexString)
        {
            string widthHexStr = hexString.Substring(0, 4);
            string heightHexStr = hexString.Substring(4, 4);
            string bitOnPixedHexStr = hexString.Substring(8, 2);
            string paletteColorNumberHexStr = hexString.Substring(10, 4);


            _width = int.Parse(widthHexStr, NumberStyles.HexNumber);
            _height = int.Parse(heightHexStr, NumberStyles.HexNumber);
            _bitsPerPixel = int.Parse(bitOnPixedHexStr, NumberStyles.HexNumber);
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

            for (int x = 0; x < _paletteSide; x++)
            {
                for (int y = 0; y < _paletteSide; y++)
                {
                    string stringArgb = _hexPalette.Substring((x * _paletteSide + y) * 8, 8);
                    int argb = int.Parse(stringArgb, NumberStyles.HexNumber);
                    Color color = Color.FromArgb(argb);

                    palette[x, y] = color;
                }
            }

            _palette = palette;
        }

        public object Clone()
        {
            var cloned = new ImageFile(_width, _height, _imageResolution, _bitsPerPixel, _paletteSize, _paletteColorNumber, _paletteSide, _palette, _hexPalette, _hexPicture);
            return cloned;
        }

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        public int BitsPerPixel { get => _bitsPerPixel; set => _bitsPerPixel = value; }
        public int PaletteColorNumber { get => _paletteColorNumber; set => _paletteColorNumber = value; }
        public int PaletteSide { get => _paletteSide; set => _paletteSide = value; }
        public int HeaderLength { get => _headerLength; }
        public int ImageResolution { get => _imageResolution; set => _imageResolution = value; }
        public int PaletteSize { get => _paletteSize; set => _paletteSize = value; }
        public string HexPicture { get => _hexPicture; set => _hexPicture = value; }
        public Color[,] Palette { get => _palette; set => _palette = value;  }

        private int _width;
        private int _height;
        private int _imageResolution;

        private int _bitsPerPixel;
        private readonly int _headerLength = 14;

        private int _paletteSize;
        private int _paletteColorNumber;
        private int _paletteSide;
        private Color[,] _palette = { };
        private string _hexPalette = "";

        private string _hexPicture = "";
    }
}
