using System;

namespace MPW.Data
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        #region Navigational
        public int AssignmentID { get; set; }
        public Assignment Assignment { get; set; }

        public int MentorID { get; set; }
        public Mentor mentor { get; set; }
        #endregion
    }
}