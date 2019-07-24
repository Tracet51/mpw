using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Assignments
{
    public class DetailsModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public DetailsModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        #endregion

        #region Model
        [BindProperty]
        public Assignment Assignment { get; set; }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        [BindProperty]
        public Document Document { get; set; }

        [TempData]
        public int AssignmentID { get; set; }

        public bool ShowDateCompleted => Assignment?.DateCompleted == new DateTime() ? false : true;
        #endregion

        #region Handlers

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AssignmentID = id.GetValueOrDefault();
            Assignment = await _context.GetAssignmentAsync(AssignmentID);

            Document = await _context.GetLatestDocumentWithAssingmnetIDAsync(AssignmentID);

            if (Assignment == null)
            {
                return NotFound();
            }

            var joinCode = Assignment.Session.Course.Pair.JoinCode;
            await CheckRole(_context, _userManager, joinCode);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Get the Assingment
            AssignmentID = Assignment.AssignmentID;
            var assignment = await _context.GetAssignmentAsync(Assignment.AssignmentID);
            var document = new Document
            {
                AssignmentID = Assignment.AssignmentID,
                Assignment = assignment
            };

            document = await 
                MemoryStreamExtention.FileToDocumentAsync(FileUpload, document);
          
            var documentSuccess = await _context.AddDocumentAsync(document);

            var assignmentSuccess = true;
            if (assignment.DateCompleted == new DateTime())
            {
                assignment.DateCompleted = DateTime.Now;
                assignmentSuccess = await _context.UpdateAssignmentAsync(assignment);
            }
            
            if (documentSuccess && assignmentSuccess)
            {
                var protege = await _context.GetProtegeByIDAsync(assignment.Session.Course.Pair.ProtegeID);
                var mentor = await _context.GetMentorByIDAsync(assignment.Session.Course.Pair.MentorID);
                var client = await _context.GetClientByIDAsync(assignment.Session.Course.Pair.ClientID);

                var subject = $"{protege?.AppUser?.Email} uploaded Assignment {assignment?.Title}";
                var message = "<h1>Assignment Uploaded</h1>" +
                    $"<p>The Assigment <strong>{assignment.Title}</strong> was submitted by <strong>{protege?.AppUser?.Email}</strong> at <strong>{assignment?.DateCompleted}</strong></p>";

                if (protege != null && mentor != null)
                {
                    await _emailSender.SendEmailAsync(protege.AppUser.Email, subject, message);
                }

                if (mentor != null)
                {
                    await _emailSender.SendEmailAsync(mentor.AppUser.Email, subject, message);

                }

                if (client != null)
                {
                    await _emailSender.SendEmailAsync(client.AppUser.Email, subject, message);
                }

                return RedirectToPage("./Details", new { id = assignment.AssignmentID } );
            }

            return RedirectToPage("/Error");
        }

        public async Task<IActionResult> OnGetDocumentAsync(int id)
        {
            var document = await _context
                .GetLatestDocumentWithAssingmnetIDAsync(id);

            return File(document.File, document.FileType, document.Name);
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var success = await _context.DeleteAssignmentAsync(Assignment.AssignmentID);

            return success ? RedirectToPage("/Sessions/Details", new { id = Assignment.SessionID }) : RedirectToPage("/Error");
        }

        #endregion

    }
}
