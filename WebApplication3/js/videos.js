$(document).ready(function () {
    // Load videos when the "Videos" tab is clicked
    $('a[href="#videos"]').on('shown.bs.tab', function (e) {
        loadVideos();
    });

    // Event handler for clicking the "Upload Video" button
    $('#submitVideoButton').click(function () {
        // Validate if all required fields are filled
        var videoName = $('#videoNameInput').val().trim();
        var description = $('#videoDescriptionInput').val().trim();
        var fileInput = $('#videoFileInput')[0];

        if (videoName === '' || description === '' || fileInput.files.length === 0) {
            alert('Please fill in all required fields.');
            return;
        }

        // Prepare form data
        var formData = new FormData();
        formData.append('videoName', videoName);
        formData.append('description', description);
        formData.append('file', fileInput.files[0]);

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

    // Event handler for confirming video selection
    $('#confirm-select-video-btn').click(function () {
        if (selectedVideo) {
            selectVideo(selectedVideo.VideoId);
        }
    });

    // Event handler for confirming video deletion
    $('#confirm-delete-video-btn').click(function () {
        if (selectedVideo) {
            deleteVideo(selectedVideo.VideoId);
        }
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
                '<button type="button" class="btn btn-danger btn-sm delete-video-btn" data-video-id="' + video['VideoId'] + '">Delete</button>' +
                '<button type="button" class="btn btn-info btn-sm play-video-button" data-video-url="' + video['VideoUrl'] + '" data-video-data="' + video['VideoData'] + '">Play</button>' +
                '<button type="button" class="btn btn-success btn-sm select-video-btn" data-video-id="' + video['VideoId'] + '">Select</button>' +
                '</td>' +
                '</tr>';

            videoTableBody.append(row);
        });
    }

    // Function to make the AJAX request and select the video
    function selectVideo(videoId) {
        $.ajax({
            url: '/api/video/selectvideo',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ VideoId: videoId }), // Pass the selected video ID in the request body
            success: function (response) {
                console.log('Video selected successfully:', response);
                loadVideos(); // Refresh the video list
                $('#selectVideoModal').modal('hide'); // Hide modal after selection
            },
            error: function (error) {
                console.error('Error selecting video:', error);
            }
        });
    }

    // Function to delete a video
    function deleteVideo(videoId) {
        $.ajax({
            url: '/api/video/deletevideo/' + videoId,
            type: 'DELETE',
            success: function (response) {
                console.log('Video deleted successfully:', response);
                loadVideos(); // Refresh the video list
                $('#deleteVideoModal').modal('hide'); // Hide modal after deletion
            },
            error: function (error) {
                console.error('Error deleting video:', error);
            }
        });
    }

    // Event handler for selecting a video
    $(document).on('click', '.select-video-btn', function () {
        var videoId = $(this).data('video-id');
        selectedVideo = { VideoId: videoId }; // Store the selected video for confirmation
        $('#selectVideoModal').modal('show');
    });

    // Event handler for deleting a video
    $(document).on('click', '.delete-video-btn', function () {
        var videoId = $(this).data('video-id');
        selectedVideo = { VideoId: videoId }; // Store the selected video for confirmation
        $('#deleteVideoModal').modal('show');
    });

    // Function to play a video
    function playVideo(videoUrl, videoData) {
        try {
            var decodedVideoData = atob(videoData);
            var videoPlayerModal = $('#videoPlayerModal');
            var videoPlayer = $('#videoPlayer');

            videoPlayer.attr('src', 'data:video/mp4;base64,' + videoData);
            videoPlayerModal.modal('show');

            videoPlayerModal.on('hidden.bs.modal', function () {
                videoPlayer.get(0).pause();
            });
        } catch (error) {
            console.error('Error playing video:', error);
        }
    }

    $(document).on('click', '.play-video-button', function () {
        var videoUrl = $(this).data('video-url');
        var videoData = $(this).data('video-data');
        playVideo(videoUrl, videoData);
    });
});
