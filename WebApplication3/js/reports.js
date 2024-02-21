$(document).ready(function () {
    // Function to fetch data from the API and update the table
    function fetchReports() {
        // Your API endpoint URL
        var apiUrl = '/api/QueueReports/GetQueueReports';

        // Additional parameters (if needed)
        var dateFilter = $('#dateFilter').val();
        console.log('Date Filter:', dateFilter); // Log date filter value

        var departmentFilters = $('.department-checkbox:checked').map(function () {
            return $(this).data('department');
        }).get();

        // Make AJAX request
        $.ajax({
            url: apiUrl,
            type: 'GET',
            data: {
                dateFilter: dateFilter, // Pass the date filter value
                departmentFilters: departmentFilters
            },
            success: function (data) {
                // Clear existing table rows
                $('#reports-list').empty();

                // Update table with new data
                $.each(data, function (index, report) {
                    var row = '<tr>' +
                        '<td>' + report.ReportID + '</td>' +
                        '<td>' + report.QueueTicket + '</td>' +
                        '<td>' + report.Department + '</td>' +
                        '<td>' + report.DoneDate + '</td>' +
                        '<td>' + report.Timer + '</td>' +
                        '</tr>';

                    $('#reports-list').append(row);
                });
            },
            error: function (error) {
                console.error('Error fetching reports: ', error);
            }
        });
    }

    // Call fetchReports initially
    fetchReports();

    // Bind fetchReports to the change event of date and department filters
    $('#dateFilter, .department-checkbox').on('change', function () {
        fetchReports();
    });
});
