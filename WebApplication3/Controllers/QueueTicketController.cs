using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class QueueTicketController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpPost]
        public IHttpActionResult PostQueueTicket([FromBody] QueueTicketModel model)
        {
            // Process the received data
            string department = model.Department;
            string currentQueueTicket = model.CurrentQueueTicket;

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
                        cmd.CommandText = "INSERT INTO QueueInformation (Department, CurrentQueueTicket, CreatedAt) VALUES (@Department, @CurrentQueueTicket, GETDATE())";
                        cmd.Parameters.AddWithValue("@Department", department);
                        cmd.Parameters.AddWithValue("@CurrentQueueTicket", currentQueueTicket);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }

                // Notify clients about the new queue ticket

                return Ok("Queue ticket information received and saved successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetQueueTicket(string department)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the latest queue ticket number for the specified department
                    using (SqlCommand command = new SqlCommand(
                        "SELECT TOP 1 CurrentQueueTicket FROM QueueInformation WHERE Department = @Department ORDER BY CreatedAt DESC", connection))
                    {
                        command.Parameters.AddWithValue("@Department", department);

                        // Execute the command and get the result
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string currentQueueTicket = result.ToString();
                            // Return the current queue ticket
                            return Ok(new { CurrentQueueTicket = currentQueueTicket });
                        }
                        else
                        {
                            // Handle the case where the result is null (no queue ticket found)
                            // You may want to log this situation or return an appropriate response
                            return BadRequest("No queue ticket found for the specified department.");
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

    public class QueueTicketModel
    {
        public string Department { get; set; }
        public string CurrentQueueTicket { get; set; }
    }
}
