using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Resources
{
    public class IndexModel : PageModel
    {
        private readonly MPW.Data.ApplicationDbContext _context;

        public IndexModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Resource> Resource { get;set; }

        public async Task OnGetAsync()
        {
            Resource = await _context.Resource
                .Include(r => r.Course).ToListAsync();
        }
    }
}
