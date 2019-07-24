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
    public class CertificateDetailsModel : PageModel
    {
        #region Constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public CertificateDetailsModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Certificate Certificate { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the details for the Certificate
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

            //gets the certificate based off the id
            Certificate = await _context.Certificate.FirstOrDefaultAsync(m => m.ID == id);

            //checks to see if certificate is null
            if (Certificate == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion
    }
}
