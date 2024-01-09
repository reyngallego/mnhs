using System.Collections.Generic;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class EmployeeController : ApiController
    {
        private EmployeeDataAccess dataAccess = new EmployeeDataAccess();

        // GET api/Employee
        public IEnumerable<Employee> Get()
        {
            return dataAccess.GetEmployees();
        }

        // POST api/Employee - Add a new employee
        [HttpPost]
        public IHttpActionResult Post([FromBody] Employee employee)
        {
            if (employee != null)
            {
                // Call your data access layer to insert the new employee data into the database.
                dataAccess.AddEmployee(employee);
                return Ok("Employee added successfully");
            }
            else
            {
                return BadRequest("Invalid employee data");
            }
        }
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] Employee employee)
        {
            if (employee != null)
            {
                // Call your data access layer to update the employee data in the database.
                dataAccess.UpdateEmployee(id, employee);
                return Ok("Employee updated successfully");
            }
            else
            {
                return BadRequest("Invalid employee data");
            }
        }
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            // Call your data access layer to delete the employee with the specified ID.
            bool deleted = dataAccess.DeleteEmployee(id);

            if (deleted)
            {
                return Ok("Employee deleted successfully");
            }
            else
            {
                return NotFound();
            }
        }

    }
}


