var pageload = 1;
var Transload = 1;
var pagesize = 30;
var NoMoredata = false;
var inProgress = false;
var CustomerVehicleId = 0;
var CustomerAccountId = 0;
var CustomerRegistrationNumber = '';

$(document).ready(function () {
    BindCustmerVehicleAccount();
});

function refreshData() {
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

function closePopup() {
    $("#btnpopupClose").trigger('click');
    $(".modal-backdrop").hide()
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
            $("#tblCustomerVehicle").removeClass('my-table-bordered').addClass('table-bordered');
            $(".animationload").hide();
            pageload++;
            datatableVariable = $('#tblCustomerVehicle').DataTable({
                data: data,
                "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
                "bScrollInfinite": true,
                "bScrollCollapse": true,
                scrollY: "48vh",
                pageResize: true,
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
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openImagePreview(this);' style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageFront + "><i class='c-blue-500 ti-camera'></i></span>");
                            }
                        }
                    },
                    {
                        'data': 'VehicleImageRear', "className": "dt-center",
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.VehicleImageRear != '' && oData.VehicleImageRear != null) {
                                $(nTd).html("<span class='cur-p icon-holder' aria-expanded='false' onclick='openImagePreview(this);' style='font-size: 12px;' src=../Attachment/VehicleImage/" + oData.VehicleImageRear + "><i class='c-blue-500 ti-camera'></i></span>");
                            }
                        }
                    },
                    { 'data': 'CustomerQueueStatusName' },
                    { 'data': 'ExceptionFlagName' },
                    { 'data': 'AccountBalance' },
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
                "        <span class='title'>Transactions</span>" +
                "    </a>" +
                "    <a class='dropdown-item ' href='javascript:void(0);' onclick='CustomerDetailsOpen(this," + oData.AccountId + ")'>" +
                "        <span class='title'>Customer</span>" +
                "    </a>" +
                "</div>" +
            "</div>");
                        }
                    },
                ],
                columnDefs: [{ "orderable": false, "targets": 7 }],
                order: [[1, 'asc']],
                width: "100%"
            });
            $('.dataTable').css('width', '1200px !important');
            datatableVariable.on('order.dt search.dt', function () {
                datatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            inProgress = false;
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblCustomerVehicle").height()) && !NoMoredata && !inProgress) {
                    AppendCustomerData();
                }
            });
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
            //datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $('#loadingdiv').hide()
        }

    });
}

function openpopup() {
    $("#warning").hide();
    $('#customerModal').modal('show');
}

function validTAGId(str) {
    var pattern = /^[0-9a-fA-F]+$/;
    return String(str).match(pattern);
}

