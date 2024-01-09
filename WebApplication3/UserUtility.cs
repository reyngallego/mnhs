using System;
using System.Data.SqlClient;
using WebApplication3; // Add the correct namespace

namespace WebApplication3
{
    public class UserUtility
    {
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
