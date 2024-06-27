using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication3
{
    public partial class Login : System.Web.UI.Page
    {
        private string connectionString = "Data Source=ULUPONG-DESKTOP\\SQLEXPRESS;Initial Catalog=moonwalk;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                Response.Redirect("mnhs.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (ValidateUser(username, password))
            {
                Session["Username"] = username;
                Response.Redirect("mnhs.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
                lblMessage.Visible = true;
            }
        }

        private bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM admin WHERE Username=@Username AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
