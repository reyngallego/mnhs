<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="WebApplication3.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="styles/dashboard.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
    <link href="styles/custom.css" rel="stylesheet" /> <!-- Link to custom CSS file -->

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</head>
<body>
   
  <div class="header">
          <h1 class="header-text"> DASHBOARD MAIN</h1>                                          
                      </div>
<div class="row">
  <div class="col-lg-4 custom-card">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">9:00AM</h5>
        <p class="card-text">RealTime Insight</p>
      </div>
    </div>
  </div>
  <div class="col-lg-4">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Top Center Card</h5>
        <p class="card-text">This is the top center card content.</p>
      </div>
    </div>
    <div class="card mt-3">
      <div class="card-body">
        <h5 class="card-title">Bottom Center Card</h5>
        <p class="card-text">This is the bottom center card content.</p>
      </div>
    </div>
  </div>
  <div class="col-lg-4">
    <div class="bargraph">
      <h5>Bar Graph</h5>
      <!-- Placeholder for bar graph -->
    </div>
  </div>
</div>

    <script>

    </script>
</body>
</html>
