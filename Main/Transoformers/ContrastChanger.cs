using Main.Image;

namespace Main.Transoformers
{
    public class ContrastChanger : ITransformer
    {
        public ContrastChanger(ImageFile image, byte contrast = 0xFF)
        {
            _image = image;
            _contrast = contrast;
        }

        public void Transform()
        {
            var sideLength = _image.PaletteSide;
            var palette = _image.Palette;
            Color[,] newPalette = new Color[sideLength, sideLength];

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    byte a = _contrast;
                    newPalette[i, j] = Color.FromArgb(a, palette[i, j]);
                }
            }

            _image.Palette = newPalette;
        }

        private ImageFile _image;
        private byte _contrast;

        public ImageFile Image { get => _image; set => _image = value; }
        public byte Contrast { get => _contrast; set => _contrast = value; }
    }
}
