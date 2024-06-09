if (args.Length != 2)
{
    Console.WriteLine("Usage: random_line <inputFile> <lineNumber>");
    return;
}

string inputFilePath = args[0];
int lineNumber;

if (!int.TryParse(args[1], out lineNumber))
{
    Console.WriteLine("Line number must be an integer.");
    return;
}

string indexDirectoryPath = inputFilePath + "_index";

if (!Directory.Exists(indexDirectoryPath))
{
    Helper.CreateIndexFiles(inputFilePath, indexDirectoryPath);
    Console.WriteLine($"Writing index to {indexDirectoryPath}... done.");
}

Helper.PrintLine(inputFilePath, indexDirectoryPath, lineNumber);