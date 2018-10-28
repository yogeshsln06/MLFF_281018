var optCCTV = {
    autoOpen: false,
    resizable: false,
    modal: true,
    width: 1050,
    height: 560,
    title: 'CCTV',
    open: function (e) {
        $(e.target).parent().css('background-color', '#1A5276');
    }
};

function OpenCCTVDialog() {
    $("#CCTV-Dialog-Box").dialog(optCCTV).dialog("open");

    // Display Initial CCTV devices
    GetCCTVDetail();
}

function GetCCTVDetail() {
    // Variable Declaration
    var rstr = '';
    var cstr = '';
    var cur_row = 1;
    var cur_col = 1;
    var complete_row = '';
    var item_added = 0;
    var col = 0;

    // Get the Atcc Data and Count
    var cctvDetails = cctves;
    var tot_rec = cctves.length;

    if (tot_rec > 0) {
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

        // Remove and clear existed ATCC device information
        $("#ItemCCTVDetail").find("tr").remove();

        // Display All Atcc devices on the screen
        if (cctvDetails != null && cctvDetails.length > 0) {
            $.each(cctvDetails, function (i, d) {
                var atmsId = d.atmsId;
                var chainage = d.chainage;
                var controlRoomID = d.controlRoomID;
                var deviceCategoryId = d.deviceCategoryId;
                var deviceId = d.deviceId;
                var deviceActiveImagePath = d.deviceActiveImagePath;
                var deviceInactiveImagePath = d.deviceInactiveImagePath;
                var deviceName = d.deviceName;
                var deviceStatus = d.devicePingStatus;
                var direction = d.direction;
                var ipAddress = d.ipAddress;
                var imgsrc = '';

                if (deviceStatus == 0) {
                    imgsrc = deviceInactiveImagePath;
                }
                else if (deviceStatus == 1) {
                    imgsrc = deviceActiveImagePath;
                }

                if (cur_col <= col) {
                    //cstr += '<td><div id="div_cctv' + deviceId + '" class="atcc_data"><div id="' + deviceId + '">' + ipAddress + '</div><div id="' + deviceName + '">' + deviceName + '</div><div><img id=imgCCTV' + deviceId + ' src=' + imgsrc + '></img></div></div></td>';
                    cstr += '<td><div id="div_cctv' + deviceId + '" class="atcc_data" style="background-color:#ff0000;">';
                    cstr += '<div style="width:20%; float:left; min-height:95px;">';
                    cstr += '<img id=imgCCTV' + deviceId + ' src=' + imgsrc + ' class="position: relative;top: 50%;"></img>';
                    cstr += '</div>';
                    cstr += '<div style="width:80%; float:right;text-align:left;font-size:11px;color:#fff;">';
                    cstr += '<div>Name: ' + deviceName + ' (' + ipAddress + ')</div>';
                    cstr += '<div>Chainage: ' + chainage + ' Km., Direction: ' + (direction == 1 ? 'Left' : 'Right') + '</div>';
                    cstr += '<div>Status:<span id="spStatusCctv' + deviceId + '"></span></div>';
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
                                $(complete_row).appendTo('#ItemCCTVDetail');
                                cur_col = 1;
                                ++cur_row;
                                rstr = '';
                                cstr = '';
                                complete_row = '';
                            }
                        }
                        else if (cur_col == col) {
                            complete_row = '<tr id="tberow' + cur_row + '">' + cstr + '</tr>';
                            $(complete_row).appendTo('#ItemCCTVDetail');
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
                            $(complete_row).appendTo('#ItemCCTVDetail');
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
        }
        // Show the live status of CCTV devices
        Get_CCTV_live_status();
    }
}

// Show live status of Atcc devices
function Get_CCTV_live_status() {
    if (cctves_live != null && cctves_live.length > 0) {
        $.each(cctves_live, function (i, cctv_item) {
            var atmsId = cctv_item.atmsId;
            var chainage = cctv_item.chainage;
            var controlRoomID = cctv_item.controlRoomID;
            var deviceCategoryId = cctv_item.deviceCategoryId;
            var deviceId = cctv_item.deviceId;
            var deviceActiveImagePath = cctv_item.deviceActiveImagePath;
            var deviceInactiveImagePath = cctv_item.deviceInactiveImagePath;
            var deviceName = cctv_item.deviceName;
            var deviceStatus = cctv_item.devicePingStatus;
            var direction = cctv_item.direction;
            var ipAddress = cctv_item.ipAddress;
            var imgsrc = '';
            var strStatus = '';

            if (deviceStatus == 0) {
                $('#div_cctv' + deviceId).attr('style', "" + "background-color:#ff0000; border-color: #ff0000;" + "");
                imgsrc = deviceInactiveImagePath;
                
            }
            else if (deviceStatus == 1) {
                $('#div_cctv' + deviceId).attr('style', "" + "background-color:#00CE67; border-color: #00CE67;" + "");
                imgsrc = deviceActiveImagePath;
                strStatus = "Active";
            }

            $('#spStatusCctv' + deviceId).text(strStatus);
            // change the image sourc as active or In active
            $('#imgCCTV' + deviceId).attr('src', imgsrc);
        });
    }
    // Set 10 seconds interval
    setTimeout(Get_CCTV_live_status, 10000);
}