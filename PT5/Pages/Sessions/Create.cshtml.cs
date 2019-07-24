using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;

namespace MPW.Pages.Sessions
{
    public class CreateModel : MPWPageModel
    {
        #region Constructor
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
        public Session Session { get; set; }

        [BindProperty]
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
                return new ForbidResult();
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
            if (course == null)
            {
                return RedirectToPage("/Error");
            }

            Session.CourseID = CourseID;

            var success = await _context.CreateSessionAsync(Session);

            if (success)
            {
                var mentor = await _context.GetMentorByIDAsync(course.Pair.MentorID);
                var client = await _context.GetClientByIDAsync(course.Pair.ClientID);
                var protege = await _context.GetClientByIDAsync(course.Pair.ProtegeID);

                var subject = $"New Session Created for {course.CourseName}";
                var message = "<h1>Session Created</h1>" +
                $"<p>The Session <strong>{Session.Name}</strong> was created by <strong>{mentor?.AppUser?.Email}</strong> and starts at <strong>{Session.StartDate}</strong></p>";

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

                return RedirectToPage("./Details", new { id = Session.SessionID });
            }
            else
            {
                return RedirectToPage("/Error");
            }

            
        }
        #endregion

    }
}