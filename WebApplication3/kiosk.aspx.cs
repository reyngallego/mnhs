using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO.Ports;
using System.Text;

namespace WebApplication3
{
    public partial class kiosk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Your Page_Load logic here if needed
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string insertQuery = "INSERT INTO student (name, department, purpose, queueticket, queuedate) " +
                                 "VALUES (@Name, @Department, @Purpose, @QueueTicket, @QueueDate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                {
                    // Use the client-side selected values
                    string selectedPurpose = Request.Form[ddlPurpose.UniqueID];
                    string selectedDepartment = Request.Form[ddlDepartment.UniqueID];
                    string generatedQueueTicket = GenerateQueueTicket(selectedDepartment, selectedPurpose);

                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Department", selectedDepartment);
                    cmd.Parameters.AddWithValue("@Purpose", selectedPurpose);
                    cmd.Parameters.AddWithValue("@QueueTicket", generatedQueueTicket);
                    cmd.Parameters.AddWithValue("@QueueDate", DateTime.Now);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Show the modal dialog with the name and queue ticket
                        ClientScript.RegisterStartupScript(GetType(), "ShowModal", $"showQueueTicket('{generatedQueueTicket}');", true);
                        PrintTicket(generatedQueueTicket, txtName.Text, selectedPurpose);

                        ClearFormFields();
                    }
                    else
                    {
                        lblMessage.Text = "Failed to insert data.";
                    }
                }
            }
        }

        private void ClearFormFields()
        {
            // Clear the form fields after successful data insertion.
            txtName.Text = "";
            ddlDepartment.SelectedIndex = 0; // Clear the selected option
            ddlPurpose.SelectedIndex = 0; // Clear the selected option
        }

        public string GeneratedQueueTicket
        {
            get
            {
                return GenerateQueueTicket(ddlDepartment.SelectedValue, ddlPurpose.SelectedValue);
            }
        }

        private string GenerateQueueTicket(string department, string purpose)
        {
            // Generate the department-specific queue ticket based on the selected department.
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string selectQuery = "SELECT MAX(queueticket) FROM student WHERE CONVERT(date, queuedate) = @QueueDate AND department = @Department";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@QueueDate", DateTime.Today); // Filter by today's date
                    cmd.Parameters.AddWithValue("@Department", department); // Use the parameter department

                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    connection.Close();

                    if (result != DBNull.Value)
                    {
                        string lastTicket = (string)result;
                        int lastTicketNumber = int.Parse(lastTicket.Split('-')[1]);
                        int nextTicketNumber = lastTicketNumber + 1;

                        // Generate the department-specific queue ticket in the format "X-XXXX" where X is the department code and XXXX is the incremented number.
                        string departmentCode = department.Substring(0, 1).ToUpper(); // Extract the first character of the department name as the code.
                        return $"{departmentCode}-{nextTicketNumber.ToString("D4")}";
                    }
                }
            }

            // If there are no existing records for today in the selected department, start with the department-specific code followed by "-0001".
            string initialDepartmentCode = department.Substring(0, 1).ToUpper(); // Extract the first character of the department name as the code.
            return $"{initialDepartmentCode}-0001";
        }

        private void PrintTicket(string ticket, string name, string purpose)
        {
            try
            {
                // Replace "COMx" with the appropriate Bluetooth COM port for your printer
                using (SerialPort bluetoothPort = new SerialPort("COM3"))
                {
                    bluetoothPort.BaudRate = 9600;
                    bluetoothPort.Open();

                    // Construct the content to be printed
                    string printContent = ConstructPrintContent(name, ticket, purpose);

                    // Send the ticket to the printer
                    bluetoothPort.Write(printContent);

                    bluetoothPort.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show an error message)
                lblMessage.Text = $"Error printing ticket: {ex.Message}";
            }
        }

        private string ConstructPrintContent(string name, string ticket, string purpose)
        {
            StringBuilder contentBuilder = new StringBuilder();

            // Add a header

            contentBuilder.AppendLine("         PUP PARANAQUE        ");
            contentBuilder.AppendLine("     QUEUE MANAGEMENT SYSTEM  ");


            // Add a separator line
            contentBuilder.AppendLine(new string('-', 30));

            // Add details
            contentBuilder.AppendLine($"Date:");
            contentBuilder.AppendLine($"{DateTime.Now.ToString("dddd, MMMM dd, yyyy HH:mm tt")}");
            contentBuilder.AppendLine($"Queue Ticket: {ticket.ToUpper()}");
            contentBuilder.AppendLine($"Name: {name}");
            contentBuilder.AppendLine($"Purpose: {purpose}");
            // Add a separator line
            contentBuilder.AppendLine(new string('-', 30));
            contentBuilder.AppendLine("Feedback Form");
            contentBuilder.AppendLine("https://docs.google.com/forms/d/e/1FAIpQLSff6riCakzCUi4SOytZ-xbzg3jyI7kMNRECyeEVoBJtcL6VEA/viewform");
            contentBuilder.AppendLine(new string('-', 30));

            // Add a footer
            contentBuilder.AppendLine("Thank you for using ");
            contentBuilder.AppendLine("PUP Queue Management System!");

            contentBuilder.AppendLine(new string('-', 30));
            contentBuilder.AppendLine(new string('-', 30));
            contentBuilder.AppendLine(new string('-', 30));
            contentBuilder.AppendLine(new string('-', 30));

            return contentBuilder.ToString();
        }
    }
}

    