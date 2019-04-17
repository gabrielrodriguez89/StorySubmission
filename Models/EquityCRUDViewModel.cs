using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigator.Client.Helpers;
using Navigator.Contracts.Models;

namespace Navigator.Client.Models.ViewModels
{
    public class EquityCRUDViewModel
    {
        #region GETTERS/SETTERS
        [ValidateObject]
        public EquityContract Story { get; set; }

        #endregion

        #region Constructors

        public EquityCRUDViewModel()
        {
            Story = new EquityContract();
        }

        #endregion
    }
}
