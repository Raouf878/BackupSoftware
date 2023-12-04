using System;
using BackupSoftware.View;
using BackupSoftware.ViewModel;

namespace BackupSoftware
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Please enter job name:");
            string jobName = Console.ReadLine();

            Console.WriteLine("Please enter source:");
            string source = Console.ReadLine();

            Console.WriteLine("Please enter destination:");
            string destination = Console.ReadLine();

            Console.WriteLine("Please enter job type (1 for Differential, 2 for Full):");
            string jobTypeInput = Console.ReadLine();


            Model.Job jobInstance = new Model.Job(jobName, source, destination, jobTypeInput);

            SingletonBackupJob backupJobInstance = SingletonBackupJob.GetBackupJobInstance(jobInstance);

            
            if (jobTypeInput == "1")
            {
                backupJobInstance.SetBackupStrategy(new DifferentialBackup());
            }
            else if (jobTypeInput == "2")
            {
                backupJobInstance.SetBackupStrategy(new FullBackup());
            }
            else
            {
                Console.WriteLine("Invalid job type input.");
                return;
            }

            string result = backupJobInstance.RunBackupJob();

            
            Views v = new Views(backupJobInstance);
            Console.WriteLine($"Your transformed sentence: {v.MsVM.RunBackupJob()}");

           
            Console.ReadLine();
        }
    }
}