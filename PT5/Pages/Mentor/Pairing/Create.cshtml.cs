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
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Mentor.Pairing
{
    public class CreateModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(MPW.Data.ApplicationDbContext context, IEmailSender emailSender, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion


        #region Model
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Client Email")]
            public string ClientEmailAddress { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Protege Email")]
            public string ProtegeEmailAddress { get; set; }

        }

        
        #endregion


        #region Handlers
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the Mentor
            var mentor = await _context.GetMentorAsync(this.Username);

            // Add the pairing to the mentor
            var mentorId = mentor.Mentor.ID;
            var joinCode = Guid.NewGuid().ToString();
            var addDate = DateTime.Now;

            var pair = new Pair
            {
                DateCreated = addDate,
                MentorID = mentorId,
                Mentor = mentor.Mentor,
                JoinCode = joinCode
            };

            await _context.AddPairAsync(pair);


            var message = "Welcome to the Mentor Protege Program powered by Esolvit Government Solutions. Please use the Code Below to join!: " + joinCode;
            var subject = "Join Mentor-Protege Program.";

            try
            {
                await _emailSender.SendEmailAsync(Input.ProtegeEmailAddress, subject, message);
                await _emailSender.SendEmailAsync(Input.ClientEmailAddress, subject, message);
            }
            catch (Exception)
            {
            }


            var mentorRole = new IdentityRole("Mentor-" + joinCode);
            var mentorRoleSuccess = await _roleManager.CreateAsync(mentorRole);

            if (mentorRoleSuccess.Succeeded)
            {
                await _userManager.AddToRoleAsync(mentor, mentorRole.Name);
            }

            else
            {
                return RedirectToPage("/Error");
            }


            return RedirectToPage("/Mentor/Pairing/Index");
        }
        #endregion

    }
}