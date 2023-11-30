using BackupSoftware.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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