using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQSignatureId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public string SignatureDate { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Nameoforganisation { get; set; }
        public string Nameofbusiness { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
        public string FileName { get; set; }
    }
}
