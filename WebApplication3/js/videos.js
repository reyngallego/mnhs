// Define your client-side models or classes here
function Videos(videoId, videoName, videoData, description, videoUrl) {
    this.VideoId = videoId;
    this.VideoName = videoName;
    this.VideoData = videoData;
    this.Description = description;
    this.VideoUrl = videoUrl;
}

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

    // Function to display videos in the table
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
                '<button type="button" class="btn btn-danger btn-sm" onclick="deleteVideo(' + video['VideoId'] + ')">Delete</button>' +
                '<button type="button" class="btn btn-info btn-sm play-video-button" data-video-url="' + video['VideoUrl'] + '">Play</button>' +
                '</td>' +
                '</tr>';

            videoTableBody.append(row);
        });
    }

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
    function playVideo(videoId, videoUrl) {
        // Call the function to initialize the video player modal
        initializeVideoPlayerModal(videoUrl);
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
        var videoId = $(this).data('video-id');
        var videoUrl = $(this).data('video-url');
        playVideo(videoId, videoUrl);
    });
});
