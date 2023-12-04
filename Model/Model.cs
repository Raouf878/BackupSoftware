namespace BackupSoftware.Model
{
    public class Job
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string JobType { get; set; }

        public Job(string name, string source, string destination, string jobType)
        {
            Name = name;
            Source = source;
            Destination = destination;
            JobType = jobType;
        }
    }
}