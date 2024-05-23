using System;
using System.Web;

namespace WebApplication3
{
    public partial class mnhs : System.Web.UI.Page
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

        protected void Logout_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            // Redirect to login page
            Response.Redirect("login.aspx");
        }
    }
}
