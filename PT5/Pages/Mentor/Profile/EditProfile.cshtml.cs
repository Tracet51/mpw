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

namespace MPW.Pages.Mentor.Profile
{
    public class EditProfileModel : MPWPageModel
    {
        #region constructors
        private readonly MPW.Data.ApplicationDbContext _context;

        public EditProfileModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Models
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
        /// Gets the address and about details for the mentor
        /// </summary>
        /// <param name="MentorID"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? MentorID)
        {
            //Gets mentor profile based on the username
            var user = await _context.GetMentorAsync(this.Username);

            //checks to see if user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            var Mentor = user.Mentor;

            //checks to see if mentor is null
            if (Mentor == null)
            {
                return Redirect("/Error");
            }

            //sets the address model to the mentor address
            Address = Mentor.Address;

            //sets the about model to the mentor about
            About = Mentor.About;
       
            return Page();
        }

        /// <summary>
        /// Updates the address and about data on the mentor profile
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //checks to see if model state is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Gets the mentor profile based on the username
            var user = await _context.GetMentorAsync(this.Username);

            //checks to see if user is null
            if (user == null)
            {
                return Redirect("/Error");
            }

            var Mentor = user.Mentor;

            //Adds the address data entered to the mentors database
            Mentor.Address.City = Address.City;
            Mentor.Address.State = Address.State;
            Mentor.Address.StreetAddress = Address.StreetAddress;
            Mentor.Address.StreetAddress2 = Address.StreetAddress2;
            Mentor.Address.ZipCode = Address.ZipCode;

            await _context.SaveChangesAsync();

            //Adds the about data to the mentor profile
            await _context.AddMentorAboutAsync(Mentor, About);

            return RedirectToPage("./Details");
        }

        #endregion
    }
}
