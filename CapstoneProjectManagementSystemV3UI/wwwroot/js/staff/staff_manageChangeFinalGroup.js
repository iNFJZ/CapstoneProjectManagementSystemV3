const showMemberRequest = document.querySelector('.showMemberRequest');
const showRequestAccept = document.querySelector('.showRequestAccept');
const showRequestReject = document.querySelector('.showRequestReject');
const requestPendingBtn = document.querySelector('.requestPendingBtn');
const requestAcceptedBtn = document.querySelector('.requestAcceptedBtn');
const requestRejectedBtn = document.querySelector('.requestRejectedBtn');
requestPendingBtn.addEventListener('click', () => {
    showMemberRequest.classList.remove('hide-form');
    showRequestReject.classList.add('hide-form');
    showRequestAccept.classList.add('hide-form');
    requestPendingBtn.classList.add('active');
    requestAcceptedBtn.classList.remove('active');
    requestRejectedBtn.classList.remove('active');
})
requestAcceptedBtn.addEventListener('click', () => {
    showMemberRequest.classList.add('hide-form');
    showRequestReject.classList.add('hide-form');
    showRequestAccept.classList.remove('hide-form');
    requestPendingBtn.classList.remove('active');
    requestAcceptedBtn.classList.add('active');
    requestRejectedBtn.classList.remove('active');
})
requestRejectedBtn.addEventListener('click', () => {
    showMemberRequest.classList.add('hide-form');
    showRequestReject.classList.remove('hide-form');
    showRequestAccept.classList.add('hide-form');
    requestPendingBtn.classList.remove('active');
    requestAcceptedBtn.classList.remove('active');
    requestRejectedBtn.classList.add('active');
})


var startNum;
var numberOfRecordsPerPage;
var countResult;
var endNum;

$(document).ready(function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListChangeFinalGroupRequests(0, searchText ,'none' ,0);
    }
})
$('#requestPendingBtn,#requestAcceptedBtn,#requestRejectedBtn,#searchNameRequestInput').on('click change', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListChangeFinalGroupRequests(0, searchText, 'none', 0);
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        GetListChangeFinalGroupRequests(1, searchText, 'none', 0);
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        GetListChangeFinalGroupRequests(2, searchText, 'none', 0);
    }
});


/*previous page*/
$('body').on('click', '#previousBtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeFinalGroupRequests(0, searchText, 'previous', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeFinalGroupRequests(1, searchText, 'previous', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeFinalGroupRequests(2, searchText, 'previous', startNum);
        }
    }
});

/*next page*/
$('body').on('click', '#nextbtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeFinalGroupRequests(0, searchText, 'next', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeFinalGroupRequests(1, searchText, 'next', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeFinalGroupRequests(2, searchText, 'next', startNum);
        }
    }
});


