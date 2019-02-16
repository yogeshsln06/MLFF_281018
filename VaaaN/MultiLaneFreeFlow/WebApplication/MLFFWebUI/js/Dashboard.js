var ResponceData = [];
var StartDT, EndDT;
var Progress = true;

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
                sleep(10000);
                reloadData();
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
}