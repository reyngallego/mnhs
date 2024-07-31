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
    <link href="styles/terms-and-conditions.css" rel="stylesheet" />
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
                            <h6 id="school-name">Moonwalk National High School</h6>
                        </div>
                        <!-- Sidebar content goes here -->
                        <ul class="nav">

                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link active" href="dashboard.aspx" data-page="dashboard.aspx">
                                    <img src="/images/icons/dashboard.png" alt="Dashboard" class="nav-icon"> <span class="nav-ul">Dashboard</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="attendance.aspx" data-page="attendance.aspx">
                                    <img src="/images/icons/attendance.png" alt="Attendance" class="nav-icon"> <span class="nav-ul">Attendance</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="studentlist.aspx" data-page="studentlist.aspx">
                                    <img src="/images/icons/student.png" alt="Student" class="nav-icon"> <span class="nav-ul">Student</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px; width: auto;">
                                <a class="nav-link" href="#" data-page="reports.aspx">
                                    <img src="/images/icons/reports.png" alt="Reports" class="nav-icon"> <span class="nav-ul">Reports</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="#" data-page="generateid.aspx">
                                    <img src="/images/icons/idgenerator.png" alt="ID Generator" class="nav-icon"> <span class="nav-ul">ID Generator</span>
                                </a>
                            </li>
                            <li class="nav-item" style="margin-bottom: 10px;">
                                <a class="nav-link" href="#" id="logoutLink">
                                    <img src="/images/icons/logout.png" alt="Log Out" class="nav-icon"> <span class="nav-ul">Log Out</span>
                                </a>
                            </li>
                        </ul>
                        <ul class="nav flex-column mt-auto" id="terms-and-conditions">
                            <li class="nav-item">
                                <a class="nav-link" href="#" data-toggle="modal" data-target="#termsModal">
                                    <span class="nav-ul">Terms and Conditions</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </nav>
                <!-- Main content area -->
                <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4 main-content">
                    <!-- Your dynamic content will be loaded here -->
                    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4 main-content">
                        <!-- Content will be loaded here -->
                    </main>
                </main>
            </div>
        </div>
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
        
        <!-- Terms and Conditions Modal -->
        <div class="modal fade" id="termsModal" tabindex="-1" role="dialog" aria-labelledby="termsModalLabel" aria-hidden="true">
            <div class="modal-dialog t-and-c" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="termsModalLabel">Terms and Conditions</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <!-- Content -->
                    <div class="modal-body terms-and-cond">
                        <p><strong>Last Updated: July 31, 2024</strong></p>
                        <p>Welcome to the Moonwalk National High School Student Attendance Monitoring System ("the System"). By accessing or using the System, you agree to be bound by these Terms and Conditions. If you do not agree to these terms, please do not use the System.</p>

                       <h3>Definitions</h3>
                        <ul>
                            <li><strong>System:</strong> Refers to the Moonwalk National High School Student Attendance Monitoring System.</li>
                            <li><strong>User:</strong> Refers to the school registrar using the System.</li>
                            <li><strong>School:</strong> Refers to Moonwalk National High School.</li>
                        </ul>

                        <h3>Acceptance of Terms</h3>
                        <p>By using the System, you agree to comply with and be bound by these Terms and Conditions. The School reserves the right to modify these terms at any time. Your continued use of the System following any changes indicates your acceptance of the new terms.</p>
 
                        <h3>License Grant</h3>
                        <p>The School grants the school registrar a non-exclusive, non-transferable, limited license to access and use the System for managing student attendance in accordance with these Terms and Conditions.</p>

                        <h3>User Obligations</h3>
                        <p>The school registrar must:</p>
                        <ul>
                            <li>Provide accurate and complete information when using the System.</li>
                            <li>Use the System in compliance with all applicable laws and regulations.</li>
                            <li>Keep login credentials secure and confidential.</li>
                            <li>Notify the School immediately of any unauthorized use of their account.</li>
                        </ul>

                        <h3>Prohibited Activities</h3>
                        <p>The school registrar may not:</p>
                        <ul>
                            <li>Use the System for any illegal or unauthorized purpose.</li>
                            <li>Interfere with or disrupt the System's functionality.</li>
                            <li>Attempt to gain unauthorized access to any part of the System.</li>
                            <li>Use the System to transmit any harmful or malicious content.</li>
                        </ul>

                        <h3>Intellectual Property</h3>
                        <p>The System and its original content, features, and functionality are owned by the School and are protected by international copyright, trademark, patent, trade secret, and other intellectual property or proprietary rights laws.</p>

                        <h3>Privacy Policy and Compliance with R.A. 10173 (Data Privacy Act of 2012)</h3>
                        <p>Your use of the System is also governed by our Privacy Policy, which outlines how we collect, use, and protect your personal information. By using the System, you consent to the practices described in the Privacy Policy.</p>
                        <p>The School is committed to strict compliance with Republic Act No. 10173, also known as the Data Privacy Act of 2012. The Act ensures the protection of personal data and maintains the rights of data subjects. The School will:</p>
                        <ul>
                            <li>Collect and process personal data only for legitimate purposes.</li>
                            <li>Implement reasonable and appropriate organizational, physical, and technical security measures to protect personal data.</li>
                            <li>Ensure the confidentiality, integrity, and availability of personal data.</li>
                            <li>Allow data subjects to exercise their rights under the Act, including the right to access, correct, and erase their personal data.</li>
                        </ul>

                        <h3>Limitation of Liability</h3>
                        <p>To the fullest extent permitted by law, the School shall not be liable for any indirect, incidental, special, consequential, or punitive damages, or any loss of profits or revenues, whether incurred directly or indirectly, or any loss of data, use, goodwill, or other intangible losses, resulting from:</p>
                        <ul>
                            <li>Your use or inability to use the System.</li>
                            <li>Any unauthorized access to or use of our servers and/or any personal information stored therein.</li>
                            <li>Any interruption or cessation of transmission to or from the System.</li>
                        </ul>

                        <h3>Disclaimer of Warranties</h3>
                        <p>The System is provided "as is" and "as available" without any warranties of any kind, either express or implied. The School does not guarantee that the System will be error-free or uninterrupted.</p>

                        <h3>Indemnification</h3>
                        <p>You agree to indemnify, defend, and hold harmless the School, its affiliates, officers, directors, employees, and agents from and against any and all claims, damages, obligations, losses, liabilities, costs, or debt, and expenses (including but not limited to attorney's fees) arising from:</p>
                        <ul>
                            <li>Your use of and access to the System.</li>
                            <li>Your violation of any term of these Terms and Conditions.</li>
                            <li>Your violation of any third-party right, including without limitation any copyright, property, or privacy right.</li>
                        </ul>

                        <h3>Termination</h3>
                        <p>The School may terminate or suspend your access to the System immediately, without prior notice or liability, for any reason whatsoever, including without limitation if you breach the Terms and Conditions. Upon termination, your right to use the System will cease immediately.</p>

                        <h3>Governing Law</h3>
                        <p>These Terms and Conditions shall be governed and construed in accordance with the laws of the Philippines, without regard to its conflict of law provisions.</p>

                        <h3>Contact Information</h3>
                        <p>If you have any questions about these Terms and Conditions, please contact us at:</p>
                        <p>
                            Moonwalk National High School<br>
                            School ID: 320203<br>
                            Daang Batang St., SAV, Moonwalk, Parañaque City<br>
                            <a href="https://moonwalknhs.edu.ph/">http://www.moonwalknhs.edu.ph</a><br>
                            (+632) 821 6702
                        </p>

                        <p>By using the System, you acknowledge that you have read, understood, and agree to be bound by these Terms and Conditions.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden button to trigger server-side logout -->
        <asp:Button ID="btnLogout" runat="server" OnClick="Logout_Click" Style="display:none" />
    </form>

    <script>
        $(document).ready(function () {

            var isContentLoading = false;

            // Load the dashboard content on page load
            loadContent("dashboard.aspx");

            // Function to load page content into main content area
            function loadContent(page, event) {
                if (event) {
                    event.preventDefault();
                }
                if (isContentLoading) return;

                isContentLoading = true;
                $.get(page, function (data) {
                    $(".main-content").html(data);
                    isContentLoading = false;
                    attachEventHandlers();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.error("Error loading content:", textStatus, errorThrown);
                    isContentLoading = false;  // Ensure flag is reset even on error
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
