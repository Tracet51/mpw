using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPW.Data
{
    public class Event
    {
        public int EventID { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; }
        public string Type { get; set; }
        public bool Attended { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Event Start Time")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Event End Time")]
        public DateTime EndTime { get; set; }

        #region Navigational
        public IList<Document> Documents { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}