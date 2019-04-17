using System;
using System.Collections.Generic;
using Navigator.Contracts.Models;
using System.Linq;
using System.Web;

namespace Navigator.Client.Models.ViewModels
{
    public class EquityStoriesViewModel
    {
        #region GETTERS/SETTERS

        public IEnumerable<EquityContract> Stories { get; set; }

        public IEnumerable<EquityContract> RecentStories { get; set; }

        public EquityContract SelectedStory { get; set; }

        public bool DisplayFullStory { get; set; }

        #endregion

        #region CONSTRUCTORS

        public EquityStoriesViewModel()
        {
            Stories = new List<EquityContract>();

            RecentStories = new List<EquityContract>();

            DisplayFullStory = false;
        }

        #endregion
    }
}