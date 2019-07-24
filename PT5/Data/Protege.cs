﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class Protege
    {
        public int ID { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string About { get; set; }
        public Address Address { get; set; }
        public IList<Certificate> Certificates { get; set; }

        [Display(Name = "Areas of Improvement")]
        public IList<AreasOfImprovement> AreasOfImprovement { get; set; }
        public IList<Pair> Pairs { get; set; }
    }
}
