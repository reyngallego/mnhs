<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="WebApplication3.admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" integrity="sha512-xwTsS3XW/2FRcPoQIPb9RGFYK3aZ+P2eKgDckwX1FmrsDpiQExJH/9cAoTC4Ld+o2dJ0tN2vJvAKB9qQvJZEMQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <link rel="stylesheet" type="text/css" href="styles/admin.css" />
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.0/chart.min.js"></script>
    <!-- Popper.js -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/js/videos.js"></script>
    <script type="text/javascript" src="/js/admin.js"></script>
    <script type="text/javascript" src="/js/reports.js"></script>


</head>

<body>

    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-maroon">
            <a class="navbar-brand text-white" href="#">Pup Paranaque Campus Queue Management System</a>


        </nav>

        <div class="container mt-4" />
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <a class="nav-link active" data-toggle="tab" href="#employees">
                    <img src="images/icons/employee.png" alt="Employee Icon" width="20" height="20"/>
                    Employees
                </a>

            </li>

            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#videos">
                    <img src="images/icons/video.png" alt="Video Icon" width="20" height="20"/>
                    Videos
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#reports">
                    <img src="images/icons/reports.png" alt="Report Icon" width="20" height="20"/>
                    Reports
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="#" data-toggle="modal" data-target="#logoutModal">
                    <img src="images/icons/logout.png" alt="Logout Icon" width="20" height="20"/>
                    Logout
                </a>
            </li>

        </ul>

        <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="logoutModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="logoutModalLabel">Logout Confirmation</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        Are you sure you want to log out of your session?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">
                            <img src="images/icons/cancel.png" alt="Cancel Icon" width="20" height="20"/>
                            Cancel
                        </button>

                        <button type="button" class="btn btn-primary" id="confirmLogoutBtn">
                            <img src="images/icons/logout.png" alt="Logout Icon" width="20" height="20"/>
                            Logout
                        </button>


                    </div>
                </div>
            </div>
        </div>

        <div class="tab-content mt-2">
            <div id="employees" class="tab-pane fade show active">
                <h2>Employee Management</h2>
                <button type="button" id="add-employee-button" class="btn btn-primary" data-toggle="modal" data-target="#addEmployeeModal">
                    <img src="images/icons/register.png" alt="Add Employee Icon" width="20" height="20">
                    Add Employee
                </button>

                <div class="modal fade" id="addEmployeeModal" tabindex="-1" role="dialog" aria-labelledby="addEmployeeModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="addEmployeeModalLabel">
                                    <img src="images/icons/register.png" alt="Add Employee Icon" width="20" height="20"/>
                                    Add Employee
                                </h5>

                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
                                </button>
                            </div>
                            <div class="modal-body" style="max-height: 500px; overflow-y: auto;">
                                <!-- Add form fields for adding a new employee -->
                                <div class="form-group">
                                    <label for="add-username-input">
                                        <img src="images/icons/employeeid.png" alt="Employee ID Icon" width="20" height="20"/>
                                        Employee ID:
                                    </label>
                                    <input type="text" class="form-control" id="add-username-input" />
                                </div>
                                <div class="form-group">
                                    <label for="add-password-input">
                                        <img src="images/icons/Password.png" alt="Password Icon" width="20" height="20"/>
                                        Password:
                                    </label>
                                    <input type="password" class="form-control" id="add-password-input" />
                                </div>
                                <div class="form-group">
                                    <label for="add-confirm-password-input">
                                        <img src="images/icons/Password.png" alt="Confirm Password Icon" width="20" height="20"/>
                                        Confirm Password:
                                    </label>
                                    <input type="password" class="form-control" id="add-confirm-password-input" />
                                </div>
                                <div class="form-group">
                                    <label for="add-firstname-input">
                                        <img src="images/icons/name.png" alt="First Name Icon" width="20" height="20">
                                        First Name:
                                    </label>
                                    <input type="text" class="form-control" id="add-firstname-input" />
                                </div>
                                <div class="form-group">
                                    <label for="add-lastname-input">
                                        <img src="images/icons/name.png" alt="Last Name Icon" width="20" height="20"/>
                                        Last Name:
                                    </label>
                                    <input type="text" class="form-control" id="add-lastname-input" />
                                </div>
                                <div class="form-group">
                                    <label for="add-department-input">
                                        <img src="images/icons/department.png" alt="Department Icon" width="20" height="20"/>
                                        Department:
                                    </label>

                                    <select class="form-control" id="add-department-input">
                                        <option value="director">Director</option>
                                        <option value="studentaffairsandservices">Student Affairs and Services</option>
                                        <option value="registrar">Registrar</option>
                                        <option value="cashier">Cashier</option>
                                    </select>
                                </div>
                                <div class="form-group">
    <label for="add-profile-image-input">
        <img src="images/icons/add.png" alt="Upload Icon" width="20" height="20"/>
        Upload Profile Image:
    </label>
    <input type="file" class="form-control-file" id="add-profile-image-input" accept="image/*" />
</div>
                            </div>
                            <div class="modal-footer">
                               <button type="button" class="btn btn-secondary" data-dismiss="modal">
    <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/> Close
