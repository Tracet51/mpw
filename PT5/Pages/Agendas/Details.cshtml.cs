using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;

namespace MPW.Pages.Agendas
{
    public class DetailsModel : MPWPageModel
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #endregion

        #region Model
        [BindProperty]
        public Agenda Agenda { get; set; }
        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var agendaID = id.GetValueOrDefault();
            Agenda = await _context.GetAgendaAsync(agendaID);

            if (Agenda == null)
            {
                return NotFound();
            }

            await CheckRole(_context, _userManager, Agenda.Session.Course.Pair.JoinCode);

            return Page();
        }

        public async Task<IActionResult> OnGetDocumentAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendaID = id.GetValueOrDefault();

            var agenda = await _context.GetAgendaAsync(agendaID);
            var document = agenda.Document;

            return File(document.File, document.FileType, document.Name);
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var success = await _context.DeleteAgendaAsync(Agenda.AgendaID);

            return success ?
                RedirectToPage("/Sessions/Details", new { id = Agenda.SessionID })
                    : RedirectToPage("/Error");
        }
        #endregion

    }
}
