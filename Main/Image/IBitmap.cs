namespace Main.Image
{
    public interface IBitmap
    {
        void ReadFromFile(string filename);

        void WriteToFile(string filename);
    }
}
