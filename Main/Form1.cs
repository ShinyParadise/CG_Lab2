using System.Drawing.Imaging;
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
        Image image;

        private void InitDrawing()
        {
            Rectangle rectangle = pictureBox1.ClientRectangle;
            bitMap = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(bitMap);
        }

        private void DrawImage(string hexPicture, int height, int width, Color[,] pallete)
        {
            int firstTwoBits = 12; // 1100
            int secondTwoBits = 3; // 0011
            int imageScale = 30;   // увеличение изображения для того чтобы его было видно на экране

            int pixelCount = 0;
            for (int y = 0; y < height * imageScale; y += imageScale)
            {
                for (int x = 0; x < width * imageScale; x += imageScale)
                {

                    int curDecimalNumber = int.Parse(hexPicture[pixelCount].ToString(), NumberStyles.HexNumber);
                    int iIdx = (curDecimalNumber & firstTwoBits) >> 2; // I-index in pallete array
                    int jIdx = curDecimalNumber & secondTwoBits; // J-index in pallete array

                    Color curColor = pallete[iIdx, jIdx];
                    pixelCount++;

                    graphics.FillRectangle(new SolidBrush(curColor), x, y, imageScale, imageScale);
                }
            }


            pictureBox1.Image = bitMap;
        }


        private void ImageHandler()
        {
            image = new Image();
            image.ReadFromFile("./6_6_image_4_4_pal.bin");

            DrawImage(image.HexPicture, image.Height, image.Width, image.Pallete);
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

                    int alphaInt = int.Parse(alpha, NumberStyles.HexNumber);
                    int redInt = int.Parse(red, NumberStyles.HexNumber);
                    int greenInt = int.Parse(green, NumberStyles.HexNumber);
                    int blueInt = int.Parse(blue, NumberStyles.HexNumber);

                    Color color = Color.FromArgb(alphaInt, redInt, greenInt, blueInt);
                    Console.WriteLine(color);

                    graphics.FillRectangle(new SolidBrush(color), x, y, 1, 1);


                    count += 6;
                }
            }


            pictureBox1.Image = bitMap;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    SaveImage();
                    break;
            }
        }

        private void SaveImage()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"binary|*.bin" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    image.WriteToFile(saveFileDialog.FileName);
                }
            }
        }

        /*private void BmpImageHandler()
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
        }*/
    }
}
