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
        protected static int elapsedTimeInSeconds = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Timer1.Interval = 1000;


                // Check if the user is logged in
                if (!UserUtility.IsUserLoggedIn())
                {
                    // Redirect to the login page if not logged in
                    Response.Redirect("login.aspx");
                    return; // Stop further execution
                }

                // User is logged in, continue with the page logic

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

                // Clear the queueticket label after processing
                lblQueueTicket.Text = "";
                lblMessage.Text = "";


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
                    else
                    {
                        // Handle the case when no queue ticket is found
                        // For example, you can return a specific message or throw an exception.
                        return "";
                    }
                }
            }
        }

        protected void btnPreviousTicket_Click(object sender, EventArgs e)
        {
            // Your logic for the Previous button click
            string currentQueueTicket = lblQueueTicket.Text; // Assuming the current queueticket is displayed in a label
            string previousQueueTicket = GetPreviousQueueTicket(lblDepartment.Text, currentQueueTicket);
            lblMessage.Text = "";
            elapsedTimeInSeconds = 0;
            Timer1.Enabled = false;
            lblservingTimeLabel.Visible = false;

            if (!string.IsNullOrEmpty(previousQueueTicket))
            {
                // Display the previous queueticket
                lblQueueTicket.Text = previousQueueTicket;
               
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
            lblMessage.Text = "";
            elapsedTimeInSeconds = 0;
            Timer1.Enabled = false;
            lblservingTimeLabel.Visible = false;


            if (!string.IsNullOrEmpty(nextQueueTicket))
            {
                // Display the next queueticket
                lblQueueTicket.Text = nextQueueTicket;
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
            string query = "SELECT TOP 1 queueticket FROM student WHERE department = @Department AND CONVERT(date, queuedate) = CONVERT(date, GETDATE()) AND IsDone = 0 AND (@CurrentQueueTicket IS NULL OR queueticket > @CurrentQueueTicket) ORDER BY queuedate, queueticket ASC";

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



        protected void btnConfirmCall_Click(object sender, EventArgs e)
        {
            try
            {
                string department = lblDepartment.Text; // Get the department
                string currentQueueTicket = lblQueueTicket.Text; // Get the current queueticket
                lblMessage.Text = "";

                // Check if currentQueueTicket is null or empty
                if (string.IsNullOrEmpty(currentQueueTicket))
                {
                    // If there's no queueticket, set a message in lblMessage
                    lblMessage.Text = "There's nothing to call.";
                    return; // Exit the method since there's nothing to call
                }



                lblservingTimeLabel.Visible = true;
                Timer1.Enabled = true; // Enable the timer



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

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            elapsedTimeInSeconds++;

            // Update the UI to display the elapsed time
            UpdateElapsedTimeUI();

            // Debug output
            Console.WriteLine($"Tick event fired. Elapsed time: {elapsedTimeInSeconds} seconds.");
        }

        private void UpdateElapsedTimeUI()
        {
            // Format the elapsed time as hours, minutes, and seconds
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTimeInSeconds);
            string formattedTime = timeSpan.ToString(@"hh\:mm\:ss");

            // Update server-side label
            lblservingTimeLabel.Text = formattedTime;

            // Call JavaScript function to update client-side label
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateTimer", $"updateTimer({elapsedTimeInSeconds});", true);
        }
        [WebMethod]
        public static int GetElapsedTime()
        {
            // This method is marked as static to be accessible from client-side script
            // You may need to adjust its accessibility based on your requirements

            // Assuming elapsedTimeInSeconds is an instance variable
            return elapsedTimeInSeconds;
        }



        protected void btnConfirmRecall_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            try
            {
                // Get the recall information from lblQueueTicket
                string recallInfo = lblQueueTicket.Text;

                // Get the department from lblDepartment
                string department = lblDepartment.Text;

                // Check if recallInfo or department is null or empty
                if (string.IsNullOrEmpty(recallInfo) || string.IsNullOrEmpty(department))
                {
                    // If either recallInfo or department is empty, display an error message
                    lblMessage.Text = "Recall information or department is missing.";
                    return;
                }

                // Insert the recall information into the database
                InsertRecallInfo(recallInfo, department);

                // Display a success message
                lblMessage.Text = "Recall student successfully.";
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                // Display an error message or log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                lblMessage.Text = "An error occurred while inserting recall information.";
            }
        }

        private void InsertRecallInfo(string recallInfo, string department)
        {
            // Connection string to your SQL Server database
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // SQL query to insert recall information into the database
            string query = "INSERT INTO RecallInformation (Recall, Department, CreatedAt) VALUES (@Recall, @Department, GETDATE())";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the SQL command to prevent SQL injection
                    command.Parameters.AddWithValue("@Recall", recallInfo);
                    command.Parameters.AddWithValue("@Department", department);

                    // Open the database connection
                    connection.Open();

                    // Execute the SQL command
                    command.ExecuteNonQuery();
                }
            }
        }





        protected async void btnConfirmDone_Click(object sender, EventArgs e)
        {
            try
            {
                string currentQueueTicket = lblQueueTicket.Text;
                string department = lblDepartment.Text;

                if (!string.IsNullOrEmpty(currentQueueTicket))
                {
                    // Stop the serving time timer when marking a ticket as Done
                    Timer1.Enabled = false;
                    lblservingTimeLabel.Visible = false;

                    // Stop the serving time timer when marking a ticket as Done
                    UpdateIsDoneForTicket(department, currentQueueTicket);

                    // Get the elapsed time in seconds
                    int elapsedSeconds = elapsedTimeInSeconds;

                    // Insert a record into QueueReport with the elapsed time
                    InsertIntoQueueReport(department, currentQueueTicket, DateTime.Now, elapsedSeconds);

                    // Reset the elapsed time to 0
                    elapsedTimeInSeconds = 0;
                    // Call the asynchronous method to update the ticket status through API
                    await UpdateIsDoneThroughAPIAsync(department, currentQueueTicket);

                    lblQueueTicket.Text = "";

                    lblMessage.Text = "Ticket marked as Done.";

                    // Reload user data after processing the queue ticket
                    LoadUserData(Session["LoggedInUsername"] as string);
                }
                else
                {
                    lblMessage.Text = "No queueticket available to mark as Done.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                // Display an error message or log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void InsertIntoQueueReport(string department, string queueticket, DateTime doneDate, int elapsedSeconds)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

            // Format elapsed time as "00:00:00"
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedSeconds);
            string formattedTime = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

            // Assuming you have a 'QueueReport' table with 'QueueTicket', 'Department', 'DoneDate', and 'Timer' columns
            string query = "INSERT INTO QueueReports (QueueTicket, Department, DoneDate, Timer) VALUES (@QueueTicket, @Department, @DoneDate, CAST(@FormattedTime AS TIME))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@QueueTicket", queueticket);
                    command.Parameters.AddWithValue("@DoneDate", doneDate);
                    command.Parameters.AddWithValue("@FormattedTime", formattedTime); // Use the formatted time

                    connection.Open();
                    command.ExecuteNonQuery();
                }
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

        private async Task UpdateIsDoneThroughAPIAsync(string department, string currentQueueTicket)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65388");

                // Set CurrentQueueTicket to department + "000" if it's empty
                var payload = new
                {
                    Department = department,
                    CurrentQueueTicket = char.ToUpper(department[0]) + "-0000"
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:65388/api/QueueTicket/PostQueueTicket", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Queue ticket information posted successfully.");
                }
                else
                {
                    Console.WriteLine($"Error posting queue ticket information. Status code: {response.StatusCode}");
                }
            }
        }
    
    }
}