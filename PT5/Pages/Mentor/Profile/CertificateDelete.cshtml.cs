using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Profile
{
    public class CertificateDeleteModel : PageModel
    {
        private readonly MPW.Data.ApplicationDbContext _context;

        #region Constructors
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
        /// Finds the certificate infomration based on id
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

            //finds the certificate based off of the id
            Certificate = await _context.Certificate.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if certificate is null
            if (Certificate == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //checks to see if id is null
            if (id == null)
            {
                return NotFound();
            }

            //gets certificate based off of id
            Certificate = await _context.Certificate.FindAsync(id);

            //checks to see if certificate is not null and removes certificate
            if (Certificate != null)
            {
                _context.Certificate.Remove(Certificate);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Details");
        }

        #endregion
    }
}
