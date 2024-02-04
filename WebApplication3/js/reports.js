$(document).ready(function () {
    var reports;  // Declare reports variable

    // Fetch and display reports on page load
    fetchAndDisplayReports();

    // Event listener for department checkboxes
    $(document).on('change', '.department-checkbox', function () {
        filterReports();
    });

    // Event listener for date filter
    $('#dateFilter').on('change', function () {
        filterReports();
    });

    function fetchAndDisplayReports() {
        $.ajax({
            type: 'GET',
            url: '/api/reports/getqueuereports',
            success: function (data) {
                reports = data;  // Set the reports variable
                displayReports(reports);
            },
            error: function () {
                alert('Error fetching data from the server.');
            }
        });
    }

    function displayReports(reports) {
        var tbody = $('#reports-list');
        tbody.empty();

        for (var i = 0; i < reports.length; i++) {
            var report = reports[i];

            var row = $('<tr>');
            row.append('<td>' + report.ReportID + '</td>');
            row.append('<td>' + report.QueueTicket + '</td>');
            row.append('<td>' + report.Department + '</td>');
            row.append('<td>' + report.DoneDate + '</td>');

            // Add Timer column
            row.append('<td>' + report.Timer + '</td>');

            row.append('<td><input type="checkbox" class="department-checkbox" data-department="' + report.Department + '"></td>');

            tbody.append(row);
        }

        filterReports(); // Apply initial filters
    }

    function filterReports() {
        var selectedDepartments = $('.department-checkbox:checked').map(function () {
            return $(this).data('department');
        }).get();

        var selectedDate = $('#dateFilter').val();

        var filteredReports = reports.filter(function (report) {
            return (selectedDepartments.length === 0 || selectedDepartments.includes(report.Department)) &&
                (selectedDate === '' || new Date(selectedDate).toISOString().split('T')[0] === report.DoneDate.split('T')[0]);
        });

        displayReports(filteredReports);
    }
});
