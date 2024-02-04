using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class QueueTicketDoneController : ApiController
    {
        [HttpGet]
        [Route("api/Ticket/GetTicketsDoneForToday")]
        public IHttpActionResult GetTicketsDoneForToday(string department)
        {
            try
            {
                int ticketsDone = RetrieveTicketsDoneForTodayFromDatabase(department);
                return Ok(ticketsDone);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private int RetrieveTicketsDoneForTodayFromDatabase(string department)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a 'student' table with 'queueticket', 'department', 'queuedate', and 'IsDone' columns
            string query = "SELECT COUNT(*) FROM student WHERE department = @Department AND CONVERT(date, queuedate) = CONVERT(date, GETDATE()) AND IsDone = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
