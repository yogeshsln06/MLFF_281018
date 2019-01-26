var Transactioncol;
var TransactionId;
var pageload = 1;
var pagesize = 10;
var NoMoredata = false;
var inProgress = false;
var searchEnable = false;
var ResidentID = '';
var Name = '';
var Mobile = '';
var EmailId = '';
var VRN = '';
var VehicleClassId = 0;
var GantryId = 0;
var ParentTranscationId = 0;
var ReviewerId = 0;
var ReviewerStatus = 0;
var TransactionCategory = 0;
var StartDate = '';
var EndDate = '';


/***************************** UnReviewed Start ****************/
function BindUnreviewedFirstLoad() {
    $("#tblUnreviewedData").removeClass('my-table-bordered').addClass('table-bordered');
    datatableVariable = $('#tblUnreviewedData').DataTable({
        data: null,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        "bScrollInfinite": true,
        "bScrollCollapse": false,
        scrollY: "39.5vh",
        scrollX: true,
        scrollCollapse: false,
        autoWidth: true,
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
            { 'data': 'REAR_VRN' },
            { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
            {
                'data': 'REAR_IMAGE',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                        if (oData.REAR_IMAGE.indexOf('http') > -1) {
                            oData.REAR_IMAGE = oData.REAR_IMAGE;
                        }
                        else {
                            oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
        },
        {
            'searchable': false,
            'targets': [0, 8, 9, 12, 13]
        }],
        width: "100%"
    });
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    $('.dataTables_scrollBody').on('scroll', function () {
        var ScrollbarHeight = ($("#tblUnreviewedData").height() - $('.dataTables_scrollBody').outerHeight())
        if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
            AppendUnreviewedData();
        }
        //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= ($("#tblUnreviewedData").height())) && !NoMoredata && !inProgress) {
        //    AppendUnreviewedData();
        //}
    });
    thId = 'tblUnreviewedDataTR';
    myVar = setInterval("myclick()", 500);
}

function BindUnreviewedFirstLoad1() {
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
                data: null,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": false,
                scrollY: "39.5vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                if (oData.REAR_IMAGE.indexOf('http') > -1) {
                                    oData.REAR_IMAGE = oData.REAR_IMAGE;
                                }
                                else {
                                    oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                },
                {
                    'searchable': false,
                    'targets': [0, 8, 9, 12, 13]
                }],
                width: "100%"
            });
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblUnreviewedData").height() - $('.dataTables_scrollBody').outerHeight())
                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
                    AppendUnreviewedData();
                }
                //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= ($("#tblUnreviewedData").height())) && !NoMoredata && !inProgress) {
                //    AppendUnreviewedData();
                //}
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
    $("#ddlGantry").val(GantryId)
    $("#ddlTransactionCategory").val(TransactionCategory);
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
    FilteUnreviewedData();
    //if (searchEnable) {
    //    $("#ddlGantry").val(GantryId)
    //    $("#ddlTransactionCategory").val(TransactionCategory);
    //    $('#StartDate').val(StartDate);
    //    $('#EndDate').val(EndDate);
    //    FilteUnreviewedData();
    //}
    //else {
    //    pageload = 1;
    //    $(".animationload").show();
    //    inProgress = true;
    //    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    //    $.ajax({
    //        type: "POST",
    //        dataType: "json",
    //        data: JSON.stringify(Inputdata),
    //        url: "UnreviewedListScroll",
    //        contentType: "application/json; charset=utf-8",
    //        success: function (data) {
    //            $(".animationload").hide();
    //            pageload++;
    //            NoMoredata = data.length < pagesize;
    //            inProgress = false;
    //            datatableVariable.clear().draw();
    //            datatableVariable.rows.add(data); // Add new data
    //            datatableVariable.columns.adjust().draw();

    //        },
    //        error: function (ex) {
    //            $(".animationload").hide();
    //        }
    //    });
    //}

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

