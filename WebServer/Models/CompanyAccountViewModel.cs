using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class CompanyAccountViewModel
    {
        [Required]
        [Display(Name = "companyEmail")]
        public string companyEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "companyPassword")]
        public string companyPassword { get; set; }

        [Display(Name = "returnUrl")]
        public string returnUrl { get; set; }
        public bool? autoLogin { get; set; }

    }
}