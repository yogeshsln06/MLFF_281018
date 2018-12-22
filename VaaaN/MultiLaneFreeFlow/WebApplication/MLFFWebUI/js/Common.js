
$(document).mouseup(function (e) {
    var container = $(".gridbtn");

     //if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.addClass('hide').removeClass('open').hide();
    }
});

function openFilter(ctrl) {
    var ddls = $(ctrl).next();
    if ($(ddls).hasClass("open")) {
        $(ddls).addClass('hide').removeClass('open').hide();
    }
    else {
        $(ddls).show().addClass('open');
    }
}

function zoomImage(ctrl) {
    var viewer = ImageViewer();
    var imgSrc = ctrl.src,
               highResolutionImage = $(ctrl).data('high-res-img');
    viewer.show(imgSrc, highResolutionImage);
}





function encodeImagetoBase64(element) {
    var file = element.files[0];
    var ParentDiv = $(element).parent();
    var ancoreTag = $(element).parent().find('a');
    var ValidateFile = file;
    var reader = new FileReader();
    reader.onloadend = function () {
        $(ancoreTag).attr("href", reader.result);
        $(ancoreTag).text(reader.result);
    }
    reader.readAsDataURL(file);

    if (element.files[0]) {
        var Preader = new FileReader();
        Preader.onload = function (e) {
            $(ParentDiv).next().find('img').attr('src', e.target.result);
        }
        Preader.readAsDataURL(element.files[0]);
    }
}

function findAndReplace(string, target, replacement) {
    var i = 0, length = string.length;
    for (i; i < length; i++) {
        string = string.replace(target, replacement);
    }
    return string;

}