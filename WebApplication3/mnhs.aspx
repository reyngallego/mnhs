<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mnhs.aspx.cs" Inherits="WebApplication3.mnhs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <link href="styles/custom.css" rel="stylesheet" />
    <link href="styles/dashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <!-- Sidebar -->
                <nav class="col-md-2 d-none d-md-block bg-light sidebar">
                    <div class="sidebar-sticky">
                        <!-- Logo -->
                        <div class="logo-title">
                            <img src="/images/logomnhs.png" alt="Logo" class="img-fluid mb-4 logo">
                            <h6>Moonwalk National Highschool</h6>
                        </div>
                        <!-- Sidebar content goes here -->
                        <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                            Main
                        </h6>
                        <ul class="nav">
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link active" href="dashboard.aspx" data-page="dashboard.aspx">
                                    <img src="/images/icons/dashboard.png" alt="Dashboard" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">Dashboard</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="attendance.aspx" data-page="attendance.aspx">
                                    <img src="/images/icons/attendance.png" alt="Attendance" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">Attendance</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="studentlist.aspx" data-page="studentlist.aspx">
                                    <img src="/images/icons/student.png" alt="Student" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">Student</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="reports.aspx" data-page="reports.aspx">
                                    <img src="/images/icons/reports.png" alt="Reports" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">Reports</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="generateid.aspx" data-page="generateid.aspx">
                                    <img src="/images/icons/idgenerator.png" alt="ID Generator" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">ID Generator</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="#" id="logoutLink">
                                    <img src="/images/icons/logout.png" alt="Log Out" class="nav-icon"> 
                                    <span style="font-size: 20px; color: black;">Log Out</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </nav>

                <!-- MAIN CONTENT -->
                <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4 main-content">
                    <!-- Main content goes here -->
                </main>
            </div>
        </div>
        
        <!-- Hidden button to trigger server-side logout -->
        <asp:Button ID="btnLogout" runat="server" OnClick="Logout_Click" Style="display:none" />
    </form>

    <!-- Logout Modal -->
    <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="logoutModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="logoutModalLabel">Logout Confirmation</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you want to log out?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="confirmLogout">Yes, Log Out</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            var isContentLoading = false;

            // Load the dashboard content on page load
            $(".main-content").load("dashboard.aspx", function () {
                isContentLoading = false;
                attachEventHandlers();
            });

            // Function to load page content into main content area
            function loadContent(page, event) {
                event.preventDefault();
                if (isContentLoading) return;

                isContentLoading = true;
                $.get(page, function (data) {
                    $(".main-content").html(data);
                    isContentLoading = false;
                    attachEventHandlers();
                });
            }

            // Function to attach event handlers to sidebar links
            function attachEventHandlers() {
                $(".nav-link").click(function (e) {
                    var page = $(this).data("page");

                    loadContent(page, e);
                });
            }

            // Initial attachment of event handlers
            attachEventHandlers();

            // Show logout modal on clicking logout link
            $("#logoutLink").click(function (e) {
                e.preventDefault();
                $("#logoutModal").modal("show");
            });

            // Handle logout confirmation
            $("#confirmLogout").click(function () {
                $("#<%= btnLogout.ClientID %>").click();
            });
        });
    </script>
</body>
</html>
