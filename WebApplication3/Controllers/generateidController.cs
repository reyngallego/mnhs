using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using static WebApplication3.Controllers.mnhsController;

namespace WebApplication3.Controllers
{
    public class generateidController : ApiController
    {
        private string connectionString = "Data Source=DESKTOP-1K0L57N;Initial Catalog=capstone;Integrated Security=True";

        [HttpGet]
        [Route("api/generateid/GetStudents")]
        public HttpResponseMessage GetStudents(string searchTerm = "", string sortColumn = "LRN", string sortOrder = "ASC")
        {
            try
            {
                List<FormData> students = new List<FormData>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT LRN, FirstName, MiddleName, LastName, Grade, Section, Adviser FROM mnhs";

                    // Add search functionality
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " WHERE " +
                                 "LOWER(FirstName) LIKE @SearchTerm OR " +
                                 "LOWER(MiddleName) LIKE @SearchTerm OR " +
                                 "LOWER(LastName) LIKE @SearchTerm OR " +
                                 "LOWER(Grade) LIKE @SearchTerm OR " +
                                 "LOWER(Section) LIKE @SearchTerm OR " +
                                 "LOWER(Adviser) LIKE @SearchTerm";
                    }

                    // Add sorting
                    query += $" ORDER BY {sortColumn} {sortOrder}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");
                        }

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
        [Route("api/generateid/GetStudentByLRN")]
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
    }
}
