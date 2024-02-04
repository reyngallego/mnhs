using System.Data.SqlClient;
using System;

namespace WebApplication3
{
    public class UserUtility
    {
        public static bool IsUserLoggedIn()
        {
            // Check if the user is logged in by checking the session
            return (System.Web.HttpContext.Current.Session["LoggedInUsername"] != null);
        }

        public static string GetUserDepartment(string username)
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
    }
}