function validateCustomerVehicle() {
    var valid = true;

    if ($("#ResidentId").val() == '') {
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

    if ($("#TagId").val() == '') {
        showError($("#TagId"), $("#TagId").attr('data-val-required'));
        valid = false;
    }
    else if ($("#TagId").val().length != 24) {
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
            $("#exampleModalLabel").text("New Vehicle");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
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
            $("#btnpopupClose").text("Cancel");
            $("#btnSaveNew").show();
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
            $("#exampleModalLabel").text("View [" + $('#VehRegNo').val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $(".animationload").hide();
            $("#fildset").attr("disabled", "disabled");
            $("#VehicleImageFront").hide();
            $("#VehicleImageRear").hide();
            $("#VehicleImageRight").hide();
            $("#VehicleImageLeft").hide();
            $("#VehicleRCNumberImagePath").hide();

            $("#lblVehicleImageFront").show();
            $("#lblVehicleImageRear").show();
            $("#lblVehicleImageRight").show();
            $("#lblVehicleImageLeft").show();
            $("#lblVehicleRCNumberImagePath").show();

            $("#imgVehicleImageFront").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageFront").val());
            $("#imgVehicleImageRear").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRear").val());
            $("#imgVehicleImageRight").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageRight").val());
            $("#imgVehicleImageLeft").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleImageLeft").val());
            $("#imgVehicleRCNumberImagePath").attr('src', "../Attachment/VehicleImage/" + $("#hfVehicleRCNumberImage").val());

            $("#btnSave").hide();
            $("#btnpopupClose").text("Close");
            $("#btnSaveNew").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function EditOpen(ctrl, id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomerVehicle?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("Update [" + $('#VehRegNo').val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#EntryIdId").attr("disabled", "disabled");
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
            $("#btnSave").text("Update");
            $("#btnpopupClose").text("Close");
            $("#btnSaveNew").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function SaveData(action) {
    if ($("#needs-validation").valid()) {
        var ValidUntil = $('#ValidUntil').val() || ''
        if (ValidUntil != '') {
            ValidUntil = DateFormat(ValidUntil);
        }

        if (validateCustomerVehicle()) {
            var EntryId = $("#EntryId").val() || '';
            var AccountId = $("#AccountId").val() || '';
            var ImageFrontChnage = true;
            var ImageRearChnage = true;
            var ImageLeftChnage = true;
            var ImageRightChnage = true;
            var RCNumberImageChnage = true;
            var PostURL = '/Registration/CustomerVehicleUpdate';
            var VehicleImageFront = $("#VehicleImageFrontPath").text().trim();
            var VehicleImageRear = $("#VehicleImageRearPath").text().trim();
            var VehicleImageRight = $("#VehicleImageRightPath").text().trim();
            var VehicleImageLeft = $("#VehicleImageLeftPath").text().trim();
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
                            if (action == 'close')
                                closePopup();

                            else {
                                $("#warning").hide();
                                ResetFildes();
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
                error: function (ex) {
                    $(".animationload").hide();
                }
            });
        }
    }
}

function HistoryRecords(ctrl, Vechileid, AccountId) {
    Transload = 1;
    CustomerVehicleId = Vechileid;
    CustomerAccountId = AccountId;
    CustomerRegistrationNumber = $(ctrl).parent().parent().parent().parent().find('td:eq(3)').text().trim();
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "TransactionHistory",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialHistory').html(result);
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
            $(".animationload").hide();
            $('#customerHistoryModal').modal('show');
            $("#HistoryModalLabel").text('View [' + CustomerRegistrationNumber + '] Transaction')
            HNoMoredata = data.length < 10
            Transload++;
            $("#tblCustomerHistoryData").removeClass('my-table-bordered').addClass('table-bordered');
            HdatatableVariable = $('#tblCustomerHistoryData').DataTable({
                data: data,
                paging: false,
                info: false,
                pageResize: true,
                autoWidth: false,
                searching: false,
                scrollCollapse: true,
                stateSave: true,
                "bScrollCollapse": true,
                "bScrollInfinite": true,
                "bAutoWidth": false,
                columns: [
                    { 'data': 'ROWNUMBER' },
                    {
                        'data': 'ENTRY_ID'
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
                    { 'data': 'AMOUNT' },

                ],
                width: "100%",
                scrollY: "55vh",
            });
            HdatatableVariable.on('order.dt search.dt', function () {
                HdatatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            //$('.modal-body').find('.dataTables_scrollBody').on('scroll', function () {
            //    if (($('.modal-body').find('.dataTables_scrollBody').scrollTop() + $('.modal-body').find('.dataTables_scrollBody').height() >= $("#tblCustomerHistoryData").height()) && !HNoMoredata) {
            //        AppendHistoryRecords();
            //    }
            //});
            HdatatableVariable.columns.adjust().draw();

            HdatatableVariable.clear().draw();
            HdatatableVariable.rows.add(CurrentData); // Add new data
            HdatatableVariable.columns.adjust().draw();

        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function AppendHistoryRecords() {
    $('#loadingdiv').show()
    var Inputdata = { AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "GetTranscationHistoryByCustomer",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#loadingdiv').hide()
            HNoMoredata = data.length < 10
            //datatableVariable.clear().draw();
            HdatatableVariable.rows.add(data); // Add new data
            HdatatableVariable.columns.adjust().draw();
            Transload++;
        },
        error: function (ex) {
            $('#loadingdiv').hide()
        }

    });

}

function CustomerDetailsOpen(ctrl,AccountId) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomer?id=" + AccountId,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("View [" + $("#FirstName").val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();

            $("#fildset").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());
            $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
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

            $("#btnSave").hide();
            $("#btnpopupClose").text("Close");
            $("#btnSaveNew").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}