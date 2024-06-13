<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="WebApplication3.mnhs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attendance</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="styles/custom.css" rel="stylesheet" />
    <link href="styles/dashboard.css" rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="js/dashboard.js"></script> 

</head>
     <script>
         $(document).ready(function () {
             fetchDashboardData();
         });

         function fetchDashboardData() {
             $.ajax({
                 url: '/api/generateid/GetDashboardData',
                 method: 'GET',
                 success: function (data) {
                     renderStudentSummary(data.StudentSummary);
                     renderAttendanceSummary(data.AttendanceSummary, data.NotTimedInCount);
                     renderBarChart(data.StudentSummary.Grades);
                     renderPieChart(data.NotTimedInCount, calculateTotalStudents(data.StudentSummary.Grades) - data.NotTimedInCount);
                 },
                 error: function () {
                     alert('Failed to fetch dashboard data');
                 }
             });
         }

         function renderStudentSummary(data) {
             document.getElementById("totalStudents").textContent = calculateTotalStudents(data.Grades);
             document.getElementById("maleStudents").textContent = calculateMaleStudents(data.Grades);
             document.getElementById("femaleStudents").textContent = calculateFemaleStudents(data.Grades);
         }

         function renderAttendanceSummary(attendanceData, notTimedInCount) {
             document.getElementById("timedInCount").textContent = attendanceData[0].TotalTimeIn;
             document.getElementById("notTimedInCount").textContent = notTimedInCount;
             document.getElementById("timedOutCount").textContent = attendanceData[0].TotalTimeOut;
             document.getElementById("notTimedOutCount").textContent = attendanceData[0].TotalRecords - attendanceData[0].TotalTimeOut;
         }

         function calculateTotalStudents(grades) {
             let total = 0;
             grades.forEach(grade => {
                 total += grade.TotalStudents;
             });
             return total;
         }

         function calculateMaleStudents(grades) {
             let totalMale = 0;
             grades.forEach(grade => {
                 totalMale += grade.MaleStudents;
             });
             return totalMale;
         }

         function calculateFemaleStudents(grades) {
             let totalFemale = 0;
             grades.forEach(grade => {
                 totalFemale += grade.FemaleStudents;
             });
             return totalFemale;
         }

         function renderBarChart(gradeData) {
             const labels = gradeData.map(grade => grade.Grade);
             const totalStudents = gradeData.map(grade => grade.TotalStudents);

             const barCtx = document.getElementById('myChart').getContext('2d');

             new Chart(barCtx, {
                 type: 'bar',
                 data: {
                     labels: labels,
                     datasets: [{
                         label: 'Total Students',
                         data: totalStudents,
                         backgroundColor: 'rgba(76, 175, 80, 0.5)',
                         borderColor: 'rgba(76, 175, 80, 1)',
                         borderWidth: 1
                     }]
                 },
                 options: {
                     scales: {
                         y: {
                             beginAtZero: true
                         }
                     }
                 }
             });
         }

         function renderPieChart(notTimedInCount, timedInCount) {
             const pieCtx = document.getElementById('myPieChart').getContext('2d');

             new Chart(pieCtx, {
                 type: 'pie',
                 data: {
                     labels: ['Not Timed In', 'Timed In'],
                     datasets: [{
                         data: [notTimedInCount, timedInCount],
                         backgroundColor: ['#FF6384', '#36A2EB'],
                         hoverBackgroundColor: ['#FF6384', '#36A2EB']
                     }]
                 },
                 options: {
                     responsive: true,
                     plugins: {
                         legend: {
                             position: 'top',
                         }
                     }
                 }
             });
         }
     </script>
