using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Assignments
{
    public class EditModel : MPWPageModel
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
        public Assignment Assignment { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var assignmentID = id.GetValueOrDefault();
            Assignment = await _context.GetAssignmentAsync(assignmentID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "There was an error updating your page. Please try again";
                return Page();
            }

            var success = await _context.UpdateAssignmentAsync(Assignment);

            if (success)
            {
                return RedirectToPage("./Details", new { id = Assignment.AssignmentID });
            }

            return RedirectToPage("/Error");
        }
        #endregion
    }
}
