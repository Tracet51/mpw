using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Client.Pairing
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

            [Display(Name = "Mentor Email")]
            [DataType(DataType.EmailAddress)]
            public string MentorUserName { get; set; }

            [Display(Name = "Join Code")]
            public string JoinCode { get; set; }

            [Display(Name = "Date Created")]
            public string DateCreated { get; set; }


            public int PairID { get; set; }
        }
        #endregion

        /// <summary>
        /// Gets the pairings associated with the clients and shows the details about them
        /// in the view.
        /// </summary>
        #region Handlers
        public async Task<IActionResult> OnGetAsync()
        {
            //Gets the client account by passing the username to GetClientAsync function in the DAL
            var client = await _context.GetClientAsync(this.Username);

            //Checks to see if user is actually a client if not then returns error
            if (client?.Client == null)
            {
                return Redirect("/Error");
            }

            //stores the displaymodel as a list and puts them in display
            this.Display = new List<DisplayModel>();

            //Gets the pairs relating to the client based off of the client ID being passed
            //into the GetClientForProtege function in the ApplicationDbContext file
            var pairs = await _context.GetPairsForClient(client.Client.ID);
            foreach (var pair in pairs)
            {
                //updates the information of the displaymodel by what is stored for the pairs
                var display = new DisplayModel
                {
                    MentorUserName = pair.Mentor?.AppUser?.UserName ?? "",
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
