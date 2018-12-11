var pageload = 1;
$(document).ready(function () {
    $("#BirthDate").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
        endDate: '-18y'
    });
    $("#ValidUntil").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
    });
});

function validEmail(str) {
    var pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
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
    if ($('#imagepath')[0].files.length === 0 && $('#hfCustomerImagePath').val() == "") {
        showError($("#imagepath"), "Profile Image Required");
        valid = false;
    }
    else {
        showError($("#imagepath"), '');
    }
    if ($('#ResidentidImage')[0].files.length === 0 && $('#hfCustomerDocumentPath').val() == "") {
        showError($('#ResidentidImage'), "Resident Id Card Image Required");
        valid = false;
    }
    else {
        showError($("#imagepath"), '');
    }
   
    return valid;
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}


function GetCityList() {
    var ProvinceId = $("#ProvinceId").val();
    $.ajax
    ({
        url: '/POS/GetCityList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            ProvinceId: +ProvinceId
        }),
        success: function (result) {
            $("#SubDistrictId").html("");
            $("#DistrictId").html("");
            $("#ddlCityId").html("");
            $('#PostalCode').val('');
            $("#ddlCityId").append
               ($('<option></option>').val(0).html('--Select City--'))
            $.each($.parseJSON(result), function (i, city) {
                $("#ddlCityId").append
                ($('<option></option>').val(city.CityId).html(city.CityName))
                if (city.CityId == $("#hfCityId").val() && pageload == 1) {
                    $("#ddlCityId").val($("#hfCityId").val());
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
    var CityId = $("#ddlCityId").val();
    $.ajax
    ({
        url: '/POS/GetDistrictList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            CityId: +CityId
        }),
        success: function (result) {
            $("#SubDistrictId").html("");
            $("#DistrictId").html("");
            $('#PostalCode').val('');
            $("#DistrictId").append
            ($('<option></option>').val(0).html('--Select District--'));
            $.each($.parseJSON(result), function (i, district) {
                $("#DistrictId").append
                ($('<option></option>').val(district.DistrictId).html(district.DistrictName))
                if (district.DistrictId == $("#hfDistrictId").val() && pageload == 1) {
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
        url: '/POS/GetSubDistrictList',
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
            ($('<option zipcode=""></option>').val(0).html('--Select Sub District--'));
            $.each($.parseJSON(result), function (i, subdistrict) {
                $("#SubDistrictId").append
                ($('<option zipcode="' + subdistrict.ZipCode + '"></option>').val(subdistrict.SubDistrictId).html(subdistrict.SubDistrictName))

                if (subdistrict.SubDistrictId == $("#hfSubDistrictId").val() && pageload == 1) {
                    $("#SubDistrictId").val($("#hfSubDistrictId").val());
                    $("#PostalCode").val($("#hfPostalCode").val());
                    pageload = pageload + 1;
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

function Preview(ctrl) {
    $("#dvPreview").html("");
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
    if (regex.test($(ctrl).val().toLowerCase())) {
        if (typeof (FileReader) != "undefined") {
            // $("#dvPreview").show();
            $("#dvPreview").append("<img />");
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#dvPreview img").attr("src", e.target.result);
            }
            reader.readAsDataURL($(ctrl)[0].files[0]);
        }
        else {
            alert("This browser does not support FileReader.");
        }

    } else {
        alert("Please upload a valid image file.");
    }
}