using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Resources
{
    public class EditModel : PageModel
    {
        private readonly MPW.Data.ApplicationDbContext _context;

        public EditModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Resource Resource { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Resource = await _context.Resource
                .Include(r => r.Course).FirstOrDefaultAsync(m => m.ResourceID == id);

            if (Resource == null)
            {
                return NotFound();
            }
           ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(Resource.ResourceID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ResourceExists(int id)
        {
            return _context.Resource.Any(e => e.ResourceID == id);
        }
    }
}
