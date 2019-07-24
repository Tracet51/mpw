using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Pairing
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
            [Display(Name = "Course Name")]
            public string CourseName { get; set; }

            [Display(Name = "Client Email")]
            [DataType(DataType.EmailAddress)]
            public string ClientUserName { get; set; }

            [Display(Name = "Mentor Email")]
            [DataType(DataType.EmailAddress)]
            public string MentorUserName { get; set; }

            [Display(Name = "Join Code")]
            public string JoinCode { get; set; }

            [Display(Name = "Date Created")]
            public string DateCreated { get; set; }

            [Display(Name = "Estimated Completion Date")]
            public string EstimatedCompletionDate { get; set; }

            public int PairID { get; set; }
        }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the Proteges Pairs and shows relevant data pertaining to the pairing.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            //Gets the protege account by passing the username to GetProtegeAsync function in the DAL
            var protege = await _context.GetProtegeAsync(Username);

            //Checks to see if user is actually a protege if not then returns error
            if (protege?.Protege == null)
            {
                return Redirect("/Error");
            }

            //Gets the pairs relating to the protege based off of the protege ID being passed
            //into the GetPairsForProtege function in the ApplicationDbContext file
            var pairs = await _context.GetPairsForProtege(protege.Protege.ID);

            //stores the displaymodel as a list and puts them in display
            this.Display = new List<DisplayModel>();
            
            foreach (var pair in pairs)
            {
                //updates the information of the displaymodel by what is stored for the pairs
                var display = new DisplayModel
                {
                    MentorUserName = pair.Mentor?.AppUser?.UserName ?? "",
                    DateCreated = pair.DateCreated.ToShortDateString(),
                    JoinCode = pair.JoinCode,
                    PairID = pair.PairID,
                    ClientUserName = pair.Client?.AppUser?.UserName ?? ""
                };

                this.Display.Add(display);
            }

            return Page();
        }
        #endregion

    }
}
