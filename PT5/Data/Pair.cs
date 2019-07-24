using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class Pair
    {
        public int PairID { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Join Code")]
        public string JoinCode { get; set; }

        #region Navigational
        public int MentorID { get; set; }
        public Mentor Mentor { get; set; }

        public int ProtegeID { get; set; }
        public Protege Protege { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }

        public IList<Course> Courses { get; set; }
        #endregion

    }
}
