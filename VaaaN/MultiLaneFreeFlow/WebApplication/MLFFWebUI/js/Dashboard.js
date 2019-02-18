var ResponceData = [];
var ResponceGantryData = [];
var StartDT, EndDT;
var Progress = true;
var stoprefresh = true;
$(document).ready(function () {
    function setHeight() {
        windowHeight = $(window).innerHeight() - 110;
        $('.container-fluid').css('max-height', windowHeight);
    };
    setHeight();

    $(window).resize(function () {
        setHeight();
    });

    BindDateTime(5);
    Progress = true;
    $("#totalFIke").val(0);
    $("#totalRIke").val(0);
    $("#totalFANPR").val(0);
    $("#totalRANPR").val(0);

    GetfirstLoadData(StartDT, EndDT, true)

});

function DateFormatTime(newDate) {
    var d = new Date(newDate);
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    hh = d.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    mints = d.getMinutes();
    mints = mints > 9 ? mints : '0' + mints;
    var time1 = hh + ":" + mints;
    return dd + '-' + mm + '-' + +yy + ' ' + time1
}

function BindDateTime(mints) {
    var cdt = new Date();
    var d = new Date(cdt.setMinutes(cdt.getMinutes() - mints));
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    hh = d.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    mints = d.getMinutes();
    mints = mints > 9 ? mints : '0' + mints;
    var time1 = hh + ":" + mints;
    var newd = new Date();
    hh = newd.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    mints = newd.getMinutes();
    mints = mints > 9 ? mints : '0' + mints;
    var time2 = hh + ":" + mints;
    StartDT = mm + '/' + dd + '/' + yy + " " + time1;
    EndDT = mm + '/' + dd + '/' + yy + " " + time2;

}

function GetfirstLoadData(StartDT, EndDT, loader) {
    if (Progress) {
        var StartDate = DateFormatTime(StartDT);
        var EndDate = DateFormatTime(EndDT);
        var Inputdata = {
            StartDate: StartDate,
            EndDate: EndDate,
        }
        if (loader)
            $(".animationload").show();
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "Dashboard/DashBoardTransactionData",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#filterModel').modal('hide');
                $('.animationload').hide();
                ResponceData = data;
                BindData(data);
                // sleep(10000);
                setTimeout(function () { reloadData(); }, 20000);
               // reloadData();
            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
    }
}



function reloadData() {
    var d = new Date();
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    hh = d.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    var time1 = 00 + ":" + 00 + ":" + 00;
    var newd = new Date();
    hh = newd.getHours();
    hh = hh > 9 ? hh : '0' + hh;
    mints = newd.getMinutes();
    mints = mints > 9 ? mints : '0' + mints;
    var time2 = hh + ":" + mints + ":" + 59;
    StartDT = mm + '/' + dd + '/' + yy + " " + time1;
    EndDT = mm + '/' + dd + '/' + yy + " " + time2;
    GetfirstLoadData(StartDT, EndDT, false);

}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

//$(window).unload(function () {
//    Progress = false;
//});

function BindData(data) {
    BindTopCount(data);
    BindTableRight(data)

}

