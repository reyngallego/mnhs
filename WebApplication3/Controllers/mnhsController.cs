using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class mnhsController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

        [HttpPost]
        [Route("api/mnhs/InsertData")]
        public HttpResponseMessage InsertData([FromBody] FormData formData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO mnhs (LRN, FirstName, MiddleName, LastName, NameExtension, Grade, Section, Birthdate, Sex, Address, ParentFullName, ParentContact, Adviser) " +
                                   "VALUES (@LRN, @FirstName, @MiddleName, @LastName, @NameExtension, @Grade, @Section, @Birthdate, @Sex, @Address, @ParentFullName, @ParentContact, @Adviser)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LRN", formData.LRN);
                        command.Parameters.AddWithValue("@FirstName", formData.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", formData.MiddleName);
                        command.Parameters.AddWithValue("@LastName", formData.LastName);
                        command.Parameters.AddWithValue("@NameExtension", formData.NameExtension);
                        command.Parameters.AddWithValue("@Grade", formData.Grade);
                        command.Parameters.AddWithValue("@Section", formData.Section);
                        command.Parameters.AddWithValue("@Birthdate", formData.Birthdate);
                        command.Parameters.AddWithValue("@Sex", formData.Sex);
                        command.Parameters.AddWithValue("@Address", formData.Address);
                        command.Parameters.AddWithValue("@ParentFullName", formData.ParentFullName);
                        command.Parameters.AddWithValue("@ParentContact", formData.ParentContact);
                        command.Parameters.AddWithValue("@Adviser", formData.Adviser);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Student added successfully.");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to add student.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation error number
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "LRN already exists.");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }


        // Method to check if LRN already exists in mnhs table
        private bool IsLRNAlreadyExists(string lrn)
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

        [HttpGet]
        [Route("api/mnhs/GetStudents")]
        public HttpResponseMessage GetStudents()
        {
            try
            {
                List<FormData> students = new List<FormData>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT LRN, FirstName, MiddleName, LastName, Grade, Section, Adviser FROM mnhs";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FormData student = new FormData
                                {
                                    LRN = reader["LRN"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    MiddleName = reader["MiddleName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Grade = reader["Grade"].ToString(),
                                    Section = reader["Section"].ToString(),
                                    Adviser = reader["Adviser"].ToString()
                                };
                                students.Add(student);
                            }
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, students);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/mnhs/GetStudentByLRN")]
        public HttpResponseMessage GetStudentByLRN(string LRN)
        {
            try
            {
                FormData student = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT LRN, FirstName, MiddleName, LastName, Grade, Section, Adviser, NameExtension, Birthdate, Sex, Address, ParentFullName, ParentContact " +
                                   "FROM mnhs " +
                                   "WHERE LRN = @LRN";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LRN", LRN);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student = new FormData
                                {
                                    LRN = reader["LRN"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    MiddleName = reader["MiddleName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    NameExtension = reader["NameExtension"].ToString(),
                                    Grade = reader["Grade"].ToString(),
                                    Section = reader["Section"].ToString(),
                                    Birthdate = Convert.ToDateTime(reader["Birthdate"]),
                                    Sex = reader["Sex"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    ParentFullName = reader["ParentFullName"].ToString(),
                                    ParentContact = reader["ParentContact"].ToString(),
                                    Adviser = reader["Adviser"].ToString()
                                };
                            }
                        }
                    }
                }

                if (student != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, student);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("api/mnhs/UpdateStudent")]
        public HttpResponseMessage UpdateStudent([FromBody] FormData formData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE mnhs SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, " +
                                   "NameExtension = @NameExtension, Grade = @Grade, Section = @Section, " +
                                   "Birthdate = @Birthdate, Sex = @Sex, Address = @Address, " +
                                   "ParentFullName = @ParentFullName, ParentContact = @ParentContact, Adviser = @Adviser " +
                                   "WHERE LRN = @LRN";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", formData.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", formData.MiddleName);
                        command.Parameters.AddWithValue("@LastName", formData.LastName);
                        command.Parameters.AddWithValue("@NameExtension", formData.NameExtension);
                        command.Parameters.AddWithValue("@Grade", formData.Grade);
                        command.Parameters.AddWithValue("@Section", formData.Section);
                        command.Parameters.AddWithValue("@Birthdate", formData.Birthdate);
                        command.Parameters.AddWithValue("@Sex", formData.Sex);
                        command.Parameters.AddWithValue("@Address", formData.Address);
                        command.Parameters.AddWithValue("@ParentFullName", formData.ParentFullName);
                        command.Parameters.AddWithValue("@ParentContact", formData.ParentContact);
                        command.Parameters.AddWithValue("@Adviser", formData.Adviser);
                        command.Parameters.AddWithValue("@LRN", formData.LRN);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Student updated successfully.");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpDelete]
        [Route("api/mnhs/DeleteStudent")]
        public HttpResponseMessage DeleteStudent(string LRN)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Disable foreign key constraints
                    DisableForeignKeyConstraints(connection);

                    // Delete the student
                    string query = "DELETE FROM mnhs WHERE LRN = @LRN";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LRN", LRN);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Re-enable foreign key constraints
                            EnableForeignKeyConstraints(connection);
                            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
                        }
                        else
                        {
                            // Re-enable foreign key constraints
                            EnableForeignKeyConstraints(connection);
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Method to disable foreign key constraints
        private void DisableForeignKeyConstraints(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'", connection);
            command.ExecuteNonQuery();
        }

        // Method to enable foreign key constraints
        private void EnableForeignKeyConstraints(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'", connection);
            command.ExecuteNonQuery();
        }

        public class FormData
        {
            public string LRN { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string NameExtension { get; set; }
            public string Grade { get; set; }
            public string Section { get; set; }
            public DateTime Birthdate { get; set; }
            public string Sex { get; set; }
            public string Address { get; set; }
            public string ParentFullName { get; set; }
            public string ParentContact { get; set; }
            public string Adviser { get; set; }
        }
    }
}