function GetListChangeFinalGroupRequests(status, searchText, pagingType, recordNumber) {
    AjaxCall('/ManageChangeFinalGroup/GetListChangeFinalGroupRequestBySearchText?status=' + status + '&&searchText=' + searchText + '&&pagingType=' + pagingType + '&&recordNumber=' + recordNumber, "POST").done(function (response) {
        if (response.listChangeFinalGroupRequest != null) {
            if (status == 0) {
                var count = 1;
                $('.showMemberRequest').html('');
                $('.showMemberRequest').append('  <div class="title">'
                    + '<p> No.</p>'
                    + '<p>From Group</p>'
                    + ' <p>From Email</p>'
                    + '<p>To Group</p>'
                    + ' <p>To Email</p>'
                    + ' <p>Action</p>'
                    + ' </div>')
                for (var i = 0; i < response.listChangeFinalGroupRequest.length; i++) {
                    $('.showMemberRequest').append('<div class="informMemRequest">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.user.fptEmail + '</p>'
                        + '<div class="buttonMemRequest">'
                        + '<button class="acceptBtn" value="' + response.listChangeFinalGroupRequest[i].changeFinalGroupRequestId + '">Accept</button>'
                        + '<button class="rejectBtn" value="' + response.listChangeFinalGroupRequest[i].changeFinalGroupRequestId + '">Reject</button>'
                        + '</div>'
                        + '</div>')
                    count++;
                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showMemberRequest').append('<div class="pagination">'
                    + '<a id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
            if (status == 1) {
                var count = 1;
                $('.showRequestAccept').html('');
                $('.showRequestAccept').append('<div class="title">'
                    + '<p> No.</p>'
                    + '<p>From Group</p>'
                    + ' <p>From Email</p>'
                    + '<p>To Group</p>'
                    + ' <p>To Email</p>'
                    + ' <p>Status</p>'
                    + ' </div>')
                for (var i = 0; i < response.listChangeFinalGroupRequest.length; i++) {
                    $('.showRequestAccept').append('<div class="informMemRequest">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.user.fptEmail + '</p>'
                        + '<div class="acceptinformMemRequest">'
                        + '<p class="acceptStatus">Accepted</p>'
                        + '</div>'
                        + '</div>')
                    count++;

                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showRequestAccept').append('<div class="pagination">'
                    + '<a id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
            if (status == 2) {
                var count = 1;
                $('.showRequestReject').html('');
                $('.showRequestReject').append('  <div class="titleRequestReject">'
                    + '<p> No.</p>'
                    + '<p>From Group</p>'
                    + '<p>From Email</p>'
                    + '<p>To Group</p>'
                    + '<p>To Email</p>'
                    + '<p>Status</p>'
                    + '<p>Staff Comment</p>'
                    + '</div>')
                for (var i = 0; i < response.listChangeFinalGroupRequest.length; i++) {
                    $('.showRequestReject').append('<div class="informRequestReject">'
                        + '<p>' + count + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].fromStudent.user.fptEmail + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.finalGroup.groupName + '</p>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].toStudent.user.fptEmail + '</p>'
                        + '<div class="acceptinformMemRequest">'
                        + '<p class="rejectStatus">Rejected</p>'
                        + '</div>'
                        + '<p>' + response.listChangeFinalGroupRequest[i].staffComment + '</p>'
                        + '</div>')
                    count++;
                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showRequestReject').append('<div class="pagination">'
                    + '<a id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
        } else {
            if (status == 0) {
                $('.showMemberRequest').html('');
                $('.showMemberRequest').append('<h2 style="color:red">There are no request yet.</h2>');
            }
            if (status == 1) {
                $('.showRequestAccept').html('');
                $('.showRequestAccept').append('<h2 style="color:red">Nothing to display.</h2>');
            }
            if (status == 2) {
                $('.showRequestReject').html('');
                $('.showRequestReject').append('<h2 style="color:red">Nothing to display.</h2>');
            }
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
        });
    });
}

//acceept request confirm
$(document).ready(function () {
    var changeFinalGroupRequestId;
    var acceptBtn;
    var rejectBtn;
    $('body').on('click', '.acceptBtn', function (e) {
        $('.showAcceptRequestConfirm').toggle('hide-form');
        changeFinalGroupRequestId = $(this).val();
        acceptBtn = $('.acceptBtn');
        acceptBtn.attr('disabled', 'disabled');
        acceptBtn.css('cursor', 'not-allowed');
        acceptBtn.css('opacity', '1.5');

        rejectBtn = $('.rejectBtn');
        rejectBtn.attr('disabled', 'disabled');
        rejectBtn.css('cursor', 'not-allowed');
        rejectBtn.css('opacity', '1.5');
    });
    $('body').on('click', '.acceptRequestConfirmBtn', function (e) {
        $('.showAcceptRequestConfirm').toggle('hide-form');
        AjaxCall('/ManageChangeFinalGroup/AcceptChangeFinalGroupRequest?changeFinalGroupRequestID=' + changeFinalGroupRequestId, 'POST')
            .done(function (response) {
                if (response == 1) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Accepted Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            if ($('#requestPendingBtn').hasClass('active')) {
                                GetListChangeFinalGroupRequests(0, searchText);
                            }
                        })
                        acceptBtn.removeAttr('disabled', 'disabled');
                        acceptBtn.css('cursor', 'pointer');
                        acceptBtn.css('opacity', '1');

                        rejectBtn.removeAttr('disabled', 'disabled');
                        rejectBtn.css('cursor', 'pointer');
                        rejectBtn.css('opacity', '1');

                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });
            });
    });
    $('body').on('click', '.cancelAcceptRequestConfirmBtn', function () {
        acceptBtn.removeAttr('disabled', 'disabled');
        acceptBtn.css('cursor', 'pointer');
        acceptBtn.css('opacity', '1');

        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');
        $('.showAcceptRequestConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showAcceptRequestConfirm', function (e) {
        if (e.target === e.currentTarget) {
            acceptBtn.removeAttr('disabled', 'disabled');
            acceptBtn.css('cursor', 'pointer');
            acceptBtn.css('opacity', '1');

            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');
            $('.showAcceptRequestConfirm').toggle('hide-form');
        }
    });
})

//reject comment request
$(document).ready(function () {
    var requestChangeFinalGroupId;
    var rejectBtn;
    var acceptBtn
    $('body').on('click', '.rejectBtn', function (e) {
        $('.commentReject').toggle('hide-form');
        requestChangeFinalGroupId = $(this).val();
        rejectBtn = $('.rejectBtn');
        rejectBtn.attr('disabled', 'disabled');
        rejectBtn.css('cursor', 'not-allowed');
        rejectBtn.css('opacity', '0.5');

        acceptBtn = $('.acceptBtn');
        acceptBtn.attr('disabled', 'disabled');
        acceptBtn.css('cursor', 'not-allowed');
        acceptBtn.css('opacity', '0.5');

    });
    $('body').on('click', '.rejectRequestConfirmBtn', function (e) {
        $('.commentReject').toggle('hide-form');
        var staffComment = $('#textAreaComent').val()
        AjaxCall('/ManageChangeFinalGroup/RejectChangeFinalGroupRequest?changeFinalGroupRequestID=' + requestChangeFinalGroupId + '&&staffComment=' + staffComment, 'POST').done(function (response) {
            if (response == 1) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Rejected Successfully</p>'
                }).then(function () {
                    $(document).ready(function () {
                        var searchText = $('#searchNameRequestInput').val();
                        if ($('#requestPendingBtn').hasClass('active')) {
                            GetListChangeFinalGroupRequests(0, searchText);
                        }
                    })
                    rejectBtn.removeAttr('disabled', 'disabled');
                    rejectBtn.css('cursor', 'pointer');
                    rejectBtn.css('opacity', '1');

                    acceptBtn.removeAttr('disabled', 'disabled');
                    acceptBtn.css('cursor', 'pointer');
                    acceptBtn.css('opacity', '1');
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
            }).then(function () {
                location.reload(true);
            });;
        });
    });
    $('body').on('click', '.cancelFormRejectRequestBtn', function () {
        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');

        acceptBtn.removeAttr('disabled', 'disabled');
        acceptBtn.css('cursor', 'pointer');
        acceptBtn.css('opacity', '1');
        $('.commentReject').toggle('hide-form');
    });
    $('body').on('click', '.commentReject', function (e) {
        if (e.target === e.currentTarget) {
            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');

            acceptBtn.removeAttr('disabled', 'disabled');
            acceptBtn.css('cursor', 'pointer');
            acceptBtn.css('opacity', '1');
            $('.commentReject').toggle('hide-form');
        }
    });
})


