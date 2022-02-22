using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class QuestionsViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionId { get; set; }
        public int SectionId { get; set; }
        public string QuestionNumber { get; set; }

        public QuestionType questionType { get; set; }
        public string QuestionText { get; set; }
        public string OptionText { get; set; }
        public List<Options> OptionsList { get; set; }
        public List<QuestionsViewModel> QuestionsViewModelList { get; set; }

        public QuestionsViewModel() { }


        public QuestionsViewModel(Questions cuestions)
        {
            QuestionId = cuestions.QuestionId;
            SectionId = cuestions.SectionId;
            QuestionNumber = cuestions.QuestionNumber;
            questionType = cuestions.questionType;
            QuestionText = cuestions.QuestionText;
        }

        public Questions GetQuestions()
        {
            return new Questions
            {
                QuestionId = QuestionId,
            SectionId = SectionId,
            QuestionNumber = QuestionNumber,
             questionType = questionType,
            QuestionText = QuestionText
        };
        }

    }
  








}
