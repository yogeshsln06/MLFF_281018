var Transcationcol;
var TranscationId;
function GetAssociatedTranscation(ctrl) {
    Transcationcol = ctrl;
    TranscationId = $(Transcationcol).attr("data-id");

    $('#loader').removeClass('fadeOut');
    $.ajax({
        type: "POST",
        url: "AssociatedTransaction?TranscationId=" + TranscationId ,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);
            openpopup();
            $('#loader').addClass('fadeOut');

        },
        error: function (x, e) {
            $('#loader').addClass('fadeOut');
            closePopup();
        }

    });
}

function openpopup() {
    $("#warning").hide();
    $("#btnAssociatedModalOpen").trigger('click');
}

function closePopup() {
    $("#btnpopupClose").trigger('click');
}

function zoomImage(ctrl) {
    //var viewer = ImageViewer();
    //var imgSrc = ctrl.src,
    //           highResolutionImage = $(ctrl).data('high-res-img');
    //viewer.show(imgSrc, highResolutionImage);
    // $('.enlargeImageModalSource').attr('src', $(ctrl).attr('src'));
    $('#enlargeImageModal').modal('show');
}

function SaveUnidentified() {
    $('#loader').removeClass('fadeOut');
    $.ajax({
        type: "POST",
        url: "SaveUnidentified?TranscationId=" + TranscationId,
        async: true,
        dataType: "JSON",
        contentType: "application/json; charset=utf-8",
        success: function (resultData) {
            $('#loader').addClass('fadeOut');
            var meassage = '';
            for (var i = 0; i < resultData.length; i++) {
                if (resultData[i].ErrorMessage == "success") {
                    closePopup();
                    $(Transcationcol).parent().parent().remove();
                }
                else if (resultData[i].ErrorMessage == 'logout') {
                    location.href = "../Login/Logout";
                    break;
                }
                else {
                    meassage = meassage + "<li>" + resultData[i].ErrorMessage + "</li>"
                }
            }
            if (meassage != '') {
                $("#warning").html("<ul>" + meassage + "</ul>");
                $("#warning").show();
            }
        },
        error: function (x, e) {
            $('#loader').addClass('fadeOut');
        }

    });
}