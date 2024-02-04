jQuery(document).ready(function ($) {
    // Declare employeeId variable outside of functions to make it accessible globally
    var employeeIdToDelete;

    // Function to load employees from the API
    function loadEmployees(editEmployeeId) {
        try {
            $.ajax({
                url: '/api/Employee',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    // Log the retrieved data to the console
                    console.log("Data retrieved:", data);
                    displayEmployees(data);

                    // Check if editEmployeeId is provided
                    if (editEmployeeId) {
                        // Trigger the edit modal for the specified employee
                        triggerEditModal(editEmployeeId);
                    }
                },
                error: function (error) {
                    // Handle errors
                    console.error("Error loading employees: " + error);
                }
            });
        } catch (e) {
            console.error("An error occurred: " + e);
        }
    }

    // Function to bind click event handlers for Edit and Delete buttons
    function bindButtonClickEvents() {
        // Use event delegation for dynamically added elements
        $('#employee-list').off('click', '.edit-employee-button').on('click', '.edit-employee-button', function (event) {
            // Prevent the default behavior of the anchor tag
            event.preventDefault();

            // Set the employeeIdToDelete variable for the edit modal
            employeeIdToDelete = $(this).data('employee-id');
            console.log('Edit button clicked. Employee ID:', employeeIdToDelete);

            // Trigger the edit modal for the specified employee
            triggerEditModal(employeeIdToDelete);
        });


        $('#employee-list').off('click', '.delete-employee-button').on('click', '.delete-employee-button', function () {
            // Set the employeeIdToDelete variable for the delete confirmation
            employeeIdToDelete = $(this).data('employee-id');
            // Show the delete confirmation modal
            $('#deleteEmployeeModal').modal('show');
        });

        // Add an event listener for the "Delete" button in the delete confirmation modal
        $('#confirm-delete-button').off('click').on('click', function () {
            // Send a DELETE request to the API to delete the employee
            deleteEmployee(employeeIdToDelete);
        });

        // Add an event listener for the "Save Changes" button
        $('#edit-save-button').off('click').on('click', function () {
            // Collect the edited data from the modal fields
            var editedEmployee = {
                Username: $('#edit-username-input').val(),
                FirstName: $('#edit-firstname-input').val(),
                LastName: $('#edit-lastname-input').val(),
                Department: $('#edit-department-input').val(),
                // Add other fields as needed
            };

            // Send the edited data to the server using an AJAX request
            $.ajax({
                url: '/api/Employee/' + employeeIdToDelete,
                type: 'PUT',
                contentType: 'application/x-www-form-urlencoded', // Change the content type
                data: editedEmployee, // No need to stringify the data
                success: function (response) {
                    // Handle success
                    console.log("Employee data updated successfully:", response);
                    // You may want to reload the employee data after a successful update
                    loadEmployees();
                    // Close the modal after a successful update
                    $('#editEmployeeModal').modal('hide');
                },
                error: function (error) {
                    // Log the error details
                    console.error("Error updating employee data:", error);

                    // Handle errors
                    alert("Error updating employee data. See console for details.");
                }
            });
        });

        // Add an event listener for a button to trigger the edit modal
    }
    // Function to display employee data in the table
    function displayEmployees(employees) {
        var employeeList = $('#employee-list');
        employeeList.empty();

        employees.forEach(function (employee) {
            var imageUrl = '';

            // Check if the employee has an image
            if (employee.Image) {
                // Convert the base64 string to a byte array
                var byteCharacters = atob(employee.Image);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var uint8Array = new Uint8Array(byteNumbers);

                // Create a blob from the Uint8Array
                var blob = new Blob([uint8Array], { type: 'image/png' });

                // Create a data URL from the blob
                imageUrl = URL.createObjectURL(blob);
            }

            employeeList.append(
                '<tr>' +
                '<td>' + employee.Id + '</td>' +
                '<td>' + employee.Username + '</td>' +
                '<td>' + employee.FirstName + '</td>' +
                '<td>' + employee.LastName + '</td>' +
                '<td>' + employee.Department + '</td>' +
                '<td><img src="' + imageUrl + '" alt="Employee Image" style="max-width: 100px; max-height: 100px;"></td>' + // Display image with maximum dimensions
                '<td>' +
                '<button class="btn btn-info edit-employee-button" data-employee-id="' + employee.Id + '" data-toggle="modal" data-target="#confirmEditModal">Edit</button>' +
                '<button class="btn btn-danger delete-employee-button" data-employee-id="' + employee.Id + '" type="button">Delete</button>' +
                '</td>' +
                '</tr>'
            );
        });


        // Add click event handlers for Edit and Delete buttons
        bindButtonClickEvents();
    }

    // Function to trigger the edit modal for a specific employee
    function triggerEditModal(employeeId) {
        // Fetch the employee data by making an API request to get the employee details by ID
        $('#confirmEditModal').modal('show');

        // Add an event listener for the "Confirm Edit" button in the confirmation modal
        $('#confirm-edit-button').off('click').on('click', function () {
            // Close the confirmation modal
            $('#confirmEditModal').modal('hide');

            // Fetch the employee data by making an API request to get the employee details by ID
            $.ajax({
                url: '/api/Employee/' + employeeId,
                type: 'GET',
                dataType: 'json',
                success: function (employeeData) {
                    // Populate the modal/form fields with the employee data for editing
                    $('#editEmployeeModal').modal('show'); // Show the edit modal

                    // Populate the input fields with the employee data
                    $('#edit-username-input').val(employeeData.Username);
                    $('#edit-firstname-input').val(employeeData.FirstName);
                    $('#edit-lastname-input').val(employeeData.LastName);

                    // Set the selected department in the dropdown
                    $('#edit-department-input').val(employeeData.Department);

                    // Show the new password and confirm password input fields
                    $('#edit-new-password-input').show();
                    $('#edit-confirm-password-input').show();

                    // Clear the password fields
                    $('#edit-old-password-input').val('');
                    $('#edit-new-password-input').val('');
                    $('#edit-confirm-password-input').val('');
                },
                error: function (error) {
                    // Handle errors
                    console.error("Error loading employee details: " + error);
                }
            });

            // Attach click event for the edit save button
            $('#edit-save-button').on('click', function () {
                // Collect the data for editing from the modal fields
                var newPassword = $('#edit-new-password-input').val();
                var confirmPassword = $('#edit-confirm-password-input').val();

                // Validate password and confirm password
                if (newPassword !== confirmPassword) {
                    alert('New password and confirm password do not match.');
                    return;
                }

                var editedEmployee = {
                    Username: $('#edit-username-input').val(),
                    Password: newPassword,
                    FirstName: $('#edit-firstname-input').val(),
                    LastName: $('#edit-lastname-input').val(),
                    Department: $('#edit-department-input').val(),
                };

                // Send the edited data to the server using an AJAX request
                $.ajax({
                    url: '/api/Employee/' + employeeId,
                    type: 'PUT', // Use PUT for update
                    contentType: 'application/x-www-form-urlencoded', // Change the content type
                    data: editedEmployee,
                    success: function (response) {
                        // Handle success
                        console.log("Employee updated successfully:", response);
                        // You may want to reload the employee data after a successful update
                        loadEmployees();
                        // Close the modal after a successful update
                        $('#editEmployeeModal').modal('hide');
                    },
                    error: function (error) {
                        // Handle errors
                        console.error("Error updating employee: " + error);
                    }
                });
            });
        });
    }
    // Function to delete an employee by ID
    function deleteEmployee(employeeId) {
        try {
            $.ajax({
                url: '/api/Employee/' + employeeId,
                type: 'DELETE',
                success: function (response) {
                    // Handle success
                    console.log("Employee deleted successfully:", response);
                    // You may want to reload the employee data after a successful delete
                    loadEmployees();
                    // Close the delete confirmation modal after a successful delete
                    $('#deleteEmployeeModal').modal('hide');
                },
                error: function (error) {
                    // Handle errors
                    console.error("Error deleting employee: " + error);
                }
            });
        } catch (e) {
            console.error("An error occurred: " + e);
        }
    }

    // Handle the Add Employee button click event
    $('#add-employee-button').click(function () {
        // Open the "Add Employee" modal
        $('#addEmployeeModal').modal('show');
    });

    // Add an event listener for the "Add Employee" button in the modal
    $('#add-save-button').on('click', function () {
        // Collect the data for the new employee from the modal fields
        var newPassword = $('#add-password-input').val();
        var confirmPassword = $('#add-confirm-password-input').val();

        // Validate password and confirm password
        if (newPassword !== confirmPassword) {
            alert('Password and confirm password do not match.');
            return;
        }

        var newEmployee = {
            Username: $('#add-username-input').val(),
            Password: newPassword,
            FirstName: $('#add-firstname-input').val(),
            LastName: $('#add-lastname-input').val(),
            Department: $('#add-department-input').val(),
        };


        // Send the data for the new employee to the server using an AJAX request
        $.ajax({
            url: '/api/Employee',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded', // Change the content type
            data: newEmployee, // No need to stringify the data
            success: function (response) {
                // Handle success
                console.log("Employee added successfully:", response);
                // You may want to reload the employee data after a successful add
                loadEmployees();
                // Close the modal after a successful add
                $('#addEmployeeModal').modal('hide');
            },
            error: function (error) {
                // Handle errors
                console.error("Error adding employee: " + error);
            }
        });
    });

    // Load employees when the page is ready and when the "Employees" tab is clicked
    function loadEmployeesOnTabClick() {
        $('a[data-toggle="tab"][href="#employees"]').on('shown.bs.tab', function (e) {
            loadEmployees(); // Load employees when the "Employees" tab is clicked
        });
    }

    loadEmployees(); // Load employees when the page is first loaded
    loadEmployeesOnTabClick();

    $('a[href="#logoutModal"]').on('click', function (e) {
        e.preventDefault();
        $('#logoutModal').modal('show');
    });

    // Add an event listener for the "Logout" button inside the modal
    $('#confirmLogoutBtn').on('click', function () {
        // Redirect to the login page
        window.location.href = "login.aspx";
    });
    // Show the edit modal immediately
    // This line ensures that the edit modal is shown only once, immediately after the page is loaded


});

