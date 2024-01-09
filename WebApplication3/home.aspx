<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication3.home" EnableEventValidation="false" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="scripts/jquery.signalR-2.4.3.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="scripts/bootstrap.bundle.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <title>home</title>
    <link rel="stylesheet" type="text/css" href="styles/home.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script type="text/javascript" src="/js/queue.js"></script>
    <script type="text/javascript">
        var loggedInDepartment = '<%= Session["LoggedInDepartment"] %>';

        $(document).ready(function () {
     document.getElementById('<%= hdnLoggedInDepartment.ClientID %>').value = loggedInDepartment;

            $("#btnNextTicket").click(function () {
                // Implement the logic for the "Next" button click
                console.log("next button clicked");
            });

            $("#btnPreviousTicket").click(function () {
                // Implement the logic for the "Previous" button click
                console.log("Previous button clicked");
            });

            $("#btnCallTicket").click(function () {
                // Implement the logic for the "Call" button click
                console.log("Call button clicked");
            });

            $("#btnRecallTicket").click(function () {
                // Implement the logic for the "Recall" button click
                console.log("Recall button clicked");
            });

            $("#btnDoneTicket").click(function () {
                // Implement the logic for the "Done" button click
                console.log("done button clicked");
            });

            
        });


    </script>

</head>
<body>
    <form runat="server">
        <input type="hidden" id="hdnLoggedInDepartment" runat="server" />

        <div class="ticket">
            <h2 id="currentServingLabel">CURRENT SERVING </h2>
            <asp:Label ID="lblQueueTicket" runat="server" CssClass="queueticket-label" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Text=""></asp:Label>
        </div>
        <div class="button-container">
           <asp:Button ID="btnPreviousTicket" runat="server" Text="Previous" OnClick="btnPreviousTicket_Click" CssClass="btn btn-primary" />
          <asp:Button ID="btnNextTicket" runat="server" Text="Next" OnClick="btnNextTicket_Click" CssClass="btn btn-primary" />
          <asp:Button ID="btnCallTicket" runat="server" Text="Call" OnClick="btnCallTicket_Click" CssClass="btn btn-info" />
          <asp:Button ID="btnRecallTicket" runat="server" Text="Recall" OnClick="btnRecallTicket_Click" CssClass="btn btn-warning" />
           <asp:Button ID="btnDoneTicket" runat="server" Text="Done" OnClick="btnDoneTicket_Click" CssClass="btn btn-success" />
        </div>
        <div class="parent-container" />
        <div class="user-data-container">
            <div class="label-container">
                <div class="label">
                    <i class="fa fa-calendar"></i>
                    <asp:Label ID="lblDate" runat="server" CssClass="date" Text=""></asp:Label>
                </div>
                <div class="label">
                    <i class="fa fa-clock"></i>
                    <asp:Label ID="lblTime" runat="server" CssClass="time" Text="Time: 00:00:00"></asp:Label>
                </div>
                <asp:Image ID="imgUser" runat="server" CssClass="user-image" AlternateText="User Image" />
                <div class="user-info-container">
                    <div class="label">
                        <i class="fa fa-user"></i>
                        <asp:Label ID="lblFirstName" runat="server" CssClass="user-info" Text=""></asp:Label>
                    </div>
                </div>
                <div class="label">
                    <i class="fa fa-briefcase"></i>
                    <asp:Label ID="lblDepartment" runat="server" CssClass="user-info" Text=""></asp:Label>
                </div>
                <div>
                    <button id="logoutButton" class="button button-logout">
                        <i class="fas fa-sign-out-alt"></i> Logout
                    </button>
                </div>
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
                        <p>Are you sure you want to log out of your session?</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-danger" id="confirmLogout">Logout</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="studentDataContainer">
           
        </div>
        <div class="clearFloat"></div>
    </form>
</body>
</html>
