using System;

namespace MPW.Data
{
    public class Note
    {
        public int NoteID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }


        #region Navigational
        public int SessionID { get; set; }
        public Session Session { get; set; }

        public int AgendaID { get; set; }
        public Agenda Agenda { get; set; }

        public int ObjectiveID { get; set; }
        public Trello Objective { get; set; }
        #endregion
    }
}