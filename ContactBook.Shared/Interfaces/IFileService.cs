namespace ContactBook.Shared.Interfaces
{
    public interface IFileService
    {
        string GetContentFromFile(string filePath);
        bool SaveContentToFile(string filePath, string content);
    }
}