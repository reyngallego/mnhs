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
