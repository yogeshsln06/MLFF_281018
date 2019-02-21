var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var month = (new Date()).getMonth() + 1;
var Startyear = 2018;
var year = (new Date()).getFullYear();
var CustomerVehcileJson = [];
var MonthId = month;
var VehicleId = 0;
var YearId = year;
var mainWidth = 0;

$(document).ready(function () {
    mainWidth = $(window).innerWidth();
    $(window).resize(function () {
        if (mainWidth != $(window).innerWidth()) {
            $(".animationload").show();
            mainWidth = $(window).innerWidth();
            thId = 'tblVBRData';
            myVar = setInterval("myclick()", 100);
        }
    });
});
function bindMonth() {
    for (var i = 0; i < monthNames.length; i++) {
        $("#monthList").append
            ($('<option></option>').val(i + 1).html(monthNames[i]))
    }
    $("#monthList").val(month);
}

function bindYear() {
    for (var i = Startyear; i <= year; i++) {
        $("#yearList").append
            ($('<option></option>').val(i).html(i))
    }
    $("#yearList").val(year);
}

function bindVRN() {
    $("#vrnList").append
           ($('<option></option>').val(0).html('Select VRN'))
    $.each((CustomerVehcileJson), function (i, vehicle) {
        if (vehicle.VehRegNo != '') {
            $("#vrnList").append
            ($('<option></option>').val(vehicle.EntryId).html(vehicle.VehRegNo))
        }

    })
}

function FirstLoadVehicleBalance() {
    var month = (new Date()).getMonth() + 1;
    var year = (new Date()).getFullYear();
    var VehicleId = 0;
    $(".animationload").show();
    var Inputdata = { VehicleId: VehicleId, Month: month, Year: year }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "VehicleBalanceReportFilter",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            BindVehcileDeatils(result.TBL_CUSTOMER_VEHICLE);
            $("#tblVBRData").removeClass('my-table-bordered').addClass('table-bordered');
            var bindDate = result.TranscationDeatils;
            if (bindDate.length == 2)
                bindDate = null;
            VBRDataVariable = $('#tblVBRData').DataTable({
                data: bindDate,
                "bScrollInfinite": true,
                "bScrollCollapse": false,
                scrollY: "39.5vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
                paging: false,
                info: false,
                searching: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'CREATION_DATE' },
                    {
                        'data': 'TRANSACTION_ID'
                    },
                    {
                        'data': 'TRANSACTION_TYPE',
                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'LANE_ID' },

                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                if (oData.FRONT_IMAGE.indexOf('http') > -1) {
                                    oData.FRONT_IMAGE = oData.FRONT_IMAGE;
                                }
                                else {
                                    oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
                                }
                                $(nTd).html("<img src=" + oData.FRONT_IMAGE + " height='40' width='60' onclick='openImagePreview(this);' />");
                            }
                        }
                    },
                    {
                        'data': 'FRONT_VIDEO_URL',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_VIDEO_URL != '' && oData.FRONT_VIDEO_URL != null) {

                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openVideo(this);' style='font-size: 18px;' path=" + oData.FRONT_VIDEO_URL + "><i class='c-blue-500 ti-video-camera'></i></span>");
                            }
                        }
                    },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                if (oData.REAR_IMAGE.indexOf('http') > -1) {
                                    oData.REAR_IMAGE = oData.REAR_IMAGE;
                                }
                                else {
                                    oData.REAR_IMAGE = "\\" + oData.v;
                                }
                                $(nTd).html("<img src=" + oData.REAR_IMAGE + " height='40' width='60' onclick='openImagePreview(this);' />");
                            }
                        }
                    },
                    {
                        'data': 'REAR_VIDEO_URL',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_VIDEO_URL != '' && oData.REAR_VIDEO_URL != null) {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openVideo(this);' style='font-size: 18px;' path=" + oData.REAR_VIDEO_URL + "><i class='c-blue-500 ti-video-camera'></i></span>");
                            }
                        }
                    },
                   {
                       'data': 'AMOUNT',
                       fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                           if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                               if (oData.AMOUNT < 0) {
                                   $(nTd).html("<span class='text-right red'>(" + ((oData.AMOUNT) * (-1)).toLocaleString('id-ID', {
                                       maximumFractionDigits: 10,
                                       style: 'currency',
                                       currency: 'IDR'
                                   }) + ")</span>");
                               }
                               else {
                                   $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                                       maximumFractionDigits: 10,
                                       style: 'currency',
                                       currency: 'IDR'
                                   }) + "</span>");
                               }
                           }

                       }
                   },
                   //{
                   //    'data': 'AMOUNT',
                   //    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   //        if (oData.AMOUNT < 0) {
                   //            $(nTd).html("<span class='text-right'>(" + CurrecyFormat( (oData.AMOUNT) * (-1)) + ")</span>");
                   //        }
                   //        else {
                   //            $(nTd).html("<span class='text-right'>" + CurrecyFormat(oData.AMOUNT) + "</span>");
                   //        }

                   //    }
                   //},
                ],
                'columnDefs': [
                {
                    "targets": 6,
                    "className": "text-center",
                },
                 {
                     "targets": 7,
                     "className": "text-center",
                 },
                  {
                      "targets": 8,
                      "className": "text-center",
                  },
                {
                    "targets": 9,
                    "className": "text-center",
                },
                {
                    "targets": 10,
                    "className": 'dt-body-right',
                },
                { targets: 'no-sort', orderable: false }
                ],
                width: "100%"
            });
            thId = 'tblVBRDataTR';
            myVar = setInterval("myclick()", 500);
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function BindVehcileDeatils(VehcileDetails) {
    if (VehcileDetails.length > 0) {
        $("#VehRegNo").val(VehcileDetails[0].VEH_REG_NO);
        $("#EntryId").val(VehcileDetails[0].ENTRY_ID);
        $("#VehicleClassName").val(VehcileDetails[0].VEHICLE_CLASS_NAME);
        $("#FirstName").val(VehcileDetails[0].CUSTOMER_NAME);
        $("#ResidentId").val(VehcileDetails[0].RESIDENT_ID);
        $("#MobileNo").val(VehcileDetails[0].MOB_NUMBER);
        $("#EmailId").val(VehcileDetails[0].EMAIL_ID);
        $("#period").val($("#monthList option:selected").text() + ' - ' + $("#yearList").val());
    }
    else {
        $("#VehRegNo").val("");
        $("#EntryId").val("");
        $("#VehicleClassName").val("");
        $("#FirstName").val("");
        $("#ResidentId").val("");
        $("#MobileNo").val("");
        $("#EmailId").val("");
        $("#period").val("");

    }
}

