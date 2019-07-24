using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Sessions
{
    public class UploadDocumentModel : MPWPageModel
    {

        #region Constructor
        private readonly ApplicationDbContext _context;

        public UploadDocumentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion
        #region Model
        [BindProperty]
        public FileUpload FileUpload { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var session = await _context.GetSessionAsync(SessionID);
            var document = new Document
            {
                SessionID = this.SessionID,
                Session = session
            };

            document = await MemoryStreamExtention.FileToDocumentAsync(FileUpload, document);

            var success = await _context.AddDocumentAsync(document);
            if (!success)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Details", new { id = SessionID });
        }
        #endregion

    }
}