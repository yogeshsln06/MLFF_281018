// Devices Initial Status
var ecbes;
var vmses;
var atcces;
var cctves;
var mets;

// Devices Live Status
var ecbes_live;
var vmses_live;
var atcces_live;
var cctves_live;
var mets_lives;

var isSessionOpened = 'false';

$(document).ready(function () {
    $('#divContent').attr('style', "width:8000px;");

    //#region ------------- ATCC ----------------------
    $("#buttonATCC").click(function () {
        OpenATCCDialog();

        $('#divAtccSummary').attr('style', 'display:block;');
        $('#divAtccDetail').attr('style', 'display:none;');

        // Change the Header of Atcc Popup
        $("#ui-id-1").text('ATCC Summary');

        // Initially show the Atcc Summary Details 
        GetAtccDetail();
    });

    $("#buttonATCCSummary").click(function () {
        $('#divAtccSummary').attr('style', 'display:block;');
        $('#divAtccDetail').attr('style', 'display:none;');

        // Change the Header of Atcc Popup
        $("#ui-id-1").text('ATCC Summary');

        // Initially show the Atcc Summary Details 
        GetAtccDetail();
    });

    $("#buttonATCCMonitor").click(function () {
        // Change the Header of Atcc Popup
        $("#ui-id-1").text('ATCC Detail');

        $("#ItemAtccDetail").find("tr:gt(0)").remove();
        $('#divAtccSummary').attr('style', 'display:none;');
        $('#divAtccDetail').attr('style', 'display:block;');
    });

    $("#buttonAtccOpenSession").click(function () {
        var selAtcc = $("#ddlAtcc").val();
        if (selAtcc <= 0) {
            alert('Please select Atcc');
            return;
        }
        isSessionOpened = 'true';

        // show atcc latest vehicle transactions..
        ShowATCCLatestTransaction();
    });

    $("#buttonAtccCloseSession").click(function () {
        isSessionOpened = 'false';
        $("#ddlAtcc").prop('selectedIndex', 0);
    });

    function ShowATCCLatestTransaction() {
        var urladdress = "/Home/ShowATCCLatestTransaction";
        var atccId = $("#ddlAtcc").val();

        if (atccId <= 0) {
            return;
        }

        if (isSessionOpened == 'false') {
            return;
        }

        $.ajax({
            type: 'GET',
            url: urladdress,
            dataType: 'json',
            async: false,
            data: {
                isSessionOpened: isSessionOpened, atccId: atccId
            },
            success: function (atccTrans) {

                $("#tblAtccRecentVehicle").find("tr:gt(0)").remove();

                // File the latest atcc transaction details
                $.each(atccTrans, function (i, atccTran) {
                    var atccTransRow = '';
                    atccTransRow = "<tr>";
                    atccTransRow += "<td>" + atccTran.ClassName + "</td>";
                    atccTransRow += "<td>" + atccTran.LaneId + "</td>";
                    atccTransRow += "<td>" + atccTran.DirectionName + "</td>";
                    atccTransRow += "<td>" + atccTran.Wheelbase + "</td>";
                    atccTransRow += "<td>" + atccTran.AxleCount + "</td>";
                    atccTransRow += "<td>" + atccTran.VehicleLength + "</td>";                    
                    atccTransRow += "<td>" + dateTimeFormat(atccTran.TransactionTime) + "</td>";
                    atccTransRow += "</tr>";
                    $("#tblAtccRecentVehicle").append(atccTransRow);
                });
            },
            error: function (ex) {
            }
        });

        if (isSessionOpened == 'true') {
            setTimeout(ShowATCCLatestTransaction, 5000); // 5 seconds interval
        }
    }
    //#endregion ATCC

    // #region ECB
    $("#buttonECB").click(function () {
        OpenECBDialog();

        $('#divECBSummary').attr('style', 'display:block;');
        $('#divECBCallMgt').attr('style', 'display:none;');
        $('#divECBCalAudit').attr('style', 'display:none;');

        // Change the Header of ECB Popup
        $("#ui-id-1").text('ECB Summary');

        // Initially show the ECB Summary Details 
        GetECBSummaryDetail();
    });

    $("#buttonECBSummary").click(function () {
        $('#divECBSummary').attr('style', 'display:block;');
        $('#divECBCallMgt').attr('style', 'display:none;');
        $('#divECBCalAudit').attr('style', 'display:none;');
    });

    $("#buttonEcbCallMgt").click(function () {
        $('#divECBSummary').attr('style', 'display:none;');
        $('#divECBCallMgt').attr('style', 'display:block;');
        $('#divECBCalAudit').attr('style', 'display:none;');

        // Populate latest ecb call history
        GetLatestECBCallHistory();
        $("#txtECBId").val('');
        $("#ddlDispositionCategory").prop('selectedIndex', 0);
        // Clear listed Facility options
        $("#txtOperatorComment").val('');
        $("#divHospitalFacility").html('');
        $("#divPoliceFacility").html('');
        $("#divFuelFacility").html('');
        $("#divHotelsFacility").html('');

        $("#txtOperatorComment").removeAttr('disabled');
        $("#buttonSaveComment").removeAttr('disabled');

        $("#buttonSaveComment").off("click"); // Remove previous added click event from button buttonSaveComment
        $("#buttonSaveComment").click(function () {
            // validate ecbId and operator comment
            var entryId = 0;
            var ecbId = "";
            var selectedCat = -1;
            var operatorComment = "";

            entryId = $("#hdEntryId").val();
            ecbId = $("#txtECBId").val();
            operatorComment = $("#txtOperatorComment").val();
            selectedCat = $("#ddlDispositionCategory").val();

            if (entryId == "" || entryId <= 0) {
                alert('Unable to get entry Id from the list.');
                return;
            }

            if (ecbId == "" || ecbId <= 0) {
                alert('Select ECB from the list');
                return;
            }
            if (selectedCat == -1) {
                alert('Please select disposition category.');
                return;
            }

            if (operatorComment == "") {
                alert('Please write the comment.');
                return;
            }

            $.ajax({
                type: "POST",
                dataType: 'json',
                cache: false,
                url: "/Home/UpdateOperatorComment",
                data: {
                    entryId: entryId,
                    comment: operatorComment,
                    dispositionCategory: selectedCat
                },
                success: function (data) {
                    if (data == 'Save') {
                        alert('Comment Saved scuccessfully.');
                        $("#hdEntryId").val('-1');
                        //$("#txtECBId").val('');
                        $("#txtOperatorComment").val('');
                        $("#ddlDispositionCategory").prop('selectedIndex', 0);
                    }
                },
                error: function () {
                    alert('Failed to update operator comment.');
                }
            });
        });
    });

    $("#buttonEcbCallAudit").click(function () {
        $('#divECBSummary').attr('style', 'display:none;');
        $('#divECBCallMgt').attr('style', 'display:none;');
        $('#divECBCalAudit').attr('style', 'display:block;');

        ResetECBAuditEntry();
    });

    function FillEcbDdl() {
        var urladdress = "/Home/GetAllECBList";
        var opt = '';
        opt = '<select id="ddlAuditECB" name="ddlAuditECB" class="dropdown">';
        opt += '<option value="-1" selected="selected">-Select-</option>';
        $.ajax({
            type: 'GET',
            url: urladdress,
            dataType: 'json',
            async: false,
            data: {},
            success: function (ecbies) {
                $.each(ecbies, function (i, ecb) {
                    opt += '<option value="' + ecb.EcbId + '">' + ecb.EcbName + '</option>';
                });
            },
            error: function (ex) {
            }
        });
        opt += '</select>';
        $("#divDdlEcb").empty();
        $("#divDdlEcb").append(opt);
    }

    function FillOperatorsDdl() {
        var urladdress = "/Home/GetAllUsersList";
        var opt = '';
        opt = '<select id="ddlECBAuditOperator" name="ddlECBAuditOperator" class="dropdown">';
        opt += '<option value="-1" selected="selected">-Select-</option>';
        $.ajax({
            type: 'GET',
            url: urladdress,
            dataType: 'json',
            async: false,
            data: {},
            success: function (operators) {
                $.each(operators, function (i, oprator) {
                    opt += '<option value="' + oprator.UserId + '">' + oprator.FirstName + ' ' + oprator.LastName + '</option>';
                });
            },
            error: function (ex) {
            }
        });
        opt += '</select>';
        $("#divDdlOperator").empty();
        $("#divDdlOperator").append(opt);
    }
    //#endregion ECB

    //#region MET
    $("#buttonMET").click(function () {
        OpenMETDialog();
        $('#divMETSummary').attr('style', 'display:block;');
        $('#divMETDetail').attr('style', 'display:none;');

        // Change the Header of MET Popup
        $("#ui-id-1").text('MET Summary');
    });

    $("#buttonMETSummary").click(function () {
        $('#divMETSummary').attr('style', 'display:block;');
        $('#divMETDetail').attr('style', 'display:none;');
        // Change the Header of MET Popup
        $("#ui-id-1").text('MET Summary');
        GetMETSummaryDetail();
    });

    $("#buttonMETMonitor").click(function () {
        $('#divMETSummary').attr('style', 'display:none;');
        $('#divMETDetail').attr('style', 'display:block;');
        // Change the Header of MET Popup
        $("#ui-id-1").text('MET Monitor');
    });

    $("#buttonMetRefresh").click(function () {
        var selMetId = '';
        selMetId = $("#ddlMets").val();
        if (selMetId <= 0) {
            alert('Please select MET.');
            return;
        }
        GetLatestMETDetails(selMetId);
    });

    //#endregion MET


    // #region CCTV
    $("#buttonCCTV").click(function () {
        OpenCCTVDialog();

        $('#divCctvSummary').attr('style', 'display:block;');
        $('#divCCTVDetail').attr('style', 'display:none;');

        // Change the Header of CCTV Popup
        $("#ui-id-1").text('CCTV Summary');
    });

    $("#buttonCCTVSummary").click(function () {
        $('#divCctvSummary').attr('style', 'display:block;');
        $('#divCCTVDetail').attr('style', 'display:none;');
        // Change the Header of CCTV Popup
        $("#ui-id-1").text('CCTV Summary');
    });

    $("#buttonCCTVMonitor").click(function () {
        $('#divCctvSummary').attr('style', 'display:none;');
        $('#divCCTVDetail').attr('style', 'display:block;');
        // Change the Header of ECB Popup
        $("#ui-id-1").text('CCTV Monitor');
    });
    //#endregion CCTV

    // Display Route Chainage... 
    DisplayRouteChainage();

    // Get all ATMS Device List from the db
    GetAllATMSDeviceList();

    // Executed every in 5 seconds
    DashboardThreadFunction();
    GetDispositionCategory();
    FillEcbDdl();
    FillOperatorsDdl();

    $("#txtECBAuditFromDate").datepicker({
        format: 'dd-mm-yyyy'
    });

    $("#txtECBAuditToDate").datepicker({
        format: 'dd-mm-yyyy'
    });

    $("#buttonEcbAuditSearch").click(function () {
        EcbAuditSerchResult();
    });

    $("#buttonEcbAuditReset").click(function () {
        ResetECBAuditEntry();
    });

    function EcbAuditSerchResult() {
        var ecbId = '';
        var operatorId = '';
        var callStatusId = '';
        var direction = '';
        var dispostionCat = '';
        var fromDate = '';
        var toDate = '';

        ecbId = $("#ddlAuditECB").val();
        operatorId = $("#ddlECBAuditOperator").val();
        callStatusId = $("#ddlECBAuditCallStatus").val();
        direction = $("#ddlECBAuditDirection").val();
        dispostionCat = $("#ddlDispositionCategoryAudit").val();
        fromDate = $("#txtECBAuditFromDate").val();
        toDate = $("#txtECBAuditToDate").val();

        // validate date
        if (fromDate != '' && toDate != '') {
            if (processdate(fromDate) > processdate(toDate)) {
                alert('From date must be less or equal to To date');
                return;
            }
        }
        // Get the filtered Call Details
        var ctr = 0;
        $("#tblECBAuditCallDetail").find("tr:gt(0)").remove();
        $.ajax({
            type: 'GET',
            url: "/Home/GetECBAuditCallDetail",
            dataType: 'json',
            async: false,
            data: {
                ecbId: ecbId, operatorId: operatorId, callStatusId: callStatusId, direction: direction, dispostionCat: dispostionCat, fromDate: fromDate, toDate: toDate
            },
            success: function (ecbCallDetails) {

                if (ecbCallDetails.length > 0) {
                    $.each(ecbCallDetails, function (i, item) {
                        $('<tr id="tablerow' + ctr + '" style="background-color:#d9d9d9">' +
                               '<td><input type="hidden" id="entryid[' + item.EntryId + ']" name="entryId[' + item.EntryId + ']" value="' + item.EntryId + '"/>' + item.EcbName + '</td>' +
                               '<td><input type="hidden" id="ecbid[' + item.EcbId + ']" name="ecbId[' + item.EcbId + ']" value="' + item.EcbId + '"/>' + item.EcbLocation + '</td>' +
                               '<td>' + item.ECBDirection + '</td>' +
                               '<td>' + JSONDateWithTime(item.CallTime) + '</td>' +
                               '<td>' + item.CallTypeName + '</td>' +
                               '<td>' + item.CallDuration + '</td>' +
                               '<td>' + item.OperatorName + '</td>' +
                               '<td>' + item.DispositionCategoryName + '</td>' +
                               '<td>' + item.OperatorComment + '</td>' +
                               '<td>' + item.AuditorComment + '</td>' +
                               '</tr>').appendTo('#tblECBAuditCallDetail');
                        ++ctr;
                    });

                    // Click Event ECB Callback List or Grid
                    $('#tblECBAuditCallDetail tr:gt(0)').click(function () {
                        $('#tblECBAuditCallDetail tr:gt(0)').css('background-color', '#D9D9D9');
                        $(this).css('background-color', '#fff');
                        var entryId = $('input[type=hidden]', $(this).find("td:first")).val();
                        // var ecbId = $('input[type=hidden]', $(this).find("td:nth-child(2)")).val();
                        $("#hfCallAuditEntryId").val(entryId);
                        //$("#videoSrc").attr("src", '');
                        $("#videoSrc").attr("src", '/images/test2.mp3');
                        var ecbAudioPlayer = $("#ecbPlayer");
                        ecbAudioPlayer[0].pause();
                        ecbAudioPlayer[0].load();//suspends and restores all audio element
                        //ecbAudioPlayer[0].oncanplaythrough = ecbAudioPlayer[0].play();
                    });
                }
                else {
                    // No information found. Clear the grid.                   
                    alert('No records found');
                }
            },
            error: function (ex) {
            }
        });

        $("#videoSrc").attr("src", '');
        var ecbAudioPlayer = $("#ecbPlayer");
        ecbAudioPlayer[0].pause();
        ecbAudioPlayer[0].load();
    }

    // Save Auditor Comment
    $("#buttonAuditorCommentSave").click(function () {
        var comment = '';
        var entryId = $("#hfCallAuditEntryId").val();
        comment = $("#txtAuditorComment").val();

        if (entryId == -1 || entryId == '') {
            alert('Entry Id not found');
            return;
        }

        if (comment == '') {
            alert('Please enter comment.');
            return;
        }

        // Save Auditor Comment in db
        $.ajax({
            type: "POST",
            dataType: 'json',
            cache: false,
            url: "/Home/UpdateAuditorComment",
            data: {
                entryId: entryId,
                comment: comment
            },
            success: function (data) {
                if (data == 'Save') {
                    alert('Auditor Comment Saved scuccessfully.');
                    $("#hfCallAuditEntryId").val('-1');
                    $("#txtAuditorComment").val('');

                    // Display data in list
                    EcbAuditSerchResult();
                }
            },
            error: function () {
                alert('Failed to update Auditor comment.');
            }
        });
    });
});

