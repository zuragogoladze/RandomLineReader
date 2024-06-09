public static class Helper
{
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
                var parts = line.Split(',');
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