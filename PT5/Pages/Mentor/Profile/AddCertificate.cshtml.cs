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

namespace MPW.Pages.Mentor.Profile
{
    public class AddCertificateModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private string _username;

        public AddCertificateModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion


        #region Data Model
        [BindProperty]
        public Certificate Certificate { get; set; }


        public string SuccessCreated { get; set; }

        public string Username
        {
            get
            {
                if (User == null)
                {
                    return this._username;
                }
                else
                {
                    return User.Identity.Name;
                }
            }

            set
            {
                this._username = value;
            }
        }

        #endregion



        #region Http

        /// <summary>
        /// Returns the default page to add certificates
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the certificate to the User's mentor profile
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //Checks to see if model state is valid
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //Gets the user mentor profile based on the username
            var user = await _context.GetMentorAsync(this.Username);

            //checks to see if user mentor section is null
            if (user.Mentor == null)
            {
                return NotFound($"Unable to load user with ID '{Username}'.");
            }

            //Adds the certificate to the mentor profile
            await _context.UpdateMentor(user.Mentor, Certificate);

            this.SuccessCreated = "Successfully Created a Certificate";

            return RedirectToPage();
        }
        #endregion
    }
}