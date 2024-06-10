/// <summary>
/// The Configuration class holds constant configuration values used by other classes in the application.
/// These values determine key settings for creating and managing index files.
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Specifies the number of lines per index file. This value determines how many lines from the input text file 
    /// will be included in each index file before creating a new index file.
    /// </summary>
    public const int LinesPerIndexFile = 100000;

    /// <summary>
    /// Specifies the file extension used for index files. This value is appended to the names of the generated index files.
    /// </summary>
    public const string IndexFileExtension = ".idx";
}