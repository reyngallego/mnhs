using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class GenerateIdController : Controller
    {
        private string connectionString = "Data Source=ULUPONG-DESKTOP\\SQLEXPRESS;Initial Catalog=StudentRecords;Integrated Security=True";

        // GET: GenerateId
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/StudentRecords/GetStudents")]
        public JsonResult GetStudents()
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
                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public class FormData
        {
            public string LRN { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Grade { get; set; }
            public string Section { get; set; }
            public string Adviser { get; set; }
        }

        public class GenerateIdData
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