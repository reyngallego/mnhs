document.getElementById('print-button').addEventListener('click', function () {
    printPreview();
});

function printPreview() {
    window.print();
}

window.onafterprint = function () {
    $(".main-content").load("reports.aspx", function () {
        isContentLoading = false;
        attachEventHandlers();
    });
};

function searchStudent() {
    var lrn = document.getElementsByName('search')[0].value;

    if (!lrn) {
        alert('Please enter the student LRN.');
        return;
    }

    $.ajax({
        url: '/api/student/search',
        method: 'GET',
        data: {
            lrn: lrn
        },
        success: function (data) {
            $('#studentName').text('Student Name: ' + data.FirstName + ' ' + data.LastName);
            $('#learnerReferenceNumber').text('Learner Reference Number: ' + data.LRN);
            $('#gradeAndSection').text('Total Present: ' + data.TotalPresent + ', Total Absent: ' + data.TotalAbsent);

            // Populate attendance table with pagination
            var recordsPerPage = 10;
            var tbody = $('#attendanceTable tbody');
            var totalRecords = data.AttendanceRecords.length;
            var totalPages = Math.ceil(totalRecords / recordsPerPage);

            function renderTable(page) {
                tbody.empty();
                var start = (page - 1) * recordsPerPage;
                var end = start + recordsPerPage;
                var paginatedRecords = data.AttendanceRecords.slice(start, end);

                paginatedRecords.forEach(function (record) {
                    var row = '<tr>' +
                        '<td>' + new Date(record.Date).toLocaleDateString() + '</td>' +
                        '<td>' + record.Day + '</td>' +
                        '<td>' + record.Status + '</td>' +
                        '<td>' + (record.TimeIn ? record.TimeIn : '') + '</td>' +
                        '<td>' + (record.TimeOut ? record.TimeOut : '') + '</td>' +
                        '</tr>';
                    tbody.append(row);
                });

                $('#entry-count').text('Showing ' + (start + 1) + ' to ' + (Math.min(end, totalRecords)) + ' of ' + totalRecords + ' entries');
            }

            function renderPagination() {
                var pagination = $('#pagination');
                pagination.empty();
                for (var i = 1; i <= totalPages; i++) {
                    var pageLink = $('<a href="#" class="page-link">' + i + '</a>');
                    pageLink.click((function (page) {
                        return function (e) {
                            e.preventDefault();
                            renderTable(page);
                        };
                    })(i));
                    pagination.append(pageLink);
                }
            }

            renderTable(1);
            renderPagination();
        },
        error: function () {
            alert('Student not found');
        }
    });
}
