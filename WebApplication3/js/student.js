$(document).ready(function () {
    // Function to populate the table with student data
    function populateTable() {
        $.ajax({
            type: "GET",
            url: "/api/mnhs/GetStudents", // Endpoint to retrieve student data
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#studentTable tbody").empty(); // Clear existing table rows
                $.each(response, function (index, student) {
                    var row = "<tr>" +
                        "<td>" + student.LRN + "</td>" +
                        "<td>" + student.FirstName + " " + student.LastName + "</td>" +
                        "<td>" + student.Grade + "</td>" +
                        "<td>" + student.Section + "</td>" +
                        "<td>" + student.Adviser + "</td>" +
                        "<td><button class='btn btn-primary edit-btn' data-student-id='" + student.LRN + "'>Edit</button> <button class='btn btn-danger'>Delete</button></td>" +
                        "</tr>";
                    $("#studentTable tbody").append(row);
                });

                // Add click event listener to edit buttons
                $(".edit-btn").click(function () {
                    var studentId = $(this).data("student-id");
                    openEditModal(studentId);
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                alert("Error: " + errorThrown);
            }
        });
    }

    // Call the populateTable function when the page loads
    populateTable();

    // Function to open the edit modal dialog with student data
    function openEditModal(studentId) {
        // Retrieve student data by ID and populate the edit modal fields
        // You need to implement this part based on your backend logic
        // Example:
        $.ajax({
            type: "GET",
            url: "/api/mnhs/GetStudentById?studentId=" + studentId, // Endpoint to retrieve student data by ID
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (student) {
                $("#edit_STUD_LRN").val(student.LRN);
                $("#edit_STUD_FNAME").val(student.FirstName);
                $("#edit_STUD_MNAME").val(student.MiddleName);
                $("#edit_STUD_LNAME").val(student.LastName);
                $("#edit_STUD_EXT").val(student.NameExtension);
                $("#edit_STUD_GRADE").val(student.Grade);
                $("#edit_STUD_SECTION").val(student.Section);
                $("#edit_STUD_BIRTHDATE").val(student.Birthdate);
                $("#edit_STUD_SEX").val(student.Sex);
                $("#edit_STUD_ADDRESS").val(student.Address);
                $("#edit_PARENT_FULLNAME").val(student.ParentFullName);
                $("#edit_PARENT_CONTACT").val(student.ParentContact);
                $("#edit_ADVISER").val(student.Adviser);

                // Show the edit modal dialog
                $("#editStudentModal").modal("show");
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                alert("Error: " + errorThrown);
            }
        });
    }

    // Function to handle form submission in the edit modal
    $("#editSaveChanges").click(function () {
        // Collect data from edit modal fields
        var formData = {
            LRN: $("#edit_STUD_LRN").val(),
            FirstName: $("#edit_STUD_FNAME").val(),
            MiddleName: $("#edit_STUD_MNAME").val(),
            LastName: $("#edit_STUD_LNAME").val(),
            NameExtension: $("#edit_STUD_EXT").val(),
            Grade: $("#edit_STUD_GRADE").val(),
            Section: $("#edit_STUD_SECTION").val(),
            Birthdate: $("#edit_STUD_BIRTHDATE").val(),
            Sex: $("#edit_STUD_SEX").val(),
            Address: $("#edit_STUD_ADDRESS").val(),
            ParentFullName: $("#edit_PARENT_FULLNAME").val(),
            ParentContact: $("#edit_PARENT_CONTACT").val(),
            Adviser: $("#edit_ADVISER").val()
        };

        // Perform validation if needed

        // Send updated data to the server
        $.ajax({
            type: "POST",
            url: "/api/mnhs/UpdateStudent",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response);
                $("#addStudentModal").modal("hide"); // Hide the edit modal dialog
                populateTable(); // Repopulate the table after successful update
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                alert("Error: " + errorThrown);
            }
        });
    });

    // Other functions and event handlers can be placed here

    // Function to handle form submission for adding a new student
    $("#btnSaveChanges").click(function () {
        var formData = {
            LRN: $("#STUD_LRN").val(),
            FirstName: $("#STUD_FNAME").val(),
            MiddleName: $("#STUD_MNAME").val(),
            LastName: $("#STUD_LNAME").val(),
            NameExtension: $("#STUD_EXT").val(),
            Grade: $("#STUD_GRADE").val(),
            Section: $("#STUD_SECTION").val(),
            Birthdate: $("#STUD_BIRTHDATE").val(),
            Sex: $("#STUD_SEX").val(),
            Address: $("#STUD_ADDRESS").val(),
            ParentFullName: $("#PARENT_FULLNAME").val(),
            ParentContact: $("#PARENT_CONTACT").val(),
            Adviser: $("#ADVISER").val()
        };

        var birthdate = new Date(formData.Birthdate);
        if (isNaN(birthdate.getTime())) {
            alert("Invalid birthdate format.");
            return;
        }

        var minDate = new Date("1753-01-01");
        var maxDate = new Date("9999-12-31");
        if (birthdate < minDate || birthdate > maxDate) {
            alert("Birthdate must be between January 1, 1753 and December 31, 9999.");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/api/mnhs/InsertData",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response);
                populateTable(); // Repopulate the table after successful insertion
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
                alert("Error: " + errorThrown);
            }
        });
    });
});
