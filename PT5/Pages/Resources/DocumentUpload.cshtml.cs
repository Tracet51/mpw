using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MPW.Data;
using MPW.Utilities;
using static Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal.ExternalLoginModel;

namespace MPW.Pages.Resources
{
    public class DocumentUploadModel : MPWPageModel
    {

        #region Constructor
        private readonly ApplicationDbContext _context;

        public DocumentUploadModel(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Model

        [BindProperty]
        public FileUpload FileUpload { get; set; }
        
        [BindProperty]
        [Required]
        [Display(Name = "Resource Name")]
        [DataType(DataType.Text)]
        public String Name { get; set; }



        [TempData]
        public int courseID { get; set; }
        #endregion


        #region Handlers
        /// <summary>
        /// The OnGet method sets the course ID to the parameter passed in.
        /// The OnPostAsync method uploads the document to the specified resource
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public IActionResult OnGet(int? courseid)
        {
            //checks to see if passed in parameter courseid is null
            if (courseid == null)
            {
                return NotFound();
            }

            //Sets CourseID to the courseid parameter passed in.
            courseID = courseid.GetValueOrDefault();

            return Page();
        }

        /// <summary>
        /// The OnPostAsync method uploads the document to the specified resource
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //checks to see if the Model State is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Creates a new Document
            var document = new Document
            {

            };

            //Passed the FileUpload model and the recently created document to the FileToDocumentAsync function int he applicationDbContext file to put it in the database in bytes.
            document = await MemoryStreamExtention.FileToDocumentAsync(FileUpload, document);

            //Creates a new resouce and fills in certain areas of the model and associates the document with the resource.
            var resource = new Resource
            {
                Category = FileUpload.Category,
                Name = Name,
                Type = "File",
                DateAdded = DateTime.Now,
                CourseID = courseID,

                 Documents = new List <Document>
                {
                     document
                }

        };


            //Adds the resource to the database
            var success = await _context.CreateResourceAsync(resource);

            if (success)
            {
                //Redirects the mentor to the specified course
                return RedirectToPage("../Mentor/Course/Index", new { id = resource.CourseID });
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        #endregion

    }
}