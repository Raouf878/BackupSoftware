using BackupSoftware.ViewModel;
using System;

namespace BackupSoftware.Model
{
    public sealed class SingletonBackupJob
    {
        private BackupJob backupJob;

        public SingletonBackupJob() { }

        public void CaculateBackupsNum()
        {
            int backupNum=0;
            if (backupJob != null)
            {
                backupNum += 1;
            }
           
        }
    }
}
