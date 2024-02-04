using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using WebApplication3.Models;

public class ReportsController : ApiController
{
    private readonly string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

    [HttpGet]
    public IHttpActionResult GetQueueReports()
    {
        List<QueueReport> queueReports = new List<QueueReport>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT ReportID, QueueTicket, Department, DoneDate FROM QueueReports";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    QueueReport report = new QueueReport
                    {
                        ReportID = Convert.ToInt32(reader["ReportID"]),
                        QueueTicket = reader["QueueTicket"].ToString(),
                        Department = reader["Department"].ToString(),
                        DoneDate = Convert.ToDateTime(reader["DoneDate"])
                    };

                    queueReports.Add(report);
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
}
