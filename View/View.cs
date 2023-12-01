using BackupSoftware.ViewModel;

namespace BackupSoftware.View
{
    class Views
    {
        public SingletonBackupJob MsVM { get; set; }

        public Views(SingletonBackupJob s)
        {
            MsVM = s;
        }
    }
}