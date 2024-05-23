<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="generateid.aspx.cs" Inherits="WebApplication3.generateid" %>

<!DOCTYPE html>
<html>
<head>
    <title>Student Management System</title>
    <meta charset="utf-8" />
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <link href="/styles/idLayout.css" rel="stylesheet" />
    <link href="/styles/custom.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <h2>ID Generator</h2>
       
        <!-- Table to display students -->
        <table id="studentTable" class="table">
            <thead>
                <tr>
                    <th>LRN</th>
                    <th>Name</th>
                    <th>Grade</th>
                    <th>Section</th>
                    <th>Adviser</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <!-- Student data will be dynamically inserted here -->
            </tbody>
        </table>
      <!-- Modal for previewing the ID -->
<div class="modal fade" id="generateIdModal" tabindex="-1" role="dialog" aria-labelledby="generateIdModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="generateIdModalLabel">ID Preview</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body" id="id-preview-modal">
                <!-- Previewing the ID with student credentials -->
                <div class="id-card-front" id="id-card-front">
                    <div class="id-card-front-header">
                        <p><b>Republic of the Philippines</b></p>
                        <p>National Capital Region</p>
                        <p>Schools Division of Parañaque City</p>
                        <h3>MOONWALK NATIONAL HIGH SCHOOL</h3>
                        <p class="address">St. Mary's St., Daang Batang St., San Agustin Village, Moonwalk, Parañaque City</p>
                    </div>
                    <div class="id-card-front-content">
                        <div class="details">
                            <img class="school-logo" src="/images/id/logo.png" alt="MwNHS Logo">
                            <img class="id-picture" src="/images/id/sample.jpg" id="student-picture"  alt="Student picture">
                            <ul>
                                <li class="lrn" id="student-lrn">LRN: <u></u></li>
                                <li id="academic-year">SY: 2023 - 2024</li>
                                <li class="student-details-front" id="student-name">Name: </li>
                                  <li>Name</li>
                                <li class="student-details-front" id="student-grade-section">Grade and Section: </li>
                                <li>Grade and Section</li>
                                <li class="student-details-front">Dr. Alpha M. Burgos, Ph. D, Ed. D </li>
                                <li>Principal</li>
                            </ul>
                        </div>
                    </div>
                    <div class="id-card-front-footer">STUDENT</div>
                </div>
                <div class="id-card-back" id="id-card-back">
                    <div class="id-card-back-header">
                        <p><strong>This ID card is non-transferrable</strong></p>
                        <p>In case of emergency, please contact:</p>
                    </div>
                    <div class="details">
                        <div class="id-card-back-content">
                            <ul>
                                <li class="student-details-back" id="ParentFullName">Name: </li>
                                  <li>Name</li>
                                <li class="student-details-back" id="studentaddress">Address: </li>
                                  <li>Address</li>
                                <li class="student-details-back" id="parentnumber">Contact number: </li>
                                 <li>Contact number</li>
                                <li class="signature"> </li>
                                  <li>Signature</li>

                            </ul>
                        </div>
                        <div class="qr-code" id="qr-code"></div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button id="print-id" class="btn btn-primary">Print ID</button>
            </div>
        </div>
    </div>
</div>

     
    <!-- Bootstrap JS dependencies (jQuery, Popper.js, Bootstrap JS) -->
   <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/dom-to-image/2.6.0/dom-to-image.min.js"></script>
    <script src="../Scripts/html2canvas.js"></script>
    <script src="../js/generate_id.js"></script>
    
       
        <script>
            function generateQRCode(identifier) {
                console.log('generateQRCode function called');

                const qrContent = `${identifier}`;
                console.log('QR content:', qrContent);

                try {
                    const qrCodeDiv = document.querySelector(".qr-code");
                    console.log('QR code div:', qrCodeDiv);

                    // Clear any existing QR code
                    qrCodeDiv.innerHTML = '';

                    new QRCode(qrCodeDiv, {
                        text: qrContent,
                        width: 100,
                        height: 100,
                        colorDark: "#000000",
                        colorLight: "#ffffff",
                        correctLevel: QRCode.CorrectLevel.H,
                    });

                    console.log('QR code generated successfully');
                } catch (error) {
                    console.error('Error generating QR code:', error);
                }
            }

            // Call generateIdModal with a test LRN (replace '1234567890' with an actual LRN)
            generateIdModal('23232');


            function printID() {
                // Capture id-card-front as canvas
                html2canvas(document.getElementById('id-card-front')).then(function (canvasFront) {
                    // Capture id-card-back as canvas
                    html2canvas(document.getElementById('id-card-back')).then(function (canvasBack) {
                        // Create a new canvas to combine id-card-front and id-card-back
                        var combinedCanvas = document.createElement('canvas');
                        var combinedContext = combinedCanvas.getContext('2d');

                        // Set the combined canvas size
                        combinedCanvas.width = Math.max(canvasFront.width, canvasBack.width);
                        combinedCanvas.height = canvasFront.height + canvasBack.height;

                        // Draw id-card-front and id-card-back on the combined canvas
                        combinedContext.drawImage(canvasFront, 0, 0);
                        combinedContext.drawImage(canvasBack, 0, canvasFront.height);

                        // Convert the combined canvas to an image
                        var imgData = combinedCanvas.toDataURL('image/png');

                        // Open a new window with the image for printing
                        var printWindow = window.open('', '_blank');
                        printWindow.document.open();
                        printWindow.document.write('<img src="' + imgData + '" style="display: block; margin: 0 auto;">');
                        printWindow.document.close();

                        // Trigger the print dialog
                        printWindow.print();
                    });
                });
            }
        </script>
</body>
</html>