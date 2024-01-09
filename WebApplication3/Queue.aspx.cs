using System;

namespace WebApplication3
{
    public partial class Queue : System.Web.UI.Page
    {
        protected string Username { get; set; }
        protected string Department { get; set; }
        protected int CurrentServing { get; set; }
        protected int TotalServedToday { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Simulate user login, replace this with actual authentication logic
            // For example, you might use ASP.NET Identity or another authentication mechanism.
            Username = "JohnDoe";
            Department = "IT";

            // Simulate queue information, replace this with actual data retrieval logic
            // For example, you might query a database to get the current serving and total served today.
            CurrentServing = 5;
            TotalServedToday = 20;
        }
    }
}