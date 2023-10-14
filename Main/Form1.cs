using Main.Image;
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
        }

        Bitmap displayedBitmap;
        BmpFile bmp;
        Graphics graphics;
        ImageFile image;

        private void InitDrawing()
        {
            Rectangle rectangle = pictureBox1.ClientRectangle;
            displayedBitmap = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(displayedBitmap);
        }

        private void DrawImage(string hexPicture, int height, int width, Color[,] palette)
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
                    int iIdx = (curDecimalNumber & firstTwoBits) >> 2; // I-index in palette array
                    int jIdx = curDecimalNumber & secondTwoBits; // J-index in palette array

                    Color curColor = palette[iIdx, jIdx];
                    pixelCount++;

                    graphics.FillRectangle(new SolidBrush(curColor), x, y, imageScale, imageScale);
                }
            }


            pictureBox1.Image = displayedBitmap;
        }


        private void ImageHandler()
        {
            image = new ImageFile();
            image.ReadFromFile("./6_6_image_4_4_pal.bin");

            DrawImage(image.HexPicture, image.Height, image.Width, image.Palette);
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = @"image|*.bmp" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bmp = new BmpFile();
                    bmp.ReadFromFile(openFileDialog.FileName);
                    displayedBitmap = bmp.Bitmap;
                    pictureBox1.Image = displayedBitmap;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bmp == null) { return; }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"image|*.bmp" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bmp.WriteToFile(saveFileDialog.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Тут писать код для обработки увеличения изображения
        }
    }
}