$(document).ready(function () {
    //validate commentReject
    $('body').on('blur', '#textAreaComent', function () {
        const commentReject = $('#textAreaComent').val();
        if (commentReject.length == 0) {
            $('#showErrorMessageCommentReject').html('This field is required')
        } else if (commentReject.length > 500) {
            $('#showErrorMessageCommentReject').html('Input less than 500 characters');
        } else {
            $('#showErrorMessageCommentReject').html('');
        }
    })
    // disable button submit
    $('body').on('blur change keyup', '#textAreaComent', function () {
        const commentReject = $('#textAreaComent').val();
        if (commentReject.length == 0) {
            $('.rejectRequestConfirmBtn').attr('disabled', 'disabled');
            $('.rejectRequestConfirmBtn').css('cursor', 'not-allowed');
            $('.rejectRequestConfirmBtn').css('opacity', '0.5');
        } else if (commentReject.length > 500) {
            $('.rejectRequestConfirmBtn').attr('disabled', 'disabled');
            $('.rejectRequestConfirmBtn').css('cursor', 'not-allowed');
            $('.rejectRequestConfirmBtn').css('opacity', '0.5');
        } else {
            $('.rejectRequestConfirmBtn').removeAttr('disabled', 'disabled');
            $('.rejectRequestConfirmBtn').css('cursor', 'pointer');
            $('.rejectRequestConfirmBtn').css('opacity', '1');
        }
    })
})
// ajax call
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}



