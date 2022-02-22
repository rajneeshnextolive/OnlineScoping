using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class QuestionnairDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionnairDetailsId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid CustomerId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
