using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CustomerQuestionnair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerQuestionnairId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsQuestionnaire { get; set; }
        public int SectionId { get; set; }
    }
}
