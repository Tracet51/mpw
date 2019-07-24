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
    public class StrategicDomainDetailsModel : PageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public StrategicDomainDetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public StrategicDomain StrategicDomain { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details for the strategic domain
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

            //Grabs the strategic domain based off the id pased in
            StrategicDomain = await _context.StrategicDomains.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if the strategic domain queried is null
            if (StrategicDomain == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion
    }
}
