var Transactioncol;
var TransactionId;
var pageload = 1;
var pagesize = 10;
var NoMoredata = false;
var inProgress = false;
var searchEnable = false;

function ResetUnreviewedFilter() {
    searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    BindDateTime();
    reloadUnreviewedData();
}

function FilteUnreviewedData() {

    var StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        StartDate = DateFormatTime(StartDate);
    }

    var EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate = DateFormatTime(EndDate);
    }
    var Inputdata = {
        StartDate: StartDate,
        EndDate: EndDate,
        GantryId: $("#ddlGantry").val(),
        TransactionCategoryId: $("#ddlTransactionCategory").val()
    }

    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnreviewedFilter",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            searchEnable = true;
            inProgress = true;
            $(".animationload").hide();
            datatableVariable.clear().draw();
            datatableVariable.rows.add(data);
            datatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function BindUnreviewedFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnreviewedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblUnreviewedData").removeClass('my-table-bordered').addClass('table-bordered');
            NoMoredata = data.length < pagesize
            pageload++;
            datatableVariable = $('#tblUnreviewedData').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: "38.5vh",
                scrollX: true,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    {
                        'data': 'TRANSACTION_ID',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a href='javascript:void(0);' onclick='GetAssociatedTranscation(this," + oData.TRANSACTION_ID + ")'>" + oData.TRANSACTION_ID + "</a>");
                        }
                    },
                    {
                        'data': 'TRANSACTION_DATETIME',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                            }
                        }

                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'CTP_VRN' },
                    { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                    { 'data': 'FRONT_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                        'data': 'VEHICLESPEED',
                    }
                ],
                'columnDefs': [
                {
                    "targets": 8,
                    "className": "text-center",
                },
                 {
                     "targets": 9,
                     "className": "text-center",
                 },
                  {
                      "targets": 12,
                      "className": "text-center",
                  },
                {
                    "targets": 13,
                    "className": "text-center",
                }],
                width: "100%"
            });
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= ($("#tblUnreviewedData").height())) && !NoMoredata && !inProgress) {
                    AppendUnreviewedData();
                }
            });
            thId = 'tblUnreviewedDataTR';
            myVar = setInterval("myclick()", 500);
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadUnreviewedData() {
    if (searchEnable) {
        FilteUnreviewedData();
    }
    else {
        pageload = 1;
        $(".animationload").show();
        inProgress = true;
        var Inputdata = { pageindex: pageload, pagesize: pagesize }
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "UnreviewedListScroll",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                pageload++;
                NoMoredata = data.length < pagesize;
                inProgress = false;
                datatableVariable.clear().draw();
                datatableVariable.rows.add(data); // Add new data
                datatableVariable.columns.adjust().draw();

            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
    }

}

function AppendUnreviewedData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnreviewedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            pageload++;
            NoMoredata = data.length < pagesize;
            datatableVariable.rows.add(data);
            datatableVariable.columns.adjust().draw();
            inProgress = false;

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function GetAssociatedTranscation(ctrl, Id) {
    Transactioncol = ctrl;
    TransactionId = Id;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "AssociatedTransaction?TransactionId=" + TransactionId,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialassociated').html(result);
            openpopup();
            $("#exampleModalLabel").text("Reviewing Transaction " + Id + "");

        },
        error: function (x, e) {
            $(".animationload").hide();
            closePopup();
        }

    });
}

