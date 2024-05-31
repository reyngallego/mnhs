document.addEventListener("DOMContentLoaded", function () {
    console.log('DOM fully loaded and parsed');

    const pieCtx = document.getElementById('myPieChart').getContext('2d');
    console.log('Pie chart context:', pieCtx);

    const pieData = {
        labels: ['Face-to-Face', 'Absent'],
        datasets: [{
            data: [70, 30],
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
    const myPieChart = new Chart(pieCtx, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    });
    console.log('Pie chart created:', myPieChart);

    const barCtx = document.getElementById('myChart').getContext('2d');
    console.log('Bar chart context:', barCtx);

    const barData = {
        labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
        datasets: [{
            label: 'Present',
            data: [25, 30, 28, 32, 27],
            backgroundColor: 'rgba(76, 175, 80, 0.5)',
            borderColor: 'rgba(76, 175, 80, 1)',
            borderWidth: 1
        },
        {
            label: 'Absent',
            data: [5, 2, 4, 1, 3],
            backgroundColor: 'rgba(244, 67, 54, 0.5)',
            borderColor: 'rgba(244, 67, 54, 1)',
            borderWidth: 1
        }]
    };
    const barOptions = {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    };
    const myChart = new Chart(barCtx, {
        type: 'bar',
        data: barData,
        options: barOptions
    });
    console.log('Bar chart created:', myChart);
});
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