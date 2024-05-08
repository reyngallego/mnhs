$(document).ready(function () {
    // Show the logout modal when the logout button is clicked
    $("#logoutButton").on("click", function (e) {
        e.preventDefault(); // Prevent the default behavior (page reload)
        $("#logoutModal").modal("show");
    });

    // Handle the cancel action
    $("#cancelLogout").on("click", function () {
        // Close the modal without performing logout actions
        $("#logoutModal").modal("hide");
    });

    // Handle the confirmation of logout
    $("#confirmLogout").on("click", function () {
        // Perform logout actions here (e.g., redirect or log out the user)
        // For demonstration, let's assume the user is redirected to login.aspx
        window.location.href = "login.aspx";
    });
    // Logic for the Call button click
    $("#btnCallTicket").click(function () {
        // Show the Call modal
        $("#callModal").modal("show");
    });

    // Logic for the Done button click
    $("#btnDoneTicket").click(function () {
        // Show the Done modal
        $("#doneModal").modal("show");
    });

    // Logic for confirming the Call action
    function confirmCall() {
        $('#callModal').modal('show');

        // Logic to handle confirmation
        $('#confirmCall').click(function () {
            // Hide the modal
            $('#callModal').modal('hide');

            // Perform the server-side action
        });
    }

    function confirmDone() {
        $('#doneModal').modal('show');

        // Logic to handle confirmation
        $('#confirmDone').click(function () {
            // Hide the modal
            $('#doneModal').modal('hide');

            // Perform the server-side action
        });
    }


    // Function to fetch and display student data based on the logged-in department
    function fetchStudentDataAndQueueTicket() {
        var apiUrl;
        switch (loggedInDepartment.toLowerCase()) {
            case "cashier":
                apiUrl = 'http://localhost:65388/api/student/GetStudentData?accountType=cashier';
                break;
            case "registrar":
                apiUrl = 'http://localhost:65388/api/student/GetStudentData?accountType=registrar';
                break;
            case "director":
                apiUrl = 'http://localhost:65388/api/student/GetStudentData?accountType=director';
                break;
            case "studentaffairsandservices":
                apiUrl = 'http://localhost:65388/api/Student/GetStudentData?accountType=studentaffairsandservices';
                break;
                // Add more cases for other departments as needed
                // case "department_2":
                //     apiUrl = 'http://localhost:65388/api/student/GetStudentData?accountType=department_2';
                //     break;
                // ...
            default:
                console.error('Unknown department:', loggedInDepartment);
                return; // Do not proceed if the department is unknown
        }

        // Fetch and display student data
        $.ajax({
            url: apiUrl,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                displayStudentData(data);
                // Fetch data again after displaying, creating a loop for long polling
                fetchStudentDataAndQueueTicket();
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error (Student Data):', error);

                // Retry fetching student data after a delay in case of an error
                setTimeout(fetchStudentDataAndQueueTicket, 5000); // Retry after 5 seconds
            }
        });
    }

    // Function to display student data
    function displayStudentData(studentData) {
        var studentTable = $('<table class="table"></table>');
        var tableHead = $('<thead><tr><th>No.</th><th>Name</th><th>Department</th><th>Purpose</th><th>QueueTicket</th><th>QueueDate</th></tr></thead>');
        var tableBody = $('<tbody></tbody>');

        // Clear existing content
        $('.studentDataContainer').empty();

        // Loop through each student and append data to the table
        $.each(studentData, function (index, student) {
            var tableRow = $('<tr></tr>');
            // Add a "No." column with sequential numbers
            tableRow.append('<td>' + (index + 1) + '</td>');
            tableRow.append('<td>' + student.name + '</td>');
            tableRow.append('<td>' + student.department + '</td>');
            tableRow.append('<td>' + student.purpose + '</td>');
            tableRow.append('<td>' + student.queueticket + '</td>');

            // Format the date to display only the date part
            var formattedDate = new Date(student.queuedate).toLocaleDateString();
            tableRow.append('<td>' + formattedDate + '</td>');

            tableBody.append(tableRow);
        });

        // Append the table head and body to the table
        studentTable.append(tableHead);
        studentTable.append(tableBody);

        // Append the table to the studentDataContainer
        $('.studentDataContainer').append(studentTable);
    }

    // Call the function to fetch and display student data
    fetchStudentDataAndQueueTicket();

});
