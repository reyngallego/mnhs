using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class ImageController : ApiController
    {
        private readonly string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpPost]
        [Route("api/images/upload")]
        public IHttpActionResult UploadImage()
        {
            try
            {
                var file = HttpContext.Current.Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        var imageData = ms.ToArray();

                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Assuming 'users' table has columns 'id' and 'image'
                            string query = "INSERT INTO users (image) VALUES (@ImageData);";
                            using (var command = new SqlCommand(query, connection))
                            {
                                command.Parameters.Add("@ImageData", System.Data.SqlDbType.VarBinary).Value = imageData;
                                command.ExecuteNonQuery();
                            }
                        }

                        return Ok(new { Message = "Image uploaded successfully" });
                    }
                }
                else
                {
                    return BadRequest("File is empty");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Internal server error: {ex.Message}"));
            }
        }
    }
}
