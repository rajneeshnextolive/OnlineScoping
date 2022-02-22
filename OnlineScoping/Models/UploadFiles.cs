using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class UploadFiles
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    }
}