function openFilterpopupVechile() {
    //$('#filterModel').modal('show');
    //$(".modal-backdrop.show").hide();
    $("#vrnList").val(VehicleId)
    $("#monthList").val(MonthId);
    $("#yearList").val(YearId);

    var modal = $("#filterModel");
    var body = $(window);
    var w = modal.width();
    var h = modal.height();
    var bw = body.width();
    var bh = body.height();
    modal.css({
        "top": "106px",
        "left": ((bw - 450)) + "px",
        "right": "49px"
    })
    $('#filterModel').modal('show');
    $(".modal-backdrop.show").hide();
}

function ReloadData() {
    $("#vrnList").val(VehicleId)
    $("#monthList").val(MonthId);
    $("#yearList").val(YearId);
    FilterVBRData();
}

function FilterVBRData() {
  

    VehicleId = $("#vrnList").val();
    MonthId = $("#monthList").val();
    YearId = $("#yearList").val();

    var Inputdata = { VehicleId: VehicleId, Month: MonthId, Year: YearId }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "VehicleBalanceReportFilter",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#filterModel').modal('hide');
            BindVehcileDeatils(result.TBL_CUSTOMER_VEHICLE);
            var bindDate = result.TranscationDeatils;
            VBRDataVariable.clear().draw();
            if (bindDate.length != 2) {
                VBRDataVariable.rows.add(bindDate);
            }
            VBRDataVariable.columns.adjust().draw();
            $(".animationload").hide();
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function ResetVBRFilter() {
    $("#vrnList").val(0);
    $("#monthList").val(month)
    $("#yearList").val(year);
    //FilterVBRData();
}

function myclick() {
    document.getElementById(thId).click();
    document.getElementById(thId).click();
    clearTimeout(myVar);
    $(".animationload").hide();
}

function openVideo(ctrl) {
    var VideoPath = $(ctrl).attr('path');
    var $video = $('#video video'),
        videoSrc = $('source', $video).attr('src', VideoPath);
    $video[0].load();
    $("#video").show();
    var modal = $("#VideoModal");
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
    $('#VideoModal').modal({ backdrop: 'static', keyboard: false })
    $('#VideoModal').modal('show');
}
