<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Dashboard</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <link href="styles/dashboard.css" rel="stylesheet" /> <!-- Link to custom CSS file -->

</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <!-- Sidebar -->
                <nav class="col-md-2 d-none d-md-block bg-light sidebar">
                    <div class="sidebar-sticky">
                        <!-- Logo -->
                        <div>
<div class="logo-title">
            <img src="/images/logomnhs.png" alt="Logo" class="img-fluid mb-4 logo">
            <h6>Moonwalk National Highschool</h6>
        </div>
                            </div>
                        <!-- Sidebar content goes here -->
                        <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                            Main
                        </h6>
                <ul class="nav">
    <li class="nav-item" style="margin-bottom: 10px;">
        <a class="nav-link active" href="dashboard.aspx" data-page="dashboard.aspx">
            <img src="/images/icons/dashboard.png" alt="Dashboard" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">Dashboard</span>
        </a>
    </li>
    <li class="nav-item" style="margin-bottom: 10px;">
        <a class="nav-link" href="attendance.aspx" data-page="attendance.aspx">
            <img src="/images/icons/attendance.png" alt="Attendance" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">Attendance</span>
        </a>
    </li>
    <li class="nav-item" style="margin-bottom: 10px;">
        <a class="nav-link" href="studentlist.aspx" data-page="studentlist.aspx">
            <img src="/images/icons/student.png" alt="Student" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">Student</span>
        </a>
    </li>
    <li class="nav-item" style="margin-bottom: 10px; width: auto;"> <!-- Adjust the width as needed -->
    <a class="nav-link" href="#" data-page="">
        <img src="/images/icons/reports.png" alt="Reports" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">Reports</span>
    </a>
</li>
    <li class="nav-item" style="margin-bottom: 10px;">
        <a class="nav-link" href="generateid.aspx" data-page="generateid.aspx">
            <img src="/images/icons/idgenerator.png" alt="ID Generator" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">ID Generator</span>
        </a>
    </li>
    <li class="nav-item" style="margin-bottom: 10px;">
        <a class="nav-link" href="#" data-page="">
            <img src="/images/icons/logout.png" alt="Log Out" style="width: 20px; height: 20px; vertical-align: middle;"> <span style="font-size: 20px; color: black;">Log Out</span>
        </a>
    </li>
</ul>




                    </div>
                </nav>

                <!-- MAIN CONTENT -->
               <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4 main-content">
               
</main>
            </div>
        </div>
    </form>

  <script>
      $(document).ready(function () {
          var isContentLoading = false; // Flag to track whether content is being loaded

          // Function to load page content into main content area
          function loadContent(page, event) {
              // Prevent default link behavior
              event.preventDefault();

              // If content is already being loaded, ignore this click event
              if (isContentLoading) {
                  return;
              }

              isContentLoading = true; // Set flag to true since content loading is starting

              // Load content into main content area
              $.get(page, function (data) {
                  $(".main-content").html(data);
                  isContentLoading = false; // Reset flag after content is loaded
                  // After loading content, re-attach event handlers to newly loaded elements
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
      });

  </script>
</body>
</html>
