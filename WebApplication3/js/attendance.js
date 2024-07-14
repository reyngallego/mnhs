var currentState = 'enter'; // Initialize the current state to 'enter'

function enterOrLeaveAttendance() {
    if (currentState === 'enter') {
        enterAttendance();
    } else {
        leaveAttendance();
    }
}

function toggleState() {
    currentState = currentState === 'enter' ? 'leave' : 'enter';
    $('#lrnInput').attr('placeholder', currentState === 'enter' ? 'Enter LRN' : 'Leave LRN');
}

$('#toggleButton').on('click', function () {
    toggleState();
});

$('#lrnInput').on('keypress', function (event) {
    if (event.which === 13) { // Check if the pressed key is Enter (key code 13)
        event.preventDefault(); // Prevent default form submission behavior
        scanLRN(); // Call the scanLRN function
    }
});

function scanLRN() {
    var lrn = $('#lrnInput').val().trim();

    if (lrn !== '') {
        if (currentState === 'enter') {
            enterAttendance();
        } else {
            leaveAttendance();
        }
        $('#lrnInput').val(''); // Clear the input field
    } else {
        alert('Please enter LRN first.');
    }
}
$(document).ready(function () {
    $.ajax({
        url: '/api/attendance/getattendancedata',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Clear the table body
            $('#attendanceTable tbody').empty();

            // Populate the table with data
            $.each(data, function (index, row) {
                var fullName = row.FirstName + ' ' + row.LastName;

                // Format time in and time out
                var timeIn = formatTime(row.TimeIn);
                var timeOut = formatTime(row.TimeOut);

                // Format date
                var date = formatDate(row.Date);

                $('#attendanceTable tbody').append(
                    '<tr>' +
                    '<td>' + row.LRN + '</td>' +
                    '<td>' + fullName + '</td>' +
                    '<td>' + timeIn + ' - ' + timeOut + '</td>' +
                    '<td>' + date + '</td>' +
                    '<td>' + row.Grade + '-' + row.Section + '</td>' +
                    '</tr>'
                );
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
});
function enterAttendance() {
    // Get the LRN value from the input field
    var lrn = $('#lrnInput').val().trim(); // Ensure there are no leading/trailing spaces
    console.log('LRN to be sent:', lrn); // Log the LRN

    $.ajax({
        url: '/api/attendance/enterattendance',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ lrn: lrn }), // Pass the LRN as an object
        success: function (response) {
            $('#statusNotification').removeClass('alert alert-danger').addClass('alert alert-success').text('Attendance entered successfully').show();
            console.log('Attendance entered successfully');
            refreshAttendanceTable();
            fetchParentContact(lrn);

            // Optional: Refresh attendance table after successful entry
            refreshAttendanceTable();

            // Assuming you want to send an SMS notification to parents
            $.ajax({
                url: '/api/parent/contact',
                type: 'GET',
                data: { lrn: lrn },
                success: function (parentResponse) {
                    var parentContacts = parentResponse.contacts;
                    var smsPayload = {
                        messages: [
                            {
                                destinations: parentContacts.map(contact => ({ to: contact })),
                                from: "ServiceSMS",
                                text: "Congratulations on sending your first message.\nGo ahead and check the delivery report in the next step."
                            }
                        ]
                    };

                    // Send SMS
                    sendSMS(smsPayload);
                },
                error: function (xhr, textStatus, errorThrown) {
                    handleAjaxError(xhr); // Handle error in fetching parent contact
                    console.error('Error fetching parent contact:', errorThrown);
                }
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 400) {
                var responseText = JSON.parse(xhr.responseText);
                if (responseText.Message === "You have already signed in.") {
                } else {
                    $('#statusNotification').removeClass('alert alert-success').addClass('alert alert-danger').text('Student Does not exist or Has Already Signed in').show();
                }
            } else {
                if (xhr.responseXML) {
                    var xmlString = new XMLSerializer().serializeToString(xhr.responseXML);
                    console.log('XML Error Response:', xmlString);
                } else {
                    console.log('Error:', errorThrown);
                    console.log('Status Code:', xhr.status);
                    console.log('Response Text:', xhr.responseText);
                }
            }
        }
    });
}
// Function to send SMS
function sendSMS(payload) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", "App 81d503eb320ed748f41e10ebee9fbb6e-ee998e47-890e-4766-9521-0c87e02fd435");
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Accept", "application/json");

    const requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: JSON.stringify(payload),
        redirect: "follow"
    };

    fetch("https://z1zpj6.api.infobip.com/sms/2/text/advanced", requestOptions)
        .then((response) => response.text())
        .then((result) => console.log(result))
        .catch((error) => console.error(error));
}
function leaveAttendance() {
    var lrn = $('#lrnInput').val();

    $.ajax({
        url: '/api/attendance/leaveattendance',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(lrn),
        success: function (response) {
            $('#statusNotification').removeClass('alert alert-danger').addClass('alert alert-success').text('Attendance left successfully').show();
            console.log('Attendance left successfully');
            refreshAttendanceTable();

        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 400) {
                // LRN does not exist or student has already left
                $('#statusNotification').removeClass('alert alert-success').addClass('alert alert-danger').text(xhr.responseJSON.Message).show();
            } else {
                console.log('Error:', errorThrown);
            }
        }
    });
}
function refreshAttendanceTable() {
    // Reload attendance data after time in recorded
    $.ajax({
        url: '/api/attendance/getattendancedata',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Clear the table body
            $('#attendanceTable tbody').empty();

            // Populate the table with data
            $.each(data, function (index, row) {
                var fullName = row.FirstName + ' ' + row.LastName;

                // Format time in and time out
                var timeIn = formatTime(row.TimeIn);
                var timeOut = formatTime(row.TimeOut);

                // Format date
                var date = formatDate(row.Date);

                $('#attendanceTable tbody').append(
                    '<tr>' +
                    '<td>' + row.LRN + '</td>' +
                    '<td>' + fullName + '</td>' +
                    '<td>' + timeIn + ' - ' + timeOut + '</td>' +
                    '<td>' + date + '</td>' +
                    '<td>' + row.Grade + '-' + row.Section + '</td>' +
                    '</tr>'
                );
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error:', errorThrown);
        }
    });
}
function formatTime(timeString) {
    if (!timeString || timeString === '0001-01-01T00:00:00') {
        return ''; // Handle empty or invalid time
    }
    var time = new Date(timeString);
    return time.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }); // Format time
}

function formatDate(dateString) {
    if (!dateString || dateString === '0001-01-01T00:00:00') {
        return ''; // Handle empty or invalid date
    }
    var date = new Date(dateString);
    return date.toLocaleDateString(); // Format date
}
