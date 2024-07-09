using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class StudentController : ApiController
    {
        // Connection string to your database
        private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

        // Model to hold the student data
        public class Student
        {
            public string LRN { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int TotalPresent { get; set; }
            public int TotalAbsent { get; set; }
            public List<AttendanceRecord> AttendanceRecords { get; set; }
        }

        // Model to hold the attendance record data
        public class AttendanceRecord
        {
            public DateTime Date { get; set; }
            public string Day { get; set; }
            public string Status { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
        }

        [HttpGet]
        [Route("api/student/search")]
        public IHttpActionResult SearchStudent(string lrn)
        {
            DateTime startDate = new DateTime(2024, 6, 13);
            DateTime endDate = DateTime.Now; // Set endDate to the present date
            Student student = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to check if the student exists
                string studentQuery = @"
                    SELECT LRN, FirstName, LastName
                    FROM AttendanceReport
                    WHERE LRN = @LRN";

                using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                {
                    studentCommand.Parameters.AddWithValue("@LRN", lrn);

                    using (SqlDataReader studentReader = studentCommand.ExecuteReader())
                    {
                        if (studentReader.Read())
                        {
                            student = new Student
                            {
                                LRN = studentReader["LRN"].ToString(),
                                FirstName = studentReader["FirstName"].ToString(),
                                LastName = studentReader["LastName"].ToString(),
                                TotalPresent = 0,
                                TotalAbsent = 0,
                                AttendanceRecords = new List<AttendanceRecord>()
                            };
                        }
                        else
                        {
                            return NotFound(); // Student not found
                        }
                    }
                }

                // Query to get student attendance records within the date range
                string query = @"
                    SELECT 
                        AttendanceReportID, LRN, AttendanceDate, DayOfWeek, AttendanceStatus, TimeIn, TimeOut
                    FROM 
                        AttendanceReport
                    WHERE 
                        LRN = @LRN AND
                        AttendanceDate BETWEEN @StartDate AND @EndDate";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LRN", lrn);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Dictionary<DateTime, bool> attendanceMap = new Dictionary<DateTime, bool>();

                        while (reader.Read())
                        {
                            DateTime attendanceDate = Convert.ToDateTime(reader["AttendanceDate"]);
                            string status = reader["AttendanceStatus"].ToString();
                            if (status.Equals("Present", StringComparison.OrdinalIgnoreCase))
                            {
                                student.TotalPresent++;
                                attendanceMap[attendanceDate] = true;
                            }

                            student.AttendanceRecords.Add(new AttendanceRecord
                            {
                                Date = attendanceDate,
                                Day = reader["DayOfWeek"].ToString(),
                                Status = status,
                                TimeIn = reader["TimeIn"].ToString(),
                                TimeOut = reader["TimeOut"].ToString()
                            });
                        }

                        // Iterate over the date range to count weekdays
                        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if (!attendanceMap.ContainsKey(date))
                                {
                                    student.TotalAbsent++;
                                }
                            }
                        }
                    }
                }
            }

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}
