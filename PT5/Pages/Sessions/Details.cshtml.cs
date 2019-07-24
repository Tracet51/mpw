using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Sessions
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
        public Session Session { get; set; }

        #endregion

        #region Handlers

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionID = id.GetValueOrDefault();

            Session = await _context.GetSessionAsync(sessionID);

            if (Session == null)
            {
                return NotFound();
            }

            var joinCode = Session.Course.Pair.JoinCode;

            await CheckRole(_context, _userManager, joinCode);

            return Page();
        }

        public async Task<IActionResult> OnGetDocumentAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentID = id.GetValueOrDefault();
            var document = await _context.GetDocumentAsync(documentID);

            return File(document.File, document.FileType, document.Name);
        }

        public async Task<IActionResult> OnPostDeleteDocumentAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentID = id.GetValueOrDefault();
            var success = await _context.DeleteDocumentAsync(documentID);

            if (success)
            {
                return RedirectToPage("./Details", new { id = Session.SessionID });

            }

            return RedirectToPage("/Error");
        }

        #endregion


    }
}
