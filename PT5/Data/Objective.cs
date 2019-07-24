using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPW.Data
{
    public class Trello
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        public DateTime DateCompleted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Estimated Completion Date")]
        public DateTime EstimatedDateCompleted { get; set; }

        #region Navigational
        public int CourseID { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}