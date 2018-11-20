function validateUser() {

    if ($("#LoginName").val() == '') {
        showError('Enter Login Name');
        return false;
    }

    if ($("#Password").val() == '') {
        showError('Enter Password');
        return false;
    }

    if ($("#Password").val().length < 5) {
        showError('Password length is too short.');

        return false;
    }


    if ($("#FirstName").val() == '') {
        showError('Enter First Name');

        return false;
    }

    if ($("#MobileNo").val() == '') {
        showError('Enter Mobile No.');

        return false;
    }

    if ($("#MobileNo").val() == '') {
        alert("Mobile Number must be entred");
        return false;
    }

    if ($("#EmailId").val() == '') {
        showError('Enter Email Id');

        return false;
    }

    if (!validEmail($("#EmailId").val())) {
        showError('Enter valid Email Id');

        return false;
    }

    if ($("#RoleId option:selected").text() == 'Select Role') {
        showError('Select Role');

        return false;
    }


    return true;
}

function EditPage_ValidateUser() {
    if ($("#FirstName").val() == '') {
        showError('Enter First Name');

        return false;
    }

    if ($("#MobileNo").val() == '') {
        showError('Enter Mobile No.');

        return false;
    }

    if (!validmobileno($("#MobileNo").val())) {
        showError('Enter valid Mobile Number');
        return false;
    }

    if ($("#EmailId").val() == '') {
        showError('Enter Email Id');

        return false;
    }

    if (!validEmail($("#EmailId").val())) {
        showError('Enter valid Email Id');

        return false;
    }

    if ($("#RoleId option:selected").text() == 'Select Role') {
        showError('Select Role');

        return false;
    }

    return true;
}

function ValidateReport() {
    if ($("#ReportCategory").val() == 0) {
        showError('Report must be selected');
        return false;
    }
    
}

function validEmail(str) {
    var pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return String(str).match(pattern);

}

function validTAGId(str) {
    var pattern = /^[0-9a-fA-F]+$/;
    return String(str).match(pattern);
}

function validmobileno(str) {
    var pattern = /^(\+62 ((\d{3}([ -]\d{3,})([- ]\d{4,})?)|(\d+)))|(\(\d+\) \d+)|\d{3}( \d+)+|(\d+[ -]\d+)|\d+$/;
    return String(str).match(pattern);
}

function validIpAddress(str) {
    var pattern = /^(?=.*[^\.]$)((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.?){4}$/;
    return String(str).match(pattern);

}

function showError(errorMsg) {
    $("#lblErrors").text('');
    $("#lblErrors").text(errorMsg);
}

function validateCustomer() {
    if ($("#FirstName").val() == '') {
        showError('Enter First Name');

        return false;
    }

    if ($("#MobileNo").val() == '') {
        showError('Enter Mobile No.');

        return false;
    }

    //if ($("#MobileNo").val().length < 13) {
    //    showError('Mobile No. must be 10 digits.');
    //    return false;
    //}

    if ($("#EmailId").val() == '') {
        showError('Enter Email Id');

        return false;
    }
    if (!validEmail($("#EmailId").val())) {
        showError('Enter valid Email Id');

        return false;
    }

    return true;
}

function validateHardware() {

    if ($("#PlazaId").val() == 0) {
        showError('Select Gantry');
        return false;
    }

    if ($("#HardwareType").val() == 0) {
        showError('Select Hardware Type');
        return false;
    }
    else if ($("#HardwareType").val() == 1) {
        if ($("#HardwarePosition").val() == 0) {
            showError('Select Hardware Position');
            return false;
        }
    }
    if ($("#HardwareName").val() == '') {
        showError('Enter Hardware Name');
        return false;
    }
    if ($("#ManufacturerName").val() == '') {
        showError('Enter Manufacturer Name');
        return false;
    }

    if ($("#ModelName").val() == '') {
        showError('Enter Model Name');
        return false;
    }



    if ($("#IpAddress").val() == '') {
        showError('Enter Ip Address');
        return false;
    }

    if (!validIpAddress($("#IpAddress").val())) {
        showError('Enter Model Name');
        return false;
    }
    return true;
}

function validatePlaza() {


    if ($("#PlazaName").val() == '') {
        showError('Enter Gantry Name');
        return false;
    }
    if ($("#Location").val() == '') {
        showError('Enter Gantry Location');
        return false;
    }
    if ($("#IpAddress").val() == '') {
        showError('Enter Ip Address');
        return false;
    }

    if (!validIpAddress($("#IpAddress").val())) {
        showError('Enter Model Name');
        return false;
    }
    return true;
}

function validateLane() {

    if ($("#PlazaId").val() == 0) {
        showError('Select Gantry');
        return false;
    }
    if ($("#Location").val() == '') {
        showError('Enter Lajur Name');
        return false;
    }
    if ($("#CameraIdFront").val() == 0) {
        showError('Select Camera Name Front');
        return false;
    }
    if ($("#CameraIdRear").val() == 0) {
        showError('Select Camera Name Rear');
        return false;
    }
    if ($("#CameraIdFront").val() == $("#CameraIdRear").val()) {
        showError('Camera Name Front & Rear must be different');
        return false;
    }
    if ($("#EtcReaderId").val() == 0) {
        showError('Select ETC Reader Name');
        return false;
    }

    return true;
}

function validateVehicleClass() {
    if ($("#Name").val() == '') {
        showError('Enter Vehicle Class Name');
        return false;
    }

    return true;
}

function validateCustomerVehicle() {
    if ($("#AccountId").val() == 0) {
        showError('User Details must be selected');
        return false;
    }
    //if ($("#Firstname").val() == '') {
    //    showError('First name must be selected');
    //    return false;
    //}

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

    return true;
}



