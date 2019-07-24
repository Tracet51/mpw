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
    public class StrategicDomainDeleteModel : PageModel
    {
        #region Constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public StrategicDomainDeleteModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Models
        [BindProperty]
        public StrategicDomain StrategicDomain { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details and the strategic Domain
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //checks to see if id is null
            if (id == null)
            {
                return NotFound();
            }

            //Gets the strategic domain based off of the id passed in
            StrategicDomain = await _context.StrategicDomains.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see strategic domain queried is null
            if (StrategicDomain == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Removes the strategic domain
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //checks to see if id passed is null
            if (id == null)
            {
                return NotFound();
            }

            //Gets the strategic domain based off of the id passed in
            StrategicDomain = await _context.StrategicDomains.FindAsync(id);

            //checks to see if strategic domain queried is not null and if not then remove the strategic domain
            if (StrategicDomain != null)
            {
                _context.StrategicDomains.Remove(StrategicDomain);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Details");
        }
        #endregion
    }
}
