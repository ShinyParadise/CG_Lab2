namespace Main.Image
{
    public interface IBitmap
    {
        public void ReadFromFile(string filename);

        public void WriteToFile(string filename);
    }
}
