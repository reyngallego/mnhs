using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class QueueReportsController : ApiController
    {
        private readonly string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpGet]
        public IHttpActionResult GetQueueReports(DateTime? dateFilter = null)
        {
            List<QueueReport> queueReports = new List<QueueReport>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ReportID, QueueTicket, Department, CAST(DoneDate AS DATE) AS DoneDate, Timer FROM QueueReports";

                // Append WHERE clause to filter by DoneDate if dateFilter is provided
                if (dateFilter != null)
                {
                    query += " WHERE CAST(DoneDate AS DATE) = @DoneDate";
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add DoneDate parameter if dateFilter is provided
                if (dateFilter != null)
                {
                    command.Parameters.AddWithValue("@DoneDate", dateFilter.Value.Date);
                }

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            TimeSpan timerValue = (TimeSpan)reader["Timer"];
                            Console.WriteLine("Timer value from database: " + timerValue);

                            DateTime doneDate = (DateTime)reader["DoneDate"];

                            QueueReport report = new QueueReport
                            {
                                ReportID = Convert.ToInt32(reader["ReportID"]),
                                QueueTicket = reader["QueueTicket"].ToString(),
                                Department = reader["Department"].ToString(),
                                DoneDate = doneDate.Date.ToString("MM/dd/yyyy"), // Format the date as needed
                                Timer = timerValue
                            };
                            queueReports.Add(report);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception for further analysis (consider using a logging library).
                            Console.WriteLine("Error processing record: " + ex.Message);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(queueReports);
        }

        // You can add other CRUD operations (POST, PUT, DELETE) here if needed
    }
}
