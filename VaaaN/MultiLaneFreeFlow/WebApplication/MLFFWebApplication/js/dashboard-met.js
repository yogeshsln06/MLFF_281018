var optMet = {
    autoOpen: false,
    resizable: false,
    modal: true,
    width: 1050,
    height: 563,
    title: 'MET',
    open: function (e) {
        $(e.target).parent().css('background-color', '#1A5276');
        $(e.target).parent().css('top', '10px');
        $(e.target).parent().css('height', '563px');
    }
};

function OpenMETDialog() {
    $("#MET-Dialog-Box").dialog(optMet).dialog("open");

    GetMETSummaryDetail();
}

function GetMETSummaryDetail() {
    // Variable Declaration
    var rstr = '';
    var cstr = '';
    var cur_row = 1;
    var cur_col = 1;
    var complete_row = '';
    var item_added = 0;
    var col = 0;
    // Get the Met Data and Count
    // var metDetails = mets;
    var tot_rec = mets.length; // Object.keys(metDetails.METSummaryData).length;

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

    // Remove and clear existed MET device information
    $("#ItemMETDetail").find("tr").remove();

    // Display All MET devices on the screen
    $.each(mets, function (i, met_item) {
        var atmsId = met_item.atmsId;
        var chainage = met_item.chainage;
        var controlRoomID = met_item.controlRoomID;
        var deviceCategoryId = met_item.deviceCategoryId;
        var deviceId = met_item.deviceId;
        var deviceActiveImagePath = met_item.deviceActiveImagePath;
        var deviceInactiveImagePath = met_item.deviceInactiveImagePath;
        var deviceName = met_item.deviceName;
        var deviceStatus = met_item.devicePingStatus;
        var direction = met_item.direction;
        var ipAddress = met_item.ipAddress;
        var imgsrc = '';
        
        if (deviceStatus == 0) {
            imgsrc = deviceInactiveImagePath;
        }
        else if(deviceStatus == 1)
        {
            imgsrc = deviceActiveImagePath;
        }

        if (cur_col <= col) {
            cstr += '<td><div id="div_met' + deviceId + '" class="atcc_data" style="background-color:#ff0000;">';
            cstr += '<div style="width:20%; float:left; min-height:95px;">';
            cstr += '<img id=imgMet' + deviceId + ' src=' + imgsrc + ' class="position: relative;top: 50%;"></img><div style="text-align:left;font-size:11px;color:#fff;">Chainage: ' + chainage + ' Km.</div></div>';
            cstr += '<div style="width:80%; float:right;text-align:left;font-size:11px;color:#fff;">';
            cstr += '<div>Name:' + deviceName + ' (' + ipAddress + ')</div>';
            cstr += '<div>Temp:<span id="spTempMet' + deviceId + '"></span><span id="spTempUnitMet' + deviceId + '"></span>, Road Temp:<span id="spRoadTempMet' + deviceId + '"></span><span id="spRoadTempUnitMet' + deviceId + '"></span></div>';
            cstr += '<div>Visibility:<span id="spVisibilityMet' + deviceId + '"></span><span id="spVisibilityUnitMet' + deviceId + '"></span>, Humidity:<span id="spHumidityMet' + deviceId + '"></span><span id="spHumidityUnitMet' + deviceId + '"></span></div>';
            cstr += '<div>Wind Direction:<span id="spWindDirecMet' + deviceId + '"></span><span id="spWindDirecUnitMet' + deviceId + '"></span>, Wind Speed:<span id="spWindSpeedMet' + deviceId + '"></span><span id="spWindSpeedUnitMet' + deviceId + '"></span></div>';
            cstr += '<div>Rain:<span id="spRainMet' + deviceId + '"></span><span id="spRainUnitMet' + deviceId + '"></span>, Status:<span id="spStatusMet' + deviceId + '"></span></div>';
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
                        $(complete_row).appendTo('#ItemMETDetail');
                        cur_col = 1;
                        ++cur_row;
                        rstr = '';
                        cstr = '';
                        complete_row = '';
                    }
                }
                else if (cur_col == col) {
                    complete_row = '<tr id="tberow' + cur_row + '">' + cstr + '</tr>';
                    $(complete_row).appendTo('#ItemMETDetail');
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
                    $(complete_row).appendTo('#ItemMETDetail');
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

    // Show the live status of MET devices
    Get_MET_live_status();
}

// Show live status of Atcc devices
function Get_MET_live_status() {
    if (mets_live != null && mets_live) {
        $.each(mets_live, function (i, met_item) {
            var atmsId = met_item.atmsId;
            var chainage = met_item.chainage;
            var controlRoomID = met_item.controlRoomID;
            var deviceCategoryId = met_item.deviceCategoryId;
            var deviceId = met_item.deviceId;
            var deviceActiveImagePath = met_item.deviceActiveImagePath;
            var deviceInactiveImagePath = met_item.deviceInactiveImagePath;
            var deviceName = met_item.deviceName;
            var deviceStatus = met_item.devicePingStatus;
            var direction = met_item.direction;
            var ipAddress = met_item.ipAddress;
            var imgsrc = '';

            if (deviceStatus == 0) {
                imgsrc = deviceInactiveImagePath;
            }
            else if(deviceStatus == 1)
            {
                imgsrc = deviceActiveImagePath;
            }

            if (deviceStatus == 0) {
                $('#div_met' + deviceId).attr('style', "" + "background-color:#ff0000; border-color: #ff0000;" + "");
                $('#spStatusMet' + deviceId).text('Inactive');
            }
            else if (deviceStatus == 1) {
                $('#div_met' + deviceId).attr('style', "" + "background-color:#00CE67; border-color: #00CE67;" + "");
                $('#spStatusMet' + deviceId).text('Active');
            }

            // change the image sourc as active or In active
            $('#imgMet' + deviceId).attr('src', imgsrc);
            GetMetCurrInfo(deviceId);
        });
    }
    // Set 10 seconds interval
    setTimeout(Get_MET_live_status, 10000);
}


function GetMetCurrInfo(metId) {
    // var lastUpdatedDate = '';
    // Get the latest Met Information by met Id.
    $.ajax({
        type: 'GET',
        url: '/Home/GetLatestMetCurrentDataByMetId',
        dataType: 'json',
        cache: false,
        data: {
            metId: metId
        },
        success: function (metCurrData) {
            if (metCurrData.length > 0) {
                $.each(metCurrData, function (a, metCurrItem) {
                    switch (metCurrItem.MetInfoType) {
                        case 1:
                            {
                                // Air Temp
                                $('#spTempMet' + metId).text(metCurrItem.DataValue);
                                $('#spTempUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 2:
                            {
                                // Humidity
                                $('#spHumidityMet' + metId).text(metCurrItem.DataValue);
                                $('#spHumidityUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 3:
                            {
                                // Visibility
                                $('#spVisibilityMet' + metId).text(metCurrItem.DataValue);
                                $('#spVisibilityUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 4:
                            {
                                // Road Temp
                                $('#spRoadTempMet' + metId).text(metCurrItem.DataValue);
                                $('#spRoadTempUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 5:
                            {
                                // Wind Direction
                                $('#spWindDirecMet' + metId).text(metCurrItem.DataValue);
                                $('#spWindDirecUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 6:
                            {
                                // Wind Speed
                                $('#spWindSpeedMet' + metId).text(metCurrItem.DataValue);
                                $('#spWindSpeedUnitMet' + metId).text(metCurrItem.DataUnit);
                                break;
                            }
                        case 7:
                            {
                                // Rain
                                $('#spRainMet' + metId).text(metCurrItem.DataValue);
                                break;
                            }
                    }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {           
            //or you can put jqXHR.responseText somewhere as complete response. Its html.
        }
    });   
}
