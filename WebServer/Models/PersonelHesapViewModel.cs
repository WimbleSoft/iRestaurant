using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class PersonelHesapViewModel
    {
        [Required]
        [Display(Name = "personelEmail")]
        public string personelEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "personelParola")]
        public string personelParola { get; set; }

        [Required]
        [Display(Name = "sirketEmail")]
        public string sirketEmail { get; set; }

        [Display(Name = "returnUrl")]
        public string returnUrl { get; set; }

    }
}