const groupExchangeFormBtn = document.querySelector('.groupExchangeFormBtn');
const studentConfirmBtn = document.querySelector('.studentConfirmBtn');
const staffConfirmBtn = document.querySelector('.staffConfirmBtn');
const formRequest = document.querySelector('.formRequest');
const showStaffConfirmRequest = document.querySelector('.showStaffConfirmRequest');
const showStudentConfirmRequest = document.querySelector('.showStudentConfirmRequest');
const myRequest = document.querySelector('.myRequest');
const anotherRequest = document.querySelector('.anotherRequest');
const myRequestConfirm = document.querySelector('.myRequestConfirm');
const anotherRequestConfirm = document.querySelector('.anotherRequestConfirm');

groupExchangeFormBtn.addEventListener('click', () => {
    groupExchangeFormBtn.classList.add('active');
    studentConfirmBtn.classList.remove('active');
    staffConfirmBtn.classList.remove('active');
    formRequest.classList.remove('hide-form');
    showStudentConfirmRequest.classList.add('hide-form');
    showStaffConfirmRequest.classList.add('hide-form');
})

studentConfirmBtn.addEventListener('click', () => {
    groupExchangeFormBtn.classList.remove('active');
    studentConfirmBtn.classList.add('active');
    staffConfirmBtn.classList.remove('active');
    formRequest.classList.add('hide-form');
    showStudentConfirmRequest.classList.remove('hide-form');
    showStaffConfirmRequest.classList.add('hide-form');
})

staffConfirmBtn.addEventListener('click', () => {
    groupExchangeFormBtn.classList.remove('active');
    studentConfirmBtn.classList.remove('active');
    staffConfirmBtn.classList.add('active');
    formRequest.classList.add('hide-form');
    showStudentConfirmRequest.classList.add('hide-form');
    showStaffConfirmRequest.classList.remove('hide-form');
})

myRequest.addEventListener('click', () => {
    myRequest.classList.add('activeLink');
    anotherRequest.classList.remove('activeLink');
    myRequestConfirm.classList.remove('hide-form');
    anotherRequestConfirm.classList.add('hide-form');
})

anotherRequest.addEventListener('click', () => {
    myRequest.classList.remove('activeLink');
    anotherRequest.classList.add('activeLink');
    myRequestConfirm.classList.add('hide-form');
    anotherRequestConfirm.classList.remove('hide-form');
})


// create change finalgroup request
$(document).ready(function () {
    var submitBtnFormExchange
    $('body').on('click', '.acceptBtn', function (e) {
        $('.showFormConfirm').toggle('hide-form');
        var toGroupName = $('#toGroupName').val();
        var toEmail = $('#toEmail').val();
        AjaxCall('/ChangeFinalGroup/SubmitFormGroupExchange?toGroupName=' + toGroupName + '&&toEmail=' + toEmail, 'POST').done(function (response) {
            if (response == 1) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Accepted Successfully</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            });
        });
    });
    $('body').on('click', '.submitBtn', function (e) {
        submitBtnFormExchange = $(this);
        submitBtnFormExchange.attr('disabled', 'disabled');
        submitBtnFormExchange.css('cursor', 'not-allowed');
        submitBtnFormExchange.css('opacity', '0.5');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('body').on('click', '.cancelBtn', function () {
        submitBtnFormExchange.removeAttr('disabled', 'disabled');
        submitBtnFormExchange.css('cursor', 'pointer');
        submitBtnFormExchange.css('opacity', '1');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showFormConfirm', function (e) {
        if (e.target === e.currentTarget) {
            submitBtnFormExchange.removeAttr('disabled', 'disabled');
            submitBtnFormExchange.css('cursor', 'pointer');
            submitBtnFormExchange.css('opacity', '1');
            $('.showFormConfirm').toggle('hide-form');
        }
    });
})

// student confirm btn
$('.studentConfirmBtn').click(function () {
    if ($('.myRequest').hasClass('activeLink')) {
        GetListChangeFinalGroupRequestFromOfStudent();
    }
    if ($('.anotherRequest').hasClass('activeLink')) {
        GetListChangeFinalGroupRequestToOfStudent();
    }
});
$('.myRequest').click(function () {
    GetListChangeFinalGroupRequestFromOfStudent();
})
$('.anotherRequest').click(function () {
    GetListChangeFinalGroupRequestToOfStudent();
})

// staff confirm btn
$('.staffConfirmBtn').click(function () {
    GetListChangeFinalGroupRequest();
})


