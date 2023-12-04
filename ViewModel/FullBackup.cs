using System;
using System.IO;

namespace BackupSoftware.ViewModel
{
    class FullBackup : IBackupStrategy
    {
        public string Backup(string source, string destination)
        {
            try
            {
                string[] files = Directory.GetFiles(source);

                foreach (string sourcePath in files)
                {
                    string fileName = Path.GetFileName(sourcePath);
                    string destinationPath = Path.Combine(destination, fileName);

                    byte[] fileBytes = File.ReadAllBytes(sourcePath);

                    File.WriteAllBytes(destinationPath, fileBytes);

                    File.SetCreationTime(destinationPath, File.GetCreationTime(sourcePath));
                    File.SetLastAccessTime(destinationPath, File.GetLastAccessTime(sourcePath));
                    File.SetLastWriteTime(destinationPath, File.GetLastWriteTime(sourcePath));
                }

                return "Full backup completed successfully.";
            }
            catch (Exception ex)
            {
                return $"Error in full backup: {ex.Message}";
            }
        }
    }
}

