using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Events
{
    public class DetailsModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DetailsModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        [BindProperty]
        public Event Event { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventID = id.GetValueOrDefault();

            Event = await _context.GetEventAsync(eventID);

            if (Event == null)
            {
                return NotFound();
            }

            await CheckRole(_context, _userManager, Event.Course.Pair.JoinCode);


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var course = await _context.GetCourseAsync(Event.CourseID);
            await CheckRole(_context, _userManager, course.Pair.JoinCode);

            var success = await _context.DeleteEventAsync(Event.EventID);
            if (!success || !IsMentor)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("/Mentor/Course/Index", new { id = Event.CourseID });
        }
        #endregion
    }
}
