<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="id_generator_maker.dashboard" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Admin Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom CSS for Admin Dashboard -->
    <link href="styles/styles.css" rel="stylesheet">
    <!-- <link href="styles/dashboard_1.css" rel="stylesheet"> -->
</head>
<body>
    <div class="wrapper">
        <div class="sidebar">
            <ul class="nav">
                <img src="gallery/mnhslogo.png" alt="Logo" class="img-fluid mt-3" style="width: 150px;">
                <div id="mwnhs-banner"><h2>Moonwalk National High School</h2></div>
                
                <h6>School ID: 320203</h6>

                <li><a href="#" class="active" id="dashboard">      <img src="gallery/dashboard.png" alt="dashboard" width="30" height="30">&nbsp;   Dashboard</a></li>
                <li><a href="#" class="active" id="attendance">     <img src="gallery/timer.png" alt="attendance" width="30" height="30">&nbsp;      Attendance</a></li>
                <li><a href="#" class="active" id="addStudentRecord"><img src="gallery/student.png" alt="dashboard" width="30" height="30">&nbsp;    Student</a></li>
                <li><a href="#" class="active" id="reports">        <img src="gallery/growth.png" alt="dashboard" width="30" height="30">&nbsp;      Reports</a></li>
                <li><a href="#" class="active" id="generateId">     <img src="gallery/qr-code.png" alt="dashboard" width="30" height="30">&nbsp;     ID Generator</a></li>
                <li><a href="#" class="active" id="logout">         <img src="gallery/logout.png" alt="dashboard" width="30" height="30">&nbsp;      Log out</a></li>
            </ul>   
        </div>
        <div class="content" id="mainContent">
        </div>
    </div>

    <!-- Bootstrap JS dependencies (jQuery, Popper.js, Bootstrap JS) -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
     <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script>
        $(document).ready(function () {
            // Use delegated event handler for dynamically added elements
            $(document).ready(function () {
                $("#mainContent").load("dashboard/dashboard.html");
            });

            $(document).on("click", "#dashboard", function (e) {
                e.preventDefault();
                $("#mainContent").load("dashboard/dashboard.html");
            });
            $(document).on("click", "#addStudentRecord", function (e) {
                e.preventDefault();
                $("#mainContent").load("dashboard/add_student_record.html");
            });
            $(document).on("click", "#generateId", function (e) {
                e.preventDefault();
                $("#mainContent").load("dashboard/generate_id.html");
            });
        });
    </script>
</body>
</html>