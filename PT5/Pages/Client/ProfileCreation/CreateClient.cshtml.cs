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

namespace MPW.Pages.Client.ProfileCreation
{
    public class CreateClientModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CreateClientModel> _logger;

        public CreateClientModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<CreateClientModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
        #endregion

        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the about section to the user's client profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // redirects user to error page
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //is querying the database for the certain users address fields based off of their username and them being
            //a client
            var user = await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                    .Include(u => u.Client)
                .SingleOrDefaultAsync();

            //Checks to see if the user exists
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var client = user.Client;

            //checks to see if user is a client
            if (client == null)
            {
                return Redirect("/Error");
            }

            //inputs what the user entered about their compnay and sets it to the users about section
            client.About = Input.About;
            await _context.SaveChangesAsync();


            _logger.LogInformation("User with ID '{UserId}' created a Mentor Profile", _userManager.GetUserId(User));

            return Redirect("./ClientAddress");
        }
    }
}