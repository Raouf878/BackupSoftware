using System;

namespace BackupSoftware.Model
{
    class Job
    {
        public string Name { get; private set; }
        public string Destination { get; private set; }
        public string Source { get; private set; }
        public string Type { get; private set; }

        public Job(string name, string destination, string source, string type)
        {
            Name = name;
            Destination = destination;
            Source = source;
            Type = type;
        }
    }
}