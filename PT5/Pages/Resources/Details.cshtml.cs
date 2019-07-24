using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Resources
{
    public class DetailsModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public DetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Resource Resource { get; set; }

        public Document Document { get; set; }
        #endregion


        #region Handlers
        /// <summary>
        /// The OnGetAsync method finds the related resource and document and course related to the resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Checks to see if passed in id is null
            if (id == null)
            {
                return NotFound();
            }

            //Uses the GetResourceAsync function from the applicationdbcontext file to query the database for the resource based off of the id passed in.
            Resource = await _context.GetResourceAsync(id.GetValueOrDefault());

            //Quereis the database for the last document uploaded for that resource
            Document = await _context.Document
                .Where(d => d.Resource.ResourceID == id)
                .OrderByDescending(d => d.UploadDate)
                .FirstOrDefaultAsync();

            //checks to see if the resource is null
            if (Resource == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// The OnGetDownloadAsync method allows you to download the file associated with the resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDownloadAsync(int id)
        {
            //Uses the GetDocumentAsync function from the applicationdbcontext file to query the database for the document
            var document = await _context.GetDocumentAsync(id);

            //Downlaods the file to your computer based on the File function.
            return File(document.File, document.FileType, document.Name);
        }
        #endregion
    }
}
