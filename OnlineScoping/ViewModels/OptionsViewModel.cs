using OnlineScoping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineScoping.ViewModels
{
    public class OptionsViewModel
    {
        
        public Guid OptionId { get; set; }
        public int OptionId1 { get; set; }

        public string OptionText { get; set; }
        public Guid QuestionId { get; set; }       
        public QuestionsViewModel Questions { get; set; }
        public bool res { get; set; }
        public bool OptionValue { get; set; }


        public OptionsViewModel() { }

        public OptionsViewModel(Options options)
        {
            OptionId = options.OptionId;
            QuestionId = options.QuestionId;
            OptionText = options.OptionText;
            OptionValue = options.OptionValue;
        }

        public Options GetQuestions()
        {
            return new Options
            {
              OptionId = OptionId,
            QuestionId = QuestionId,
            OptionText = OptionText,
            OptionValue=OptionValue,
        };
        }
    }
}