function openFilterpopupUnReviewed() {
    $("#ddlGantry").val(GantryId)
    $("#ddlTransactionCategory").val(TransactionCategory);
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

function FilteUnreviewedData() {
    var StartDate1 = '';
    var EndDate1 = '';

    StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        var StartDate1 = DateFormatTime(StartDate);
    }
    else {
        $('#StartDate').val(StartDate);
    }

    EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate1 = DateFormatTime(EndDate);
    }
    else {
        $('#EndDate').val(EndDate);
    }

    if ($("#ddlGantry").val() != 0) {
        GantryId = $("#ddlGantry").val();
    }
    else {
        GantryId = 0;
    }

    if ($("#ddlTransactionCategory").val() != 0) {
        TransactionCategory = $("#ddlTransactionCategory").val();
    }
    else {
        TransactionCategory = 0;
    }
    var Inputdata = {
        StartDate: StartDate1,
        EndDate: EndDate1,
        GantryId: GantryId,
        TransactionCategoryId: TransactionCategory
    }

    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "UnreviewedFilter",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#filterModel').modal('hide');
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

function ResetUnreviewedFilter() {
    //searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    ClearBindDateTime();
    //reloadUnreviewedData();
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

function BindAssociatedData(Seconds, dtCount) {
    if (dtCount > 0) {
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
                            { 'data': 'REAR_VRN' },
                            { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                            {
                                'data': 'REAR_IMAGE',
                                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                    if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                        if (oData.REAR_IMAGE.indexOf('http') > -1) {
                                            oData.REAR_IMAGE = oData.REAR_IMAGE;
                                        }
                                        else {
                                            oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
                        },
                        {
                            'searchable': false,
                            'targets': [0, 1, 8, 9, 12, 13]
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
    else
    {
        closePopup();
        alert("Selected Transaction already reviewed please refresh your data!!!");
        $(Transactioncol).parent().parent().remove();
    }
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
                    alert('Transaction ID ' + TransactionId + ' set as UNIDENTIFIED!!!')
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
                        var res = resultData[i].ErrorMessage.split("-999-");
                        for (var j = 0; j < res.length; j++) {
                            Msg = Msg + "\xb7 " + res[j].replace('success', '') + " \n";
                        }
                        close = true
                    }
                    else {
                        Msg = Msg + "\xb7 " + resultData[i].ErrorMessage + " \n";
                    }

                }
                if (close) {
                    if (Msg == '')
                        alert("Reviewed successfully !")
                    else
                        alert(Msg);
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
/***************************** UnReviewed Stop ****************/

/***************************** Reviewed Start ****************/
function BindReviewedFirstLoad() {
    $("#tblReviewedData").removeClass('my-table-bordered').addClass('table-bordered');
    $(".animationload").hide();
    RevieweddatatableVariable = $('#tblReviewedData').DataTable({
        data: null,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        "bScrollInfinite": true,
        "bScrollCollapse": false,
        scrollY: "39.5vh",
        scrollX: true,
        scrollCollapse: false,
        autoWidth: true,
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
            { 'data': 'REAR_VRN' },
            { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
            {
                'data': 'REAR_IMAGE',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                        if (oData.REAR_IMAGE.indexOf('http') > -1) {
                            oData.REAR_IMAGE = oData.REAR_IMAGE;
                        }
                        else {
                            oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
       },
         {
             'searchable': false,
             'targets': [0, 8, 9, 12, 13]
         }],
        width: "100%"
    });
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    $('.dataTables_scrollBody').on('scroll', function () {
        var ScrollbarHeight = ($("#tblReviewedData").height() - $('.dataTables_scrollBody').outerHeight())
        if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
            AppendReviewedData();
        }

    });
    thId = 'tblReviewedDataTR';
    myVar = setInterval("myclick()", 500);
}
function BindReviewedFirstLoad1() {
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
                "bScrollCollapse": false,
                scrollY: "39.5vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                if (oData.REAR_IMAGE.indexOf('http') > -1) {
                                    oData.REAR_IMAGE = oData.REAR_IMAGE;
                                }
                                else {
                                    oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
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
               },
                 {
                     'searchable': false,
                     'targets': [0, 8, 9, 12, 13]
                 }],
                width: "100%"
            });
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblReviewedData").height() - $('.dataTables_scrollBody').outerHeight())
                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
                    AppendReviewedData();
                }
                //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= ($("#tblReviewedData").height())) && !NoMoredata && !inProgress) {
                //    AppendReviewedData();
                //}
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
    $("#ddlGantry").val(GantryId);
    if (ParentTranscationId != 0)
        $("#ParentTranscationId").val(ParentTranscationId);
    else
        $("#ParentTranscationId").val('');

    $("#ReviewerId").val(ReviewerId);

    $("#ReviewerStatus").val(ReviewerStatus);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
    FilteReviewedData();
    //if (searchEnable) {
    //    $("#ddlGantry").val(GantryId);
    //    if (ParentTranscationId != 0)
    //        $("#ParentTranscationId").val(ParentTranscationId);
    //    else
    //        $("#ParentTranscationId").val('');

    //    $("#ReviewerId").val(ReviewerId);

    //    $("#ReviewerStatus").val(ReviewerStatus);
    //    $("#PlateNumber").val(VRN);
    //    $("#VehicleClassId").val(VehicleClassId);
    //    $('#StartDate').val(StartDate);
    //    $('#EndDate').val(EndDate);
    //    FilteReviewedData();
    //}
    //else {
    //    pageload = 1;
    //    $(".animationload").show();
    //    inProgress = true;
    //    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    //    $.ajax({
    //        type: "POST",
    //        dataType: "json",
    //        data: JSON.stringify(Inputdata),
    //        url: "ReviewedListScroll",
    //        contentType: "application/json; charset=utf-8",
    //        success: function (data) {
    //            $(".animationload").hide();
    //            pageload++;
    //            NoMoredata = data.length < pagesize;
    //            RevieweddatatableVariable.clear().draw();
    //            RevieweddatatableVariable.rows.add(data); // Add new data
    //            RevieweddatatableVariable.columns.adjust().draw();

    //            inProgress = false;
    //        },
    //        error: function (ex) {
    //            $(".animationload").hide();
    //        }
    //    });
    //}
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

function openFilterpopupReviewed() {

    $("#ddlGantry").val(GantryId);
    if (ParentTranscationId != 0)
        $("#ParentTranscationId").val(ParentTranscationId);
    else
        $("#ParentTranscationId").val('');

    $("#ReviewerId").val(ReviewerId);

    $("#ReviewerStatus").val(ReviewerStatus);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
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

function FilteReviewedData() {
    var StartDate1 = '';
    var EndDate1 = '';

    StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        var StartDate1 = DateFormatTime(StartDate);
    }
    else {
        $('#StartDate').val(StartDate);
    }

    EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate1 = DateFormatTime(EndDate);
    }
    else {
        $('#EndDate').val(EndDate);
    }
    if ($("#ddlGantry").val() != 0) {
        GantryId = $("#ddlGantry").val();
    }
    else {
        GantryId = 0;
    }
    if ($("#ParentTranscationId").val() != '') {
        ParentTranscationId = $("#ParentTranscationId").val();
    }
    else {
        ParentTranscationId = '';
    }
    if ($("#ReviewerId").val() != 0) {
        ReviewerId = $("#ReviewerId").val();
    }
    else {
        ReviewerId = 0;
    }
    if ($("#ReviewerStatus").val() != 0) {
        ReviewerStatus = $("#ReviewerStatus").val();
    }
    else {
        ReviewerStatus = 0;
    }
    if ($("#PlateNumber").val() != '') {
        VRN = $("#PlateNumber").val();
    }
    else {
        VRN = '';
    }
    if ($("#VehicleClassId").val() != 0) {
        VehicleClassId = $("#VehicleClassId").val();
    }
    else {
        VehicleClassId = 0;
    }

    var Inputdata = {
        GantryId: GantryId,
        StartDate: StartDate1,
        EndDate: EndDate1,
        PlateNumber: VRN,
        VehicleClassId: VehicleClassId,
        ParentTranscationId: ParentTranscationId,
        ReviewerId: ReviewerId,
        ReviewerStatus: ReviewerStatus

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
            $('#filterModel').modal('hide');
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

function ResetReviewedFilter() {
    //searchEnable = false;
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    ClearBindDateTime();
    //reloadReviewedData();
}

/***************************** Reviewed End ****************/

/***************************** Charged Start ****************/
function BindChargedFirstLoad() {
    $("#tblChargedData").removeClass('my-table-bordered').addClass('table-bordered');
    $(".animationload").hide();
    ChargeddatatableVariable = $('#tblChargedData').DataTable({
        data: null,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        "bScrollInfinite": true,
        "bScrollCollapse": false,
        scrollY: "39.5vh",
        scrollX: true,
        scrollCollapse: false,
        autoWidth: true,
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
            { 'data': 'REAR_VRN' },
            { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
            {
                'data': 'REAR_IMAGE',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                        if (oData.REAR_IMAGE.indexOf('http') > -1) {
                            oData.REAR_IMAGE = oData.REAR_IMAGE;
                        }
                        else {
                            oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
                        }
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
             {
                 'data': 'AMOUNT',
                 fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                     if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                         $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                             maximumFractionDigits: 0,
                             style: 'currency',
                             currency: 'IDR'
                         }) + "</span>");
                     }

                 }
             },
             {
                 'data': 'GATEWAY_RESPONSE_CODE',
                 fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                     if (oData.GATEWAY_RESPONSE_CODE != 0) {
                         $(nTd).html(oData.GATEWAY_RESPONSE_CODE + ' - ' + oData.OPERATOR_RESPONSE_TEXT);
                     }
                     else {
                         $(nTd).html('');
                     }

                 }
             },
             { 'data': 'TSOURCE' },
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
       },
       {
           "targets": 15,
           "className": 'dt-body-right',
       },
        {
            'searchable': false,
            'targets': [0, 8, 9, 12, 13]
        }
        ],
        width: "100%"
    });
    $('.dataTable').css('width', '1200px !important');
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    $('.dataTables_scrollBody').on('scroll', function () {
        var ScrollbarHeight = ($("#tblChargedData").height() - $('.dataTables_scrollBody').outerHeight())
        if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
            AppendChargedData();
        }
        //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblChargedData").height()) && !NoMoredata && !inProgress) {
        //    AppendChargedData();
        //}
    });
    thId = 'tblChargedDataTR';
    myVar = setInterval("myclick()", 500);
}

