using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Resources
{
    public class CreateLinksModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public CreateLinksModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Model

        [TempData]
        public int CourseID { get; set; }

        [BindProperty]
        public Resource Resource { get; set; }


        #endregion

        #region Handlers
        /// <summary>
        /// The on Get method checks to see if the courseID is not null and gets the course.
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public IActionResult OnGet(int? courseID)
        {
            //Checks to see if CourseID is null
            if (courseID == null)
            {
                return NotFound();
            }

            //Sets CourseID from the courseID passed to the model
            CourseID = courseID.GetValueOrDefault();
            return Page();

        }

        /// <summary>
        /// The onPostAsync method checks to see if the model state is valid and creates a new resource with a link
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //checks to see if the modelstate is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Sets the resource courseid for the resource being created to the CourseID above
            this.Resource.CourseID = CourseID;

            //Sets the resource Date Added for the resource being created to the DateTime right now
            this.Resource.DateAdded = DateTime.Now;

            //Sets the resource Type for the resource being created to link
            this.Resource.Type = "Link";

            if(this.Resource.Link.Substring(0,4) != "http")
            {
                string Link = "https://" + this.Resource.Link;
                this.Resource.Link = Link;
            }

            //Adds the resource to the database
            var success = await _context.CreateResourceAsync(this.Resource);

            if (success)
            {
                //Redirects the mentor to the specified course
                return RedirectToPage("../Mentor/Course/Index", new { id = CourseID });
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        #endregion
    }
}