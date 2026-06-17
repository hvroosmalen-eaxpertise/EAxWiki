namespace EAxWiki.Core.Interfaces;

public interface IOutputWriter
{
    Task CreateDirectoryAsync(string path);
    Task WriteFileAsync(string filePath, string content);
}
