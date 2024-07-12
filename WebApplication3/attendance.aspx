<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="attendance.aspx.cs" Inherits="WebApplication3.attendance" %>

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
      var currentState = 'enter'; // Initialize the current state to 'enter'

      function enterOrLeaveAttendance() {
          if (currentState === 'enter') {
              enterAttendance();
          } else {
              leaveAttendance();
          }
      }

      function toggleState() {
          currentState = currentState === 'enter' ? 'leave' : 'enter';
          $('#lrnInput').attr('placeholder', currentState === 'enter' ? 'Enter LRN' : 'Leave LRN');
      }

      $('#toggleButton').on('click', function () {
          toggleState();
      });

      $('#lrnInput').on('keypress', function (event) {
          if (event.which === 13) { // Check if the pressed key is Enter (key code 13)
              event.preventDefault(); // Prevent default form submission behavior
              scanLRN(); // Call the scanLRN function
          }
      });

      function scanLRN() {
          var lrn = $('#lrnInput').val().trim();

          if (lrn !== '') {
              if (currentState === 'enter') {
                  enterAttendance();
              } else {
                  leaveAttendance();
              }
              $('#lrnInput').val(''); // Clear the input field
          } else {
              alert('Please enter LRN first.');
          }
      }
  </script>
</head>
<body>
    <header>
        <div class="header1">
            <div class="header-overlay"></div>
            <h1>Attendance</h1>
        </div>
    </header>
    <div class="container">
        <div class="form-group">
            <div class="header2">
                <h1>SCAN RESULT</h1>
            </div>
            <label for="searchInput">Scan Student LRN:</label>
            <div class="input-group">
                <input type="text" id="lrnInput" placeholder="Enter LRN" class="long-input" />
                <div class="input-group-append">
                    <button class="btn btn-primary" type="button" onclick="scanLRN()">Scan</button>
                </div>
            </div>
        </div>

        <button id="toggleButton" class="btn btn-secondary" type="button">TIMEIN/TIMEOUT</button> <br />
        <div class="stud-attendance">
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
    </div>


        <!-- Status notification -->
    

    <!-- Modal for adding new attendance record -->
    <div class="modal fade" id="addAttendanceModal" tabindex="-1" role="dialog" aria-labelledby="addAttendanceModalLabel" aria-hidden="true">
        <!-- Modal content -->
    </div>

    <!-- Include any other modals or scripts as needed -->
</body>
</html>
