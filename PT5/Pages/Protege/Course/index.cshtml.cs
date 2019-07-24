
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;


namespace MPW.Pages.Protege.Course
{
    public class indexModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public indexModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Data.Course Course { get; set; }

        public int EventsToDisplay => Course.Events.Count;
        public int SessionsToDisplay => Course.Sessions.Count;
        public int ResoucesToDisplay => Course.Resources.Count;
        public int ObjectiveToDisplay => Course.Objectives.Count;
        #endregion

        /// <summary>
        /// Shows the course data specified for what course the user is in and based off of
        /// the pairing.
        /// </summary>
        /// <param name="courseID"></param>
        #region DataHandlers
        public async Task<IActionResult> OnGetAsync(int courseID)
        {
            //gets the course ID from the database
            Course = await _context.GetCourseAsync(courseID);

            //Gets the Protege profile by passing in their username to the GetProtegeAsync
            //function in the applicationDbContext
            var appUser = await _context.GetProtegeAsync(Username);

            //Sets the appuser as a protege
            var protege = appUser.Protege;

            //checks to see if the course exists or if the protege exists
            //if not then returns message saying not able to find course id for that specified user
            if (Course == null || protege == null)
            {
                return NotFound($"Unable to course user with ID '{courseID}' for user {Username}");
            }

            //checks to see if the protege ID for the user matches the one associated with course
            //if not then returns error message telling the user they cannot find the user id for that
            //course
            else if (Course.Pair.ProtegeID != protege.ID)
            {
                return NotFound($"Unable to course user with ID '{courseID}' for user {Username}");
            }

            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnGetDownloadAsync(int id)
        {
            var resource = await _context.Resource.Include(m => m.Documents).Where(m => m.ResourceID == id).FirstOrDefaultAsync();
            var resourceDocument = resource.Documents.FirstOrDefault();
            var document = await _context.GetDocumentAsync(Convert.ToInt32(resourceDocument.DocumentID));

            return File(document.File, document.FileType, document.Name);
        }
        #endregion
    }
}