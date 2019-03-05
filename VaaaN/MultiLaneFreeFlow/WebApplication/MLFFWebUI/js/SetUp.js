/*User List Start*/
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
           {
               'data': 'UserDob',
               fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   $(nTd).html(DateFormatTime(oData.UserDob));
               }
           },
           {
               'data': 'AccountExpiryDate',
               fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                   $(nTd).html(DateFormatTime(oData.AccountExpiryDate));
               }
           },
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
function reloadUser() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "UserReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblUserData.clear().draw();
            tblUserData.rows.add(JSON.parse(data));
            tblUserData.columns.adjust().draw();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
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
            $("#LoginName").val(' ');
            $("#UserDob").attr("data-provide", "datepicker").attr("readolny", true);
            $("#AccountExpiryDate").attr("data-provide", "datepicker").attr("readolny", true);
            //$("#AccountExpiryDate").val("");
            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").show();
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
            $("#UserDob").attr("data-provide", "datepicker").attr("readolny", true);
            $("#AccountExpiryDate").attr("data-provide", "datepicker").attr("readolny", true);
            $("#btnSave").show();
            $("#btnSave").text("Update");
            //$("#btnCancel").hide();
            $("#btnClose").show();
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
                LoginName: $.trim($("#LoginName").val()),
                Password: $.trim($("#Password").val()),
                FirstName: $.trim($('#FirstName').val()),
                LastName: $.trim($('#LastName').val()),
                UserDob: $('#UserDob').val(),
                Address: $.trim($("#Address").val()),
                MobileNo: $.trim($("#MobileNo").val()),
                EmailId: $.trim($("#EmailId").val()),
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
                            if (action == 'close') {
                                reloadUser();
                                closePopup();
                            }
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
    if ($("#RoleId").val() == null) {
        showError($("#RoleId"), $("#RoleId").attr('data-val-required'));
        valid = false;
    }
    else {
         showError($("#RoleId"), '');
    }
    return valid;
}
function UserDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetUser?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $("#SetUpLabel").text("View " + $("#FirstName").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#RoleId").val($("#SlRoleId").val());
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
            $('#UserDob').prop('readonly', false);
            $('#AccountExpiryDate').prop('readonly', false);
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
/*User List End*/

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
            $("#btnClose").show();
            //$("#btnCancel").show();
            $("#btnSaveNew").show();
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
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#Name").val() + "]");
            openpopup();
            $("#Id").attr("disabled", "disabled");
            $("#btnSave").show();
            $("#btnClose").show();
           // $("#btnCancel").hide();
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
            //$("#btnCancel").hide();
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
    if ($("#HardwareType").val() == '') {
        showError($("#HardwareType"), $("#HardwareType").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#HardwareType"), '');
    }
    if ($("#HardwarePosition").val() == '') {
        showError($("#HardwarePosition"), $("#HardwarePosition").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#HardwarePosition"), '');
    }
    if ($("#IpAddress").val() == '') {
        showError($("#IpAddress"), $("#IpAddress").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#IpAddress"), '');
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
        columnDefs: [{ "orderable": false, "targets": 8, "className": "text-center", }, ],
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
        url: "HardwareReload",
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
            $("#btnClose").show();
            //$("#btnCancel").show();
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
            if (Id == 0) {
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
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#HardwareName").val() + "]");
            openpopup();
            $("#HardwareId").attr("disabled", "disabled");
            $("#HardwareType").val($("#hfHardwareType").val());
            $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").show();
            $("#btnClose").show();
           // $("#btnCancel").hide();
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
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#HardwareName").val() + "");
            openpopup();
            $("#HardwareId").attr("disabled", "disabled");
            $("#HardwareType").val($("#hfHardwareType").val());
            $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").hide();
            $("#btnpopupClose").removeClass('btn-outline-secondary').addClass('btn-outline-danger');
            $("#btnpopupClose").show();
            //$("#btnCancel").hide();
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
    if ($("#EtcAntennaNameFront").val() == '') {
        showError($("#EtcAntennaNameFront"), $("#EtcAntennaNameFront").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#EtcAntennaNameFront"), '');
    }
    if ($("#EtcAntennaNameRear").val() == '') {
        showError($("#EtcAntennaNameRear"), $("#EtcAntennaNameRear").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#EtcAntennaNameRear"), '');
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
                    else {
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
        columnDefs: [{ "orderable": false, "targets": 14, "className": "text-center", }, ],
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
        url: "LaneReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblLaneData.clear().draw();
            tblLaneData.rows.add(JSON.parse(data));
            tblLaneData.columns.adjust().draw();

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
            $("#btnClose").show();
           // $("#btnCancel").show();
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
                                reloadLaneData();
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
            $("#EtcAntennaNameFront").val($("#hfAntennaIdFront").val());
            $("#EtcAntennaNameRear").val($("#hfAntennaIdRear").val());
            $("#btnSave").show();
            $("#btnClose").show();
            $("#btnUpdateCancel").show();
            //$("#btnCancel").hide();
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
        url: "/SetUp/GetLane?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#LaneName").val() + "");
            openpopup();
            //$("#HardwareType").val($("#hfHardwareType").val());
           // $("#HardwarePosition").val($("#hfHardwarePosition").val());
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#btnUpdate").hide();
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
                    $(nTd).html("<a href='javascript:void(0);' onclick='GantryDetail(this," + oData.PlazaId + ")'>" + oData.PlazaId + "</a>");
                }
            },
            { 'data': 'PlazaName' },
            { 'data': 'Location' },
            { 'data': 'IpAddress' },
            { 'data': 'Longitude' },
            { 'data': 'Latitude' },
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
        columnDefs: [{ "orderable": false, "targets": 7, "className": "text-center", }, ],
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
function reloadGantry() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "GantryReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblGantryData.clear().draw();
            tblGantryData.rows.add(JSON.parse(data));
            tblGantryData.columns.adjust().draw();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
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
            //$("#btnCancel").hide();
            $("#btnClose").show();
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
            $("#SetUpLabel").text("Register New Gantry");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#PlazaId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
           // $("#btnCancel").show();
            $("#btnClose").show();
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
                Longitude: parseFloat($("#Longitude").val()),
                Latitude: parseFloat($("#Latitude").val()),
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
                                reloadGantry();
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
function GantryDetail(ctrl, Id) {
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
            $("#SetUpLabel").text("View " + $("#PlazaName").val() + "");
            openpopup();
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
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
                    $(nTd).html("<a href='javascript:void(0);' onclick='RoleDetail(this," + oData.RoleId + ")'>" + oData.RoleId + "</a>");
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
                }
            },
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
        columnDefs: [{ "orderable": false, "targets": 4, "className": "text-center", }, ],
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
function reloadRole() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "RoleReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblRolesData.clear().draw();
            tblRolesData.rows.add(JSON.parse(data));
            tblRolesData.columns.adjust().draw();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
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
            //$("#btnCancel").hide();
            $("#btnClose").show();
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
            $("#btnClose").show();
            //$("#btnCancel").show();
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
                                reloadRole();
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
function RoleDetail(ctrl, Id) {
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
            $("#SetUpLabel").text("View " + $("#RoleName").val() + "");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#ISActive").val() == 1 ? $("#DisplayActive").prop('checked', true) : $("#DisplayActive").prop('checked', false);
            $("#DisplayActive").attr("disabled", "disabled");
            $("#btnSave").hide();
           // $("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
/********Roles End**********/

//******Province Start ******
function validateProvince() {
    var valid = true;
    if ($("#ProvinceName").val() == '') {
        showError($("#ProvinceName"), $("#ProvinceName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ProvinceName"), '');
    }
    return valid;
}
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
                    $(nTd).html("<a href='javascript:void(0);' onclick='ProvinceDetail(this," + oData.ProvinceId + ")'>" + oData.ProvinceId + "</a>");
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
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditProvince(this," + oData.ProvinceId + ")'>" +
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
function reloadProvinceData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "ProvinceReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblProvinceData.clear().draw();
            tblProvinceData.rows.add(JSON.parse(data));
            tblProvinceData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}
function EditProvince(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetProvince?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();

            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#ProvinceName").val() + "]");
            $("#ProvinceId").attr("disabled", "disabled");
            openpopup();

            $("#btnSave").show();
            //$("#btnCancel").hide();
            $("#btnClose").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function NewProvince() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "ProvinceNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Province");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#ProvinceId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").show();
            //$("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function SaveProvinceData(action) {
    if ($("#needs-validation").valid()) {
        if (validateProvince()) {
            var Id = $("#ProvinceId").val() || '';
            var PostURL = '/SetUp/ProvinceUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/ProvinceAdd'
            }
            var Inputdata = {
                ProvinceId: Id,
                ProvinceName: $("#ProvinceName").val(),
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
                                reloadProvinceData();
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
function ProvinceDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetProvince?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#ProvinceName").val() + "");
            openpopup();
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
//******Province End ******

//******City Start ******
function validateCity() {
    var valid = true;
    if ($("#CityName").val() == '') {
        showError($("#CityName"), $("#CityName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#CityName"), '');
    }
    if ($("#ProvinceId").val() == 0) {
        showError($("#ProvinceId"), $("#ProvinceId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ProvinceId"), '');
    }
    return valid;
}
function BindCityData(CityListData) {
    var obj = JSON.parse(CityListData);
    tblCityData = $('#tblCityData').DataTable({
        data: obj,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'CITY_ID',
                 orderable: false
             },
            {
                'data': 'CITY_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='CityDetail(this," + oData.CITY_ID + ")'>" + oData.CITY_ID + "</a>");
                }
            },
            { 'data': 'CITY_NAME', },
            { 'data': 'PROVINCE_NAME', },
            {
                'data': 'CITY_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditCity(this," + oData.CITY_ID + ")'>" +
        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 4, "className": "text-center", }, ],
    });
    tblCityData.on('order.dt search.dt', function () {
        tblCityData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblCityData.columns.adjust();
    thId = 'tblCityDataTR';
    myVar = setInterval("myclick()", 500);
}
function reloadCity() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "CityReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblCityData.clear().draw();
            tblCityData.rows.add(JSON.parse(data));
            tblCityData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}
function EditCity(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetCity?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#CityName").val() + "]");
            $("#CityId").attr("disabled", "disabled");
            openpopup();
            $("#btnSave").show();
           // $("#btnCancel").hide();
            $("#btnClose").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function NewCity() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "CityNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Kabupaten/Kota");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#CityId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").show();
           // $("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function SaveCityData(action) {
    if ($("#needs-validation").valid()) {
        if (validateCity()) {
            var Id = $("#CityId").val() || '';
            var PostURL = '/SetUp/CityUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/CityAdd'
            }
            var Inputdata = {
                CityId: Id,
                CityName: $("#CityName").val(),
                ProvinceId: $("#ProvinceId").val(),
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
                                reloadCity();
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
function CityDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetCity?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#CityName").val() + "");
            openpopup();
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
//******City End ******


//******District Start ******
function validateDistrict() {
    var valid = true;
    if ($("#DistrictName").val() == '') {
        showError($("#DistrictName"), $("#DistrictName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#DistrictName"), '');
    }
    if ($("#CityId").val() == 0) {
        showError($("#CityId"), $("#CityId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#CityId"), '');
    }
    return valid;
}
function BindDistrictData(DistrictDataList) {
    var obj = JSON.parse(DistrictDataList);
    tblDistrictData = $('#tblDistrictData').DataTable({
        data: obj,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'DISTRICT_ID',
                 orderable: false
             },
            {
                'data': 'DISTRICT_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='DistrictDetail(this," + oData.DISTRICT_ID + ")'>" + oData.DISTRICT_ID + "</a>");
                }
            },
            { 'data': 'DISTRICT_NAME', },
            { 'data': 'PROVINCE_NAME', },
            { 'data': 'CITY_NAME', },
            {
                'data': 'DISTRICT_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditDistrict(this," + oData.DISTRICT_ID + ")'>" +
        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 5, "className": "text-center", }, ],
    });
    tblDistrictData.on('order.dt search.dt', function () {
        tblDistrictData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblDistrictData.columns.adjust();
    thId = 'tblDistrictDataTR';
    myVar = setInterval("myclick()", 500);
}
function reloadDistrictData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "DistrictReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblDistrictData.clear().draw();
            tblDistrictData.rows.add(JSON.parse(data));
            tblDistrictData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}
function EditDistrict(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetDistrict?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#DistrictName").val() + "]");
            $("#DistrictId").attr("disabled", "disabled");
            openpopup();
            $("#btnSave").show();
            //$("#btnCancel").hide();
            $("#btnClose").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function NewDistrict() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "DistrictNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Kecamatan");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#DistrictId").val('0').attr("disabled", "disabled");
            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").show();
           // $("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function SaveDistrictData(action) {
    if ($("#needs-validation").valid()) {
        if (validateDistrict()) {
            var Id = $("#DistrictId").val() || '';
            var PostURL = '/SetUp/DistrictUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/DistrictAdd'
            }
            var Inputdata = {
                DistrictId: Id,
                DistrictName: $("#DistrictName").val(),
                CityId: $("#CityId").val(),
                ProvinceId: $("#ProvinceId").val()
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
                                reloadDistrictData();
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
function DistrictDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetDistrict?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#DistrictName").val() + "");
            openpopup();
            $("#btnSave").hide();
           // $("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
//******District End ******

//******SubDistrict Start ******
function validateSubDistrict() {
    var valid = true;
    if ($("#SubDistrictName").val() == '') {
        showError($("#SubDistrictName"), $("#SubDistrictName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#SubDistrictName"), '');
    }
    if ($("#DistrictId").val() == 0) {
        showError($("#DistrictId"), $("#DistrictId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#DistrictId"), '');
    }
    return valid;
}
function BindSubDistrictData(SubDistrictDataList) {
    var obj = JSON.parse(SubDistrictDataList);
    tblSubDistrictData = $('#tblSubDistrictData').DataTable({
        data: obj,
        "oLanguage": { "sSearch": '<a class="btn searchBtn" id="searchBtn"><i class="ti-search"></i></a>' },
        scrollY: "42vh",
        scrollX: false,
        paging: false,
        info: false,
        columns: [
             {
                 'data': 'SUB_DISTRICT_ID',
                 orderable: false
             },
            {
                'data': 'SUB_DISTRICT_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<a href='javascript:void(0);' onclick='SubDistrictDetail(this," + oData.SUB_DISTRICT_ID + ")'>" + oData.SUB_DISTRICT_ID + "</a>");
                }
            },
            { 'data': 'SUB_DISTRICT_NAME', },
           
            { 'data': 'PROVINCE_NAME', },
            { 'data': 'CITY_NAME', },
             { 'data': 'DISTRICT_NAME', },
            {
                'data': 'SUB_DISTRICT_ID',
                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                    $(nTd).html("<div class='dropdown' style='padding-left: 14px;'>" +
                        "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
            "<span class='icon-holder'>" +
                "<i class='c-blue-500 ti-menu-alt'></i>" +
            "</span>" +
        "</a>" +
        " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
        "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditSubDistrict(this," + oData.SUB_DISTRICT_ID + ")'>" +
        "        <span class='title'>Update</span>" +
        "    </a>" +
        "</div>" +
    "</div>");
                }
            },
        ],
        columnDefs: [{ "orderable": false, "targets": 6, "className": "text-center", }, ],
    });
    tblSubDistrictData.on('order.dt search.dt', function () {
        tblSubDistrictData.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('.dataTables_filter input').attr("placeholder", "Search this list…");
    tblSubDistrictData.columns.adjust();
    thId = 'tblSubDistrictDataTR';
    myVar = setInterval("myclick()", 500);
}
function reloadSubDistrictData() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "SubDistrictReload",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $(".animationload").hide();
            tblSubDistrictData.clear().draw();
            tblSubDistrictData.rows.add(JSON.parse(data));
            tblSubDistrictData.columns.adjust().draw();

        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function EditSubDistrict(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetSubDistrict?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();

            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("Update [" + $("#SubDistrictName").val() + "]");
            $("#SubDistrictId").attr("disabled", "disabled");
            openpopup();

            $("#btnSave").show();
           // $("#btnCancel").hide();
            $("#btnClose").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function NewSubDistrict() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "SubDistrictNew",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#SetUpLabel").text("Register New Kelurahan/Desa");
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#SubDistrictId").val('0').attr("disabled", "disabled");

            $("#btnSave").show();
            $("#btnSave").text("Save");
            $("#btnClose").show();
            //$("#btnCancel").show();
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
function SaveSUbDistrictData(action) {
    if ($("#needs-validation").valid()) {
        if (validateSubDistrict()) {
            var Id = $("#SubDistrictId").val() || '';
            var PostURL = '/SetUp/SubDistrictUpdate';
            if (Id == 0) {
                Id = 0;
                PostURL = '/SetUp/SubDistrictAdd'
            }
            var Inputdata = {
                SubDistrictId: Id,
                SubDistrictName: $("#SubDistrictName").val(),
                ProvinceId: $("#ProvinceId").val(),
                CityId: $("#CityId").val(),
                DistrictId: $("#DistrictId").val(),
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
                                reloadSubDistrictData();
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
function SubDistrictDetail(ctrl, Id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/SetUp/GetSubDistrict?id=" + Id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialModel').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#SetUpLabel").text("View " + $("#SubDistrictName").val() + "");
            openpopup();
            $("#btnSave").hide();
            //$("#btnCancel").hide();
            $("#fildset").attr("disabled", "disabled");
        },
        error: function (x, e) {
            $(".animationload").hide();
        }
    });
}
//******SubDistrict End ******

function myclick() {
    //document.getElementById(thId).click();
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

function DateFormatTime(newDate) {

    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(newDate);
    var d = new Date(parseFloat(results[1]));
    var date = new Date(d).toDateString("yyyy/MM/dd");
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = monthNames[d.getMonth()];
    yy = d.getFullYear();
    return dd + '-' + mm + '-' + +yy;
}

var monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];