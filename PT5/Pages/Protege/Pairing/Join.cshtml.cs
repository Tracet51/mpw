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

namespace MPW.Pages.Protege.Pairing
{
    public class JoinModel : MPWPageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public JoinModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Data Model
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Join Code")]
            public string ProtegeJoinCode { get; set; }
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
            //Checks to see if the model state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the Protege
            var protege = await _context.GetProtegeAsync(this.Username);


            //Checks to see if the user is a data.protege type if not then redirects to an
            //error
            if (protege?.Protege == null)
            {
                return Redirect("/Error");
            }

            //gets the protege ID
            var protegeID = protege.Protege.ID;

            Input.ProtegeJoinCode = new string(Input.ProtegeJoinCode.Where(jc => !char.IsWhiteSpace(jc)).ToArray());

            //Gets the Pair based off of the join code enetered
            var pair = _context.Pair.Where(p => p.JoinCode == Input.ProtegeJoinCode).SingleOrDefault();

            //sets the pair protege ID to the users protege ID
            pair.ProtegeID = protegeID;

            //Sets the Pair protege form as the user
            pair.Protege = protege.Protege;
            await _context.SaveChangesAsync();

            var protegeRole = new IdentityRole("Protege-" + pair.JoinCode);
            var protegeRoleSucess = await _roleManager.CreateAsync(protegeRole);

            if (protegeRoleSucess.Succeeded)
            {
                await _userManager.AddToRoleAsync(protege, protegeRole.Name);
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