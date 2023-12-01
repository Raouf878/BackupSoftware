using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using BackupSoftware.Model;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace BackupSoftware.ViewModel
{
    class SingletonBackupJob
    {
        private static SingletonBackupJob instance;
        private static readonly object lockObject = new object();
        private Job jb;
        private List<Job> Jobs = new List<Job>();

        public SingletonBackupJob(Job jb)
        {
            this.jb = jb;
        }

        public static SingletonBackupJob GetBackupJobInstance(Job jb)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SingletonBackupJob(jb);
                    }
                }
            }
            return instance;
        }

        public String RunBackupJob()
        {
            try
            {
                CopyFiles(jb.Source, jb.Destination);
                return "Backup job completed successfully.";
            }
            catch (FileNotFoundException ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
            catch (UnauthorizedAccessException ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
            catch (IOException ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
        }

        private void CopyFiles(string source, string destination)
        {
            try
            {
                // Get all files in the source directory
                string[] files = Directory.GetFiles(source);

                // Iterate through each file and copy it to the destination directory
                foreach (string sourcePath in files)
                {
                    // Construct the destination path by combining the destination directory and the file name
                    string fileName = Path.GetFileName(sourcePath);
                    string destinationPath = Path.Combine(destination, fileName);

                    // Read all bytes from the source file
                    byte[] fileBytes = File.ReadAllBytes(sourcePath);

                    // Write the bytes to a new file at the destination
                    File.WriteAllBytes(destinationPath, fileBytes);

                    // Set the creation time, last access time, and last write time of the destination file
                    File.SetCreationTime(destinationPath, File.GetCreationTime(sourcePath));
                    File.SetLastAccessTime(destinationPath, File.GetLastAccessTime(sourcePath));
                    File.SetLastWriteTime(destinationPath, File.GetLastWriteTime(sourcePath));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle unauthorized access exception
                throw new UnauthorizedAccessException($"Access denied. Error details: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Handle general I/O exception during file copy
                throw new IOException($"Error during file copy. Error details: {ex.Message}");
            }
        }
    }
}