function BindTopCount(data) {
    $("#totalFIke").text(data[4].TOTALVEHICLE);
    $("#totalRIke").text(data[5].TOTALVEHICLE);
    $("#totalFANPR").text(data[8].TOTALVEHICLE + data[11].TOTALVEHICLE);
    $("#totalRANPR").text(data[9].TOTALVEHICLE + data[12].TOTALVEHICLE);
    var TotalFrontANPR;
    var binddata = {
        TOTAL_VEHICLE:data[0].TOTALVEHICLE,
        TOTAL_REGISTERED:data[1].TOTALVEHICLE,
        TOTAL_UNREGISTERED: data[2].TOTALVEHICLE,

        TOTAL_IKE: data[3].TOTALVEHICLE,
        TOTAL_FRONTIKE:data[4].TOTALVEHICLE,
        TOTAL_REARIKE: data[5].TOTALVEHICLE,
        TOTAL_ANPR: data[6].TOTALVEHICLE,
        TOTAL_UNIDENTIFIEDVRN: data[7].TOTALVEHICLE,
        TOTAL_FRONT_UNIDENTIFIEDVRN: data[8].TOTALVEHICLE,
        TOTAL_REAR_UNIDENTIFIEDVRN: data[9].TOTALVEHICLE,
        TOTAL_DETECTED_VRN: data[10].TOTALVEHICLE,
        TOTAL_FRONT_DETECTEDVRN: data[11].TOTALVEHICLE,
        TOTAL_REAR_DETECTEDVRN: data[12].TOTALVEHICLE,
        TOTAL_TWOWHEELEDIKE: data[13].TOTALVEHICLE,
        TOTAL_SMALLIKE: data[14].TOTALVEHICLE,
        TOTAL_MEDIUMIKE: data[15].TOTALVEHICLE,
        TOTAL_LARGEIKE: data[16].TOTALVEHICLE,
        TOTAL_TWOWHEELEDIKEFRONT: data[17].TOTALVEHICLE,
        TOTAL_SMALLIKEFRONT: data[18].TOTALVEHICLE,
        TOTAL_MEDIUMIKEFRONT: data[19].TOTALVEHICLE,
        TOTAL_LARGEIKEFRONT: data[20].TOTALVEHICLE,
        TOTAL_TWOWHEELEDIKEREAR: data[21].TOTALVEHICLE,
        TOTAL_SMALLIKEREAR: data[22].TOTALVEHICLE,
        TOTAL_MEDIUMIKEREAR: data[23].TOTALVEHICLE,
        TOTAL_LARGEIKEREAR: data[24].TOTALVEHICLE,

        TOTAL_TWOWHEELEDANPR: data[25].TOTALVEHICLE,
        TOTAL_SMALL_ANPR: data[26].TOTALVEHICLE,
        TOTAL_MEDIUM_ANPR: data[27].TOTALVEHICLE,
        TOTAL_LARGE_ANPR: data[28].TOTALVEHICLE,

        //TotalFrontANPR:TOTAL_TWOWHEELED_ANPRFRONT+TOTAL_SMALLANPR_FRONT+TOTAL_MEDIUMANPR_FRONT+TOTAL_LARGEANPR_FRONT,
        TOTAL_TWOWHEELED_ANPRFRONT: data[29].TOTALVEHICLE,
        TOTAL_SMALLANPR_FRONT: data[30].TOTALVEHICLE,
        TOTAL_MEDIUMANPR_FRONT: data[31].TOTALVEHICLE,
        TOTAL_LARGEANPR_FRONT: data[32].TOTALVEHICLE,
        TOTAL_TWOWHEELED_ANPRREAR: data[33].TOTALVEHICLE,
        TOTAL_SMALL_ANPRREAR: data[34].TOTALVEHICLE,
        TOTAL_MEDIUM_ANPRREAR: data[35].TOTALVEHICLE,
        TOTAL_LARGE_ANPRREAR: data[36].TOTALVEHICLE,

        TOTAL_AUTOCHARGED: data[37].TOTALVEHICLE,
        TOTAL_NOTCHARGED: data[38].TOTALVEHICLE,
        TOTAL_MANUALCHARGED: data[39].TOTALVEHICLE,
        TOTAL_REVIEWED: data[40].TOTALVEHICLE,
        TOTAL_VIOLATION: data[41].TOTALVEHICLE,
        TOTAL_UNIDENTIFIED: data[42].TOTALVEHICLE,
        TOTAL_MERGED: data[43].TOTALVEHICLE,
        TOTAL_SMSSEND: data[44].TOTALVEHICLE,
        TOTAL_SUCCESSFULSEND: data[45].TOTALVEHICLE,
        TOTAL_AVERAGESMS_TIMEINSECONDS: data[46].TOTALVEHICLE,

        TOTAL_REGISTERED_VEHICLE: data[47].TOTALVEHICLE,
        TOTAL_PROCESSED_VEHICLE: data[48].TOTALVEHICLE,
        TOTAL_CHARGEABLE_VEHICLE: data[49].TOTALVEHICLE,
        TOTAL_NON_CHARGEABLE_VEHICLE: data[50].TOTALVEHICLE,
        TOTAL_BLACK_LISTED_VEHICLE:data[51].TOTALVEHICLE,
    }
    if (stoprefresh)
        bindChart(binddata);
}

function BindTableRight(data) {
    var TotalVehiclePass = data[0].TOTALVEHICLE;
    var TotalRVehiclePass = data[1].TOTALVEHICLE;
    var TOTAL_UNREGISTERED = data[2].TOTALVEHICLE;
    var TotalCVehicle = data[38].TOTALVEHICLE;
    var TotalSendSms = data[44].TOTALVEHICLE;
    var SuceessSendSms = data[45].TOTALVEHICLE;

    var Per = 0;
    $("#TotalVehicle_DB").text(TotalVehiclePass);

    $("#RegisteredVehicle_DB").text(TotalRVehiclePass);
    Per = (((TotalRVehiclePass / TotalVehiclePass) * 100) || 0).toFixed();
    $("#RegisteredVehicle_DB_PER").text(Per + '%');
    $("#RegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    $("#UnRegisteredVehicle_DB").text(TOTAL_UNREGISTERED);
    Per = (((TOTAL_UNREGISTERED / TotalVehiclePass) * 100) || 0).toFixed();
    $("#UnRegisteredVehicle_DB_PER").text(Per + '%');
    $("#UnRegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    $("#ChargedVehicle_DB").text(TotalCVehicle);
    Per = (((TotalCVehicle / TotalRVehiclePass) * 100) || 0).toFixed();
    $("#ChargedVehicle_DB_PER").text(Per + '%');
    $("#ChargedVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    $("#SendSms_DB").text(TotalSendSms);
    Per = (((TotalSendSms / TotalCVehicle) * 100) || 0).toFixed();
    $("#SendSms_DB_Per").text(Per + '%');
    $("#SendSms_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    $("#Success_SendSMS_DB").text(SuceessSendSms);
    Per = (((SuceessSendSms / TotalSendSms) * 100) || 0).toFixed();
    $("#Success_SendSMS_DB_Per").text(Per + '%');
    $("#Success_SendSMS_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");


}



function intToString(value) {
    var suffixes = ["", "k", "m", "b", "t"];
    var suffixNum = Math.floor(("" + value).length / 3);
    var shortValue = parseFloat((suffixNum != 0 ? (value / Math.pow(1000, suffixNum)) : value).toPrecision(2));
    if (shortValue % 1 != 0) {
        var shortNum = shortValue.toFixed(1);
    }
    return shortValue + suffixes[suffixNum];
}
function GetGantry() {
            $(".animationload").show();
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "Dashboard/DashBoardTransactionData",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#filterModel').modal('hide');
                $('.animationload').hide();
                ResponceGantryData = data;
                console.log(ResponceGantryData);
            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
}