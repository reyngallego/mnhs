/*

var node = document.getElementById('id-card-front');
var counter = 0;

function htmlToImg() {
    html2canvas(document.body, {
        allowTaint: true,
        foreignObjectRendering: true,
        useCORS: true
    })
        .then(function (canvas) {
            var imgUrl = canvas.toDataURL("image/png");
            imgDown(imgUrl);
        })
        .catch(function (error) {
            console.error('Something went wrong', error);
        });
}

function imgDown(imgUrl) {
    var a = document.createElement('a');
    a.href = imgUrl;
    a.download = "htmlToImg_" + counter + ".png";
    a.click();
    ++counter;
}

*/


let printBtn = document.querySelector("#print-id");
var counter = 1

printBtn.addEventListener("click", function () {
    html2canvas(document.querySelector("#id-modal-preview")).then(function (canvas) {
        var link = document.querySelector("#id-modal-preview");
        link.setAttribute("download", "htmlToImg"+ counter +".png");
        link.setAttribute(
            "href",
            canvas.toDataURL("image/png").replace("image/png", "image/octet-stream")
        );
        link.click();
    });

    ++counter;

    window.print();
});

saveBtn.addEventListener("click", function () {
    
});