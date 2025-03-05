$(document).ready(function () {
    $('#signInEmail').attr('disabled', 'disabled');
    $('#signInEmail').css('cursor', 'not-allowed');
    $('#signInEmail').css('opacity', '0.5');
    $('.showHideNewPW').click(function () {
        if ($("#newpassword").attr('type') === 'password') {
            $("#newpassword").attr('type', 'text');
            $('.showHideNewPW').removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            $("#newpassword").attr('type', 'password');
            $('.showHideNewPW').removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });
    $('.showHideRenewPW').click(function () {
        if ($("#repassword").attr('type') === 'password') {
            $("#repassword").attr('type', 'text');
            $('.showHideRenewPW').removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            $("#repassword").attr('type', 'password');
            $('.showHideRenewPW').removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });


    $('#confirmpassalternativeMailInput').blur(function () {
        var inputPassword = $('#passalternativeMailInput').val()
        var inputConfirmPassword = $('#confirmpassalternativeMailInput').val();
        if (inputConfirmPassword.length != 0) {
            if (inputConfirmPassword === inputPassword) {
                $('#errorconfirmpassalternativeMailInput').html('');
            } else {
                $('#errorconfirmpassalternativeMailInput').html('The confirm password not match with password');
            }
        } else {
            $('#errorconfirmpassalternativeMailInput').html('This field is required');
        }
    })

    $('#passalternativeMailInput,#confirmpassalternativeMailInput').keyup(function () {
        var inputPassword = $('#passalternativeMailInput').val()
        var inputConfirmPassword = $('#confirmpassalternativeMailInput').val();
        if (inputPassword == 0 || inputConfirmPassword == 0 || !regextPass.test(inputPassword) || inputConfirmPassword !== inputPassword) {
            $('#submitBtn__setPassword').attr('disabled', true);
            $('#submitBtn__setPassword').css('cursor', 'not-allowed');
            $('#submitBtn__setPassword').css('opacity', '0.5');
        } else {
            $('#submitBtn__setPassword').removeAttr('disabled');
            $('#submitBtn__setPassword').css('cursor', 'pointer');
            $('#submitBtn__setPassword').css('opacity', '1');
        }
    })
    //validate resetPass
    var regextPass = /^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^\w\s])\S{8,32}$/;
    $('#newpassword').keypress(function (e) {
        var key = e.keyCode;
        if (key === 32) {
            e.preventDefault();
        }
    });
    $('#repassword').keypress(function (e) {
        var key = e.keyCode;
        if (key === 32) {
            e.preventDefault();
        }
    });

    $('#newpassword').blur(function () {
        var inputNewPassword = $('#newpassword').val();
        if (inputNewPassword.length != 0) {
            if (regextPass.test(inputNewPassword)) {
                $('#errorInputNewPassword').html('');
            } else {
                $('#errorInputNewPassword').html('Input 8-32 characters of which at least 1 uppercase letter and 1 special character');
            }
        } else {
            $('#errorInputNewPassword').html('This field is required');
        }
    })

    $('#repassword').blur(function () {
        var inputNewPassword = $('#newpassword').val();
        var inputConfirmNewPassword = $('#repassword').val();
        if (inputConfirmNewPassword.length != 0) {
            if (inputConfirmNewPassword === inputNewPassword) {
                $('#errorInputConfirmNewPassword').html('');
            } else {
                $('#errorInputConfirmNewPassword').html('The confirm password not match with password');
            }
        } else {
            $('#errorInputConfirmNewPassword').html('This field is required');
        }
    });

    $('#newpassword, #repassword').keyup(function () {
        var inputNewPassword = $('#newpassword').val();
        var inputConfirmNewPassword = $('#repassword').val();
        if (inputNewPassword.length == 0 || !regextPass.test(inputNewPassword) || inputConfirmNewPassword.length == 0 ||inputConfirmNewPassword !== inputNewPassword) {
            $('#signInEmail').attr('disabled', 'disabled');
            $('#signInEmail').css('cursor', 'not-allowed');
            $('#signInEmail').css('opacity', '0.5');
        } else {
            $('#signInEmail').removeAttr('disabled', 'disabled');
            $('#signInEmail').css('cursor', 'pointer');
            $('#signInEmail').css('opacity', '1');
        }
    });
});


