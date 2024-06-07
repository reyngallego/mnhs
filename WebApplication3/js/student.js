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
                        "<td><button class='btn btn-primary edit-btn' data-student-id='" + student.LRN + "'>Edit</button> <button class='btn btn-danger delete-btn' data-student-id='" + student.LRN + "'>Delete</button></td>" +
                        "</tr>";
                    $("#studentTable tbody").append(row);
                });

                // Unbind previously bound event handlers to avoid multiple bindings
                $(document).off("click", ".edit-btn");
                $(document).off("click", "#addStudentBtn");
                $(document).off("click", ".delete-btn");

                // Rebind event handlers
                $(document).on("click", ".edit-btn", function (event) {
                    console.log("Edit button clicked"); // Log when the edit button is clicked

                    // Prevent default behavior of the button
                    event.preventDefault();
                    console.log("Default behavior prevented"); // Log after preventing default behavior

                    var studentId = $(this).data("student-id");
                    console.log("Student ID: " + studentId); // Log the student ID

                    // Display confirmation dialog
                    var confirmEdit = confirm("Are you sure you want to edit this student?");
                    console.log("User confirmation: " + confirmEdit); // Log the user's confirmation response

                    if (confirmEdit) {
                        console.log("User confirmed to edit the student with ID: " + studentId); // Log if the user confirms
                        openEditModal(studentId);
                    } else {
                        console.log("User canceled the edit action for student with ID: " + studentId); // Log if the user cancels
                    }
                });

                $("#addStudentBtn").click(function (event) {
                    event.preventDefault();
                    console.log("Add Student button clicked"); // Log the button click

                    // Display confirmation dialog
                    var confirmAdd = confirm("Are you sure you want to add a new student?");
                    console.log("User confirmation: " + confirmAdd); // Log the user's confirmation response

                    if (confirmAdd) {
                        console.log("User confirmed to add a new student"); // Log if the user confirms
                        // Open the "Add Student" modal only if the user confirms
                        $('#addStudentModal').modal('show');
                    } else {
                        console.log("User canceled the add student action"); // Log if the user cancels
                        // If the user cancels, do nothing (no need to open the modal)
                    }
                });


                $(document).on("click", ".delete-btn", function (event) {
                    console.log("Delete button clicked"); // Log when the delete button is clicked

                    event.preventDefault(); // Prevent default behavior of the button
                    console.log("Default behavior prevented"); // Log after preventing default behavior

                    var studentId = $(this).data("student-id");
                    console.log("Student ID: " + studentId); // Log the student ID

                    // Display confirmation dialog
                    var confirmDelete = confirm("Are you sure you want to delete this student?");
                    console.log("User confirmation: " + confirmDelete); // Log the user's confirmation response

                    if (confirmDelete) {
                        console.log("User confirmed to delete the student with ID: " + studentId); // Log if the user confirms
                        // If user confirms deletion, proceed with AJAX call to delete student
                        $.ajax({
                            type: "DELETE",
                            url: "/api/mnhs/DeleteStudent?LRN=" + studentId,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                alert(response); // Alert success message
                                populateTable(); // Repopulate the table after successful deletion
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                console.log(xhr.responseText); // Log the error response
                                alert("Error: " + errorThrown); // Alert error message
                            }
                        });
                    } else {
                        console.log("User canceled the delete action for student with ID: " + studentId); // Log if the user cancels
                    }
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
    function openEditModal(LRN) {
        // Retrieve student data by LRN and populate the edit modal fields
        // You need to implement this part based on your backend logic
        // Example:
        $.ajax({
            type: "GET",
            url: "/api/mnhs/GetStudentByLRN?LRN=" + LRN, // Pass LRN as a query parameter
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (student) {
                // Populate the edit modal fields with student data
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
        var lrn = $("#STUD_LRN").val();
        var parentContact = $("#PARENT_CONTACT").val();

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

        // Perform LRN and Parent Contact validation
        if (!validateLRN(formData.LRN)) {
            alert("LRN should contain numbers only.");
            return;
        }

        if (!validateParentContact(formData.ParentContact)) {
            alert("Parent's contact number should contain numbers only and have a maximum of 11 digits.");
            return;
        }

        // Send updated data to the server
        $.ajax({
            type: "POST",
            url: "/api/mnhs/UpdateStudent",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response); // Alert success message
                $("#editStudentModal").modal("hide"); // Hide the edit modal dialog
                populateTable(); // Repopulate the table after successful update
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText); // Log the error response
                alert("Error: " + errorThrown); // Alert error message
            }
        });
    });

    // Function to handle form submission for adding a new student
    $("#btnSaveChanges").click(function (event) {
        event.preventDefault(); // Prevent default behavior of the button
        console.log("Add Student button clicked"); // Log when the Add Student button is clicked

        // Display confirmation dialog
        var confirmAdd = confirm("Are you sure you want to add a new student?");
        console.log("User confirmation: " + confirmAdd); // Log the user's confirmation response

        if (!confirmAdd) {
            console.log("User canceled the add student action"); // Log if the user cancels
            return; // If the user cancels, do nothing
        }

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

        // Perform LRN and Parent Contact validation
        if (!validateLRN(formData.LRN)) {
            alert("LRN should contain numbers only.");
            return;
        }

        if (!validateParentContact(formData.ParentContact)) {
            alert("Parent's contact number should contain numbers only and have a maximum of 11 digits.");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/api/mnhs/InsertData",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response); // This will show the success message returned from the server
                populateTable(); // Repopulate the table after successful insertion
                $('#addStudentModal').modal('hide'); // Hide the modal after saving changes
                clearModalForm(); // Clear the modal form fields
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status === 400) {
                    // LRN already exists, show warning message
                    alert(xhr.responseJSON.Message);
                } else {
                    console.log(xhr.responseText);
                    alert("Error: " + errorThrown);
                }
            }
        });
    });

    // Function to clear the modal form fields
    function clearModalForm() {
        $("#STUD_LRN").val("");
        $("#STUD_FNAME").val("");
        $("#STUD_MNAME").val("");
        $("#STUD_LNAME").val("");
        $("#STUD_EXT").val("");
        $("#STUD_GRADE").val("Grade 7"); // Reset to default value
        $("#STUD_SECTION").val("Section A"); // Reset to default value
        $("#STUD_BIRTHDATE").val("");
        $("#STUD_SEX").val("Male"); // Reset to default value
        $("#STUD_ADDRESS").val("");
        $("#PARENT_FULLNAME").val("");
        $("#PARENT_CONTACT").val("");
        $("#ADVISER").val("Mr. Smith"); // Reset to default value
    }

    // Function to validate LRN
    function validateLRN(lrNumber) {
        var regex = /^[0-9]+$/; // Regex to match numbers only
        return regex.test(lrNumber);
    }

    // Function to validate Parent Contact Number
    function validateParentContact(contactNumber) {
        var regex = /^[0-9]{1,12}$/; // Regex to match numbers only and maximum of 11 digits
        return regex.test(contactNumber);
    }

});
