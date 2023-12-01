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

        private void CopyFiles(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine(sourcePath);
                throw new FileNotFoundException($"Source file not found: {sourcePath}");
            }
            try
            {
                File.Copy(sourcePath, destinationPath, true);
                File.SetCreationTime(destinationPath, File.GetCreationTime(sourcePath));
                File.SetLastAccessTime(destinationPath, File.GetLastAccessTime(sourcePath));
                File.SetLastWriteTime(destinationPath, File.GetLastWriteTime(sourcePath));
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Access denied. Error details: {ex.Message}");
            }
            catch (IOException ex)
            {
                throw new IOException($"Error during file copy. Error details: {ex.Message}");
            }
        }
    }
}