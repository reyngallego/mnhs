﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="id_generator_maker.dashboard" %>

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
            <h1>Welcome to the Admin Dashboard!</h1>
            <p>This is a basic Bootstrap template for an admin dashboard with tabs on the left side.</p>
        </div>
    </div>

    <!-- Bootstrap JS dependencies (jQuery, Popper.js, Bootstrap JS) -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>

    <script>
        $(document).ready(function () {
            // Use delegated event handler for dynamically added elements
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

        function printID() {
            // Capture modal-header as canvas
            html2canvas(document.querySelector('.modal-header')).then(function (canvas) {
                var modalHeaderCanvas = canvas;

                // Create a new canvas to combine modal-header and id-card-front
                var combinedCanvas = document.createElement('canvas');
                var combinedContext = combinedCanvas.getContext('2d');

                // Set the combined canvas size
                combinedCanvas.width = modalHeaderCanvas.width;
                combinedCanvas.height = modalHeaderCanvas.height + document.getElementById('id-card-front').offsetHeight;

                // Draw modal-header and id-card-front on the combined canvas
                combinedContext.drawImage(modalHeaderCanvas, 0, 0);
                combinedContext.drawImage(document.getElementById('id-card-front'), 0, modalHeaderCanvas.height);

                // Convert the combined canvas to an image and trigger the print dialog
                var imgData = combinedCanvas.toDataURL('image/png');
                var windowContent = '<!DOCTYPE html>';
                windowContent += '<html>'
                windowContent += '<head><title>Print ID</title></head>';
                windowContent += '<body>'
                windowContent += '<img src="' + imgData + '">';
                windowContent += '</body>';
                windowContent += '</html>';

                var printWin = window.open('', '', 'width=' + combinedCanvas.width + ',height=' + combinedCanvas.height);
                printWin.document.open();
                printWin.document.write(windowContent);
                printWin.document.close();
                printWin.focus();
                printWin.print();
                printWin.close();
            });
        }



    </script>
</body>
</html>