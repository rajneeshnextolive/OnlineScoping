using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class CEQQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CEQQuestionId { get; set; }
        public string QuestionNumber { get; set; }
        public int QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string StandardRecommendation { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Title { get; set; }
        public int RowNumber { get; set; }
        public int Section { get; set; }
    //    public CEQQuestions QuestionData { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Weighting { get; set; }

        public string VariableName { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }
    }
}
