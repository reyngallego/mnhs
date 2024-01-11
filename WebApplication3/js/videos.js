// Define your client-side models or classes here
function Videos(videoId, videoName, videoData, description, videoUrl) {
    this.VideoId = videoId;
    this.VideoName = videoName;
    this.VideoData = videoData;
    this.Description = description;
    this.VideoUrl = videoUrl;
}
var selectedVideo;

$(document).ready(function () {
    // Load videos when the "Videos" tab is clicked
    $('a[href="#videos"]').on('shown.bs.tab', function (e) {
        loadVideos();
    });

    // Event handler for clicking the "Upload Video" button
    $('#submitVideoButton').click(function () {
        // Prepare form data
        var formData = new FormData();
        formData.append('videoName', $('#videoNameInput').val());
        formData.append('description', $('#videoDescriptionInput').val());
        formData.append('file', $('#videoFileInput')[0].files[0]);

        // AJAX request to upload the video
        $.ajax({
            url: '/api/video/uploadvideo',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                // Refresh the video list after a successful upload
                loadVideos();
                $('#uploadVideoModal').modal('hide');
            },
            error: function (error) {
                console.error('Error uploading video:', error);
            }
        });
    });

    
    // Function to load videos from the server
    function loadVideos() {
        $.ajax({
            url: '/api/video/getvideos',
            method: 'GET',
            success: function (data) {
                displayVideos(data);
            },
            error: function (xhr, status, error) {
                console.error('Error loading videos:', xhr.responseText); // Log the full response text
            }
        });
    }

    function displayVideos(videos) {
        var videoTableBody = $('#video-list');
        videoTableBody.empty();

        $.each(videos, function (index, video) {
            var row =
                '<tr>' +
                '<td>' + video['VideoId'] + '</td>' +
                '<td>' + video['VideoName'] + '</td>' +
                '<td>' + video['Description'] + '</td>' +
                '<td>' +
                '<button type="button" class="btn btn-danger btn-sm delete-video-btn">Delete</button>' +
                '<button type="button" class="btn btn-info btn-sm play-video-button" data-video-url="' + video['VideoUrl'] + '" data-video-data="' + video['VideoData'] + '">Play</button>' +
                '<button type="button" class="btn btn-success btn-sm select-video-btn" data-video-id="' + video['VideoId'] + '">Select</button>' +
                '</td>' +
                '</tr>';

            videoTableBody.append(row);
        });

        // Attach event handler using event delegation
        videoTableBody.on('click', '.delete-video-btn', function () {
            // Get the VideoId from the data attribute
            var videoId = $(this).closest('tr').find('td:first').text();
            // Call the deleteVideo function with the extracted VideoId
            deleteVideo(videoId);
        });
    }
    // Function to make the AJAX request and select the video
    function selectVideo(videoId) {
        // Make an AJAX request to select the video
        $.ajax({
            url: '/api/video/selectvideo',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ VideoId: videoId }), // Pass the selected video ID in the request body
            success: function (response) {
                console.log('Video selected successfully:', response);

                // Perform any UI updates or other actions as needed
                // For example, you can display a success message or refresh the video list
                loadVideos(); // Refresh the video list
            },
            error: function (error) {
                console.error('Error selecting video:', error);
            }
        });
    }
    // Assuming your videos are rendered dynamically, delegate the click event to a parent element
    $('#video-list').on('click', '.select-video-btn', function () {
        // Extract the video ID from the data attribute
        var videoId = $(this).data('video-id');

        // Log the selected video ID to the console
        console.log('Selected Video ID:', videoId);

        // Call the selectVideo function to make the AJAX request
        selectVideo(videoId);
    });
    $('#video-list').on('click', '.delete-video-button', function () {
        // Extract the video ID from the data attribute
        var videoId = $(this).closest('tr').find('td:first').text();

        // Call the deleteVideo function
        deleteVideo(videoId);
    });
    // Function to delete a video
    function deleteVideo(videoId) {
        // Implement the logic to delete a video using AJAX
        // You may want to confirm the deletion with a modal
        // and then make an AJAX request to the server

        $.ajax({
            url: '/api/video/deletevideo/' + videoId,
            type: 'DELETE',
            success: function (response) {
                // Refresh the video list after a successful deletion
                loadVideos();
            },
            error: function (error) {
                console.error('Error deleting video:', error);
            }
        });
    }

    // Function to play a video
    function playVideo(videoUrl, videoData) {
        try {
            // Decode base64 and set as the source of the video player
            var decodedVideoData = atob(videoData);

            console.log('Decoded Video Data:', decodedVideoData);

            var videoPlayerModal = $('#videoPlayerModal');
            var videoPlayer = $('#videoPlayer');

            videoPlayer.attr('src', 'data:video/mp4;base64,' + videoData);

            videoPlayerModal.modal('show');

            // Pause the video when the modal is closed
            videoPlayerModal.on('hidden.bs.modal', function () {
                videoPlayer.get(0).pause();
            });
        } catch (error) {
            console.error('Error playing video:', error);
        }
    }


    // Add this function to initialize the video player modal
    function initializeVideoPlayerModal(videoUrl) {
        console.log('Initializing video player with URL:', videoUrl);

        var videoPlayerModal = $('#videoPlayerModal');
        var videoPlayer = $('#videoPlayer');

        videoPlayer.attr('src', videoUrl);

        videoPlayerModal.modal('show');

        // Pause the video when the modal is closed
        videoPlayerModal.on('hidden.bs.modal', function () {
            videoPlayer.get(0).pause();
        });
    }

    $(document).on('click', '.play-video-button', function () {
        var videoUrl = $(this).data('video-url');
        var videoData = $(this).data('video-data');
        playVideo(videoUrl, videoData);
    });

});
