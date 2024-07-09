<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Attendance Result</title>
    <link href="styles/stud-attendance-summary.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="banner">
            <div class="overlay"> 
                <img src="images/logomnhs.png" alt="Logo" class="banner-logo"/>
            </div>
        </div>
        <div class="content">
            <div class="container">
                <div id="attendance">
                    <h1 id="heading-title">Individual Attendance Record</h1>
                    
                    <button type="button" class="close-button" id="close-button">Close</button>
                    <div id="stud-info">
                        <p><strong>LRN: </strong>352990120291</p>
                        <p><strong>Name: </strong>Harris, Mohammad Jaoshannee Ortega</p>
                        <p><strong>Grade and Section: </strong> 11 - Gold</p>
                    </div>
                    <table id="attendance-table">
                        <thead>
                            <tr>
                                <th>Date</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>2024-06-01</td>
                                <td>Present</td>
                            </tr>
                            <tr>
                                <td>2024-06-02</td>
                                <td>Absent</td>
                            </tr>
                            <!-- Additional rows can be added here -->
                        </tbody>
                    </table>
                </div>
                <div id="footer-button"></div>
            </div>
        </div>
    </form>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#close-button').on('click', function () {
                $('.content-wrapper').load('stud-search-list.aspx');
            });

            // When a table row is clicked in the attendance table
            $('#attendance-table tbody tr').on('click', function () {
                var date = $(this).find('td').eq(0).text();
                var status = $(this).find('td').eq(1).text();
                var timeIn = $(this).find('td').eq(2).text();
                var timeOut = $(this).find('td').eq(3).text();
                $('#as-date').text("Date: " + date);
                $('#as-status').text("Status: " + status);
                $('#as-timein').text("Time In: " + timeIn);
                $('#as-timeout').text("Time Out: " + timeOut);
                $('#attendance-summ-modal').css("display", "block");
            });
        });
    </script>
</body>
</html>
