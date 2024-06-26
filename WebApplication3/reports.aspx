﻿<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reports</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
<link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/jquery/dist/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/moment/min/moment.min.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>


    <link href="styles/reports.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->

   <script>
       $(document).ready(function () {
           $('#dateRange').daterangepicker({
               locale: {
                   format: 'MM/DD/YYYY'
               },
               opens: 'center', // This should center the date picker
               autoUpdateInput: false // Prevents the input field from being automatically updated
           }, function (start, end, label) {
               $('#dateRange').val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
           });

           $('#calendarIcon').click(function () {
               $('#dateRange').focus(); // Trigger focus event on the hidden input field
           });
       });

       function searchStudent() {
           var lrn = document.getElementsByName('search')[0].value;
           var dateRange = document.getElementById('dateRange').value;
           var dates = dateRange.split(' - ');
           var startDate = dates[0];
           var endDate = dates[1];

           $.ajax({
               url: '/api/student/search',
               method: 'GET',
               data: {
                   lrn: lrn,
                   startDate: startDate,
                   endDate: endDate
               },
               success: function (data) {
                   $('#studentName').text('Student Name: ' + data.Student.FirstName + ' ' + data.Student.LastName);
                   $('#learnerReferenceNumber').text('Learner Reference Number: ' + data.Student.LRN);
                   $('#gradeAndSection').text('Total Present: ' + data.Student.TotalPresent + ', Total Absent: ' + data.Student.TotalAbsent);

                   // Populate attendance table
                   var tbody = $('#attendanceTable tbody');
                   tbody.empty();
                   data.AttendanceRecords.forEach(function (record) {
                       var row = '<tr>' +
                           '<td>' + new Date(record.Date).toLocaleDateString() + '</td>' +
                           '<td>' + record.Day + '</td>' +
                           '<td>' + record.Status + '</td>' +
                           '<td>' + (record.TimeIn ? record.TimeIn : '') + '</td>' +
                           '<td>' + (record.TimeOut ? record.TimeOut : '') + '</td>' +
                           '</tr>';
                       tbody.append(row);
                   });
               },
               error: function () {
                   alert('Student not found');
               }
           });
       }
   </script>

</head>
<body>
    <h2>Reports</h2>
    <div class="topnav">
        <div class="header">
            <h6>Generate Reports</h6>
        </div>
        <div class="individual">
            <h4>Individual Attendance Records</h4>
        </div>
        <div class="container">
            <div class="container-actions">
                <div id="actions">
                    <img src="/images/icons/export.png" alt="Share" style="width: 20px; height: 20px; vertical-align: middle;margin:0px 5px"> <span style="font-size: 20px; color: black;"></span>
                    <img src="/images/icons/download.png" alt="Download" style="width: 20px; height: 25px; vertical-align: middle;margin:0px 5px"> <span style="font-size: 20px; color: black;"></span>
                    <img src="/images/icons/printer.png" alt="Printer" style="width: 20px; height: 20px; vertical-align: middle;margin:0px 10px"> <span style="font-size: 20px; color: black;"></span>
                </div>
           <div class="search-container">
    <form action="javascript:searchStudent();">
        <input type="text" placeholder="Search Student" name="search">
        <span class="calendar-icon" id="calendarIcon" style="cursor: pointer;">&#128197;</span> <!-- Calendar icon -->
        <input type="text" id="dateRange" style="display:none;">
    </form>
</div>


            </div>
            <div class="studentdata1">
                <h4 id="studentName">Student Name: </h4>
                <h4 id="learnerReferenceNumber">Learner Reference Number: </h4>
                <h4 id="gradeAndSection">Grade and Section:</h4>

            </div>
        </div>
    </div>

    <div id="report-details">
        <div class="box1">
            <!-- Detailed Data -->
            <h4>Detailed Data</h4>
            <table id="attendanceTable">
                <thead>
                    <tr class="fill">
                        <th>Date</th>
                        <th>Days</th>
                        <th>Status</th>
                        <th>Time In</th>
                        <th>Time Out</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- Data will be populated here -->
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
