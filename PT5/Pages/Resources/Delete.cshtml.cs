using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Resources
{
    public class DeleteModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public DeleteModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Model

        [BindProperty]
        public Resource Resource { get; set; }

        [BindProperty]
        public Document Document { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// The OnGetSync method takes in the id for the resource and finds the resource and the documents and course related to the resources.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //checks to see if the id passed is null
            if (id == null)
            {
                return NotFound();
            }

            //Uses the GetResourceAsync function from the applicationdbcontext file to query the database for the resource based off of the id passed in.
            Resource = await _context.Resource.FindAsync(id);

            //queries the database and grabs the first document that is related to the resource
            Document = await _context.Document
                .Where(d => d.Resource.ResourceID == id)
                .OrderByDescending(d => d.UploadDate)
                .FirstOrDefaultAsync();

            //checks to see if Resource is null
            if (Resource == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// The onPostSync method takes in the id for a resource and deletes the specified resource and any documents related to it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //checks to see if the id is null
            if (id == null)
            {
                return NotFound();
            }

            //Uses the GetResourceAsync function from the applicationdbcontext file to query the database for the resource based off of the id passed in.
            Resource = await _context.Resource.FindAsync(id);

            //Gets the course id related to the resource
            var CourseID = Resource.CourseID;

            //passes the resource ID to the Delete resource function in applicationDbContext file to delete the resource
            var success = await _context.DeleteResource(Resource.ResourceID);

            //checks to see if deletion was successful
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
