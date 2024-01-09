using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication3.Models;
using System.Collections.Generic;

namespace WebApplication3.Controllers
{

    public class VideoController : ApiController
    {
        private readonly string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        [HttpGet]
        [Route("api/video/getvideos")]
        public IHttpActionResult GetVideos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT videoid, videoname, videodata, description FROM videos", connection))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        var videos = new List<Video>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int videoId = Convert.ToInt32(dt.Rows[i]["videoid"]);
                            string videoName = dt.Rows[i]["videoname"].ToString();
                            byte[] videoData = (byte[])dt.Rows[i]["videodata"];
                            string description = dt.Rows[i]["description"].ToString();
                            string videoUrl = $"/api/video/playvideo/{videoId}";

                            videos.Add(new Video
                            {
                                VideoId = videoId,
                                VideoName = videoName,
                                VideoData = videoData,
                                Description = description,
                                VideoUrl = videoUrl
                            });
                        }

                        return Ok(videos);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/video/uploadvideo")]
        public async Task<IHttpActionResult> UploadVideo()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var videoName = httpRequest.Form["videoName"];
                var description = httpRequest.Form["description"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var fileType = Path.GetExtension(postedFile.FileName);

                    using (var stream = new MemoryStream())
                    {
                        postedFile.InputStream.CopyTo(stream);
                        byte[] videoData = stream.ToArray();

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO videos (videoname, videodata, filetype, description) VALUES (@videoname, @videodata, @filetype, @description)", connection))
                            {
                                cmd.Parameters.AddWithValue("@videoname", videoName);
                                cmd.Parameters.AddWithValue("@videodata", videoData);
                                cmd.Parameters.AddWithValue("@filetype", fileType);
                                cmd.Parameters.AddWithValue("@description", description);

                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                return Ok("Video uploaded successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/video/deletevideo/{videoId}")]
        public IHttpActionResult DeleteVideo(int videoId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM videos WHERE videoid = @videoid", connection))
                    {
                        cmd.Parameters.AddWithValue("@videoid", videoId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok($"Video with ID {videoId} deleted successfully.");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/video/playvideo/{videoId}")]
        public IHttpActionResult PlayVideo(int videoId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT videodata FROM videos WHERE videoid = @videoid", connection))
                    {
                        cmd.Parameters.AddWithValue("@videoid", videoId);

                        var videoData = cmd.ExecuteScalar() as byte[];

                        if (videoData != null)
                        {
                            return Ok(videoData);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
