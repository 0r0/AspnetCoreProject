using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Models
{
    public class Product
    {
        [Display(Name="Product Id")]
        public int Id { get; set; }
        [Display(Name="Product Name")]
        public string Name { get; set; }
        [Display(Name="Product Price")]
        public float Price { get; set; }
        [Display(Name="Product Quantity")]
        public int Quantity { get; set; }



    }
}
