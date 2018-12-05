var ResponceData = [];

$(document).ready(function () {
    $('.prev i').removeClass();
    $('.prev i').addClass("fa fa-chevron-left");

    $('.next i').removeClass();

    $('.next i').addClass("fa fa-chevron-right");
    $('#module_2').css({ "background-color": "#00B4CE" });
    $('#module_2').css({ "font-weight": "bold" });
    $('#submodule_9').css({ "background-color": "#00B2aa" });
    $('#submodule_9').css({ "font-weight": "bold" });


    $("#AccountId").hide();
    var typingTimer;                //timer identifier
    var doneTypingInterval = 500;   //time in ms, 5 second for example
    var $input = $('#inputToSearch');
    var $Finput = $('#FirstName');

    $Finput.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(FnameTyping, 20);
    });

    $Finput.on('keydown', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(FnameTyping, 20);
    });
    //on keyup, start the countdown
    $input.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    });
    //on keydown, clear the countdown
    $input.on('keydown', function () {
        clearTimeout(typingTimer);
    });

    function FnameTyping() {
        $("#AccountId").val(0);
        $("#LastName").val('');
        if ($("#FirstName").val() == '') {
            $("#searchCust").text('Find Customer');
        }
        else {
            $("#searchCust").text('Validate Customer');
        }
        $("#spnTick").removeClass('glyphicon-ok').addClass('glyphicon-remove');


    }

    //user is "finished typing," do something
    function doneTyping() {
        //do something

        FilterJson($("#inputToSearch").val(), "#inputToSearch");
    }

    //For IE only
    $("#inputToSearch").bind("mouseup", function (e) {
        var $input = $(this);
        if ($input.val() != '')
            setTimeout(function () {
                var newValue = $input.val();
                if (newValue == "") {
                    FilterJson('', '#inputToSearch');
                    $('#inputToSearch').removeClass('notFound');
                }
            }, 1);
    });

});

function Validation() {
    debugger;
    if (!validateCustomerVehicle()) {
        return false;
    }
    return true;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function validateAlpha() {
    var textInput = document.getElementById("FirstName").value;
    textInput = textInput.replace(/[^A-Za-z]/g, "");
    document.getElementById("FirstName").value = textInput;

}

function validateLastname() {
    var textInput2 = document.getElementById("LastName").value;
    textInput2 = textInput2.replace(/[^A-Za-z]/g, "");
    document.getElementById("LastName").value = textInput2;
}

function HideModel() {
    $('#SearchUserModal').modal('hide');
}

function btnSearchUser() {
    $.ajax({
        type: 'GET',
        url: "/POS/GetUserData",
        cache: false,
        dataType: "json",
        success: function (data) {
            ResponceData = JSON.parse(data.Data);
            BindUserList(ResponceData);
            if ($('#FirstName').val() == '') {
                $('#inputToSearch').val('');
                BindUserList(ResponceData);
            }
            else {
                $('#inputToSearch').val($('#FirstName').val());
                FilterJson($('#FirstName').val());
            }
            $('#SearchUserModal').modal('show');
        },
        error: function (ex) {
            $("#SearchUserModal").modal(show);
        }
    });
}

function BindUserList(data) {
    var TR;
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            TR = TR + "<tr style='cursor:pointer;' onclick=selectUser(" + data[i].AccountId + ",this)><td >" + (i + 1) + "</td><td style='text-align:left'>" + data[i].FirstName + "</td><td style='text-align:left'>" + replacenull(data[i].LastName) + "</td><td>" + replacenull(data[i].ResidentId) + "</td><td>" + replacenull(data[i].MobileNo) + "</td>" +
          "<td style='text-align:left'>" + replacenull(data[i].EmailId) + "</td></tr>"
        }
    }
    else {
        TR = "<tr><td colspan='5'>No user found</td></tr>";
    }
    $("#tableCustomerList tbody").html(TR);
}

function replacenull(str) {
    var rep = '';
    if (str != null)
        rep = str;
    return rep;
}

function selectUser(AccountId, TR) {
    $("#AccountId").val(AccountId);
    $("#FirstName").val($(TR).find('td:nth-child(2)').text());
    $("#LastName").val($(TR).find('td:nth-child(3)').text());
    $("#spnTick").removeClass('glyphicon-remove').addClass('glyphicon-ok');
    HideModel();
}

