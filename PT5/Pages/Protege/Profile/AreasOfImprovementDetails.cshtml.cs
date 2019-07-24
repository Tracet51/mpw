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
    public class AreasOfImprovementDetailsModel : PageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public AreasOfImprovementDetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Models
        public AreasOfImprovement AreasOfImprovement { get; set; }
        #endregion

        #region Handlers

        /// <summary>
        /// Gets the details for the specific areas of improvement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            //checks to see if id passed is null
            if (id == null)
            {
                return NotFound();
            }

            //Queries the database for AreasOfImprovement based on the id passed in
            AreasOfImprovement = await _context.AreasOfImprovement.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if specified areas of improvement is null
            if (AreasOfImprovement == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion
    }
}
