using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;

namespace MPW.Pages.PublicProfiles
{
    public class MentorPublicProfileModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public MentorPublicProfileModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public Data.Mentor Mentor { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(string mentorUsername)
        {
            Mentor = await _context.GetPublicMentorAsync(mentorUsername);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }
        #endregion

    }
}
