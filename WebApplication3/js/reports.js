$(document).ready(function () {
    // Function to fetch data from the API and update the table
    function fetchReports() {
        // Your API endpoint URLs
        var apiUrl = '/api/QueueReports/GetQueueReports';
        var totalTodayUrl = '/api/QueueReports/TotalToday';
        var averageTimerUrl = '/api/QueueReports/AverageTimer'; // New API endpoint for average timer

        // Additional parameters (if needed)
        var dateFilter = $('#dateFilter').val();
        console.log('Date Filter:', dateFilter); // Log date filter value

        var departmentFilters = $('.department-checkbox:checked').map(function () {
            return $(this).data('department');
        }).get();

        // Make AJAX request to fetch reports
        $.ajax({
            url: apiUrl,
            type: 'GET',
            data: {
                dateFilter: dateFilter, // Pass the date filter value
                departmentFilters: departmentFilters // Pass the department filters
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

                // Fetch total processed today
                $.ajax({
                    url: totalTodayUrl,
                    type: 'GET',
                    data: {
                        dateFilter: dateFilter.toString(), // Convert date filter to string
                        departmentFilters: departmentFilters // Pass the department filters
                    },
                    success: function (totalToday) {
                        // Update total processed today in HTML
                        $('#totalProcessed').text(totalToday);
                    },
                    error: function (error) {
                        console.error('Error fetching total processed today: ', error);
                    }
                });

                $.ajax({
                    url: averageTimerUrl,
                    type: 'GET',
                    data: {
                        dateFilter: dateFilter, // Pass the date filter value
                        departmentFilters: departmentFilters // Pass the department filters
                    },
                    success: function (averageTimer) {
                        // Check if averageTimer is a valid time string in the format "hh:mm:ss"
                        if (/^\d{2}:\d{2}:\d{2}$/.test(averageTimer)) {
                            // Update average processed time in HTML
                            $('#averageProcessed').text(averageTimer); // Display the time directly
                        } else {
                            // Display "N/A" if average timer is not valid
                            $('#averageProcessed').text("N/A");
                        }
                    },
                    error: function (error) {
                        console.error('Error fetching average processed time: ', error);
                        // Display "N/A" if there's an error
                        $('#averageProcessed').text("N/A");
                    }
                });

                // Fetch department-wise total processed data and render bar graph
                fetchDepartmentTotals();
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

    // Function to format time from seconds to HH:mm:ss format
    function formatTime(seconds) {
        var hours = Math.floor(seconds / 3600);
        var minutes = Math.floor((seconds % 3600) / 60);
        var remainingSeconds = seconds % 60;
        return hours + ":" + padZero(minutes) + ":" + padZero(remainingSeconds);
    }

    // Function to pad single digits with leading zeros
    function padZero(num) {
        return (num < 10 ? '0' : '') + num;
    }

    $(document).ready(function () {
        // Function to fetch data from the API and update the table and bar graph
        function fetchReports() {
            // Your API endpoint URLs
            var totalRegistrarUrl = '/api/QueueReports/TotalRegistrar';
            var totalCashierUrl = '/api/QueueReports/TotalCashier';
            var totalStudentAffairsUrl = '/api/QueueReports/TotalStudentAffairs';
            var totalDirectorUrl = '/api/QueueReports/TotalDirector';

            // Additional parameters (if needed)
            var dateFilter = $('#dateFilter').val();
            var departmentFilters = $('.department-checkbox:checked').map(function () {
                return $(this).data('department');
            }).get();

            // Make AJAX requests to fetch total processed for each department
            $.when(
                $.ajax({ url: totalRegistrarUrl, type: 'GET', data: { dateFilter: dateFilter, departmentFilters: departmentFilters } }),
                $.ajax({ url: totalCashierUrl, type: 'GET', data: { dateFilter: dateFilter, departmentFilters: departmentFilters } }),
                $.ajax({ url: totalStudentAffairsUrl, type: 'GET', data: { dateFilter: dateFilter, departmentFilters: departmentFilters } }),
                $.ajax({ url: totalDirectorUrl, type: 'GET', data: { dateFilter: dateFilter, departmentFilters: departmentFilters } })
            ).done(function (registrarTotal, cashierTotal, studentAffairsTotal, directorTotal) {
                var totals = {
                    registrar: registrarTotal[0],
                    cashier: cashierTotal[0],
                    STUDENTAFFAIRSANDSERVICES: studentAffairsTotal[0],
                    director: directorTotal[0]
                };
                renderBarGraph(totals); // Render the bar graph
            }).fail(function (error) {
                console.error('Error fetching total processed: ', error);
            });
        }

        // Function to render the bar graph
        function renderBarGraph(totals) {
            // Define custom department labels
            var customLabels = {
                'registrar': 'Registrar',
                'cashier': 'Cashier',
                'STUDENTAFFAIRSANDSERVICES': 'Student Affairs and Services',
                'director': 'Director'
            };

            // Extract department names and totals from the response
            var departments = Object.keys(totals).map(function (key) {
                return customLabels[key] || key; // Use custom label if available, otherwise use the original department name
            });
            var counts = Object.values(totals);

            // Create a bar graph using Chart.js
            var ctx = document.getElementById('barGraph').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: departments, // Use custom department labels as labels for x-axis
                    datasets: [{
                        label: 'Total Processed',
                        data: counts, // Use total counts as data for y-axis
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)'
                            // Add more colors if you have more departments
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)'
                            // Add more colors if you have more departments
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
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

    });
