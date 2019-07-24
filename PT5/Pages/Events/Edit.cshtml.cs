using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;
using Microsoft.AspNetCore.Identity;

namespace MPW.Pages.Events
{
    public class EditModel : MPWPageModel
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        [BindProperty]
        public Data.Event Event { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? eventID)
        {


            var id = eventID.GetValueOrDefault();
            if (eventID == null)
            {
                return NotFound();
            }

            Event = await _context.GetEventAsync(id);

            if (Event == null)
            {
                return NotFound();

            }

            await CheckRole(_context, _userManager, Event.Course.Pair.JoinCode);

            if (!IsMentor)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _context.UpdateEventAsync(Event);

            if (!success)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Details", new { id = Event.EventID });
        }

        #endregion



    }
}
