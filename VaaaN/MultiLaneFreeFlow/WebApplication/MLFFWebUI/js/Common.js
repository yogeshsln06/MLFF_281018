
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
    if (element.files.length > 0) {
        var file = element.files[0];
        //var filesize = file.size;
        var filename = file.name || '';
        var fileSize = Math.round(file.size / 1024);
        if (fileSize > 500) {
            $(ParentDiv).find('input:file').val('');
            alert("Please select image less than 500 KB.");
            return false;
        }
        //else {
        //    debugger;
        //    var url = URL.createObjectURL(file);
        //    var img = new Image();
        //    img.src = url;


        //    img.onload = function () {
        //        img_width = img.width;
        //        img_height = img.height;
        //        context.drawImage(img, 0, 0, 300, 600);
        //    }
        //}
        $(element).parent().find('.filespan').text('Update File');
        $(element).parent().find('.text-danger').text('');
        if ($(element).parent().find('br').length == 0) {
            $("<br/>").insertAfter($(element));
        }
        $(element).prev().removeAttr('onclick').attr("onclick", "trigerImg(this)").show();


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
                $(element).parent().find('img').attr('src', e.target.result).hide();
            }
            Preader.readAsDataURL(element.files[0]);
        }
    }
    else {
        $(element).parent().find('br').remove();
        $(element).prev().hide();
        $(element).next().find('span').text('Attach File');
        $(element).parent().find('img').attr('src', '');
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
    if ($("#fildset").find('input:text').length > 0)
        $("#fildset").find('input:text').val('');
    if ($("#fildset").find('input:file').length > 0)
        $("#fildset").find('input:file').val('');
    if ($("#fildset").find('input:number').length > 0)
        $("#fildset").find('input:number').val('');
    if ($("#fildset").find('input:email').length > 0)
        $("#fildset").find('input:email').val('');
    if ($("#fildset").find('input:datetime').length > 0)
        $("#fildset").find('input:datetime').val('');
}

function openImagePreview(ctrl) {
    var modalImg = document.getElementById("img01");
    modalImg.src = $(ctrl).attr('src');
    var modal = $("#ImageModal");
    var body = $(window);
    var w = modal.width();
    var h = modal.height();
    var bw = body.width();
    var bh = body.height();
    modal.css({
        "position": "absolute",
        "top": ((bh - h) / 2) + "px",
        "left": ((bw - w) / 2) + "px"
    })
    $('#ImageModal').modal({ backdrop: 'static', keyboard: false })
    $('#ImageModal').modal('show');
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).parent().find('.text-danger').text(errorMsg);
}

function GetCityList() {
    var ProvinceId = $("#ProvinceId").val();
    $.ajax
    ({
        url: '/Registration/GetCityList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            ProvinceId: +ProvinceId
        }),
        success: function (result) {
            $("#CityId").html("");
            $("#DistrictId").html("");
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');

            $("#CityId").append
               ($('<option></option>').val(0).html(''));

            $("#DistrictId").append
           ($('<option></option>').val(0).html(''));

            $("#SubDistrictId").append
          ($('<option zipcode=""></option>').val(0).html(''));

            $.each($.parseJSON(result), function (i, city) {
                $("#CityId").append
                ($('<option></option>').val(city.CityId).html(city.CityName))
                if (city.CityId == $("#hfCityId").val()) {
                    $("#CityId").val($("#hfCityId").val());
                    GetDistrictList();
                }
            })
        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetDistrictList() {
    var CityId = $("#CityId").val();
    $.ajax
    ({
        url: '/Registration/GetDistrictList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            CityId: +CityId
        }),
        success: function (result) {

            $("#DistrictId").html("");
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');
            $("#DistrictId").append
            ($('<option></option>').val(0).html(''));

            $("#SubDistrictId").append
        ($('<option zipcode=""></option>').val(0).html(''));
            $.each($.parseJSON(result), function (i, district) {
                $("#DistrictId").append
                ($('<option></option>').val(district.DistrictId).html(district.DistrictName))
                if (district.DistrictId == $("#hfDistrictId").val()) {
                    $("#DistrictId").val($("#hfDistrictId").val());
                    GetSubDistrictList();
                }
            })



        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetSubDistrictList() {
    var DistrictId = $("#DistrictId").val();
    $.ajax
    ({
        url: '/Registration/GetSubDistrictList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            DistrictId: +DistrictId
        }),
        success: function (result) {
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');
            $("#SubDistrictId").append
            ($('<option zipcode=""></option>').val(0).html(''));
            $.each($.parseJSON(result), function (i, subdistrict) {
                $("#SubDistrictId").append
                ($('<option zipcode="' + subdistrict.ZipCode + '"></option>').val(subdistrict.SubDistrictId).html(subdistrict.SubDistrictName))

                if (subdistrict.SubDistrictId == $("#hfSubDistrictId").val()) {
                    $("#SubDistrictId").val($("#hfSubDistrictId").val());
                    $("#PostalCode").val($("#hfPostalCode").val());
                }
            })
        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetZip(ctrl) {
    var option = $('option:selected', ctrl).attr('zipcode');
    $('#PostalCode').val(option || '');
}

function ChnagePasswordOpen() {
    $("#CurrentPassword").val('')
    showError($("#CurrentPassword"), '');
    $("#NewPassword").val('')
    showError($("#NewPassword"), '');
    $('#password_modal').modal('show');
}

function ChnagePassword() {
    var validate = true;
    if ($("#CurrentPassword").val() == '') {
        showError($("#CurrentPassword"), $("#CurrentPassword").attr('data-val-required'));
        validate = false;
    }
    else {
        showError($("#CurrentPassword"), '');
    }
    if ($("#NewPassword").val() == '') {
        showError($("#NewPassword"), $("#NewPassword").attr('data-val-required'));
        validate = false;
    }
    else {
        showError($("#NewPassword"), '');
    }
    if (validate) {
        var Inputdata = {
            CurrentPassword: $("#CurrentPassword").val(),
            NewPassword: $("#NewPassword").val(),
        }

        $(".animationload").show();
        $.ajax({
            type: "POST",
            url: "/SetUp/ChnagePassword",
            dataType: "JSON",
            async: true,
            data: JSON.stringify(Inputdata),
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {

                $(".animationload").hide();
                if (resultData[0].ErrorMessage == 'changed') {
                    $('#password_modal').modal('hide');
                }
                else {
                    alert("unable to chnage password")
                }

            },
            error: function (ex) {
                $(".animationload").hide();
            }

        });
    }

}

function openFilterpopup() {
    $('#filterModel').modal('show');
    $(".modal-backdrop.show").hide();
}

function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;

    return true;
}

function trigerImg(ctrl) {
    $(ctrl).parent().find('img').trigger('click');
}

function CurrecyFormat(bilangan) {
    if (bilangan == null) {
        return ""
    }
    else {
        var number_string = bilangan.toString(),
            sisa = number_string.length % 3,
            rupiah = number_string.substr(0, sisa),
            ribuan = number_string.substr(sisa).match(/\d{3}/g);

        if (ribuan) {
            separator = sisa ? '.' : '';
            rupiah += separator + ribuan.join('.');
        }
        return rupiah;
    }
}

