using System;

namespace MPW.Data
{
    public class Comment
    {
        public int CommentID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        #region Navigational
        public int AssignmentID { get; set; }
        public Assignment Assignment { get; set; }

        public int ProtegeID { get; set; }
        public Protege Protege { get; set; }
        #endregion
    }
}