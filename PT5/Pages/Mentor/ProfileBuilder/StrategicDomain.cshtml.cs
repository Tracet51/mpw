using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.ProfileBuilder
{
    public class StrategicDomainModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private string _username;

        public StrategicDomainModel(MPW.Data.ApplicationDbContext context)
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

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            var user = await _context.GetMentorAsync(this.Username);

            if (user.Mentor == null)
            {
                return NotFound($"Unable to load user with ID '{Username}'.");
            }

            if (user.Mentor.StrategicDomains == null)
            {
                user.Mentor.StrategicDomains = new List<StrategicDomain>();
            }

            await _context.UpdateMentor(user.Mentor, this.StrategicDomain);

            this.SuccessfullyCreated = true;

            return RedirectToPage();
        }
        #endregion
    }
}