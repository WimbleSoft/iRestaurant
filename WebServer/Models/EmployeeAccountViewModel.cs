using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iRestaurant.Models
{
    public class EmployeeAccountViewModel
    {
        [Required]
        [Display(Name = "employeeEmail")]
        public string employeeEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "employeePassword")]
        public string employeePassword { get; set; }

        [Required]
        [Display(Name = "companyEmail")]
        public string companyEmail { get; set; }

        [Display(Name = "returnUrl")]
        public string returnUrl { get; set; }

    }
}