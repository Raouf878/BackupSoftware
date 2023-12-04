using BackupSoftware.Model;
using BackupSoftware.View;
using BackupSoftware.ViewModel;

namespace BackupSoftware
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(Properties.Resources.Lang);

            string codeline = Console.ReadLine();
            Language bc = new Language(codeline);
            LanguageManager bcm = new LanguageManager(bc);
            ChosedLanguage viewlan = new ChosedLanguage(bcm);
            viewlan.chosingLanguage.translate();
            Console.WriteLine(Properties.Resources.NumJ);

            int numberOfJobs;

            while (!int.TryParse(Console.ReadLine(), out numberOfJobs) || numberOfJobs <= 0)
            {
                viewlan.chosingLanguage.translate();
                Console.WriteLine(Properties.Resources.InputError);
            }

            BackupManager backupManager = new BackupManager();

            for (int i = 1; i <= numberOfJobs; i++)
            {
                viewlan.chosingLanguage.translate();

                Console.WriteLine(Properties.Resources.Job + $" {i}:");
                string jobName = Console.ReadLine();

                Console.WriteLine(Properties.Resources.Sou + $" {i}:");
                string sourcePath = Console.ReadLine();

                Console.WriteLine(Properties.Resources.Path + $"{i}:");
                string destinationPath = Console.ReadLine();

                Console.WriteLine(Properties.Resources.TypeBack + $"{i}" + Properties.Resources.TypeF + " / " + Properties.Resources.TypeD + " :");
                string backupType = Console.ReadLine();

                while (string.IsNullOrEmpty(backupType) || (!backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) && !backupType.Equals("Full", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine(Properties.Resources.InputError);
                    backupType = Console.ReadLine();
                }

                BackupSoftware.Model.Job job = new BackupSoftware.Model.Job(jobName, sourcePath, destinationPath, backupType);
                backupManager.AddBackupJob(job);

                IBackupStrategy strategy = backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) ? new DifferentialBackup()
                    : (IBackupStrategy)new FullBackup();

                // Set the strategy for the last added job
                backupManager.GetLastBackupJob().SetBackupStrategy(strategy);
            }

            // Now that all jobs are added, run backup jobs
            backupManager.RunBackupJobs();

            Console.WriteLine(Properties.Resources.Exit);
            Console.ReadKey();
        }
    }
}