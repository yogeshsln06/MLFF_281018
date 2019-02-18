var ResponceData = [];
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
                //sleep(10000);
                //reloadData();
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
    //BindTopCount(data);
    BindTableRight(data)

}

function BindTopCount(data) {
    $("#totalFIke").text(data[4].TOTALVEHICLE);
    $("#totalRIke").text(data[5].TOTALVEHICLE);
    $("#totalFANPR").text(data[8].TOTALVEHICLE + data[11].TOTALVEHICLE);
    $("#totalRANPR").text(data[9].TOTALVEHICLE + data[12].TOTALVEHICLE);
    var TotalFrontANPR;
    var binddata = {
        TOTAL_VEHICLE: 1000,//data[1].TOTALVEHICLE
        TOTAL_REGISTERED: 499,//data[2].TOTALVEHICLE
        TOTAL_UNREGISTERED: 500,//data[3].TOTALVEHICLE

        TOTAL_IKE: 510,//data[4].TOTALVEHICLE
        TOTAL_FRONTIKE: 520,//data[5].TOTALVEHICLE
        TOTAL_REARIKE: 530,//data[6].TOTALVEHICLE
        TOTAL_ANPR: 540,//data[7].TOTALVEHICLE
        TOTAL_UNIDENTIFIEDVRN: 550,//data[8].TOTALVEHICLE
        TOTAL_FRONT_UNIDENTIFIEDVRN: 560,//data[9].TOTALVEHICLE
        TOTAL_REAR_UNIDENTIFIEDVRN: 570,//data[10].TOTALVEHICLE
        TOTAL_DETECTED_VRN: 580,//data[11].TOTALVEHICLE
        TOTAL_FRONT_DETECTEDVRN: 590,//data[12].TOTALVEHICLE
        TOTAL_REAR_DETECTEDVRN: 600,//data[13].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKE: 610,//data[14].TOTALVEHICLE
        TOTAL_SMALLIKE: 620,//data[15].TOTALVEHICLE
        TOTAL_MEDIUMIKE: 630,//data[16].TOTALVEHICLE
        TOTAL_LARGEIKE: 640,//data[17].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKEFRONT: 650000,//data[18].TOTALVEHICLE
        TOTAL_SMALLIKEFRONT: 660000,//data[19].TOTALVEHICLE
        TOTAL_MEDIUMIKEFRONT: 670000,//data[20].TOTALVEHICLE
        TOTAL_LARGEIKEFRONT: 680000,//data[21].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKEREAR: 690000,//data[22].TOTALVEHICLE
        TOTAL_SMALLIKEREAR: 700000,//data[23].TOTALVEHICLE
        TOTAL_MEDIUMIKEREAR: 710000,//data[24].TOTALVEHICLE
        TOTAL_LARGEIKEREAR: 720000,//data[25].TOTALVEHICLE

        TOTAL_TWOWHEELEDANPR: 730,//data[26].TOTALVEHICLE
        TOTAL_SMALL_ANPR: 740,//data[27].TOTALVEHICLE
        TOTAL_MEDIUM_ANPR: 750,//data[28].TOTALVEHICLE
        TOTAL_LARGE_ANPR: 760,//data[29].TOTALVEHICLE

        //TotalFrontANPR:TOTAL_TWOWHEELED_ANPRFRONT+TOTAL_SMALLANPR_FRONT+TOTAL_MEDIUMANPR_FRONT+TOTAL_LARGEANPR_FRONT,
        TOTAL_TWOWHEELED_ANPRFRONT: 770000,//data[30].TOTALVEHICLE
        TOTAL_SMALLANPR_FRONT: 780000,//data[31].TOTALVEHICLE
        TOTAL_MEDIUMANPR_FRONT: 790000,//data[32].TOTALVEHICLE
        TOTAL_LARGEANPR_FRONT: 800000,//data[33].TOTALVEHICLE
        TOTAL_TWOWHEELED_ANPRREAR: 810000,//data[34].TOTALVEHICLE
        TOTAL_SMALL_ANPRREAR: 820000,//data[35].TOTALVEHICLE
        TOTAL_MEDIUM_ANPRREAR: 830000,//data[36].TOTALVEHICLE
        TOTAL_LARGE_ANPRREAR: 840000, //data[37].TOTALVEHICLE

        TOTAL_AUTOCHARGED: 850,//data[38].TOTALVEHICLE
        TOTAL_NOTCHARGED: 860,//data[39].TOTALVEHICLE
        TOTAL_MANUALCHARGED: 870,//data[40].TOTALVEHICLE
        TOTAL_REVIEWED: 880,//data[41].TOTALVEHICLE
        TOTAL_VIOLATION: 890,//data[42].TOTALVEHICLE
        TOTAL_UNIDENTIFIED: 900,//data[43].TOTALVEHICLE
        TOTAL_MERGED: 910,//data[44].TOTALVEHICLE
        TOTAL_SMSSEND: 920,//data[45].TOTALVEHICLE
        TOTAL_SUCCESSFULSEND: 930,//data[46].TOTALVEHICLE
        TOTAL_AVERAGESMS_TIMEINSECONDS: 940, //data[47].TOTALVEHICLE

        TOTAL_REGISTERED_VEHICLE: 2000000, //data[48].TOTALVEHICLE
        TOTAL_PROCESSED_VEHICLE: 180000, //data[49].TOTALVEHICLE
        TOTAL_CHARGEABLE_VEHICLE: 130000, //data[50].TOTALVEHICLE
        TOTAL_NON_CHARGEABLE_VEHICLE: 990000, //data[51].TOTALVEHICLE
        TOTAL_BLACK_LISTED_VEHICLE: 0, //data[52].TOTALVEHICLE
    }
    if (stoprefresh)
        bindChart(binddata);
}

function BindTableRight(data) {
    var TotalVehiclePass = data[1].TOTALVEHICLE;
    var TotalRVehiclePass = data[2].TOTALVEHICLE;
    var TotalCVehicle = data[38].TOTALVEHICLE;
    var Per = 0;
    $("#RegisteredVehicle_DB").text(TotalRVehiclePass);
    Per = ((TotalRVehiclePass / TotalVehiclePass) * 100) || 0;
    $("#RegisteredVehicle_DB_PER").text(Per + '%');
    $("#RegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    $("#ChargedVehicle_DB").text(TotalCVehicle);
    Per = ((TotalCVehicle / TotalRVehiclePass) * 100) || 0;
    $("#ChargedVehicle_DB_PER").text(Per + '%');
    $("#ChargedVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //$("#totalFANPR").text(data[8].TOTALVEHICLE + data[11].TOTALVEHICLE);
    //$("#totalRANPR").text(data[9].TOTALVEHICLE + data[12].TOTALVEHICLE);
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