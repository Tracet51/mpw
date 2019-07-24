using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;

namespace MPW.Pages.Events
{
    public class CreateModel : MPWPageModel
    {
        #region Cosntructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CreateModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        #endregion

        #region Model
        [BindProperty]
        public Event Event { get; set; }

        [TempData]
        public int CourseID { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? courseID)
        {
            if (courseID == null)
            {
                return NotFound();
            }
            
            CourseID = courseID.GetValueOrDefault();
            var course = await _context.GetCourseAsync(CourseID);


            await CheckRole(_context, _userManager, course.Pair.JoinCode);
            if (!IsMentor)
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

            var course = await _context.GetCourseAsync(CourseID);
            Event.Course = course;
            Event.CourseID = CourseID;

            var success = await _context.AddEventAsync(Event);

            if (!success)
            {
                return RedirectToPage("/Error");
            }

            var mentor = await _context.GetMentorByIDAsync(course.Pair.MentorID);
            var protege = await _context.GetProtegeByIDAsync(course.Pair.ProtegeID);
            var client = await _context.GetClientByIDAsync(course.Pair.ClientID);

            var subject = $"{mentor?.AppUser?.Email} created an Event {Event.EventName}";
            var message = "<h1>Assignment Uploaded</h1>" +
                $"<p>The Event <strong>{Event.EventName}</strong> was created by <strong>{mentor?.AppUser?.Email}</strong> and starts at <strong>{Event.StartDate}</strong></p>";

            if (mentor != null)
            {
                await _emailSender.SendEmailAsync(mentor.AppUser.Email, subject, message);
            }

            if (protege != null)
            {
                await _emailSender.SendEmailAsync(protege.AppUser.Email, subject, message);
            }

            if (client != null)
            {
                await _emailSender.SendEmailAsync(client.AppUser.Email, subject, message);
            }

            return RedirectToPage("/Mentor/Course/Index", new { id = course.CourseID });
        }
        #endregion
    }
}