function FilterAssociatedData() {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "GetAssociated?Seconds=" + $("#filterSec").val(),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $(".animationload").hide();
            if (data != "No Record Found") {
                $("#tblAssociatedData").removeClass('my-table-bordered').addClass('table-bordered');
                AssociateddatatableVariable.clear().draw();
                AssociateddatatableVariable.rows.add(data); // Add new data
                AssociateddatatableVariable.columns.adjust().draw();
            }
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function BindAssociatedData(Seconds) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "GetAssociated?Seconds=" + Seconds,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $(".animationload").hide();
            $("#filterSec").val(30);
            if (data != "No Record Found") {
                $("#tblAssociatedData").removeClass('my-table-bordered').addClass('table-bordered');
                AssociateddatatableVariable = $('#tblAssociatedData').DataTable({
                    data: data,
                    "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                    "bScrollInfinite": true,
                    "bScrollCollapse": true,
                    scrollY: 230,
                    scroller: {
                        loadingIndicator: true
                    },
                    processing: true,
                    scrollCollapse: true,
                    stateSave: true,
                    autoWidth: false,
                    paging: false,
                    info: false,
                    columns: [
                        {
                            'data': 'TRANSACTION_ID',
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='checkbox' class='checkBox' value=" + oData.TRANSACTION_ID + " />");
                            }
                        },
                        { 'data': 'TRANSACTION_ID' },
                        {
                            'data': 'TRANSACTION_DATETIME',
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                    oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                    $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                                }
                            }

                        },
                        { 'data': 'PLAZA_NAME' },
                        { 'data': 'CTP_VRN' },
                        { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                        { 'data': 'FRONT_VRN' },
                        { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                        {
                            'data': 'FRONT_IMAGE',
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                    oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                        { 'data': 'REAR_VRN' },
                        { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                        {
                            'data': 'REAR_IMAGE',
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                    oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                            'data': 'VEHICLESPEED',
                        }
                    ],
                    'columnDefs': [
                                    {
                                        "targets": 8,
                                        "className": "text-center",
                                    },
                                     {
                                         "targets": 9,
                                         "className": "text-center",
                                     },
                                      {
                                          "targets": 12,
                                          "className": "text-center",
                                      },
                                    {
                                        "targets": 13,
                                        "className": "text-center",
                                    }],
                    width: "100%"
                });
                $('.dataTables_filter input').attr("placeholder", "Search this list…");
                thId = 'tblAssociatedDataTR';
                myVar = setInterval("myclick()", 500);
                inProgress = false;
            }
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function openpopup() {
    $("#warning").hide();
    $('#AssociatedModal').modal({ backdrop: 'static', keyboard: false })
    $('#AssociatedModal').modal('show');
    $("#txtVRN").val('');
    $("#ddlAuditedVehicleClass").val(0)
}

function SaveUnidentified() {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "SaveUnidentified?TransactionId=" + TransactionId,
        async: true,
        dataType: "JSON",
        contentType: "application/json; charset=utf-8",
        success: function (resultData) {
            $(".animationload").hide();
            var meassage = '';
            for (var i = 0; i < resultData.length; i++) {
                if (resultData[i].ErrorMessage == "success") {
                    closePopup();
                    $(Transactioncol).parent().parent().remove();
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
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function Complete() {
    if ($("#txtVRN").val() == '') {
        alert('Please Enter Audited VRN');
        return false;
    }
    if ($("#ddlAuditedVehicleClass").val() < 1) {
        alert('Please Select Audited Vehicle Class');
        return false;
    }
    var selectedIDs = new Array();
    $('input:checkbox.checkBox').each(function () {
        if ($(this).prop('checked')) {
            selectedIDs.push($(this).val());
        }
    });

    if (selectedIDs.length > 2) {
        alert('You cannot join more than two trancation.');
        return false;
    }

    if (confirm('Are you sure you want to reviewed ?')) {
        var InputData = {
            AssociatedTransactionIds: selectedIDs,
            TransactionId: TransactionId,
            VehRegNo: $('#txtVRN').val(),
            vehicleClassID: $('#ddlAuditedVehicleClass').val(),
            Seconds: $("#filterSec").val(),
        }
        $('#loader').show('fadeOut');
        $.ajax({
            type: "POST",
            url: "CompleteReviewed",
            dataType: "JSON",
            async: true,
            data: JSON.stringify(InputData),
            contentType: "application/json; charset=utf-8",
            success: function (resultData) {
                $(".animationload").hide();
                var Msg = '';
                var close = false;
                for (var i = 0; i < resultData.length; i++) {
                    if (resultData[i].ErrorMessage.toLowerCase().indexOf('success') > -1) {
                        close = true
                    }
                    else {
                        Msg = Msg + "\xb7 " + resultData[i].ErrorMessage + " \n";
                    }

                }
                if (close) {
                    alert("Reviewed successfully !")
                    closePopup();
                    $(Transactioncol).parent().parent().remove();
                }
                else {
                    alert(Msg);
                }
            },
            error: function (x, e) {
                $(".animationload").hide();
            }

        });
    }
    else {
        // Do nothing!
    }
}


function BindReviewedFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ReviewedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblReviewedData").removeClass('my-table-bordered').addClass('table-bordered');
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            RevieweddatatableVariable = $('#tblReviewedData').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: "38.5vh",
                scrollX: true,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'TRANSACTION_ID' },
                    {
                        'data': 'TRANSACTION_DATETIME',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                            }
                        }

                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'CTP_VRN' },
                    { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                    { 'data': 'FRONT_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                    { 'data': 'VEHICLESPEED', },
                    { 'data': 'AUDITED_VRN' },
                    { 'data': 'AUDIT_VEHICLE_CLASS_NAME' },
                    { 'data': 'MEARGED_TRAN_ID' },
                    { 'data': 'AUDITOR_NAME' },
                    { 'data': 'TRANS_STATUS_NAME' },
                ],
                'columnDefs': [
               {
                   "targets": 8,
                   "className": "text-center",
               },
                {
                    "targets": 9,
                    "className": "text-center",
                },
                 {
                     "targets": 12,
                     "className": "text-center",
                 },
               {
                   "targets": 13,
                   "className": "text-center",
               }],
                width: "100%"
            });
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= ($("#tblUnreviewedData").height())) && !NoMoredata && !inProgress) {
                    AppendReviewedData();
                }
            });
            thId = 'tblReviewedDataTR';
            myVar = setInterval("myclick()", 500);
            inProgress = false;

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadReviewedData() {
    if (searchEnable) {
        FilteReviewedData();
    }
    else {
        pageload = 1;
        $(".animationload").show();
        inProgress = true;
        var Inputdata = { pageindex: pageload, pagesize: pagesize }
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "ReviewedListScroll",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                pageload++;
                NoMoredata = data.length < pagesize;
                RevieweddatatableVariable.clear().draw();
                RevieweddatatableVariable.rows.add(data); // Add new data
                RevieweddatatableVariable.columns.adjust().draw();

                inProgress = false;
            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
    }
}

