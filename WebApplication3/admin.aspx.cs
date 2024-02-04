using System;

namespace WebApplication3
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the user is logged in
                if (!UserUtility.IsUserLoggedIn())
                {
                    // Redirect to the login page if not logged in
                    Response.Redirect("login.aspx");
                    return; // Stop further execution
                }

                // User is logged in, continue with the page logic
            }
        }
    }
}
