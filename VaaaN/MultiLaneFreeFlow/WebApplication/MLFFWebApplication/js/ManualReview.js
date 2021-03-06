﻿var ResponceData = [];
var apiPath = '';
var noimagepath = "";
var novideo = "";
var activeTab = '#front'
$(document).ready(function () {
    noimagepath = $("#hfAPIPath").val() + "Content/images/noimage.png";
    novideo = $("#hfAPIPath").val() + "Content/images/sample.mp4";
    $("#frontplateImg").attr('src', noimagepath);
    $("#frontvechileImg").attr('src', noimagepath);
    $("#rearplateImg").attr('src', noimagepath);
    $("#rearvechileImg").attr('src', noimagepath);
    //$("#rearvideo").attr('src', novideo);
    $("#frontvideoview").hide();
    //$("#rearvideoview").hide();

    jQuery('#StartDate').datetimepicker({
        format: 'm/d/Y H:i',

    });

    jQuery('#EndDate').datetimepicker({
        format: 'm/d/Y H:i',

    });

    var d = new Date();

    dd = d.getDate();

    dd = dd > 9 ? dd : '0' + dd;

    mm = (d.getMonth() + 1);


    mm = mm > 9 ? mm : '0' + mm;


    yy = d.getFullYear();

    $("#StartDate").val(mm + '/' + dd + '/' + yy + " 00:01");
    $("#EndDate").val(mm + '/' + dd + '/' + yy + " 23:59");

    $("#GenerateReport").click(function () {
        GenerateReport();
    });
    $(".nav-tabs a").click(function () {
        activeTab = $(this).attr('href');
        $(this).tab('show');
    });

});





function filterData() {
    if ($("#ddlGantry").val() == 0) {
        alert("Gantry harus dipilih ");
        return false;
    }
    var InputDate = {
        StartDate: $("#StartDate").val() + ":00", EndDate: $("#EndDate").val() + ":00",
        GantryId: $("#ddlGantry").val(), VehicleClassId: $("#ddlVehicleClass").val(),
        PlateNumber: $("#PlateNumber").val(),
        TransactionCategoryId: $("#ddlTransactionCategory").val()
    }
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: "FilterManulReview",
        dataType: "JSON",
        async: true,
        data: JSON.stringify(InputDate),
        contentType: "application/json; charset=utf-8",
        success: function (JsonfilterData) {
            $('#loader').hide();
            ResponceData = JsonfilterData;
            var TR;
            if (ResponceData.length > 0) {
                for (var i = 0; i < ResponceData.length; i++) {
                    TR = TR + "<tr><td>" + ResponceData[i].TRANSACTION_ID + "</td><td>" + ResponceData[i].TRANSACTION_DATETIME + "</td><td>" + replacenull(ResponceData[i].CTP_VRN) + "</td><td>" + replacenull(ResponceData[i].FRONT_VRN) + "</td>" +
                       "<td>" + replacenull(ResponceData[i].REAR_VRN) + "</td><td>" + replaceYesNo(ResponceData[i].AUDIT_STATUS) + "</td><td> <i class='fa fa-info-circle' aria-hidden='true' onclick='FilterDataBtTranscationId(" + ResponceData[i].TRANSACTION_ID + ")'></i> </td></tr>"

                    //<a onclick='FilterDataBtTranscationId(" + ResponceData[i].TRANSACTION_ID + ")'>More Details</a>
                }
            }
            else {
                TR = "<tr><td colspan='7'>No record Exists</td></tr>";
            }
            $("#tableTransaction tbody").html(TR);

        },
        error: function (x, e) {
            $('#loader').hide();
        }

    });
}

function replacenull(str) {
    var rep = '';
    if (str != null)
        rep = str;
    return rep;
}

function replaceYesNo(str) {
    var rep = 'Yes';
    if (str == null || str == 0)
        rep = 'No';
    return rep;
}

function replaceAmount(str) {
    var rep = 0;
    if (str == null || str == 0)
        rep = 0;
    else {
        rep = str;
    }
    return (rep).toFixed(3);;
}

