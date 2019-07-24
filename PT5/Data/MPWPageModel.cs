using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPW.Data
{

    public class MPWPageModel : PageModel
    {
        private string _username;
        public string Username
        {
            get => User == null ? this._username : User.Identity.Name;

            set => this._username = value;
        }

        public string Error { get; set; }

        public bool IsMentor { get; set; }
        public bool IsProtege { get; set; }
        public bool IsClient { get; set; }

        public async Task CheckRole(ApplicationDbContext context, UserManager<AppUser> userManager, string pairJoinCode)
        {
            // Initialize all possible ones to false
            this.IsMentor = false;
            this.IsProtege = false;
            this.IsClient = false;

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return;
            }

            var mentorRole = "Mentor-" + pairJoinCode;
            if (await userManager.IsInRoleAsync(user, mentorRole))
            {
                this.IsMentor = true;
                return;
            }

            var protegeRole = "Protege-" + pairJoinCode;
            if (await userManager.IsInRoleAsync(user, protegeRole))
            {
                this.IsProtege = true;
                return;
            }

            var clientRole = "Client-" + pairJoinCode;
            if (await userManager.IsInRoleAsync(user, clientRole))
            {
                this.IsClient = true;
                return;
            }
        }
    }
}
