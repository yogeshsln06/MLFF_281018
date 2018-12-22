var pageload = 1;
var pagesize = 30;
var NoMoredata = false;
var inProgress = false;


$(document).ready(function () {
    BindCustmerAccount();
});
function closePopup() {
    $("#btnpopupClose").trigger('click');
    $(".modal-backdrop").hide()
}

function reloadData() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "CustomerAccountListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;

        },
        error: function (ex) {
            $(".animationload").hide();


        }

    });
}

function BindCustmerAccount() {
    pageload = 1;
    $(".animationload").show();
    inProgress = true;
    var Inputdata = { pageindex: pageload, pagesize: pagesize }
    $.ajax({
        type: "POST",
        dataType: "json",
        data: JSON.stringify(Inputdata),
        url: "CustomerAccountListScroll",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".animationload").hide();
            NoMoredata = data.length < pagesize
            pageload++;
            datatableVariable = $('#tblCustomerData').DataTable({
                data: data,
                scrollY: 230,
                scrollCollapse: true,
                scroller: {
                    loadingIndicator: true
                },
                bJQueryUI: true,
                stateSave: true,
                paging: false,
                columns: [
                     { 'data': 'AccountId', "autowidth": true },
                    {
                        'data': 'AccountId', "autowidth": true,
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<a href='javascript:void(0);' onclick='DetailsOpen(this," + oData.AccountId + ")'>" + oData.AccountId + "</a>");
                        }
                    },
                    { 'data': 'ResidentId', "autowidth": true },
                    { 'data': 'FirstName', "autowidth": true },
                    { 'data': 'Address', "autowidth": true },
                    { 'data': 'MobileNo', "autowidth": true },
                    { 'data': 'EmailId', "autowidth": true },
                   {
                       'data': 'AccountId', "autowidth": true,
                       fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                           $(nTd).html("<div class='dropdown'>" +
                              "<a class='dropdown-toggle no-after peers fxw-nw ai-c lh-1' data-toggle='dropdown' href='javascript:void(0);' id='dropdownMenuButton' aria-haspopup='true' aria-expanded='false' onclick='openFilter(this)' id='gridbtn'>" +
                   "<span class='icon-holder'>" +
                        "<i class='c-blue-500 ti-angle-down'></i>" +
                    "</span>" +
                "</a>" +
                " <div class='dropdown-menu dropdown-menu-right myfilter gridbtn' role='menu' id='ddlFilter' style='width:160px; left:110px!important;'>" +
                "    <a class='dropdown-item' href='javascript:void(0);' onclick='EditOpen(this," + oData.AccountId + ")'>" +
                "        <span class='icon-holder'>" +
                "            <i class='c-blue-500 ti-pencil'></i>" +
                "        </span>" +
                "        <span class='title'>Edit</span>" +
                "    </a>" +
                "    <a class='dropdown-item' href='javascript:void(0);' onclick='HistoryRecords(this," + oData.AccountId + ")'>" +
                "        <span class='icon-holder'>" +
                "            <i class='c-blue-500 ti-exchange-vertical'></i>" +
                "        </span>" +
                "        <span class='title'>History</span>" +
                "    </a>" +
                "</div>" +
            "</div>");
                       }
                   },
                ],
                columnDefs: [{ "orderable": false, "targets": 7 }],
                order: [[1, 'asc']]
            });
            datatableVariable.on('order.dt search.dt', function () {
                datatableVariable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            inProgress = false;
            $('.dataTables_scrollBody').on('scroll', function () {
                if (($('.dataTables_scrollBody').scrollTop() + $('.dataTables_scrollBody').height() >= $("#tblCustomerData").height()) && !NoMoredata && !inProgress) {
                    AppendCustomerData();
                }
            });
        },
        error: function (ex) {
            $(".animationload").hide();
        }
    });
}

