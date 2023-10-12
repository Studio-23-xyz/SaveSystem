using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;

namespace Studio23.SS2.SaveSystem.Utilities
{
    public class Stitcher
    {
        public async UniTask ArchiveFiles(string archiveFilePath, List<string> filesToArchive)
        {
            try
            {
                await using (var archiveStream = new FileStream(archiveFilePath, FileMode.Create))
                await using (var writer = new BinaryWriter(archiveStream))
                {
                    foreach (var filePath in filesToArchive)
                        if (File.Exists(filePath))
                        {
                            // Write the file name and length
                            var fileName = Path.GetFileName(filePath);
                            var fileLength = new FileInfo(filePath).Length;
                            writer.Write(fileName);
                            writer.Write(fileLength);

                            // Write the file content
                            var fileBytes = await File.ReadAllBytesAsync(filePath);
                            writer.Write(fileBytes);
                        }
                }

                Console.WriteLine("Files archived successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error archiving files: " + ex.Message);
            }
        }

        public async UniTask ExtractFiles(string archiveFilePath, string extractDirectory)
        {
            try
            {
                await using (var archiveStream = new FileStream(archiveFilePath, FileMode.Open))
                using (var reader = new BinaryReader(archiveStream))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        // Read the file name and length
                        var fileName = reader.ReadString();
                        var fileLength = reader.ReadInt64();

                        // Read the file content
                        var fileBytes = reader.ReadBytes((int)fileLength);

                        // Write the file to the extraction directory
                        var filePath = Path.Combine(extractDirectory, fileName);
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