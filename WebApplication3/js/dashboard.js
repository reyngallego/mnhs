function renderPieChart(data) {
    const pieCtx = document.getElementById('myPieChart').getContext('2d');

    const pieData = {
        labels: ['Present', 'Absent'],
        datasets: [{
            data: [data[0].TotalTimeIn, data[0].TotalRecords - data[0].TotalTimeIn],
            backgroundColor: ['#115D33', '#4CBB17'],
            hoverBackgroundColor: ['#115D33', '#4CBB17'],
            borderWidth: 1,
        }]
    };

    const pieOptions = {
        responsive: false,
        plugins: {
            legend: {
                position: 'top',
            },
            tooltip: {
                enabled: true,
            }
        }
    };

    new Chart(pieCtx, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    });
}
// Function to update time and date
function updateTimeDate() {
    var currentDate = new Date();
    var options = { weekday: 'long', month: 'long', day: 'numeric' };
    var dateString = currentDate.toLocaleDateString('en-US', options);
    var timeString = currentDate.toLocaleTimeString();
    document.getElementById("dateDisplay").textContent = dateString;
    document.getElementById("timeDisplay").textContent = timeString;
}

// Update time and date initially and every second
updateTimeDate();
setInterval(updateTimeDate, 1000);