// #region ------------------ Helpter Method -------------------
function DisplayRouteChainage() {
    // top Channage
    var $topSliderDiv = $("#slider-top-addon");
    var $bottomSliderDiv = $("#slider-bottom-addon");
    var $top_dis = '';
    var $bottom_dis = '';

    for (var i = 0; i < 300; i++) {
        if (i % 2 == 0) {
            $top_dis = '<span style="top:5px;color:white; font-size:9px;position:relative;">' + '|' + i + '</span>';
        }
        else {
            $top_dis = '';
        }
        $topSliderDiv.append(
                   $('<div id=tc' + i + '>' + $top_dis + '<span id=top_atcc' + i + ' style="width:5px"/><span id=top_cctv' + i + ' style="width:5px"/><span id=top_vms' + i + ' style="width:5px"/><span id=top_ecb' + i + ' style="width:5px"/><span id=top_vids' + i + ' style="width:5px"/><span id=top_met' + i + ' style="width:5px"/></div>').css({
                       'float': 'left', 'width': '25px',
                       'background-color': '#3399FF', 'height': '5'
                   })
                );
    }

    for (var i = 0; i < 300; i++) {
        if (i % 2 == 0) {
            $bottom_dis = '<span style="top:-22px;color:white; font-size:9px;position:relative;">' + '|' + i + '</span>';
        }
        else {
            $bottom_dis = '';
        }
        $bottomSliderDiv.append(
                   $('<div id=bc' + i + '>' + $bottom_dis + '<span id=btm_atcc' + i + ' style="width:5px"/><span id=btm_cctv' + i + ' style="width:5px"/><span id=btm_vms' + i + ' style="width:5px"/><span id=btm_ecb' + i + ' style="width:5px"/><span id=btm_vids' + i + ' style="width:5px"/><span id=btm_met' + i + ' style="width:5px"/></div>').css({
                       'float': 'left', 'width': '25px',
                       'background-color': '#3399FF', 'height': '5'
                   })
               );
    }
}

