using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Profile
{
    public class DetailModel : MPWPageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Data.Protege Protege { get; set; }
        #endregion

        /// <summary>
        /// Show the user the details of their protege profile.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            //Querys the database to find the protege account linked to the user
            //and gets the proteges certificates, areasofimporvement, and address
            Protege = await _context.Protege
                .Include(m => m.AppUser)
                .Include(m => m.Certificates)
                .Include(m => m.AreasOfImprovement)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.AppUser.UserName == Username);

            //checks to see if protege exists if not then returns not found
            if (Protege == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}