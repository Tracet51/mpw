using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Protege.Profile
{
    public class AddAreasOfImprovementModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public AddAreasOfImprovementModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        [BindProperty]
        public AreasOfImprovement AreasOfImprovement { get; set; }

        public bool SuccessfullyCreated { get; set; }

        #endregion

        #region Handlers

        /// <summary>
        /// Opens the page to enter data
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Adds the users areas of improvement section to the User's Protege profile
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            //Is checking to see if the data entered does not fit what is specified in the model. If not then returns the 
            // an error page
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //is querying the database for the certain users Areas of Improvement fields based off of their username and them being
            //a Protege
            var user = await _context.Users
                .Where(u => u.UserName == User.Identity.Name)
                    .Include(u => u.Protege)
                        .ThenInclude(m => m.AreasOfImprovement)
                   .FirstOrDefaultAsync();

            //Checks to see if the user is null and if the user is not a protege
            if (user == null || user.Protege == null)
            {
                return Redirect("/Error");
            }

            //Checks to see if the users Areas of Improvement are null
            if (user.Protege.AreasOfImprovement == null)
            {
                user.Protege.AreasOfImprovement = new List<AreasOfImprovement>();
            }

            // Adds the inputed Areas of Improvement to the users protege profile
            user.Protege.AreasOfImprovement.Add(AreasOfImprovement);

            await _context.SaveChangesAsync();

            this.SuccessfullyCreated = true;

            return RedirectToPage();
        }
        #endregion
    }
}