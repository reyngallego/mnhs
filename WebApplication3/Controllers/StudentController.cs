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

        // Endpoint to fetch student details and attendance records by LRN
        [HttpGet]
        [Route("api/student/search")]
        public IHttpActionResult GetStudentByLRN(string lrn)
        {
            try
            {
                Student student = null;
                List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Fetch student details
                    string studentQuery = "SELECT * FROM AttendanceReport WHERE LRN = @LRN";
                    using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                    {
                        studentCommand.Parameters.AddWithValue("@LRN", lrn);
                        SqlDataReader reader = studentCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            student = new Student
                            {
                                LRN = reader["LRN"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                TotalPresent = reader["TotalPresent"] != DBNull.Value ? (int)reader["TotalPresent"] : 0,
                                TotalAbsent = reader["TotalAbsent"] != DBNull.Value ? (int)reader["TotalAbsent"] : 0,
                                AbsentDate = reader["AbsentDate"] != DBNull.Value ? (DateTime)reader["AbsentDate"] : (DateTime?)null
                            };
                        }
                        reader.Close();
                    }

                    if (student == null)
                    {
                        return NotFound(); // Student not found
                    }

                    // Fetch attendance records from AttendanceReport table
                    string attendanceQuery = "SELECT AttendanceDate, Daysofweek, AttendanceStatus, TimeIn, TimeOut FROM AttendanceReport WHERE LRN = @LRN";
                    using (SqlCommand attendanceCommand = new SqlCommand(attendanceQuery, connection))
                    {
                        attendanceCommand.Parameters.AddWithValue("@LRN", lrn);
                        SqlDataReader reader = attendanceCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            var record = new AttendanceRecord
                            {
                                Date = reader["AttendanceDate"] != DBNull.Value ? (DateTime)reader["AttendanceDate"] : DateTime.MinValue,
                                Day = reader["DayOfWeek"].ToString(),
                                Status = reader["AttendanceStatus"].ToString(),
                                TimeIn = reader["TimeIn"] != DBNull.Value ? (TimeSpan)reader["TimeIn"] : (TimeSpan?)null,
                                TimeOut = reader["TimeOut"] != DBNull.Value ? (TimeSpan)reader["TimeOut"] : (TimeSpan?)null
                            };
                            attendanceRecords.Add(record);
                        }
                        reader.Close();
                    }
                }

                return Ok(new { Student = student, AttendanceRecords = attendanceRecords });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Internal server error
            }
        }
    }

    public class AttendanceRecord
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string Status { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
    }
}

public class Student
{
    public string LRN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TotalPresent { get; set; }
    public int TotalAbsent { get; set; }
    public DateTime? AbsentDate { get; set; }
}

