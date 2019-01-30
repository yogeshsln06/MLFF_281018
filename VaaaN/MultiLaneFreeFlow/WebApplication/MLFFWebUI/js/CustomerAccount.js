var myVar;
var thId;
var pageload = 1;
var Transload = 1;
var pagesize = 30;
var CustomerAccountId = 0;
var CustomerName = '';
var NoMoredata = false;
var HNoMoredata = false;
var inProgress = false;
var CurrentData = [];
var CustomerId = 0;
var searchEnable = false;

var CutomerId = 0;
var ResidentID = '';
var Name = '';
var Mobile = '';
var EmailId = '';
var VRN = '';
var boolfliter = false;



$(document).ready(function () {
    $("#sidebar-toggle").bind("click", function () {
        $(".animationload").show();
        thId = 'tblCustomerDataTR';
        myVar = setInterval("myclick()", 500);
    });
   
    BindCustmerAccount();
});

function closePopup() {
    $("#btnpopupClose").trigger('click');
    $(".modal-backdrop").hide()
}

function reloadData() {
    if (searchEnable) {
        if (CutomerId || 0 != 0)
            $("#txtCustomerID").val(parseInt(CutomerId));
        $("#txtResidentID").val(ResidentID);
        $("#txtName").val(Name);
        $("#txtMobile").val(Mobile);
        $("#txtEmail").val(EmailId);
        $("#txtVRN").val(VRN);
        FilteCustomerData();
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
            url: "CustomerAccountListScroll",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $(".animationload").hide();
                pageload++;
                NoMoredata = data.length < pagesize;
                inProgress = false;
                NoMoredata = false;
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

function BindCustmerAccount() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    var count = 0;
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "CustomerAccountListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            NoMoredata = data.length < pagesize
            pageload++;
            inProgress = false;
            datatableVariable = $('#tblCustomerData').DataTable({
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
                     { 'data': 'AccountId' },
                    {
                        'data': 'AccountId',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a href='javascript:void(0);' onclick='DetailsOpen(this," + oData.AccountId + ")'>" + oData.AccountId + "</a>");
                        }
                    },
                    { 'data': 'ResidentId' },
                    { 'data': 'FirstName' },
                    { 'data': 'Address' },
                    { 'data': 'MobileNo' },
                    { 'data': 'EmailId' },
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
                "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditOpen(this," + oData.AccountId + ")'>" +

                "        <span class='title'>Update</span>" +
                "    </a>" +
                "    <a class='dropdown-item ' href='javascript:void(0);' onclick='HistoryRecords(this," + oData.AccountId + ")'>" +

                "        <span class='title'>Transaction</span>" +
                "    </a>" +
                 "    <a class='dropdown-item ' href='javascript:void(0);' onclick='VehicleRecords(this," + oData.AccountId + ")'>" +

                "        <span class='title'>Vehicle</span>" +
                "    </a>" +
                "</div>" +
            "</div>");
                       }
                   },
                ],
                columnDefs: [
                    { "orderable": false, "targets": 7 },
                    { 'searchable': false, 'targets': [0, 6] }
                ],
                order: [[1, 'asc']],
            });
            datatableVariable.on('order.dt search.dt', function () {
                datatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            $('.dataTables_filter input').attr("placeholder", "Search this list…");
            $('.dataTables_scrollBody').on('scroll', function () {
                var ScrollbarHeight = ($("#tblCustomerData").height() - $('.dataTables_scrollBody').outerHeight())
                if ($('.dataTables_scrollBody').scrollTop() > ScrollbarHeight && ScrollbarHeight > 0 && !NoMoredata && !inProgress && !searchEnable) {
                    AppendCustomerData();
                }
                //if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblCustomerData").height()) && !NoMoredata && !inProgress) {
                //    AppendCustomerData();
                //}
            });
            datatableVariable.columns.adjust();

            thId = 'tblCustomerDataTR';
            myVar = setInterval("myclick()", 500);
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function AppendCustomerData() {
    inProgress = true;
    $(".animationload").show();
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "CustomerAccountListScroll?pageIndex=" + pageload + "&pagesize=" + pagesize,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
            $(".animationload").hide()
            //datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();

        },
        error: function (ex) {
            $(".animationload").hide()
        }

    });

}

