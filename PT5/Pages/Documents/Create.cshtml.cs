using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MPW.Data;
using MPW.Utilities;

namespace MPW.Pages.Documents
{
    public class CreateModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public CreateModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        [BindProperty]
        public FileUpload FileUpload { get; set; }

        #endregion

        #region Handlers

        public IActionResult OnGet()
        {
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var document = new Document
            {

            };

            document = await MemoryStreamExtention.FileToDocumentAsync(FileUpload, document);

            var success = await _context.AddDocumentAsync(document);
            if (!success)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        #endregion


    }
}
