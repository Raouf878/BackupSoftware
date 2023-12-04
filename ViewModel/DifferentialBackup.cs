using System;
using System.IO;

namespace BackupSoftware.ViewModel
{
    class DifferentialBackup : IBackupStrategy
    {
        private DateTime lastBackupTime;

        public DifferentialBackup()
        {
            lastBackupTime = DateTime.MinValue;
        }

        public string Backup(string source, string destination)
        {
            try
            {
                string[] files = Directory.GetFiles(source);

                foreach (string sourcePath in files)
                {
                    if (File.GetLastWriteTime(sourcePath) > lastBackupTime)
                    {
                        string fileName = Path.GetFileName(sourcePath);
                        string destinationPath = Path.Combine(destination, fileName);

                        byte[] fileBytes = File.ReadAllBytes(sourcePath);

                        File.WriteAllBytes(destinationPath, fileBytes);

                        File.SetCreationTime(destinationPath, File.GetCreationTime(sourcePath));
                        File.SetLastAccessTime(destinationPath, File.GetLastAccessTime(sourcePath));
                        File.SetLastWriteTime(destinationPath, File.GetLastWriteTime(sourcePath));
                    }
                }

                lastBackupTime = DateTime.Now;

                return "Differential backup completed successfully.";
            }
            catch (Exception ex)
            {
                return $"Error in differential backup: {ex.Message}";
            }
        }
    }
}

