<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication3.home" EnableEventValidation="false" EnableViewState="true" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src=" https://code.jquery.com/jquery-3.7.1.min.js"></script>
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

        function updateTimer(hours, minutes, seconds) {
        var lblservingTimeLabel = document.getElementById('<%= lblservingTimeLabel.ClientID %>');
        lblservingTimeLabel.innerHTML = hours + ":" + minutes + ":" + seconds;
        }
           function updateTimerOnLoad() {
               $.ajax({
                   type: "POST",
                   url: "home.aspx/GetElapsedTime",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (data) {
                       // Update the timer with the received elapsed time
                       updateTimer(data.d);
                   },
                   error: function (error) {
                       console.error("Error calling GetElapsedTime: " + error.statusText);
                   }
               });
           }
           updateTimerOnLoad();

        // Function to update date and time
       function updateDateTime() {
    // Update Date
    var currentDate = new Date();
    var options = { year: 'numeric', month: 'long', day: 'numeric' };
    var formattedDate = currentDate.toLocaleDateString(undefined, options);
    $("#<%= lblDate.ClientID %>").text("Date: " + formattedDate);

    // Update Time
    var hours = currentDate.getHours();
    var ampm = hours >= 12 ? 'PM' : 'AM';

    // Convert hours to 12-hour format
    hours = hours % 12;
    hours = hours ? hours : 12; // If hours is 0, set it to 12

    var minutes = currentDate.getMinutes();
    var seconds = currentDate.getSeconds();

    // Format hours, minutes, and seconds to have leading zeros
    hours = (hours < 10) ? "0" + hours : hours;
    minutes = (minutes < 10) ? "0" + minutes : minutes;
    seconds = (seconds < 10) ? "0" + seconds : seconds;

    var formattedTime = "Time: " + hours + ":" + minutes + ":" + seconds + " " + ampm;
    $("#<%= lblTime.ClientID %>").text(formattedTime);

       }
        // Function to fetch and update Tickets Done
        function fetchAndDisplayTicketsDone() {
            $.ajax({
                url: "http://localhost:65388/api/Ticket/GetTicketsDoneForToday?department=" + loggedInDepartment,
                type: "GET",
                success: function (data) {
                    // Update the content of the specified HTML element with the retrieved value
                    $("#ticketsDoneValue").text(data);
                },
                error: function (error) {
                    console.error("Error calling API: " + error.statusText);
                }
            });
        }

        // Initial fetch and display
        fetchAndDisplayTicketsDone();

        // Set interval to update tickets done every 5 seconds (adjust as needed)
        setInterval(fetchAndDisplayTicketsDone, 5000);

        // Initial update
        updateDateTime();

        // Set interval to update time every second
        setInterval(updateDateTime, 1000);

        $("#btnNextTicket").click(function () {
            // Implement the logic for the "Next" button click
            console.log("next button clicked");
        });

        $("#btnPreviousTicket").click(function () {
            // Implement the logic for the "Previous" button click
            console.log("Previous button clicked");
        });

        $("#btnCallTicket").click(function () {
            // Show the "Call" modal
            $('#callModal').modal('show');
        });

        $("#btnRecallTicket").click(function () {
            // Implement the logic for the "Recall" button click
            $('#recallModal').modal('show');
        });

        $("#btnDoneTicket").click(function () {
            // Show the "Done" modal
            $('#doneModal').modal('show');
        });

           // Function to handle "Confirm Call" button click inside the "Call" modal
        $("#confirmCall").click(function () {
            // Add your logic here for confirming the call
            // For example, you may want to make an API call or perform some other action
            console.log("Call confirmed");
            // Close the modal after confirmation
            $('#callModal').modal('hide');
        });

           // Function to handle "Confirm Done" button click inside the "Done" modal
        $("#confirmDone").click(function () {
            // Add your logic here for confirming that the task is done
            console.log("Done confirmed");
            // Close the modal after confirmation
            $('#doneModal').modal('hide');
        });

        $("#cancelCall").click(function () {
            // Add your logic here for canceling the call
            console.log("Call canceled");
            // Close the modal after cancelation
            $('#callModal').modal('hide');
        });

           // Function to handle "Cancel" button click inside the "Done" modal
        $("#cancelDone").click(function () {
            // Add your logic here for canceling the task as done
            console.log("Done canceled");
            // Close the modal after cancelation
            $('#doneModal').modal('hide');
        });

       

           // Function to handle "Confirm Recall" button click inside the "Recall" modal
        $("#confirmRecall").click(function () {
            // Add your logic here for confirming the recall
            // For example, you may want to make an API call or perform some other action
            console.log("Recall confirmed");
            // Close the modal after confirmation
            $('#recallModal').modal('hide');

        });
           // Function to handle "Cancel Recall" button click inside the "Recall" modal
        $("#cancelRecall").click(function () {
            // Add your logic here for canceling the recall
            console.log("Recall canceled");
            // Close the modal after cancelation
            $('#recallModal').modal('hide');
        });
        });
    
    </script>