function AppendReviewedData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ReviewedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            pageload++;
            NoMoredata = data.length < pagesize;
            RevieweddatatableVariable.rows.add(data); // Add new data
            RevieweddatatableVariable.columns.adjust().draw();

            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function ResetReviewedFilter() {
    searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    BindDateTime();
    reloadReviewedData();
}

function FilteReviewedData() {
    var StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        StartDate = DateFormatTime(StartDate);
    }

    var EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate = DateFormatTime(EndDate);
    }
    var Inputdata = {
        GantryId: $("#ddlGantry").val(),
        StartDate: StartDate,
        EndDate: EndDate,
        PlateNumber: $("#PlateNumber").val(),
        VehicleClassId: $("#VehicleClassId").val(),
        ParentTranscationId: $("#ParentTranscationId").val(),
        ReviewerId: $("#ReviewerId").val(),
        ReviewerStatus: $("#ReviewerStatus").val()

    }
    inProgress = true;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ReviewedFilter",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            searchEnable = true;
            $(".animationload").hide();
            RevieweddatatableVariable.clear().draw();
            RevieweddatatableVariable.rows.add(data);
            RevieweddatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}


function BindChargedFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ChargedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblChargedData").removeClass('my-table-bordered').addClass('table-bordered');
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            ChargeddatatableVariable = $('#tblChargedData').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: "38.5vh",
                scrollX: true,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'TRANSACTION_ID' },
                    {
                        'data': 'TRANSACTION_DATETIME',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                            }
                        }

                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'CTP_VRN' },
                    { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                    { 'data': 'FRONT_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
                                $(nTd).html("<img src=" + oData.REAR_IMAGE + " height='40' width='60' onclick='openImagePreview(this);' />");
                            }
                        }
                    },
                    {
                        'data': 'REAR_VIDEO_URL',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_VIDEO_URL != '' && oData.REAR_VIDEO_URL != null) {

                                $(nTd).html("<span class='cur-p  icon-holder' aria-expanded='false' onclick='openVideo(this);' style='font-size: 18px;' path=" + oData.REAR_VIDEO_URL + "><i class='c-blue-500 ti-video-camera'></i></span>");
                            }
                        }
                    },
                    {
                        'data': 'VEHICLESPEED',

                    },
                    { 'data': 'AMOUNT' },
                ],
                'columnDefs': [
               {
                   "targets": 8,
                   "className": "text-center",
               },
                {
                    "targets": 9,
                    "className": "text-center",
                },
                 {
                     "targets": 12,
                     "className": "text-center",
                 },
               {
                   "targets": 13,
                   "className": "text-center",
               }],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblChargedData").height()) && !NoMoredata && !inProgress) {
                    AppendChargedData();
                }
            });
            thId = 'tblChargedDataTR';
            myVar = setInterval("myclick()", 500);
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadChargedData() {
    if (searchEnable) {
        FilterChargedData();
    }
    else {
        pageload = 1;
        $(".animationload").show();
        inProgress = true;
        var Inputdata = { pageindex: pageload, pagesize: pagesize }
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "ChargedListScroll",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                pageload++;
                NoMoredata = data.length < pagesize;
                ChargeddatatableVariable.clear().draw();
                ChargeddatatableVariable.rows.add(data); // Add new data
                ChargeddatatableVariable.columns.adjust().draw();
                inProgress = false;
            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
    }

}

