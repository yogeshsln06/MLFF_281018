﻿var pageload = 1;
var Transload = 1;
var pagesize = 50;
var NoMoredata = false;
var inProgress = false;
var CustomerVehicleId = 0;
var CustomerAccountId = 0;
var CustomerRegistrationNumber = '';
var CustomerAccountJson = [];
var VehicleId = 0;
var searchEnable = false;
var CutomerId = 0;
var ResidentID = '';
var Name = '';
var Mobile = '';
var EmailId = '';
var VRN = '';
var VRCN = '';
var VehicleClassId = 0;
var QueueStatus = 0;
var ExceptionFlag = 0;
var Status = 0;
var mainWidth = 0;
var windowHeight = 300;
$(document).ready(function () {
    $("#sidebar-toggle").bind("click", function () {
        $(".animationload").show();
        thId = 'tblCustomerVehicleTR';
        myVar = setInterval("myclick()", 500);
    });
    BindCustmerVehicleAccount();

    mainWidth = $(window).innerWidth();
    $(window).resize(function () {
        if (mainWidth != $(window).innerWidth()) {
            setHeight();
            $(".animationload").show();
            thId = 'tblCustomerVehicleTR';
            mainWidth = $(window).innerWidth();
            myVar = setInterval("myclick()", 100);
        }
    });
});

function setHeight() {
    windowHeight = $(window).innerHeight()-400;
    //windowHeight = (windowHeight * 40) / 100

    $(".dataTables_scrollBody").css("height", windowHeight);
};

function refreshData() {
    if (searchEnable) {
        if (CutomerId || 0 != 0)
            $("#txtCustomerID").val(parseInt(CutomerId));
        $("#txtResidentID").val(ResidentID);
        $("#txtName").val(Name);
        $("#txtMobile").val(Mobile);
        $("#txtEmail").val(EmailId);
        $("#txtVRN").val(VRN);
        $("#txtVRCN").val(VRCN);
        $("#ddlVehicleClassId").val(VehicleClassId);
        $("#ddlQueueStatus").val(QueueStatus);
        $("#ddlExceptionFlag").val(ExceptionFlag);
        FilteCustomerData();
    }
    else {
        pageload = 1;
        $(".animationload").show();
        inProgress = true;
        NoMoredata = false;
        var Inputdata = { pageindex: pageload, pagesize: pagesize }
        $.ajax({
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Inputdata),
            url: "CustomerVehicleListScroll",
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

function closePopup() {
    $("#btnpopupClose").trigger('click');
    $(".modal-backdrop").hide()
}

function openImgV(ctrl) {
    $(ctrl).next().trigger('click');
}


function BindCustmerVehicleAccount() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "CustomerVehicleListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            NoMoredata = data.length < pagesize
            pageload++;
            inProgress = false;
            $("#tblCustomerVehicle").removeClass('my-table-bordered').addClass('table-bordered');
            datatableVariable = $('#tblCustomerVehicle').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": false,
                //scrollY: "39.5vh",
                scrollY: windowHeight,
                scrollX: true,
                scrollCollapse: false,
                autoWidth: true,
                paging: false,
                info: false,
                columns: [
                    { 'data': 'EntryId' },
                    {
                        'data': 'EntryId',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a href='javascript:void(0);' onclick='DetailsOpen(this," + oData.EntryId + ")'>" + oData.EntryId + "</a>");
                        }
                    },
                    { 'data': 'VehRegNo' },
                    { 'data': 'VehicleClassName' },
                    {
                        'data': 'VehicleImageFront', "className": "dt-center",
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.VehicleImageFront != '' && oData.VehicleImageFront != null) {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openImgV(this);' style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageFront + "><i class='c-blue-500 ti-camera'></i></span><img onclick='zoomImage(this);' style='display:none;' src='../Attachment/VehicleImage/" + oData.VehicleImageFront + "' data-high-res-src='' alt='' class='gallery-items'>");
                            }
                            else {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false'  style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageFront + "><i class='c-gray-500 ti-camera'></i></span>");
                            }
                        }
                    },
                    {
                        'data': 'VehicleImageRear', "className": "dt-center",
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.VehicleImageRear != '' && oData.VehicleImageRear != null) {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openImgV(this);' style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageRear + "><i class='c-blue-500 ti-camera'></i></span><img onclick='zoomImage(this);' style='display:none;' src='../Attachment/VehicleImage/" + oData.VehicleImageRear + "' data-high-res-src='' alt='' class='gallery-items'>");
                            }
                            else {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false'  style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageRear + "><i class='c-gray-500 ti-camera'></i></span>");
                            }
                        }
                    },
                    { 'data': 'CustomerQueueStatusName' },
                    { 'data': 'ExceptionFlagName' },
                    {
                        'data': 'AccountBalance',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            var bal = oData.AccountBalance;

                            if (bal != '' && bal != null) {
                                if (bal < 0) {
                                    bal = (bal) * (-1);
                                    $(nTd).html("<span class='text-right red'>(" + bal.toLocaleString('id-ID', {
                                        maximumFractionDigits: 10,
                                        style: 'currency',
                                        currency: 'IDR'
                                    }) + ")</span>");
                                }
                                else {
                                    $(nTd).html("<span class='text-right'>" + bal.toLocaleString('id-ID', {
                                        maximumFractionDigits: 10,
                                        style: 'currency',
                                        currency: 'IDR'
                                    }) + "</span>");
                                }
                            }
                        }
                    },
                    {
                        'data': 'AccountId',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                              "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
                   "<span class='icon-holder'>" +
                        "<i class='c-blue-500 ti-menu-alt'></i>" +
                    "</span>" +
                "</a>" +
                " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
                "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditOpen(this," + oData.EntryId + ")'>" +
                "        <span class='title'>Update</span>" +
                "    </a>" +
                "    <a class='dropdown-item ' href='javascript:void(0);' onclick='HistoryRecords(this," + oData.EntryId + "," + oData.AccountId + ")'>" +
                "        <span class='title'>Transaction</span>" +
                "    </a>" +
                "    <a class='dropdown-item ' href='javascript:void(0);' onclick='CustomerDetailsOpen(this," + oData.AccountId + ")'>" +
                "        <span class='title'>Customer</span>" +
                "    </a>" +
                "</div>" +
            "</div>");
                        }
                    },
                ],
                columnDefs: [{ "orderable": false, "targets": 7 },
                 { 'searchable': false, 'targets': [0, 4, 5, 9] },
                 { "targets": 8, "className": 'dt-body-right', }],
                order: [[1, 'asc']],
            });
            //$('.dataTable').css('width', '1200px !important');
            datatableVariable.on('order.dt search.dt', function () {
                datatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblCustomerVehicle").height() - $('.dataTables_scrollBody').outerHeight())
                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
                    AppendCustomerData();
                }
            });
            thId = 'tblCustomerVehicleTR';
            myVar = setInterval("myclick()", 500);
           
        },
        error: function (ex) {
            $(".animationload").hide();
        }

    });
}

