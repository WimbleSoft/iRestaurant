using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class SirketHesapViewModel
    {
        [Required]
        [Display(Name = "sirketEmail")]
        public string sirketEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "sirketParola")]
        public string sirketParola { get; set; }

        [Display(Name = "returnUrl")]
        public string returnUrl { get; set; }
        public bool? otomatikGiris { get; set; }

    }
}