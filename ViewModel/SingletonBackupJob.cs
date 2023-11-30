using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using BackupSoftware.Model;
using Microsoft.VisualBasic.FileIO;

namespace BackupSoftware.ViewModel
{
    class SingletonBackupJob
    {
        private Job jb;
        private List<Job> Jobs = new List<Job>();

        public SingletonBackupJob(Job jb)
        {
            this.jb = jb;
        }

        public String RunBackupJob()
        {
            CopyFiles(jb.SOURCE, jb.DESTINATION);
            return null;
        }

        private void CopyFiles(string sourcePath, string destinationPath)
        {
            
            System.IO.File.Copy(sourcePath, destinationPath, true);
            System.IO.File.SetCreationTime(destinationPath, System.IO.File.GetCreationTime(sourcePath));
            System.IO.File.SetLastAccessTime(destinationPath, System.IO.File.GetLastAccessTime(sourcePath));
            System.IO.File.SetLastWriteTime(destinationPath, System.IO.File.GetLastWriteTime(sourcePath));
        }

        public SingletonBackupJob GetBackupJonInstance()
        {
            return new SingletonBackupJob(jb);
        }
    }
}