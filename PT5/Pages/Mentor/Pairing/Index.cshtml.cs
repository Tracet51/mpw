using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Pairing
{
    public class IndexModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public IndexModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Model
        public IList<DisplayModel> Display { get; set; }

        public class DisplayModel
        {
            
            [Display(Name = "Protege Email")]
            [DataType(DataType.EmailAddress)]
            public string ProtegeUserName { get; set; }

            [Display(Name = "Client Email")]
            [DataType(DataType.EmailAddress)]
            public string ClientUserName { get; set; }

            [Display(Name = "Join Code")]
            public string JoinCode { get; set; }

            [Display(Name = "Date Created")]
            public string DateCreated { get; set; }

            [Display(Name = "Pair ID")]
            public int PairID { get; set; }
        }
        #endregion

        #region Handlers
       
        public async Task<IActionResult> OnGetAsync()
        {
            var mentor = await _context.GetMentorAsync(this.Username);
            if (mentor?.Mentor == null)
            {
                return Redirect("/Error");
            }

            var pairs = await _context.GetPairsForMentorAsync(mentor.Mentor.ID);

            this.Display = new List<DisplayModel>();
            foreach (var pair in pairs)
            {
                var display = new DisplayModel
                {
                    ClientUserName = pair.Client?.AppUser?.UserName ?? "",
                    DateCreated = pair.DateCreated.ToShortDateString(),
                    JoinCode = pair.JoinCode,
                    PairID = pair.PairID,
                    ProtegeUserName = pair.Protege?.AppUser?.UserName ?? ""
                };

                this.Display.Add(display);
            }

            return Page();
        }
        #endregion

    }
}
