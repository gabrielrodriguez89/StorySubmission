using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;


namespace Navigator.Contracts.Models
{
    public class EquityQuestionTemplateContract 
    {
        public int Id { get; set; }

        // isActive is a bool value used to toggle hidden and public questions (admin use only)
        [DefaultValue(false)]
        public bool IsActive { get; set; }
        [AllowHtml]
        [Required]
        public string Question { get; set; }
        public string CreatedBy { get; set; }

        public EquityQuestionTemplateContract()
        {
            Id = 0;
            IsActive = true;
        }
    }
}
