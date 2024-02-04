
var countdownTimeout;

// Function to format seconds as HH:MM:SS
function formatTime(seconds) {
    var hours = Math.floor(seconds / 3600);
    var minutes = Math.floor((seconds % 3600) / 60);
    var remainingSeconds = seconds % 60;

    var formattedTime =
        (hours < 10 ? "0" : "") + hours + ":" +
        (minutes < 10 ? "0" : "") + minutes + ":" +
        (remainingSeconds < 10 ? "0" : "") + remainingSeconds;

    return formattedTime;
}

// Function to start the countdown timer
function startCountdown(seconds) {
    $("#lblservingTimeLabel").text("" + formatTime(seconds));
    countdownTimeout = setTimeout(function () {
        startCountdown(seconds + 1);
    }, 1000);
}

// Function to stop the countdown timer
function stopCountdown() {
    clearTimeout(countdownTimeout);
    var formattedTimer = formatTime(countdownSeconds); // Assuming countdownSeconds is the variable storing the timer value
    $("#lblservingTimeLabel").text("Serving Time");

    // Update the hidden field with the formatted timer value
    $("#<%= hdnFormattedTimer.ClientID %>").val(formattedTimer);
}