function AppendCustomerData() {
    inProgress = true;
    $('#loadingdiv').show()
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "CustomerAccountListScroll?pageIndex=" + pageload + "&pagesize=" + pagesize,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#loadingdiv').hide()
            //datatableVariable.clear().draw();
            datatableVariable.rows.add(data); // Add new data
            datatableVariable.columns.adjust().draw();
            pageload++;
            NoMoredata = data.length < pagesize;
            inProgress = false;
        },
        error: function (ex) {
            $('#loadingdiv').hide()
        }

    });

}



function GetCityList() {
    var ProvinceId = $("#ProvinceId").val();
    $.ajax
    ({
        url: '/Registration/GetCityList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            ProvinceId: +ProvinceId
        }),
        success: function (result) {
            $("#CityId").html("");
            $("#DistrictId").html("");
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');

            $("#CityId").append
               ($('<option></option>').val(0).html('--Select Kabupaten/Kota--'));

            $("#DistrictId").append
           ($('<option></option>').val(0).html('--Select Kecamatan--'));

            $("#SubDistrictId").append
          ($('<option zipcode=""></option>').val(0).html('--Select Kelurahan/Desa--'));

            $.each($.parseJSON(result), function (i, city) {
                $("#CityId").append
                ($('<option></option>').val(city.CityId).html(city.CityName))
                if (city.CityId == $("#hfCityId").val() && pageload == 1) {
                    $("#CityId").val($("#hfCityId").val());
                    GetDistrictList();
                }
            })
        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetDistrictList() {
    var CityId = $("#CityId").val();
    $.ajax
    ({
        url: '/Registration/GetDistrictList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            CityId: +CityId
        }),
        success: function (result) {

            $("#DistrictId").html("");
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');
            $("#DistrictId").append
            ($('<option></option>').val(0).html('--Select Kecamatan--'));

            $("#SubDistrictId").append
        ($('<option zipcode=""></option>').val(0).html('--Select Kelurahan/Desa--'));
            $.each($.parseJSON(result), function (i, district) {
                $("#DistrictId").append
                ($('<option></option>').val(district.DistrictId).html(district.DistrictName))
                if (district.DistrictId == $("#hfDistrictId").val() && pageload == 1) {
                    $("#DistrictId").val($("#hfDistrictId").val());
                    GetSubDistrictList();
                }
            })



        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetSubDistrictList() {
    var DistrictId = $("#DistrictId").val();
    $.ajax
    ({
        url: '/Registration/GetSubDistrictList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({
            DistrictId: +DistrictId
        }),
        success: function (result) {
            $("#SubDistrictId").html("");
            $('#PostalCode').val('');
            $("#SubDistrictId").append
            ($('<option zipcode=""></option>').val(0).html('--Select Kelurahan/Desa--'));
            $.each($.parseJSON(result), function (i, subdistrict) {
                $("#SubDistrictId").append
                ($('<option zipcode="' + subdistrict.ZipCode + '"></option>').val(subdistrict.SubDistrictId).html(subdistrict.SubDistrictName))

                if (subdistrict.SubDistrictId == $("#hfSubDistrictId").val() && pageload == 1) {
                    $("#SubDistrictId").val($("#hfSubDistrictId").val());
                    $("#PostalCode").val($("#hfPostalCode").val());
                    pageload = pageload + 1;
                }
            })
        },
        error: function () {
            alert("Whooaaa! Something went wrong..")
        },
    });
}

function GetZip(ctrl) {
    var option = $('option:selected', ctrl).attr('zipcode');
    $('#PostalCode').val(option || '');
}

function validEmail(str) {
    var pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return String(str).match(pattern);
}

function validMobile(str) {
    var pattern = /^\d+$/;
    return String(str).match(pattern);
}

function validateCustomer() {
    var valid = true;
    if ($("#ResidentId").val() == '') {
        showError($("#ResidentId"), $("#ResidentId").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#ResidentId"), '');
    }
    if ($("#FirstName").val() == '') {
        showError($("#FirstName"), $("#FirstName").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#FirstName"), '');
    }
    if ($("#MobileNo").val() == '') {
        showError($("#MobileNo"), $("#MobileNo").attr('data-val-required'));
        valid = false;
    }
    else if (!validMobile($("#MobileNo").val())) {
        showError($("#MobileNo"), 'Enter Valid Mobile Number');
        valid = false;
    }
    else {
        showError($("#MobileNo"), '');
    }
    if ($("#EmailId").val() == '') {
        showError($("#EmailId"), $("#EmailId").attr('data-val-required'));
        valid = false;
    }
    else if (!validEmail($("#EmailId").val())) {
        showError($("#EmailId"), 'Enter Valid Email Id');
        valid = false;
    }
    else {
        showError($("#EmailId"), '');
    }
    if ($("#Address").val() == '') {
        showError($("#Address"), $("#Address").attr('data-val-required'));
        valid = false;
    }
    else {
        showError($("#Address"), '');
    }
    if ($('#ResidentidImage')[0].files.length === 0 && $('#hfCustomerDocumentPath').val() == "") {
        showError($('#ResidentidImage'), "Identity Card Image Required");
        valid = false;
    }
    else {
        showError($("#imagepath"), '');
    }

    return valid;
}

function showError(ctrlid, errorMsg) {
    $(ctrlid).next().text(errorMsg);
}

function openpopup() {
    $("#warning").hide();
    $('#customerModal').modal('show');
}



function OpenCustmerRegistration() {
    $.ajax({
        type: "GET",
        url: "/MRM/AssociatedTransaction?transactionId=" + dataid + "&TransactionCategoryId=" + $("#ddlTransactionCategory").val(),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $('#loader').hide();
            $('#partialassociated').html(result);
            $('#partialassociated').show();
            $('#Associatedmodel').modal('show');

        },
        error: function (x, e) {
            $('#loader').hide();
            $('#Associatedmodel').modal('hide');
        }

    });
}

function NewCustomer() {
    $(".animationload").show();
    $.ajax({
        type: "GET",
        url: "/Registration/NewCustomer",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $("#exampleModalLabel").text("New Customer");
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#BirthDate").attr("data-provide", "datepicker").attr("readolny", true);
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });
}

function DetailsOpen(ctrl, id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomer?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialassociated').html(result);
            $("#exampleModalLabel").text("View [" + $("#FirstName").val() + "]");
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            openpopup();
           
            $("#fildset").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());
            $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            GetCityList();
            if ($("#hfBirthPlace").val() || '' != '') {
                $('#BirthPlace option').each(function (index, option) {
                    if (option.text == $("#hfBirthPlace").val()) {
                        $(this).attr("selected", "selected");
                    }
                    // option will contain your item
                });
                //$("#BirthPlace option[text='" + $("#hfBirthPlace").val() + "']").attr("selected", "selected");
            }
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function EditOpen(ctrl, id) {
    $(".animationload").show();
    $.ajax({
        type: "POST",
        url: "/Registration/GetCustomer?id=" + id,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (result) {
            $(".animationload").hide();
            $('#partialassociated').html(result);
            $('form').attr("id", "needs-validation").attr("novalidate", "novalidate");
            $("#exampleModalLabel").text("Update [" + $("#FirstName").val() + "]");
            openpopup();
            $("#AccountId").attr("disabled", "disabled");
            $("#ProvinceId").val($("#hfProvinceId").val());
            $("#imgPreview").attr('src', "../Attachment/Customer/" + $("#hfCustomerDocumentPath").val());
            GetCityList();
          
            $("#ValidUntil").attr("data-provide", "datepicker").attr("readolny", true);
            $("#BirthDate").attr("data-provide", "datepicker").attr("readolny", true);
            if ($("#hfBirthPlace").val() || '' != '') {
                $('#BirthPlace option').each(function (index, option) {
                    if (option.text == $("#hfBirthPlace").val()) {
                        $(this).attr("selected", "selected");
                    }
                    // option will contain your item
                });
                //$("#BirthPlace option[text='" + $("#hfBirthPlace").val() + "']").attr("selected", "selected");
            }
        },
        error: function (x, e) {
            $(".animationload").hide();
        }

    });

}

function SaveData(action) {
    if ($("#needs-validation").valid()) {
        var BirthDate = $('#BirthDate').val() || ''
        if (BirthDate != '') {
            BirthDate = DateFormat(BirthDate);
        }

        var ValidUntil = $('#ValidUntil').val() || ''
        if (ValidUntil != '') {
            ValidUntil = DateFormat(ValidUntil);
        }

        if (validateCustomer()) {
            var AccountId = $("#AccountId").val() || '';
            var PostURL = '/Registration/CustomerUpdate';
            var ImagePath = $("#ResidentidImagePath").text().trim();
            if (AccountId == '') {
                AccountId = 0;
                PostURL = '/Registration/CustomerAdd'
            }
            else {
                if ($("#ResidentidImagePath").text().trim() == '') {
                    ImagePath = $("#hfCustomerDocumentPath").val();
                    PostURL = '/Registration/CustomerUpdate?ImageChnage=' + false;
                }
                else {
                    ImagePath = $("#ResidentidImagePath").text().trim();
                    PostURL = '/Registration/CustomerUpdate?ImageChnage=' + true;
                }
            }
            var Inputdata = {
                AccountId: AccountId,
                ResidentId: $("#ResidentId").val(),
                FirstName: $("#FirstName").val(),
                BirthPlace: $('#BirthPlace option:selected').text(),
                BirthDate: BirthDate,
                Nationality: $('#Nationality').val(),
                Gender: $('#Gender').val(),
                MaritalStatus: $('#MaritalStatus').val(),
                Occupation: $('#Occupation').val(),
                ValidUntil: ValidUntil,
                MobileNo: $("#MobileNo").val(),
                EmailId: $("#EmailId").val(),
                ProvinceId: $("#ProvinceId").val(),
                CityId: $("#CityId").val(),
                DistrictId: $("#DistrictId").val(),
                SubDistrictId: $("#SubDistrictId").val(),
                RT: $("#RT").val(),
                RW: $("#RW").val(),
                Address: $("#Address").val(),
                PostalCode: $("#PostalCode").val(),
                ResidentidcardImagePath: ImagePath
            }

            $(".animationload").show();
            $.ajax({
                type: "POST",
                url: PostURL,
                dataType: "JSON",
                async: true,
                data: JSON.stringify(Inputdata),
                contentType: "application/json; charset=utf-8",
                success: function (resultData) {
                    $(".animationload").hide();
                    var meassage = '';
                    for (var i = 0; i < resultData.length; i++) {
                        if (resultData[i].ErrorMessage == "success") {
                            if (action == 'close')
                                closePopup();

                            else {
                                $("#warning").hide();
                                $('#needs-validation')[0].reset();
                            }
                            break;
                        }
                        else if (resultData[i].ErrorMessage == 'logout') {
                            location.href = "../Login/Logout";
                            break;
                        }
                        else {
                            meassage = meassage + "<li>" + resultData[i].ErrorMessage + "</li>"
                        }
                    }
                    if (meassage != '') {
                        $("#warning").html("<ul>" + meassage + "</ul>");
                        $("#warning").show();
                    }
                },
                error: function (ex) {
                    $(".animationload").hide();
                }

            });

        }

    }
}

function DateFormat(newDate) {
    var d = new Date(newDate);
    dd = d.getDate();
    dd = dd > 9 ? dd : '0' + dd;
    mm = (d.getMonth() + 1);
    mm = mm > 9 ? mm : '0' + mm;
    yy = d.getFullYear();
    return dd + '-' + mm + '-' + +yy
}