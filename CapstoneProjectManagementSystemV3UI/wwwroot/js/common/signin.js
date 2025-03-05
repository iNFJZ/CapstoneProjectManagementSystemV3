$(document).ready(function () {
    //$('#signInEmail').attr('disabled', 'disabled');
    //$('#signInEmail').css('cursor', 'not-allowed');
    //$('#signInEmail').css('opacity', '0.5');

    $('.showHidePW').click(function () {
        if ($(".password").attr('type') === 'password') {
            $(".password").attr('type', 'text');
            $('.showHidePW').removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            $(".password").attr('type', 'password');
            $('.showHidePW').removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });


    //var regexEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
    //var regexPass = /^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^\w\s])\S{8,32}$/;

    //$('#inputEmail').blur(function () {
    //    var inputEmail = $('#inputEmail').val();
    //    if (inputEmail.length != 0) {
    //        if (inputEmail.length < 100) {
    //            if (!regexEmail.test(inputEmail)) {
    //                $('#errorInputEmail').html('Email is invalid');
    //            } else {
    //                $('#errorInputEmail').html('');
    //            }
    //        } else {
    //            $('#errorInputEmail').html('Input less than 100 characters');
    //        }
    //    } else {
    //        $('#errorInputEmail').html('This fied is required');
    //    }
    //});
    //$('#inputPassword').blur(function () {
    //    var inputPassword = $('#inputPassword').val();
    //    if (inputPassword.length != 0) {
    //        if (regexPass.test(inputPassword)) {
    //            $('#errorInputPassword').html('');
    //        } else {
    //            $('#errorInputPassword').html('Input 8-32 characters of which at least 1 uppercase letter and 1 special character');
    //        }
    //    } else {
    //        $('#errorInputPassword').html('This field is required');
    //    }
    //})

    $('#inputPassword').keypress(function (e) {
        var key = e.keyCode;
        if (key === 32) {
            e.preventDefault();
        }
    });

    //$('#inputEmail,#inputPassword').keyup(function () {
    //    var inputEmail = $('#inputEmail').val();
    //    var inputPassword = $('#inputPassword').val();
    //    if (inputEmail.length != 0 && inputPassword.length != 0 && regexEmail.test(inputEmail) && regexPass.test(inputPassword) && inputEmail.length < 100) {
    //        $('#signInEmail').removeAttr('disabled', 'disabled');
    //        $('#signInEmail').css('cursor', 'pointer');
    //        $('#signInEmail').css('opacity', '1');
    //    } else {
    //        $('#signInEmail').attr('disabled', 'disabled');
    //        $('#signInEmail').css('cursor', 'not-allowed');
    //        $('#signInEmail').css('opacity', '0.5');
    //    }
    //});
});

