using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ReportFilterModel
    {
        public DateTime? DateFilter { get; set; }
        public List<string> DepartmentFilters { get; set; }
    }
}