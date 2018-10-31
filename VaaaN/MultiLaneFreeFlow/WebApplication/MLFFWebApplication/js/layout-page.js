$(document).ready(function () {
    $("#Admin").attr("aria-expanded", "true");
    $("#AdminsubMenu").removeClass("list-group-item list-group-item-info collapsed");
    $("#Admin").addClass("list-group-item list-group-item-info");
    $("#AdminsubMenu").attr("aria-expanded", "true");
    $("#AdminsubMenu").removeClass("list-group-submenu");
    $("#AdminsubMenu").addClass("list-group-submenu collapse in")
    $("#buttonChange").focus();
    $("#buttonChange").click(function () {
        // validation
        var old_pass = $("#txtoldpassword").val();
        var new_pass = $("#txtnewPassword").val();

        $("#errormsg").text('');
        if (old_pass == '') {
            $("#errormsg").text('Old Password required !');
            return false;
        }
        if (new_pass == '') {
            $("#errormsg").text('New Password required !');
            return false;
        }


        $.ajax({
            type: 'POST',
            url: "@Url.Action('ChangePassword', 'Home')",
            dataType: 'json',
            data: {
                old_pass: old_pass, new_pass: new_pass
            },
            success: function (data) {
                if (data == "Not Allowed") {
                    alert("Super Admin password cant be changed");
                }
                else if (data == "changed") {
                    alert("Your password has been changed successfully.");
                } else if (data == "not changed") {
                    alert("Password not changed. Please try again later")
                }
                else {
                    alert('Failed to sent mail. Please try again later');
                }
            },
            error: function (ex) {
                // $("#errormsg").text('Trouble in login. Exception');
            }
        });
    });
    console.log(window.location.pathname);

    $('#MainMenu [href^="' + window.location.pathname.toLowerCase() + '"]').addClass('active');
});

