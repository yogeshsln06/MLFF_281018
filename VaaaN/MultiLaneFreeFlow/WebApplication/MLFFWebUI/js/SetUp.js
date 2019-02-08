﻿/*User List */
//function openpopup() {
//    $("#warning").hide();
//    $('#customerModal').modal('show');
//}
function BindUserData(UserDataList) {
    tblUserData = $('#tblUserData').DataTable({
        data: UserDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'UserId',
                 orderable: false
             },
             {
                 'data': 'UserId',
                 fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                     $(nTd).html("<a href='javascript:void(0);' onclick='UserDetail(this," + oData.UserId + ")'>" + oData.UserId + "</a>");
                 }
             },
           { 'data': 'LoginName', },
           { 'data': 'FirstName', },
           { 'data': 'MobileNo', },
           { 'data': 'EmailId', },
           { 'data': 'RoleName', },
            { 'data': 'UserDob', },
           {'data': 'CreationDate',},
           { 'data': 'AccountExpiryDate', },
           {
               'data': 'UserStatus',
               fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   if (oData.UserStatus == true) {
                       $(nTd).html("Active");
                   }
                   else {
                       $(nTd).html("Deactive");
                   }
               }
           },
           {
               'data': 'UserId',
               fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                       "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
           "<span class='icon-holder'>" +
               "<i class='c-blue-500 ti-menu-alt'></i>" +
           "</span>" +
       "</a>" +
       " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
       "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditUser(this," + oData.UserId + ")'>" +

       "        <span class='title'>Update</span>" +
       "    </a>" +
       "</div>" +
   "</div>");
               }
           },
        ],
        //columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblUserData.on('order.dt search.dt', function () {
        tblUserData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblUserData.columns.adjust();
    thId = 'tblUserDataTR';
    myVar = setInterval("myclick()", 500);
}
function NewUser() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "UserNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New User");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#UserId").val('0').attr("disabled", "disabled");
            $("#Password").val('');
            $("#LoginName").val('');
            //$("#AccountExpiryDate").val("");
            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function EditUser(ctrl, id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetUser?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
           $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#FirstName").val() + "]");
            openpopup();
            $("#UserId").attr("disabled", "disabled");
            $("#LoginName").attr("disabled", "disabled");
            $("#Password").attr("disabled", "disabled");
            $("#RoleId").val($("#SlRoleId").val());
            
            $("#btnSave").show();
            $("#btnSave").text("Update");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function SaveData(action) {
    if ($("#needs-validation").valid()) {
        var UserDob = $('#UserDob').val() || ''
        if (UserDob != '') {
            UserDob = DateFormat(UserDob);
        }
        var AccountExpiryDate = $('#AccountExpiryDate').val() || ''
        if (AccountExpiryDate != '') {
            AccountExpiryDate = DateFormat(AccountExpiryDate);
        }
        if (validateUser()) {
            var UserId = $("#UserId").val() || '';
            var PostURL = '/SetUp/UserUpdate';
           // var ImagePath = $("#ResidentidImagePath").text().trim();
            if (UserId == 0) {
                UserId = 0;
                PostURL = '/SetUp/AddUser'
            }
            var Inputdata = {
                UserId: UserId,
                LoginName: $("#LoginName").val(),
                Password: $("#Password").val(),
                FirstName: $('#FirstName').val(),
                LastName: $('#LastName').val(),
                UserDob: $('#UserDob').val(),
                Address: $("#Address").val(),
                MobileNo: $("#MobileNo").val(),
                EmailId: $("#EmailId").val(),
                AccountExpiryDate: $("#AccountExpiryDate").val(),
                RoleId: $("#RoleId").val(),
                UserStatus: $("#UserStatus").val(),
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
                                $('#needs-validation')[0].reset();
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
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}
function validateUser() {
    var valid = true;
    if ($("#UserId").val() == '') {
        showError($("#UserId"), $("#UserId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#UserId"), '');
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
    //if ($("#Address").val() == '') {
    //    showError($("#Address"), $("#Address").attr('data-val-required'));
    //    valid = false;
    //}
    //else {
    //    showError($("#Address"), '');
    //}
    return valid;
}

/********Vehicle Classification Start**********/
function validateClass() {
    var valid = true;
    if ($("#Name").val() == '') {
        showError($("#Name"), $("#Name").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ResidentId"), '');
    }
    return valid;
}

function BindClassfificationData(VehicleClassJson) {
    tblVehicleClassData = $('#tblVehicleClassData').DataTable({
        data: VehicleClassJson,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             { 'data': 'Id' },
            {
                'data': 'Id',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='VehcileClassDetail(this," + oData.Id + ")'>" + oData.Id + "</a>");
                }
            },
            { 'data': 'Name' },
           {
               'data': 'Id',
               fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                      "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
           "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditVehcileClass(this," + oData.Id + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
               }
           },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblVehicleClassData.on('order.dt search.dt', function () {
        tblVehicleClassData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblVehicleClassData.columns.adjust();
    thId = 'tblVehicleClassDataTR';
    myVar = setInterval("myclick()", 500);
}

function reloadClassfificationData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "ClassificationReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblVehicleClassData.clear().draw();
            tblVehicleClassData.rows.add(JSON.parse(data));
            tblVehicleClassData.columns.adjust().draw();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function NewClassfification() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "NewClassification",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Classification");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#Id").attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").hide();
            $("#btnCancel").show();
            //$("#btnSaveNew").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function SaveClassfificationData(action) {
    if ($("#needs-validation").valid()) {
        if (validateClass()) {
            var Id = $("#Id").val() || '';
            var PostURL = '/SetUp/UpdateClassification';
            if (Id == '') {
                Id = 0;
                PostURL = '/SetUp/AddClassification'
            }
            var Inputdata = {
                Id: Id,
                Name: $("#Name").val()
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
                                reloadClassfificationData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetFieldes();
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
        else {
            $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
            $("#warning").show();
        }

    }
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}

function EditVehcileClass(ctrl, Id) {
    $(ctrl).parent().addClass('hide').removeClass('open').hide();
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetClassification?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("View " + $("#Name").val() + "");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#Id").attr("disabled", "disabled");
            $("#btnSave").show();
            $("#btnClose").hide();
            $("#btnUpdateCancel").show();
            $("#btnCancel").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function VehcileClassDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetClassification?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $("#SetUpLabel").text("View " + $("#Name").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#Id").attr("disabled", "disabled");
            $("#btnSave").hide();
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
            $("#btnpopupClose").show();
            $("#btnCancel").hide();
            $("#btnUpdate").hide();
            $("#btnUpdateCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

/********Vehicle Classification END**********/

/********Hardware Start**********/
function validateHardware() {
    var valid = true;
    if ($("#HardwareName").val() == '') {
        showError($("#HardwareName"), $("#HardwareName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#HardwareName"), '');
    }
    if ($("#HardwareType").val() == 0) {
        showError($("#HardwareType"), $("#HardwareType").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#HardwareType"), '');
    }
    if ($("#HardwarePosition").val() == 0) {
        showError($("#HardwarePosition"), $("#HardwarePosition").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#HardwarePosition"), '');
    }
    return valid;
}
function BindHardwareData(HardwareDataList) {
    tblHardwareData = $('#tblHardwareData').DataTable({
        data: HardwareDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
            { 'data': 'HardwareId' },
            {
                'data': 'HardwareId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='HardwareDetail(this," + oData.HardwareId + ")'>" + oData.HardwareId + "</a>");
                }
            },
            { 'data': 'HardwareName' },
            { 'data': 'HardwareType' },
            { 'data': 'HardwarePosition' },
            { 'data': 'ManufacturerName' },
            { 'data': 'ModelName' },
            { 'data': 'IpAddress' },
            {
                'data': 'HardwareId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditHardware(this," + oData.HardwareId + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblHardwareData.on('order.dt search.dt', function () {
        tblHardwareData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblHardwareData.columns.adjust();
    thId = 'tblHardwareDataTR';
    myVar = setInterval("myclick()", 500);
}

function reloadHardwareData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "ClassificationReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblHardwareData.clear().draw();
            tblHardwareData.rows.add(JSON.parse(data));
            tblHardwareData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function NewHardware() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "HardwareNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Hardware");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#HardwareId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").hide();
            $("#btnCancel").show();
            //$("#btnSaveNew").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function SaveHardwareData(action) {
    if ($("#needs-validation").valid()) {
        if (validateHardware()) {
            var Id = $("#HardwareId").val() || '';
            var PostURL = '/SetUp/HardwareUpdate';
            if (Id == '') {
                Id = 0;
                PostURL = '/SetUp/HardwareAdd'
            }
            var Inputdata = {
                HardwareId: Id,
                HardwareName: $("#HardwareName").val(),
                HardwareType: $("#HardwareType").val(),
                HardwarePosition: $("#HardwarePosition").val(),
                ManufacturerName: $("#ManufacturerName").val(),
                ModelName: $("#ModelName").val(),
                IpAddress: $("#IpAddress").val(),
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
                                reloadClassfificationData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetFieldes();
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
        else {
            $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
            $("#warning").show();
        }

    }
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}

function EditHardware(ctrl, Id) {
    $(ctrl).parent().addClass('hide').removeClass('open').hide();
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetHardware?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Update " + $("#Name").val() + "");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#HardwareId").attr("disabled", "disabled");
            $("#HardwareType").val($("#hfHardwareType").val());
            $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").show();
            $("#btnClose").hide();
            $("#btnUpdateCancel").show();
            $("#btnCancel").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function HardwareDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetHardware?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $("#SetUpLabel").text("View " + $("#Name").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#HardwareId").attr("disabled", "disabled");
            $("#HardwareType").val($("#hfHardwareType").val());
            $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").hide();
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
            $("#btnpopupClose").show();
            $("#btnCancel").hide();
            $("#btnUpdate").hide();
            $("#btnUpdateCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

/********Hardware End**********/

/********Lane Start**********/
function validateLane() {
    var valid = true;
    if ($("#LaneName").val() == '') {
        showError($("#LaneName"), $("#LaneName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#LaneName"), '');
    }
    if ($("#PlazaId").val() == 0) {
        showError($("#PlazaId"), $("#PlazaId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#PlazaId"), '');
    }
    if ($("#CameraNameFront").val() == 0) {
        showError($("#CameraNameFront"), $("#CameraNameFront").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#CameraNameFront"), '');
    }
    if ($("#CameraIdRear").val() == 0) {
        showError($("#CameraIdRear"), $("#CameraIdRear").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#CameraIdRear"), '');
    }
    if ($("#EtcReaderId").val() == 0) {
        showError($("#EtcReaderId"), $("#EtcReaderId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#EtcReaderId"), '');
    }
    return valid;
}
function BindLaneData(LaneDataList) {
    tblLaneData = $('#tblLaneData').DataTable({
        data: LaneDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
            {
                'data': 'LaneId',
                orderable: false
            },
            {
                'data': 'LaneId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='LaneDetail(this," + oData.LaneId + ")'>" + oData.LaneId + "</a>");
                }
            },
            { 'data': 'LaneName' },
            { 'data': 'PlazaId' },
            { 'data': 'PlazaName' },
            { 'data': 'CameraIdFront' },
            { 'data': 'CameraNameFront' },
            { 'data': 'CameraIdRear' },
            { 'data': 'CameraNameRear' },
            { 'data': 'AntennaIdFront' },
            { 'data': 'EtcAntennaNameFront' },
            { 'data': 'AntennaIdRear' },
            { 'data': 'EtcAntennaNameRear' },
            {
                'data': 'LaneTypeId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    if (oData.LaneTypeId == "1") {
                        $(nTd).html("Normal");
                    }
                    else
                    {
                        $(nTd).html("Trans");
                    }
                  
                }
            },
            {
                'data': 'LaneId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditLane(this," + oData.LaneId + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblLaneData.on('order.dt search.dt', function () {
        tblLaneData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblLaneData.columns.adjust();
    thId = 'tblLaneDataTR';
    myVar = setInterval("myclick()", 500);
}

function reloadLaneData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "ClassificationReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblHardwareData.clear().draw();
            tblHardwareData.rows.add(JSON.parse(data));
            tblHardwareData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function NewLane() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "LaneNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Lane");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#LaneId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").hide();
            $("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function SaveLaneData(action) {
    if ($("#needs-validation").valid()) {
        if (validateLane()) {
            var Id = $("#LaneId").val() || '';
            var PostURL = '/SetUp/LaneUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/LaneAdd'
            }
            var Inputdata = {
                LaneId: Id,
                PlazaId: $("#PlazaId").val(),
                LaneName: $("#LaneName").val(),
                LaneTypeId: $("#LaneTypeId").val(),
                CameraIdFront: $("#CameraIdFront").val(),
                CameraIdRear: $("#CameraIdRear").val(),
                EtcReaderId: $("#EtcReaderId").val(),
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
                                reloadClassfificationData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetFieldes();
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
        else {
            $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
            $("#warning").show();
        }
    }
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}

function EditLane(ctrl, Id) {
    $(ctrl).parent().addClass('hide').removeClass('open').hide();
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetLane?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
           
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#LaneName").val() + "]");
            openpopup();
            $("#LaneId").attr("disabled", "disabled");
            $("#btnSave").show();
            $("#btnClose").hide();
            $("#btnUpdateCancel").show();
            $("#btnCancel").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function LaneDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetHardware?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $("#SetUpLabel").text("View " + $("#Name").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#HardwareId").attr("disabled", "disabled");
            $("#HardwareType").val($("#hfHardwareType").val());
            $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").hide();
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
            $("#btnpopupClose").show();
            $("#btnCancel").hide();
            $("#btnUpdate").hide();
            $("#btnUpdateCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

/********Lane End**********/

//******Gantry Start ******
function validateGantry() {
    var valid = true;
    if ($("#PlazaName").val() == '') {
        showError($("#PlazaName"), $("#PlazaName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#PlazaName"), '');
    }
    if ($("#Location").val() == '') {
        showError($("#Location"), $("#Location").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#Location"), '');
    }
    if ($("#IpAddress").val() == 0) {
        showError($("#IpAddress"), $("#IpAddress").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#IpAddress"), '');
    }
    return valid;
}

function BindGantryData(GantryDataList) {
    tblGantryData = $('#tblGantryData').DataTable({
        data: GantryDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'PlazaId',
                 orderable: false
             },
            {
                'data': 'PlazaId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='LaneDetail(this," + oData.PlazaId + ")'>" + oData.PlazaId + "</a>");
                }
            },
            { 'data': 'PlazaName' },
            { 'data': 'Location' },
            { 'data': 'IpAddress' },
            {
                'data': 'PlazaId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditGantry(this," + oData.PlazaId + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblGantryData.on('order.dt search.dt', function () {
        tblGantryData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblGantryData.columns.adjust();
    thId = 'tblGantryDataTR';
    myVar = setInterval("myclick()", 500);
}

function EditGantry(ctrl, Id) {
    $(ctrl).parent().addClass('hide').removeClass('open').hide();
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetGantry?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#PlazaName").val() + "]");
            $("#PlazaId").attr("disabled", "disabled");
            openpopup();
           
            $("#btnSave").show();
            $("#btnSave").text("Update");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function NewGantry() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "GantryNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Plaza");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#PlazaId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function SaveGantryData(action) {
    if ($("#needs-validation").valid()) {
        if (validateGantry()) {
            var Id = $("#PlazaId").val() || '';
            var PostURL = '/SetUp/GantryUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/GantryAdd'
            }
            var Inputdata = {
                PlazaId: Id,
                PlazaName: $("#PlazaName").val(),
                Location: $("#Location").val(),
                IpAddress: $("#IpAddress").val(),
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
                                reloadClassfificationData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetFieldes();
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
        else {
            $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
            $("#warning").show();
        }
    }
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}
/********Gantry End**********/

//******Roles Start ******

function validateRole() {
    var valid = true;
    if ($("#RoleName").val() == '') {
        showError($("#RoleName"), $("#RoleName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#RoleName"), '');
    }
    return valid;
}

function BindRolesData(RolesDataList) {
    tblRolesData = $('#tblRolesData').DataTable({
        data: RolesDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'RoleId',
                 orderable: false
             },
            {
                'data': 'RoleId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='RolesDetail(this," + oData.RoleId + ")'>" + oData.RoleId + "</a>");
                }
            },
            {
                'data': 'RoleName',
            },
            {
                'data': 'ISActive',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                if (oData.ISActive == 1) {
                $(nTd).html("Active");
                }
                else {
                $(nTd).html("Deactive");
                }
      }},
            {
                'data': 'RoleId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditRoles(this," + oData.RoleId + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblRolesData.on('order.dt search.dt', function () {
        tblRolesData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblRolesData.columns.adjust();
    thId = 'tblRolesDataTR';
    myVar = setInterval("myclick()", 500);
}

function EditRoles(ctrl, Id) {
    //$(ctrl).parent().addClass('hide').removeClass('open').hide();
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetRoles?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
           
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#ISActive").val() == 1 ? $("#DisplayActive").prop('checked', true) : $("#DisplayActive").prop('checked', false);
            $("#SetUpLabel").text("Update [" + $("#RoleName").val() + "]");
            $("#RoleId").attr("disabled", "disabled");
            openpopup();
          
            $("#btnSave").show();
            $("#btnCancel").hide();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}

function NewRoles() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "RolesNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Roles");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#RoleId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").hide();
            $("#btnCancel").show();
            //$("#btnSaveNew").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function SaveRolesData(action) {
    if ($("#needs-validation").valid()) {
        if (validateRole()) {
            var Id = $("#RoleId").val() || '';
            var PostURL = '/SetUp/RolesUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/RolesAdd'
            }
            var Inputdata = {
                RoleId: Id,
                RoleName: $("#RoleName").val(),
                ISActive: $("#ISActive").val(),
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
                                reloadClassfificationData();
                                closePopup();
                            }
                            else {
                                $("#warning").hide();
                                ResetFieldes();
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
        else {
            $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
            $("#warning").show();
        }
    }
    else {
        $("#warning").html("<ul><li>Please fill are mandatory fields</li></ul>");
        $("#warning").show();
    }
}
/********Roles End**********/

//******Province Start ******
function BindProvinceData(ProvinceDataList) {
    tblProvinceData = $('#tblProvinceData').DataTable({
        data: ProvinceDataList,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'ProvinceId',
                 orderable: false
             },
            {
                'data': 'ProvinceId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='RolesDetail(this," + oData.ProvinceId + ")'>" + oData.ProvinceId + "</a>");
                }
            },
            {
                'data': 'ProvinceName',
            },
            {
                'data': 'ProvinceId',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditRoles(this," + oData.ProvinceId + ")'>" +

        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 3, "className": "text-center", }, ],
    });
    tblProvinceData.on('order.dt search.dt', function () {
        tblProvinceData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblProvinceData.columns.adjust();
    thId = 'tblProvinceDataTR';
    myVar = setInterval("myclick()", 500);
}
//******Province End ******
function myclick() {
    document.getElementById(thId).click();
    document.getElementById(thId).click();
    clearTimeout(myVar);
    $(".animationload").hide();
}

function closePopup() {
    $('#SetUp').modal('hide');
    $(".modal-backdrop").hide()
}

function openpopup() {
    $("#warning").hide();
    $('#SetUp').modal({ backdrop: 'static', keyboard: false })
    $('#SetUp').modal('show');
}

function ResetFieldes() {
    $("#Password").val("");
    $("#UserDob").val("");
    $("#AccountExpiryDate").val("");
    $("#fildset").find('input:text').val('');
    $("#fildset").find('select').val(null);
    $('input[type=checkbox]').prop('checked', false);
}