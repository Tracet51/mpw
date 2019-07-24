using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MPW.Pages.PublicProfiles
{
    public class ClientPublicProfileModel : PageModel
    {
        #region Constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public ClientPublicProfileModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Data.Client Client { get; set; }
        #endregion

        /// <summary>
        /// Gets the Clients Details for their account.
        /// </summary>
        #region Handlers
        public async Task<IActionResult> OnGetAsync(string clientUsername)
        {
            Client = await _context.GetPublicClientAsync(clientUsername);

            //Checks to see if client exists if not return page not found.
            if (Client == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion
    }
}
