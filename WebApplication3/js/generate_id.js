// Global scope functions
function generateIdModal(LRN) {
    $.ajax({
        type: "GET",
        url: `/api/generateid/GetStudentByLRN?LRN=${LRN}`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (student) {
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

function generateQRCode(identifier) {
    const qrContent = `${identifier}`;

    try {
        const qrCodeDiv = document.querySelector(".qr-code");
        qrCodeDiv.innerHTML = '';

        new QRCode(qrCodeDiv, {
            text: qrContent,
            width: 100,
            height: 100,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H,
        });
    } catch (error) {
        console.error('Error generating QR code:', error);
    }
}

function printID() {
    html2canvas(document.getElementById('id-card-front')).then(function (canvasFront) {
        html2canvas(document.getElementById('id-card-back')).then(function (canvasBack) {
            var combinedCanvas = document.createElement('canvas');
            var combinedContext = combinedCanvas.getContext('2d');

            combinedCanvas.width = Math.max(canvasFront.width, canvasBack.width);
            combinedCanvas.height = canvasFront.height + canvasBack.height;

            combinedContext.drawImage(canvasFront, 0, 0);
            combinedContext.drawImage(canvasBack, 0, canvasFront.height);

            var imgData = combinedCanvas.toDataURL('image/png');

            var printWindow = window.open('', '_blank');
            printWindow.document.open();
            printWindow.document.write('<img src="' + imgData + '" style="display: block; margin: 0 auto;">');
            printWindow.document.close();

            printWindow.print();
        });
    });
}

(() => {
    let currentPage = localStorage.getItem('currentPage') ? parseInt(localStorage.getItem('currentPage')) : 1;
    const pageSize = 10;

    function populateTable(searchTerm = "", gradeFilter = "all", sectionFilter = "all", sortColumn = "LRN", sortOrder = "ASC", pageNumber = currentPage, pageSize = 10) {
        console.log(`Fetching students for page ${pageNumber} with searchTerm: ${searchTerm}, gradeFilter: ${gradeFilter}, sectionFilter: ${sectionFilter}`);
        $.ajax({
            type: "GET",
            url: `/api/generateid/GetStudents?searchTerm=${searchTerm}&gradeFilter=${gradeFilter}&sectionFilter=${sectionFilter}&sortColumn=${sortColumn}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log("Response received:", response);
                $("#studentTable tbody").empty();

                $.each(response.Students, function (index, student) {
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

                const totalRecords = response.TotalRecords;
                const totalPages = Math.ceil(totalRecords / pageSize);
                $("#page-info").text(`Page ${currentPage} of ${totalPages}`);

                const startRecord = (currentPage - 1) * pageSize + 1;
                const endRecord = Math.min(currentPage * pageSize, totalRecords);
                $("#entries-info").text(`Showing ${startRecord} to ${endRecord} out of ${totalRecords} entries`);

                $("#prev-page").prop("disabled", currentPage === 1);
                $("#next-page").prop("disabled", currentPage === totalPages);

                // Reattach event handlers
                attachEventHandlers();
            },
            error: function (error) {
                console.error('Error fetching students:', error);
            }
        });
    }

    function filterTable() {
        const searchTerm = $("#search-input").val();
        const gradeFilter = $("#grade-filter").val();
        const sectionFilter = $("#section-filter").val();
        populateTable(searchTerm, gradeFilter, sectionFilter, "LRN", "ASC", currentPage, pageSize);
    }

    function changePage(delta) {
        currentPage += delta;
        localStorage.setItem('currentPage', currentPage);
        filterTable();
    }

    function attachEventHandlers() {
        $("#search-input").off("keyup").on("keyup", function () {
            currentPage = 1; // Reset to first page
            localStorage.setItem('currentPage', currentPage);
            filterTable();
        });

        $("#grade-filter").off("change").on("change", function () {
            const selectedGrade = $(this).val();
            const $sectionFilter = $("#section-filter");
            $sectionFilter.empty();
            $sectionFilter.append('<option value="all">All</option>');

            if (selectedGrade !== "all") {
                const sections = {
                    "Grade 7": ["Section A", "Section B", "Section C"],
                    "Grade 8": ["Section A", "Section B", "Section C"],
                    "Grade 9": ["Section A", "Section B", "Section C"],
                    "Grade 10": ["Section A", "Section B", "Section C"],
                };
                sections[selectedGrade].forEach(function (section) {
                    $sectionFilter.append('<option value="' + section + '">' + section + '</option>');
                });
            }

            currentPage = 1; // Reset to first page
            localStorage.setItem('currentPage', currentPage);
            filterTable();
        });

        $("#section-filter").off("change").on("change", function () {
            currentPage = 1; // Reset to first page
            localStorage.setItem('currentPage', currentPage);
            filterTable();
        });

        $("#prev-page").off("click").on("click", function (event) {
            event.preventDefault();
            changePage(-1);
        });

        $("#next-page").off("click").on("click", function (event) {
            event.preventDefault();
            changePage(1);
        });

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
    }

    $(document).ready(function () {
        console.log("Document ready, populating table...");
        populateTable();
        attachEventHandlers();
    });
})();
