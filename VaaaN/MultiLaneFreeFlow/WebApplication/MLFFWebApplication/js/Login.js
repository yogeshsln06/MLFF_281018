$(document).ready(function () {

    $("#txtemailid").keypress(function () {
        $("#lblerrormsg").html('');
    });

    $("#txtpassword").keypress(function () {
        $("#lblerrormsg").html('');
    });

    $("#buttonSignIn").click(function () {

        // Validate user entry
        if ($("#txtemailid").val().length == 0) {
            $("#lblerrormsg").html('Email should not be blank!');
            return false;
        }

        // Validate Email Id
        if (!fnIsValidEmail($('#txtemailid').val())) {
            $("#lblerrormsg").html('Invalid Email Id!');
            return false;
        }

        if ($("#txtpassword").val().length == 0) {            
            $("#lblerrormsg").html('Password should not be blank!');
            return false;
        }

        $.ajax({
            type: 'POST',
            url: '../Home/ValidateUser/',    // '@Url.Action("/Home/GetModels")', // we are calling json method
            dataType: 'json',
            data: { email: $("#txtemailid").val(), password: $("#txtpassword").val() },
            success: function (result) {
                if (result === 'valid') {
                    var url = '/Admin/Dashboard';
                    window.location.href = url;
                }
                else (result == 'Invalid')
                {
                    $("#lblerrormsg").html('Invalid email id or passowrd');

                }
                //alert(result);
            },
            error: function (ex) {
                $("#lblerrormsg").html('Invalid email id or passowrd');
            }
        });

       
        return false;
    });
});

// Validation of Email
function fnIsValidEmail(strEmail) {
    var regex = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return regex.test(strEmail);
}