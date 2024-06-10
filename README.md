# RandomLineReader

## Overview

`RandomLineReader` is a .NET 7 console application that reads a specific line from a given file and writes the index to a separate index file. The application is distributed as self-contained images for both macOS and Windows, allowing it to be run without requiring the .NET runtime to be installed on the user's machine.

## Running the Application

### Windows

1. **Open Command Prompt** and navigate to the directory where the Windows version of the application is located:
   ```sh
   cd path\to\RandomLineReader\publish\windows
2. **Run the application** with the input file and index as arguments:
    ```sh
    RandomLineReader.exe ../input_file.txt 2

### macOS

1. **Open Command Prompt** and navigate to the directory where the Windows version of the application is located:
   ```sh
   cd path/to/RandomLineReader/publish/macos
2. **Make the application executable** (if not already executable):
    ```sh
    chmod +x RandomLineReader
3. **Run the application** with the input file and index as arguments:
    ```sh
    ./RandomLineReader ../input_file.txt 2