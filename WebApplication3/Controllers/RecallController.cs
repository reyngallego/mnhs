using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class RecallController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpPost]
        public IHttpActionResult PostRecallInfo([FromBody] RecallModel model)
        {
            // Process the received data
            string recallInfo = model.Recall; // Get the recall information from lblQueueTicket
            string department = model.Department; // Get the department

            // Store the data in the database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;

                        // Use parameterized query to prevent SQL injection
                        cmd.CommandText = "INSERT INTO RecallInformation (Recall, Department, CreatedAt) VALUES (@Recall, @Department, GETDATE())";
                        cmd.Parameters.AddWithValue("@Recall", recallInfo);
                        cmd.Parameters.AddWithValue("@Department", department);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Recall information received and saved successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        public IHttpActionResult GetRecallInfo(string department)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the latest recall information for the specified department
                    using (SqlCommand command = new SqlCommand(
                        "SELECT TOP 1 Recall FROM RecallInformation WHERE Department = @Department ORDER BY CreatedAt DESC", connection))
                    {
                        command.Parameters.AddWithValue("@Department", department);

                        // Execute the command and get the result
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string recallInfo = result.ToString();
                            // Return the recall information
                            return Ok(new { RecallInfo = recallInfo });
                        }
                        else
                        {
                            // Handle the case where the result is null (no recall info found)
                            return BadRequest("No recall information found for the specified department.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, return an error response, etc.)
                return InternalServerError(ex);
            }
        }
    }

    public class RecallModel
    {
        public string Recall { get; set; }
        public string Department { get; set; }
    }
}
