using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPW.Data;
using MPW.Pages.Mentor.ProfileBuilder;

namespace MPW.Pages.Mentor.ProfileBuilder
{
    public class CreateModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private string _username;

        public CreateModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region DataModel
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Tell Us About Your Company")]
            [DataType(DataType.MultilineText)]
            public string About { get; set; }

        }

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
            
            var user = await _context.GetAppUserAsync(Username);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Username}'.");
            }

            var mentor = user.Mentor;
            if (mentor == null)
            {
                return Redirect("/Error");
            }

            await _context.AddMentorAboutAsync(mentor, Input.About);

            return Redirect("./Address");
        }
    }
}