namespace Main.Image
{
    public interface ISaveable
    {
        void ReadFromFile(string filename);

        void WriteToFile(string filename);
    }
}
