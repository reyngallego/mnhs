<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="WebApplication3.mnhs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reports</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <script src="../js/reports.js"></script>
    <link href="styles/reports.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
</head>
<body>
    <header>
        <div class="header1">
            <div class="header-overlay"></div>
            <h1>Reports</h1>
        </div>
    </header>
    <div class="container">
        <div class="form-group">
            <div class="report-header">

                <!-- This header only visible to the printing of student records -->
                <div class="school-header">
                    <div id="school-header-logo">
                        <img src="/images/logomnhs.png"/>
                    </div>
                    <div id="school-header">
                        <p><b>Republic of the Philippines</b></p>
                        <p>National Capital Region</p>
                        <p>Schools Division of Parañaque City</p>
                        <h3 id="school-name"><b>MOONWALK NATIONAL HIGH SCHOOL</b></h3>
                    </div>
                    <div id="school-header-1">
                        <div id="deped-pque-logo">
                            <img src="/images/deped-pque.png"/>
                        </div>
                        <div id="bagong-pilipinas-logo">
                            <img src="/images/bagong-pilipinas.png"/>
                        </div>
                    </div>
                    
                </div>

                <!-- Student Attendance Records -->
                <div class="header2">
                    <h1>STUDENT ATTENDANCE RECORDS</h1>
                </div>
                <div class="actions">
                    <div class="search-action">
                        <form action="javascript:searchStudent();">
                            <input type="text" placeholder="Enter student LRN" name="search" id="report-search"/>
                            <button type="submit" class="search-button">
                                <div class="search-icon"></div>
                            </button>
                        </form>
                    </div>
                    <div id="actions">
                        <button>
                            <img src="/images/icons/download.png" alt="Download" style="width: 20px; height: 25px; vertical-align: middle;margin:0px 5px"/>
                            <span style="font-size: 20px; color: black;"></span>
                        </button>
                        <button class="action-button" id="print-button" type="button">
                            <img src="/images/icons/printer.png" alt="Printer" />
                            <span style="font-size: 20px; color: black;"></span>
                        </button>
                    </div>
                </div>
                <div class="stud-info">
                    <h4 id="studentName">Student Name: </h4>
                    <h4 id="learnerReferenceNumber">Learner Reference Number: </h4>
                    <h4 id="gradeAndSection">Grade and Section:</h4>
                </div>
            </div>
            <div id="report-details">
                <div class="box1">
                    <!-- Detailed Data -->
                    <h4 id="detailed-data">Detailed Data</h4>
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
                    <div id="pagination" class="mt-3">
                        <!-- Pagination controls will be populated here -->
                    </div>
                    <div id="entry-count" class="mt-2">
                        <!-- Entry count will be populated here -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
