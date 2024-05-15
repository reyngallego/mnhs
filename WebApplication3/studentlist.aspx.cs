using System;
using System.Web.Services;

namespace WebApplication3
{
    public partial class attendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static object SearchAttendance(string name)
        {
            // Implement your logic to search attendance data based on the name
            // For demonstration purposes, I'm returning dummy data
            return new[]
            {
                new { Name = "John Doe", Time = "9:00 AM", Date = "2024-05-14", Class = "Math" },
                new { Name = "Jane Smith", Time = "10:30 AM", Date = "2024-05-14", Class = "Science" }
            };
        }
    }
}
