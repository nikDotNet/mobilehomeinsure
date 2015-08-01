using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Models
{
    public class ContactViewModel
    {
        [Display(Name = "Name")]
        [Required]
        public string senderName { get; set; }
        [Display(Name = "Email Address")]
        [Required]
        public string senderEmail { get; set; }
        [Display(Name = "Subject")]
        [Required]
        public string subject { get; set; }
        [Display(Name = "Message")]
        [Required]
        public string message { get; set; }

    }
}