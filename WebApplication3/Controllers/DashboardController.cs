using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class DashboardController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

        [HttpGet]
        [Route("api/generateid/GetDashboardData")]
        public HttpResponseMessage GetDashboardData()
        {
            try
            {
                var studentSummary = GetStudentData();
                var attendanceSummary = GetAttendanceData();
                var notTimedInCount = GetNotTimedInCount();

                var dashboardData = new
                {
                    StudentSummary = studentSummary,
                    AttendanceSummary = attendanceSummary,
                    NotTimedInCount = notTimedInCount,
                };

                return Request.CreateResponse(HttpStatusCode.OK, dashboardData);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private StudentDataViewModel GetStudentData()
        {
            var studentData = new StudentDataViewModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        Grade,
                        COUNT(*) AS TotalStudents,
                        SUM(CASE WHEN Sex = 'Male' THEN 1 ELSE 0 END) AS MaleStudents,
                        SUM(CASE WHEN Sex = 'Female' THEN 1 ELSE 0 END) AS FemaleStudents
                    FROM mnhs
                    WHERE Grade IN ('Grade 7', 'Grade 8', 'Grade 9', 'Grade 10')
                    GROUP BY Grade";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    studentData.Grades.Add(new GradeData
                    {
                        Grade = reader["Grade"].ToString(),
                        TotalStudents = reader.GetInt32(reader.GetOrdinal("TotalStudents")),
                        MaleStudents = reader.GetInt32(reader.GetOrdinal("MaleStudents")),
                        FemaleStudents = reader.GetInt32(reader.GetOrdinal("FemaleStudents"))
                    });
                }
            }
            return studentData;
        }

        private List<AttendanceDataViewModel> GetAttendanceData()
        {
            var attendanceData = new List<AttendanceDataViewModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        COUNT(*) AS TotalRecords,
                        SUM(CASE WHEN TimeIn IS NOT NULL THEN 1 ELSE 0 END) AS TotalTimeIn,
                        SUM(CASE WHEN TimeOut IS NOT NULL THEN 1 ELSE 0 END) AS TotalTimeOut
                    FROM AttendanceReport
                    WHERE CAST(AttendanceDate AS DATE) = CAST(GETDATE() AS DATE)";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    attendanceData.Add(new AttendanceDataViewModel
                    {
                        TotalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords")),
                        TotalTimeIn = reader.GetInt32(reader.GetOrdinal("TotalTimeIn")),
                        TotalTimeOut = reader.GetInt32(reader.GetOrdinal("TotalTimeOut"))
                    });
                }
            }
            return attendanceData;
        }

        private int GetNotTimedInCount()
        {
            int totalEnrolledStudents = GetTotalEnrolledStudents();
            int totalTimedInStudents = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        COUNT(DISTINCT LRN) AS TotalTimedIn
                    FROM AttendanceReport
                    WHERE CAST(AttendanceDate AS DATE) = CAST(GETDATE() AS DATE)
                        AND TimeIn IS NOT NULL";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    totalTimedInStudents = reader.GetInt32(reader.GetOrdinal("TotalTimedIn"));
                }
            }

            return totalEnrolledStudents - totalTimedInStudents;
        }



        private int GetTotalEnrolledStudents()
        {
            int totalEnrolledStudents = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) AS TotalEnrolledStudents
                    FROM mnhs";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    totalEnrolledStudents = reader.GetInt32(reader.GetOrdinal("TotalEnrolledStudents"));
                }
            }
            return totalEnrolledStudents;
        }

        public class StudentDataViewModel
        {
            public List<GradeData> Grades { get; set; } = new List<GradeData>();
        }

        public class GradeData
        {
            public string Grade { get; set; }
            public int TotalStudents { get; set; }
            public int MaleStudents { get; set; }
            public int FemaleStudents { get; set; }
        }

        public class AttendanceDataViewModel
        {
            public int TotalRecords { get; set; }
            public int TotalTimeIn { get; set; }
            public int TotalTimeOut { get; set; }
        }
    }
}
