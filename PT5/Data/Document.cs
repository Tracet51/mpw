using System;

namespace MPW.Data
{
    public class Document
    {
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileType { get; set; }
        public string Category { get; set; }
        public long FileSize { get; set; }
        public byte[] File { get; set; }


        #region Navigational
        public int AssignmentID { get; set; }
        public Assignment Assignment { get; set; }

        public int EventID { get; set; }
        public Event Event { get; set; }

        public int SessionID { get; set; }
        public Session Session { get; set; }

        public int ResourceID { get; set; }
        public Resource Resource { get; set; }
        #endregion
    }
}
