$(document).ready(function () {
    $('.submitFormBtn').click(function () {
        var request = {};
        var student = {};
        request.title = $('.inputTitle').val();
        request.supportMessge = $('.inputDescription').val();
        student.studentID = $('.studentId').val();
        request.student = student;
        request.phoneNumber = $('.phoneNumber').val();
        console.log(request);
        AjaxCall('/Support/AddRequest', JSON.stringify(request), "POST").done(function (response) {
            if (response == true) {
                $('.inputTitle').val("");
                $('.inputDescription').val("");
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Send Request Successfully!</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">Something wrong! Please try again later</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
});

$(document).ready(function () {
    $('.submitFormBtn').attr('disabled', 'disabled');
    $('.submitFormBtn').css('cursor', 'not-allowed');
    $('.submitFormBtn').css('opacity', '0.5');
    $('#inputTitle').blur(function () {
        if ($('#inputTitle').val().replace(/\s/g, "").length <= 0) {
            $('#errorinputTitle').html('This field is required');
        } else if ($('#inputTitle').val().length > 500) {
            $('#errorinputTitle').html('Input less than 500 characters');
        } else {
            $('#errorinputTitle').html('');
        }
    })

    $('#desSupportForm').blur(function () {
        if ($('#desSupportForm').val().replace(/\s/g, "").length <= 0) {
            $('#errordesSupportForm').html('This field is required');
        } else if ($('#desSupportForm').val().length > 2000) {
            $('#errordesSupportForm').html('Input less than 2000 characters');
        } else {
            $('#errordesSupportForm').html('');
        }
    })

    $('body').on('blur keyup', '#inputTitle, #desSupportForm', function () {
        if ($('#inputTitle').val().replace(/\s/g, "").length <= 0 || $('#inputTitle').val().length > 500
            || $('#desSupportForm').val().replace(/\s/g, "").length <= 0 || ($('#desSupportForm').val().length > 2000)) {
            $('#submitFormSupportBtn').attr('disabled', 'disabled');
            $('#submitFormSupportBtn').css('cursor', 'not-allowed');
            $('#submitFormSupportBtn').css('opacity', '0.5');
        } else {
            $('#submitFormSupportBtn').removeAttr('disabled', 'disabled');
            $('#submitFormSupportBtn').css('cursor', 'pointer');
            $('#submitFormSupportBtn').css('opacity', '1');
        }
    });
})


// Ajax 
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}