using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MPW.Data;

namespace MPW.Pages.Objectives
{
    public class ListModel : MPWPageModel
    {
        #region Constructor
        private readonly MPW.Data.ApplicationDbContext _context;

        public ListModel(MPW.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Model
        public IList<Trello> Objective { get; set; }

        [TempData]
        public int CourseID { get; set; }

        public Course Course { get; set; }
        #endregion


        #region Handlers
        public async Task OnGetAsync(int courseID)
        {
            this.Objective = await _context.GetObjectivesAsync(courseID);
            this.Course = this.Objective.FirstOrDefault().Course;
        }
        #endregion

    }
}
