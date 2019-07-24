using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MPW.Pages.PublicProfiles
{
    public class ProtegePublicProfileModel : PageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public ProtegePublicProfileModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region DataModel
        public Data.Protege Protege { get; set; }
        #endregion

        /// <summary>
        /// Show the user the details of their protege profile.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string protegeUsername)
        {
            //Querys the database to find the protege account linked to the user
            //and gets the proteges certificates, areasofimporvement, and address
            Protege = await _context.GetPublicProtegeAsync(protegeUsername);

            //checks to see if protege exists if not then returns not found
            if (Protege == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
