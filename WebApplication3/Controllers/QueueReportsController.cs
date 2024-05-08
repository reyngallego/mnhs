using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using WebApplication3.Models;
using System.Globalization;

namespace WebApplication3.Controllers
{
    public class QueueReportsController : ApiController
    {
        private readonly string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpGet]
        public IHttpActionResult GetQueueReports([FromUri] ReportFilterModel filters)
        {
            List<QueueReport> queueReports = new List<QueueReport>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Build the SQL query
                string query = "SELECT ReportID, QueueTicket, Department, CAST(DoneDate AS DATE) AS DoneDate, Timer FROM QueueReports WHERE";

                // Check if date filter is provided, if not, use today's date
                if (filters.DateFilter != null)
                {
                    query += " CAST(DoneDate AS DATE) = @DoneDate";
                }
                else
                {
                    query += " CAST(DoneDate AS DATE) = CAST(GETDATE() AS DATE)";
                }

                // Append WHERE clause to filter by departments if departmentFilters are provided
                if (filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                {
                    query += " AND Department IN (";

                    for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                    {
                        query += "@DeptParam" + i;
                        if (i < filters.DepartmentFilters.Count - 1)
                        {
                            query += ",";
                        }
                    }

                    query += ")";
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add DoneDate parameter if dateFilter is provided
                if (filters.DateFilter != null)
                {
                    command.Parameters.AddWithValue("@DoneDate", filters.DateFilter.Value.Date);
                }

                // Add Department parameters if departmentFilters are provided
                if (filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                {
                    for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@DeptParam" + i, filters.DepartmentFilters[i]);
                    }
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
        [HttpGet]
        [Route("api/QueueReports/TotalToday")]
        public IHttpActionResult GetTotalToday([FromUri] ReportFilterModel filters)
        {
            string totalToday = "N/A";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Build the SQL query to count total records for today with optional filters
                string query = "SELECT COUNT(*) FROM QueueReports WHERE CAST(DoneDate AS DATE) = CAST(GETDATE() AS DATE)";

                // Add filter conditions for departments if provided
                if (filters != null && filters.DateFilter != null)
                {
                    query = "SELECT COUNT(*) FROM QueueReports WHERE CAST(DoneDate AS DATE) = @DateFilter";

                    if (filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                    {
                        query += " AND Department IN (";

                        for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                        {
                            query += "@DeptParam" + i;
                            if (i < filters.DepartmentFilters.Count - 1)
                            {
                                query += ",";
                            }
                        }

                        query += ")";
                    }
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add DateFilter parameter if provided
                if (filters != null && filters.DateFilter != null)
                {
                    command.Parameters.AddWithValue("@DateFilter", filters.DateFilter.Value.Date);
                }

                // Add Department parameters if departmentFilters are provided
                if (filters != null && filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                {
                    for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@DeptParam" + i, filters.DepartmentFilters[i]);
                    }
                }

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalToday = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(totalToday);
        }


        [HttpGet]
        [Route("api/QueueReports/AverageTimer")]
        public IHttpActionResult GetAverageTimer([FromUri] ReportFilterModel filters)
        {
            double totalTimerTicks = 0;
            int totalCount = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Build the SQL query to fetch timer values
                string query = "SELECT Timer FROM QueueReports WHERE";

                // Check if date filter is provided, if not, use today's date
                if (filters.DateFilter != null)
                {
                    query += " CAST(DoneDate AS DATE) = @DoneDate";
                }
                else
                {
                    // Use today's date in the SQL query
                    query += " CAST(DoneDate AS DATE) = CAST(GETDATE() AS DATE)";
                }

                // Append WHERE clause to filter by departments if departmentFilters are provided
                if (filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                {
                    query += " AND Department IN (";

                    for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                    {
                        query += "@DeptParam" + i;
                        if (i < filters.DepartmentFilters.Count - 1)
                        {
                            query += ",";
                        }
                    }

                    query += ")";
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add DoneDate parameter if dateFilter is provided
                if (filters.DateFilter != null)
                {
                    command.Parameters.AddWithValue("@DoneDate", filters.DateFilter.Value.Date);
                }

                // Add Department parameters if departmentFilters are provided
                if (filters.DepartmentFilters != null && filters.DepartmentFilters.Count > 0)
                {
                    for (int i = 0; i < filters.DepartmentFilters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@DeptParam" + i, filters.DepartmentFilters[i]);
                    }
                }

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Inside the while loop where you're reading the Timer values
                    while (reader.Read())
                    {
                        try
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("Timer")))
                            {
                                TimeSpan timerValue = TimeSpan.ParseExact(reader["Timer"].ToString(), @"hh\:mm\:ss", CultureInfo.InvariantCulture);
                                totalTimerTicks += timerValue.Ticks;
                                totalCount++;
                            }
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

            // Calculate the average timer
            TimeSpan averageTimer = TimeSpan.FromTicks((long)(totalTimerTicks / totalCount));

            // Format the average timer as a string
            string formattedAverageTimer = $"{averageTimer.Hours:D2}:{averageTimer.Minutes:D2}:{averageTimer.Seconds:D2}";

            return Ok(formattedAverageTimer);
        }

        [HttpGet]
        [Route("api/QueueReports/TotalRegistrar")]
        public IHttpActionResult GetTotalRegistrar([FromUri] ReportFilterModel filters)
        {
            return GetTotalForDepartment("Registrar", filters);
        }

        [HttpGet]
        [Route("api/QueueReports/TotalCashier")]
        public IHttpActionResult GetTotalCashier([FromUri] ReportFilterModel filters)
        {
            return GetTotalForDepartment("Cashier", filters);
        }

        [HttpGet]
        [Route("api/QueueReports/TotalStudentAffairs")]
        public IHttpActionResult GetTotalStudentAffairs([FromUri] ReportFilterModel filters)
        {
            return GetTotalForDepartment("STUDENTAFFAIRSANDSERVICES", filters);
        }

        [HttpGet]
        [Route("api/QueueReports/TotalDirector")]
        public IHttpActionResult GetTotalDirector([FromUri] ReportFilterModel filters)
        {
            return GetTotalForDepartment("Director", filters);
        }

        // Common method to fetch total for a specific department
        private IHttpActionResult GetTotalForDepartment(string department, ReportFilterModel filters)
        {
            string total = "N/A";

            // Build the SQL query to count total records for today with optional filters
            string query = $"SELECT COUNT(*) FROM QueueReports WHERE Department = @Department";

            // Add filter conditions for date if provided
            if (filters != null && filters.DateFilter != null)
            {
                query += " AND CAST(DoneDate AS DATE) = @DateFilter";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Department", department);

                // Add DateFilter parameter if provided
                if (filters != null && filters.DateFilter != null)
                {
                    command.Parameters.AddWithValue("@DateFilter", filters.DateFilter.Value.Date);
                }

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return Ok(total);
        }
    }
}
