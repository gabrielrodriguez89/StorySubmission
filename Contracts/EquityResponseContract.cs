using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;

namespace Navigator.Contracts.Models
{
    public class EquityResponseContract
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Questions { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
}
