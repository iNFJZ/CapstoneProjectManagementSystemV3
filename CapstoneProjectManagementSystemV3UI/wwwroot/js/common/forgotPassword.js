var regexEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
$(document).ready(function () {
    $('#sendEmail').attr('disabled', 'disabled');
    $('#sendEmail').css('cursor', 'not-allowed');
    $('#sendEmail').css('opacity', '0.5');

    $('#inputEmailVerify').blur(function () {
            var inputEmailVerify = $('#inputEmailVerify').val();
            if (inputEmailVerify.length != 0) {
                if (inputEmailVerify.length < 100) {
                    if (!regexEmail.test(inputEmailVerify)) {
                        $('#errorInputEmailVerify').html('Email is invalid');
                    } else {
                        $('#errorInputEmailVerify').html('');
                    }
                } else {
                    $('#errorInputEmailVerify').html('Input less than 100 characters');
                }
            } else {
                $('#errorInputEmailVerify').html('This fied is required');
            }
    })

    $('#inputEmailVerify').keyup(function () {
        var inputEmailVerify = $('#inputEmailVerify').val();
        if (inputEmailVerify.length != 0 && regexEmail.test(inputEmailVerify) && inputEmailVerify.length < 100) {
            $('#sendEmail').removeAttr('disabled', 'disabled');
            $('#sendEmail').css('cursor', 'pointer');
            $('#sendEmail').css('opacity', '1');
        } else {
            $('#sendEmail').attr('disabled', 'disabled');
            $('#sendEmail').css('cursor', 'not-allowed');
            $('#sendEmail').css('opacity', '0.5');
        }
    });

})