
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BackupSoftware.ViewModel
{
    public class LogFile
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
    }
    public class LogEntry
    { 
        public string JobName { get; }
        public List<BackupLogEntry> BackupLogEntries { get; }
        public LogEntry(string jobName) { JobName = jobName; BackupLogEntries = new List<BackupLogEntry>(); 
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


