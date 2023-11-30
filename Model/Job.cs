using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSoftware.Model
{
    class Job
    {
        private string Name;
        private string Destination;
        private string Source;
        private string Type;

        public Job(string _Name, string _Destination, string _Source, string _Type)
        {
            Name = _Name;
            Destination = _Destination;
            Source = _Source;
            Type = _Type;
        }
        public String NAME { get => Name; set => Name = value; }
        public String DESTINATION { get => Destination; set => Destination = value; }
        public String SOURCE { get => Source; set => Source = value; }
        public String TYPE { get => Type; set => Type = value; }


    }
}
