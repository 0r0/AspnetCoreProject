using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Models
{
    public class EmployeeSortPagingView
    {
        public List<Employee> Employees { get; set; }
        public int CurrentIndexPage { get; set; }
        public int PageCount { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public string CurrentSortField { get; set; }
        public string CurrentSortOrder { get; set; }
    }
}
