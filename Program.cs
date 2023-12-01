using BackupSoftware.View;
using System;
using BackupSoftware.Model;
using BackupSoftware.ViewModel;
using BackupSoftware.View;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace BackupSoftware
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter source:");
            string source = Console.ReadLine();

            Console.WriteLine("Please enter destination:");
            string destination = Console.ReadLine();

            Console.WriteLine("Please enter job name:");
            string jobName = Console.ReadLine();

            Console.WriteLine("Please enter job type:");
            string jobType = Console.ReadLine();

            // Create a Job with user-entered values
            Job jb = new Job(source, destination, jobName, jobType);

            // Create a SingletonBackupJob with the Job
            SingletonBackupJob backupJob = new SingletonBackupJob(jb);

            // Assuming that Views class and MsVM property are properly defined
            Views v = new Views(backupJob);
            Console.WriteLine($"Your transformed sentence: {v.MsVM.RunBackupJob()}");
        }
    }
}