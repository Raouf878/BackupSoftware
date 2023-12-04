using BackupSoftware.ViewModel;

namespace BackupSoftware.View
{
    class Views
    {
        public BackupJob MsVM { get; set; }

        public Views(BackupJob s)
        {
            MsVM = s;
        }
    }
}