using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Models
{
    public class EmployeeView
    {
        public List<Employee> Employees { get; set; }
        public int CurrentPageIndex {get;set;}
        public int PageCount { get; set; }




        
    }
}
