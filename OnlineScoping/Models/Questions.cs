using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.Models
{
    public class Questions
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionId { get; set; }
        public int SectionId { get; set; }
        public string QuestionNumber { get; set; }

        public QuestionType questionType { get; set; }
        public string QuestionText { get; set; }


        public Guid QuestionnaireId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int RowNumber { get; set; }
        public int InputType { get; set; }
        public string  Note { get; set; }
        public int Days { get; set; }
        public int Cost { get; set; }
        public string Requirement { get; set; }
        public string VariableName { get; set; }
        public bool isMandatory { get; set; }
        public string QuestionValue { get; set; }
    }


    public enum QuestionType
    {
        multipleResponseType,
        DateTimeType,
        TextType,
        none,
        MultipleCheckType,
        Other
    }
}
