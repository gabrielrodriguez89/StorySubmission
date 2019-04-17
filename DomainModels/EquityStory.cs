using Navigator.Service.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Navigator.Service.Models.DomainModels
{
    /// <summary>
    /// equity DomainModel
    /// </summary>
    public class EquityStory : AdditionalInfo
    {
        // database assigned Id for primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // Title of Story
        [Required]
        public string Title { get; set; }
        // short description of story (limited to 150 characters)
        [Required]
        public string Description { get; set; }
        // published date (nullable)
        public DateTime PublishDate { get; set; }
        // isActive is a bool value used o toggle hidden and public stories (admin use only)
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        // users name that posted the story (or is selected as primary contact)
        [DefaultValue("")]
        public string ContactName { get; set; }
        // email for selected user
        [DefaultValue("")]
        [EmailAddress]
        public string ContactEmail { get; set; }
        // company number for selected user
        [DefaultValue("")]
        public string ContactPhone { get; set; }
        // username from AD
        public string UserName { get; set; }
   
        public string ArticleContent { get; set; }
        /*  
         *   comma deliminated questions that were active when this weas answered
         *   (questions that are changed after this is stored will not update these questions)
        */
        [DefaultValue(null)]
        public string Questions { get; set; }
        // img posted with equity story (optional)
        [DefaultValue(null)]
        public string ImgThumb { get; set; }
    }
}