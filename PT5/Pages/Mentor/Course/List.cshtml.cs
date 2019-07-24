﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;

namespace MPW.Pages.Mentor.Course
{
    public class ListModel : MPWPageModel
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        public ListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model

        [BindProperty]
        public IList<Data.Course> Courses { get; set; }

        [BindProperty]
        public string JoinCode { get; set; }

        #endregion

        #region Handlers
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pairID = id.GetValueOrDefault();
            var pair = await _context.GetPairAsync(pairID);

            Courses = pair.Courses;
            JoinCode = pair.JoinCode;

            return Page();
        }
        #endregion



    }
}
