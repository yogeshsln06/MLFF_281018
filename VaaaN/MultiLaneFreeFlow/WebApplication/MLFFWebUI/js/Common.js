
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


function validEmail(str) {
    var pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return String(str).match(pattern);
}

function validMobile(str) {
    var pattern = /^\d+$/;
    return String(str).match(pattern);
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}

function DateFormat(newDate) {
    var d = new Date(newDate);
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    return dd + '-' + mm + '-' + +yy
}

function encodeImagetoBase64(element) {
    var ParentDiv = $(element).parent();
    var file = element.files[0];
    var filesize = file.size;
    if (filesize > 1500000) {
        $(ParentDiv).find('input:file').val('');
        alert("Please Select less than 1.5 MB.");
        return false;
    }

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

function ResetFildes() {
    $("#fildset").find('input:text').val('');
    $("#fildset").find('input:file').val('');
    $("#fildset").find('input:number').val('');
    $("#fildset").find('input:datetime').val('');
}

function openImagePreview(ctrl) {
    var modalImg = document.getElementById("img01");
    modalImg.src = $(ctrl).attr('src');
    $("#btnImageModalOpen").trigger('click');
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}