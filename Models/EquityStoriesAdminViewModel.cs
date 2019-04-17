using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Navigator.Contracts.Models;

namespace Navigator.Client.Models.ViewModels
{
    public class EquityStoriesAdminViewModel
    {
        public IEnumerable<EquityContract> Stories { get; set; }

        public IEnumerable<EquityQuestionsContract> Questions { get; set; }
        public IEnumerable<EquityResponseContract> Response { get; set; }
        public EquityStoriesAdminViewModel()
        {
            Stories = new List<EquityContract>();
            Response = new List<EquityResponseContract>();
            Questions = new List<EquityQuestionsContract>();
        }
    }
}