function BindChargedFirstLoad1() {
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
                "bScrollCollapse": false,
                scrollY: "39.5vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
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
                    { 'data': 'REAR_VRN' },
                    { 'data': 'NFP_VEHICLE_CLASS_NAME_REAR' },
                    {
                        'data': 'REAR_IMAGE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.REAR_IMAGE != '' && oData.REAR_IMAGE != null) {
                                if (oData.REAR_IMAGE.indexOf('http') > -1) {
                                    oData.REAR_IMAGE = oData.REAR_IMAGE;
                                }
                                else {
                                    oData.REAR_IMAGE = "\\" + oData.REAR_IMAGE;
                                }
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
                     {
                         'data': 'AMOUNT',
                         fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                             if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                                 $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                                     maximumFractionDigits: 0,
                                     style: 'currency',
                                     currency: 'IDR'
                                 }) + "</span>");
                             }

                         }
                     },
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
               },
               {
                   "targets": 15,
                   "className": 'dt-body-right',
               },
                {
                    'searchable': false,
                    'targets': [0, 8, 9, 12, 13]
                }
                ],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblChargedData").height() - $('.dataTables_scrollBody').outerHeight())
                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
                    AppendChargedData();
                }
                //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblChargedData").height()) && !NoMoredata && !inProgress) {
                //    AppendChargedData();
                //}
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
    $("#ddlGantry").val(GantryId)
    $("#ResidentId").val(ResidentID);
    $("#Name").val(Name);
    $("#Mobile").val(Mobile);
    $("#Email").val(EmailId);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
    FilterChargedData();
    //if (searchEnable) {
    //    $("#ddlGantry").val(GantryId)
    //    $("#ResidentId").val(ResidentID);
    //    $("#Name").val(Name);
    //    $("#Mobile").val(Mobile);
    //    $("#Email").val(EmailId);
    //    $("#PlateNumber").val(VRN);
    //    $("#VehicleClassId").val(VehicleClassId);
    //    $('#StartDate').val(StartDate);
    //    $('#EndDate').val(EndDate);
    //    FilterChargedData();
    //}
    //else {
    //    pageload = 1;
    //    $(".animationload").show();
    //    inProgress = true;
    //    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    //    $.ajax({
    //        type: "POST",
    //        dataType: "json",
    //        data: JSON.stringify(Inputdata),
    //        url: "ChargedListScroll",
    //        contentType: "application/json; charset=utf-8",
    //        success: function (data) {
    //            $(".animationload").hide();
    //            pageload++;
    //            NoMoredata = data.length < pagesize;
    //            ChargeddatatableVariable.clear().draw();
    //            ChargeddatatableVariable.rows.add(data); // Add new data
    //            ChargeddatatableVariable.columns.adjust().draw();
    //            inProgress = false;
    //        },
    //        error: function (ex) {
    //            $(".animationload").hide();
    //        }
    //    });
    //}

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

