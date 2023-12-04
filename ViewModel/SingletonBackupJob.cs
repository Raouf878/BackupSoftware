using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using BackupSoftware.Model;

namespace BackupSoftware.ViewModel
{
    interface IBackupStrategy
    {
        string Backup(string source, string destination);
    }

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

    class BackupManager
    {
        private List<BackupJob> backupJobs;

        public BackupManager()
        {
            backupJobs = new List<BackupJob>();
        }
        public BackupJob GetLastBackupJob()
        {
            return backupJobs.LastOrDefault();
        }
        public void AddBackupJob(Job jobInstance)
        {
            if (backupJobs.Count < 5)
            {
                var backupJob = new BackupJob(jobInstance);
                backupJobs.Add(backupJob);
            }
            else
            {
                Console.WriteLine("Error: Maximum number of backup jobs (5) reached.");
            }
        }

        public void RunBackupJobs()
        {
            foreach (var backupJob in backupJobs)
            {
                backupJob.RunBackupJob();
            }
        }
    }

    class BackupJob
    {
        private Job jobInstance;
        private LogFile logFile;
        private IBackupStrategy backupStrategy;

        public BackupJob(Job jobInstance)
        {
            this.jobInstance = jobInstance;
            this.logFile = new LogFile();
        }

        public void SetBackupStrategy(IBackupStrategy strategy)
        {
            this.backupStrategy = strategy;
        }

        public string RunBackupJob()
        {
            try
            {
                if (backupStrategy == null)
                {
                    return "Error: Backup strategy not set.";
                }

                string sourcePath = jobInstance.Source;
                string destinationPath = jobInstance.Destination;

                if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
                {
                    return "Error: Source or destination path is null or empty.";
                }

                sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sourcePath);
                destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationPath);

                if (!Directory.Exists(sourcePath))
                {
                    return $"Error: Source directory does not exist - {sourcePath}";
                }

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                string result = backupStrategy.Backup(sourcePath, destinationPath);
                logFile.WriteLogFile(jobInstance.Name, sourcePath, destinationPath, result);
                return result;
            }
            catch (Exception ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
        }
    }

    class LogFile
    {
        private List<LogEntry> logEntries;

        public LogFile()
        {
            logEntries = new List<LogEntry>();
        }

        public void WriteLogFile(string jobName, string source, string destination, string result)
        {
            try
            {
                List<BackupFileInfo> backupFilesInfo = new List<BackupFileInfo>();

                string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BackupLogFile");
                Directory.CreateDirectory(logDirectory);  

                string logFilePath = Path.Combine(logDirectory, "BackupLogFile");

                if (!File.Exists(logFilePath))
                {
                    using (StreamWriter createLogFile = File.CreateText(logFilePath))
                    {
                        createLogFile.WriteLine("Log file created at: " + DateTime.Now);
                    }
                }

                string[] files = Directory.GetFiles(source);

                foreach (string sourcePath in files)
                {
                    BackupFileInfo fileInfo = new BackupFileInfo
                    {
                        FileName = Path.GetFileName(sourcePath),
                        SizeInBytes = new FileInfo(sourcePath).Length,
                        SourceTimestamp = File.GetLastWriteTime(sourcePath),
                        TransferTimestamp = DateTime.Now,
                        SourcePath = sourcePath,
                        DestinationPath = Path.Combine(destination, Path.GetFileName(sourcePath)),
                    };

                    byte[] fileBytes = File.ReadAllBytes(sourcePath);
                    File.WriteAllBytes(fileInfo.DestinationPath, fileBytes);

                    FileInfo destinationInfo = new FileInfo(fileInfo.DestinationPath);
                    destinationInfo.CreationTimeUtc = fileInfo.SourceTimestamp;
                    destinationInfo.LastAccessTimeUtc = fileInfo.SourceTimestamp;
                    destinationInfo.LastWriteTimeUtc = fileInfo.SourceTimestamp;

                    fileInfo.TransferTimeInMilliseconds = (DateTime.Now - fileInfo.TransferTimestamp).TotalMilliseconds;
                    backupFilesInfo.Add(fileInfo);
                }

                BackupLogEntry logEntry = new BackupLogEntry
                {
                    JobName = jobName,
                    SourceDirectory = source,
                    DestinationDirectory = destination,
                    Result = result,
                    BackupFilesInfo = backupFilesInfo
                };

                logEntries.Find(entry => entry.JobName == jobName)?.BackupLogEntries.Add(logEntry);

                string jsonLogEntry = JsonConvert.SerializeObject(logEntry, Formatting.Indented);

                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(jsonLogEntry);
                }

                Console.WriteLine($"Log: Job '{jobName}' from '{source}' to '{destination}': {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        private class LogEntry
        {
            public string JobName { get; }
            public List<BackupLogEntry> BackupLogEntries { get; }

            public LogEntry(string jobName)
            {
                JobName = jobName;
                BackupLogEntries = new List<BackupLogEntry>();
            }
        }

        public class BackupFileInfo
        {
            public string FileName { get; set; }
            public long SizeInBytes { get; set; }
            public DateTime SourceTimestamp { get; set; }
            public DateTime TransferTimestamp { get; set; }
            public string SourcePath { get; set; }
            public string DestinationPath { get; set; }
            public double TransferTimeInMilliseconds { get; set; }
        }

        public class BackupLogEntry
        {
            public string JobName { get; set; }
            public string SourceDirectory { get; set; }
            public string DestinationDirectory { get; set; }
            public string Result { get; set; }
            public List<BackupFileInfo> BackupFilesInfo { get; set; }
        }

    }

    
}