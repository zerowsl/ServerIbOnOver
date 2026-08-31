namespace ServerOver.Utils;

public static class FileUtils
{
    public static void DirMakeSureExists(string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public static void DirDelNoError(string dir)
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
    }

    public static void FileDelNoError(string file)
    {
        if (File.Exists(file))
            File.Delete(file);
    }
}