function validateCustomer() {
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
    if ($('#ResidentidImage')[0].files.length === 0 && $('#hfCustomerDocumentPath').val() == "") {
        showError($('#ResidentidImage'), "Resident Card Image Required");
        valid = false;
    }
    else {
        showError($("#imagepath"), '');
    }

    return valid;
}

function openpopup() {
    $("#warning").hide();

    $('#customerModal').modal({ backdrop: 'static', keyboard: false })
    $('#customerModal').modal('show');

}

function OpenCustmerRegistration() {
    $.ajax({
        type: "GET",
        url: "/MRM/AssociatedTransaction?transactionId=" + dataid + "&TransactionCategoryId=" + $("#ddlTransactionCategory").val(),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#loader').hide();
            $('#partialassociated').html(result);
            $('#partialassociated').show();
            $('#Associatedmodel').modal('show');

        },
        error: function (x, e) {
            $('#loader').hide();
            $('#Associatedmodel').modal('hide');
        }

    });
}

function NewCustomer() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "/Registration/NewCustomer",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#exampleModalLabel").text("Register New Customer");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#lblResidentidImagePath").hide();
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#BirthDate").attr("data-provide", "datepicker").attr("readolny", true);
            //$(".form_datetime").datepicker({
            //    dateFormat: "mm/dd/yy",
            //    showOtherMonths: true,
            //    selectOtherMonths: true,
            //    changeMonth: true,
            //    changeYear: true,
            //}).attr("readolny", true);
            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnpopupClose").text('Cancel').removeClass('btn-outline-secondary').addClass('btn-outline-danger').show();
            $("#btnpopupCancel").removeClass('btn-outline-danger').addClass('btn-outline-secondary').hide();
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
        url: "/Registration/GetCustomer?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
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

            $("#btnSave").hide();

            //$("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
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
    CustomerId = id || 0
    if (CustomerId == 0)
        CustomerId = $("#AccountId").val();
    $('#partialassociated').html("");
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomer?id=" + CustomerId,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#exampleModalLabel").text("Update " + $("#FirstName").val() + "");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#ResidentId").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());
            $("#imgPreview").hide();
            if ($("#hfCustomerDocumentPath").val() == '') {
                $("#ResidentidImage").show();
                $("#lblResidentidImagePath").hide();
            }
            else {
                $("<br/>").insertBefore($("#labelImage"));
                $("#labelImage").find('span').text('Update File');
                //$("#labelImage").prepend("<br/>");
                $("#ResidentidImage").hide();
                $("#lblResidentidImagePath").show();
                $("#imgResidentidImagePath").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            }

            // $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            GetCityList();

            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#BirthDate").attr("data-provide", "datepicker").attr("readolny", true);
            //$(".form_datetime").datepicker({
            //    dateFormat: "mm/dd/yy",
            //    showOtherMonths: true,
            //    selectOtherMonths: true,
            //}).attr("readolny", true);
            if ($("#hfBirthPlace").val() || '' != '') {
                $('#BirthPlace option').each(function (index, option) {
                    if (option.text == $("#hfBirthPlace").val()) {
                        $(this).attr("selected", "selected");
                    }
                    // option will contain your item
                });
                //$("#BirthPlace option[text='" + $("#hfBirthPlace").val() + "']").attr("selected", "selected");
            }

            $("#btnSave").show();
            //$("#btnSave").text("Update");
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger').addClass('').text('Cancel').show();
            $("#btnpopupUpdateCancel").hide();
            $("#btnSaveNew").hide();
            $("#btnpopupCancel").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function SaveData(action) {
    if ($("#needs-validation").valid()) {
        var BirthDate = $('#BirthDate').val() || ''
        if (BirthDate != '') {
            BirthDate = DateFormat(BirthDate);
        }

        var ValidUntil = $('#ValidUntil').val() || ''
        if (ValidUntil != '') {
            ValidUntil = DateFormat(ValidUntil);
        }

        if (validateCustomer()) {
            var AccountId = $("#AccountId").val() || '';
            var PostURL = '/Registration/CustomerUpdate';
            var ImagePath = $("#ResidentidImagePath").text().trim();
            if (AccountId == '') {
                AccountId = 0;
                PostURL = '/Registration/CustomerAdd'
            }
            else {
                if ($("#ResidentidImagePath").text().trim() == '') {
                    ImagePath = $("#hfCustomerDocumentPath").val();
                    PostURL = '/Registration/CustomerUpdate?ImageChnage=' + false;
                }
                else {
                    ImagePath = $("#ResidentidImagePath").text().trim();
                    PostURL = '/Registration/CustomerUpdate?ImageChnage=' + true;
                }
            }
            var Inputdata = {
                AccountId: AccountId,
                ResidentId: $("#ResidentId").val(),
                FirstName: $("#FirstName").val(),
                BirthPlace: $('#BirthPlace option:selected').text(),
                BirthDate: BirthDate,
                Nationality: $('#Nationality').val(),
                Gender: $('#Gender').val(),
                MaritalStatus: $('#MaritalStatus').val(),
                Occupation: $('#Occupation').val(),
                ValidUntil: ValidUntil,
                MobileNo: $("#MobileNo").val(),
                EmailId: $("#EmailId").val(),
                ProvinceId: $("#ProvinceId").val(),
                CityId: $("#CityId").val(),
                DistrictId: $("#DistrictId").val(),
                SubDistrictId: $("#SubDistrictId").val(),
                RT: $("#RT").val(),
                RW: $("#RW").val(),
                Address: $("#Address").val(),
                PostalCode: $("#PostalCode").val(),
                ResidentidcardImagePath: ImagePath
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
                                reloadData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetCustomerFildes();
                            }
                            break;
                        }
                        else if (resultData[i].ErrorMessage == 'logout') {
                            location.href = "../Login/Logout";
                            break;
                        }
                        else {
                            if (resultData[i].ErrorMessage == "Resident Id already exists") {
                                alert("Resident Id already exists");
                                $("#warning").hide();
                                // ResetCustomerFildes();
                                break;
                            }
                            else {
                                meassage = meassage + "<li>" + resultData[i].ErrorMessage + "</li>"
                            }
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

function DateFormat(newDate) {
    var d = new Date(newDate);
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    return dd + '-' + mm + '-' + +yy
}

function HistoryRecords(ctrl, AccountId) {
    Transload = 1;
    CustomerAccountId = AccountId;
    CustomerName = $(ctrl).parent().parent().parent().parent().find('td:eq(3)').text().trim();
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
    var Inputdata = { AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    var count = 0;
    $.ajax({

        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "GetTranscationHistoryByCustomer",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            CurrentData = data;
            $('#customerHistoryModal').modal({ backdrop: 'static', keyboard: false })
            $('#customerHistoryModal').modal('show');
            $("#HistoryModalLabel").text('View ' + CustomerName + ' Transaction')
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
                    "targets": 2,
                    "className": "text-left",
                },
                {
                    "targets": 7,
                    "className": 'dt-body-right',
                }
                ],
            });
            HdatatableVariable.on('order.dt search.dt', function () {
                HdatatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            $('.modal-body').find('.dataTables_scrollBody').on('scroll', function () {
                console.log($('.modal-body').find('.dataTables_scrollBody').length);
                if (($('.modal-body').find('.dataTables_scrollBody').scrollTop() + $('.modal-body').find('.dataTables_scrollBody').height() >= $("#tblCustomerHistoryData").height()) && !HNoMoredata) {
                    AppendHistoryRecords();
                }
            });
            HdatatableVariable.columns.adjust().draw();
            thId = 'tblCustomerHistoryDataTR';
            myVar = setInterval("myclick()", 500);
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });

}

function AppendHistoryRecords() {
    $(".animationload").show()
    var Inputdata = { AccountId: CustomerAccountId, pageindex: Transload, pagesize: 10 }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "GetTranscationHistoryByCustomer",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide()
            HNoMoredata = data.length < 10
            Transload++;
            //datatableVariable.clear().draw();
            HdatatableVariable.rows.add(data); // Add new data
            HdatatableVariable.columns.adjust().draw();

        },
        error: function (ex) {
            $(".animationload").hide()
        }

    });

}

