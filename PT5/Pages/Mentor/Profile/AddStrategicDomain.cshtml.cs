using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Profile
{
    public class AddStrategicDomainModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private string _username;

        public AddStrategicDomainModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Data Model
        [BindProperty]
        public StrategicDomain StrategicDomain { get; set; }

        public bool SuccessfullyCreated { get; set; }

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

        #region Handlers
        /// <summary>
        /// Returns the default page to add strategic domain
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }


        /// <summary>
        /// Adds strategic domains to the User's mentor profile
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //checks to see if model state is valid
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //gets the users mentor profile based on the username
            var user = await _context.GetMentorAsync(this.Username);

            //checks to see if the mentor is null
            if (user.Mentor == null)
            {
                return NotFound($"Unable to load user with ID '{Username}'.");
            }

            //checks to see mentors strategic domains is null if so creates a new list
            if (user.Mentor.StrategicDomains == null)
            {
                user.Mentor.StrategicDomains = new List<StrategicDomain>();
            }

            //Adds the strategic domains to the mentor profile
            await _context.UpdateMentor(user.Mentor, this.StrategicDomain);

            this.SuccessfullyCreated = true;

            return RedirectToPage();
        }
        #endregion
    }
}