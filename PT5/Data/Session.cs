using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{
    public class Session
    {
        public int SessionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        #region Navigational
        public IList<Assignment> Assignments { get; set; }
        public IList<Event> Events { get; set; }
        public IList<Document> Documents { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public int AgendaID { get; set; }
        public Agenda Agenda { get; set; }
        #endregion

    }
}