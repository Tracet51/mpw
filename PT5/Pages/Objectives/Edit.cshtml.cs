using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Objectives
{
    public class EditModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public EditModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Model
        [BindProperty]
        public Trello Objective { get; set; }

        [BindProperty]
        public bool Completed { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int id)
        {

            Objective = await _context.GetObjectiveAsync(id);

            if (Objective == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Completed)
            {
                Objective.DateCompleted = DateTime.Now;
            }

            var success = await _context.UpdateObjectiveAsync(Objective);

            if (success)
            {
                return RedirectToPage("./List", new { Objective.CourseID });
            }
            else
            {
                return Page();
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var objectiveID = Objective.ID;
            var courseID = Objective.CourseID;

            var success = await _context.DeleteObjective(objectiveID);

            if (!success)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./List", new { courseID });
        }
        #endregion

    }
}
