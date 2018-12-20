function openpopup() {
    $("#warning").hide();
    $("#btnpopupOpen").trigger('click');
}

function closePopup() {
    $("#btnpopupClose").trigger('click');
}

function validEmail(str) {
    var pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return String(str).match(pattern);
}

function validMobile(str) {
    var pattern = /^\d+$/;
    return String(str).match(pattern);
}

function validTAGId(str) {
    var pattern = /^[0-9a-fA-F]+$/;
    return String(str).match(pattern);
}

function validateCustomerVehicle() {
    var valid = true;

    if ($("#ResidentId").val() == '') {
        showError($("#ResidentId"), $("#ResidentId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ResidentId"), '');
    }

    if ($("#FirstName").val() == '') {
        showError($("#FirstName"), $("#FirstName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#FirstName"), '');
    }

    if ($("#MobileNo").val() == '') {
        showError($("#MobileNo"), $("#MobileNo").attr('data-val-required'));
        valid = false;
    }
    else if (!validMobile($("#MobileNo").val())) {
        showError($("#MobileNo"), 'Enter Valid Mobile Number');
        valid = false;
    }
    else {
        showError($("#MobileNo"), '');
    }

    if ($("#EmailId").val() == '') {
        showError($("#EmailId"), $("#EmailId").attr('data-val-required'));
        valid = false;
    }
    else if (!validEmail($("#EmailId").val())) {
        showError($("#EmailId"), 'Enter Valid Email Id');
        valid = false;
    }
    else {
        showError($("#EmailId"), '');
    }

    if ($("#Address").val() == '') {
        showError($("#Address"), $("#Address").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#Address"), '');
    }

    if ($("#VehicleRCNumber").val() == '') {
        showError($("#VehicleRCNumber"), $("#VehicleRCNumber").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehicleRCNumber"), '');
    }

    if ($("#VehRegNo").val() == '') {
        showError($("#VehRegNo"), $("#VehRegNo").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehRegNo"), '');
    }

    if ($("#OwnerName").val() == '') {
        showError($("#OwnerName"), $("#OwnerName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#OwnerName"), '');
    }

    if ($("#OwnerAddress").val() == '') {
        showError($("#OwnerAddress"), $("#OwnerAddress").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#OwnerAddress"), '');
    }

    if ($('#VehicleImageFront')[0].files.length === 0 && $('#hfVehicleImageFront').val() == "") {
        showError($("#VehicleImageFront"), 'Front Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageFront"), '');
    }

    if ($('#VehicleImageRear')[0].files.length === 0 && $('#hfVehicleImageRear').val() == "") {
        showError($("#VehicleImageRear"), 'Rear Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageRear"), '');
    }

    if ($('#VehicleImageRight')[0].files.length === 0 && $('#hfVehicleImageRight').val() == "") {
        showError($("#VehicleImageRight"), 'Right Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageRight"), '');
    }

    if ($('#VehicleImageLeft')[0].files.length === 0 && $('#hfVehicleImageLeft').val() == "") {
        showError($("#VehicleImageLeft"), 'Left Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageLeft"), '');
    }

    if ($('#VehicleRCNumberImagePath')[0].files.length === 0 && $('#hfVehicleRCNumberImage').val() == "") {
        showError($("#VehicleRCNumberImagePath"), 'Registration Certificate Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleRCNumberImagePath"), '');
    }

    if ($("#TidFront").val() == '') {
        showError($("#TidFront"), $("#TidFront").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#TidFront"), '');
    }

    if ($("#TidRear").val() == '') {
        showError($("#TidRear"), $("#TidRear").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#TidRear"), '');
    }

    if ($("#VehicleClassId").val() == 0) {
        showError($("#VehicleClassId"), $("#VehicleClassId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehicleClassId"), '');
    }

    if ($("#TagId").val() == '') {
        showError($("#TagId"), $("#TagId").attr('data-val-required'));
        valid = false;
    }
    else if ($("#TagId").val().length != 24) {
        showError($("#TagId"), 'Valid EPC Required');
        valid = false;
    }
    else if (!validTAGId($("#TagId").val())) {
        showError($("#TagId"), 'Valid EPC Required');
        valid = false;
    }
    else {
        showError($("#TagId"), '');
    }


    return valid;
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}

function GetTagId() {
    if ($("#VehRegNo").val() == '') {
        showError('Enter Vehicle Registration No');
        return false;
    }
    else if ($("#VehicleClassId").val() == 0) {
        showError('Select Vehicle Class');
        return false;
    }
    var InpurField = {
        VehicleClassId: $("#VehicleClassId").val(),
        VehRegNo: $("#VehRegNo").val()
    }

    $.ajax({
        type: "POST",
        url: "GetTagId",
        dataType: "JSON",
        async: true,
        data: JSON.stringify(InpurField),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#TagId").val(data.Data);
        },
        error: function (ex) {

        }
    });
}

function zoomImage(ctrl) {
    var viewer = ImageViewer();
    var imgSrc = ctrl.src,
               highResolutionImage = $(ctrl).data('high-res-img');
    viewer.show(imgSrc, highResolutionImage);
}

function openImg(ctrl) {
    var id = $(ctrl).attr('id')
    $("#" + id.replace('lbl', 'img')).trigger('click');
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

}

function NewCustomerVehicle() {
    $('#loader').removeClass('fadeOut');
    $.ajax({
        type: "GET",
        url: "/Registration/NewCustomerVehicle",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $("#exampleModalLabel").text("New Vehicle");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#EntryId").attr("disabled", "disabled");
            $('#loader').addClass('fadeOut');
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#VehicleImageFront").show();
            $("#VehicleImageRear").show();
            $("#VehicleImageRight").show();
            $("#VehicleImageLeft").show();
            $("#VehicleRCNumberImagePath").show();

            $("#lblVehicleImageFront").hide();
            $("#lblVehicleImageRear").hide();
            $("#lblVehicleImageRight").hide();
            $("#lblVehicleImageLeft").hide();
            $("#lblVehicleRCNumberImagePath").hide();
        },
        error: function (x, e) {
            $('#loader').addClass('fadeOut');
        }

    });
}

function DetailsOpen(ctrl, id) {
    $('#loader').removeClass('fadeOut');
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomerVehicle?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("View [" + $('#VehRegNo').val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $('#loader').addClass('fadeOut');
            $("#fildset").attr("disabled", "disabled");
            $("#VehicleImageFront").hide();
            $("#VehicleImageRear").hide();
            $("#VehicleImageRight").hide();
            $("#VehicleImageLeft").hide();
            $("#VehicleRCNumberImagePath").hide();

            $("#lblVehicleImageFront").show();
            $("#lblVehicleImageRear").show();
            $("#lblVehicleImageRight").show();
            $("#lblVehicleImageLeft").show();
            $("#lblVehicleRCNumberImagePath").show();

            $("#imgVehicleImageFront").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageFront").val());
            $("#imgVehicleImageRear").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRear").val());
            $("#imgVehicleImageRight").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRight").val());
            $("#imgVehicleImageLeft").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageLeft").val());
            $("#imgVehicleRCNumberImagePath").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleRCNumberImage").val());
        },
        error: function (x, e) {
            $('#loader').addClass('fadeOut');
        }

    });

}

function EditOpen(ctrl, id) {
    $('#loader').removeClass('fadeOut');
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomerVehicle?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("Update [" + $('#VehRegNo').val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#EntryIdId").attr("disabled", "disabled");
            $('#loader').addClass('fadeOut');
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#VehicleImageFront").show();
            $("#VehicleImageRear").show();
            $("#VehicleImageRight").show();
            $("#VehicleImageLeft").show();
            $("#VehicleRCNumberImagePath").show();

            $("#lblVehicleImageFront").hide();
            $("#lblVehicleImageRear").hide();
            $("#lblVehicleImageRight").hide();
            $("#lblVehicleImageLeft").hide();
            $("#lblVehicleRCNumberImagePath").hide();
        },
        error: function (x, e) {
            $('#loader').addClass('fadeOut');
        }

    });

}

function SaveData(action) {
    if ($("#needs-validation").valid()) {
        var ValidUntil = $('#ValidUntil').val() || ''
        if (ValidUntil != '') {
            ValidUntil = DateFormat(ValidUntil);
        }

        if (validateCustomerVehicle()) {
            var EntryId = $("#EntryId").val() || '';
            var AccountId = $("#AccountId").val() || '';
            var ImageFrontChnage = true;
            var ImageRearChnage = true;
            var ImageLeftChnage = true;
            var ImageRightChnage = true;
            var RCNumberImageChnage = false;
            var PostURL = '/Registration/CustomerVehicleUpdate';
            var VehicleImageFront = $("#VehicleImageFrontPath").text().trim();
            var VehicleImageRear = $("#VehicleImageRearPath").text().trim();
            var VehicleImageRight = $("#VehicleImageRightPath").text().trim();
            var VehicleImageLeft = $("#VehicleImageLeftPath").text().trim();
            var VehicleRCNumberImagePath = $("#VehicleRCNumberImagePathPath").text().trim();
            if (EntryId == '') {
                EntryId = 0;
                PostURL = '/Registration/CustomerVehicleAdd'
            }
            else {
                if ($("#VehicleImageFrontPath").text().trim() == '') {
                    VehicleImageFront = $("#hfVehicleImageFront").val();
                    ImageFrontChnage = false;
                }

                if ($("#VehicleImageRearPath").text().trim() == '') {
                    VehicleImageRear = $("#hfVehicleImageRear").val();
                    ImageRearChnage = false;
                }

                if ($("#VehicleImageRightPath").text().trim() == '') {
                    VehicleImageRight = $("#hfVehicleImageRight").val();
                    ImageRightChnage = false;
                }

                if ($("#VehicleImageLeftPath").text().trim() == '') {
                    VehicleImageLeft = $("#hfVehicleImageLeft").val();
                    ImageLeftChnage = false;
                }

                if ($("#VehicleRCNumberImagePathPath").text().trim() == '') {
                    VehicleRCNumberImagePath = $("#hfVehicleRCNumberImage").val();
                    RCNumberImageChnage = false;
                }

            }

            var Inputdata = {
                EntryId: EntryId,
                AccountId: AccountId,
                ResidentId: $("#ResidentId").val(),
                FirstName: $("#FirstName").val(),
                Address: $('#Address').val(),
                MobileNo: $('#MobileNo').val(),
                EmailId: $('#EmailId').val(),
                VehicleRCNumber: $('#VehicleRCNumber').val(),
                VehRegNo: $('#VehRegNo').val(),
                OwnerName: $("#OwnerName").val(),
                OwnerAddress: $("#OwnerAddress").val(),
                Brand: $("#Brand").val(),
                VehicleType: $("#VehicleType").val(),
                VehicleCategory: $("#VehicleCategory").val(),
                Model: $("#Model").val(),
                ManufacturingYear: $("#ManufacturingYear").val(),
                CyclinderCapacity: $("#CyclinderCapacity").val(),
                FrameNumber: $("#FrameNumber").val(),
                EngineNumber: $("#EngineNumber").val(),
                VehicleColor: $("#VehicleColor").val(),
                FuelType: $("#FuelType").val(),
                LicencePlateColor: $("#LicencePlateColor").val(),
                RegistrationYear: $("#RegistrationYear").val(),
                VehicleOwnershipDocumentNumber: $('#VehicleOwnershipDocumentNumber').val(),
                LocationCode: $('#LocationCode').val(),
                RegistrationQueueNumber: $('#RegistrationQueueNumber').val(),
                ValidUntil: ValidUntil,
                VehicleImageFront: VehicleImageFront,
                VehicleImageRear: VehicleImageRear,
                VehicleImageRight: VehicleImageRight,
                VehicleImageLeft: VehicleImageLeft,
                VehicleRCNumberImagePath: VehicleRCNumberImagePath,
                ExceptionFlag: $("#ExceptionFlag").val(),
                TidFront: $("#TidFront").val(),
                TidRear: $("#TidRear").val(),
                VehicleClassId: $("#VehicleClassId").val(),
                TagId: $("#TagId").val(),
                QueueStatus: $("#QueueStatus").val(),
                AccountBalance: $("#AccountBalance").val(),

                ImageFrontChnage: ImageFrontChnage,
                ImageRearChnage: ImageRearChnage,
                ImageLeftChnage: ImageLeftChnage,
                ImageRightChnage: ImageRightChnage,
                RCNumberImageChnage: RCNumberImageChnage,
            }

            $('#loader').removeClass('fadeOut');
            $.ajax({
                type: "POST",
                url: PostURL,
                dataType: "JSON",
                async: true,
                data: JSON.stringify(Inputdata),
                contentType: "application/json; charset=utf-8",
                success: function (resultData) {
                    $('#loader').addClass('fadeOut');
                    var meassage = '';
                    for (var i = 0; i < resultData.length; i++) {
                        if (resultData[i].ErrorMessage == "success") {
                            if (action == 'close')
                                closePopup();

                            else {
                                $("#warning").hide();
                                $('#needs-validation')[0].reset();
                            }
                            break;
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
                error: function (ex) {
                    $('#loader').addClass('fadeOut');
                }
            });
        }
    }
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