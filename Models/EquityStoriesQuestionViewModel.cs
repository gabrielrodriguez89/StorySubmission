using Navigator.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Navigator.Client.Models.ViewModels
{
    public class EquityStoriesQuestionViewModel
    {
        public IEnumerable<EquityQuestionsContract> Questions { get; set; }
        public EquityQuestionsContract Question { get; set; }
        public EquityStoriesQuestionViewModel()
        {
            Questions = new List<EquityQuestionsContract>();
            Question = new EquityQuestionsContract();
        }
    }
}