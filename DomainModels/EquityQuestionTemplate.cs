using Navigator.Service.Models.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navigator.Service.Models.DomainModels
{
    /// <summary>
    ///  equity story questions 
    /// </summary>
    public class EquityQuestionTemplate
    {
        // database assigned id
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // isActive is a bool value used o toggle hidden and public questions (admin use only)
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        //[Required]
        //public int EquityStoryId { get; set; }

        // question for equity stories 
        public string CreatedBy { get; set; }
        public string Question { get; set; }
        
    }
}