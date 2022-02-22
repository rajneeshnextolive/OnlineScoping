using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQWorkstationAssessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQWorkstationAssessmentId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid ProjectId { get; set; }
        public string Descriptionofthedevice { get; set; }
        public string OperatingSystem { get; set; }
        public string Usernameandpassword { get; set; }
        public string Confirmationthatthedevice { get; set; }
        public string Confirmationthatadminaccess { get; set; }
        public string TestLocation { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}
