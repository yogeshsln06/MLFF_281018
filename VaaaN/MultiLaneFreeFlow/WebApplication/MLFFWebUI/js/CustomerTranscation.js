var Transactioncol;
var TransactionId;
var pageload = 1;
var pagesize = 10;
var NoMoredata = false;
var inProgress = false;
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
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;

            datatableVariable = $('#tblUnreviewedData').DataTable({
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
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            inProgress = false;
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblUnreviewedData").height()) && !NoMoredata && !inProgress) {
                    AppendUnreviewedData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadData() {
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


        },
        error: function (x, e) {
            $(".animationload").hide();
            closePopup();
        }

    });
}

function BindAssociatedData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "GetAssociated",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $(".animationload").hide();
            if (data != "No Record Found") {
                $("#tblAssociatedData").removeClass('my-table-bordered').addClass('table-bordered');
                datatableVariable = $('#tblAssociatedData').DataTable({
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
                    width: "100%"
                });
                $('.dataTables_filter input').attr("placeholder", "Search this list…");
            }
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function openpopup() {
    $("#warning").hide();
    //$("#btnAssociatedModalOpen").trigger('click');
    $('#AssociatedModal').modal('show');
    $("#txtVRN").val('');
    $("#ddlAuditedVehicleClass").val(0)
}

function closePopup() {
    $("#btnpopupClose").trigger('click');
    //$(".modal-backdrop").hide()
    $('#AssociatedModal').modal('hide');
}

function openImagePreview(ctrl) {
    var modalImg = document.getElementById("img01");
    modalImg.src = $(ctrl).attr('src');
    $("#btnImageModalOpen").trigger('click');
}

function openVideo(ctrl) {
    var VideoPath = $(ctrl).attr('path');
    var $video = $('#video video'),
        videoSrc = $('source', $video).attr('src', VideoPath);
    $video[0].load();
    $("#video").show();
    $("#btnVideoModalOpen").trigger('click');
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
            vehicleClassID: $('#ddlAuditedVehicleClass').val()
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
            datatableVariable = $('#tblReviewedData').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: '48vh',
                scroller: {
                    loadingIndicator: true
                },
                scrollX: true,
                processing: true,
                scrollCollapse: true,
                stateSave: true,
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
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            inProgress = false;
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblReviewedData").height()) && !NoMoredata && !inProgress) {
                    AppendReviewedData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadReviewedData() {
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

            datatableVariable = $('#tblChargedData').DataTable({
                //"oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                //"bScrollInfinite": true,
                //"bScrollCollapse": true,
                //scrollY: '48vh',
                //scroller: {
                //    loadingIndicator: true
                //},
                //scrollX: true,
                //processing: true,
                //scrollCollapse: true,
                //stateSave: true,
                //autoWidth: true,
                //paging: false,
                //info: false,
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
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            inProgress = false;
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblChargedData").height()) && !NoMoredata && !inProgress) {
                    AppendChargedData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadChargedData() {
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
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            datatableVariable = $('#tblTopUpData').DataTable({
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
            inProgress = false;
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblTopUpData").height()) && !NoMoredata && !inProgress) {
                    AppendTopUpData();
                }
            });

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function reloadTopUpData() {
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