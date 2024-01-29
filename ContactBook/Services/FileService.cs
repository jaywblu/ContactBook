using System.Diagnostics;

namespace ContactBook.Services;

public interface IFileService
{
    bool SaveContentToFile(string filePath, string content);
    string GetContentFromFile(string filePath);
}

public class FileService() : IFileService
{

    public string GetContentFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                using var sr = new StreamReader(filePath);
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public bool SaveContentToFile(string filePath, string content)
    {
        try
        {
            using (var sw = new StreamWriter(filePath))
            {
                //sw.WriteLine(content);
                sw.Write(content);
            }
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
