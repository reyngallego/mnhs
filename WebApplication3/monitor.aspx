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
         <link rel="stylesheet" type="text/css" href="styles/monitor.css" />

   <script>
       var currentVideoId = null;

       function updateVideoSource() {
           // Fetch all selected videos from the server
           $.ajax({
               url: 'http://localhost:65388/api/video/getselectedvideos',
               method: 'GET',
               success: function (data) {
                   if (data.length > 0) {
                       // Create an array to store video URLs
                       var videoUrls = [];

                       // Iterate through the selected videos and get video URLs
                       for (var i = 0; i < data.length; i++) {
                           var videoId = data[i].VideoId;
                           var videoUrl = 'http://localhost:65388/api/video/playvideo/' + videoId;
                           videoUrls.push(videoUrl);
                       

                           // Set the video source using the PlayVideo endpoint
                           $('#PlayVideo').attr('src', 'http://localhost:65388/api/video/playvideo/' + videoId);

                           // Mute the video
                           $('#PlayVideo').prop('muted', true);

                           $('#PlayVideo').prop('loop', true);

                           // Listen for the "ended" event on the video element
                           $('#PlayVideo').on('ended', function () {
                               // After the current video ends, fetch the next video and play it
                               updateVideoSource();
                           });
                       }
                   }
               },
               error: function (error) {
                   console.error('Error fetching selected videos:', error);
               }
           });
       }

       function updateRegistrarQueueTicket() {
           $.ajax({
               url: 'http://localhost:65388/api/QueueTicket/GetQueueTicket?department=registrar',
               method: 'GET',
               success: function (data) {
                   // Update the registrar queue ticket number on the page
                   $('#registrarQueueTicket').text(data.CurrentQueueTicket);
               },
               error: function (error) {
                   console.error('Error fetching registrar queue ticket:', error);
               }
           });
       }
   function updateCashierQueueTicket() {
       $.ajax({
           url: 'http://localhost:65388/api/QueueTicket/GetQueueTicket?department=cashier',
           method: 'GET',
           success: function (data) {
               // Update the registrar queue ticket number on the page
               $('#cashierQueueTicket').text(data.CurrentQueueTicket);
           },
           error: function (error) {
               console.error('Error fetching cashier queue ticket:', error);
           }
       });
   }
   function updateDirectorQueueTicket() {
       $.ajax({
           url: 'http://localhost:65388/api/QueueTicket/GetQueueTicket?department=director',
           method: 'GET',
           success: function (data) {
               // Update the registrar queue ticket number on the page
               $('#directorQueueTicket').text(data.CurrentQueueTicket);
           },
           error: function (error) {
               console.error('Error fetching director queue ticket:', error);
           }
       });
   }
   function updateStudentaffairsandservicesQueueTicket() {
       $.ajax({
           url: 'http://localhost:65388/api/QueueTicket/GetQueueTicket?department=studentaffairsandservices',
           method: 'GET',
           success: function (data) {
               // Update the registrar queue ticket number on the page
               $('#studentaffairsandservicesQueueTicket').text(data.CurrentQueueTicket);
           },
           error: function (error) {
               console.error('Error fetching studentaffairsandservices queue ticket:', error);
           }
       });
   }
    // Update registrar queue ticket on page load
   $(document).ready(function () {
       updateRegistrarQueueTicket();
       updateVideoSource();

       setInterval(function () {
           updateRegistrarQueueTicket();
           
       }, 5000);
   });

    $(document).ready(function () {
        updateCashierQueueTicket();

        // Update cashier queue ticket every 5 seconds (adjust as needed)
        setInterval(function () {
            updateCashierQueueTicket();
        }, 5000);
    });

    $(document).ready(function () {
        updateDirectorQueueTicket();

        // Update director queue ticket every 5 seconds (adjust as needed)
        setInterval(function () {
            updateDirectorQueueTicket();
        }, 5000);
    });

    $(document).ready(function () {
        updateStudentaffairsandservicesQueueTicket();

        // Update studentaffairsandservices queue ticket every 5 seconds (adjust as needed)
        setInterval(function () {
            updateStudentaffairsandservicesQueueTicket();
        }, 5000);
    });

  
    // Variable to store the previous recall information for each department
    var previousRecallInfo = {
        registrar: "",
        cashier: "",
        studentaffairsandservices: "",
        director: ""
    };

    // Variable to track whether the page has loaded for the first time
    var isFirstLoad = true;

    // Function to update recall information on the server
    function updateRecallInfo(recallInfo, department) {
        $.ajax({
            url: 'http://localhost:65388/api/Recall/PostRecallInfo',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ RecallInformation: recallInfo, Department: department }),
            success: function (response) {
                console.log('Recall information updated successfully:', response);
            },
            error: function (error) {
                console.error('Error updating recall information:', error);
            }
        });
    }

    // Function to retrieve recall information from the server
    function getRecallInfo(department) {
        $.ajax({
            url: 'http://localhost:65388/api/Recall/GetRecallInfo?department=' + department,
            method: 'GET',
            success: function (response) {
                console.log('Recall information retrieved successfully for ' + department + ':', response);

                // Update the hidden field value with the retrieved recall information
                $('#' + department + 'queueticketrecall').val(response.RecallInfo);

                // Speak the recall information if it's updated or if it's the first load
                if (response.RecallInfo !== previousRecallInfo[department] || isFirstLoad) {
                    console.log('New recall information detected for ' + department + '. Speaking...');
                    // Speak the recall information only if it's not the first load
                    if (!isFirstLoad) {
                        speak(response.RecallInfo, department);
                    }
                    // Update the previous recall information
                    previousRecallInfo[department] = response.RecallInfo;
                }
            },
            error: function (error) {
                console.error('Error retrieving recall information for ' + department + ':', error);
            }
        });
    }

    // Function to speak text
    function speak(text, department) {
        const speech = new SpeechSynthesisUtterance();
        speech.text = text + ". Please come to " + department + " department."; // Adding the department message
        speech.volume = 1;
        speech.rate = 1;
        speech.pitch = 1;
        window.speechSynthesis.speak(speech);
    }

    // Update recall information on page load for each department
    $(document).ready(function () {
        // Periodically update recall information for each department
        setInterval(function () {
            getRecallInfo('registrar');
            getRecallInfo('cashier');
            getRecallInfo('studentaffairsandservices');
            getRecallInfo('director');
            // After the first load, set isFirstLoad to false
            if (isFirstLoad) {
                isFirstLoad = false;
            }
        }, 5000); // Update every 5 seconds, adjust the interval as needed
    });

    // Example: update recall information for registrar when a button is clicked
    $('#updateRegistrarRecallButton').click(function () {
        var recallInfo = $('#registrarqueueticketrecall').val();
        var department = 'registrar';
        updateRecallInfo(recallInfo, department);
    });

    // Example: update recall information for cashier when a button is clicked
    $('#updateCashierRecallButton').click(function () {
        var recallInfo = $('#cashierqueueticketrecall').val();
        var department = 'cashier';
        updateRecallInfo(recallInfo, department);
    });

    // Example: update recall information for student affairs and services when a button is clicked
    $('#updateStudentAffairsRecallButton').click(function () {
        var recallInfo = $('#studentaffairsandservicesqueueticketrecall').val();
        var department = 'studentaffairsandservices';
        updateRecallInfo(recallInfo, department);
    });

    // Example: update recall information for director when a button is clicked
    $('#updateDirectorRecallButton').click(function () {
        var recallInfo = $('#directorqueueticketrecall').val();
        var department = 'director';
        updateRecallInfo(recallInfo, department);
    });

