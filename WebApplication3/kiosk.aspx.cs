using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

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
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.Text);
                    cmd.Parameters.AddWithValue("@Purpose", txtPurpose.Text);
                    cmd.Parameters.AddWithValue("@QueueTicket", GenerateQueueTicket(ddlDepartment.Text)); // Pass the selected department
                    cmd.Parameters.AddWithValue("@QueueDate", DateTime.Now);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Show the modal dialog with the name and queue ticket
                        ClientScript.RegisterStartupScript(GetType(), "ShowModal", $"showQueueTicket('{GeneratedQueueTicket}');", true);
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
            ddlDepartment.Text = ""; // Clear the selected option
            txtPurpose.Text = "";
        }

        public string GeneratedQueueTicket
        {
            get
            {
                return GenerateQueueTicket(ddlDepartment.Text);
            }
        }

        private string GenerateQueueTicket(string department)
        {
            // Generate the department-specific queue ticket based on the selected department.
            string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
            string selectQuery = "SELECT MAX(queueticket) FROM student WHERE CONVERT(date, queuedate) = @QueueDate AND department = @Department";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@QueueDate", DateTime.Today); // Filter by today's date
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.Text); // Use the parameter departmentName

                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    connection.Close();

                    if (result != DBNull.Value)
                    {
                        string lastTicket = (string)result;
                        int lastTicketNumber = int.Parse(lastTicket.Split('-')[1]);
                        int nextTicketNumber = lastTicketNumber + 1;

                        // Generate the department-specific queue ticket in the format "X-XXXX" where X is the department code and XXXX is the incremented number.
                        string departmentCode = ddlDepartment.Text.Substring(0, 1).ToUpper(); // Extract the first character of the department name as the code.
                        return $"{departmentCode}-{nextTicketNumber.ToString("D4")}";
                    }
                }
            }

            // If there are no existing records for today in the selected department, start with the department-specific code followed by "-0001".
            string initialDepartmentCode = ddlDepartment.Text.Substring(0, 1).ToUpper(); // Extract the first character of the department name as the code.
            return $"{initialDepartmentCode}-0001";


        }

    }
}
