<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebApplication3.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MwNHS Student Attendance Monitor</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="styles/custom.css" rel="stylesheet" />
    <link href="styles/dashboard.css" rel="stylesheet" />
    <link href="styles/index.css" rel="stylesheet" />
    <link href="styles/stud-attendance-summary.css" rel="stylesheet" /> <!-- Added the stylesheet for modal styling -->
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid no-scroll">
            <div class="row no-scroll">
                <!-- LEFT PANE -->
                <div class="col-md-6 left-column">
                    <div class="left-pane">
                        <div class="left-pane-overlay"></div>
                        <div class="left-pane-content">
                            <img src="images/logomnhs.png" alt="Logo" class="landing-logo">
                            <label class="title">Student Attendance Monitoring System</label>
                            <div class="search-container">
                                <input type="text" class="search-bar" placeholder="Enter student's surname">
                                <button type="button" class="search-button">
                                    <div class="search-icon"></div>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="footer-landing">
                        <div class="footer-images">
                            <img src="images/moonwalkLogo.png" alt="Moonwalk Logo" class="footer-logo">
                            <img src="images/malasaquitLogo.png" alt="Malasaquit Logo" class="footer-logo">
                            <img src="images/logopup.png" alt="PUP Logo" class="footer-logo">
                        </div>
                        <h1 class="footer-caption">In partnership with Polytechnic University of the Philippines - Parañaque City Campus</h1>
                    </div>
                </div>
                
                <!-- RIGHT PANE -->
                <div class="col-md-6 main-content">
                    <div class="content-wrapper">
                        <!-- Main content goes here -->
                    </div>
                </div>
            </div>
        </div>

        <!-- The Modal -->
        <div id="attendance-summ-modal" class="summ-modal">
            <div class="modal-content">
                <span class="close">&times;</span>
                <div class="as-modal-content">
                    <p id="as-date"></p>
                    <p id="as-status"></p>
                    <p id="as-timein"></p>
                    <p id="as-timeout"></p>
                </div>
            </div>
        </div>
    </form>

    <script>
        $(document).ready(function () {
            var isContentLoading = false;

            // Load the student search list content on page load
            $(".content-wrapper").load("stud-search-list.aspx", function () {
                //$(".content-wrapper").load("parent-portal-placeholder.aspx", function () {
                isContentLoading = false;
                attachEventHandlers();
            });

            // Attach event handlers
            function attachEventHandlers() {
                $('.close-button').on('click', function (event) {
                    loadContent('stud-search-list.aspx', event);
                });

                $('#search-table tbody tr').on('click', function (event) {
                    var studentId = $(this).data('id');
                    if (studentId) {
                        loadContent('stud-attendance-summary.aspx?studentId=' + studentId, event);
                    }
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

                // Get the <span> element that closes the modal
                var span = document.getElementsByClassName("close")[0];

                // When the user clicks on <span> (x), close the modal
                span.onclick = function () {
                    $('#attendance-summ-modal').css("display", "none");
                }

                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == document.getElementById("attendance-summ-modal")) {
                        $('#attendance-summ-modal').css("display", "none");
                    }
                }
            }

            // Function to load page content into main content area
            function loadContent(page, event) {
                event.preventDefault();
                if (isContentLoading) return;

                isContentLoading = true;
                $.get(page, function (data) {
                    $(".content-wrapper").html(data);
                    isContentLoading = false;
                    attachEventHandlers();
                });
            }
        });
    </script>
</body>
</html>
