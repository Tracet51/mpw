using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Course
{
    public class CreateModel : MPWPageModel
    {

        #region Constructors
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public CreateModel(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        #endregion

        #region Model

        [BindProperty]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [BindProperty]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [BindProperty]
        [Display(Name = "Estimated End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [TempData]
        public string JoinCode { get; set; }

        #endregion

        #region Handlers

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JoinCode = id;
            return Page();
        
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mentor = await _context.GetMentorAsync(Username);
            var mentorID = mentor.Mentor.ID;
            var pair = await _context.GetPairAsync(mentorID, JoinCode);

            if (pair == null)
            {
                return NotFound();
            }

            var pairID = pair.PairID;


            var course = new Data.Course
            {
                CourseName = CourseName,
                PairID = pairID,
                StartDate = StartDate,
                EndDate = EndDate
            };

            var success = await _context.AddCourseAsync(course);

            if (!success)
            {
                return RedirectToPage("/Error");
            }

            var dbCourse = await _context.GetCourseAsync(course);

            var courseID = dbCourse.CourseID;
            
            var subject = course.CourseName + " New Course Created";
            var message = $"<h1>Hello!</h1> <p>A new course: {course.CourseName} was just created<p/>.";

            var protege = await _context.GetProtegeByIDAsync(course.Pair.ProtegeID);
            var client = await _context.GetClientByIDAsync(course.Pair.ClientID);

            if (protege != null)
            {
                await _emailSender.SendEmailAsync(protege.AppUser.Email, subject, message);

            }
            if (mentor != null)
            {
                await _emailSender.SendEmailAsync(mentor.Email, subject, message);

            }
            if (client != null)
            {
                await _emailSender.SendEmailAsync(client.AppUser.Email, subject, message);
            }

            return RedirectToPage("./Index", new { id = courseID });
        }

        #endregion

    }
}