</button>

<button type="button" class="btn btn-primary" id="add-save-button">
    <img src="images/icons/Add.png" alt="Add Employee Icon" width="20" height="20"/> Add Employee
</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Edit Employee Modal -->

                <div class="modal fade" id="editEmployeeModal" tabindex="-1" role="dialog" aria-labelledby="editEmployeeModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                               <h5 class="modal-title" id="editEmployeeModalLabel">
    <img src="images/icons/register.png" alt="Edit Employee Icon" width="20" height="20"/> Edit Employee
</h5>

<button type="button" class="close" data-dismiss="modal" aria-label="Close">
    <span aria-hidden="true">
        <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
    </span>
</button>
                            </div>
            <div class="modal-body" style="max-height: 500px; overflow-y: auto;">
                                <!-- Edit form fields go here -->
                                <div class="form-group">
    <label for="edit-username-input">
        <img src="images/icons/employeeid.png" alt="Employee ID Icon" width="20" height="20"/> EmployeeId:
    </label>
    <input type="text" class="form-control" id="edit-username-input" />
</div>

                               <div class="form-group">
    <label for="edit-new-password-input">
        <img src="images/icons/Password.png" alt="Password Icon" width="20" height="20"/> New Password:
    </label>
    <input type="password" class="form-control" id="edit-new-password-input" />
</div>

<div class="form-group">
    <label for="edit-confirm-password-input">
        <img src="images/icons/Password.png" alt="Password Icon" width="20" height="20"/> Confirm Password:
    </label>
    <input type="password" class="form-control" id="edit-confirm-password-input" />
</div>

<div class="form-group">
    <label for="edit-firstname-input">
        <img src="images/icons/name.png" alt="First Name Icon" width="20" height="20"/> First Name:
    </label>
    <input type="text" class="form-control" id="edit-firstname-input" />
</div>

<div class="form-group">
    <label for="edit-lastname-input">
        <img src="images/icons/name.png" alt="Last Name Icon" width="20" height="20"/> Last Name:
    </label>
    <input type="text" class="form-control" id="edit-lastname-input" />
</div>

<div class="form-group">
    <label for="edit-department-input">
        <img src="images/icons/department.png" alt="Department Icon" width="20" height="20"/> Department:
    </label>
    <select class="form-control" id="edit-department-input">
        <option value="director">Director</option>
        <option value="student_affairs">Student Affairs and Services</option>
        <option value="registrar">Registrar</option>
        <option value="cashier">Cashier</option>
    </select>
</div>
                                <div class="form-group">
                                    <label for="add-profile-image-input">
        <img src="images/icons/add.png" alt="Upload Icon" width="20" height="20"/>
        Change Profile Image:
    </label>
    <input type="file" class="form-control-file" id="edit-profile-image-input" accept="image/*" />

                                </div>
                            </div>
                            <div class="modal-footer">
                               <button type="button" class="btn btn-secondary" data-dismiss="modal">
    <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/> Close
</button>

<button type="button" class="btn btn-primary" id="edit-save-button">
    <img src="images/icons/add.png" alt="Save Icon" width="20" height="20"/> Save Changes
</button>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="confirmEditModal" tabindex="-1" role="dialog" aria-labelledby="confirmEditModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="confirmEditModalLabel">
    <img src="images/icons/register.png" alt="Confirm Edit Icon" width="20" height="20"/> Confirm Edit
</h5>

<button type="button" class="close" data-dismiss="modal" aria-label="Close">
    <span aria-hidden="true">
        <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
    </span>
</button>
                            </div>
                            <div class="modal-body">
                                Are you sure you want to edit this employee?
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">
    <img src="images/icons/cancel.png" alt="Cancel Icon" width="20" height="20"/> Cancel
</button>

<button type="button" class="btn btn-primary" id="confirm-edit-button">
    <img src="images/icons/Edit.png" alt="Confirm Edit Icon" width="20" height="20"/> Confirm Edit
</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Delete Employee Modal -->
                <div class="modal fade" id="deleteEmployeeModal" tabindex="-1" role="dialog" aria-labelledby="deleteEmployeeModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
<h5 class="modal-title" id="deleteEmployeeModalLabel">
    <img src="images/icons/delete.png" alt="Delete Icon" width="20" height="20"/>
    Delete Employee
</h5>
<button type="button" class="close" data-dismiss="modal" aria-label="Close">
    <span aria-hidden="true">
        <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
    </span>
</button>

                            </div>
                            <div class="modal-body">
                                <p>Are you sure you want to delete this employee?</p>
                            </div>
                            <div class="modal-footer">
                               <button type="button" class="btn btn-secondary" data-dismiss="modal">
    <img src="images/icons/cancel.png" alt="Cancel Icon" width="20" height="20"/>
    Cancel
</button>
<button type="button" class="btn btn-danger" id="confirm-delete-button">
    <img src="images/icons/delete.png" alt="Delete Icon" width="20" height="20"/>
    Delete