function GetAllATMSDeviceList() {
    var urladdress = "/Home/GetAllATMSDeviceList";
    $.ajax({
        type: 'GET',
        url: urladdress,
        dataType: 'json',
        success: function (data) {
            // get atcc data
            ecbes = jQuery.grep(data, function (dev, i) {
                return dev.deviceCategoryId == 1;
            });

            // get vms data
            vmses = jQuery.grep(data, function (dev, i) {
                return dev.deviceCategoryId == 2;
            });

            // get mets data
            mets = jQuery.grep(data, function (dev, i) {
                return dev.deviceCategoryId == 3;
            });

            FillMetDdl(mets);

            $("#ddlMets").change(function () {
                var selMetId = '';
                selMetId = $("#ddlMets").val();
                if (selMetId > 0) {
                    GetLatestMETDetails(selMetId);
                }
                //alert($('option:selected', this).text());
            });

            // get atcc data
            atcces = jQuery.grep(data, function (dev, i) {
                return dev.deviceCategoryId == 5;
            });

            // fill Atcc ddl
            FillAtccDdl(atcces);

            // get cctv data
            cctves = jQuery.grep(data, function (dev, i) {
                return dev.deviceCategoryId == 6;
            });

            $.each(data, function (i, d) {
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

                if (deviceStatus == 0) {
                    str_status = 'Inactive';
                    imagepath = deviceInactiveImagePath;
                }
                else if (deviceStatus == 1) {
                    str_status = 'Active';
                    imagepath = deviceActiveImagePath;
                }

                var divObj;
                switch (deviceCategoryId) {
                    case 1:
                        {
                            // ecb
                            if (direction == 1) {
                                divObj = $('#top_ecb' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:10px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_ecb' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:10px;cursor:pointer;';
                            }
                            break;
                        }
                    case 2:
                        {
                            // vms
                            if (direction == 1) {
                                divObj = $('#top_vms' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_vms' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            break;
                        }
                    case 3:
                        {
                            // met
                            if (direction == 1) {
                                divObj = $('#top_met' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_met' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            break;
                        }
                    case 4:
                        {
                            // vids
                            if (direction == 1) {
                                divObj = $('#top_vids' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_vids' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                            }
                            break;
                        }
                    case 5:
                        {
                            // atcc
                            if (direction == 1) {
                                divObj = $('#top_atcc' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_atcc' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                            }
                            break;
                        }
                    case 6:
                        {
                            // cctv                            
                            if (direction == 1) {
                                divObj = $('#top_cctv' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                            }
                            else if (direction == 2) {
                                divObj = $('#btm_cctv' + chainage + '');
                                str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                            }
                            break;
                        }
                }

                $(divObj).text('');
                $(divObj).attr('style', 'float: left; width: 5px; background-color: rgb(51, 153, 255); height: 10px;');
                $('<img />').attr('src', "" + imagepath + "")
                .attr('id', 'id_' + deviceId + '_cat_' + deviceCategoryId)
                .attr('title', deviceName + '(' + str_status + ')')
                .attr('style', str_style)
                .appendTo($(divObj));
            });
        }
    });
}

function DashboardThreadFunction() {
    //var APIURL = "http://localhost:62925/api/home/GetDashboardStatusJSONData";
    var APIURL = "http://192.168.1.84:5520/api/Home/GetDashboardStatusJSONData";
    //var APIURL = "http://192.168.1.207:5520/api/Home/GetDashboardStatusJSONData";
    $.ajax({
        type: 'GET',
        url: APIURL,
        dataType: 'jsonp',
        success: function (data) {
            if (data != '') {
                var parseData = JSON.parse(data);

                // get atcc data
                ecbes_live = jQuery.grep(parseData, function (dev, i) {
                    return dev.deviceCategoryId == 1;
                });

                // get vms data
                vmses_live = jQuery.grep(parseData, function (dev, i) {
                    return dev.deviceCategoryId == 2;
                });

                // get met live data
                mets_live = jQuery.grep(parseData, function (dev, i) {
                    return dev.deviceCategoryId == 3;
                });

                // get atcc data
                atcces_live = jQuery.grep(parseData, function (dev, i) {
                    return dev.deviceCategoryId == 5;
                });


                // get cctv data
                cctves_live = jQuery.grep(parseData, function (dev, i) {
                    return dev.deviceCategoryId == 6;
                });

                $.each(parseData, function (i, d) {
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

                    var str_status = 'Active';
                    var imagepath = '';
                    var str_style = '';

                    if (deviceStatus == 0) {
                        str_status = 'Inactive';
                        imagepath = deviceInactiveImagePath;
                    }
                    else if (deviceStatus == 1) {
                        str_status = 'Active';
                        imagepath = deviceActiveImagePath;
                    }

                    var divObj;
                    switch (deviceCategoryId) {
                        case 1:
                            {
                                // ecb
                                if (direction == 1) {
                                    divObj = $('#top_ecb' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:10px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_ecb' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:10px;cursor:pointer;';
                                }
                                break;
                            }
                        case 2:
                            {
                                // vms
                                if (direction == 1) {
                                    divObj = $('#top_vms' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_vms' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                break;
                            }
                        case 3:
                            {
                                // met
                                if (direction == 1) {
                                    divObj = $('#top_met' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_met' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                break;
                            }
                        case 4:
                            {
                                // vids
                                if (direction == 1) {
                                    divObj = $('#top_vids' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_vids' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:15px;cursor:pointer;';
                                }
                                break;
                            }
                        case 5:
                            {
                                // atcc
                                if (direction == 1) {
                                    divObj = $('#top_atcc' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_atcc' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                                }
                                break;
                            }
                        case 6:
                            {
                                // cctv                            
                                if (direction == 1) {
                                    divObj = $('#top_cctv' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                                }
                                else if (direction == 2) {
                                    divObj = $('#btm_cctv' + chainage + '');
                                    str_style = 'top:-45px;position:relative;left:5px;cursor:pointer;';
                                }
                                break;
                            }
                    }

                    $(divObj).text('');
                    $(divObj).attr('style', 'float: left; width: 5px; background-color: rgb(51, 153, 255); height: 5px;');
                    $('<img />').attr('src', "" + imagepath + "")
                    .attr('id', 'id_' + deviceId + '_cat_' + deviceCategoryId)
                    .attr('title', deviceName + '(' + str_status + ')')
                    .attr('style', str_style)
                    .attr('onclick', 'myfunction(' + deviceCategoryId + ', ' + deviceId + ')')
                    .appendTo($(divObj));

                });
            }
        }
    });
    setTimeout(DashboardThreadFunction, 5000); // 5 seconds interval
}

function myfunction(catId, id) {
    alert(catId + ', ' + id);
}

function GetLatestECBCallHistory() {
    var ctr = 0;
    var urladdress = "/Home/GetLatestECBCallHistory";

    // Remove and clear existed ECB device information    
    $("#tblECBCallHistory").find("tr:gt(0)").remove();
    $.ajax({
        type: 'GET',
        url: urladdress,
        dataType: 'json',
        success: function (ecbCallHistory) {
            $.each(ecbCallHistory, function (i, item) {
                $('<tr id="tablerow' + ctr + '" style="background-color:#d9d9d9">' +
                       '<td><input type="hidden" id="entryid[' + item.EntryId + ']" name="entryId[' + item.EntryId + ']" value="' + item.EntryId + '"/>' + item.EcbName + '</td>' +
                       '<td><input type="hidden" id="ecbid[' + item.EcbId + ']" name="ecbId[' + item.EcbId + ']" value="' + item.EcbId + '"/>' + item.EcbLocation + '</td>' +
                       '<td><input type="hidden" id="auditorComment[' + item.AuditorComment + ']" name="auditorComment[' + item.AuditorComment + ']" value="' + item.AuditorComment + '"/>' + item.ECBDirection + '</td>' +
                       '<td><input type="hidden" id="operatorComment[' + item.OperatorComment + ']" name="operatorComment[' + item.OperatorComment + ']" value="' + item.OperatorComment + '"/>' + JSONDateWithTime(item.CallTime) + '</td>' +
                       '<td>' + item.CallDuration + '</td>' +
                       '<td>' + item.CallTypeName + '</td>' +
                       '</tr>').appendTo('#tblECBCallHistory');
                ++ctr;
            });            

            // Click Event ECB Callback List or Grid
            $('#tblECBCallHistory tr:gt(0)').click(function () {
                $('#tblECBCallHistory tr:gt(0)').css('background-color', '#D9D9D9');
                $(this).css('background-color', '#007ACC');
                var entryId = $('input[type=hidden]', $(this).find("td:first")).val();
                var ecbId = $('input[type=hidden]', $(this).find("td:nth-child(2)")).val();

                // get Auditor Comment. If Found then disable the Operator Comment.
                var auditorComment = $('input[type=hidden]', $(this).find("td:nth-child(3)")).val();
                if (auditorComment != '') {
                    // get Auditor Comment. If Found then disable the Operator Comment.
                    $("#txtOperatorComment").val(operatorComment = $('input[type=hidden]', $(this).find("td:nth-child(4)")).val());

                    // Get Operator Comment and Disable Operataor comment text box and save button
                    $("#txtOperatorComment").attr('disabled', 'disabled');
                    $("#buttonSaveComment").attr('disabled', 'disabled');
                }
                else {
                    $("#txtOperatorComment").removeAttr('disabled');
                    $("#buttonSaveComment").removeAttr('disabled');
                }

                // Assign ECBId and Entry Id
                $("#txtECBId").val(ecbId);
                $("#hdEntryId").val(entryId);

                // Fill Available Facilities
                var facilites;
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    url: "/Home/GetFacilitiesByECBId",
                    data: {
                        ecbId: ecbId
                    },
                    success: function (facilities) {
                        var facHospital = '<ul>';
                        var facPolice = '<ul>';
                        var facFuel = '<ul>';
                        var facHotel = '<ul>';

                        $.each(facilities, function (i, fac) {
                            var facCatId = fac.FacilityCategoryId;
                            switch (facCatId) {
                                case 1:
                                    {
                                        facHospital += '<li>' + fac.FacilityDetail + '</li>';
                                        break;
                                    }
                                case 2:
                                    {
                                        facPolice += '<li>' + fac.FacilityDetail + '</li>';
                                        break;
                                    }
                                case 3:
                                    {
                                        facFuel += '<li>' + fac.FacilityDetail + '</li>';
                                        break;
                                    }
                                case 4:
                                    {
                                        facHotel += '<li>' + fac.FacilityDetail + '</li>';
                                        break;
                                    }
                            }
                        });

                        facHospital += '</ul>';
                        facPolice += '</ul>';
                        facFuel += '</ul>';
                        facHotel += '</ul>';
                        $("#divHospitalFacility").html(facHospital);
                        $("#divPoliceFacility").html(facPolice);
                        $("#divFuelFacility").html(facFuel);
                        $("#divHotelsFacility").html(facHotel);
                    },
                    error: function () {
                        alert('Failed to get facilities.');
                    }
                });
            });
        }
    });
}

function ResetECBAuditEntry() {
    $("#ddlAuditECB").prop('selectedIndex', 0);
    $("#ddlECBAuditOperator").prop('selectedIndex', 0);
    $("#ddlECBAuditCallStatus").prop('selectedIndex', 0);
    $("#ddlECBAuditDirection").prop('selectedIndex', 0);
    $("#ddlDispositionCategoryAudit").prop('selectedIndex', 0);
    $("#txtECBAuditFromDate").val('');
    $("#txtECBAuditToDate").val('');

    $("#hfCallAuditEntryId").val('-1');
    $("#txtAuditorComment").val('');
    $("#tblECBAuditCallDetail").find("tr:gt(0)").remove();

}

function JSONDateWithTime(dateStr) {
    jsonDate = dateStr;
    var d = new Date(parseInt(jsonDate.substr(6)));
    var m, day;
    m = d.getMonth() + 1;
    if (m < 10)
        m = '0' + m
    if (d.getDate() < 10)
        day = '0' + d.getDate()
    else
        day = d.getDate();
    var formattedDate = day + "/" + m + "/" + d.getFullYear();
    var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
    var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var formattedTime = hours + ":" + minutes + ":" + d.getSeconds();
    formattedDate = formattedDate + " " + formattedTime;
    return formattedDate;
}

function GetDispositionCategory() {
    var urladdress = "/Home/GetDispositionCategory";
    var opt = '';
    var opt1 = '';
    $.ajax({
        type: 'GET',
        url: urladdress,
        dataType: 'json',
        data: {},
        success: function (category) {
            opt = '<select id="ddlDispositionCategory" name="ddlDispositionCategory" class="dropdown">';
            opt += '<option value="-1" selected="selected">-Select-</option>';
            opt1 = '<select id="ddlDispositionCategoryAudit" name="ddlDispositionCategoryAudit" class="dropdown">';
            opt1 += '<option value="-1" selected="selected">-Select-</option>';

            $.each(category, function (i, cat) {
                opt += '<option value="' + cat.CategoryId + '">' + cat.CategoryName + '</option>';
                opt1 += '<option value="' + cat.CategoryId + '">' + cat.CategoryName + '</option>';
            });
            opt += '<option value="-99">Other</option>';
            opt += '</select>';
            opt1 += '<option value="-99">Other</option>';
            opt1 += '</select>';
            $("#divDispositionCategory").empty();
            $("#divDispositionCategory").append(opt);
            $("#divDispositionCategoryAudit").empty();
            $("#divDispositionCategoryAudit").append(opt1);
        },
        error: function (ex) {
            opt = '<select id="ddlDispositionCategory" name="ddlDispositionCategory" class="dropdown">';
            opt += '<option value="-1" selected="selected">-Select-</option>';
            opt += '</select>';

            opt1 = '<select id="ddlDispositionCategoryAudit" name="ddlDispositionCategoryAudit" class="dropdown">';
            opt1 += '<option value="-1" selected="selected">-Select-</option>';
            opt1 += '</select>';

            $("#divDispositionCategory").empty();
            $("#divDispositionCategory").append(opt);

            $("#divDispositionCategoryAudit").empty();
            $("#divDispositionCategoryAudit").append(opt1);
            //alert('Failed to retrieve Dispostion Categories.' + ex);
        }
    });
    return opt;
}

function processdate(date) {
    var parts = date.split("-");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function dateTimeFormat(dateTimeValue) {
    var dt = new Date(parseInt(dateTimeValue.replace(/(^.*\()|([+-].*$)/g, '')));
    var dateTimeFormat = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear() + ' ' + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return dateTimeFormat;
}

function FillAtccDdl(atcces) {

    var opt = '';
    opt = '<select id="ddlAtcc" name="ddlAtcc" class="dropdown w125">';
    opt += '<option value="-1" selected="selected">-Select-</option>';
    if (atcces.length > 0) {
        $.each(atcces, function (i, atcc) {
            opt += '<option value="' + atcc.deviceId + '">' + atcc.deviceName + '</option>';
        });
    }
    opt += '</select>';
    $("#divATCCddl").empty();
    $("#divATCCddl").append(opt);
}

function FillMetDdl(mets) {

    var opt = '';
    opt = '<select id="ddlMets" name="ddlMets" class="dropdown w125">';
    opt += '<option value="-1" selected="selected">-Select-</option>';
    if (mets.length > 0) {
        $.each(mets, function (i, met) {
            opt += '<option value="' + met.deviceId + '">' + met.deviceName + '</option>';
        });
    }
    opt += '</select>';
    $("#divMetsCollections").empty();
    $("#divMetsCollections").append(opt);
}

function GetLatestMETDetails(selMetId) {
    var urladdress = '/Home/GetLatestMetCurrentDataByMetId';
    // Get the latest Met Information by met Id.
    $.ajax({
        type: 'GET',
        url: urladdress,
        dataType: 'json',
        data: {
            metId: selMetId
        },
        success: function (curMetData) {
            var strMetData = '';
            var lastUpdatedDate = '';
            strMetData = '<table class="table table-bordered table-responsive">'

            if (curMetData.length > 0) {
                $.each(curMetData, function (i, metInfo) {
                    if (lastUpdatedDate.length == 0) {
                        lastUpdatedDate = metInfo.DataCollectionDate + ' ' + metInfo.DataCollectionTime;
                    }

                    switch (metInfo.MetInfoType) {
                        case 1:
                            {
                                // Air Temp
                                strMetData += '<tr><td> Air Temprature: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 2:
                            {
                                //Humidity
                                strMetData += '<tr><td> Humidity: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 3:
                            {
                                // Visibility
                                strMetData += '<tr><td> Visibility: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 4:
                            {
                                //Road Temp
                                strMetData += '<tr><td> Road Temprature: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 5:
                            {
                                // Wind Direction
                                strMetData += '<tr><td> Wind Direction: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 6:
                            {
                                // Wind Speed
                                strMetData += '<tr><td> Wind Speed: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                        case 7:
                            {
                                // Rain
                                strMetData += '<tr><td> Rain: </td><td>' + metInfo.DataValue + ' ' + metInfo.DataUnit + '</td></tr>';
                                break;
                            }
                    }
                });
                strMetData += '</table>';

                // Last Updated Date
                $("#divMetLastUpdatedDate").empty();
                $("#divMetLastUpdatedDate").append(lastUpdatedDate);

                // Show the selected MET's complete current information
                $("#divSelectedMetDetails").empty();
                $("#divSelectedMetDetails").append(strMetData);

            }
            else {
                alert('No current information found');
            }
        },
        error: function (ex) {
            alert('Unable to read met information.');
        }
    });
}
// #endregion