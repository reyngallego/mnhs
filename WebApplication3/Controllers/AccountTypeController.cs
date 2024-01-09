using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

public class AccountTypeController : ApiController
{
    [HttpGet]
    public IHttpActionResult GetAccountTypes()
    {
        try
        {
            // Retrieve distinct account types from the student table
            DataTable accountTypes = GetDistinctAccountTypesFromDatabase();

            if (accountTypes != null && accountTypes.Rows.Count > 0)
            {
                return Ok(accountTypes);
            }
            else
            {
                return NotFound(); // Or return an appropriate response for an empty result
            }
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    public DataTable GetDistinctAccountTypesFromDatabase()
    {
        string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";
        string query = "SELECT DISTINCT department AS AccountType FROM student";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }
    }
}
