using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Agendas
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
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public int SessionID { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SessionID = id.GetValueOrDefault();

            var session = await _context.GetSessionAsync(SessionID);
            if (session == null)
            {
                return NotFound();
            }

            await CheckRole(_context, _userManager, session.Course.Pair.JoinCode);

            if (!IsProtege)
            {
                return RedirectToPage("/Identity/Account/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var session = await _context.GetSessionAsync(SessionID);

            if (session == null)
            {
                return RedirectToPage("/Error");
            }

            var fileUpload = new FileUpload
            {
                Category = "Agenda",
                Document = FileUpload
            };

            var document = new Document();

            document = await MemoryStreamExtention.FileToDocumentAsync(fileUpload, document);

            var agenda = new Agenda
            {
                CreationDate = DateTime.Now,
                Name = Name,
                SessionID = SessionID,
                Session = session,
                Document = document
            };

            var success = await _context.AddAgendaAsync(agenda);

            if (success)
            {
                var protege = await _context.GetProtegeByIDAsync(session.Course.Pair.ProtegeID);
                var mentor = await _context.GetMentorByIDAsync(session.Course.Pair.MentorID);
                var client = await _context.GetClientByIDAsync(session.Course.Pair.ClientID);

                var subject = $"{protege?.AppUser?.Email} uploaded Agenda {agenda.Name}";
                var message = "<h1>Agenda Uploaded</h1>" +
                    $"<p>The Assigment <strong>{agenda.Name}</strong> was submitted by <strong>{protege?.AppUser?.Email}</strong> at <strong>{agenda.CreationDate}</strong></p>";

                if (protege != null)
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

                return RedirectToPage("/Sessions/Details", new { id = session.SessionID });
            }

            return RedirectToPage("./Index");
        }
        #endregion

    }
}