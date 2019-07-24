using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Objectives
{
    public class DetailsModel : PageModel
    {
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Trello Objective { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var objectiveID = id ?? default(int);

            Objective = await _context.GetObjectiveAsync(objectiveID);

            if (Objective == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
