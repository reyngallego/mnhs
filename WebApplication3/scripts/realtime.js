function updateData() {
    // Make an AJAX request to fetch the updated data
    $.ajax({
        type: 'POST',
        url: 'DataFetch.aspx', // Update with your server-side endpoint
        dataType: 'json',
        success: function (data) {
            // Handle the updated data here
            if (data) {
                // Update UI elements with the fetched data
                document.getElementById('timestamp').textContent = data.timestamp;
                // Add more lines to update other UI elements as needed
            }
        },
        error: function () {
            // Handle errors if the request fails
            console.error('Error fetching data.');
        }
    });
}

// Call the updateData function initially and then at regular intervals
$(document).ready(function () {
    // Fetch and update data initially
    updateData();

    // Set up a timer to periodically update data (every 5 seconds in this example)
    setInterval(updateData, 5000); // Adjust the interval as needed (in milliseconds)
});