using Main.Image;

namespace Main.Transoformers
{
    public class ContrastChanger : ITransformer
    {
        public ContrastChanger(ImageFile image, short contrast)
        {
            _image = image;
            _contrast = contrast;
        }

        public void Transform()
        {
            
        }

        private ImageFile _image;
        private short _contrast;
    }
}
