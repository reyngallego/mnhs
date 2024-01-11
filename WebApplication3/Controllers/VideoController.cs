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

                            // Handle DBNull for videodata
                            byte[] videoData = DBNull.Value.Equals(dt.Rows[i]["videodata"]) ? null : (byte[])dt.Rows[i]["videodata"];

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

                        // Insert video information into the database without saving to file
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
        public HttpResponseMessage PlayVideo(int videoId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Get the list of video IDs from the videos table
                    List<int> videoIds = new List<int>();
                    using (SqlCommand videoIdsCmd = new SqlCommand("SELECT videoid FROM videos", connection))
                    {
                        using (SqlDataReader reader = videoIdsCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                videoIds.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    // Find the index of the current videoId in the list
                    int currentIndex = videoIds.IndexOf(videoId);

                    // Calculate the index of the next video (if not found, set to 0)
                    int nextIndex = (currentIndex + 1) % videoIds.Count;

                    // Get the next videoId
                    int nextVideoId = videoIds[nextIndex];

                    // Retrieve the videodata for the next video
                    using (SqlCommand cmd = new SqlCommand("SELECT videodata FROM videos WHERE videoid = @videoid", connection))
                    {
                        cmd.Parameters.AddWithValue("@videoid", nextVideoId);

                        var videoData = cmd.ExecuteScalar() as byte[];

                        if (videoData != null)
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ByteArrayContent(videoData)
                            };
                            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("video/mp4");

                            return response;
                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.NotFound);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        // Endpoint to handle the selection of a video
        [HttpPost]
        [Route("api/video/selectvideo")]
        public IHttpActionResult SelectVideo([FromBody] VideoSelectionRequest request)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // First, select the video details from the original table
                    using (SqlCommand selectCmd = new SqlCommand("SELECT videoid, videoname, videodata, description FROM videos WHERE videoid = @videoid", connection))
                    {
                        selectCmd.Parameters.AddWithValue("@videoid", request.VideoId);

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(selectCmd);
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            int videoId = Convert.ToInt32(dt.Rows[0]["videoid"]);
                            string videoName = dt.Rows[0]["videoname"].ToString();

                            // Handle DBNull for videodata
                            byte[] videoData = DBNull.Value.Equals(dt.Rows[0]["videodata"]) ? null : (byte[])dt.Rows[0]["videodata"];

                            string description = dt.Rows[0]["description"].ToString();

                            // Insert the selected video into the new table
                            using (SqlCommand insertCmd = new SqlCommand("INSERT INTO SelectedVideos (videoid, videoname, videodata, description) VALUES (@videoid, @videoname, @videodata, @description)", connection))
                            {
                                insertCmd.Parameters.AddWithValue("@videoid", videoId);
                                insertCmd.Parameters.AddWithValue("@videoname", videoName);
                                insertCmd.Parameters.AddWithValue("@videodata", videoData);
                                insertCmd.Parameters.AddWithValue("@description", description);

                                int rowsAffected = insertCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Log the selected video details
                                    Console.WriteLine($"Selected Video inserted into SelectedVideos table: {videoName}");

                                    // Return the selected video details
                                    return Ok(new
                                    {
                                        VideoId = videoId,
                                        VideoName = videoName,
                                        Description = description
                                        // Add other properties as needed
                                    });
                                }
                                else
                                {
                                    return InternalServerError(new Exception("Failed to insert selected video into SelectedVideos table."));
                                }
                            }
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
        [Route("api/video/getselectedvideos")]
        public IHttpActionResult GetSelectedVideos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT videoid, videoname, videodata, description FROM SelectedVideos", connection))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        var selectedVideos = new List<Video>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int videoId = Convert.ToInt32(dt.Rows[i]["videoid"]);
                            string videoName = dt.Rows[i]["videoname"].ToString();

                            // Handle DBNull for videodata
                            byte[] videoData = DBNull.Value.Equals(dt.Rows[i]["videodata"]) ? null : (byte[])dt.Rows[i]["videodata"];

                            string description = dt.Rows[i]["description"].ToString();
                            string videoUrl = $"/api/video/playvideo/{videoId}";

                            selectedVideos.Add(new Video
                            {
                                VideoId = videoId,
                                VideoName = videoName,
                                VideoData = videoData,
                                Description = description,
                                VideoUrl = videoUrl
                            });
                        }

                        return Ok(selectedVideos);
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