function VehicleRecords(ctrl, AccountId) {
    Transload = 1;
    CustomerAccountId = AccountId;
    CustomerName = $(ctrl).parent().parent().parent().parent().find('td:eq(3)').text().trim();
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "VehicleListData",
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

function BindCustmerVehicleAccount() {
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { AccountId: CustomerAccountId }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "GetVehicleListByAccount",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tblVehicleList").removeClass('my-table-bordered').addClass('table-bordered');
            $('#customerHistoryModal').modal({ backdrop: 'static', keyboard: false })
            $('#customerHistoryModal').modal('show');
            $("#HistoryModalLabel").text('View ' + CustomerName + ' Vehicle')

            datatableVariableVehicle = $('#tblVehicleList').DataTable({
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
                    { 'data': 'EntryId' },
                    {
                        'data': 'EntryId',

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
                    {
                        'data': 'AccountBalance',
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            if (oData.AccountBalance != '' && oData.AccountBalance != null) {
                                if (oData.AccountBalance < 0) {
                                    $(nTd).html("<span class='text-right red'>(" + ((oData.AccountBalance) * (-1)).toLocaleString('id-ID', {
                                        maximumFractionDigits: 0,
                                        style: 'currency',
                                        currency: 'IDR'
                                    }) + ")</span>");
                                }
                                else {
                                    $(nTd).html("<span class='text-right'>" + oData.AccountBalance.toLocaleString('id-ID', {
                                        maximumFractionDigits: 0,
                                        style: 'currency',
                                        currency: 'IDR'
                                    }) + "</span>");
                                }
                            }

                        }
                    },
                ],
                columnDefs: [{ "orderable": false, "targets": 6 },
                 { "targets": 8, "className": 'dt-body-right', }],
                order: [[1, 'asc']],
                width: "100%"
            });
            datatableVariableVehicle.on('order.dt search.dt', function () {
                datatableVariableVehicle.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            thId = 'tblVehicleListTR';
            myVar = setInterval("myclick()", 500);
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

function FilteCustomerData() {
    boolfliter = false;
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
    if (boolfliter) {
        NoMoredata = true;
        var Inputdata = {
            ResidentId: ResidentID,
            MobileNo: Mobile,
            EmailId: EmailId,
            FirstName: Name,
            VehRegNo: VRN,
            AccountId: CutomerId
        }
        $(".animationload").show();
        $.ajax({
            type: "POST",
            url: "CustomerAccountFilter",
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
                    searchEnable = true;
                    datatableVariable.clear().draw();
                    datatableVariable.rows.add(data); // Add new data
                    datatableVariable.columns.adjust().draw();
                }
            },
            error: function (ex) {
                $(".animationload").hide();
            }

        });
    }
    else {
        searchEnable = false;
        $('#filterModel').modal('hide');
        // alert('At least one option must be fill for search !');
        reloadData();
    }
}

function MakeCSV() {
    $(".animationload").show();
    if (searchEnable) {
        if (CutomerId || 0 != 0)
            $("#txtCustomerID").val(parseInt(CutomerId));
        $("#txtResidentID").val(ResidentID);
        $("#txtName").val(Name);
        $("#txtMobile").val(Mobile);
        $("#txtEmail").val(EmailId);
        $("#txtVRN").val(VRN);
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
    }
    var Inputdata = {
        ResidentId: ResidentID,
        MobileNo: Mobile,
        EmailId: EmailId,
        FirstName: Name,
        VehRegNo: VRN,
        AccountId: CutomerId,
        SearchEnable: searchEnable
    }
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/CSV/ExportCustomerAccountFilter",
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
    //reloadData();
}

function openImg(ctrl) {
    var id = $(ctrl).attr('id')
    $("#" + id.replace('lbl', 'img')).trigger('click');
}

function ResetCustomerFildes() {
    $("#fildset").find('.text-box').val('');
    $("#fildset").find('input:file').val('');
    $("#ResidentidImage").parent().find('br').remove();
    $("#ResidentidImage").prev().hide();
    $("#ResidentidImage").next().find('span').text('Attach File');
    $("#ResidentidImage").parent().find('img').attr('src', '');
}

function openFilterpopupCust() {
    if (CutomerId || 0 != 0)
        $("#txtCustomerID").val(parseInt(CutomerId));
    $("#txtResidentID").val(ResidentID);
    $("#txtName").val(Name);
    $("#txtMobile").val(Mobile);
    $("#txtEmail").val(EmailId);
    $("#txtVRN").val(VRN);

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