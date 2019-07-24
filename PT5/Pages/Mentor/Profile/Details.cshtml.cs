using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Profile
{
    public class DetailsModel : MPWPageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Data.Mentor Mentor { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details for mentor
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            //Gets all the data associated with the mentor based on the username
            Mentor = await _context.Mentor
                .Include(m => m.AppUser)
                .Include(m => m.Certificates)
                .Include(m => m.StrategicDomains)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == Username);

            //checks to see if mentor is null
            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion
    }
}
