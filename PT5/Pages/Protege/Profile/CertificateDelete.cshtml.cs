using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Profile
{
    public class CertificateDeleteModel : PageModel
    {

        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public CertificateDeleteModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        [BindProperty]
        public Certificate Certificate { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details for the specified certificate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //checks to see if id is null
            if (id == null)
            {
                return NotFound();
            }

            //Queries the database for the certificate based on the id passed in
            Certificate = await _context.Certificate.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if certificate is null
            if (Certificate == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Deletes the specified certificate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //checks to see if id passed in is null
            if (id == null)
            {
                return NotFound();
            }

            //Queries the database for the certificate based on the id passed in
            Certificate = await _context.Certificate.FindAsync(id);

            //Checks to see if certificate is not nul if not then deletes the certificate
            if (Certificate != null)
            {
                _context.Certificate.Remove(Certificate);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Detail");
        }
        #endregion
    }
}
