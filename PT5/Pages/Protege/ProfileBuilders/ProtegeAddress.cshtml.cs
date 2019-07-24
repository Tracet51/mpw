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

namespace MPW.Pages.Protege.ProfileBuilders
{
    public class ProtegeAddressModel : PageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ProtegeAddressModel> _logger;

        public ProtegeAddressModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<ProtegeAddressModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Model
        [BindProperty]
        public Address AddressData { get; set; }
        #endregion

        #region Handlers
        //Returning the Protege Address page
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the Address to the user's protege profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // error page
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            //_context.Address.Add(Address);

            //is querying the database for the certain users address fields based off of their username and them being
            //a Protege
            var user = await _context.Users
               .Where(u => u.UserName == User.Identity.Name)
                   .Include(u => u.Protege)
                    .ThenInclude(p => p.Address)
               .FirstOrDefaultAsync();

            var protege = user.Protege;

            //Checking to see if the user is null
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //Checking to see if the user entered data into the required fields if not then it returns an error page
            if (AddressData == null)
            {
                return Redirect("/Error");
            }


            //Is setting the Protege address field to data entered by user
            protege.Address = AddressData;

            //Saving changes to the database
            await _context.SaveChangesAsync();


            _logger.LogInformation("User with ID '{UserId}' created a Protege Profile", _userManager.GetUserId(User));

            //Redirecting the page to the protege certificate page
            return Redirect("./ProtegeCertificates");
        }
        #endregion

    }
}