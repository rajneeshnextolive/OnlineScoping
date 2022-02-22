using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class MethodologyeViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "File Type")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "Upload File")]
        [RegularExpression(@"(.*?)\.(doc|docx|pdf)$",
        ErrorMessage = "Only allows file types of docx, doc and pdf.")]
        public IFormFile FileData { get; set; }
        public string FileName { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FileContents { get; set; }

        public List<MethodologyeViewModel> MethodologiesList { get; set; }
    }
}