function AppendChargedData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ChargedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            pageload++;
            NoMoredata = data.length < pagesize;
            ChargeddatatableVariable.rows.add(data); // Add new data
            ChargeddatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function ResetChargedFilter() {
    searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    BindDateTime();
    reloadChargedData();
}

function FilterChargedData() {
    var StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        StartDate = DateFormatTime(StartDate);
    }

    var EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate = DateFormatTime(EndDate);
    }
    var Inputdata = {
        GantryId: $("#ddlGantry").val(),
        StartDate: StartDate,
        EndDate: EndDate,
        ResidentId: $("#ResidentId").val(),
        Name: $("#Name").val(),
        Email: $("#Email").val(),
        PlateNumber: $("#PlateNumber").val(),
        VehicleClassId: $("#VehicleClassId").val()
    }
    inProgress = true;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ChargedFilter",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            searchEnable = true;
            $(".animationload").hide();
            ChargeddatatableVariable.clear().draw();
            ChargeddatatableVariable.rows.add(data);
            ChargeddatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function BindUnIdentifiedFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnidentifiedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblUnidentifiedData").removeClass('my-table-bordered').addClass('table-bordered');
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            datatableVariable = $('#tblUnidentifiedData').DataTable({
                data: data,
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: 230,
                scroller: {
                    loadingIndicator: true
                },
                processing: true,
                scrollCollapse: true,
                stateSave: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'TRANSACTION_ID' },
                    {
                        'data': 'TRANSACTION_DATETIME',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                            }
                        }

                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'CTP_VRN' },
                    { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                    { 'data': 'FRONT_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                        'data': 'VEHICLESPEED',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.VEHICLESPEED != '' && oData.VEHICLESPEED != null) {
                                $(nTd).html("" + oData.VEHICLESPEED + "KM/H");
                            }
                        }

                    },
                ],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            inProgress = false;
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblUnidentifiedData").height()) && !NoMoredata && !inProgress) {
                    AppendUnIdentifiedData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadUnIdentifiedData() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnidentifiedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function AppendUnIdentifiedData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnidentifiedListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}


function BindViolationFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ViolationListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblViolationData").removeClass('my-table-bordered').addClass('table-bordered');
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            datatableVariable = $('#tblViolationData').DataTable({
                data: data,
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: 230,
                scroller: {
                    loadingIndicator: true
                },
                processing: true,
                scrollCollapse: true,
                stateSave: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'TRANSACTION_ID' },
                    {
                        'data': 'TRANSACTION_DATETIME',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.TRANSACTION_DATETIME != '' && oData.TRANSACTION_DATETIME != null) {
                                oData.TRANSACTION_DATETIME = oData.TRANSACTION_DATETIME.replace('T', ' ');
                                $(nTd).html("" + oData.TRANSACTION_DATETIME + "");
                            }
                        }

                    },
                    { 'data': 'PLAZA_NAME' },
                    { 'data': 'CTP_VRN' },
                    { 'data': 'CTP_VEHICLE_CLASS_NAME' },
                    { 'data': 'FRONT_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_FRONT' },
                    {
                        'data': 'FRONT_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.FRONT_IMAGE != '' && oData.FRONT_IMAGE != null) {
                                oData.FRONT_IMAGE = "\\" + oData.FRONT_IMAGE;
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
                                $(nTd).html("<img src=" + oData.REAR_IMAGE + " height='40' width='60' onclick='openImagePreview(this);' />");
                            }
                        }
                    },
                    {
                        'data': 'REAR_VIDEO_URL',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_VIDEO_URL != '' && oData.REAR_VIDEO_URL != null) {

                                $(nTd).html("<span class='cur-p  icon-holder' aria-expanded='false' onclick='openVideo(this);' style='font-size: 18px;' path=" + oData.REAR_VIDEO_URL + "><i class='c-blue-500 ti-video-camera'></i></span>");
                            }
                        }
                    },
                    {
                        'data': 'VEHICLESPEED',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.VEHICLESPEED != '' && oData.VEHICLESPEED != null) {
                                $(nTd).html("" + oData.VEHICLESPEED + "KM/H");
                            }
                        }

                    },
                ],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            inProgress = false;
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblViolationData").height()) && !NoMoredata && !inProgress) {
                    AppendViolationData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadViolationData() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ViolationListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function AppendViolationData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "ViolationListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}


