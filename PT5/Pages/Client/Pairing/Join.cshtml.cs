using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPW.Data;

namespace MPW.Pages.Client.Pairing
{
    public class JoinModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public JoinModel(MPW.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #endregion

        #region Data Model
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Join Code")]
            public string ClientJoinCode { get; set; }
        }
        #endregion

        /// <summary>
        /// Adds the current Protege to a course specified by their join code.
        /// </summary>
        #region Handlers
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the Client
            var client = await _context.GetClientAsync(this.Username);

            //Checks to see if the user is actually a client
            if (client?.Client == null)
            {
                return Redirect("/Error");
            }

            //gets the client ID
            var clientID = client.Client.ID;

            //Gets the Pair based off of the join code enetered
            Input.ClientJoinCode = new string(Input.ClientJoinCode.Where(jc => !char.IsWhiteSpace(jc)).ToArray());
            var pair = _context.Pair.Where(p => p.JoinCode == Input.ClientJoinCode).SingleOrDefault();

            //sets the pair protege ID to the users protege ID
            pair.ClientID = clientID;

            //Sets the Pair protege form as the user
            pair.Client = client.Client;
            await _context.SaveChangesAsync();

            var clientRole = new IdentityRole("Client-" + pair.JoinCode);
            var clientSuccess = await _roleManager.CreateAsync(clientRole);

            if (clientSuccess.Succeeded)
            {
                await _userManager.AddToRoleAsync(client, clientRole.Name);
            }

            else
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");

        }
        #endregion
    }
}