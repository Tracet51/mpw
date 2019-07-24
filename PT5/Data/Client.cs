using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class Client
    {
        public int ID { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string About { get; set; }
        public Address Address { get; set; }
        public string CompanySize { get; set; }
        public IList<Pair> Pairs { get; set; }
    }
}
