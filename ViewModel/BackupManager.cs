using BackupSoftware.Model;
using System;
using System.Collections.Generic;

namespace BackupSoftware.ViewModel
{
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
    }
}



