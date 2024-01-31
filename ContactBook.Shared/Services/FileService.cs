using ContactBook.Shared.Interfaces;
using System.Diagnostics;

namespace ContactBook.Shared.Services;

public class FileService() : IFileService
{
    /// <summary>
    /// Retrieves content from specified filepath
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>String</returns>
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

    /// <summary>
    /// Saves content to specified filepath
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    /// <returns>Boolean</returns>
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
