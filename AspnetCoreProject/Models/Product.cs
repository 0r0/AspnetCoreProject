using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreProject.Validators;
namespace AspnetCoreProject.Models
{
    public class Product
    {
        [Display(Name="Product Id")]
        [Required]
        public int Id { get; set; }
        [Display(Name="Product Name")]
        [Required]
        public string Name { get; set; }
        [Display(Name="Product Price")]
        [Required]
        public float Price { get; set; }
        [Display(Name="Product Quantity")]
        [Required]
        [StockValidationAttribute]
        public int Quantity { get; set; }



    }
}
