using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class parentController : ApiController
    {
        // Connection string to your database
        private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

        // Endpoint to fetch parent contact by LRN
        [HttpGet]
        [Route("api/parent/contact")]
        public IHttpActionResult GetParentContact(string lrn)
        {
            try
            {
                // SQL query to fetch parent contact based on LRN
                string query = "SELECT ParentContact FROM mnhs WHERE LRN = @LRN";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LRN", lrn);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Retrieve parent contact from the database
                            string parentContact = reader["ParentContact"].ToString();
                            return Ok(new { contacts = parentContact.Split(',') });
                        }
                        else
                        {
                            return NotFound(); // LRN not found
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex); // Internal server error
            }
        }
    }
}
