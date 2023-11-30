using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BackupSoftware.ViewModel
{
    class LogFile
    {
        private SingletonBackupJob MyLogFile;

        public LogFile(SingletonBackupJob myLogFile)
        {
            this.MyLogFile = myLogFile;
        }
    }
}