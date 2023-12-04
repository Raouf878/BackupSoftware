using System;
using BackupSoftware.Model;
using BackupSoftware.ViewModel;

namespace BackupSoftware
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of jobs you want to create:");
            int numberOfJobs;

            while (!int.TryParse(Console.ReadLine(), out numberOfJobs) || numberOfJobs <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
            }

            BackupManager backupManager = new BackupManager();

            for (int i = 1; i <= numberOfJobs; i++)
            {
                Console.WriteLine($"Enter the name for Job{i}:");
                string jobName = Console.ReadLine();

                Console.WriteLine($"Enter the source path for Job{i}:");
                string sourcePath = Console.ReadLine();

                Console.WriteLine($"Enter the destination path for Job{i}:");
                string destinationPath = Console.ReadLine();

                Console.WriteLine($"Enter the type of backup for Job{i} (Differential or Full):");
                string backupType = Console.ReadLine();

                while (string.IsNullOrEmpty(backupType) || (!backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) && !backupType.Equals("Full", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Invalid input. Please enter 'Differential' or 'Full'.");
                    backupType = Console.ReadLine();
                }

                BackupSoftware.Model.Job job = new BackupSoftware.Model.Job(jobName, sourcePath, destinationPath, backupType);
                backupManager.AddBackupJob(job);

                IBackupStrategy strategy = backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase)
                    ? new DifferentialBackup()
                    : (IBackupStrategy)new FullBackup();

                // Set the strategy for the last added job
                backupManager.GetLastBackupJob().SetBackupStrategy(strategy);
            }

            // Now that all jobs are added, run backup jobs
            backupManager.RunBackupJobs();

            Console.WriteLine("All jobs created and executed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}