function openFilterpopupCharged() {
    $("#ddlGantry").val(GantryId)
    $("#ResidentId").val(ResidentID);
    $("#Name").val(Name);
    $("#Mobile").val(Mobile);
    $("#Email").val(EmailId);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
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

function FilterChargedData() {
    var StartDate1 = '';
    var EndDate1 = '';

    StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        var StartDate1 = DateFormatTime(StartDate);
    }
    else {
        $('#StartDate').val(StartDate);
    }

    EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate1 = DateFormatTime(EndDate);
    }
    else {
        $('#EndDate').val(EndDate);
    }
    if ($("#ddlGantry").val() != 0) {
        GantryId = $("#ddlGantry").val();
    }
    else {
        GantryId = 0;
    }
    if ($("#ResidentId").val() != '') {
        ResidentID = $("#ResidentId").val();
    }
    else {
        ResidentID = '';
    }
    if ($("#Name").val() != '') {
        Name = $("#Name").val();
    }
    else {
        Name = '';
    }
    if ($("#Email").val() != '') {
        EmailId = $("#Email").val();
    }
    else {
        EmailId = '';
    }
    if ($("#PlateNumber").val() != '') {
        VRN = $("#PlateNumber").val();
    }
    else {
        VRN = '';
    }
    if ($("#VehicleClassId").val() != 0) {
        VehicleClassId = $("#VehicleClassId").val();
    }
    else {
        VehicleClassId = 0;
    }
    var Inputdata = {
        GantryId: GantryId,
        StartDate: StartDate1,
        EndDate: EndDate1,
        ResidentId: ResidentID,
        Name: Name,
        Email: EmailId,
        PlateNumber: VRN,
        VehicleClassId: VehicleClassId
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
            $('#filterModel').modal('hide');
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

function ResetChargedFilter() {
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    ClearBindDateTime();
}

/***************************** Charged End ****************/

/***************************** Top-Up Start ****************/
function BindTopUpFirstLoad() {
    $("#tblTopUpData").removeClass('my-table-bordered').addClass('table-bordered');
    TopUpdatatableVariable = $('#tblTopUpData').DataTable({
        data: null,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        "bScrollInfinite": true,
        "bScrollCollapse": false,
        scrollY: "39.5vh",
        scrollX: true,
        scrollCollapse: false,
        autoWidth: true,
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
            {
                'data': 'AMOUNT',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                        $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                            maximumFractionDigits: 0,
                            style: 'currency',
                            currency: 'IDR'
                        }) + "</span>");
                    }

                }
            },

        ],
        'columnDefs': [
        {
            "targets": 6,
            "className": 'dt-body-right'
        },
        ],
        width: "100%"
    });
    $('.dataTable').css('width', '1200px !important');
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    $('.dataTables_scrollBody').on('scroll', function () {
        var ScrollbarHeight = ($("#tblTopUpData").height() - $('.dataTables_scrollBody').outerHeight())


        //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblTopUpData").height()) && !NoMoredata && !inProgress) {
        //    //console.log("scrollBody : " + $('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height());
        //    //console.log("table : " + $("#tblTopUpData").height());
        //    AppendTopUpData();
        //}

        if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
            AppendTopUpData();
        }
    });
    thId = 'tblTopUpDataTR';
    myVar = setInterval("myclick()", 500);
}

