using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class CEQQuestionResponseViewModel
    {
        public Guid CEQResponseId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid QuestionId { get; set; }
        public int OptionType { get; set; }
        public Guid OptionId { get; set; }
        public DateTime OptionDate { get; set; }
        public Guid QuestionnaireId { get; set; }
        public int Cost { get; set; }
        public int Days { get; set; }
        public string Requirement { get; set; }


        public CEQQuestionsViewModel QuestionnaireData { get; set; }
        public CEQQuestions QuestionData { get; set; }

        public decimal Weighting { get; set; }
        public string Response { get; set; }
    }

    public class CEQQuestionResponseResult
    {
        public int sectionid { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Weightingdata { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Responsedata { get; set; }
        public string Name { get; set; }
    }


}
