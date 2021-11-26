using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AspnetCoreProject.Models;

namespace AspnetCoreProject.Validators
{
    public class StockValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Product product =(Product) validationContext.ObjectInstance;
            if (product.Quantity < 5)
                return new ValidationResult("Product stock has minimum 5");
            return ValidationResult.Success;
        }
    }
}
