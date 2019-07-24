using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;
using Microsoft.AspNetCore.Identity;

namespace MPW.Pages.Protege.Course
{
    public class ListModel : MPWPageModel
    {
        #region Cosntructor
        private readonly MPW.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ListModel(MPW.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        public IList<Data.Course> Courses { get; set; }

        public string JoinCode { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? pairID)
        {
            await CheckRole(_context, _userManager, JoinCode);
            if (pairID == null)
            {
                return NotFound();
            }

            var id = pairID.GetValueOrDefault();
            var pair = await _context.GetPairAsync(id);

            Courses = pair.Courses;
            JoinCode = pair.JoinCode;

            return Page();
        }
        #endregion

    }
}
