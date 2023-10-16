using Main.Image;

namespace Main.Transoformers
{
    public class GammaChanger : ITransformer
    {
        public GammaChanger(ImageFile image, float gamma = 1f)
        {
            _image = image;
            _gamma = gamma;
        }

        public void Transform()
        {
            var sideLength = _image.PaletteSide;
            var palette = _image.Palette;
            Color[,] newPalette = new Color[sideLength, sideLength];

            for (int i = 0; i<sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    byte r = palette[i, j].R;
                    byte g = palette[i, j].G;
                    byte b = palette[i, j].B;

                    newPalette[i, j] = Color.FromArgb(
                        palette[i, j].A,
                        (int)Math.Round(r * _gamma % 255),
                        (int)Math.Round(g * _gamma % 255),
                        (int)Math.Round(b * _gamma % 255)
                    );
                }
            }

            _image.Palette = newPalette;
        }

        public float Gamma { get => _gamma; set => _gamma = value; }
        public ImageFile Image { get => _image; set => _image = value; }

        private ImageFile _image;
        private float _gamma;
    }
}