function FilterJson(SearchText) {
    if (ResponceData.length > 0) {
        var FilteredData = $.grep(ResponceData, function (element, index) {
            return element.FirstName.toLowerCase().indexOf(SearchText.toLowerCase()) > -1 || element.LastName.toLowerCase().indexOf(SearchText.toLowerCase()) > -1 ||
                element.EmailId.toLowerCase().indexOf(SearchText.toLowerCase()) > -1 || element.MobileNo.indexOf(SearchText) > -1;
        });
        BindUserList(FilteredData);
    }
}

function validateCustomerVehicle() {
    if ($("#AccountId").val() == 0) {
        showError('Customer must be validate');
        return false;
    }
    if ($("#VehicleClassId").val() == 0) {
        showError('Select Vehicle Class');
        return false;
    }

    if ($("#VehRegNo").val() == '') {
        showError('Enter Vehicle Registration No');
        return false;
    }

    if ($("#TagId").val() == '') {
        showError('Enter Tag Id');
        return false;
    }
    else if ($("#TagId").val().length != 24) {
        showError('Enter valid TAG Id');
        return false;
    }
    else if (!validTAGId($("#TagId").val())) {
        showError('Enter valid TAG Id');
        return false;
    }

    else if ($("#VehicleRegistrationCerticateId").val() == '') {
        showError('Enter Vehicle Certificate Id');
        return false;
    }
    else if ($("#VehicleRegistrationCerticateId").val() == '') {
        showError('Enter Vehicle Certificate Id');
        return false;
    }
    else if ($("#Address").val() == '') {
        showError('Address must be entered');
        return false;
    }
    else if ($("#Brand").val() == 0) {
        showError('Brand must be selected');
        return false;
    }
    else if ($("#VehicleType").val() == 0) {
        showError('Vehicle Type must be selected');
        return false;
    }
    else if ($("#VehicleCategory").val() == 0) {
        showError('Vehicle Category must be selected');
        return false;
    }
    else if ($("#FuelType").val() == 0) {
        showError('Fuel Type must be selected');
        return false;
    }
    else if ($("#ModelNo").val() == '') {
        showError('Model Number must be entered');
        return false;
    }
    else if ($("#ManufacturingYear").val() == '') {
        showError('Manufacturing Year must be entered');
        return false;
    }
    else if ($("#CyclinderCapacity").val() == '') {
        showError('Cyclinder Capacity must be entered');
        return false;
    }
    else if ($("#FrameNumber").val() == '') {
        showError('Frame Number must be entered');
        return false;
    }
    else if ($("#EngineNumber").val() == '') {
        showError('Engine Number must be entered');
        return false;
    }
    else if ($("#VehicleColor").val() == 0) {
        showError('Vehicle Color must be selected');
        return false;
    }
    else if ($("#LicencePlateColor").val() == 0) {
        showError('Licence Plate Color must be selected');
        return false;
    }
    else if ($("#RegistrationYear").val() == '') {
        showError('Registration Year must be entered');
        return false;
    }
    else if ($("#LocationCode").val() == '') {
        showError('Location Code must be entered');
        return false;
    }
    else if ($("#ValidUntil").val() == '') {
        showError('Valid Until must be entered');
        return false;
    }
    else if ($('#FrontImage')[0].files.length === 0 && hfVehicleImageFront.val() == "") {
        alert("Front Image Required");
        $('#FrontImage').focus();
        return false;
    }
    else if ($('#RearImage')[0].files.length === 0 && hfVehicleImageRear.val() == "") {
        alert("Rear Image Required");
        $('#RearImage').focus();
        return false;
    }
    else if ($('#RightSideImage')[0].files.length === 0 && hfVehicleImageRightSide.val() == "") {
        alert("Right side Image Required");
        $('#RightSideImage').focus();
        return false;
    }
    else if ($('#LeftSideImage')[0].files.length === 0 && hfVehicleImageLeftSide.val() == "") {
        alert("Left side Image Required");
        $('#LeftSideImage').focus();
        return false;
    }
    return true;
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
        url: "/POS/GetTagId",
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

function showError(errorMsg) {
    $("#lblErrors").text('');
    $("#lblErrors").text(errorMsg);
}