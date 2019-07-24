using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MPW.Data;

namespace MPW.Pages
{
    public class SelectUserTypeModel : MPWPageModel
    {
        #region Constructors
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<SelectUserTypeModel> _logger;

        public SelectUserTypeModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<SelectUserTypeModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Helpers
        SelectList PopulateUserType(AppUser user)
        {
            var userTypes = new List<SelectListItem>();
            if (user.Mentor == null)
            {
                var mentor = new SelectListItem("0", "Mentor");
                userTypes.Add(mentor);
            }

            if (user.Protege == null)
            {
                var protege = new SelectListItem("1", "Protege");
                userTypes.Add(protege);
            }

            if (user.Client == null)
            {
                var client = new SelectListItem("2", "Client");
                userTypes.Add(client);
            }

            var typesSelect = new SelectList(userTypes, "Text", "Value");

            return typesSelect;

        }
        #endregion

        #region DataModel

        [Display(Name = "User Type")]
        public SelectList UserType { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public int TypeId { get; set; }
        }
        #endregion

        #region Http
        public async  Task<IActionResult> OnGetAsync()
        {
            var user = await _context.GetAppUserAsync(Username);
            
            this.UserType = this.PopulateUserType(user);

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!ModelState.IsValid)
            {
                this.UserType = this.PopulateUserType(user);
                return Page();
            }

            var selected = this.Input.TypeId;


            switch (selected)
            {
                default:
                    this.UserType = this.PopulateUserType(user);
                    return RedirectToPage("/");
                case 0:
                    var mentor = new Data.Mentor();
                    user.Mentor = mentor;
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync(); 
                    return Redirect("/Mentor/ProfileBuilder/Create");
                case 1:
                    var protege = new Data.Protege();
                    user.Protege = protege;
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    return Redirect("/Protege/ProfileBuilders/CreateProtege");
                case 2:
                    var Client = new Data.Client();
                    user.Client = Client;
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    return Redirect("/Client/ProfileCreation/CreateClient");
            }
        }
        #endregion
    }
}