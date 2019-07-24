using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Client.Profile
{
    public class DetailModel : MPWPageModel
    {
        #region Constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailModel(MPW.Data.ApplicationDbContext context)
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
        public async Task<IActionResult> OnGetAsync()
        {
            //Gets the Client from the database plus the appuser and their address
            //based off of their username.
            Client = await _context.Client
                .Include(m => m.AppUser)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == Username);

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