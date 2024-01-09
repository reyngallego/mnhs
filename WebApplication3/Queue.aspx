<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Queue.aspx.cs" Inherits="WebApplication3.Queue" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Queue Management System</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="styles/queue.css" />
    <style>
        /* Add your custom styles here if needed */
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row">
                <div class="col-md-6 offset-md-3">
                    <div class="card mb-3">
                        <div class="usercontainer">
                            <h5 class="card-title">User Information</h5>
                            <p class="card-text">Username: <%= Username %></p>
                            <p class="card-text">Department: <%= Department %></p>
                        </div>
                    </div>

                    <table class="tablecontainer">
                        <thead>
                            <tr>
                                <th>Queue Number</th>
                                <th>Customer Name</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Table rows for queued customers -->
                            <tr>
                                <td>1</td>
                                <td>John Doe</td>
                            </tr>
                            <tr>
                                <td>2</td>
                                <td>Jane Smith</td>
                            </tr>
                            <!-- Add more rows as needed -->

                            <!-- You can dynamically generate these rows using JavaScript if needed -->
                        </tbody>
                    </table>

                    <!-- Control buttons -->
                    <div class="ButtonContainer">
                        <button class="btn btn-primary mx-2">Previous</button>
                        <button class="btn btn-success mx-2">Call</button>
                        <button class="btn btn-warning mx-2">Recall</button>
                        <button class="btn btn-danger mx-2">Done</button>
                        <button class="btn btn-primary mx-2">Next</button>
                    </div>

                    <div class="card mt-3">
                        <div class="QueueContainer">
                            <h5 class="card-title">Queue Information</h5>
                            <p class="card-text">Current Serving: <%= CurrentServing %></p>
                            <p class="card-text">Total Served Today: <%= TotalServedToday %></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
