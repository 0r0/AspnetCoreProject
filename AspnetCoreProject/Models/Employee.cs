using AspnetCoreProject.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreProject.Models
{
    public partial class Employee
    {
        [Display(Name ="Employee Id")]
        [Required]
        public int Id { get; set; }
        [Display(Name =" name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name ="family")]
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required]
        public int Age { get; set; }
        [Display(Name ="Creational Time")]
        public DateTime Created { get; set; }

        public virtual EmployeePicture EmployeePicture { get; set; }
    }
}
