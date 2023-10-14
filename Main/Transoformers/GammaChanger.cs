using Main.Image;

namespace Main.Transoformers
{
    public class GammaChanger : ITransformer
    {
        public GammaChanger(ImageFile image, short gamma)
        {
            _image = image;
            _gamma = gamma;
        }

        public void Transform()
        {

        }

        private ImageFile _image;
        private short _gamma;
    }
}
