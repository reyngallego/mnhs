$(document).ready(function () {

    var searchInput = $("#searchInput");
    var sortSelect = $("#sortSelect");

    // Function to populate the table with student data
    function populateTable() {
        var searchTerm = searchInput.val().toLowerCase();
        var sortColumn = sortSelect.val();
        var sortOrder = 'ASC'; // You can modify this if you need a different sort order

        $.ajax({
            type: "GET",
            url: "/api/mnhs/GetStudents", // Endpoint to retrieve student data
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                searchTerm: searchTerm,
                sortColumn: sortColumn,
                sortOrder: sortOrder
            },
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
                    event.preventDefault();
                    var studentId = $(this).data("student-id");
                    var confirmEdit = confirm("Are you sure you want to edit this student?");
                    if (confirmEdit) {
                        openEditModal(studentId);
                    }
                });

                $("#addStudentBtn").click(function (event) {
                    event.preventDefault();
                    var confirmAdd = confirm("Are you sure you want to add a new student?");
                    if (confirmAdd) {
                        $('#addStudentModal').modal('show');
                    }
                });

                $(document).on("click", ".delete-btn", function (event) {
                    event.preventDefault();
                    var studentId = $(this).data("student-id");
                    var confirmDelete = confirm("Are you sure you want to delete this student?");
                    if (confirmDelete) {
                        $.ajax({
                            type: "DELETE",
                            url: "/api/mnhs/DeleteStudent?LRN=" + studentId,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                alert(response);
                                populateTable();
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                alert("Error: " + errorThrown);
                            }
                        });
                    }
                });

                paginateTable(); // Call pagination after populating the table
            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Error: " + errorThrown);
            }
        });
    }

    // Function to open the edit modal dialog with student data
    function openEditModal(LRN) {
        $.ajax({
            type: "GET",
            url: "/api/mnhs/GetStudentByLRN?LRN=" + LRN,
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
                $("#editStudentModal").modal("show");
            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Error: " + errorThrown);
            }
        });
    }

    // Function to handle form submission in the edit modal
    $("#editSaveChanges").click(function () {
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
        if (!validateLRN(formData.LRN) || !validateParentContact(formData.ParentContact)) {
            alert("Validation error in LRN or Parent Contact.");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/api/mnhs/UpdateStudent",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                alert(response);
                $("#editStudentModal").modal("hide");
                populateTable();
            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Error: " + errorThrown);
            }
        });
    });

    // Function to handle form submission for adding a new student
    $("#btnSaveChanges").click(function (event) {
        event.preventDefault();
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

        if (!validateLRN(formData.LRN) || !validateParentContact(formData.ParentContact)) {
            alert("Validation error in LRN or Parent Contact.");
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
                populateTable();
                $('#addStudentModal').modal('hide');
                clearModalForm();
            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Error: " + errorThrown);
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
        $("#STUD_GRADE").val("Grade 7");
        $("#STUD_SECTION").val("Section A");
        $("#STUD_BIRTHDATE").val("");
        $("#STUD_SEX").val("Male");
        $("#STUD_ADDRESS").val("");
        $("#PARENT_FULLNAME").val("");
        $("#PARENT_CONTACT").val("");
        $("#ADVISER").val("Mr. Smith");
    }

    // Function to validate LRN
    function validateLRN(lrNumber) {
        var regex = /^[0-9]+$/;
        return regex.test(lrNumber);
    }

    // Function to validate Parent Contact Number
    function validateParentContact(contactNumber) {
        var regex = /^[0-9]{1,12}$/;
        return regex.test(contactNumber);
    }

    // Pagination functionality
    function paginateTable() {
        var rowsPerPage = 10;
        var rows = $("#studentTable tbody tr");
        var rowsCount = rows.length;
        var pageCount = Math.ceil(rowsCount / rowsPerPage);
        var pagination = $("#pagination");

        pagination.empty();

        for (var i = 1; i <= pageCount; i++) {
            pagination.append("<li class='page-item'><a class='page-link' href='#'>" + i + "</a></li>");
        }

        pagination.find("li:first").addClass("active");

        rows.hide();
        rows.slice(0, rowsPerPage).show();

        pagination.find("a").click(function (e) {
            e.preventDefault();
            var page = $(this).text();

            var start = (page - 1) * rowsPerPage;
            var end = start + rowsPerPage;

            rows.hide();
            rows.slice(start, end).show();

            pagination.find("li").removeClass("active");
            $(this).parent().addClass("active");
        });
    }

    // Event handlers for search and sort
    searchInput.on("input", populateTable);
    sortSelect.on("change", populateTable);

    // Call the populateTable function when the page loads
    populateTable();
});
