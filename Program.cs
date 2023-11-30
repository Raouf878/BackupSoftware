using BackupSoftware.View;
using System;
using BackupSoftware.Model;
using BackupSoftware.ViewModel;
using BackupSoftware.View;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using BackupSoftware.Model;
using BackupSoftware.View;
using BackupSoftware.ViewModel;

namespace BackupSoftware
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter source");
            string s = Console.ReadLine();
            Console.WriteLine("Please enter Destination");
            string d = Console.ReadLine();
            string n = Console.ReadLine();
            string t = Console.ReadLine();
            Job jb = new Job(n, s, d, t);
            SingletonBackupJob Backvm = new SingletonBackupJob(jb);
            Views v = new Views(Backvm);

            Console.WriteLine($"Your transformed sentence:{v.MsVM.RunBackupJob()}");
        }
    }
}