function BindTopUpFirstLoad() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "TopUpListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblTopUpData").removeClass('my-table-bordered').addClass('table-bordered');
            NoMoredata = data.length < pagesize
            pageload++;
            TopUpdatatableVariable = $('#tblTopUpData').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: "38.5vh",
                scrollX: true,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    { 'data': 'ENTRY_ID' },
                    {
                        'data': 'CREATION_DATE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.CREATION_DATE != '' && oData.CREATION_DATE != null) {
                                oData.CREATION_DATE = oData.CREATION_DATE.replace('T', ' ');
                                $(nTd).html("" + oData.CREATION_DATE + "");
                            }
                        }

                    },
                    { 'data': 'VEH_REG_NO' },
                    { 'data': 'VEHICLE_CLASS_NAME' },
                    { 'data': 'FIRST_NAME' },
                    { 'data': 'AMOUNT' },
                ],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblTopUpData").height()) && !NoMoredata && !inProgress) {
                    AppendTopUpData();
                }
            });
            thId = 'tblTopUpDataTR';
            myVar = setInterval("myclick()", 500);
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadTopUpData() {
    if (searchEnable) {
        FilterTopUpData();
    }
    else {
        pageload = 1;
        $(".animationload").show();
        inProgress = true;
        var Inputdata = { pageindex: pageload, pagesize: pagesize }
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "TopUpListScroll",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                pageload++;
                NoMoredata = data.length < pagesize;
                TopUpdatatableVariable.clear().draw();
                TopUpdatatableVariable.rows.add(data); // Add new data
                TopUpdatatableVariable.columns.adjust().draw();

                inProgress = false;
            },
            error: function (ex) {
                $(".animationload").hide();
            }
        });
    }

}

function AppendTopUpData() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "TopUpListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            pageload++;
            NoMoredata = data.length < pagesize;
            TopUpdatatableVariable.rows.add(data); // Add new data
            TopUpdatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function ResetTopUpFilter() {
    searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    BindDateTime();
    reloadTopUpData();
}

function FilterTopUpData() {
    var StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        StartDate = DateFormatTime(StartDate);
    }

    var EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate = DateFormatTime(EndDate);
    }
    var Inputdata = {
        StartDate: StartDate,
        EndDate: EndDate,
        ResidentId: $("#ResidentId").val(),
        Name: $("#Name").val(),
        Email: $("#Email").val(),
        PlateNumber: $("#PlateNumber").val(),
        VehicleClassId: $("#VehicleClassId").val()
    }
    inProgress = true;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "TopUpFilter",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            searchEnable = true;
            $(".animationload").hide();
            TopUpdatatableVariable.clear().draw();
            TopUpdatatableVariable.rows.add(data);
            TopUpdatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}


function myclick() {
    document.getElementById(thId).click();
    document.getElementById(thId).click();
    clearTimeout(myVar);
    $(".animationload").hide();
}

function DateChnaged(ctrl, src) {
    var cdt = new Date($(ctrl).val());
    if (src == 1) {
        var d = new Date(cdt.setMinutes(cdt.getMinutes() + 15));
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
        $("#EndDate").val(mm + '/' + dd + '/' + yy + " " + time1);
    }
    else {
        var d = new Date(cdt.setMinutes(cdt.getMinutes() - 15));
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
        $("#StartDate").val(mm + '/' + dd + '/' + yy + " " + time1);
    }
}

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

function bindSecond() {
    for (var i = 1; i < 13; i++) {
        $("#filterSec").append
              ($('<option></option>').val(i * 5).html(i * 5))
    }
}

function BindDateTime() {
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
    $("#StartDate").val(mm + '/' + dd + '/' + yy + " " + time1);
    $("#EndDate").val(mm + '/' + dd + '/' + yy + " " + time2);
}

function closePopup() {
    $('#AssociatedModal').modal('hide');
}

function openImagePreview(ctrl) {
    $('#ImageModal').modal({ backdrop: 'static', keyboard: false })
    $("#ImageModal").modal('show');
    var modalImg = document.getElementById("img01");
    modalImg.src = $(ctrl).attr('src');

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