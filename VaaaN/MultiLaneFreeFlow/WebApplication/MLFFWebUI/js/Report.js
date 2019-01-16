var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var month = (new Date()).getMonth() + 1;
var Startyear = 2018;
var year = (new Date()).getFullYear();
var CustomerVehcileJson = [];
function bindMonth() {
    for (var i = 0; i < monthNames.length; i++) {
        $("#monthList").append
            ($('<option></option>').val(i + 1).html(monthNames[i]))
    }
    $("#monthList").val(month);
}

function bindYear() {
    for (var i = Startyear; i <= year; i++) {
        $("#yearList").append
            ($('<option></option>').val(i).html(i))
    }
    $("#yearList").val(year);
}

function bindVRN() {
    $.each((CustomerVehcileJson), function (i, vehicle) {
        if (vehicle.VehRegNo != '') {
            $("#vrnList").append
            ($('<option></option>').val(vehicle.EntryId).html(vehicle.VehRegNo))
        }

    })
}

