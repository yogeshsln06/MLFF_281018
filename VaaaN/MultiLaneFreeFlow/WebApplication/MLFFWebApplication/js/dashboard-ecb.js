var optEcb = {
    autoOpen: false,
    resizable: false,
    modal: true,
    width: 1050,
    height: 563,
    title: 'ECB',
    open: function (e) {
        $(e.target).parent().css('background-color', '#1A5276');
        $(e.target).parent().css('top', '10px');
        $(e.target).parent().css('height', '563px');
    }
};

function OpenECBDialog() {
    $("#ECB-Dialog-Box").dialog(optEcb).dialog("open");
}

function GetECBSummaryDetail() {
    // Variable Declaration
    var rstr = '';
    var cstr = '';
    var cur_row = 1;
    var cur_col = 1;
    var complete_row = '';
    var item_added = 0;
    var col = 0;
    // Get the Atcc Data and Count
    var ecbdetails = ecbes;
    var tot_rec = ecbes.length; // Object.keys(ecbdetails.ECBSummaryData).length;

    // Set columns displayed on the screen 
    if (tot_rec > 0 && tot_rec < 10) {
        col = 3;
    }
    else if (tot_rec > 9 && tot_rec < 17) {
        col = 4;
    }
    else if (tot_rec > 16) {
        col = 5;
    }

    // Remove and clear existed ECB device information
    $("#ItemECBDetail").find("tr").remove();

    // Display All ECB devices on the screen
    $.each(ecbdetails, function (i, ecb_item) {
        var atmsId = ecb_item.atmsId;
        var chainage = ecb_item.chainage;
        var controlRoomID = ecb_item.controlRoomID;
        var deviceCategoryId = ecb_item.deviceCategoryId;
        var deviceId = ecb_item.deviceId;
        var deviceActiveImagePath = ecb_item.deviceActiveImagePath;
        var deviceInactiveImagePath = ecb_item.deviceInactiveImagePath;
        var deviceName = ecb_item.deviceName;
        var deviceStatus = ecb_item.devicePingStatus;
        var direction = ecb_item.direction;
        var ipAddress = ecb_item.ipAddress;
        var imgsrc = '';

        if (deviceStatus == 0) {
            imgsrc = deviceInactiveImagePath;
        }
        else if(deviceStatus == 1)
        {
            imgsrc = deviceActiveImagePath;
        }

        if (cur_col <= col) {
            // cstr += '<td><div id="div_ecb' + deviceId + '" class="atcc_data"><div id="' + deviceId + '">' + ipAddress + '</div><div id="' + deviceName + '">' + deviceName + '</div><div><img id=imgEcb' + deviceId + ' src=' + imgsrc + '></img></div></div></td>';
            cstr += '<td><div id="div_ecb' + deviceId + '" class="atcc_data" style="background-color:#ff0000;">';
            cstr += '<div style="width:20%; float:left; min-height:95px;">';
            cstr += '<img id=imgEcb' + deviceId + ' src=' + imgsrc + ' class="position: relative;top: 50%;"></img></div>';
            cstr += '<div style="width:80%; float:right;text-align:left;font-size:11px;color:#fff;">';
            cstr += '<div>Name: ' + deviceName + ' (' + ipAddress + ')</div>';
            cstr += '<div>Chainage: ' + chainage + ' Km.</div>';
            cstr += '<div>Battary: ' + 'Unknown' + '</div>';
            cstr += '<div>Door: ' + 'Unknown' + '</div>';
            cstr += '<div>Status:<span id="spStatusEcb' + deviceId + '"></span></div>';
            cstr += '</div>';
            cstr += '</div></td>';

            ++item_added;

            // Check if blank columns needed in grids
            if (item_added == tot_rec) {
                if (cur_col < col) {
                    var left_col = 0;
                    left_col = col - cur_col;

                    // Add blank columns
                    if (left_col > 0) {
                        for (var i = 0; i < left_col; i++) {
                            cstr += '<td><div class="atcc_data">&nbsp;<div></td>';
                            ++cur_col;
                        }
                        complete_row = '<tr id="tberow' + cur_row + '">' + cstr + '</tr>';
                        $(complete_row).appendTo('#ItemECBDetail');
                        cur_col = 1;
                        ++cur_row;
                        rstr = '';
                        cstr = '';
                        complete_row = '';
                    }
                }
                else if (cur_col == col) {
                    complete_row = '<tr id="tberow' + cur_row + '">' + cstr + '</tr>';
                    $(complete_row).appendTo('#ItemECBDetail');
                    cur_col = 1;
                    ++cur_row;
                    rstr = '';
                    cstr = '';
                    complete_row = '';
                }
            }
            else {
                if (cur_col == col) {
                    complete_row = '<tr id="tberow' + cur_row + '">' + cstr + '</tr>';
                    $(complete_row).appendTo('#ItemECBDetail');
                    cur_col = 1;
                    ++cur_row;
                    rstr = '';
                    cstr = '';
                    complete_row = '';
                }
                else {
                    ++cur_col;
                }
            }
        }
    });

    // Show the live status of ECB devices
    Get_ECB_live_status();
}

// Show live status of Atcc devices
function Get_ECB_live_status() {
    if (ecbes_live != null && ecbes_live) {
        $.each(ecbes_live, function (i, ecb_item) {
            var atmsId = ecb_item.atmsId;
            var chainage = ecb_item.chainage;
            var controlRoomID = ecb_item.controlRoomID;
            var deviceCategoryId = ecb_item.deviceCategoryId;
            var deviceId = ecb_item.deviceId;
            var deviceActiveImagePath = ecb_item.deviceActiveImagePath;
            var deviceInactiveImagePath = ecb_item.deviceInactiveImagePath;
            var deviceName = ecb_item.deviceName;
            var deviceStatus = ecb_item.devicePingStatus;
            var direction = ecb_item.direction;
            var ipAddress = ecb_item.ipAddress;
            var imgsrc = '';
            var strStatus = '';

            if (deviceStatus == 0) {
                imgsrc = deviceInactiveImagePath;
                strStatus = 'Inactive';
            }
            else if(deviceStatus == 1)
            {
                imgsrc = deviceActiveImagePath;
                strStatus = 'Active';
            }

            if (deviceStatus == 0) {
                $('#div_ecb' + deviceId).attr('style', "" + "background-color:#ff0000; border-color: #ff0000;" + "");
            }
            else {
                $('#div_ecb' + deviceId).attr('style', "" + "background-color:#00CE67; border-color: #00CE67;" + "");
            }
            // change the status as active or In active
            $("#spStatusEcb" + deviceId).text(strStatus);
            // change the image sourc as active or In active
            $('#imgEcb' + deviceId).attr('src', imgsrc);
        });
    }
    // Set 10 seconds interval
    setTimeout(Get_ECB_live_status, 10000);
}