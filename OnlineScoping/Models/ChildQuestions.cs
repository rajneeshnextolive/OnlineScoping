using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class ChildQuestions
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ChildQuestionId { get; set; }
        public Guid QuestionId { get; set; }
        public int SectionId { get; set; }
        public string QuestionNumber { get; set; }

        public QuestionType questionType { get; set; }
        public string QuestionText { get; set; }
        public Guid QuestionnaireId { get; set; }
        public int Days { get; set; }
        public int Cost { get; set; }
        public string Requirement { get; set; }
        public string VariableName { get; set; }
        public string QuestionValue { get; set; }
        public bool isMandatory { get; set; }
        public enum ChildQuestionType
        {
            multipleResponseType,
            DateTimeType,
            TextType
        }
    }
}