// accepted request exchange group
$(document).ready(function () {
    var changeFinalGroupRequestId;
    var acceptAnotherRequest;
    var rejectAnotherRequest;
    $('body').on('click', '.acceptRequestBtn', function (e) {
        $('.showAcceptRequestConfirm').toggle('hide-form');
        changeFinalGroupRequestId = $(this).val();
        acceptAnotherRequest = $('.acceptRequestBtn');
        acceptAnotherRequest.attr('disabled', 'disabled');
        acceptAnotherRequest.css('cursor', 'not-allowed');
        acceptAnotherRequest.css('opacity', '0.5');

        rejectAnotherRequest = $('.rejectRequestBtn');
        rejectAnotherRequest.attr('disabled', 'disabled');
        rejectAnotherRequest.css('cursor', 'not-allowed');
        rejectAnotherRequest.css('opacity', '0.5');
    });
    $('body').on('click', '.acceptRequestConfirmBtn', function (e) {
        $('.showAcceptRequestConfirm').toggle('hide-form');
        AjaxCall('/ChangeFinalGroup/AcceptChangeFinalGroupRequest?changeFinalGroupRequestId=' + changeFinalGroupRequestId, 'POST').done(function (response) {
            if (response == 1) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Accepted Successfully</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    $(document).ready(function () {
                        GetListChangeFinalGroupRequestToOfStudent();
                    })
                    acceptAnotherRequest.removeAttr('disabled', 'disabled');
                    acceptAnotherRequest.css('cursor', 'pointer');
                    acceptAnotherRequest.css('opacity', '1');

                    rejectAnotherRequest.removeAttr('disabled', 'disabled');
                    rejectAnotherRequest.css('cursor', 'pointer');
                    rejectAnotherRequest.css('opacity', '1');
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                location.reload(true);
            });
        });
    });
    $('body').on('click', '.cancelAcceptRequestConfirmBtn', function () {
        acceptAnotherRequest.removeAttr('disabled', 'disabled');
        acceptAnotherRequest.css('cursor', 'pointer');
        acceptAnotherRequest.css('opacity', '1');

        rejectAnotherRequest.removeAttr('disabled', 'disabled');
        rejectAnotherRequest.css('cursor', 'pointer');
        rejectAnotherRequest.css('opacity', '1');
        $('.showAcceptRequestConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showAcceptRequestConfirm', function (e) {
        if (e.target === e.currentTarget) {
            acceptAnotherRequest.removeAttr('disabled', 'disabled');
            acceptAnotherRequest.css('cursor', 'pointer');
            acceptAnotherRequest.css('opacity', '1');

            rejectAnotherRequest.removeAttr('disabled', 'disabled');
            rejectAnotherRequest.css('cursor', 'pointer');
            rejectAnotherRequest.css('opacity', '1');
            $('.showAcceptRequestConfirm').toggle('hide-form');
        }
    });
})

// rejected request exchange group
$(document).ready(function () {
    var changeFinalGroupRequestId;
    var rejectAnotherRequest;
    $('body').on('click', '.rejectRequestBtn', function (e) {
        $('.showRejectRequestConfirm').toggle('hide-form');
        changeFinalGroupRequestId = $(this).val();
        acceptAnotherRequest = $('.acceptRequestBtn');
        acceptAnotherRequest.attr('disabled', 'disabled');
        acceptAnotherRequest.css('cursor', 'not-allowed');
        acceptAnotherRequest.css('opacity', '0.5');

        rejectAnotherRequest = $('.rejectRequestBtn');
        rejectAnotherRequest.attr('disabled', 'disabled');
        rejectAnotherRequest.css('cursor', 'not-allowed');
        rejectAnotherRequest.css('opacity', '0.5');
    });

    $('body').on('click', '.rejectRequestConfirmBtn', function (e) {
        $('.showRejectRequestConfirm').toggle('hide-form');
        AjaxCall('/ChangeFinalGroup/RejectChangeFinalGroupRequest?changeFinalGroupRequestId=' + changeFinalGroupRequestId, 'POST').done(function (response) {
            if (response == 1) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Rejected Successfully</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    $(document).ready(function () {
                        GetListChangeFinalGroupRequestToOfStudent();
                    })
                    rejectAnotherRequest.removeAttr('disabled', 'disabled');
                    rejectAnotherRequest.css('cursor', 'pointer');
                    rejectAnotherRequest.css('opacity', '1');

                    acceptAnotherRequest.removeAttr('disabled', 'disabled');
                    acceptAnotherRequest.css('cursor', 'pointer');
                    acceptAnotherRequest.css('opacity', '1');
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                location.reload(true);
            });
        });
    });
    $('body').on('click', '.cancelRejectRequestConfirmBtn', function () {
        rejectAnotherRequest.removeAttr('disabled', 'disabled');
        rejectAnotherRequest.css('cursor', 'pointer');
        rejectAnotherRequest.css('opacity', '1');

        acceptAnotherRequest.removeAttr('disabled', 'disabled');
        acceptAnotherRequest.css('cursor', 'pointer');
        acceptAnotherRequest.css('opacity', '1');
        $('.showRejectRequestConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showRejectRequestConfirm', function (e) {
        if (e.target === e.currentTarget) {
            rejectAnotherRequest.removeAttr('disabled', 'disabled');
            rejectAnotherRequest.css('cursor', 'pointer');
            rejectAnotherRequest.css('opacity', '1');

            acceptAnotherRequest.removeAttr('disabled', 'disabled');
            acceptAnotherRequest.css('cursor', 'pointer');
            acceptAnotherRequest.css('opacity', '1');
            $('.showRejectRequestConfirm').toggle('hide-form');
        }
    });
})