</button>
                            </div>
                        </div>
                    </div>
                </div>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>No.</th>
                            <th>EmployeeId</th>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Department</th>
                            <th>Image</th>
                            <th>Actions</th>

                        </tr>
                    </thead>
                    <tbody id="employee-list">
                        <!-- Employee data will be displayed here -->
                    </tbody>
                </table>
            </div>
            <div id="reports" class="tab-pane fade">
                <h2>Reports Management</h2>

                <div class="form-group">
                    <label for="dateFilter">Select Date:</label>
                    <input type="date" class="form-control" id="dateFilter" />
                </div>

                <div class="form-group">
                    <label>Filter by Department:</label>
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input department-checkbox" id="registrarCheckbox" data-department="registrar"/>
                        <label class="form-check-label" for="registrarCheckbox">Registrar</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input department-checkbox" id="cashierCheckbox" data-department="cashier"/>
                        <label class="form-check-label" for="cashierCheckbox">Cashier</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input department-checkbox" id="directorCheckbox" data-department="director"/>
                        <label class="form-check-label" for="directorCheckbox">Director</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input department-checkbox" id="studentAffairsCheckbox" data-department="student_affairs"/>
                        <label class="form-check-label" for="studentAffairsCheckbox">Student Affairs and Services</label>
                    </div>
                </div>

                <table class="table table-bordered" id="reports-table">
                    <thead>
                        <tr>
                            <th>ReportID</th>
                            <th>QueueTicket</th>
                            <th>Department</th>
                            <th>Done Date</th>
                            <th>time finished</th>

                        </tr>
                    </thead>
                    <tbody id="reports-list">
                        <!-- Reports data will be displayed here -->
                    </tbody>
                </table>

            </div>


            <div id="videos" class="tab-pane fade">
                <h2>Video Management</h2>

<button type="button" id="upload-video-button" class="btn btn-primary" data-toggle="modal" data-target="#uploadVideoModal">
    <img src="images/icons/add.png" alt="Upload Icon" width="20" height="20"/>
    Upload Video
</button>

                <!-- Video Upload Modal -->
                <div class="modal fade" id="uploadVideoModal" tabindex="-1" role="dialog" aria-labelledby="uploadVideoModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                               <h5 class="modal-title" id="uploadVideoModalLabel">
    <img src="images/icons/video.png" alt="Upload Video Icon" width="20" height="20"/>
    Upload Video
</h5>
<button type="button" class="close" data-dismiss="modal" aria-label="Close">
    <span aria-hidden="true">
        <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
    </span>
</button>
                            </div>
                            <div class="modal-body">
                              <form id="uploadVideoForm">
    <div class="form-group">
        <label for="videoNameInput">
            <img src="images/icons/video.png" alt="Video Icon" width="20" height="20"/>
            Video Name:
        </label>
        <input type="text" class="form-control" id="videoNameInput" required />
    </div>
    <div class="form-group">
        <label for="videoDescriptionInput">
            <img src="images/icons/description.png" alt="Description Icon" width="20" height="20"/>
            Description:
        </label>
        <textarea class="form-control" id="videoDescriptionInput" required></textarea>
    </div>
    <div class="form-group">
        <label for="videoFileInput">
            <img src="images/icons/videofile.png" alt="Video File Icon" width="20" height="20"/>
            Select Video:
        </label>
        <input type="file" class="form-control-file" id="videoFileInput" accept="video/*" required />
    </div>
</form>

                            </div>
                            <div class="modal-footer">
                               <button type="button" class="btn btn-secondary" data-dismiss="modal">
    <img src="images/icons/cancel.png" alt="Close Icon" width="20" height="20"/>
    Close
</button>
<button type="button" class="btn btn-primary" id="submitVideoButton">
    <img src="images/icons/videofile.png" alt="Upload Icon" width="20" height="20"/>
    Upload Video
</button>

                            </div>
                        </div>
                    </div>
                </div>
                <!-- Loading Modal -->
                <div class="modal fade" id="loadingModal" tabindex="-1" role="dialog" aria-labelledby="loadingModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="loadingModalLabel">Uploading Video</h5>
                            </div>
                            <div class="modal-body">
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Delete Video Modal -->
                <div class="modal fade" id="deleteVideoModal" tabindex="-1" role="dialog" aria-labelledby="deleteVideoModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="deleteVideoModalLabel">Delete Video</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <p>Are you sure you want to delete this video?</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                <button type="button" class="btn btn-danger" id="confirm-delete-video-btn">Delete</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Select Video Modal -->
                <div class="modal fade" id="selectVideoModal" tabindex="-1" role="dialog" aria-labelledby="selectVideoModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="selectVideoModalLabel">Select Video</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <p>Do you want to select this video?</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                <button type="button" class="btn btn-success" id="confirm-select-video-btn">Select</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal fade" id="videoPlayerModal" tabindex="-1" role="dialog" aria-labelledby="videoPlayerModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="videoPlayerModalLabel">Video Player</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <!-- Video player goes here -->
                                <video id="videoPlayer" controls style="width: 100%;">
                                    Your browser does not support the video tag.
                                </video>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Video Table -->
                <table class="table table-bordered mt-3" id="video-table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Video Name</th>
                            <th>Description</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody id="video-list">
                        <!-- Video data will be displayed here -->
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
