using System;
using System.Data.SqlClient;
using System.Web.Security;

namespace WebApplication3
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            // Get the username and password entered by the user directly from the TextBox controls
            string enteredUsername = username.Text;
            string enteredPassword = password.Text;

            // Check if the username and password match a valid user in your system
            if (IsValidUser(enteredUsername, enteredPassword))
            {
                // Set the authentication cookie
                FormsAuthentication.SetAuthCookie(enteredUsername, false);

                // Store username and department in session
                Session["LoggedInUsername"] = enteredUsername;
                Session["LoggedInDepartment"] = GetUserDepartment(enteredUsername);

                // Check if the user is an admin
                if (IsAdminUser(enteredUsername))
                {
                    // Redirect admin to admin.aspx
                    Response.Redirect("admin.aspx");
                }
                else
                {
                    // Redirect non-admin to home.aspx
                    Response.Redirect("home.aspx");
                }
            }
            else
            {
                // Display an error message if the credentials are invalid
                errorMessage.InnerText = "Error: Invalid credentials";
            }
        }

        // Replace this method with your own logic to validate the user
        private bool IsValidUser(string enteredUsername, string enteredPassword)
        {
            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // SQL query to check if the username and password match
            string query = "SELECT COUNT(*) FROM users WHERE username = @Username AND password = @Password";


            // Set the user's department in session
            Session["LoggedInDepartment"] = GetUserDepartment(enteredUsername);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", enteredUsername);
                        command.Parameters.AddWithValue("@Password", enteredPassword);

                        // Execute the query and check if there is a match
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count is greater than 0, there is a match
                        return count > 0;
                    }
                }
            }
            catch 
            {
                // Log or handle the exception as needed
                // For simplicity, you can also throw the exception to indicate a failure
                throw;
            }
        }
        private string GetUserDepartment(string username)
        {
            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // SQL query to get the department for the given username
            string query = "SELECT department FROM users WHERE username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
            }

            return string.Empty;
        }
        // Method to check if the user is an admin
        private bool IsAdminUser(string enteredUsername)
        {
            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // SQL query to get the isAdmin value for the given username
            string query = "SELECT isAdmin FROM users WHERE username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", enteredUsername);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int isAdmin = Convert.ToInt32(result);
                        return isAdmin == 1;
                    }
                }
            }

            return false;
        }
    }
}