function FilterDataBtTranscationId(TranId) {

    var retJson = $.grep(ResponceData, function (element, index) {
        return element.TRANSACTION_ID == TranId;

    });

    if (retJson.length > 0) {
        var Imagepath = $("#hfAPIPath").val();

        $("#LANE_NAME").val(replacenull(retJson[0].LANE_NAME));
        $("#TAG_ID").val(replacenull(retJson[0].TAG_ID));
        $("#LANE_NAME").val(replacenull(retJson[0].LANE_NAME));
        $("#TRANSACTION_ID").val(replacenull(retJson[0].TRANSACTION_ID));
        $("#TRANSACTION_DATETIME").val(replacenull(retJson[0].TRANSACTION_DATETIME));
        $("#CTP_VRN").val(replacenull(retJson[0].CTP_VRN));
        $("#CTP_VEHICLE_CLASS_NAME").val(replacenull(retJson[0].CTP_VEHICLE_CLASS_NAME));
        $("#FRONT_VRN").val(replacenull(retJson[0].FRONT_VRN));
        $("#REAR_VRN").val(replacenull(retJson[0].REAR_VRN));
        $("#NFP_VEHICLE_CLASS_NAME_FRONT").val(replacenull(retJson[0].NFP_VEHICLE_CLASS_NAME_FRONT));
        $("#REAR_VRN").val(replacenull(retJson[0].REAR_VRN));
        $("#NFP_VEHICLE_CLASS_NAME_REAR").val(replacenull(retJson[0].NFP_VEHICLE_CLASS_NAME_REAR));
        $("#AUDIT_STATUS").val(replaceYesNo(retJson[0].AUDIT_STATUS));
        $("#AMOUNT").val(replaceAmount(retJson[0].AMOUNT));
        $("#AUDITOR_ID").val(retJson[0].AUDITOR_ID);
        $("#AUDIT_DATE").val(retJson[0].AUDIT_DATE);
        $("#AUDITED_VEHICLE_CLASS_ID").val(retJson[0].AUDITED_VEHICLE_CLASS_ID);

        if (replacenull(retJson[0].FRONT_IMAGE) == '') {
            $("#frontplateImg").attr('src', noimagepath);
        }
        else {
            $("#frontplateImg").attr('src', Imagepath + retJson[0].FRONT_IMAGE);
        }
        if (replacenull(retJson[0].FRONT_VEHICLE_THUMBNAIL) == '') {
            $("#frontvechileImg").attr('src', noimagepath);
        }
        else {
            $("#frontvechileImg").attr('src', Imagepath + retJson[0].FRONT_VEHICLE_THUMBNAIL);
        }
        // 
        if (replacenull(retJson[0].REAR_IMAGE) == '') {
            $("#rearplateImg").attr('src', noimagepath);
        }
        else {
            $("#rearplateImg").attr('src', Imagepath + retJson[0].REAR_IMAGE);
        }

        if (replacenull(retJson[0].REAR_VEHICLE_THUMBNAIL) == '') {
            $("#rearvechileImg").attr('src', noimagepath);
        }
        else {
            $("#rearvechileImg").attr('src', Imagepath + retJson[0].REAR_VEHICLE_THUMBNAIL);
        }

        if (replacenull(retJson[0].FRONT_VIDEO_URL) == '') {
            var $video = $('#frontvideo video'),
            videoSrc = $('source', $video).attr('src', novideo);
            $video[0].load();
            //$video[0].play();
        }
        else {
            var $video = $('#frontvideo video'),
            videoSrc = $('source', $video).attr('src', retJson[0].FRONT_VIDEO_URL);
            $video[0].load();
            //$video[0].play();
            // $("#frontvideo").attr('src', retJson[0].FRONT_VIDEO_URL);
        }
        if (replacenull(retJson[0].REAR_VIDEO_URL) == '') {
            var $video = $('#rearvideo video'),
            videoSrc = $('source', $video).attr('src', novideo);
            $video[0].load();
        }
        else {
            var $video = $('#rearvideo video'),
            videoSrc = $('source', $video).attr('src', retJson[0].REAR_VIDEO_URL);
            $video[0].load();
        }


    }
    else {
    }

}

function handleClick(radio) {
    if ($(radio).val() == "Image") {
        $(activeTab + "videoview").hide();
        $(activeTab + "imgeview").show();
    }
    else if ($(radio).val() == "Video") {
        $(activeTab + "imgeview").hide();
        $(activeTab + "videoview").show();
    }
}

function openImage(image) {
    var imgpath = $(image).attr('src');
    $("#myimage").attr('src', imgpath);
    var modal = document.getElementById('myModal');
    modal.style.display = "block";
}

function imageclose() {
    var modal = document.getElementById('myModal');
    modal.style.display = "none";

}

function GenerateReport() {
    if ($("#ReportCategory").val() == 0) {
        alert('Report must be selected');
        return false;
    }
    var Inputdata = {
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        ReportCategory: $("#ReportCategory").val(),
    }
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: '../Report/GenerateReport',
        dataType: "JSON",
        async: true,
        data: JSON.stringify(Inputdata),
        contentType: "application/json; charset=utf-8",
        success: function (JsonfilterData) {
            $('#loader').hide();
            location.href = "../Report/ReportPage";

        },
        error: function (ex) {
            $('#loader').hide();
            alert('Unable to generate report ' + ex)

        }

    });
}

function downlaodVideo(rootsrc) {
    var navigateURL = '';
    if (rootsrc == 'front') {
        var $video = $('#frontvideo video'),
            navigateURL = $('source', $video).attr('src');
    }
    else if (rootsrc == 'rear') {
        var $video = $('#rearvideo video'),
                   navigateURL = $('source', $video).attr('src');
    }
    if (navigateURL != '') {
        //var win = window.open(navigateURL, '_blank');

    }
}

