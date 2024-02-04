using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WebApplication3.Models;

using System.Web;

namespace WebApplication3
{
    public class EmployeeDataAccess
    {
        private string connectionString = "Data Source=DESKTOP-M20CR1S\\SQLEXPRESS;Initial Catalog=capstone;Integrated Security=True";

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT id, username, firstname, lastname, department, image FROM users", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Id = (int)reader["id"],
                                Username = reader["username"].ToString(),
                                FirstName = reader["firstname"].ToString(),
                                LastName = reader["lastname"].ToString(),
                                Department = reader["department"].ToString(),
                            };

                            // Check if the "image" column is not DBNull
                            if (reader["image"] != DBNull.Value)
                            {
                                employee.Image = (byte[])reader["image"];
                            }

                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a SQL SELECT query to retrieve employee data based on employeeId
                string selectQuery = "SELECT id, username, firstname, lastname, department, image FROM users WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    // Set the parameter value
                    command.Parameters.AddWithValue("@Id", employeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Id = (int)reader["id"],
                                Username = reader["username"].ToString(),
                                FirstName = reader["firstname"].ToString(),
                                LastName = reader["lastname"].ToString(),
                                Department = reader["department"].ToString(),
                            };

                            // Check if the "image" column is not DBNull
                            if (reader["image"] != DBNull.Value)
                            {
                                employee.Image = (byte[])reader["image"];
                            }

                            return employee;
                        }
                    }
                }
            }

            // Return null if no employee with the specified ID is found
            return null;
        }

        public void UpdateEmployee(int employeeId, Employee updatedEmployee)
        {
            // Get the current employee data for the specified ID
            Employee existingEmployee = GetEmployeeById(employeeId);

            if (existingEmployee != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define a SQL UPDATE query to update employee data based on employeeId
                    string updateQuery = "UPDATE users SET " +
                                         "username = ISNULL(@Username, username), " +
                                         "firstname = ISNULL(@FirstName, firstname), " +
                                         "lastname = ISNULL(@LastName, lastname), " +
                                         "department = ISNULL(@Department, department), " +
                                         "password = ISNULL(@Password, password) " +
                                         "WHERE id = @Id";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@Id", employeeId);
                        command.Parameters.AddWithValue("@Username", (object)updatedEmployee.Username ?? DBNull.Value);
                        command.Parameters.AddWithValue("@FirstName", (object)updatedEmployee.FirstName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@LastName", (object)updatedEmployee.LastName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Department", (object)updatedEmployee.Department ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Password", (object)updatedEmployee.Password ?? DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // Handle the case where no employee with the specified ID is found
                throw new Exception($"Employee with ID {employeeId} not found");
            }
        }

        public bool DeleteEmployee(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a SQL DELETE query to delete the employee data based on the employeeId
                string deleteQuery = "DELETE FROM users WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Set the parameter value
                    command.Parameters.AddWithValue("@Id", employeeId);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected (employee deleted) and return a boolean result
                    return rowsAffected > 0;
                }
            }
        }

        private byte[] GetDefaultImageBytes()
        {
            // Assuming the image is named default-image.jpg
            string imagePath = HttpContext.Current.Server.MapPath("~/images/default-image.jpg");

            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath);
            }

            // Return a placeholder or handle the case when the default image is not found
            return null;
        }

        public void AddEmployee(Employee newEmployee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a SQL INSERT query to add a new employee
                string insertQuery = "INSERT INTO users (username, password, firstname, lastname, department, image) " +
                                     "VALUES (@Username, @Password, @FirstName, @LastName, @Department, @Image)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Set the parameter values
                    command.Parameters.AddWithValue("@Username", newEmployee.Username);
                    command.Parameters.AddWithValue("@Password", newEmployee.Password); // Assuming newEmployee.Password is provided
                    command.Parameters.AddWithValue("@FirstName", newEmployee.FirstName);
                    command.Parameters.AddWithValue("@LastName", newEmployee.LastName);
                    command.Parameters.AddWithValue("@Department", newEmployee.Department);

                    // Assuming ImageData is a byte array property in your Employee class
                    if (newEmployee.Image != null)
                    {
                        command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = newEmployee.Image;
                    }
                    else
                    {
                        // If Image is null, set a default image (replace defaultImageBytes with your default image byte array)
                        byte[] defaultImageBytes = GetDefaultImageBytes(); // Implement this method to get the default image bytes
                        command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = defaultImageBytes;
                    }

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
