var ResponceData = [];
var ResponceGantryData = [];
//var StartDT, EndDT;
var Progress = true;
var stoprefresh = true;
var StartDate = '';
var EndDate = '';
$(document).ready(function () {
    function setHeight() {
        windowHeight = $(window).innerHeight() - 110;
        $('.container-fluid').css('max-height', windowHeight);
    };
    setHeight();

    $(window).resize(function () {
        setHeight();
    });
  
    Progress = true;
    $("#totalFIke").val(0);
    $("#totalRIke").val(0);
    $("#totalFANPR").val(0);
    $("#totalRANPR").val(0);
    GetMSMQData();
    GetMSMQChartData();
    BindDateTime(5);
    myVar = setInterval("GetMSMQData()", 20000);
    myVar = setInterval("GetMSMQChartData()", 20000);
});
// get Data From MSMQ
function GetMSMQData() {
    $.ajax({
        type: 'GET',
        url: "Dashboard/GetMSMQDashBoardCounter",
        cache: false,
        dataType: "json",
        success: function (data) {
            ResponceData = data.Data;
            var ChargedVehcileCount = (ResponceData.RegisterCharged.ChargedClass.MoterCycleCount + ResponceData.RegisterCharged.ChargedClass.SmallCount + ResponceData.RegisterCharged.ChargedClass.MediumCount + ResponceData.RegisterCharged.ChargedClass.BigCount);

            var FrontIKECount = (ResponceData.Register.FrontIKE.MoterCycleCount + ResponceData.Register.FrontIKE.SmallCount + ResponceData.Register.FrontIKE.MediumCount + ResponceData.Register.FrontIKE.BigCount);
            var RearIKECount = (ResponceData.Register.RearIKE.MoterCycleCount + ResponceData.Register.RearIKE.SmallCount + ResponceData.Register.RearIKE.MediumCount + ResponceData.Register.RearIKE.BigCount);

            var FrontRegisterANPRCount = (ResponceData.Register.FrontANPR.MoterCycleCount + ResponceData.Register.FrontANPR.SmallCount + ResponceData.Register.FrontANPR.MediumCount + ResponceData.Register.FrontANPR.BigCount);
            var RearRegisterANPRCount = (ResponceData.Register.RearANPR.MoterCycleCount + ResponceData.Register.RearANPR.SmallCount + ResponceData.Register.RearANPR.MediumCount + ResponceData.Register.RearANPR.BigCount);


             var FrontUnRegisterANPRCount = (ResponceData.UnRegister.FrontANPR.MoterCycleCount + ResponceData.UnRegister.FrontANPR.SmallCount + ResponceData.UnRegister.FrontANPR.MediumCount + ResponceData.UnRegister.FrontANPR.BigCount);
            var RearUnRegisterANPRCount = (ResponceData.UnRegister.RearANPR.MoterCycleCount + ResponceData.UnRegister.RearANPR.SmallCount + ResponceData.UnRegister.RearANPR.MediumCount + ResponceData.UnRegister.RearANPR.BigCount);

            var FrontUnIdentifiedANPRCount = (ResponceData.UnIdentified.FrontANPR.MoterCycleCount + ResponceData.UnIdentified.FrontANPR.SmallCount + ResponceData.UnIdentified.FrontANPR.MediumCount + ResponceData.UnIdentified.FrontANPR.BigCount);
           var RearUnIdentifiedANPRCount = (ResponceData.UnIdentified.RearANPR.MoterCycleCount + ResponceData.UnIdentified.RearANPR.SmallCount + ResponceData.UnIdentified.RearANPR.MediumCount + ResponceData.UnIdentified.RearANPR.BigCount);
           
            //var FrontANPRCount = (ResponceData.FrontANPR.MoterCycleCount + ResponceData.FrontANPR.SmallCount + ResponceData.FrontANPR.MediumCount + ResponceData.FrontANPR.BigCount);
            //var RearANPRCount = (ResponceData.RearANPR.MoterCycleCount + ResponceData.RearANPR.SmallCount + ResponceData.RearANPR.MediumCount + ResponceData.RearANPR.BigCount);
           
           var FrontANPRCount = (FrontRegisterANPRCount + FrontUnRegisterANPRCount + FrontUnIdentifiedANPRCount );
           var RearANPRCount = (RearRegisterANPRCount + RearUnRegisterANPRCount + RearUnIdentifiedANPRCount);

            var TotalPassed = Math.max(FrontANPRCount, RearANPRCount, FrontIKECount, RearIKECount);
            var TotalRegisterPassed = Math.max(FrontIKECount, RearIKECount);
            var VehicleUnidentitfiedCount = (FrontUnIdentifiedANPRCount + RearUnIdentifiedANPRCount);
            var VehicleChargedCount = (ResponceData.RegisterCharged.ChargedClass.MoterCycleCount + ResponceData.RegisterCharged.ChargedClass.SmallCount + ResponceData.RegisterCharged.ChargedClass.MediumCount + ResponceData.RegisterCharged.ChargedClass.BigCount);
            var TotalRegisterVehicle = ResponceData.TotalRegisterVehicleCount;

            $("#totalFIke").text(FrontIKECount);
            $("#totalRIke").text(RearIKECount);
            $("#totalFANPR").text(FrontANPRCount);
            $("#totalRANPR").text(RearANPRCount);

            var TotalVehiclePass = TotalPassed;
            var TotalRVehiclePass = TotalRegisterPassed;
            var TOTAL_UNREGISTERED = TotalPassed - TotalRegisterPassed;
            var TotalCVehicle = VehicleChargedCount;
            var TotalSendSms = VehicleChargedCount;

            var Per = 0;
            $("#TotalVehicle_DB").text(TotalVehiclePass);
            Per = (((TotalVehiclePass / TotalVehiclePass) * 100) || 0).toFixed();
            $("#TotalVehicle_DB_PER").text(Per + '%');
            $("#TotalVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
            //------------
            $("#RegisteredVehicle_DB").text(TotalRVehiclePass);
            Per = (((TotalRVehiclePass / TotalVehiclePass) * 100) || 0).toFixed();
            $("#RegisteredVehicle_DB_PER").text(Per + '%');
            $("#RegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
            //------------
            $("#UnRegisteredVehicle_DB").text(TOTAL_UNREGISTERED);
            Per = (((TOTAL_UNREGISTERED / TotalVehiclePass) * 100) || 0).toFixed();
            $("#UnRegisteredVehicle_DB_PER").text(Per + '%');
            $("#UnRegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

            //-----------------
            $("#ChargedVehicle_DB").text(TotalCVehicle);
            if (TotalRVehiclePass != 0) {
                Per = (((TotalCVehicle / TotalRVehiclePass) * 100) || 0).toFixed();
                $("#ChargedVehicle_DB_PER").text(Per + '%');
            } else {
                Per = 0;
            }
            $("#ChargedVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

            //-------------
            $("#SendSms_DB").text(TotalSendSms);
            if (TotalCVehicle != 0) {
                Per = (((TotalSendSms / TotalCVehicle) * 100) || 0).toFixed();
                $("#SendSms_DB_Per").text(Per + '%');
            } else {
                Per = 0;
            }
            $("#SendSms_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
            CallBundleJS(ResponceData.Register, ResponceData.UnRegister, ResponceData.UnIdentified);
            if (stoprefresh)
                bindChart(ResponceData, TotalRVehiclePass, TOTAL_UNREGISTERED);
        },
        error: function (ex) {
        }
    });
}

// Prepare First chart
function bindChart(data, TotalRVehiclePass, TOTAL_UNREGISTERED) {
    var totalvehicle = TotalRVehiclePass + TOTAL_UNREGISTERED;
    var totalVisitors = totalvehicle;
    var visitorsData = {
        "New vs Returning Visitors": [{
            click: visitorsChartDrilldownHandler,
            cursor: "pointer",
            explodeOnClick: false,
            innerRadius: "75%",
            legendMarkerType: "square",
            name: "New vs Returning Visitors",
            radius: "100%",
            showInLegend: true,
            startAngle: 90,
            type: "doughnut",
            dataPoints: [
                { y: TotalRVehiclePass, name: "REGISTERED VEHICLES", color: "#E7823A" },
                { y: TOTAL_UNREGISTERED, name: "UNREGISTERED VEHICLES", color: "#546BC1" },
            ]
        }],
        "REGISTERED VEHICLES": [{
            type: "column",
            name: "FRONT IKE",
            showInLegend: "true",
            dataPoints: [
                { label: "TWO-WHEELED", y: data.Register.FrontIKE.MoterCycleCount, color: "#cc6600" },
                { label: "SMALL", y: data.Register.FrontIKE.SmallCount, color: "#cc6600" },
                { label: "MEDIUM", y: data.Register.FrontIKE.MediumCount, color: "#cc6600" },
                { label: "LARGE", y: data.Register.FrontIKE.BigCount, color: "#cc6600" },
            ]
        },
{
    type: "column",
    name: "REAR IKE",
    showInLegend: "true",
    dataPoints: [
                 { label: "TWO-WHEELED", y: data.Register.RearIKE.MoterCycleCount },
                { label: "SMALL", y: data.Register.RearIKE.SmallCount },
                { label: "MEDIUM", y: data.Register.RearIKE.MediumCount },
                { label: "LARGE", y: data.Register.RearIKE.BigCount },
    ]
},
{
    type: "column",
    name: "ANPR FRONT",
    showInLegend: "true",
    dataPoints: [
               { label: "TWO-WHEELED", y: data.Register.FrontANPR.MoterCycleCount },
                { label: "SMALL", y: data.Register.FrontANPR.SmallCount },
                { label: "MEDIUM", y: data.Register.FrontANPR.MediumCount },
                { label: "LARGE", y: data.Register.FrontANPR.BigCount },
    ]
},
{
    type: "column",
    name: "ANPR REAR",
    showInLegend: "true",
    dataPoints: [
                { label: "TWO-WHEELED", y: data.Register.RearANPR.MoterCycleCount },
                { label: "SMALL", y: data.Register.RearANPR.SmallCount },
                { label: "MEDIUM", y: data.Register.RearANPR.MediumCount },
                { label: "LARGE", y: data.Register.RearANPR.BigCount },
    ]
}
        ],
        "UNREGISTERED VEHICLES": [{
            color: "#546BC1",
            name: "Returning Visitors",
            type: "doughnut",
            xValueFormatString: "MMM YYYY",
            dataPoints: [
                { label: "REAR TWO-WHEELED", y: data.UnRegister.RearANPR.MoterCycleCount, color: "#E7823A" },
                { label: "REAR SMALL", y: data.UnRegister.RearANPR.SmallCount, color: "#546BC1" },
                { label: "MEDIUM", y: data.UnRegister.RearANPR.MediumCount, color: "#E7823A" },
                { label: "REAR LARGE", y: data.UnRegister.RearANPR.BigCount, color: "#546BC1" },
                { label: "FRONT TWO-WHEELED", y: data.UnRegister.FrontANPR.MoterCycleCount, color: "#72777a" },
                { label: "FRONT SMALL", y: data.UnRegister.FrontANPR.SmallCount, color: "#ff6969" },
                { label: "FRONT MEDIUM", y: data.UnRegister.FrontANPR.MediumCount, color: "#ff6969" },
                { label: "FRONT LARGE", y: data.UnRegister.FrontANPR.BigCount, color: "#ff6969" },

                //{ label: "TOTAL REAR UNIDENTIFIED VRN", y: data.TOTAL_REAR_UNIDENTIFIEDVRN, color: "#E7823A" },
                //{ label: "TOTAL REAR DETECTED VRN", y: data.TOTAL_REAR_DETECTEDVRN, color: "#546BC1" },
                //{ label: "TOTAL FRONT UNIDENTIFIED VRN", y: data.TOTAL_FRONT_UNIDENTIFIEDVRN, color: "#72777a" },
                //{ label: "TOTAL FRONT DETECTED VRN", y: data.TOTAL_FRONT_DETECTEDVRN, color: "#ff6969" },
            ]
        }],
        "TOTAL IKE": [{
            click: visitorsChartDrilldownHandler,
            cursor: "pointer",
            explodeOnClick: false,
            innerRadius: "75%",
            legendMarkerType: "square",
            name: "New vs Returning Visitors",
            radius: "100%",
            showInLegend: true,
            startAngle: 90,
            color: "#50cdc8",
            name: "Returning Visitors",
            type: "doughnut",
            xValueFormatString: "MMM YYYY",
            dataPoints: [
                { label: "TOTAL FRONT IKE", y: data.TOTAL_FRONTIKE, name: "FRONT IKE", color: "#222222" },
                { label: "TOTAL REAR IKE", y: data.TOTAL_REARIKE, name: "REAR IKE", },
                //{ label: "TOTAL TWO-WHEELED -IKE", y: data.TOTAL_TWOWHEELEDIKE },
                //{ label: "TOTAL SMALL -IKE", y: data.TOTAL_SMALLIKE },
                //{ label: "TOTAL MEDIUM -IKE", y: data.TOTAL_MEDIUMIKE },
                //{ label: "TOTAL LARGE -IKE", y: data.TOTAL_LARGEIKE },
                //{ label: "TWO-WHEELED -IKE-FRONT", y: data.TOTAL_TWOWHEELEDIKEFRONT },
                //{ label: "TOTAL SMALL -IKE-FRONT", y: data.TOTAL_SMALLIKEFRONT },
                //{ label: "TOTAL MEDIUM -IKE-FRONT", y: data.TOTAL_MEDIUMIKEFRONT },
                //{ label: "TOTAL LARGE -IKE-FRONT", y: data.TOTAL_LARGEIKEFRONT },
                //{ label: "TOTAL TWO-WHEELED -IKE-REAR", y: data.TOTAL_TWOWHEELEDIKEREAR },
                //{ label: "TOTAL SMALL -IKE-REAR", y: data.TOTAL_SMALLIKEREAR },
                //{ label: "TOTAL MEDIUM -IKE-REAR", y: data.TOTAL_MEDIUMIKEREAR },
                //{ label: "TOTAL LARGE -IKE-REAR", y: data.TOTAL_LARGEIKEREAR },
            ]
        }],
        "TOTAL ANPR": [{
            color: "#ff6969",
            name: "Returning Visitors",
            type: "column",
            xValueFormatString: "MMM YYYY",
            dataPoints: [
                { label: "TOTAL TWO-WHEELED -ANPR", y: data.TOTAL_TWOWHEELEDANPR },
                { label: "TOTAL SMALL -ANPR", y: data.TOTAL_SMALL_ANPR },
                { label: "TOTAL MEDIUM -ANPR", y: data.TOTAL_MEDIUM_ANPR },
                { label: "TOTAL LARGE -ANPR", y: data.TOTAL_LARGE_ANPR },
                { label: "TOTAL TWO-WHEELED -ANPR-FRONT", y: data.TOTAL_TWOWHEELED_ANPRFRONT },
                { label: "TOTAL SMALL -ANPR-FRONT", y: data.TOTAL_SMALLANPR_FRONT },
                { label: "TOTAL MEDIUM -ANPR-FRONT", y: data.TOTAL_MEDIUMANPR_FRONT },
                { label: "TOTAL LARGE -ANPR-FRONT", y: data.TOTAL_LARGEANPR_FRONT },
                { label: "TOTAL TWO-WHEELED -ANPR-REAR", y: data.TOTAL_TWOWHEELED_ANPRREAR },
                { label: "TOTAL SMALL -ANPR-REAR", y: data.TOTAL_SMALL_ANPRREAR },
                { label: "TOTAL MEDIUM -ANPR-REAR", y: data.TOTAL_MEDIUM_ANPRREAR },
                { label: "TOTAL LARGE -ANPR-REAR", y: data.TOTAL_LARGE_ANPRREAR },
            ]
        }],
        "FRONT IKE": [{
            color: "#ff6969",
            name: "Returning Visitors",
            type: "column",
            xValueFormatString: "MMM YYYY",
            dataPoints: [
              { label: "TWO-WHEELED -IKE-FRONT", y: data.TOTAL_TWOWHEELEDIKEFRONT },
              { label: "TOTAL SMALL -IKE-FRONT", y: data.TOTAL_SMALLIKEFRONT },
              { label: "TOTAL MEDIUM -IKE-FRONT", y: data.TOTAL_MEDIUMIKEFRONT },
              { label: "TOTAL LARGE -IKE-FRONT", y: data.TOTAL_LARGEIKEFRONT },
            ]
        }],
        "REAR IKE": [{
            color: "#ff6969",
            name: "Returning Visitors",
            type: "column",
            xValueFormatString: "MMM YYYY",
            dataPoints: [
               { label: "TOTAL TWO-WHEELED -IKE-REAR", y: data.TOTAL_TWOWHEELEDIKEREAR },
                { label: "TOTAL SMALL -IKE-REAR", y: data.TOTAL_SMALLIKEREAR },
                { label: "TOTAL MEDIUM -IKE-REAR", y: data.TOTAL_MEDIUMIKEREAR },
                { label: "TOTAL LARGE -IKE-REAR", y: data.TOTAL_LARGEIKEREAR },
            ]
        }]
    };

    var newVSReturningVisitorsOptions = {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Vehicle Details"
        },
        subtitles: [{
            text: "Total Vehicle :" + totalvehicle,
            backgroundColor: "#2eacd1",
            fontSize: 16,
            fontColor: "white",
            padding: 5
        }],
        legend: {
            fontFamily: "calibri",
            fontSize: 14,
            itemTextFormatter: function (e) {
                return e.dataPoint.name;//+ ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
            }
        },
        data: []
    };

    var visitorsDrilldownedChartOptions = {
        animationEnabled: true,
        theme: "light2",
        axisX: {
            labelFontColor: "#717171",
            lineColor: "#a2a2a2",
            tickColor: "#a2a2a2"
        },
        axisY: {
            gridThickness: 0,
            includeZero: false,
            labelFontColor: "#717171",
            lineColor: "#a2a2a2",
            tickColor: "#a2a2a2",
            lineThickness: 1
        },
        data: []
    };

    newVSReturningVisitorsOptions.data = visitorsData["New vs Returning Visitors"];
    $("#chartContainer").CanvasJSChart(newVSReturningVisitorsOptions);

    function visitorsChartDrilldownHandler(e) {
        e.chart.options = visitorsDrilldownedChartOptions;
        e.chart.options.data = visitorsData[e.dataPoint.name];
        e.chart.options.title = { text: e.dataPoint.name }
        e.chart.render();
        $("#backButton").toggleClass("invisible");
        stoprefresh = false;
    }

    $("#backButton").click(function () {
        if (stoprefresh == false) {
            $(this).toggleClass("invisible");
            newVSReturningVisitorsOptions.data = visitorsData["New vs Returning Visitors"];
            $("#chartContainer").CanvasJSChart(newVSReturningVisitorsOptions);
            //$("#backButton").hide();
            //$("#backButton").toggleClass("invisible");
            stoprefresh = true;
        }
    });
}

///---------------
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
    StartDate = mm + '/' + dd + '/' + yy + " " + time1;
    EndDate = mm + '/' + dd + '/' + yy + " " + time2;
    GetStackChartData(StartDate, EndDate);

}

function GetfirstLoadData(StartDate, EndDate, loader) {
    if (Progress) {
        var StartDate = DateFormatTime(StartDate);
        var EndDate = DateFormatTime(EndDate);
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

                setTimeout(function () { reloadData(); }, 20000);
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
    StartDate = mm + '/' + dd + '/' + yy + " " + time1;
    EndDate = mm + '/' + dd + '/' + yy + " " + time2;
    GetfirstLoadData(StartDate, EndDate, false);

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

function BindTableRight(data) {
    var TotalVehiclePass = data[0].TOTALVEHICLE;
    var TotalRVehiclePass = data[1].TOTALVEHICLE;
    var TOTAL_UNREGISTERED = data[2].TOTALVEHICLE;
    var TotalCVehicle = data[37].TOTALVEHICLE;
    var TotalSendSms = data[44].TOTALVEHICLE;
    var SuceessSendSms = data[45].TOTALVEHICLE;
    var TotalManualChargeVehicle = data[39].TOTALVEHICLE;
    var TotalManualSendSms = data[52].TOTALVEHICLE;
    var ManualSuceessSendSms = data[53].TOTALVEHICLE;

    var Per = 0;
    $("#TotalVehicle_DB").text(TotalVehiclePass);
    Per = (((TotalVehiclePass / TotalVehiclePass) * 100) || 0).toFixed();
    $("#TotalVehicle_DB_PER").text(Per + '%');
    $("#TotalVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
    //------------
    $("#RegisteredVehicle_DB").text(TotalRVehiclePass);
    Per = (((TotalRVehiclePass / TotalVehiclePass) * 100) || 0).toFixed();
    $("#RegisteredVehicle_DB_PER").text(Per + '%');
    $("#RegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
    //------------
    $("#UnRegisteredVehicle_DB").text(TOTAL_UNREGISTERED);
    Per = (((TOTAL_UNREGISTERED / TotalVehiclePass) * 100) || 0).toFixed();
    $("#UnRegisteredVehicle_DB_PER").text(Per + '%');
    $("#UnRegisteredVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //-----------------
    $("#ChargedVehicle_DB").text(TotalCVehicle);
    if (TotalRVehiclePass != 0) {
        Per = (((TotalCVehicle / TotalRVehiclePass) * 100) || 0).toFixed();
        $("#ChargedVehicle_DB_PER").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#ChargedVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //-------------
    $("#SendSms_DB").text(TotalSendSms);
    if (TotalCVehicle != 0) {
        Per = (((TotalSendSms / TotalCVehicle) * 100) || 0).toFixed();
        $("#SendSms_DB_Per").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#SendSms_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //----------------
    $("#Success_SendSMS_DB").text(SuceessSendSms);
    if (TotalSendSms != 0) {
        Per = (((SuceessSendSms / TotalSendSms) * 100) || 0).toFixed();
        $("#Success_SendSMS_DB_Per").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#Success_SendSMS_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //------------
    $("#ManualChargedVehicle_DB").text(TotalManualChargeVehicle);
    if (TotalRVehiclePass != 0) {
        Per = (((TotalManualChargeVehicle / TotalRVehiclePass) * 100) || 0).toFixed();
        $("#ManualChargedVehicle_DB_PER").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#ManualChargedVehicle_DB_PER").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
    //------------
    $("#ManualSendSms_DB").text(TotalManualSendSms);
    if (TotalManualChargeVehicle != 0) {
        Per = (((TotalManualSendSms / TotalManualChargeVehicle) * 100) || 0).toFixed();
        $("#ManualSendSms_DB_Per").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#ManualSendSms_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");

    //---------------
    $("#ManualSuccess_SendSMS_DB").text(ManualSuceessSendSms);
    if (TotalManualSendSms != 0) {
        Per = (((ManualSuceessSendSms / TotalManualSendSms) * 100) || 0).toFixed();
        $("#ManualSuccess_SendSMS_DB_Per").text(Per + '%');
    } else {
        Per = 0;
    }
    $("#ManualSuccess_SendSMS_DB_Per").next().find('.progress-bar').attr('aria-valuenow', Per).css("width", Per + "%");
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

function ResetFilter() {
    $("#filterbox").find('input:text').val('');
    //$("#filterbox").find('select').val(0);
    BindDateTimeforDate();
}
function ClearBindDateTime() {
    var cdt = new Date();
    var d = new Date(cdt.setMinutes(cdt.getMinutes() - 30));
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
    $('#StartDate').val(mm + '/' + dd + '/' + yy + " " + time1);
    $('#EndDate').val(mm + '/' + dd + '/' + yy + " " + time2);
}
function BindDateTimeforDate() {
    var cdt = new Date();
    var d = new Date(cdt.setMinutes(cdt.getMinutes() - 30));
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
    StartDate = mm + '/' + dd + '/' + yy + " " + time1;
    EndDate = mm + '/' + dd + '/' + yy + " " + time2;
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
}

function openFilterpopup() {
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
    var modal = $("#filterModel");
    var body = $(window);
    var w = modal.width();
    var h = modal.height();
    var bw = body.width();
    var bh = body.height();
    modal.css({
        "top": "106px",
        "left": ((bw - 450)) + "px",
        "right": "30px"
    })
    $('#filterModel').modal('show');
    $(".modal-backdrop.show").hide();
}

// download register and Unregistered File
function DownLoadTransactionReport() {
    var StartDate = DateFormatTime($("#StartDate").val());
    var EndDate = DateFormatTime($("#EndDate").val());
    var Inputdata = {
        StartDate: StartDate,
        EndDate: EndDate,
    }
    inProgress = true;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "/CSV/ExportCSVTranscations",
        contentType: "application/json; charset=utf-8",
        success: function (Path) {
            $('#filterModel').modal('hide');
            $('.animationload').hide();
            if (Path.toLowerCase() == "no data to export." || Path.toLowerCase() == "file exported successfully") {
                alert(Path)
                return;
            }
            if (Path.toLowerCase().search(".csv") > -1 || Path.toLowerCase().search(".pdf") > -1 || Path.toLowerCase().search(".zip") > -1) {
                var res = Path.split(";");
                for (var j = 0; j < res.length; j++) {
                    window.open("../Attachment/ExportFiles/" + res[j]);
                }
            }
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

// Prepare second chart
function GetStackChartData(StartDate, EndDate) {
    var StartDate = DateFormatTime(StartDate);
    var EndDate = DateFormatTime(EndDate);
   // $(".animationload").show();
    $('#load').show(); // Show loading animation
    $('#chartContainerstack').hide(); // Hide content until loaded
    var Inputdata = {
        StartDate: StartDate,
        EndDate: EndDate,
    }
    $.ajax({
        type: "POST",
        dataType: "json",
       // processData: true,
        async:true,
        data: JSON.stringify(Inputdata),
        url: "Dashboard/StackChartData",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#filterModel').modal('hide');
            //$('.animationload').hide();
            $('#load').hide(); // Hide loading animation
            $('#chartContainerstack').show(); // Show content
        },
        error: function (ex) {
            $('#load').hide(); // Hide loading animation
            $('#chartContainerstack').show(); // Show content
        }
    });
}

function BindStackChartData(data) {

    var binddata_Stackchart = {
        TOTAL_VEHICLE: data[0].TOTALDETAILS,
        TOTAL_REGISTERED: data[1].TOTALDETAILS,
        TOTAL_UNREGISTERED: data[2].TOTALDETAILS,
        TOTAL_UNIDENTIFIED_VRN: data[3].TOTALDETAILS,
        TOPUP_AMOUNT: data[4].TOTALDETAILS,
        CHARGED_AMOUNT: data[5].TOTALDETAILS
    }
   
    bindStackedColumnChart(binddata_Stackchart);
}

function GetMSMQChartData() {
    $.ajax({
        type: 'GET',
        url: "Dashboard/GetMSMQChartData",
        cache: false,
        dataType: "json",
        success: function (data) {
            BindStackChartData(data);
            //console.log(data);
        },
        error: function (ex) {
        }
    });
}
