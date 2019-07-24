using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Documents
{
    public class DetailsModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Document Document { get; set; }
        #endregion


        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentID = id.GetValueOrDefault();

            Document = await _context.GetDocumentAsync(documentID);

            if (Document == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetDownloadAsync(int id)
        {
            var document = await _context.GetDocumentAsync(id);

            return File(document.File, document.FileType, document.Name);
        }
        #endregion


    }
}
