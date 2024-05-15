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
    private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

   
    [HttpGet]
    [Route("api/attendance/getattendancedata")]
    public HttpResponseMessage GetAttendanceData()
    {
        try
        {
            List<AttendanceData> attendanceData = new List<AttendanceData>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT LRN, FirstName, LastName, TimeIn, TimeOut, Date, Grade, Section FROM StudentAttendance";
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
            // Check if LRN exists in the mnhs table
            if (!IsLRNExistInMnhs(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN does not exist in mnhs table.");
            }

            // Check if LRN has already signed in
            if (IsLRNAlreadySignedIn(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have already signed in.");
            }

            // Create a connection to the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Create a SQL command to insert LRN, current time, and today's date
                string query = "INSERT INTO StudentAttendance (LRN, TimeIn, Date) VALUES (@LRN, GETDATE(), CONVERT(date, GETDATE()))";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LRN", lrn); // Add LRN as parameter

                    // Execute the command
                    cmd.ExecuteNonQuery();
                }
            }

            // Return success response
            return Request.CreateResponse(HttpStatusCode.OK, "Attendance entered successfully");
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Unique constraint violation error number
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN already exists in the attendance records.");
            }
            else
            {
                // Return error response for other exceptions
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
    // Method to check if LRN has already signed in
    private bool IsLRNAlreadySignedIn(string lrn)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "SELECT COUNT(1) FROM StudentAttendance WHERE LRN = @LRN";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@LRN", lrn);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }


    // Method to check if LRN exists in the mnhs table
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
            // Check if LRN exists in the mnhs table
            if (!IsLRNExistInMnhs(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN does not exist in mnhs table.");
            }

            // Check if LRN has not signed in yet
            if (!IsLRNAlreadySignedIn(lrn))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have not signed in yet.");
            }

            // Create a connection to the database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Create a SQL command to update TimeOut for the specified LRN
                string query = "UPDATE StudentAttendance SET TimeOut = GETDATE() WHERE LRN = @LRN AND TimeOut IS NULL";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LRN", lrn); // Add LRN as parameter

                    // Execute the command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if no rows were affected (LRN already timed out)
                    if (rowsAffected == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have already signed out.");
                    }
                }
            }

            // Return success response
            return Request.CreateResponse(HttpStatusCode.OK, "Attendance left successfully");
        }
        catch (Exception ex)
        {
            // Return error response if an exception occurs
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