</head>
<body>

    <form runat="server">
        <input type="hidden" id="hdnLoggedInDepartment" runat="server" />
 <div class="ticket">
            <div class="row">
                <div class="col-md-4">
<asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
      <asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
                    <h2 id="servingTimeLabel" class="left-section">SERVING TIME</h2>
                    <div
                <asp:Label ID="lblservingTimeLabel" runat="server" Text="" Visible="true"></asp:Label>

               <asp:Timer ID="Timer1" runat="server"  OnTick="Timer1_Tick" Enabled="false"></asp:Timer>
                         </ContentTemplate>
       
</asp:UpdatePanel>
                        </div>
                </div>
                <div class="col-md-4">
                    <h2 id="currentServingLabel" class="center-section current-serving-label">CURRENT SERVING</h2>
                    <asp:Label ID="lblQueueTicket" runat="server" CssClass="queueticket-label" />
                    <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Text=""></asp:Label>

                </div>
                <div class="col-md-4">
                    <h2 id="ticketsDoneLabel" class="right-section">TICKETS DONE</h2>
                    <div id="ticketsDoneValue" class="tickets-done-label"></div>
                  

                </div>
               
            </div>
     </div>

        <div class="button-container">
           <asp:Button ID="btnPreviousTicket" runat="server" Text="Previous" OnClick="btnPreviousTicket_Click" CssClass="btn btn-previous"/>
          <asp:Button ID="btnNextTicket" runat="server" Text="Next" OnClick="btnNextTicket_Click" CssClass="btn btn-next" />
         <button type="button" id="btnCallTicket" class="btn btn-call" onclick="showCallModal()">Call</button>
           <button type="button" id="btnRecallTicket" class="btn btn-recall" onclick="recallTicket()">Recall</button>
          <button type="button" id="btnDoneTicket" class="btn btn-success">Done</button>

        </div>
        <!-- Call Modal -->
<div class="modal fade" id="callModal" tabindex="-1" role="dialog" aria-labelledby="callModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="callModalLabel">Call Student</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Are you sure you want to call this student?</p>
            </div>
            <div class="modal-footer">
        <button type="button" class="btn btn-secondary" id="cancelCall" data-dismiss="modal">Cancel</button>
    <asp:Button runat="server" ID="btnConfirmCall" CssClass="btn btn-primary" Text="Confirm" OnClick="btnConfirmCall_Click" />
            </div>
        </div>
    </div>
</div>

<!-- Recall Modal -->
<div class="modal fade" id="recallModal" tabindex="-1" role="dialog" aria-labelledby="recallModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="recallModalLabel">Recall Student</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Do you want to recall this student?</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" id="cancelRecall" data-dismiss="modal">Cancel</button>
                <asp:Button runat="server" ID="btnConfirmRecall" CssClass="btn btn-primary" Text="Confirm" OnClick="btnConfirmRecall_Click" />
            </div>
        </div>
    </div>
</div>

<!-- Done Modal -->
<div class="modal fade" id="doneModal" tabindex="-1" role="dialog" aria-labelledby="doneModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="doneModalLabel">Done with Student</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Are you sure you are done with this student?</p>
            </div>
            <div class="modal-footer">
        <button type="button" class="btn btn-secondary" id="cancelDone" data-dismiss="modal">Cancel</button>
    <asp:Button runat="server" ID="btnConfirmDone" CssClass="btn btn-primary" Text="Confirm" OnClientClick="confirmDoneClicked()" OnClick="btnConfirmDone_Click" />
            </div>
        </div>
    </div>
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
                       <button type="button" class="btn btn-secondary" id="cancelLogout">Cancel</button>

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
