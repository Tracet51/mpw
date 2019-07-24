using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPW.Data
{
    public class Agenda
    {
        public int AgendaID { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        #region Navigational

        public int DocumentID { get; set; }
        public Document Document { get; set; }

        public int SessionID { get; set; }
        public Session Session { get; set; }
        #endregion
    }
}