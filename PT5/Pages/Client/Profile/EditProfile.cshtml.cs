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

namespace MPW.Pages.Client.Profile
{
    public class EditProfileModel : MPWPageModel
    {
        #region Constructors
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
        /// Finds the clients Address and about information
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? ClientID)
        {
            //Gets the clients user profile
            var user = await _context.GetClientAsync(this.Username);

            //checks to see if the user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            //Sets client to the users client profile
            var Client = user.Client;


            //Checks to see if client is null
            if (Client == null)
            {
                return Redirect("/Error");
            }

            //Assigns clients address to address field
            Address = Client.Address;

            //Assigns clients about to about field
            About = Client.About;

            return Page();
        }

        /// <summary>
        /// Sets the users info to what was entered in the view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //Checks to see if model state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Gets the client profile based on the username
            var user = await _context.GetClientAsync(this.Username);

            //Checks to see if user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            var Client = user.Client;

            //Sets client address fields to what was entered in the view
            Client.Address.City = Address.City;
            Client.Address.State = Address.State;
            Client.Address.StreetAddress = Address.StreetAddress;
            Client.Address.StreetAddress2 = Address.StreetAddress2;
            Client.Address.ZipCode = Address.ZipCode;

            await _context.SaveChangesAsync();

            //Adds the about entered to the client
            await _context.AddClientAboutAsync(Client, About);

            return RedirectToPage("./Detail");
        }
        #endregion
    }
}