<style>
    body {
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 0;
        display: flex;
        flex-direction: column;
        min-height: 100vh;
        background-color: #f0f0f0;
    }
    .header1{
       
    }
    header, footer {
        background-color: #4C844B;
        color: white;
        text-align: center;
        padding: 20px 0;
    }

    footer {
        margin-top: auto;
    }

    .container {
        display: flex;
        justify-content: space-between;
        flex-wrap: nowrap;
        padding: 20px;
        box-sizing: border-box;
        flex-direction: row;
    }

    .content {
        display: flex;
        flex-direction: column;
    }

    .box, .box2, .box3, .colorbox2 {
        background-color: #fff;
        border: 1px solid #104210;
        border-radius: 8px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        margin: 15px;
        box-sizing: border-box;
    }

    .colorbox2 {
        background-color: #DAFFBD;
    }

    .box {
        padding: 30px;
        width: 380px;
        height: 340px;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        align-items: flex-start;
    }

    .box .timebox {
        display: flex;
        flex-direction: row;
    }

    #timeDisplay.lowered {
        margin-top: 3px;
        font-size: 30px;

    }
     #dateDisplay {
        font-size: 30px;
    }
    

    .box2, .colorbox2 {
        padding: 20px;
        width: 380px;
        height: 155px;
        display: flex;
        align-items: center;
        flex-direction: row;
    }

    .box3 {
        padding: 15px;
        width: 380px;
        height: 340px;
    }

    .chart-container {
        width: 80%;
        max-width: 600px;
        margin: auto;
        border-radius: 8px;
    }

    .chart-container h3 {
        text-align: center;
    }

    .innerbox {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: center;
        text-align: left;
        margin-left: 15px;
        margin-top: -24px;
    }

    .img_case {
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .img_case img {
        border-radius: 8px;
    }

    .innerbox h1 {
        font-size: 30px;
        margin: 0;
        color: #333;
    }

    .innerbox h3 {
        font-size: 24px;
        margin: 5px 0 0;
        color: #666;
    }

    .malebox, .femalebox {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 10px;
        border-radius: 8px;
        margin: 10px;
        background-color: #DAFFBD;
    }

    @media (max-width: 768px) {
        .box {
            width: calc(50% - 30px);
        }
    }

    @media (max-width: 480px) {
        .box {
            width: calc(100% - 30px);
        }
    }
</style>

<body>
    <header>
         <div class="header1">
        <h1>Dashboard</h1>
             </div>
    </header>

    <div class="container">
        <div class="content">
            <div class="box">
                <div class="timebox">
                    <img src="/images/icons/dashboard/day.png" alt="icon" width="100px" height="100px">
                    <div id="timeDisplay" class="lowered">Time</div>
                </div>
                <div class="datebox">
                    <h4>Today:</h4>
                    <div id="dateDisplay">Date</div>
                </div>
            </div>
        </div>
    
    <div class="content">
        <div class="colorbox2">
            <div class="img_case">
                <img src="/images/icons/student.png" alt="icon" width="80px" height="80px">
            </div>
            <div class="innerbox">
                <h1>Enrolled Students</h1>
                <h3 id="totalStudents">Loading...</h3>
            </div>
        </div>
        <div class="colorbox2">
            <div class="malebox">
                <h3>Male</h3>
                <h3 id="maleStudents">Loading...</h3>
            </div>
            <div class="femalebox">
                <h3>Female</h3>
                <h3 id="femaleStudents">Loading...</h3>
            </div>
        </div>
    </div>
        <div class="content">
            <div class="box3">
            <div class="chart-container">
                <canvas id="myChart" width="300" height="300"></canvas>
            </div>
        </div>
    </div>
    </div>

   <div class="container">
        <div class="content">
            <div class="box2">
                <div class="img_case">
                    <img src="/images/icons/dashboard/attendance.png" alt="icon" width="80px" height="80px">
                </div>
                <div class="innerbox">
                    <h1>Timed In</h1>
                    <h3 id="timedInCount">Loading...</h3>
                </div>
            </div>
            <div class="box2">
                <div class="img_case">
                    <img src="/images/icons/dashboard/warning.png" alt="icon" width="80px" height="80px">
                </div>
                <div class="innerbox">
                    <h1>Not Timed In</h1>
                    <h3 id="notTimedInCount">Loading...</h3>
                </div>
            </div>
        </div>

        <div class="content">
            <div class="box2">
                <div class="img_case">
                    <img src="/images/icons/dashboard/timeout.png" alt="icon" width="80px" height="80px">
                </div>
                <div class="innerbox">
                    <h1>Timed Out</h1>
                    <h3 id="timedOutCount">Loading...</h3>
                </div>
            </div>
            <div class="box2">
                <div class="img_case">
                    <img src="/images/icons/dashboard/warning.png" alt="icon" width="80px" height="80px">
                </div>
                <div class="innerbox">
                    <h1>Not Timed Out</h1>
                    <h3 id="notTimedOutCount">Loading...</h3>
                </div>
            </div>
        </div>

        <div class="content">
            <div class="box3">
                <div class="chart-container">
                    <h4>Total Student Present</h4>
                    <canvas id="myPieChart" width="250" height="250"></canvas>
                </div>
            </div>
        </div>
    </div>



   
</body>
</html>