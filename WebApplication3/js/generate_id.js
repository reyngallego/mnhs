function populateTable() {
    $.ajax({
        type: "GET",
        url: "/api/mnhs/GetStudents",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#studentTable tbody").empty();

            $.each(response, function (index, student) {
                var row = `
                    <tr>
                        <td>${student.LRN}</td>
                        <td>${student.FirstName} ${student.LastName}</td>
                        <td>${student.Grade}</td>
                        <td>${student.Section}</td>
                        <td>${student.Adviser}</td>
                        <td>
                      <button type="button" class="btn btn-primary btn-sm mb-3" data-toggle="modal" data-target="#generateIdModal" onclick="generateIdModal('${student.LRN}')">
    Generate ID
</button>
                        </td>
                    </tr>`;
                $("#studentTable tbody").append(row);
            });
        },
        error: function (error) {
            console.error('Error fetching students:', error);
        }
    });
}
function generateIdModal(LRN) {
    console.log("Generate ID button clicked with LRN:", LRN);

    // Retrieve student data by LRN and populate the modal fields
    $.ajax({
        type: "GET",
        url: "/api/generateid/GetStudentByLRN?LRN=" + LRN,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (student) {
            // Populate the modal fields with student data
            $("#student-lrn").text(" LRN: " + LRN);
            $("#student-name").text(" " + student.FirstName + " " + student.LastName);
            $("#student-grade-section").text("" + student.Grade + " " + student.Section);
            $("#student-adviser").text(" " + student.Adviser);
            $("#ParentFullName").text(" " + student.ParentFullName);
            $("#studentaddress").text(" " + student.Address);
            $("#parentnumber").text(" " + student.ParentContact);

            generateQRCode(LRN);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            alert("Error: " + errorThrown);
        }
    });
}



// Ensure that the table is populated on document ready
$(document).ready(function () {
    populateTable();

    let printBtn = document.querySelector("#print-id");
    var counter = 1;

    printBtn.addEventListener("click", function () {
        html2canvas(document.querySelector("#id-modal-preview")).then(function (canvas) {
            var link = document.createElement("a");
            link.setAttribute("download", "htmlToImg" + counter + ".png");
            link.setAttribute(
                "href",
                canvas.toDataURL("image/png").replace("image/png", "image/octet-stream")
            );
            link.click();
        });

        ++counter;

        window.print();
    });
});