function AppendCustomerData() {
    inProgress = true;
    $('#loadingdiv').show()
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "CustomerVehicleListScroll?pageIndex=" + pageload + "&pagesize=" + pagesize,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#loadingdiv').hide()
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
            //datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();

        },
        error: function (ex) {
            $('#loadingdiv').hide()
        }

    });
}

function openpopup() {
    $("#warning").hide();
    $('#VehicleModal').modal({ backdrop: 'static', keyboard: false })
    $('#VehicleModal').modal('show');
    if (CustomerAccountJson.length == null) {
        alert("No Customer Account exists");
    }
    else if (CustomerAccountJson.length == 0) {
        alert("No Customer Account exists");
    }
    else {
        $.each((CustomerAccountJson), function (i, residentId) {
            if (residentId.ResidentId != '') {
                $("#ResidentId").append
                ($('<option></option>').val(residentId.ResidentId).html(residentId.ResidentId))
            }

        })
    }
}

function validTAGId(str) {
    var pattern = /^[0-9a-fA-F]+$/;
    return String(str).match(pattern);
}

function validateCustomerVehicle() {
    var valid = true;

    if ($("#ResidentId").val() == 0) {
        showError($("#ResidentId"), $("#ResidentId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ResidentId"), '');
    }

    if ($("#FirstName").val() == '') {
        showError($("#FirstName"), $("#FirstName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#FirstName"), '');
    }

    if ($("#MobileNo").val() == '') {
        showError($("#MobileNo"), $("#MobileNo").attr('data-val-required'));
        valid = false;
    }
    else if (!validMobile($("#MobileNo").val())) {
        showError($("#MobileNo"), 'Enter Valid Mobile Number');
        valid = false;
    }
    else {
        showError($("#MobileNo"), '');
    }

    if ($("#EmailId").val() == '') {
        showError($("#EmailId"), $("#EmailId").attr('data-val-required'));
        valid = false;
    }
    else if (!validEmail($("#EmailId").val())) {
        showError($("#EmailId"), 'Enter Valid Email Id');
        valid = false;
    }
    else {
        showError($("#EmailId"), '');
    }

    if ($("#Address").val() == '') {
        showError($("#Address"), $("#Address").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#Address"), '');
    }

    if ($("#VehicleRCNumber").val() == '') {
        showError($("#VehicleRCNumber"), $("#VehicleRCNumber").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehicleRCNumber"), '');
    }

    if ($("#VehRegNo").val() == '') {
        showError($("#VehRegNo"), $("#VehRegNo").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehRegNo"), '');
    }

    if ($("#OwnerName").val() == '') {
        showError($("#OwnerName"), $("#OwnerName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#OwnerName"), '');
    }

    if ($("#OwnerAddress").val() == '') {
        showError($("#OwnerAddress"), $("#OwnerAddress").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#OwnerAddress"), '');
    }

    if ($('#VehicleImageFront')[0].files.length === 0 && $('#hfVehicleImageFront').val() == "") {
        showError($("#VehicleImageFront"), 'Front Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageFront"), '');
    }

    if ($('#VehicleImageRear')[0].files.length === 0 && $('#hfVehicleImageRear').val() == "") {
        showError($("#VehicleImageRear"), 'Rear Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageRear"), '');
    }

    if ($('#VehicleImageRight')[0].files.length === 0 && $('#hfVehicleImageRight').val() == "") {
        showError($("#VehicleImageRight"), 'Right Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageRight"), '');
    }

    if ($('#VehicleImageLeft')[0].files.length === 0 && $('#hfVehicleImageLeft').val() == "") {
        showError($("#VehicleImageLeft"), 'Left Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleImageLeft"), '');
    }

    if ($('#VehicleRCNumberImagePath')[0].files.length === 0 && $('#hfVehicleRCNumberImage').val() == "") {
        showError($("#VehicleRCNumberImagePath"), 'Registration Certificate Image is required');
        valid = false;
    }
    else {
        showError($("#VehicleRCNumberImagePath"), '');
    }

    if ($("#TidFront").val() == '') {
        showError($("#TidFront"), $("#TidFront").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#TidFront"), '');
    }

    if ($("#TidRear").val() == '') {
        showError($("#TidRear"), $("#TidRear").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#TidRear"), '');
    }

    if ($("#VehicleClassId").val() == 0) {
        showError($("#VehicleClassId"), $("#VehicleClassId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#VehicleClassId"), '');
    }

    //if ($("#TagId").val() == '') {
    //    showError($("#TagId"), $("#TagId").attr('data-val-required'));
    //    valid = false;
    //}

    if ($("#TagId").val() != '') {
        if ($("#TagId").val().length != 24) {
            showError($("#TagId"), 'Valid EPC Required');
            valid = false;
        }
        else if (!validTAGId($("#TagId").val())) {
            showError($("#TagId"), 'Valid EPC Required');
            valid = false;
        }
        else {
            showError($("#TagId"), '');
        }
    }


    if ($("#QueueStatus").val() == 0) {
        showError($("#QueueStatus"), $("#QueueStatus").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#QueueStatus"), '');
    }

    if ($("#ExceptionFlag").val() == 0) {
        showError($("#ExceptionFlag"), $("#ExceptionFlag").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ExceptionFlag"), '');
    }

    return valid;
}

function GetTagId() {
    if ($("#VehRegNo").val() == '') {
        showError('Enter Vehicle Registration No');
        return false;
    }
    else if ($("#VehicleClassId").val() == 0) {
        showError('Select Vehicle Class');
        return false;
    }
    var InpurField = {
        VehicleClassId: $("#VehicleClassId").val(),
        VehRegNo: $("#VehRegNo").val()
    }

    $.ajax({
        type: "POST",
        url: "GetTagId",
        dataType: "JSON",
        async: true,
        data: JSON.stringify(InpurField),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#TagId").val(data.Data);
        },
        error: function (ex) {

        }
    });
}

function zoomImage(ctrl) {
    var viewer = ImageViewer();
    var imgSrc = ctrl.src,
               highResolutionImage = $(ctrl).data('high-res-img');
    viewer.show(imgSrc, highResolutionImage);
}

function openImg(ctrl) {
    var id = $(ctrl).attr('id')
    $("#" + id.replace('lbl', 'img')).trigger('click');
}

function NewCustomerVehicle() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "/Registration/NewCustomerVehicle",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $("#exampleModalLabel").text("Register New Vehicle");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#FirstName").attr("disabled", "disabled");
            $("#Address").attr("disabled", "disabled");
            $("#MobileNo").attr("disabled", "disabled");
            $("#EmailId").attr("disabled", "disabled");
            $("#EntryId").attr("disabled", "disabled");
            $(".animationload").hide();
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#VehicleImageFront").show();
            $("#VehicleImageRear").show();
            $("#VehicleImageRight").show();
            $("#VehicleImageLeft").show();
            $("#VehicleRCNumberImagePath").show();

            $("#lblVehicleImageFront").hide();
            $("#lblVehicleImageRear").hide();
            $("#lblVehicleImageRight").hide();
            $("#lblVehicleImageLeft").hide();
            $("#lblVehicleRCNumberImagePath").hide();

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnpopupClose").text('Cancel').removeClass('btn-outline-secondary').addClass('btn-outline-danger').show();
            $("#btnpopupCancel").removeClass('btn-outline-danger').addClass('btn-outline-secondary').hide();
            $("#btnSaveNew").show();
            Status = 0;
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function DetailsOpen(ctrl, id) {
    $(".animationload").show();

    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomerVehicle?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);

            $("#exampleModalLabel").text("View " + $('#VehRegNo').val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");

            $(".animationload").hide();
            $("#ValidUntil").attr("readonly", false);
            openpopup();
            $("#fildset").attr("disabled", "disabled");
            $("#VehicleImageFront").hide();
            $("#VehicleImageRear").hide();
            $("#VehicleImageRight").hide();
            $("#VehicleImageLeft").hide();
            $("#VehicleRCNumberImagePath").hide();


            $("#labelImageFront").hide();
            $("#labelImageRear").hide();
            $("#labelImageRight").hide();
            $("#labelImageLeft").hide();
            $("#labelImageRC").hide();

            $("#lblVehicleImageFront").show();
            $("#lblVehicleImageRear").show();
            $("#lblVehicleImageRight").show();
            $("#lblVehicleImageLeft").show();
            $("#lblVehicleRCNumberImagePath").show();

            if ($("#hfVehicleImageFront").val() == '') {
                $("#lblVehicleImageFront").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled');
                $("#lblVehicleImageFront").text('Attach File');
            }
            if ($("#hfVehicleImageRear").val() == '') {
                $("#lblVehicleImageRear").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled').text('Attach File');
            }
            if ($("#hfVehicleImageRight").val() == '') {
                $("#lblVehicleImageRight").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled').text('Attach File');
            }
            if ($("#hfVehicleImageLeft").val() == '') {
                $("#lblVehicleImageLeft").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled').text('Attach File');
            }
            if ($("#hfVehicleRCNumberImage").val() == '') {
                $("#lblVehicleRCNumberImagePath").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled').text('Attach File');
            }
            $("#imgVehicleImageFront").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageFront").val());
            $("#imgVehicleImageRear").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRear").val());
            $("#imgVehicleImageRight").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRight").val());
            $("#imgVehicleImageLeft").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageLeft").val());
            $("#imgVehicleRCNumberImagePath").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleRCNumberImage").val());


            $.grep(CustomerAccountJson, function (element, index) {
                if (element.AccountId == $("#AccountId").val()) {
                    if (element.ResidentId == '')
                        alert("Resident Id not found please update customer acount first")
                    else
                        $("#ResidentId").val(element.ResidentId);

                }
            });

            $("#btnSave").hide();

            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
            $("#btnpopupClose").hide();
            $("#btnpopupCancel").hide();
            $("#btnpopupUpdate").show();
            $("#btnSaveNew").hide();


        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function EditOpen(ctrl, id) {
    $(ctrl).parent().addClass('hide').removeClass('open').hide();
    OpenUpdatepopUp(id)
}

function OpenUpdatepopUp(id) {
    VehicleId = id || 0
    if (VehicleId == 0)
        VehicleId = $("#EntryId").val();
    $('#partialassociated').html("");
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomerVehicle?id=" + VehicleId,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("Update " + $('#VehRegNo').val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#EntryIdId").attr("disabled", "disabled");
            $("#FirstName").attr("disabled", "disabled");
            $("#Address").attr("disabled", "disabled");
            $("#MobileNo").attr("disabled", "disabled");
            $("#EmailId").attr("disabled", "disabled");
            $("#EntryId").attr("disabled", "disabled");
            $.grep(CustomerAccountJson, function (element, index) {
                if (element.AccountId == $("#AccountId").val()) {
                    if (element.ResidentId == '')
                        alert("Resident Id not found please update customer acount first")
                    else
                        $("#ResidentId").val(element.ResidentId);

                }
            });
            $(".animationload").hide();
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);

            $("#imgVehicleImageFront").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageFront").val());
            $("#imgVehicleImageRear").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRear").val());
            $("#imgVehicleImageRight").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRight").val());
            $("#imgVehicleImageLeft").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageLeft").val());
            $("#imgVehicleRCNumberImagePath").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleRCNumberImage").val());
            if ($("#hfVehicleImageFront").val() == '') {
                $("#VehicleImageFront").show();
                $("#lblVehicleImageFront").hide();
                $("#labelImageFront").show();
            }
            else {
                $("<br/>").insertBefore($("#labelImageFront"));
                $("#VehicleImageFront").hide();
                $("#lblVehicleImageFront").show();
                $("#labelImageFront").find('span').text('Update File').show();
            }

            if ($("#hfVehicleImageRear").val() == '') {
                $("#VehicleImageRear").show();
                $("#lblVehicleImageRear").hide();
                $("#labelImageRear").show();

            }
            else {
                $("#VehicleImageRear").hide();
                $("#lblVehicleImageRear").show();
                $("<br/>").insertBefore($("#labelImageRear"));
                $("#labelImageRear").find('span').text('Update File').show();
            }

            if ($("#hfVehicleImageRight").val() == '') {
                $("#VehicleImageRight").show();
                $("#lblVehicleImageRight").hide();
                $("#labelImageRight").show();

            }
            else {
                $("#VehicleImageRight").hide();
                $("#lblVehicleImageRight").show();
                $("<br/>").insertBefore($("#labelImageRight"));
                $("#labelImageRight").find('span').text('Update File').show();
            }
            if ($("#hfVehicleImageLeft").val() == '') {
                $("#VehicleImageLeft").show();
                $("#lblVehicleImageLeft").hide();

                $("#labelImageLeft").show();
            }
            else {
                $("#VehicleImageLeft").hide();
                $("#lblVehicleImageLeft").show();
                $("<br/>").insertBefore($("#labelImageLeft"));
                $("#labelImageLeft").find('span').text('Update File').show();
            }
            if ($("#hfVehicleRCNumberImage").val() == '') {
                $("#VehicleRCNumberImagePath").show();
                $("#lblVehicleRCNumberImagePath").hide();
                $("#labelImageRC").show();

            }
            else {
                $("#VehicleRCNumberImagePath").hide();
                $("#lblVehicleRCNumberImagePath").show();
                $("<br/>").insertBefore($("#labelImageRC"));
                $("#labelImageRC").find('span').text('Update File').show();
            }
            Status = $("#QueueStatus").val();
            $("#btnSave").show();
            //$("#btnSave").text("Update");
            $("#btnpopupUpdateCancel").hide();
            $("#btnSaveNew").hide();
            $("#btnpopupCancel").hide();
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger').addClass('').text('Cancel').show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function SaveData(action) {
    $("#warning").html("");
    if ($("#needs-validation").valid()) {
        var ValidUntil = $('#ValidUntil').val() || ''
        if (ValidUntil != '') {
            ValidUntil = DateFormat(ValidUntil);
        }

        if (validateCustomerVehicle()) {
            if ($("#TidFront").val() != $("#TidRear").val()) {
                var SendEmail = false;
                if ($("#QueueStatus").val() == 3) {
                    if (Status != $("#QueueStatus").val()) {
                        SendEmail = true;
                    }
                }
                var EntryId = $("#EntryId").val() || '';
                var AccountId = $("#AccountId").val() || '';
                var ImageFrontChnage = true;
                var ImageRearChnage = true;
                var ImageLeftChnage = true;
                var ImageRightChnage = true;
                var RCNumberImageChnage = true;
                var PostURL = '/Registration/CustomerVehicleUpdate';
                var VehicleImageFront = $("#VehicleImageFrontPath").attr('href');//$("#VehicleImageFrontPath").text().trim();
                var VehicleImageRear = $("#VehicleImageRearPath").attr('href'); //$("#VehicleImageRearPath").text().trim();
                var VehicleImageRight = $("#VehicleImageRightPath").attr('href'); //$("#VehicleImageRightPath").text().trim();
                var VehicleImageLeft = $("#VehicleImageLeftPath").attr('href'); //$("#VehicleImageLeftPath").text().trim();
                var VehicleRCNumberImagePath = $("#VehicleRCNumberImagePathPath").text().trim();
                if (EntryId == '') {
                    EntryId = 0;
                    PostURL = '/Registration/CustomerVehicleAdd'
                }
                else {
                    if ($("#VehicleImageFrontPath").text().trim() == '') {
                        VehicleImageFront = $("#hfVehicleImageFront").val();
                        ImageFrontChnage = false;
                    }

                    if ($("#VehicleImageRearPath").text().trim() == '') {
                        VehicleImageRear = $("#hfVehicleImageRear").val();
                        ImageRearChnage = false;
                    }

                    if ($("#VehicleImageRightPath").text().trim() == '') {
                        VehicleImageRight = $("#hfVehicleImageRight").val();
                        ImageRightChnage = false;
                    }

                    if ($("#VehicleImageLeftPath").text().trim() == '') {
                        VehicleImageLeft = $("#hfVehicleImageLeft").val();
                        ImageLeftChnage = false;
                    }

                    if ($("#VehicleRCNumberImagePathPath").text().trim() == '') {
                        VehicleRCNumberImagePath = $("#hfVehicleRCNumberImage").val();
                        RCNumberImageChnage = false;
                    }

                }

                var Inputdata = {
                    EntryId: EntryId,
                    AccountId: AccountId,
                    ResidentId: $("#ResidentId").val(),
                    FirstName: $("#FirstName").val(),
                    Address: $('#Address').val(),
                    MobileNo: $('#MobileNo').val(),
                    EmailId: $('#EmailId').val(),
                    VehicleRCNumber: $('#VehicleRCNumber').val(),
                    VehRegNo: $('#VehRegNo').val(),
                    OwnerName: $("#OwnerName").val(),
                    OwnerAddress: $("#OwnerAddress").val(),
                    Brand: $("#Brand").val(),
                    VehicleType: $("#VehicleType").val(),
                    VehicleCategory: $("#VehicleCategory").val(),
                    Model: $("#Model").val(),
                    ManufacturingYear: $("#ManufacturingYear").val(),
                    CyclinderCapacity: $("#CyclinderCapacity").val(),
                    FrameNumber: $("#FrameNumber").val(),
                    EngineNumber: $("#EngineNumber").val(),
                    VehicleColor: $("#VehicleColor").val(),
                    FuelType: $("#FuelType").val(),
                    LicencePlateColor: $("#LicencePlateColor").val(),
                    RegistrationYear: $("#RegistrationYear").val(),
                    VehicleOwnershipDocumentNumber: $('#VehicleOwnershipDocumentNumber').val(),
                    LocationCode: $('#LocationCode').val(),
                    RegistrationQueueNumber: $('#RegistrationQueueNumber').val(),
                    ValidUntil: ValidUntil,
                    VehicleImageFront: VehicleImageFront,
                    VehicleImageRear: VehicleImageRear,
                    VehicleImageRight: VehicleImageRight,
                    VehicleImageLeft: VehicleImageLeft,
                    VehicleRCNumberImagePath: VehicleRCNumberImagePath,
                    ExceptionFlag: $("#ExceptionFlag").val(),
                    TidFront: $("#TidFront").val(),
                    TidRear: $("#TidRear").val(),
                    VehicleClassId: $("#VehicleClassId").val(),
                    TagId: $("#TagId").val(),
                    QueueStatus: $("#QueueStatus").val(),
                    AccountBalance: $("#AccountBalance").val(),

                    ImageFrontChnage: ImageFrontChnage,
                    ImageRearChnage: ImageRearChnage,
                    ImageLeftChnage: ImageLeftChnage,
                    ImageRightChnage: ImageRightChnage,
                    RCNumberImageChnage: RCNumberImageChnage,
                    SendEmail: SendEmail
                }

                $(".animationload").show();
                $.ajax({
                    type: "POST",
                    url: PostURL,
                    dataType: "JSON",
                    async: true,
                    data: JSON.stringify(Inputdata),
                    contentType: "application/json; charset=utf-8",
                    success: function (resultData) {
                        $(".animationload").hide();
                        var meassage = '';
                        for (var i = 0; i < resultData.length; i++) {
                            if (resultData[i].ErrorMessage == "success") {
                                if (action == 'close') {
                                    refreshData();
                                    closePopup();
                                }

                                else {
                                    $("#warning").hide();
                                    refreshData();
                                    ResetCustFildes();
                                }
                                break;
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
                    error: function (xhr) {
                        if (xhr.responseText.toLowerCase().indexOf('length') > 0)
                            alert("Please select small size image")
                        else {
                            alert('somthing went wrong!')
                        }
                        // alert(xhr.responseText)
                        $(".animationload").hide();
                    }
                });
            }
            else {
                $("#warning").html("<ul><li>Rear TID must be different from Front TID.</li></ul>");
                $("#warning").show();
            }

        }
        else {
            $("#warning").html("<ul><li>Please fill the mandatory fields</li></ul>");
            $("#warning").show();
        }
    }
    else {
        $("#warning").html("<ul><li>Please fill the mandatory fields</li></ul>");
        $("#warning").show();
    }
}

function HistoryRecords(ctrl, Vechileid, AccountId) {
    Transload = 1;
    CustomerVehicleId = Vechileid;
    CustomerAccountId = AccountId;
    CustomerRegistrationNumber = $(ctrl).parent().parent().parent().parent().find('td:eq(2)').text().trim();
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "TransactionHistory",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $(ctrl).parent().addClass('hide').removeClass('open').hide();
            $('#partialHistory').html(result);
            $("#customerHistoryModal").find("#btnpopupClose").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
            closePopup();
        }

    });

}

function BindHistoryRecords() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { VehicleId: CustomerVehicleId, AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    var count = 0;
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "GetTranscationHistoryByCustomereVehicle",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            CurrentData = data;
            $('#customerHistoryModal').modal({ backdrop: 'static', keyboard: false })
            $('#customerHistoryModal').modal('show');
            $("#HistoryModalLabel").text('View ' + CustomerRegistrationNumber + ' Transaction')
            HNoMoredata = data.length < 10
            Transload++;
            $("#tblCustomerHistoryData").removeClass('my-table-bordered').addClass('table-bordered');
            HdatatableVariable = $('#tblCustomerHistoryData').DataTable({
                data: data,
                "bScrollInfinite": true,
                "bScrollCollapse": false,
                scrollY: "55vh",
                scrollX: true,
                scrollCollapse: false,
                autoWidth: false,
                paging: false,
                info: false,
                searching: false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    {
                        'data': 'MYTRANID'
                    },
                    {
                        'data': 'CREATION_DATE',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.CREATION_DATE != '' && oData.CREATION_DATE != null) {
                                oData.CREATION_DATE = oData.CREATION_DATE.replace('T', ' ');
                                $(nTd).html("" + oData.CREATION_DATE + "");
                            }
                        }

                    },

                    { 'data': 'TRANSACTION_TYPE_NAME' },
                    { 'data': 'VEH_REG_NO' },
                    { 'data': 'VEHICLE_CLASS_NAME' },
                    { 'data': 'PLAZA_NAME' },
                    {
                        'data': 'AMOUNT',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.AMOUNT != '' && oData.AMOUNT != null) {
                                $(nTd).html("<span class='text-right'>" + oData.AMOUNT.toLocaleString('id-ID', {
                                    maximumFractionDigits: 10,
                                    style: 'currency',
                                    currency: 'IDR'
                                }) + "</span>");
                            }

                        }
                    },

                ],
                'columnDefs': [{ "targets": 2, "className": "text-left", }, { "targets": 7, "className": 'dt-body-right', }],
                width: "100%",

            });
            HdatatableVariable.on('order.dt search.dt', function () {
                HdatatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            //$('.modal-body').find('.dataTables_scrollBody').on('scroll', function () {
            $("#tblCustomerHistoryData_wrapper").find('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblCustomerHistoryData").height() - $("#tblCustomerHistoryData_wrapper").find('.dataTables_scrollBody').outerHeight())
                if ($("#tblCustomerHistoryData_wrapper").find('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !HNoMoredata && !inProgress) {
                    AppendHistoryRecords();
                }
            });
            HdatatableVariable.columns.adjust().draw();

            thId = 'tblCustomerHistoryDataTR';
            myVar = setInterval("myclick()", 500);
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function AppendHistoryRecords() {
    inProgress = true;
    $(".animationload").show();
    //var Inputdata = { AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    var Inputdata = { VehicleId: CustomerVehicleId, AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "GetTranscationHistoryByCustomereVehicle",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            HNoMoredata = data.length < 10
            Transload++;
            //datatableVariable.clear().draw();
            HdatatableVariable.rows.add(data); // Add new data
            HdatatableVariable.columns.adjust().draw();
            inProgress = false;
        },
        error: function (ex) {
            $(".animationload").hide();
        }

    });

}

function CustomerDetailsOpen(ctrl, AccountId) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomer?id=" + AccountId,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(ctrl).parent().addClass('hide').removeClass('open').hide();
            $(".animationload").hide();
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("View " + $("#FirstName").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#BirthDate").attr("readonly", false);
            $("#ValidUntil").attr("readonly", false);
            $("#fildset").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());

            if ($("#hfCustomerDocumentPath").val() == '') {
                $("#lblResidentidImagePath").removeAttr('onclick').removeClass('btn-link').addClass('btn-link-disabled');
                $("#lblResidentidImagePath").text('Attach File');
            }

            $("#labelImage").hide();
            $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            $("#imgResidentidImagePath").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            $("#ResidentidImage").hide();
            GetCityList();
            if ($("#hfBirthPlace").val() || '' != '') {
                $('#BirthPlace option').each(function (index, option) {
                    if (option.text == $("#hfBirthPlace").val()) {
                        $(this).attr("selected", "selected");
                    }
                    // option will contain your item
                });
                //$("#BirthPlace option[text='" + $("#hfBirthPlace").val() + "']").attr("selected", "selected");
            }
            $("#lblResidentidImagePath").show();
            $("#imgPreview").hide();


            $('#VehicleModal').find("#btnpopupClose").hide();
            $("#ResidentidImage").hide();
            $("#btnSave").hide();
            $("#btnpopupCancel").hide();
            $("#btnSaveNew").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function myclick() {
    document.getElementById(thId).click();
    document.getElementById(thId).click();
    clearTimeout(myVar);
    setHeight();
    $(".animationload").hide();
}

function FilteCustomerData() {

    var boolfliter = false;
    if ($("#txtCustomerID").val() != '') {
        var numbers = /^[0-9]+$/;
        if (!$("#txtCustomerID").val().match(numbers)) {
            alert('Customer Id should be numeric');
            $("#txtCustomerID").focus();
            return false;
        }
        boolfliter = true;
        CutomerId = $("#txtCustomerID").val();
    }
    else {
        CutomerId = 0;
    }
    if ($("#txtResidentID").val() != '') {
        if (!$("#txtResidentID").val().match(numbers)) {
            alert('Resident Id should be numeric');
            $("#txtResidentID").focus();
            return false;
        }
        boolfliter = true;
        ResidentID = $("#txtResidentID").val();

    }
    else {
        ResidentID = '';
    }
    if ($("#txtName").val() != '') {
        boolfliter = true;
        Name = $("#txtName").val();
    }
    else {
        Name = '';
    }
    if ($("#txtMobile").val() != '') {
        if (!$("#txtMobile").val().match(numbers)) {
            alert('Mobile Phone should be numeric');
            $("#txtMobile").focus();
            return false;
        }
        boolfliter = true;
        Mobile = $("#txtMobile").val();
    }
    else {
        Mobile = '';
    }
    if ($("#txtEmail").val() != '') {
        boolfliter = true;
        EmailId = $("#txtEmail").val();
    }
    else {
        EmailId = '';
    }
    if ($("#txtVRN").val() != '') {
        boolfliter = true;
        VRN = $("#txtVRN").val();
    }
    else {
        VRN = '';
    }
    if ($("#txtVRCN").val() != '') {
        boolfliter = true;
        VRCN = $("#txtVRCN").val();
    }
    else {
        VRCN = '';
    }
    if ($("#ddlVehicleClassId").val() != 0) {
        boolfliter = true;
        VehicleClassId = $("#ddlVehicleClassId").val();
    }
    else {
        VehicleClassId = 0;
    }
    if ($("#ddlQueueStatus").val() != 0) {
        boolfliter = true;
        QueueStatus = $("#ddlQueueStatus").val();
    }
    else {
        QueueStatus = 0;
    }
    if ($("#ddlExceptionFlag").val() != 0) {
        boolfliter = true;
        ExceptionFlag = $("#ddlExceptionFlag").val();
    }
    else {
        ExceptionFlag = 0;
    }
    if (boolfliter) {
        NoMoredata = true;
        var Inputdata = {
            ResidentId: ResidentID,
            MobileNo: Mobile,
            EmailId: EmailId,
            FirstName: Name,
            VehRegNo: VRN,
            AccountId: CutomerId,
            VehicleRCNumber: VRCN,
            VehicleClassId: VehicleClassId,
            QueueStatus: QueueStatus,
            ExceptionFlag: ExceptionFlag,
        }
        $(".animationload").show();
        $.ajax({
            type: "POST",
            url: "CustomerVehicleFilter",
            dataType: "JSON",
            async: true,
            data: JSON.stringify(Inputdata),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                if (data == 'logout') {
                    location.href = "../Login/Logout";
                }
                else if (data == 'failed') {
                    alert("somthing went wrong!");
                }
                else {
                    $('#filterModel').modal('hide');
                    datatableVariable.clear().draw();
                    datatableVariable.rows.add(data); // Add new data
                    datatableVariable.columns.adjust().draw();

                    searchEnable = true;
                }
            },
            error: function (ex) {
                $(".animationload").hide();
            }

        });
    }
    else {
        $('#filterModel').modal('hide');
        searchEnable = false;
        refreshData();
    }
}

function MakeCSV() {
    $(".animationload").show();
    var boolfliter = false;
    if (searchEnable) {
        if (CutomerId || 0 != 0)
            $("#txtCustomerID").val(parseInt(CutomerId));
        $("#txtResidentID").val(ResidentID);
        $("#txtName").val(Name);
        $("#txtMobile").val(Mobile);
        $("#txtEmail").val(EmailId);
        $("#txtVRN").val(VRN);
        $("#txtVRCN").val(VRCN);
        $("#ddlVehicleClassId").val(VehicleClassId);
        $("#ddlQueueStatus").val(QueueStatus);
        $("#ddlExceptionFlag").val(ExceptionFlag);

        if ($("#txtCustomerID").val() != '') {
            var numbers = /^[0-9]+$/;
            if (!$("#txtCustomerID").val().match(numbers)) {
                alert('Customer Id should be numeric');
                $("#txtCustomerID").focus();
                return false;
            }
            boolfliter = true;
            CutomerId = $("#txtCustomerID").val();
        }
        if ($("#txtResidentID").val() != '') {
            if (!$("#txtResidentID").val().match(numbers)) {
                alert('Resident Id should be numeric');
                $("#txtResidentID").focus();
                return false;
            }
            boolfliter = true;
            ResidentID = $("#txtResidentID").val();
        }
        if ($("#txtName").val() != '') {

            boolfliter = true;
            Name = $("#txtName").val();
        }
        if ($("#txtMobile").val() != '') {
            if (!$("#txtMobile").val().match(numbers)) {
                alert('Mobile Phone should be numeric');
                $("#txtMobile").focus();
                return false;
            }
            boolfliter = true;
            Mobile = $("#txtMobile").val();
        }
        if ($("#txtEmail").val() != '') {
            boolfliter = true;
            EmailId = $("#txtEmail").val();
        }
        if ($("#txtVRN").val() != '') {
            boolfliter = true;
            VRN = $("#txtVRN").val();
        }
        if ($("#txtVRCN").val() != '') {
            boolfliter = true;
            VRCN = $("#txtVRCN").val();
        }
        if ($("#ddlVehicleClassId").val() != 0) {
            boolfliter = true;
            VehicleClassId = $("#ddlVehicleClassId").val();
        }
        if ($("#ddlQueueStatus").val() != 0) {
            boolfliter = true;
            QueueStatus = $("#ddlQueueStatus").val();
        }
        if ($("#ddlExceptionFlag").val() != 0) {
            boolfliter = true;
            ExceptionFlag = $("#ddlExceptionFlag").val();
        }
    }
    var Inputdata = {
        ResidentId: ResidentID,
        MobileNo: Mobile,
        EmailId: EmailId,
        FirstName: Name,
        VehRegNo: VRN,
        AccountId: CutomerId,
        VehicleRCNumber: VRCN,
        VehicleClassId: VehicleClassId,
        QueueStatus: QueueStatus,
        ExceptionFlag: ExceptionFlag,
        SearchEnable: searchEnable
    }
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/CSV/ExportCustomerVehicleFilter",
        dataType: "JSON",
        async: true,
        data: JSON.stringify(Inputdata),
        contentType: "application/json; charset=utf-8",
        success: function (Path) {
            $('.animationload').hide();
            if (Path.toLowerCase() == "no data to export." || Path.toLowerCase() == "file exported successfully") {
                alert(Path)
                return;
            }
            if (Path.toLowerCase().search(".csv") > -1 || Path.toLowerCase().search(".pdf") > -1 || Path.toLowerCase().search(".zip") > -1)
                window.location.href = "../Attachment/ExportFiles/" + Path;
            else
                alert(Path);
        },
        error: function (ex) {
            $(".animationload").hide();
        }

    });


}


function ResetFilter() {
    $("#filterModel").find('input:text').val('');
    $("#filterModel").find('input:file').val('');
    $("#filterModel").find('select').val(0);

}

function GetCustomerDetails(ctrl) {
    var ResidentId = $(ctrl).val()

    var FilteredData = $.grep(CustomerAccountJson, function (element, index) {
        if (element.ResidentId == ResidentId) {
            $("#AccountId").val(element.AccountId);
            $("#FirstName").val(element.FirstName);
            $("#Address").val(element.Address);
            $("#MobileNo").val(element.MobileNo);
            $("#EmailId").val(element.EmailId);
        }
    });

}

function openFilterpopupCust() {
    if (CutomerId || 0 != 0)
        $("#txtCustomerID").val(parseInt(CutomerId));
    $("#txtResidentID").val(ResidentID);
    $("#txtName").val(Name);
    $("#txtMobile").val(Mobile);
    $("#txtEmail").val(EmailId);
    $("#txtVRN").val(VRN);
    $("#txtVRCN").val(VRCN);
    $("#ddlVehicleClassId").val(VehicleClassId);
    $("#ddlQueueStatus").val(QueueStatus);
    $("#ddlExceptionFlag").val(ExceptionFlag);
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

function ResetCustFildes() {
    $("#fildset").find('.text-box').val('');
    $("#fildset").find('input:file').val('');
    $("#fildset").find('select').val(0);


    $("#VehicleImageFront").parent().find('br').remove();
    $("#VehicleImageFront").prev().hide();
    $("#VehicleImageFront").next().find('span').text('Attach File');
    $("#VehicleImageFront").parent().find('img').attr('src', '');

    $("#VehicleImageRear").parent().find('br').remove();
    $("#VehicleImageRear").prev().hide();
    $("#VehicleImageRear").next().find('span').text('Attach File');
    $("#VehicleImageRear").parent().find('img').attr('src', '');

    $("#VehicleImageRight").parent().find('br').remove();
    $("#VehicleImageRight").prev().hide();
    $("#VehicleImageRight").next().find('span').text('Attach File');
    $("#VehicleImageRight").parent().find('img').attr('src', '');

    $("#VehicleImageLeft").parent().find('br').remove();
    $("#VehicleImageLeft").prev().hide();
    $("#VehicleImageLeft").next().find('span').text('Attach File');
    $("#VehicleImageLeft").parent().find('img').attr('src', '');

    $("#VehicleRCNumberImagePath").parent().find('br').remove();
    $("#VehicleRCNumberImagePath").prev().hide();
    $("#VehicleRCNumberImagePath").next().find('span').text('Attach File');
    $("#VehicleRCNumberImagePath").parent().find('img').attr('src', '');


    //$("#ResidentId").val(0);
    //$("#AccountId").val('');
}