using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;

namespace MPW.Pages.Sessions
{
    public class EditModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EditModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        [BindProperty]
        public Session Session { get; set; }

        public int SessionID { get; set; }
        #endregion

        #region Handlers

        public async Task<IActionResult> OnGetAsync(int? sessionID)
        {
            if (sessionID == null)
            {
                return NotFound();
            }

            SessionID = sessionID.GetValueOrDefault();

            Session = await _context.GetSessionAsync(SessionID);
            if (Session == null)
            {
                return NotFound();
            }

            await CheckRole(_context, _userManager, Session.Course.Pair.JoinCode);

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

            var success = await _context.UpdateSessionAsync(Session);

            if (success)
            {
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