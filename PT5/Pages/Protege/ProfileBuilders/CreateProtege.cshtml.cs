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

namespace MPW.Pages.Protege
{
    public class CreateProtegeModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CreateProtegeModel> _logger;

        public CreateProtegeModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<CreateProtegeModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Model
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Tell Us About Your Company")]
            [DataType(DataType.MultilineText)]
            public string About { get; set; }

        }
        #endregion

        #region Handlers
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the about section to the user's protege profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // redirects user to error page
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //is querying the database for the certain users address fields based off of their username and them being
            //a protege
            var user = await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                    .Include(u => u.Protege)
                .FirstOrDefaultAsync();

            //Checks to see if user exists
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var protege = user.Protege;
            //Checks to see if user is protege
            if (protege == null)
            {
                return Redirect("/Error");
            }


            //Sets what the user entered about their company to the users profile.
            protege.About = Input.About;
            await _context.SaveChangesAsync();


            _logger.LogInformation("User with ID '{UserId}' created a Protege Profile", _userManager.GetUserId(User));

            return Redirect("./ProtegeAddress");
        }
        #endregion
    }
}