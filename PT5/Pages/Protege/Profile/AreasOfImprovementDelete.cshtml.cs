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
    public class AreasOfImprovementDeleteModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public AreasOfImprovementDeleteModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Models
        [BindProperty]
        public AreasOfImprovement AreasOfImprovement { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details for the areas of improvement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //checks to see id is null
            if (id == null)
            {
                return NotFound();
            }

            //Queries the database for areas of improvement based on the id passed in
            AreasOfImprovement = await _context.AreasOfImprovement.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if areas of improvement is null
            if (AreasOfImprovement == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Deletes the Specified areas of improvement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //checks to see if id is null
            if (id == null)
            {
                return NotFound();
            }

            //Queries the database for areas of improvement based on the id passed in
            AreasOfImprovement = await _context.AreasOfImprovement.FindAsync(id);

            //checks to see if areas of improvement is not null and if not then delete areas of improvment
            if (AreasOfImprovement != null)
            {
                _context.AreasOfImprovement.Remove(AreasOfImprovement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Detail");
        }
        #endregion
    }
}