function BindTopUpFirstLoad1() {
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
                "bScrollCollapse": false,
                scrollY: "39.5vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
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
                    {
                        'data': 'AMOUNT',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                                $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                                    maximumFractionDigits: 0,
                                    style: 'currency',
                                    currency: 'IDR'
                                }) + "</span>");
                            }

                        }
                    },

                ],
                'columnDefs': [
                {
                    "targets": 6,
                    "className": 'dt-body-right'
                },
                ],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblTopUpData").height() - $('.dataTables_scrollBody').outerHeight())


                //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblTopUpData").height()) && !NoMoredata && !inProgress) {
                //    //console.log("scrollBody : " + $('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height());
                //    //console.log("table : " + $("#tblTopUpData").height());
                //    AppendTopUpData();
                //}

                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
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
    $("#ResidentId").val(ResidentID);
    $("#Name").val(Name);
    $("#Mobile").val(Mobile);
    $("#Email").val(EmailId);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
    FilterTopUpData();
    //if (searchEnable) {
    //    $("#ResidentId").val(ResidentID);
    //    $("#Name").val(Name);
    //    $("#Mobile").val(Mobile);
    //    $("#Email").val(EmailId);
    //    $("#PlateNumber").val(VRN);
    //    $("#VehicleClassId").val(VehicleClassId);
    //    $('#StartDate').val(StartDate);
    //    $('#EndDate').val(EndDate);
    //    FilterTopUpData();
    //}
    //else {
    //    pageload = 1;
    //    $(".animationload").show();
    //    inProgress = true;
    //    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    //    $.ajax({
    //        type: "POST",
    //        dataType: "json",
    //        data: JSON.stringify(Inputdata),
    //        url: "TopUpListScroll",
    //        contentType: "application/json; charset=utf-8",
    //        success: function (data) {
    //            $(".animationload").hide();
    //            pageload++;
    //            NoMoredata = data.length < pagesize;
    //            TopUpdatatableVariable.clear().draw();
    //            TopUpdatatableVariable.rows.add(data); // Add new data
    //            TopUpdatatableVariable.columns.adjust().draw();

    //            inProgress = false;
    //        },
    //        error: function (ex) {
    //            $(".animationload").hide();
    //        }
    //    });
    //}

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

