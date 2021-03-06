﻿var ResponceData = [];

$(document).ready(function () {
    //$('.prev i').removeClass();
    //$('.prev i').addClass("fa fa-chevron-left");

    //$('.next i').removeClass();

    //$('.next i').addClass("fa fa-chevron-right");
    //$('#module_2').css({ "background-color": "#00B4CE" });
    //$('#module_2').css({ "font-weight": "bold" });
    //$('#submodule_9').css({ "background-color": "#00B2aa" });
    //$('#submodule_9').css({ "font-weight": "bold" });

    $("#ValidUntil").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
    });
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
            TR = TR + "<tr style='cursor:pointer;' onclick=selectUser(" + data[i].AccountId + ",this)><td >" + (i + 1) + "</td><td style='text-align:left'>" + data[i].FirstName + "</td><td>" + replacenull(data[i].ResidentId) + "</td><td>" + replacenull(data[i].MobileNo) + "</td>" +
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
    $("#Residentid").val($(TR).find('td:nth-child(3)').text());
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
    var valid = true;
    if ($("#AccountId").val() == 0) {
        showError($("#AccountId"), 'Customer must be validate');
        valid = false;
    }

    if ($("#VehicleRCNumber").val() == '') {
        showError($("#VehicleRCNumber"), 'Registration Certificate Number Required');
        valid = false;
    }
    else {
        showError($("#VehicleRCNumber"), '');
    }
    if ($("#VehRegNo").val() == '') {
        showError($("#VehRegNo"), 'Vehicle Registration Number Required');
        valid = false;
    }
    else {
        showError($("#VehRegNo"), '');
    }
    if ($("#OwnerName").val() == '') {
        showError($("#OwnerName"), 'Owner Name Required');
        valid = false;
    }
    else {
        showError($("#OwnerName"), '');
    }
    if ($("#OwnerAddress").val() == '') {
        showError($("#OwnerAddress"), 'Owner Address Required');
        valid = false;
    }
    else {
        showError($("#OwnerAddress"), '');
    }
    if ($("#ValidUntil").val() == '') {
        showError($("#ValidUntil"), 'Valid Until Required');
        valid = false;
    }
    if ($("#TidFront").val() == '') {
        showError($("#TidFront"), 'Front TID Required');
        valid = false;
    }
    else {
        showError($("#TidFront"), '');
    }
    if ($("#TidRear").val() == '') {
        showError($("#TidRear"), 'Rear TID Required');
        valid = false;
    }
    else {
        showError($("#TidRear"), '');
    }
    if ($("#VehicleClassId").val() == 0) {
        showError($("#VehicleClassId"), 'Vehicle Class Required');
        valid = false;
    }
    else {
        showError($("#VehicleClassId"), '');
    }
    if ($("#TagId").val() == '') {
        showError($("#TagId"), 'Valid EPC Required');
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
    if ($("#AccountBalance").val().length = '') {
        showError($("#AccountBalance"), 'Account Balance Required');
        valid = false;
    }
    else {
        showError($("#AccountBalance"), '');
    }
    if ($('#FrontImage')[0].files.length === 0 && $('#hfVehicleImageFront').val() == "") {
        showError($("#FrontImage"), "Front Image Required");
        valid = false;
    }
    else {
        showError($("#FrontImage"), '');
    }
    if ($('#RearImage')[0].files.length === 0 && $('#hfVehicleImageRear').val() == "") {
        showError($('#RearImage'), "Rear Image Required");
        valid = false;
    }
    else {
        showError($("#RearImage"), '');
    }
    if ($('#RightImage')[0].files.length === 0 && $('#hfVehicleImageRight').val() == "") {
        showError($('#RightImage'), "Right Image Required");
        valid = false;
    }
    else {
        showError($("#RightImage"), '');
    }
    if ($('#LeftImage')[0].files.length === 0 && $('#hfVehicleImageLeft').val() == "") {
        showError($('#LeftImage'), "Left side Image Required");
        valid = false;
    }
    else {
        showError($("#LeftImage"), '');
    }
    if ($('#VehicleRCNumberImage')[0].files.length === 0 && $('#hfVehicleRCNumberImage').val() == "") {
        showError($('#VehicleRCNumberImage'), "Registration Certificate Image Required");
        valid = false;
    }
    else {
        showError($("#VehicleRCNumberImage"), '');
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

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}

function validTAGId(str) {
    var pattern = /^[0-9a-fA-F]+$/;
    return String(str).match(pattern);
}
var TR = null;
function rechareData(ctrl) {
    TR = $(ctrl).parent().parent();
    $("#txtVrnNo").val($(TR).find('td:nth-child(3)').text().trim());
    $("#txtMobileNo").val($(TR).find('td:nth-child(2)').text().trim());
    $('#RechargeData').modal('show');
}
function RechargeAmount() {
  
    var InputData = {
        VehRegNo: $(TR).find('td:nth-child(3)').text().trim(),
        EntryId: 0,
        AccountBalance: $('#txtAmount').val()
    }
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: "RechargeAmount",
        dataType: "JSON",
        async: true,
        data: JSON.stringify(InputData),
        contentType: "application/json; charset=utf-8",
        success: function (JsonfilterData) {
            $('#loader').hide();
            ResponceData = JsonfilterData.Data;
            if (ResponceData.Meassage == "Success") {
                $(TR).find('td:nth-child(7)').text(ResponceData.UpdateAmount);
                $("#closepopup").trigger("click");
            }
            else {
                $("#lblErrors").text(ResponceData.Meassage);
            }
            
        },
        error: function (x, e) {
            $("#lblErrors").text("somthing went wrong");
            $('#loader').hide();
        }

    });
}