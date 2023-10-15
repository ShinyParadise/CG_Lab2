using Main.Image;
using Main.Transoformers;
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
        }

        Bitmap displayedBitmap;
        BmpFile bmp;
        Graphics graphics;
        ImageFile image;
        GammaChanger gammaChanger;
        ContrastChanger contrastChanger;

        private void InitDrawing()
        {
            Rectangle rectangle = pictureBox1.ClientRectangle;
            displayedBitmap = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(displayedBitmap);
        }

        private void DrawImage(ImageFile image)
        {
            graphics.Clear(Color.Transparent);
            string hexPicture = image.HexPicture;
            int height = image.Height;
            int width = image.Width;
            Color[,] palette = image.Palette;

            int firstTwoBits = 12; // 1100
            int secondTwoBits = 3; // 0011
            int imageScale = 30;   // увеличение изображения для того чтобы его было видно на экране

            int pixelCount = 0;
            for (int x = 0; x < height * imageScale; x += imageScale)
            {
                for (int y = 0; y < width * imageScale; y += imageScale)
                {

                    int curDecimalNumber = int.Parse(hexPicture[pixelCount].ToString(), NumberStyles.HexNumber);
                    int idX = (curDecimalNumber & firstTwoBits) >> 2; // I-index in palette array
                    int idY = curDecimalNumber & secondTwoBits; // J-index in palette array

                    Color curColor = palette[idX, idY];
                    pixelCount++;

                    graphics.FillRectangle(new SolidBrush(curColor), y, x, imageScale, imageScale);
                }
            }
            pictureBox1.Image = displayedBitmap;
        }


        private void ImageHandler()
        {
            image = new ImageFile();
            image.ReadFromFile("./5_5_image_5_5_pal.bin");

            gammaChanger = new GammaChanger(image);
            contrastChanger = new ContrastChanger(image, 50);

            DrawImage(image);
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
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"image|*.bmp" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            image.Scale();
            DrawImage(image);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            gammaChanger = new GammaChanger((ImageFile)image.Clone(), trackBar1.Value / 2f);
            gammaChanger.Transform();
            DrawImage(gammaChanger.Image);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            var value = BitConverter.GetBytes(trackBar2.Value)[0];
            contrastChanger = new ContrastChanger((ImageFile)image.Clone(), value);
            contrastChanger.Transform();
            DrawImage(contrastChanger.Image);
        }
    }
}
