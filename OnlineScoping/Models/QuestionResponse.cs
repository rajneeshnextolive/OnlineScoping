using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class QuestionResponse
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ResponseId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ChildQuestionId { get; set; }
        public Guid ProjectId { get; set; }
        public int OptionType { get; set; }
        public Guid OptionId { get; set; }
        public DateTime OptionDate { get; set; }
        public string Responsetext { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Guid MultipleResponseId { get; set; }
        public bool IsChecked { get; set; }
        public int Days { get; set; }
        public int Cost { get; set; }
        public string Requirement { get; set; }
        public string VariableName { get; set; }

    }
}
