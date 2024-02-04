using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class QueueReport
    {
        public int ReportID { get; set; }
        public string QueueTicket { get; set; }
        public string Department { get; set; }
        public DateTime DoneDate { get; set; }
    }
}