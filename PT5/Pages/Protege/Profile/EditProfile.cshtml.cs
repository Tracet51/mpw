using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Profile
{
    public class EditProfileModel : MPWPageModel
    {

        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public EditProfileModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model

        [BindProperty]
        public Address Address { get; set; }

        [Required]
        [Display(Name = "Tell Us About Your Company")]
        [DataType(DataType.MultilineText)]
        [BindProperty]
        public string About { get; set; }
        #endregion

        #region Handlers
        /// <summary>
        /// Gets the Proteges Address and About Details
        /// </summary>
        /// <param name="ProtegeID"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? ProtegeID)
        {
            //Gets the user based off of the username
            var user = await _context.GetProtegeAsync(this.Username);

            //Checks to see if user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            //Gets the protege profile based on the user
            var Protege = user.Protege;

            //Checks to see if protege exists
            if (Protege == null)
            {
                return Redirect("/Error");
            }

            //Sets the Address to the Protege Address data
            Address = Protege.Address;

            //Sets the About to the Protege About data
            About = Protege.About;

            return Page();
        }

        /// <summary>
        /// Inputs the updated address and about information to the Protege profile
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //checks to see if model state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Gets the protege profile based on the username
            var user = await _context.GetProtegeAsync(this.Username);

            //checks to see if the user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            //Sets protege to the protege profile
            var Protege = user.Protege;

            //Adds the Inputed address data to the protege address data
            Protege.Address.City = Address.City;
            Protege.Address.State = Address.State;
            Protege.Address.StreetAddress = Address.StreetAddress;
            Protege.Address.StreetAddress2 = Address.StreetAddress2;
            Protege.Address.ZipCode = Address.ZipCode;

            await _context.SaveChangesAsync();

            //Adds the About section the protege profile
            await _context.AddProtegeAboutAsync(Protege, About);

            return RedirectToPage("./Detail");
        }

        #endregion
    }
}
