using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPW.Data;

namespace MPW.Pages.Mentor.ProfileBuilder
{
    public class AddressModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private string _username;

        public AddressModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region DataModel
        [BindProperty]
        public Address AddressMentor { get; set; }

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

            var user = await _context.GetMentorAsync(this.Username);

            var mentor = user.Mentor;
            if (mentor == null)
            {
                return NotFound($"Unable to load user with ID '{Username}'.");
            }


            if (mentor.Address != null)
            {
                return Redirect("/Error");
            }

            await _context.UpdateMentor(mentor, AddressMentor);
          
            return Redirect("./Certificates");
        }
        #endregion

        #region Seed
        #endregion

    }
}