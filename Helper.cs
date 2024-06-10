/// <summary>
/// The Helper class provides static methods to create index files for a given input text file and 
/// retrieve specific lines from the input file using these index files. This class facilitates 
/// efficient random access to lines in a large text file by creating and using index files.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Creates index files from an input text file, splitting the input file into multiple index files
    /// based on a specified number of lines per index file. Each line in the index files contains the 
    /// line number and the byte offset of the corresponding line in the input file.
    /// </summary>
    /// <param name="inputFilePath">The path to the input text file that needs to be indexed.</param>
    /// <param name="indexDirectoryPath">The directory path where the index files will be created.</param>
    public static void CreateIndexFiles(string inputFilePath, string indexDirectoryPath)
    {
        Directory.CreateDirectory(indexDirectoryPath);
        using (StreamReader file = new StreamReader(inputFilePath))
        {
            long offset = 0;
            string? line;
            int lineNum = 0;
            int fileIndex = 0;

            StreamWriter indexFile = new StreamWriter(Path.Combine(indexDirectoryPath, $"index_{fileIndex}{Configuration.IndexFileExtension}"));

            while ((line = file.ReadLine()) != null)
            {
                if (lineNum % Configuration.LinesPerIndexFile == 0 && lineNum != 0)
                {
                    indexFile.Close();
                    fileIndex++;
                    indexFile = new StreamWriter(Path.Combine(indexDirectoryPath, $"index_{fileIndex}{Configuration.IndexFileExtension}"));
                }
                indexFile.WriteLine($"{lineNum},{offset}");
                offset += line.Length + Environment.NewLine.Length; // Adding the length of newline
                lineNum++;
            }

            indexFile.Close();
        }
    }

    /// <summary>
    /// Prints a specific line from the input file using the index files to determine the line's byte offset.
    /// </summary>
    /// <param name="inputFilePath">The path to the input text file.</param>
    /// <param name="indexDirectoryPath">The directory path where the index files are located.</param>
    /// <param name="lineNumber">The line number to be printed from the input file.</param>
    public static void PrintLine(string inputFilePath, string indexDirectoryPath, int lineNumber)
    {
        long? offset = GetLineOffset(indexDirectoryPath, lineNumber);
        if (offset == null)
        {
            Console.WriteLine($"Line number {lineNumber} not found.");
            return;
        }

        using (FileStream fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
        using (StreamReader file = new StreamReader(fileStream))
        {
            file.BaseStream.Seek(offset.Value, SeekOrigin.Begin);
            string? line = file.ReadLine();
            if (line != null)
            {
                Console.WriteLine(line);
            }
            else
            {
                Console.WriteLine($"Line number {lineNumber} could not be read.");
            }
        }
    }

    /// <summary>
    /// Retrieves the byte offset of a specific line number from the appropriate index file.
    /// </summary>
    /// <param name="indexDirectoryPath">The directory path where the index files are located.</param>
    /// <param name="lineNumber">The line number whose byte offset is to be retrieved.</param>
    /// <returns>The byte offset of the specified line number, or null if the line number is not found.</returns>
    private static long? GetLineOffset(string indexDirectoryPath, int lineNumber)
    {
        int fileIndex = lineNumber / Configuration.LinesPerIndexFile;
        string indexFilePath = Path.Combine(indexDirectoryPath, $"index_{fileIndex}{Configuration.IndexFileExtension}");

        if (!File.Exists(indexFilePath))
        {
            return null;
        }

        using (StreamReader indexFile = new StreamReader(indexFilePath))
        {
            string? line;
            while ((line = indexFile.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                if (int.TryParse(parts[0], out int num) && long.TryParse(parts[1], out long offset))
                {
                    if (num == lineNumber)
                    {
                        return offset;
                    }
                }
            }
        }

        return null;
    }
}