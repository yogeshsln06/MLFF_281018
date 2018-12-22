var ResponceData = [];
$(document).ready(function () {
    $('.prev i').removeClass();
    $('.prev i').addClass("fa fa-chevron-left");
    $('.next i').removeClass();
    $('.next i').addClass("fa fa-chevron-right");
    //$('#module_4').css({ "background-color": "#00B4CE" });
    //$('#module_4').css({ "font-weight": "bold" });
    $("#tableCrossTalk").freezeHeader({ 'height': '150px' });
    $("#tableNodeFluxFront").freezeHeader({ 'height': '150px' });
    $("#tableNodeFluxRear").freezeHeader({ 'height': '150px' });
    myVar = setInterval("GetMSMQData()", 2000);
    //GetMSMQData()
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
    if (data.length > 0) {
        TR = '';
        for (var i = 0; i < data.length; i++) {
            if (data[i].PacketName.toLowerCase() == 'crosstalk') {
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].PlazaName + "</td><td style='text-align:left'>" + data[i].LaneName + "</td><td style='text-align:left'>" + replacenull(data[i].VehicleClassName) + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
              "<td>" + data[i].Datepacket + "</td><td>" + data[i].TagId + "</td></tr>"

                if ($("#tableCrossTalk tbody tr").length > 0) {
                    $('#tableCrossTalk tbody tr:first').before(TR);
                }
                else {
                    $("#tableCrossTalk tbody:last-child").append(TR);
                }
                $("#tableCrossTalk").freezeHeader({ 'height': '200px' });
            }
            else if (data[i].PacketName.toLowerCase() == "nodeflux - front") {
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].PlazaName + "</td><td style='text-align:left'>" + data[i].LaneName + "</td><td style='text-align:left'>" + replacenull(data[i].VehicleClassName) + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
"<td>" + data[i].Datepacket + "</td><td><a class='dropdown-item' href='javascript:void(0);' onclick='ViewFiles('" + data[i].NumberPlatePath + "','" + data[i].VehiclePath + "','" + data[i].VideoURL + "');'<span class='icon-holder'><i class='c-blue-500 ti-clip'></i></span></a></td></tr>"



                if ($("#tableNodeFluxFront tbody tr").length > 0) {
                    $('#tableNodeFluxFront tbody tr:first').before(TR);
                }
                else {
                    $("#tableNodeFluxFront tbody:last-child").append(TR);
                }
            }
            else if (data[i].PacketName.toLowerCase() == 'nodeflux - rear') {
                TR = "<tr style='cursor:pointer;'><td style='text-align:left'>" + data[i].PlazaName + "</td><td style='text-align:left'>" + data[i].LaneName + "</td><td style='text-align:left'>" + replacenull(data[i].VehicleClassName) + "</td><td>" + replacenull(data[i].VRN) + "</td>" +
               "<td>" + data[i].Datepacket + "</td><td><a class='dropdown-item' href='javascript:void(0);' onclick='ViewFiles('" + data[i].NumberPlatePath + "','" + data[i].VehiclePath + "','" + data[i].VideoURL + "');'<span class='icon-holder'><i class='c-blue-500 ti-clip'></i></span></a></td></tr>"

                if ($("#tableNodeFluxRear tbody tr").length > 0) {
                    $('#tableNodeFluxRear tbody tr:first').before(TR);
                }
                else {
                    $("#tableNodeFluxRear tbody:last-child").append(TR);
                }
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
    var Imagepath = $("#hfAPIPath").val();
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