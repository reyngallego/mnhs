using System;
using System.Web.Services;

namespace WebApplication3
{
    public partial class studentlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["Username"] == null)
            {
                // If not, redirect to login page
                Response.Redirect("login.aspx");
            }
        }

      
    }
}
