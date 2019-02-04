/*User List */
function openpopup() {
    $("#warning").hide();
    $('#customerModal').modal('show');
}

function NewUser() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "/SetUp/NewUserMaster",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#exampleModalLabel").text("New User");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#UserDob").attr("data-provide", "datepicker").attr("readolny", true);
            $("#AccountExpiryDate").attr("data-provide", "datepicker").attr("readolny", true);
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
function EditUser(ctrl, id) {
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
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#exampleModalLabel").text("Update [" + $("#FirstName").val() + "]");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());
            $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            GetCityList();

            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#BirthDate").attr("data-provide", "datepicker").attr("readolny", true);
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
        var UserDob = $('#UserDob').val() || ''
        if (UserDob != '') {
            UserDob = DateFormat(UserDob);
        }

        var AccountExpiryDate = $('#AccountExpiryDate').val() || ''
        if (AccountExpiryDate != '') {
            AccountExpiryDate = DateFormat(AccountExpiryDate);
        }

        if (validateUser()) {
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
}
function validateUser() {
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

function SaveLaneData(action) {
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

/********Gantry End**********/

//******User Start ******

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
           {'data': 'LoginName', },
           {'data': 'FirstName', },
           { 'data': 'RoleName', },
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
/********User End**********/
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

function ResetFildes() {
  
    $("#fildset").find('input:text').val('');
 
    $("#fildset").find('select').val(0);
}