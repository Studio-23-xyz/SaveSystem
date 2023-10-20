using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;

namespace Studio23.SS2.SaveSystem.Utilities
{
    internal class Stitcher
    {
        internal async UniTask ArchiveFiles(string archiveFilePath, List<string> filesToArchive)
        {
            try
            {
                await using (var archiveStream = new FileStream(archiveFilePath, FileMode.Create))
                await using (var writer = new BinaryWriter(archiveStream))
                {
                    foreach (var filePath in filesToArchive)
                    {
                        if (File.Exists(filePath))
                        {
                            // Calculate the relative path
                            var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, filePath);

                            // Write the relative path, file name, and length
                            var fileName = Path.GetFileName(filePath);
                            var fileLength = new FileInfo(filePath).Length;
                            writer.Write(relativePath);
                            writer.Write(fileName);
                            writer.Write(fileLength);

                            // Write the file content
                            var fileBytes = await File.ReadAllBytesAsync(filePath);
                            writer.Write(fileBytes);
                        }
                    }
                }

                Console.WriteLine("Files archived successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error archiving files: " + ex.Message);
            }
        }

        internal async UniTask ExtractFiles(string archiveFilePath, string extractDirectory)
        {
            try
            {
                await using (var archiveStream = new FileStream(archiveFilePath, FileMode.Open))
                using (var reader = new BinaryReader(archiveStream))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        // Read the relative path, file name, and length
                        var relativePath = reader.ReadString();
                        var fileName = reader.ReadString();
                        var fileLength = reader.ReadInt64();

                        // Combine the relative path and extract directory to get the full path
                        var filePath = Path.Combine(extractDirectory, relativePath, fileName);

                        // Ensure the directory for the file exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        // Read the file content
                        var fileBytes = reader.ReadBytes((int)fileLength);

                        // Write the file to the extraction directory
                        await File.WriteAllBytesAsync(filePath, fileBytes);
                    }
                }

                Console.WriteLine("Files extracted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting files: " + ex.Message);
            }
        }

    }
}