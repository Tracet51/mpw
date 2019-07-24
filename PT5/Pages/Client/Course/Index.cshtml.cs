
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;


namespace MPW.Pages.Client.Course
{
    public class IndexModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public IndexModel(MPW.Data.ApplicationDbContext context)
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
        /// <returns></returns>
        #region Handlers
        public async Task<IActionResult> OnGetAsync(int courseID)
        {
            //gets the course from the database based off of the course ID.
            Course = await _context.GetCourseAsync(courseID);

            //Gets the Client profile by passing in their username to the GetClientAsync
            //function in the applicationDbContext
            var appUser = await _context.GetClientAsync(Username);

            //Sets the appuser as a Client
            var client = appUser.Client;

            //checks to see if the course exists or if the course exists
            //if not then returns message saying not able to find course id for that specified user
            if (Course == null || client == null)
            {
                return NotFound($"Unable to course user with ID '{courseID}' for user {Username}");
            }

            //checks to see if the Client ID for the user matches the one associated with course
            //if not then returns error message telling the user they cannot find the user id for that
            //course
            else if (Course.Pair.ClientID != client.ID)
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