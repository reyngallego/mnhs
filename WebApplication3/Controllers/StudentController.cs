using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Http;
using System.Linq;

public class StudentController : ApiController
{
    private static List<Student> students = new List<Student>();
    private static readonly object lockObject = new object();

    // Inject the AccountTypeController to fetch distinct account types
    private AccountTypeController accountTypeController = new AccountTypeController();

    [HttpGet]
    public IHttpActionResult GetStudentData(string accountType)
    {
        try
        {
            // Check if the provided account type is valid
            if (!IsValidAccountType(accountType))
            {
                return BadRequest("Invalid account type");
            }

            DataTable studentData = GetStudentDataFromDatabase(accountType);

            if (studentData != null && studentData.Rows.Count > 0)
            {
                return Ok(studentData);
            }
            else
            {
                // Start the long polling thread to wait for updates
                Thread longPollingThread = new Thread(() => LongPollingThread(accountType));
                longPollingThread.Start();

                return Ok(studentData); // Return the initial data
            }
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    private DataTable GetStudentDataFromDatabase(string accountType)
    {
        string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        // Define the base query without the account type condition
        string baseQuery = "SELECT ROW_NUMBER() OVER (ORDER BY queueticket) AS RowNumber, name, department, purpose, queueticket, queuedate FROM student WHERE CONVERT(DATE, queuedate) = CONVERT(DATE, GETDATE()) AND isDone = 0";

        // Customize the query based on the account type
        string query = "";
        switch (accountType.ToLower())
        {
            case "cashier":
                query = baseQuery + " AND department = 'Cashier'";
                break;
            case "registrar":
                query = baseQuery + " AND department = 'Registrar'";
                break;
            case "director":
                query = baseQuery + " AND department = 'Director'";
                break;
            case "studentaffairsandservices":
                query = baseQuery + " AND department = 'studentaffairsandservices'";
                break;
            // Add more cases for other account types as needed
            // case "account_type_2":
            //     query = baseQuery + " AND department = 'Department_2'";
            //     break;
            // ...
            default:
                // If no specific account type is provided, use the base query
                query = baseQuery;
                break;
        }
    

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(dataTable);

                // Define the primary key for the DataTable
                DataColumn[] primaryKeys = new DataColumn[1];
                primaryKeys[0] = dataTable.Columns["queueticket"];
                dataTable.PrimaryKey = primaryKeys;

                return dataTable;
            }
        }
    }

    private void LongPollingThread(string accountType)
    {
        lock (lockObject)
        {
            try
            {
                // Wait for a signal or timeout (e.g., 30 seconds)
                Monitor.Wait(lockObject, 30000);

                // When a signal is received, notify clients with updated data
                NotifyClients(accountType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in long polling thread: " + ex.Message);
            }
        }
    }

    private void NotifyClients(string accountType)
    {
        // Notify clients about the data change (in a real-world scenario, you'd use a better notification mechanism)
        students.Clear(); // Clear the existing data
        DataTable updatedData = GetStudentDataFromDatabase(accountType);
        foreach (DataRow row in updatedData.Rows)
        {
            students.Add(new Student
            {
                Name = row["name"].ToString(),
                Department = row["department"].ToString(),
                Purpose = row["purpose"].ToString(),
                Queueticket = row["queueticket"].ToString(),
                Queuedate = ((DateTime)row["queuedate"]).ToString("yyyy-MM-dd") // Format the date as needed
                                                                                // Add more properties as needed
            });
        }

        // Return control to the waiting clients
        Monitor.PulseAll(lockObject);
    }

    // Check if the provided account type is valid
    private bool IsValidAccountType(string accountType)
    {
        DataTable accountTypes = accountTypeController.GetDistinctAccountTypesFromDatabase();
        return accountTypes.AsEnumerable().Any(row => row.Field<string>("AccountType").Equals(accountType, StringComparison.OrdinalIgnoreCase));
    }

    public class Student
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Purpose { get; set; }
        public string Queueticket { get; set; }

        public string Queuedate { get; set; }

        // Add more properties as needed
    }
}
