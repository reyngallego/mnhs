using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class QueueController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpPost]
        public IHttpActionResult SubmitQueueData(QueueDataModel data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert data into the database
                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO student (name, department, purpose) VALUES (@Name, @Department, @Purpose); SELECT SCOPE_IDENTITY();", connection))
                    {
                        command.Parameters.AddWithValue("@Name", data.Name);
                        command.Parameters.AddWithValue("@Department", data.Department);
                        command.Parameters.AddWithValue("@Purpose", data.Purpose);

                        // Execute the command and get the inserted identity (queueticket)
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int queueTicket = Convert.ToInt32(result);
                            // You can return the queueticket or any other relevant information
                            return Ok(new { QueueTicket = queueTicket });
                        }
                        else
                        {
                            // Handle the case where the result is null
                            // You may want to log this situation or return an appropriate response
                            return BadRequest("Failed to retrieve QueueTicket.");
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

        public class QueueDataModel
        {
            public string Name { get; set; }
            public string Department { get; set; }
            public string Purpose { get; set; }
        }
    }
}