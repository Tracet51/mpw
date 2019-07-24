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

namespace MPW.Pages.Client.ProfileCreation
{
    public class ClientAddressModel : PageModel
    {
            #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ClientAddressModel> _logger;

        public ClientAddressModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<ClientAddressModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region DataModel
        [BindProperty]
        public Address AddressClientData { get; set; }
        #endregion

        #region Http
        //Returning the Client Address page
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the Address to the user's client profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // redirects user to error page
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //_context.Address.Add(Address);

            //is querying the database for the certain users address fields based off of their username and them being
            //a client
            var user = await _context.Users
               .Where(u => u.UserName == User.Identity.Name)
                   .Include(u => u.Client)
                    .ThenInclude(c => c.Address)
               .SingleOrDefaultAsync();

            var client = user.Client;

            //Checking to see if the user is null
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //Checking to see if the user entered data into the required fields if not then it returns an error page
            if (AddressClientData == null)
            {
                return Redirect("/Error");
            }

            //Is setting the client address field to data entered by user
            client.Address = AddressClientData;

            //Saving changes to the database
            await _context.SaveChangesAsync();


            _logger.LogInformation("User with ID '{UserId}' created a Protege Profile", _userManager.GetUserId(User));

            //Is redirecting the user to the join pairing page after they submit the form
            return RedirectToPage("../Pairing/Join");
        }
        #endregion
    }
}