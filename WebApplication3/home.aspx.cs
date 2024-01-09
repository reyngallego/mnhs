using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Services;
using WebApplication3;
namespace WebApplication3
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the username from the session
                string username = Session["LoggedInUsername"] as string;

                // Get the department from the session
                string loggedInDepartment = UserUtility.GetUserDepartment(username);

                // Store the department in the hidden field
                hdnLoggedInDepartment.Value = loggedInDepartment;

                if (!string.IsNullOrEmpty(username))
                {
                    LoadUserData(username);
                }
                else
                {
                    // Redirect to the login page if the username is not found in the session
                    Response.Redirect("login.aspx");
                }
            }
        }

        protected bool ShowSuccessMessage
        {
            get
            {
                if (ViewState["ShowSuccessMessage"] != null)
                {
                    return (bool)ViewState["ShowSuccessMessage"];
                }
                return false;
            }
            set
            {
                ViewState["ShowSuccessMessage"] = value;
            }
        }
        private DataTable GetUserDataFromDatabase(string username)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string query = "SELECT firstName, lastName, department, image FROM users WHERE username = @Username"; // Add the WHERE clause

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter for the username
                    command.Parameters.AddWithValue("@Username", username);

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    connection.Open();
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        protected void LoadUserData(string username)
        {
            DataTable userData = GetUserDataFromDatabase(username);

            if (userData != null && userData.Rows.Count > 0)
            {
                string firstName = userData.Rows[0]["FirstName"].ToString();
                string lastName = userData.Rows[0]["LastName"].ToString();
                string fullName = firstName + " " + lastName;
                lblFirstName.Text = fullName;
                lblDepartment.Text = userData.Rows[0]["Department"].ToString();
                lblDate.Text = DateTime.Now.ToShortDateString();
                lblTime.Text = DateTime.Now.ToShortTimeString();

                // Convert binary image data to Base64 string
                byte[] imageBytes = (byte[])userData.Rows[0]["image"];
                string base64String = Convert.ToBase64String(imageBytes);

                // Set the Base64-encoded image string as the ImageUrl
                imgUser.ImageUrl = "data:image/jpeg;base64," + base64String; // Assuming the image format is JPEG

                // Fetch and display queueticket data
                string department = lblDepartment.Text; // Use the department from the label

                string queueticket = GetQueueTicketForToday(department);

                if (!string.IsNullOrEmpty(queueticket))
                {
                    // Display queueticket in the label
                    lblQueueTicket.Text = queueticket;
                }
                else
                {
                    lblQueueTicket.Text = "There's no Queue Ticket Today";
                }
            }
        }

        private string GetQueueTicketForToday(string department)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a 'student' table with 'queueticket', 'department', 'queuedate', and 'IsDone' columns
            string query = "SELECT MIN(queueticket) AS FirstQueueTicket FROM student WHERE department = @Department AND CONVERT(date, queuedate) = CONVERT(date, GETDATE()) AND IsDone = 0";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }

            return ""; // Return empty string if no queueticket found
        }


        protected void btnPreviousTicket_Click(object sender, EventArgs e)
        {
            // Your logic for the Previous button click
            string currentQueueTicket = lblQueueTicket.Text; // Assuming the current queueticket is displayed in a label
            string previousQueueTicket = GetPreviousQueueTicket(lblDepartment.Text, currentQueueTicket);

            if (!string.IsNullOrEmpty(previousQueueTicket))
            {
                // Display the previous queueticket
                lblQueueTicket.Text = previousQueueTicket;
                lblMessage.Text = ""; // Clear any previous messages
            }
            else
            {
                // Display a message for no previous queueticket
                lblMessage.Text = "No previous queueticket available";
            }
        }

        protected void btnNextTicket_Click(object sender, EventArgs e)
        {
            // Your logic for the Next button click
            string currentQueueTicket = lblQueueTicket.Text; // Assuming the current queueticket is displayed in a label
            string nextQueueTicket = GetNextQueueTicket(lblDepartment.Text, currentQueueTicket);

            if (!string.IsNullOrEmpty(nextQueueTicket))
            {
                // Display the next queueticket
                lblQueueTicket.Text = nextQueueTicket;
                lblMessage.Text = ""; // Clear any previous messages
            }
            else
            {
                // Display a message for no next queueticket available
                lblMessage.Text = "No next queueticket available";
            }
        }
        private string GetPreviousQueueTicket(string department, string currentQueueTicket)
        {
            string previousQueueTicket = "";
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a 'student' table with 'queueticket', 'department', 'queuedate', and 'IsDone' columns
            string query = "SELECT TOP 1 queueticket FROM student WHERE department = @Department AND CONVERT(date, queuedate) = CONVERT(date, GETDATE()) AND IsDone = 0 AND (@CurrentQueueTicket IS NULL OR queueticket < @CurrentQueueTicket) ORDER BY queuedate DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@CurrentQueueTicket", string.IsNullOrEmpty(currentQueueTicket) ? (object)DBNull.Value : currentQueueTicket);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        previousQueueTicket = result.ToString();
                    }
                }
            }

            return previousQueueTicket;
        }
        private string GetNextQueueTicket(string department, string currentQueueTicket)
        {
            string nextQueueTicket = "";
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a 'student' table with 'queueticket', 'department', 'queuedate', and 'IsDone' columns
            string query = "SELECT TOP 1 queueticket FROM student WHERE department = @Department AND CONVERT(date, queuedate) = CONVERT(date, GETDATE()) AND IsDone = 0 AND (@CurrentQueueTicket IS NULL OR queueticket > @CurrentQueueTicket) ORDER BY queuedate ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@CurrentQueueTicket", string.IsNullOrEmpty(currentQueueTicket) ? (object)DBNull.Value : currentQueueTicket);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        nextQueueTicket = result.ToString();
                    }
                }
            }

            return nextQueueTicket;
        }


        protected void btnCallTicket_Click(object sender, EventArgs e)
        {
            try
            {
                string department = lblDepartment.Text; // Get the department
                string currentQueueTicket = lblQueueTicket.Text; // Get the current queueticket

                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Set the base URL of your API
                    client.BaseAddress = new Uri("http://localhost:65388");

                    // Create the payload to send to the API
                    var payload = new
                    {
                        Department = department,
                        CurrentQueueTicket = currentQueueTicket
                    };

                    // Convert the payload to JSON
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

                    // Make the HTTP POST request to your API endpoint
                    var response = client.PostAsync("http://localhost:65388/api/QueueTicket/PostQueueTicket", content).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Do something with the successful response, if needed
                        // For example, you can display a success message
                        Console.WriteLine("Queue ticket information posted successfully.");
                    }
                    else
                    {
                        // Handle the case where the request was not successful
                        // Display an error message or log the error
                        Console.WriteLine($"Error posting queue ticket information. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                // Display an error message or log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        protected void btnRecallTicket_Click(object sender, EventArgs e)
        {
            // Your logic for the Recall button click
        }

        protected void btnDoneTicket_Click(object sender, EventArgs e)
        {
            // Your logic for the Done button click

            string currentQueueTicket = lblQueueTicket.Text; // Assuming the current queueticket is displayed in a label
            string department = lblDepartment.Text; // Assuming the department is displayed in a label

            if (!string.IsNullOrEmpty(currentQueueTicket))
            {
                UpdateIsDoneForTicket(department, currentQueueTicket);
                lblMessage.Text = "Ticket marked as Done.";
            }
            else
            {
                lblMessage.Text = "No queueticket available to mark as Done.";
            }
        }

        private void UpdateIsDoneForTicket(string department, string queueticket)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Assuming you have a 'student' table with 'queueticket', 'department', 'queuedate', and 'IsDone' columns
            string query = "UPDATE student SET IsDone = 1 WHERE department = @Department AND queueticket = @QueueTicket";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@QueueTicket", queueticket);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
