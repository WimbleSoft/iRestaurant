using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class EmailFormModel
    {
        [Required]
        public string fromName { get; set; }

        [Required, EmailAddress]
        public string fromEmail { get; set; }

        [Required]
        public string toName { get; set; }
        [Required]
        public string subject { get; set; }

        [Required, EmailAddress]
        public string toEmail { get; set; }

        [Required]
        public string message { get; set; }
    }
}