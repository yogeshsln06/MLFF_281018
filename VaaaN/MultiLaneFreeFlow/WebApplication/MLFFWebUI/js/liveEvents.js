var ResponceData = [];
$(document).ready(function () {
    //function setHeight() {
    //    windowHeight = $(window).innerHeight() - 113;
    //    $('.container-fluid').css('max-height', windowHeight);
    //};
    //setHeight();

    //$(window).resize(function () {
    //    setHeight();
    //});

    $("#tblIKE").freezeHeader({ 'height': '400px' });
    $("#tblANPRFront").freezeHeader({ 'height': '400px' });
    $("#tblANPRRear").freezeHeader({ 'height': '400px' });
    myVar = setInterval("GetMSMQData()", 2000);
    //GetMSMQData()
    //$('body').attr("style", "");
});

function filterData() {
    var GantryId = $("#ddlGantry").val();
    if (GantryId > 0) {
        var FilteredData = $.grep(ResponceData, function (element, index) {
            return element.PlazaId == GantryId;
        });
        BindUserList(FilteredData);
    }
    else {
        GetMSMQData();
    }
}

function GetMSMQData() {
    $.ajax({
        type: 'GET',
        url: "GetMSMQLiveEvent",
        cache: false,
        dataType: "json",
        success: function (data) {
            //ResponceData = JSON.parse(data.Data);
            ResponceData = data.Data;
            BindEventList(ResponceData);
        },
        error: function (ex) {
            //$("#SearchUserModal").modal(show);
        }
    });
}

function BindEventList(data) {
    var TR;
    var loc = "Front"
    if (data.length > 0) {
        TR = '';
        for (var i = 0; i < data.length; i++) {
            if (data[i].PacketName.toLowerCase() == 'crosstalk') {
                if (data[i].DeviceLocation == "1") {
                    loc = "Front";
                }
                else {
                    loc = "Rear";
                }
                //
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].Datepacket + "</td><td style='text-align:left'>" + data[i].LaneName + "-" + loc + "</td><td style='text-align:left'>" + data[i].TagId + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
              "<td>" + replacenull(data[i].VehicleClassName) + "</td></tr>"

                if ($("#tblIKE tbody tr").length > 0) {
                    $('#tblIKE tbody tr:first').before(TR);
                }
                else {
                    $("#tblIKE tbody:last-child").append(TR);
                }
                //$("#tblIKE").freezeHeader({ 'height': '200px' });
            }
            else if (data[i].PacketName.toLowerCase() == "nodeflux - front") {
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].Datepacket + "</td><td style='text-align:left'>" + data[i].LaneName + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
              "<td>" + replacenull(data[i].VehicleClassName) + "</td></tr>"
                if ($("#tblANPRFront tbody tr").length > 0) {
                    $('#tblANPRFront tbody tr:first').before(TR);
                }
                else {
                    $("#tblANPRFront tbody:last-child").append(TR);
                }
            }
            else if (data[i].PacketName.toLowerCase() == 'nodeflux - rear') {
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].Datepacket + "</td><td style='text-align:left'>" + data[i].LaneName + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
              "<td>" + replacenull(data[i].VehicleClassName) + "</td></tr>"

                if ($("#tblANPRRear tbody tr").length > 0) {
                    $('#tblANPRRear tbody tr:first').before(TR);
                }
                else {
                    $("#tblANPRRear tbody:last-child").append(TR);
                }
                $("#tblANPRRear").freezeHeader({ 'height': '400px' });
            }
        }
    }

}


function replacenull(str) {
    var rep = '';
    if (str != null)
        rep = str;
    return rep;
}

function ViewFiles(vrnPath, VehiclePath, VideoURL) {
    //debugger;
    if (replacenull(vrnPath) == '') {
        $("#myimageVrn").attr('src', noimagepath);
    }
    else {
        $("#myimageVrn").attr('src', "/" + findAndReplace(vrnPath, '^', '/'));
    }

    if (replacenull(VehiclePath) == '') {
        $("#myimageVehicle").attr('src', noimagepath);
    }
    else {
        $("#myimageVehicle").attr('src', "/" + findAndReplace(VehiclePath, '^', '/'));
    }

    var $video = $('#rearvideo video'),
           videoSrc = $('source', $video).attr('src', VideoURL);
    $video[0].load();

    var modal = document.getElementById('myModal');
    modal.style.display = "block";
}


function imageclose() {
    var modal = document.getElementById('myModal');
    modal.style.display = "none";

}


function findAndReplace(string, target, replacement) {
    var i = 0, length = string.length;
    for (i; i < length; i++) {
        string = string.replace(target, replacement);
    }
    return string;

}