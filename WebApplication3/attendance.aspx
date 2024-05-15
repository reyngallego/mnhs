﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="attendance.aspx.cs" Inherits="WebApplication3.mnhs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attendance</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="styles/attendance.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="js/attendance.js"></script> <!-- Link to custom JavaScript file -->


    <script>
        // Your JavaScript code can go here if needed
    </script>
</head>
<body>
    <div class="container">
    <h1 class="mt-5">Attendance</h1>
    <div class="form-group">
        <label for="searchInput">Enter Student LRN:</label>
        <div class="input-group">
            <input type="text" id="lrnInput" placeholder="Enter LRN">
            <div class="input-group-append">
                <button class="btn btn-primary" type="button" onclick="enterAttendance()">Enter</button>
                <button class="btn btn-danger" type="button" onclick="leaveAttendance()">Leave</button>
            </div>
        </div>
    </div>
</div>
        <!-- Status notification -->
                    <h5>Status:</h5>
        <div class="form-group">
            <div id="statusNotification" class="status-notification" style="display: none;"></div>
        </div>
        <div class="form-group">

        </div>
        <table id="attendanceTable" class="table">
            <thead>
                <tr>
                    <th>LRN</th>
                    <th>Name</th>
                    <th>Time</th>
                    <th>Date</th> <!-- Add new column for status -->
                    <th>Class</th>
                </tr>
            </thead>
            <tbody>
                <!-- Attendance data will be dynamically inserted here -->
            </tbody>
        </table>
    </div>

    <!-- Modal for adding new attendance record -->
    <div class="modal fade" id="addAttendanceModal" tabindex="-1" role="dialog" aria-labelledby="addAttendanceModalLabel" aria-hidden="true">
        <!-- Modal content -->
    </div>

    <!-- Include any other modals or scripts as needed -->
</body>
</html>