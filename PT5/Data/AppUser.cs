using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string CompanyName { get; set; }

        [PersonalData]
        public string Field { get; set; }

        [PersonalData]
        public DateTime DateCreate { get; set; }

        [PersonalData]
        public string AlternatePhoneNumber { get; set; }

        public virtual Mentor Mentor { get; set; }

        public virtual Protege Protege { get; set; }

        public virtual Client Client { get; set; }

    }
}
