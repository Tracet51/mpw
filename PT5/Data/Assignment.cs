using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPW.Data
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime DateCompleted { get; set; }

        #region Navigational
        public IList<Comment> Comments { get; set; }
        public IList<Feedback> Feedback { get; set; }
        public IList<Document> Documents { get; set; }

        public int SessionID { get; set; }
        public Session Session { get; set; }
        #endregion
    }
}