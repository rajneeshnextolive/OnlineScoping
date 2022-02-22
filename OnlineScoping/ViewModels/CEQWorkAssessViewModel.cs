using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CEQWorkAssessViewModel
    {
        public Guid CEQWorkstationAssessmentId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
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
