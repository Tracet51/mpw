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

namespace MPW.Pages.Assignments
{
    public class CreateModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CreateModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }
        #endregion


        #region Model

        [BindProperty]
        public Assignment Assignment { get; set; }

        [BindProperty]
        public int SessionID { get; set; }

        #endregion

        #region Handlers

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SessionID = id.GetValueOrDefault();
            return Page();
        }

        /// <summary>
        /// Creates an assignment, sends an email to the protege
        /// </summary>
        /// <returns>The post async.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Assignment.SessionID = SessionID;
            var session = await _context.GetSessionAsync(SessionID);
            Assignment.Session = session;

            var success = await _context.AddAssingmentAsync(Assignment);
            var subject = session.Course.CourseName + " New Assigment Created";
            var message = $"<h1>Hello!</h1> <p>A new assigment: {Assignment.Title} was just created<p/>.<br/><p>Due Date: {Assignment.DueDate}</p> <br/> <p>Description: {Assignment.Description}</p>";

            if (success)
            {
                var protegeID = session.Course.Pair.ProtegeID;
                var protege = await _context.GetProtegeByIDAsync(protegeID);
                    

                if (protege != null)
                {
                    try
                    {
                        await _emailSender.SendEmailAsync(
                            protege.AppUser.Email,
                            subject,
                            message);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                var client = await _context.GetClientByIDAsync(session.Course.Pair.ClientID);
                if (client != null)
                {
                    await _emailSender.SendEmailAsync(
                            client.AppUser.Email,
                            subject,
                            message);
                }

                return RedirectToPage("/Sessions/Details", new { id = SessionID });
            }

            return RedirectToPage("/Error");
            
        }

        #endregion




    }
}