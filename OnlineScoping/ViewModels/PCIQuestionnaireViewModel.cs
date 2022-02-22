using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class PCIQuestionnaireViewModel
    {
        public Guid PCIQuestionnaireId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public DateTime Date { get; set; }
        public string IPAddress { get; set; }
        public string FQDN { get; set; }
        public string Description { get; set; }
        public string Ownership { get; set; }
    }
}