function GetListChangeFinalGroupRequestFromOfStudent() {
    $('div').remove('.titleMyRequestConfirm');
    $('div').remove('.informMyRequestConfirm');
    AjaxCall('/ChangeFinalGroup/GetListChangeFinalGroupRequestFromOfStudent', 'POST').done(function (response) {
        if (response != null) {
            $('.myRequestConfirm').append(' <div class="titleMyRequestConfirm">'
                + '<p> No.</p>'
                + '<p>From Group</p>'
                + '<p>From Email</p>'
                + '<p>To Group</p>'
                + '<p>To Email</p>'
                + '<p>Status Of Student</p>'
                + '</div>')
            var count = 1;
            for (var i = 0; i < response.length; i++) {
                if (response[i].statusOfToStudent == 0) {
                    $('.myRequestConfirm').append('<div class="informMyRequestConfirm">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + '<p class="pendingStatus">Pending</p>'
                        + '</div> ');
                }
                if (response[i].statusOfToStudent == 1) {
                    $('.myRequestConfirm').append('<div class="informMyRequestConfirm">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + '<p class="acceptedStatus">Accepted</p>'
                        + '</div> ');
                }
                if (response[i].statusOfToStudent == 2) {
                    $('.myRequestConfirm').append('<div class="informMyRequestConfirm">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + '<p class="rejectedStatus">Rejected</p>'
                        + '</div> ');
                }
                count++;
            }
        } else {
            $('.myRequestConfirm').html('');
            $('.myRequestConfirm').append("<p class='notifiChangeGroupRequest'>You don't have any request change FinalGroup</p>");
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
            timer: 1000,
            showConfirmButton: false
        });
    })
}

function GetListChangeFinalGroupRequestToOfStudent() {
    $('div').remove('.titleAnotherRequestConfirm');
    $('div').remove('.informAnotherRequestConfirm');
    AjaxCall('/ChangeFinalGroup/GetListChangeFinalGroupRequestToOfStudent', 'POST').done(function (response) {
        if (response != null) {
            $('.anotherRequestConfirm').append(' <div class="titleAnotherRequestConfirm">'
                + '<p> No.</p>'
                + '<p>From Group</p>'
                + '<p>From Email</p>'
                + '<p>To Group</p>'
                + '<p>To Email</p>'
                + '<p>Status Of Student</p>'
                + '</div>')
            var count = 1;
            for (var i = 0; i < response.length; i++) {
                if (response[i].statusOfToStudent == 0) {
                    $('.anotherRequestConfirm').append('<div class="informAnotherRequestConfirm">'
                        + '<p>' + count + '</p >'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + '<div class="anotherRequestBtn">'
                        + '<button class="acceptRequestBtn" value="' + response[i].changeFinalGroupRequestId + '">Accept</button>'
                        + '<button class="rejectRequestBtn" value="' + response[i].changeFinalGroupRequestId + '">Reject</button>'
                        + '</div>'
                        + '</div>');
                }
                if (response[i].statusOfToStudent == 1) {
                    $('.anotherRequestConfirm').append('<div class="informAnotherRequestConfirm">'
                        + '<p>' + count + '</p >'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + '<p class="acceptedStatus">Accepted</p>'
                        + '</div>');
                }
                if (response[i].statusOfToStudent == 2) {
                    $('.anotherRequestConfirm').append('<div class="informAnotherRequestConfirm">'
                        + '<p>' + count + '</p >'
                        + '<p>' + response[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response[i].toStudent.user.fptEmail + '</p>'
                        + ' <p class="rejectedStatus">Rejected</p>'
                        + '</div>');
                }
                count++;
            }
        } else {
            $('.anotherRequestConfirm').html('');
            $('.anotherRequestConfirm').append("<p class='notifiChangeGroupRequest'>You don't have any request change FinalGroup</p>");
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
            timer: 1000,
            showConfirmButton: false
        });
    })
}

