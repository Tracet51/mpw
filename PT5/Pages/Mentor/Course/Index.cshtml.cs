using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Course
{
    public class IndexModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        public Data.Course Course { get; set; }

        public int EventsToDisplay => Course.Events.Count;
        public int SessionsToDisplay => Course.Sessions.Count;
        public int ResoucesToDisplay => Course.Resources.Count;
        public int ObjectiveToDisplay => Course.Objectives.Count;
        #endregion
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseID = id.GetValueOrDefault();

            Course = await _context.GetCourseAsync(courseID);

            var appUser = await _context.GetMentorAsync(Username);
            var mentor = appUser.Mentor;

            await CheckRole(_context, _userManager, Course.Pair.JoinCode);

            if (Course == null || mentor == null)
            {
                return NotFound($"Unable to course user with ID '{courseID}' for user {Username}" );
            }
            else if (Course.Pair.MentorID != mentor.ID)
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
    }
}
