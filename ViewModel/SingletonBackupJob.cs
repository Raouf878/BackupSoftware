using System;

namespace BackupSoftware.Model
{
    public sealed class SingletonBackupJob
    {
        private static SingletonBackupJob instance = null;
        private Model.Job backupJob;



        public static SingletonBackupJob GetBackupJobInstance(Model.Job job)
        {
            if (instance == null)
            {
                instance = new SingletonBackupJob();
            }

            instance.backupJob = job;
            return instance;
        }

        public Model.Job GetBackupJob()
        {
            if (backupJob == null)
            {
                throw new InvalidOperationException("Backup job has not been set.");
            }
            return backupJob;
        }
    }
}
