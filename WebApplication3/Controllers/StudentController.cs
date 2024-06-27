using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class StudentController : ApiController
    {
        // Connection string to your database
        private string connectionString = "Data Source=ULUPONG-DESKTOP\\SQLEXPRESS;Initial Catalog=moonwalk;Integrated Security=True";

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
            Student student = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to get student information and attendance records
                string query = @"
                    SELECT 
                        LRN, FirstName, LastName, TotalPresent, TotalAbsent,
                        AttendanceDate, DayOfWeek, AttendanceStatus, TimeIn, TimeOut
                    FROM 
                        AttendanceReport
                    WHERE 
                        LRN = @LRN";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LRN", lrn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (student == null)
                            {
                                student = new Student
                                {
                                    LRN = reader["LRN"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    TotalPresent = Convert.ToInt32(reader["TotalPresent"]),
                                    TotalAbsent = Convert.ToInt32(reader["TotalAbsent"]),
                                    AttendanceRecords = new List<AttendanceRecord>()
                                };
                            }

                            student.AttendanceRecords.Add(new AttendanceRecord
                            {
                                Date = Convert.ToDateTime(reader["AttendanceDate"]),
                                Day = reader["DayOfWeek"].ToString(),
                                Status = reader["AttendanceStatus"].ToString(),
                                TimeIn = reader["TimeIn"].ToString(),
                                TimeOut = reader["TimeOut"].ToString()
                            });
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