function openFilterpopupTopup() {
    $("#ResidentId").val(ResidentID);
    $("#Name").val(Name);
    $("#Mobile").val(Mobile);
    $("#Email").val(EmailId);
    $("#PlateNumber").val(VRN);
    $("#VehicleClassId").val(VehicleClassId);
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

function FilterTopUpData() {
    var StartDate1 = '';
    var EndDate1 = '';

    StartDate = $('#StartDate').val() || ''
    if (StartDate != '') {
        var StartDate1 = DateFormatTime(StartDate);
    }
    else {
        $('#StartDate').val(StartDate);
    }

    EndDate = $('#EndDate').val() || ''
    if (EndDate != '') {
        EndDate1 = DateFormatTime(EndDate);
    }
    else {
        $('#EndDate').val(EndDate);
    }
    if ($("#ResidentId").val() != '') {
        ResidentID = $("#ResidentId").val();
    }
    else {
        ResidentID = '';
    }
    if ($("#Name").val() != '') {
        Name = $("#Name").val();
    }
    else {
        Name = '';
    }
    if ($("#Email").val() != '') {
        EmailId = $("#Email").val();
    }
    else {
        EmailId = '';
    }
    if ($("#PlateNumber").val() != '') {
        VRN = $("#PlateNumber").val();
    }
    else {
        VRN = '';
    }
    if ($("#VehicleClassId").val() != 0) {
        VehicleClassId = $("#VehicleClassId").val();
    }
    else {
        VehicleClassId = 0;
    }
    var Inputdata = {
        StartDate: StartDate1,
        EndDate: EndDate1,
        ResidentId: ResidentID,
        Name: Name,
        Email: EmailId,
        PlateNumber: VRN,
        VehicleClassId: VehicleClassId
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
            $('#filterModel').modal('hide');
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

function ResetTopUpFilter() {
    $("#filterbox").find('input:text').val('');
    $("#filterbox").find('select').val(0);
    ClearBindDateTime();
}

/***************************** Top-Up End ****************/



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
    StartDate = mm + '/' + dd + '/' + yy + " " + time1;
    EndDate = mm + '/' + dd + '/' + yy + " " + time2;
    $('#StartDate').val(StartDate);
    $('#EndDate').val(EndDate);
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