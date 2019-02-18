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
    BindTopCount(data);

    
}

function BindTopCount(data)
{
    $("#totalFIke").text(data[4].TOTALVEHICLE);
    $("#totalRIke").text(data[5].TOTALVEHICLE);
    $("#totalFANPR").text(data[8].TOTALVEHICLE + data[11].TOTALVEHICLE);
    $("#totalRANPR").text(data[9].TOTALVEHICLE + data[12].TOTALVEHICLE);
    var TotalFrontANPR;
    var binddata = {
        TOTAL_VEHICLE: 1000,//data[1].TOTALVEHICLE
        TOTAL_REGISTERED:499,//data[2].TOTALVEHICLE
        TOTAL_UNREGISTERED: 500,//data[3].TOTALVEHICLE

        TOTAL_IKE: 510,//data[4].TOTALVEHICLE
        TOTAL_FRONTIKE: 520,//data[5].TOTALVEHICLE
        TOTAL_REARIKE: 530,//data[6].TOTALVEHICLE
        TOTAL_ANPR: 540,//data[7].TOTALVEHICLE
        TOTAL_UNIDENTIFIEDVRN:550 ,//data[8].TOTALVEHICLE
        TOTAL_FRONT_UNIDENTIFIEDVRN:560,//data[9].TOTALVEHICLE
        TOTAL_REAR_UNIDENTIFIEDVRN:570,//data[10].TOTALVEHICLE
        TOTAL_DETECTED_VRN:580,//data[11].TOTALVEHICLE
        TOTAL_FRONT_DETECTEDVRN: 590,//data[12].TOTALVEHICLE
        TOTAL_REAR_DETECTEDVRN: 600,//data[13].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKE: 610,//data[14].TOTALVEHICLE
        TOTAL_SMALLIKE: 620,//data[15].TOTALVEHICLE
        TOTAL_MEDIUMIKE: 630,//data[16].TOTALVEHICLE
        TOTAL_LARGEIKE: 640,//data[17].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKEFRONT: 650,//data[18].TOTALVEHICLE
        TOTAL_SMALLIKEFRONT: 660,//data[19].TOTALVEHICLE
        TOTAL_MEDIUMIKEFRONT: 670,//data[20].TOTALVEHICLE
        TOTAL_LARGEIKEFRONT: 680,//data[21].TOTALVEHICLE
        TOTAL_TWOWHEELEDIKEREAR: 690,//data[22].TOTALVEHICLE
        TOTAL_SMALLIKEREAR: 700,//data[23].TOTALVEHICLE
        TOTAL_MEDIUMIKEREAR: 710,//data[24].TOTALVEHICLE
        TOTAL_LARGEIKEREAR: 720,//data[25].TOTALVEHICLE

        TOTAL_TWOWHEELEDANPR: 730,//data[26].TOTALVEHICLE
        TOTAL_SMALL_ANPR: 740,//data[27].TOTALVEHICLE
        TOTAL_MEDIUM_ANPR: 750,//data[28].TOTALVEHICLE
        TOTAL_LARGE_ANPR: 760,//data[29].TOTALVEHICLE
       
        //TotalFrontANPR:TOTAL_TWOWHEELED_ANPRFRONT+TOTAL_SMALLANPR_FRONT+TOTAL_MEDIUMANPR_FRONT+TOTAL_LARGEANPR_FRONT,
        TOTAL_TWOWHEELED_ANPRFRONT: 770,//data[30].TOTALVEHICLE
        TOTAL_SMALLANPR_FRONT: 780,//data[31].TOTALVEHICLE
        TOTAL_MEDIUMANPR_FRONT: 790,//data[32].TOTALVEHICLE
        TOTAL_LARGEANPR_FRONT: 800,//data[33].TOTALVEHICLE
        TOTAL_TWOWHEELED_ANPRREAR: 810,//data[34].TOTALVEHICLE
        TOTAL_SMALL_ANPRREAR: 820,//data[35].TOTALVEHICLE
        TOTAL_MEDIUM_ANPRREAR: 830,//data[36].TOTALVEHICLE
        TOTAL_LARGE_ANPRREAR: 840, //data[37].TOTALVEHICLE

        TOTAL_AUTOCHARGED: 850,//data[38].TOTALVEHICLE
        TOTAL_NOTCHARGED: 860,//data[39].TOTALVEHICLE
        TOTAL_MANUALCHARGED: 870,//data[40].TOTALVEHICLE
        TOTAL_REVIEWED: 880,//data[41].TOTALVEHICLE
        TOTAL_VIOLATION: 890,//data[42].TOTALVEHICLE
        TOTAL_UNIDENTIFIED: 900,//data[43].TOTALVEHICLE
        TOTAL_MERGED: 910,//data[44].TOTALVEHICLE
        TOTAL_SMSSEND: 920,//data[45].TOTALVEHICLE
        TOTAL_SUCCESSFULSEND: 930,//data[46].TOTALVEHICLE
        TOTAL_AVERAGESMS_TIMEINSECONDS: 940 //data[47].TOTALVEHICLE
    }
    if (stoprefresh)
        bindChart(binddata);
}