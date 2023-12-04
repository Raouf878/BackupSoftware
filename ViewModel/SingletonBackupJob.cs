using Newtonsoft.Json;
using System;
using System.IO;

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

    class SingletonBackupJob
    {
        private static SingletonBackupJob instance;
        private static readonly object lockObject = new object();
        private Model.Job jobInstance;
        private LogFile logFile;
        private IBackupStrategy backupStrategy;


        private SingletonBackupJob() { }
        public SingletonBackupJob(Model.Job jobInstance)
        {
            this.jobInstance = jobInstance;
            this.logFile = new LogFile(this, "C:\\Users\\LENOVO\\Desktop\\ali C_C++\\ali\\LogFile\\Logfile.txt");
        }

        public static SingletonBackupJob GetBackupJobInstance(Model.Job jobInstance)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SingletonBackupJob(jobInstance);
                    }
                }
            }
            return instance;
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

                string result = backupStrategy.Backup(jobInstance.Source, jobInstance.Destination);
                logFile.WriteLogFile(jobInstance.Name, jobInstance.Source, jobInstance.Destination, result);
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
        private SingletonBackupJob backupJob;
        private string logFilePath;

        public LogFile(SingletonBackupJob backupJob, string logFilePath)
        {
            this.backupJob = backupJob;
            this.logFilePath = logFilePath;
        }

        public void WriteLogFile(string jobName, string source, string destination, string result)
        {
            try
            {
                List<BackupFileInfo> backupFilesInfo = new List<BackupFileInfo>();

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

                
                string jsonLogEntry = JsonConvert.SerializeObject(logEntry, Formatting.Indented);

                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

                
                if (!File.Exists(logFilePath))
                {
                    using (StreamWriter createLogFile = File.CreateText(logFilePath))
                    {
                        createLogFile.WriteLine("Log file created at: " + DateTime.Now);
                    }
                }
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

        class Job
        {
            public string Name { get; set; }
            public string Source { get; set; }
            public string Destination { get; set; }
        }
    }
}
