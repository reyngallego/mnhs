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
        public HttpResponseMessage GetStudents(string searchTerm = "", string gradeFilter = "all", string sectionFilter = "all", string sortColumn = "LRN", string sortOrder = "ASC", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                List<FormData> students = new List<FormData>();
                int totalRecords = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // First, get the total number of records
                    string countQuery = "SELECT COUNT(*) FROM mnhs";
                    if (!string.IsNullOrEmpty(searchTerm) || gradeFilter != "all" || sectionFilter != "all")
                    {
                        countQuery += " WHERE ";
                        List<string> filters = new List<string>();

                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            filters.Add("LOWER(FirstName) LIKE @SearchTerm OR LOWER(MiddleName) LIKE @SearchTerm OR LOWER(LastName) LIKE @SearchTerm OR LOWER(Grade) LIKE @SearchTerm OR LOWER(Section) LIKE @SearchTerm OR LOWER(Adviser) LIKE @SearchTerm");
                        }
                        if (gradeFilter != "all")
                        {
                            filters.Add("Grade = @GradeFilter");
                        }
                        if (sectionFilter != "all")
                        {
                            filters.Add("Section = @SectionFilter");
                        }

                        countQuery += string.Join(" AND ", filters);
                    }

                    using (SqlCommand countCommand = new SqlCommand(countQuery, connection))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            countCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");
                        }
                        if (gradeFilter != "all")
                        {
                            countCommand.Parameters.AddWithValue("@GradeFilter", gradeFilter);
                        }
                        if (sectionFilter != "all")
                        {
                            countCommand.Parameters.AddWithValue("@SectionFilter", sectionFilter);
                        }

                        totalRecords = (int)countCommand.ExecuteScalar();
                    }

                    // Calculate the starting row for the page
                    int startRow = (pageNumber - 1) * pageSize;

                    // Main query to fetch the students with pagination
                    string query = "SELECT LRN, FirstName, MiddleName, LastName, Grade, Section, Adviser FROM mnhs";

                    if (!string.IsNullOrEmpty(searchTerm) || gradeFilter != "all" || sectionFilter != "all")
                    {
                        query += " WHERE ";
                        List<string> filters = new List<string>();

                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            filters.Add("LOWER(FirstName) LIKE @SearchTerm OR LOWER(MiddleName) LIKE @SearchTerm OR LOWER(LastName) LIKE @SearchTerm OR LOWER(Grade) LIKE @SearchTerm OR LOWER(Section) LIKE @SearchTerm OR LOWER(Adviser) LIKE @SearchTerm");
                        }
                        if (gradeFilter != "all")
                        {
                            filters.Add("Grade = @GradeFilter");
                        }
                        if (sectionFilter != "all")
                        {
                            filters.Add("Section = @SectionFilter");
                        }

                        query += string.Join(" AND ", filters);
                    }

                    // Add sorting and pagination
                    query += $" ORDER BY {sortColumn} {sortOrder} OFFSET @StartRow ROWS FETCH NEXT @PageSize ROWS ONLY";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");
                        }
                        if (gradeFilter != "all")
                        {
                            command.Parameters.AddWithValue("@GradeFilter", gradeFilter);
                        }
                        if (sectionFilter != "all")
                        {
                            command.Parameters.AddWithValue("@SectionFilter", sectionFilter);
                        }
                        command.Parameters.AddWithValue("@StartRow", startRow);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

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

                var response = new
                {
                    TotalRecords = totalRecords,
                    Students = students
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
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
