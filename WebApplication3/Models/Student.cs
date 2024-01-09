using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Student
    {
        public int RowNumber { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Purpose { get; set; }
        public string QueueTicket { get; set; }
        public DateTime QueueDate { get; set; }
    }
}