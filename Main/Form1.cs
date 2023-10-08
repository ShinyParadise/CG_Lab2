using System.Globalization;

namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitDrawing();
            ImageHandler();
            //BmpImageHandler();
        }

        Bitmap bitMap;
        Graphics graphics;

        private void InitDrawing()
        {
            Rectangle rectangle = pictureBox1.ClientRectangle;
            bitMap = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(bitMap);
        }

        private string ReadFile(string filePath)
        {
            string hexString = "";
            int curHexIdx;

            FileStream fs = new FileStream(filePath, FileMode.Open);

            while ((curHexIdx = fs.ReadByte()) != -1)
            {
                hexString += string.Format("{0:X2}", curHexIdx);
            }

            fs.Close();

            return hexString;
        }


        private Color[,] GetPalleteArray(int palleteSide, string hexPallete)
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

            return pallete;
        }


        private void DrawImage(string hexPicture, int height, int width, Color[,] pallete)
        {

            int firstTwoBits = 12; // 1100
            int secondTwoBits = 3; // 0011

            int imageScale = 30;
            int count = 0; // небольшой костыль
            for (int y = 0; y < height * imageScale; y += imageScale)
            {
                for (int x = 0; x < width * imageScale; x += imageScale)
                {

                    int curDecimalNumber = int.Parse(hexPicture[count].ToString(), NumberStyles.HexNumber);
                    int iIdx = (curDecimalNumber & firstTwoBits) >> 2; // I-index in pallete array
                    int jIdx = curDecimalNumber & secondTwoBits; // J-index in pallete array

                    Color curColor = pallete[iIdx, jIdx];
                    count++;


                    graphics.FillRectangle(new SolidBrush(curColor), x, y, imageScale, imageScale);
                }
            }


            pictureBox1.Image = bitMap;
        }


        private void ImageHandler()
        {

            int width;
            int height;
            int bitsOnPixel;
            int palleteColorNumber;
            int palleteSideLength;
            int headerLength = 7 * 2;
            int imageSquare;
            int palleteSize;


            string hexString = ReadFile(@".\labaARGB.bin");


            // заголовок
            string widthHexStr = hexString.Substring(0, 4);
            string heightHexStr = hexString.Substring(4, 4);
            string bitOnPixedHexStr = hexString.Substring(8, 2);
            string palleteColorNumberHexStr = hexString.Substring(10, 4);


            width = int.Parse(widthHexStr, NumberStyles.HexNumber);
            height = int.Parse(heightHexStr, NumberStyles.HexNumber);
            bitsOnPixel = int.Parse(bitOnPixedHexStr, NumberStyles.HexNumber);
            palleteColorNumber = int.Parse(palleteColorNumberHexStr, NumberStyles.HexNumber);
            palleteSideLength = (int)Math.Sqrt(palleteColorNumber);
            imageSquare = width * height;

            palleteSize = (imageSquare * bitsOnPixel) / 8;


            string hexPallete = hexString.Substring(headerLength, hexString.Length - headerLength - palleteSize * 2);
            Color[,] pallete = GetPalleteArray(palleteSideLength, hexPallete);


            string hexPicture = hexString.Substring(142, hexString.Length - 142);
            DrawImage(hexPicture, height, width, pallete);
        }


        private void DrawBmpPicture(string hexPicture, int height, int width)
        {

            int imageOffset = 200;

            int count = 0;
            for (int y = height + imageOffset; y > imageOffset; y--)
            {
                for (int x = imageOffset; x < width + imageOffset; x++)
                {
                    string curHexColor = hexPicture.Substring(count, 6);
                    string alpha = "FF";
                    string red = curHexColor.Substring(0, 2);
                    string green = curHexColor.Substring(2, 2);
                    string blue = curHexColor.Substring(4, 2);

                    int alphaInt = Int32.Parse(alpha, NumberStyles.HexNumber);
                    int redInt = Int32.Parse(red, NumberStyles.HexNumber);
                    int greenInt = Int32.Parse(green, NumberStyles.HexNumber);
                    int blueInt = Int32.Parse(blue, NumberStyles.HexNumber);

                    Color color = Color.FromArgb(alphaInt, redInt, greenInt, blueInt);
                    Console.WriteLine(color);

                    graphics.FillRectangle(new SolidBrush(color), x, y, 1, 1);


                    count += 6;
                }
            }


            pictureBox1.Image = bitMap;
        }


        private void BmpImageHandler()
        {
            int width;
            int height;

            string hexString = ReadFile(@".\image_24b.bmp");

            string widthHexStr = hexString.Substring(18 * 2, 2);
            string heightHexStr = hexString.Substring(22 * 2, 2);

            width = int.Parse(widthHexStr, NumberStyles.HexNumber);
            height = int.Parse(heightHexStr, NumberStyles.HexNumber);


            string hexPicture = hexString.Substring(54 * 2, hexString.Length - 54 * 2);
            DrawBmpPicture(hexPicture, height, width);
        }
    }
}
