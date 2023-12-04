using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSoftware.ViewModel
{
    interface IBackupStrategy
    {
        string Backup(string source, string destination);
    }
}

