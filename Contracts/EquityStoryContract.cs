using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Navigator.Contracts.Models
{
    public class EquityStoryContract
    {
        #region GETTERS/SETTERS

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [DisplayName("Short Description")]
        [Required(ErrorMessage = "Please enter a short description")]
        [StringLength(150, ErrorMessage = "Description Length Cannot Exceed 150 Characters.")]
        public string Description { get; set; }

        [DisplayName("Publish Date")]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        
        public bool IsActive { get; set; }

        [DisplayName("Name")]
        [DefaultValue("")]
        public string ContactName { get; set; }

        [DisplayName("Email Address")]
        [DefaultValue("")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [DisplayName("Phone Number")]
        [DefaultValue("")]
        public string ContactPhone { get; set; }

        [DefaultValue("")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a short description")]
        public List<EquityQuestionContract> Questions { get; set; }

        [AllowHtml]
        public string ArticleContent { get; set; }
        
        [DefaultValue("")]
        public string ImgThumb { get; set; }

        public string CreatedBy { get; set; }

        #endregion

        #region CONSTRUCTORS

        public EquityStoryContract()
        {
            Questions  = new List<EquityQuestionContract>();
            PublishDate = DateTime.Now;
        }
        
        #endregion
    }

    public class EquityQuestionContract
    {
        [AllowHtml]
        [Required]
        public string QuestionText { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Please answer the question to continue.")]
        public string AnswerText { get; set; }
    }
}
