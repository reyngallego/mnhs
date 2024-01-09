using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class ButtonsController : ApiController
    {
        [HttpGet]
        [Route("api/buttons/getqueueticket/{accountType}")]
        public IHttpActionResult GetQueueTicket(string accountType)
        {
            try
            {
                string queueTicket = GetQueueTicketFromDatabase(accountType);

                if (!string.IsNullOrEmpty(queueTicket))
                {
                    return Ok(queueTicket);
                }
                else
                {
                    return NotFound(); // Or return an appropriate response for an empty result
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/buttons/markasdone/{accountType}")]
        public IHttpActionResult MarkAsDone(string accountType)
        {
            try
            {
                // Update the database to mark the ticket as done
                if (MarkTicketAsDoneInDatabase(accountType))
                {
                    return Ok("Ticket marked as done successfully");
                }
                else
                {
                    return NotFound(); // Or return an appropriate response for a failed operation
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/buttons/markasnext/{accountType}")]
        public IHttpActionResult MarkAsNext(string accountType, [FromBody] string currentTicket)
        {
            try
            {
                // Implement the logic to get the next ticket from the database
                string nextTicket = GetNextTicketFromDatabase(accountType, currentTicket);

                if (!string.IsNullOrEmpty(nextTicket))
                {
                    return Ok(nextTicket);
                }
                else
                {
                    return NotFound(); // Or return an appropriate response for an empty result
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/buttons/markasprevious/{accountType}")]
        public IHttpActionResult MarkAsPrevious(string accountType, [FromBody] string currentTicket)
        {
            try
            {
                // Implement the logic to get the previous ticket from the database
                string previousTicket = GetPreviousTicketFromDatabase(accountType, currentTicket);

                if (!string.IsNullOrEmpty(previousTicket))
                {
                    return Ok(previousTicket);
                }
                else
                {
                    return NotFound(); // Or return an appropriate response for an empty result
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private string GetNextTicketFromDatabase(string accountType, string currentTicket)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string query = $"SELECT TOP 1 queueticket FROM student WHERE CONVERT(DATE, queuedate) = CONVERT(DATE, GETDATE()) AND isDone = 0 AND department = '{accountType}' AND queueticket > '{currentTicket}' ORDER BY queueticket ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }

        private string GetPreviousTicketFromDatabase(string accountType, string currentTicket)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string query = $"SELECT TOP 1 queueticket FROM student WHERE CONVERT(DATE, queuedate) = CONVERT(DATE, GETDATE()) AND isDone = 0 AND department = '{accountType}' AND queueticket < '{currentTicket}' ORDER BY queueticket DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }

        private bool MarkTicketAsDoneInDatabase(string accountType)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a field like 'isDone' in your 'student' table
            string updateQuery = $"UPDATE student SET isDone = 1 WHERE CONVERT(DATE, queuedate) = CONVERT(DATE, GETDATE()) AND isDone = 0 AND department = '{accountType}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // If at least one row was affected, the update was successful
                    return rowsAffected > 0;
                }
            }
        }

        private string GetQueueTicketFromDatabase(string accountType)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string query = $"SELECT queueticket FROM student WHERE CONVERT(DATE, queuedate) = CONVERT(DATE, GETDATE()) AND isDone = 0 AND department = '{accountType}' ORDER BY queueticket ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }
    }
}
