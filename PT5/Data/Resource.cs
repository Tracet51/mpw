using System;
using System.Collections.Generic;

namespace MPW.Data
{
    public class Resource
    {
        public int ResourceID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime DateAdded { get; set; }
        public string Link { get; set; }

        #region Navigational
        public int CourseID { get; set; }
        public Course Course { get; set; }

        public IList<Document> Documents { get; set; }
        #endregion
    }
}