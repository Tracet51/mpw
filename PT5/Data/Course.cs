using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPW.Data
{
    public class Course
    {
        public int CourseID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseName { get; set; }

        #region Navigational
        public int PairID { get; set; }
        public Pair Pair { get; set; }

        public IList<Trello> Objectives { get; set; }
        public IList<Session> Sessions { get; set; }
        public IList<Event> Events { get; set; } 
        public IList<Resource> Resources { get; set; }
        #endregion
    }
}