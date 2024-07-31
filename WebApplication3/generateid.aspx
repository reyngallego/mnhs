<!DOCTYPE html>
<html>
<head>
    <title>Student Management System</title>
    <meta charset="utf-8" />
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <link href="/styles/idLayout.css" rel="stylesheet" />
    <link href="/styles/idGenerator.css" rel="stylesheet" />
    <link href="/styles/custom.css" rel="stylesheet" />
    <style>
        
      
    </style>
</head>
<body>
    <header>
    <div class="header1">
        <div class="header-overlay"></div>
        <h1>ID Generator</h1>
    </div>
</header>
     <div class="container">
        <!-- Search and Filter section -->
        <div id="id-generator-search">
            <form action="/search" method="GET" class="search-container">
                <input type="text" placeholder="Search students..." class="search-input" id="searchInput">
                <img src="/images/icons/search.png" alt="Search" class="search-icon">
            </form>
            <div class="filter-section">
                <label for="grade-filter">Grade:</label>
                <select id="grade-filter" class="form-control">
                    <option value="all">All</option>
                    <option value="Grade 7">Grade 7</option>
                    <option value="Grade 8">Grade 8</option>
                    <option value="Grade 9">Grade 9</option>
                    <option value="Grade 10">Grade 10</option>
                </select>
                <label for="section-filter">Section:</label>
                <select id="section-filter" class="form-control">
                    <option value="all">All</option>
                </select>
            </div>
        </div>


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
        
        <!-- Pagination Controls -->
        <div id="pagination-controls">
            <button type="button" id="prev-page" class="pagination-button">Previous</button>
            <span id="page-info">Page 1</span>
            <button type="button" id="next-page" class="pagination-button">Next</button>
        </div>

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
                                    <img class="id-picture" src="/images/id/sample.jpg" id="student-picture" alt="Student picture">
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
           
            
            
        </script>
</body>
</html>