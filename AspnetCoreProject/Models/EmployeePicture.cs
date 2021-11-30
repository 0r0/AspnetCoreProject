using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreProject.Models
{
    public partial class EmployeePicture
    {
        public int EmpId { get; set; }
        [Required(ErrorMessage ="it is required field")]
        public string ImageInfo { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime Created { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
