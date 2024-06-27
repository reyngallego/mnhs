using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;

public class AttendanceController : ApiController
{
    private string connectionString = "Data Source=ULUPONG-DESKTOP\\SQLEXPRESS;Initial Catalog=moonwalk;Integrated Security=True";

    [HttpGet]
    [Route("api/attendance/getattendancedata")]
    public HttpResponseMessage GetAttendanceData()
    {
        try
        {
            List<AttendanceData> attendanceData = new List<AttendanceData>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT LRN, FirstName, LastName, TimeIn, TimeOut, Date, Grade, Section " +
                               "FROM StudentAttendance " +
                               "WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AttendanceData data = new AttendanceData
                        {
                            LRN = reader["LRN"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            TimeIn = reader.IsDBNull(reader.GetOrdinal("TimeIn")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TimeIn")),
                            TimeOut = reader.IsDBNull(reader.GetOrdinal("TimeOut")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TimeOut")),
                            Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Date")),
                            Grade = reader["Grade"].ToString(),
                            Section = reader["Section"].ToString()
                        };
                        attendanceData.Add(data);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, attendanceData);
        }
        catch (Exception ex)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("api/attendance/enterattendance")]
    public HttpResponseMessage EnterAttendance([FromBody] string lrn)
    {
        try
        {
            if (!IsLRNExistInMnhs(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN does not exist in mnhs table.");
            }

            if (IsLRNAlreadySignedInToday(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have already signed in today.");
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Insert into StudentAttendance
                string query = "INSERT INTO StudentAttendance (LRN, TimeIn, Date) VALUES (@LRN, GETDATE(), CONVERT(date, GETDATE()))";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LRN", lrn);
                    cmd.ExecuteNonQuery();
                }

                // Retrieve student details from mnhs table
                string selectQuery = "SELECT FirstName, LastName FROM mnhs WHERE LRN = @LRN";
                string firstName = "", lastName = "";
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                {
                    selectCmd.Parameters.AddWithValue("@LRN", lrn);
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            firstName = reader["FirstName"].ToString();
                            lastName = reader["LastName"].ToString();
                        }
                    }
                }

                // Insert into AttendanceReport
                string reportQuery = "INSERT INTO AttendanceReport (LRN, FirstName, LastName, AttendanceDate, DayOfWeek, AttendanceStatus, TimeIn) " +
                                     "VALUES (@LRN, @FirstName, @LastName, CONVERT(date, GETDATE()), DATENAME(dw, GETDATE()), 'Present', GETDATE())";
                using (SqlCommand reportCmd = new SqlCommand(reportQuery, con))
                {
                    reportCmd.Parameters.AddWithValue("@LRN", lrn);
                    reportCmd.Parameters.AddWithValue("@FirstName", firstName);
                    reportCmd.Parameters.AddWithValue("@LastName", lastName);
                    reportCmd.ExecuteNonQuery();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Attendance entered successfully");
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have already signed in today.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }



    private bool IsLRNAlreadySignedInToday(string lrn)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT COUNT(1) FROM StudentAttendance WHERE LRN = @LRN AND CAST(Date AS DATE) = CAST(GETDATE() AS DATE)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@LRN", lrn);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }

    private bool IsLRNExistInMnhs(string lrn)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT COUNT(1) FROM mnhs WHERE LRN = @LRN";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@LRN", lrn);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }

    [HttpPost]
    [Route("api/attendance/leaveattendance")]
    public HttpResponseMessage LeaveAttendance([FromBody] string lrn)
    {
        try
        {
            if (!IsLRNExistInMnhs(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN does not exist in mnhs table.");
            }

            if (!IsLRNAlreadySignedInToday(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have not signed in yet today.");
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Update TimeOut in StudentAttendance
                string query = "UPDATE StudentAttendance SET TimeOut = GETDATE() WHERE LRN = @LRN AND TimeOut IS NULL AND CAST(Date AS DATE) = CAST(GETDATE() AS DATE)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LRN", lrn);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have already signed out.");
                    }
                }

                // Update TimeOut in AttendanceReport, increment TotalPresent, and set ReportMonth and ReportYear
                string reportQuery = @"
                UPDATE AttendanceReport 
                SET 
                    TimeOut = GETDATE(), 
                    TotalPresent = CASE WHEN AttendanceStatus = 'Present' THEN TotalPresent + 1 ELSE TotalPresent END,
                    ReportMonth = DATENAME(MONTH, GETDATE()),
                    ReportYear = YEAR(GETDATE())
                WHERE 
                    LRN = @LRN 
                    AND TimeIn IS NOT NULL 
                    AND TimeOut IS NULL 
                    AND CAST(AttendanceDate AS DATE) = CAST(GETDATE() AS DATE)";
                using (SqlCommand reportCmd = new SqlCommand(reportQuery, con))
                {
                    reportCmd.Parameters.AddWithValue("@LRN", lrn);
                    reportCmd.ExecuteNonQuery();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Attendance left successfully");
        }
        catch (Exception ex)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public class AttendanceData
    {
        public string LRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime Date { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
    }
}
