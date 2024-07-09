<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stud-search-list.aspx.cs" Inherits="WebApplication3.stud_search_list" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Attendance Search List</title>
    <link href="styles/stud-attendance-summary.css" rel="stylesheet" />
    <link href="styles/stud-search-list.css" rel="stylesheet" />
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
                    <h1 id="heading-title">Search results</h1>
                    <table id="stud-search-res">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Grade and Section</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr data-id="352990120291">
                                <td>Harris, Mohammad Jaoshannee O.</td>
                                <td>11 - Gold</td>
                            </tr>
                            <!-- Additional rows can be added here -->
                        </tbody>
                    </table>
                </div>
                <div id="footer-button">
                </div>
            </div>
        </div>
    </form>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#stud-search-res tbody tr').on('click', function () {
                var studentId = $(this).data('id');
                if (studentId) {
                    // Load the attendance summary for the clicked student
                    $('.content-wrapper').load('stud-attendance-summary.aspx?studentId=' + studentId);
                }
            });

            $('.close-button').on('click', function () {
                // Load the stud-search-list when close button is clicked
                $('.content-wrapper').load('stud-search-list.aspx');
            });
        });
    </script>
</body>
</html>