</script>
       
</head>
<body>
        <img id="logo" src="/images/logo2.png" alt="PUP Logo" />  


    <form id="form1" runat="server">
                <asp:HiddenField ID="queueticketrecall" runat="server" />

        <div class="container-fluid">
     <!-- Video Player Section -->
<div class="row mt-4">
    <div class="col-md-6 video-container">
<h1 style="font-size: 24px !important; color: white !important;">PUP Paranaque Campus Queue Management System</h1>
        <video id="PlayVideo" class="w-100" controls autoplay>
            Your browser does not support the video tag.
        </video>
    </div>

    <!-- Department Queues Section -->
    <div class="col-md-6 card-container">
        <div class="card">
            <div class="card-header bg-primary text-white">
                Cashier Current Serving:
            </div>
            <div class="card-body">
                <!-- Display Cashier Queue Ticket Here -->
                <h1 id="cashierQueueTicket" runat="server">-</h1>
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-success text-white">
                Registrar Current Serving:
            </div>
            <div class="card-body">
                <!-- Display Registrar Queue Ticket Here -->
                <h1 id="registrarQueueTicket" runat="server">-</h1>
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-info text-white">
                Student Affairs Current Serving:
            </div>
            <div class="card-body">
                <!-- Display Student Affairs Queue Ticket Here -->
                <h1 id="studentaffairsandservicesQueueTicket" runat="server">-</h1>
            </div>
        </div>

        <div class="card">
            <div class="card-header bg-warning text-white">
                Director Current Serving:
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