using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class Mentor
    {
        public int ID { get; set; }

       
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string About { get; set; }
        public Address Address { get; set; }
        public IList<Certificate> Certificates { get; set; }
        public IList<StrategicDomain> StrategicDomains { get; set; }
        public IList<Pair> Pairs { get; set; }
    }
}
