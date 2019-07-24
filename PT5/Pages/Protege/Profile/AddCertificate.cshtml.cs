using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Profile
{
    public class AddCertificateModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddCertificateModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #endregion

        #region Models
        [BindProperty]
        public Certificate Certificate { get; set; }


        public string SuccessCreated { get; set; }
        #endregion

        #region Handlers

        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the certificate to the User's Protege profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // an error page
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //is querying the database for the certain users Certificate fields based off of their username and them being
            //a Protege
            var user = await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                    .Include(u => u.Protege)
                        .ThenInclude(c => c.Certificates)
                .FirstOrDefaultAsync();

            // Checks to see if the user is null and if the user is not a protege
            if (user == null && user.Protege == null)
            {
                return Redirect("/Error");
            }

            // Checks to see if the users certificates are null
            if (user.Protege.Certificates == null)
            {
                user.Protege.Certificates = new List<Certificate>();
            }

            // Adds the inputed certificates to the users protege profile
            user.Protege.Certificates.Add(Certificate);

            _context.Certificate.Add(Certificate);
            await _context.SaveChangesAsync();

            this.SuccessCreated = "Successfully Created a Certificate";

            return RedirectToPage();
        }

        #endregion
    }
}