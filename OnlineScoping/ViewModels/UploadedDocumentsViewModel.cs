using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class UploadedDocumentsViewModel
    {
        public Guid UploadedDocumentsId { get; set; }
        public string FileName { get; set; }
        public Nullable<DateTime> FileModifiedTime { get; set; }
        public string FileExtension { get; set; }
        public string FileContents { get; set; }
        public Nullable<long> FileLength { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid Createdby { get; set; }
        public string CheckSum { get; set; }
        [Required]
        [Display(Name = "Upload File")]
        //[RegularExpression(@"(.*?)\.(doc|docx|pdf)$",
        //ErrorMessage = "Only allows file types of docx, doc and pdf.")]

        public IFormFile MyImage { set; get; }
        public Guid CustomerRequestsId { get; set; }
        public string ProjectName { get; set; }

        public string Image { get; set; } 
        public DateTime CreatedDate { get; set; }

        //public List<UploadedDocumentsViewModel> UpDataList { get; set; }
        public List<UploadedDocumentsViewModel> UploadedDocViewList { get; set; }
        //public UploadedDocumentsViewModel() { }

        //public UploadedDocumentsViewModel(UploadedDocumentsViewModel UploadedDocuments)
        //{
        //    FileName = UploadedDocuments.FileName;
        //    UpDataList = UploadedDocuments.UploadedDocViewList;
        //    ProjectId = UploadedDocuments.ProjectId;
        //}
    }
}
