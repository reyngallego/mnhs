document.getElementById('print-button').addEventListener('click', function () {
    printPreview();
});

function printPreview() {
    window.print();
}

window.onafterprint = function () {
    $(".main-content").load("reports.aspx", function () {
        isContentLoading = false;
        attachEventHandlers();
    });
};