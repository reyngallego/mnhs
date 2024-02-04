using System.Collections.Generic;
using System.Web.Http;
using WebApplication3.Models;
using System;
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

        // GET api/Employee/{id}
        public IHttpActionResult Get(int id)
        {
            Employee employee = dataAccess.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound(); // Return 404 Not Found if the employee with the specified ID is not found
            }

            return Ok(employee); // Return the employee data if found
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

        // PUT api/Employee/5 - Update an existing employee
        [HttpPut]
        public IHttpActionResult Put([FromUri] int id, [FromBody] Employee employee)
        {
            if (employee != null)
            {
                // Call your data access layer to update the employee data in the database.
                try
                {
                    dataAccess.UpdateEmployee(id, employee);
                    return Ok("Employee updated successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Invalid employee data");
            }
        }

        // DELETE api/Employee/5 - Delete an employee by ID
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