function GetListChangeFinalGroupRequest() {
    $('div').remove('.title');
    $('div').remove('.informStaffConfirmRequest');
    AjaxCall('/ChangeFinalGroup/GetListChangeFinalGroupRequest', 'POST').done(function (response) {
        if (response != null) {
            $('.showStaffConfirmRequest').append("<div class='title'>"
                + "<p> No.</p >"
                + "<p>From Group</p>"
                + "<p>From Email</p>"
                + "<p>To Group</p>"
                + "<p>To Email</p>"
                + "<p>Staff's Comment</p>"
                + "<p>Status Staff</p>"
                + "</div>")
            var count = 1;
            for (var i = 0; i < response.length; i++) {
                if (response[i].statusOfStaff == 0) {
                    $('.showStaffConfirmRequest').append("<div class='informStaffConfirmRequest'>"
                        + "<p>" + count + "</p>"
                        + "<p>" + response[i].fromStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].fromStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].toStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].toStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].staffComment + "</p>"
                        + "<p class='pendingStatus'>Pending</p>"
                        + "</div>");
                }
                if (response[i].statusOfStaff == 1) {
                    $('.showStaffConfirmRequest').append("<div class='informStaffConfirmRequest'>"
                        + "<p>" + count + "</p>"
                        + "<p>" + response[i].fromStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].fromStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].toStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].toStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].staffComment + "</p>"
                        + "<p class='acceptedStatus'>Accepted</p>"
                        + "</div>");
                }
                if (response[i].statusOfStaff == 2) {
                    $('.showStaffConfirmRequest').append("<div class='informStaffConfirmRequest'>"
                        + "<p>" + count + "</p>"
                        + "<p>" + response[i].fromStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].fromStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].toStudent.finalGroup.groupName + "</p>"
                        + "<p>" + response[i].toStudent.user.fptEmail + "</p>"
                        + "<p>" + response[i].staffComment + "</p>"
                        + "<p class='rejectedStatus'>Rejected</p>"
                        + "</div>");
                }
                count++;
            }
        } else {
            $('.showStaffConfirmRequest').html('');
            $('.showStaffConfirmRequest').append("<p class='notifiChangeGroupRequest'> You don't have any request change FinalGroup</p>");
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
            timer: 1000,
            showConfirmButton: false
        });
    })
}

$(document).ready(function () {
    const regexEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;


    $('.submitBtn').attr('disabled', 'disabled');
    $('.submitBtn').css('cursor', 'not-allowed');
    $('.submitBtn').css('opacity', '0.5');

    $('#toGroupName').blur(function () {
        if ($('#toGroupName').val().replace(/\s/g, "").length <= 0) {
            $('#errorGroupName').html('This field is required');
        } else if ($('#toGroupName').val().length > 100) {
            $('#errorGroupName').html('Input less than 100 characters');
        } else {
            $('#errorGroupName').html('');
        }
    })

    $('#toEmail').blur(function () {
        if ($('#toEmail').val().replace(/\s/g, "").length <= 0) {
            $('#errorEmail').html('This field is required');
        } else if ($('#toEmail').val().length > 100) {
            $('#errorEmail').html('Input less than 100 characters');
        } else if (!regexEmail.test($('#toEmail').val())) {
            $('#errorEmail').html('This email invalid');
        } else {
            $('#errorEmail').html('');
        }
    })

    $('body').on('blur keyup', '#toGroupName, #toEmail', function () {
        if ($('#toGroupName').val().replace(/\s/g, "").length <= 0 || $('#toGroupName').val().length > 100
            || $('#toEmail').val().replace(/\s/g, "").length <= 0 || $('#toEmail').val().length > 100) {
            $('.submitBtn').attr('disabled', 'disabled');
            $('.submitBtn').css('cursor', 'not-allowed');
            $('.submitBtn').css('opacity', '0.5');
        } else {
            $('.submitBtn').removeAttr('disabled', 'disabled');
            $('.submitBtn').css('cursor', 'pointer');
            $('.submitBtn').css('opacity', '1');
        }
    });
})


//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}
