<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="studentlist.aspx.cs" Inherits="WebApplication3.mnhs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

   <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <script src="js/student.js"></script>

    <script>
      
         
    </script>
</head>
<body>
    <h1 class="mt-5">Student List</h1>
                  <div class="add-student-button">
<button id="addStudentBtn" type="button" class="btn btn-primary" data-toggle="modal" data-target="#">Add Student</button>
</div>
     <table id="studentTable" class="table">
    <thead>
        <tr>
            <th>LRN</th>
            <th>Name</th>
            <th>Grade</th>
            <th>Section</th>
            <th>Adviser</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        <!-- Student data will be dynamically inserted here -->
    </tbody>
</table>

   <!-- Modal for editing student -->
<div class="modal fade" id="editStudentModal" tabindex="-1" role="dialog" aria-labelledby="editStudentModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="editStudentModalLabel">Edit Student Details</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!-- Student edit form -->
                <form id="editStudentForm">
                    <div class="form-group">
                        <label for="edit_STUD_LRN">LRN:</label>
                        <input type="text" class="form-control" id="edit_STUD_LRN">
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_FNAME">First Name:</label>
                        <input type="text" class="form-control" id="edit_STUD_FNAME">
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_MNAME">Middle Name:</label>
                        <input type="text" class="form-control" id="edit_STUD_MNAME">
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_LNAME">Last Name:</label>
                        <input type="text" class="form-control" id="edit_STUD_LNAME">
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_EXT">Name Extension:</label>
                        <input type="text" class="form-control" id="edit_STUD_EXT">
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_GRADE">Grade:</label>
                        <select class="form-control" id="edit_STUD_GRADE">
                            <option value="Grade 7">Grade 7</option>
                            <option value="Grade 8">Grade 8</option>
                            <option value="Grade 9">Grade 9</option>
                            <!-- Add more grade options as needed -->
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_SECTION">Section:</label>
                        <select class="form-control" id="edit_STUD_SECTION">
                            <option value="Section A">Section A</option>
                            <option value="Section B">Section B</option>
                            <option value="Section C">Section C</option>
                            <!-- Add more section options as needed -->
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_BIRTHDATE">Birthdate:</label>
            <input type="text" id="edit_STUD_BIRTHDATE" class="form-control" disabled>
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_SEX">Sex:</label>
                        <select class="form-control" id="edit_STUD_SEX">
                            <option value="Male">Male</option>
                            <option value="Female">Female</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="edit_STUD_ADDRESS">Address:</label>
                        <input type="text" class="form-control" id="edit_STUD_ADDRESS">
                    </div>

                    <!-- Parent details -->
                    <h5>Parent Information</h5>
                    <div class="form-group">
                        <label for="edit_PARENT_FULLNAME">Parent's Full Name:</label>
                        <input type="text" class="form-control" id="edit_PARENT_FULLNAME">
                    </div>
                    <div class="form-group">
                        <label for="edit_PARENT_CONTACT">Parent's Contact Number:</label>
                        <input type="text" class="form-control" id="edit_PARENT_CONTACT">
                    </div>

                    <!-- Adviser -->
                    <div class="form-group">
                        <label for="edit_ADVISER">Adviser:</label>
                        <select class="form-control" id="edit_ADVISER">
                            <option value="Mr. Smith">Mr. Smith</option>
                            <option value="Ms. Johnson">Ms. Johnson</option>
                            <!-- Add more adviser options as needed -->
                        </select>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                <button type="button" id="editSaveChanges" class="btn btn-primary">Save changes</button>
            </div>
        </div>
    </div>
</div>


    <!-- Modal for adding student -->
    <div class="modal fade" id="addStudentModal" tabindex="-1" role="dialog" aria-labelledby="addStudentModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addStudentModalLabel">Add New Student</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- Student fill-up form -->
                  <div class="modal-body">
    <!-- Student fill-up form -->
    <form>
        <div class="form-group">
            <label for="STUD_LRN">LRN:</label>
            <input type="text" class="form-control" id="STUD_LRN">
        </div>
        <div class="form-group">
            <label for="STUD_FNAME">First Name:</label>
            <input type="text" class="form-control" id="STUD_FNAME">
        </div>
        <div class="form-group">
            <label for="STUD_MNAME">Middle Name:</label>
            <input type="text" class="form-control" id="STUD_MNAME">
        </div>
        <div class="form-group">
            <label for="STUD_LNAME">Last Name:</label>
            <input type="text" class="form-control" id="STUD_LNAME">
        </div>
        <div class="form-group">
            <label for="STUD_EXT">Name Extension:</label>
            <input type="text" class="form-control" id="STUD_EXT">
        </div>
       <div class="form-group">
    <label for="STUD_GRADE">Grade:</label>
    <select class="form-control" id="STUD_GRADE">
        <option value="Grade 7">Grade 7</option>
        <option value="Grade 8">Grade 8</option>
        <option value="Grade 9">Grade 9</option>
        <!-- Add more grade options as needed -->
    </select>
</div>
<div class="form-group">
    <label for="STUD_SECTION">Section:</label>
    <select class="form-control" id="STUD_SECTION">
        <option value="Section A">Section A</option>
        <option value="Section B">Section B</option>
        <option value="Section C">Section C</option>
        <!-- Add more section options as needed -->
    </select>
</div>
        <div class="form-group">
            <label for="STUD_BIRTHDATE">Birthdate:</label>
            <input type="date" class="form-control" id="STUD_BIRTHDATE">
        </div>
        <div class="form-group">
            <label for="STUD_SEX">Sex:</label>
            <select class="form-control" id="STUD_SEX">
                <option value="Male">Male</option>
                <option value="Female">Female</option>
            </select>
        </div>
        <div class="form-group">
            <label for="STUD_ADDRESS">Address:</label>
            <input type="text" class="form-control" id="STUD_ADDRESS">
        </div>

        <!-- Parent details -->
        <h5>Parent Information</h5>
        <div class="form-group">
            <label for="PARENT_FULLNAME">Parent's Full Name:</label>
            <input type="text" class="form-control" id="PARENT_FULLNAME">
        </div>
        <div class="form-group">
            <label for="PARENT_CONTACT">Parent's Contact Number:</label>
            <input type="text" class="form-control" id="PARENT_CONTACT">
        </div>

        <!-- Adviser -->
        <div class="form-group">
            <label for="ADVISER">Adviser:</label>
            <select class="form-control" id="ADVISER">
                <option value="Mr. Smith">Mr. Smith</option>
                <option value="Ms. Johnson">Ms. Johnson</option>
                <!-- Add more adviser options as needed -->
            </select>
        </div>
    </form>
</div>

<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
<button type="button" id="btnSaveChanges" class="btn btn-primary">Save changes</button>
</div>
      


            </div>
        </div>
</body>
</html>
