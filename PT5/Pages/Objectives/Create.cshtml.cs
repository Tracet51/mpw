using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;

namespace MPW.Pages.Objectives
{
    public class CreateModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public CreateModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion
        
        #region Model
        [BindProperty]
        public Trello Objective { get; set; }

        [BindProperty]
        public int CourseID { get; set; }



        #endregion

        #region Handlers
        public IActionResult OnGet(int courseID)
        {
            this.CourseID = courseID;
            return Page();

        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                this.Error = "Something went wrong. Try Again.";
                return Page();
            }

            var courseID = this.CourseID;

            this.Objective.CourseID = courseID;
            this.Objective.DateAdded = DateTime.Now;

            await _context.AddObjectiveAsync(this.Objective);

            return RedirectToPage("./List", new { courseID });
        }

        
        #endregion

    }
}