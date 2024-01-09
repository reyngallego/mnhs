$(document).ready(function () {
    // Function to fetch purpose data from the API
    function fetchPurposeData() {
        $.ajax({
            url: "/api/purpose",
            method: "GET",
            success: function (data) {
                // Clear existing purpose list
                $("#purpose-list").empty();

                // Populate purpose list with data from the API
                data.forEach(function (purpose) {
                    $("#purpose-list").append(
                        "<tr><td>" + purpose.PurposeID + "</td><td>" + purpose.PurposeType + "</td></tr>"
                    );
                });
            },
            error: function (error) {
                console.error("Error fetching purpose data:", error);
            },
        });
    }

    // Initial fetch of purpose data
    fetchPurposeData();

    // Event listener for tab click
    $("a[data-toggle='tab']").on("shown.bs.tab", function (e) {
        // Check if the clicked tab is the "Purpose" tab
        if ($(e.target).attr("href") === "#purpose") {
            // Fetch and display purpose data
            fetchPurposeData();
        }
    });
});