<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="monitor.aspx.cs" Inherits="WebApplication3.monitor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.12/signalr.min.js"></script>

  <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <title>Queue Management System Monitor</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
      <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.min.js"></script>
   <script>
    var connection = new signalR.HubConnectionBuilder().withUrl('/QueueTicketHub').build();

    connection.on('UpdateQueueTicket', function (department, queueTicket) {
        // Update the queue ticket number on the page
        $('#' + department + 'QueueTicket').text(queueTicket);

        // Speak the new queue ticket number
        speakQueueTicketNumber(department, queueTicket);
    });

    connection.start().then(function () {
        console.log('SignalR connected');
    }).catch(function (err) {
        return console.error(err.toString());
    });

    function updateQueueTicket(department) {
        $.ajax({
            url: 'http://localhost:65388/api/QueueTicket/GetQueueTicket?department=' + department,
            method: 'GET',
            success: function (data) {
                // Invoke SignalR hub to notify connected clients about the update
                connection.invoke('UpdateQueueTicket', department, data.CurrentQueueTicket);
            },
            error: function (error) {
                console.error('Error fetching ' + department + ' queue ticket:', error);
            }
        });
    }

    function speakQueueTicketNumber(department, queueTicket) {
        // Use Web Speech API to convert text to speech
        var msg = new SpeechSynthesisUtterance('New queue ticket for ' + department + '. Ticket number ' + queueTicket);
        window.speechSynthesis.speak(msg);
    }

    $(document).ready(function () {
        // Update queue ticket for each department
        updateQueueTicket('cashier');
        updateQueueTicket('registrar');
        updateQueueTicket('studentAffairs');
        updateQueueTicket('director');
    });
</script>
       
</head>
<body>
     
    <form id="form1" runat="server">
        <div class="container-fluid">
            <!-- Video Player Section -->
           <div class="row mt-4">
                <div class="col-md-8 offset-md-2">
                    <!-- Your video player code goes here -->
                    <video id="queueVideo" class="w-100" controls>
                        <source src="your-video-source.mp4" type="video/mp4">
                        Your browser does not support the video tag.
                    </video>
                </div>
            </div>

            <!-- Department Queues Section -->
            <div class="row mt-4">
                <div class="col-md-3">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            Cashier Queue
                        </div>
                        <div class="card-body">
                            <!-- Display Cashier Queue Ticket Here -->
                            <h1 id="cashierQueueTicket" runat="server">-</h1>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card">
                        <div class="card-header bg-success text-white">
                            Registrar Queue
                        </div>
                        <div class="card-body">
                            <!-- Display Registrar Queue Ticket Here -->
                            <h1 id="registrarQueueTicket" runat="server">-</h1>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card">
                        <div class="card-header bg-info text-white">
                            Student Affairs Queue
                        </div>
                        <div class="card-body">
                            <!-- Display Student Affairs Queue Ticket Here -->
                            <h1 id="studentaffairsandservicesQueueTicket" runat="server">-</h1>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card">
                        <div class="card-header bg-warning text-white">
                            Director Queue
                        </div>
                        <div class="card-body">
                            <!-- Display Director Queue Ticket Here -->
                            <h1 id="directorQueueTicket" runat="server">-</h1>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>