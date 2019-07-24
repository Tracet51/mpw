using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MPW.Data
{
    public class FileUpload
    {

        [Display(Name = "Document")]
        public IFormFile Document { get; set; }

        public string Category { get; set; }
    }
}