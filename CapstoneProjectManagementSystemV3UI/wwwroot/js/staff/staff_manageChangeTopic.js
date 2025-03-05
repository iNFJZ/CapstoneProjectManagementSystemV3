const showMemberRequest = document.querySelector('.showRequestTopic');
const showRequestAccept = document.querySelector('.showRequestAccepted');
const showRequestReject = document.querySelector('.showRequestRejected');
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
var globalStatus;
var globalSearchText;
var globalPagingType;
var globalRecordNumber;

//load list request change topic pending when redirect page here
$(document).ready(function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListChangeTopicRequests(3, searchText, 'none', 0);
    }
    var connection = new signalR.HubConnectionBuilder().withUrl("/realTimeHub").build();
    connection.start().then(function () {
    }).catch(function (err) {
        console.error(err.toString());
    });
    connection.on("receiveMessage", function (user, message) {
        if (user == userId)
        GetListChangeTopicRequests(globalStatus, globalSearchText, globalPagingType, globalRecordNumber);
    });
})


//// get list request change topic manage of staff 
$('#requestPendingBtn,#requestAcceptedBtn,#requestRejectedBtn,#searchNameRequestInput').on('click change', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListChangeTopicRequests(3, searchText, 'none', 0);
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        GetListChangeTopicRequests(4, searchText, 'none', 0);
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        GetListChangeTopicRequests(-1, searchText, 'none', 0);
    }
});

/*previous page*/
$('body').on('click', '#previousBtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeTopicRequests(3, searchText, 'previous', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeTopicRequests(4, searchText, 'previous', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListChangeTopicRequests(-1, searchText, 'previous', startNum);
        }
    }
});

/*next page*/
$('body').on('click', '#nextbtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeTopicRequests(3, searchText, 'next', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeTopicRequests(4, searchText, 'next', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListChangeTopicRequests(-1, searchText, 'next', startNum);
        }
    }
});



function GetListChangeTopicRequests(status, searchText, pagingType, recordNumber) {
    globalStatus = status;
    globalSearchText = searchText;
    globalPagingType = pagingType;
    globalRecordNumber = recordNumber;
    AjaxCall('/ManageChangeTopic/GetListRequestChangeTopic?status=' + status + '&&searchText=' + searchText + '&&pagingType=' + pagingType + '&&recordNumber=' + recordNumber, "POST").done(function (response) {
        if (response.changeTopicRequests != null) {
            if (status == 3) {
                $('.showRequestTopic').html('');
                $('.showRequestTopic').append(' <div class="title">'
                    + '<div class= "showTitleRequest">'
                    + '<p>No.</p>'
                    + '<p>Group Name</p>'
                    + '<p>Old Topic</p>'
                    + '<p>New Topic</p>'
                    + '<p>Email Supervisor</p>'
                    + '</div>'
                    + '<p>Action</p>'
                    + ' </div>');
                var count = 1;
                for (var i = 0; i < response.changeTopicRequests.length; i++) {
                    $('.showRequestTopic').append('<div class="informRequest">'
                        + '<div class= "showInformRequest" id="' + response.changeTopicRequests[i].requestID + '">'
                        + '<p>' + count + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].finalGroup.groupName + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].emailSuperVisor + '</p>'
                        + '</div>'
                        + '<div class="buttonRequest">'
                        + '<button class="acceptBtn" value="' + response.changeTopicRequests[i].requestID + '">Accept</button>'
                        + '<button class="rejectBtn" value="' + response.changeTopicRequests[i].requestID + '">Reject</button>'
                        + '</div>'
                        + '</div>');
                    count++;
                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showRequestTopic').append('<div class="pagination">'
                    + '<a id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
            if (status == 4) {
                $('.showRequestAccepted').html('');
                $('.showRequestAccepted').append(' <div class="title">'
                    + '<div class= "showTitleRequest">'
                    + '<p>No.</p>'
                    + '<p>Group Name</p>'
                    + '<p>Old Topic</p>'
                    + '<p>New Topic</p>'
                    + '<p>Email Supervisor</p>'
                    + '</div>'
                    + '<p>Status</p>'
                    + '</div>');
                var count = 1;
                for (var i = 0; i < response.changeTopicRequests.length; i++) {
                    $('.showRequestAccepted').append(' <div class="informRequest">'
                        + '<div class= "showInformRequest" id="' + response.changeTopicRequests[i].requestID + '">'
                        + '<p>' + count + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].finalGroup.groupName + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].emailSuperVisor + '</p>'
                        + '</div>'
                        + '<div class= "acceptedStatus">'
                        + '<p id="acceptedStatus">Accepted</p>'
                        + '</div>'
                        + '</div>');
                    count++;
                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showRequestAccepted').append('<div class="pagination">'
                    + '<a  id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a  id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
            if (status == -1) {
                $('.showRequestRejected').html('');
                $('.showRequestRejected').append(' <div class="titleRejected">'
                    + '<p>No.</p>'
                    + '<p>Group Name</p>'
                    + '<p>Old Topic</p>'
                    + '<p>New Topic</p>'
                    + '<p>Email Supervisor</p>'
                    + '<p>Status</p>'
                    + '<p>Reason reject</p>'
                    + '</div>');
                var count = 1;
                for (var i = 0; i < response.changeTopicRequests.length; i++) {
                    $('.showRequestRejected').append(' <div class="informRejectedRequest" id="' + response.changeTopicRequests[i].requestID + '">'
                        + '<p>' + count + '</p>'
                        + '<a href="#" class="nameGroup">' + response.changeTopicRequests[i].finalGroup.groupName + '</a>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response.changeTopicRequests[i].emailSuperVisor + '</p>'
                        + '<div class="rejectedStatus">'
                        + '<p id="rejectedStatus">Rejected</p>'
                        + '</div>'
                        + '<p>' + response.changeTopicRequests[i].staffComment + '</p>'
                        + '</div >');
                    count++;
                }
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.showRequestRejected').append('<div class="pagination">'
                    + '<a  id = "previousBtn" > <i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a  id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>')
            }
        } else {
            if (status == 3) {
                $('.showRequestTopic').html('');
                $('.showRequestTopic').append('<h2 style="color:red">There are no request yet.</h2>');
            }
            if (status == 4) {
                $('.showRequestAccepted').html('');
                $('.showRequestAccepted').append('<h2 style="color:red">Nothing to display.</h2>');
            }
            if (status == -1) {
                $('.showRequestRejected').html('');
                $('.showRequestRejected').append('<h2 style="color:red">Nothing to display.</h2>');
            }
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: '<p class="popupTitle">Something is wrong! Try again later</p>'
        }).then(function () {
            window.location = "/ManageChangeTopic/Index";
        });
    });
}

$(document).ready(function () {
    var changeTopicRequestId;
    var accepBtn;
    var rejectBtn;
    $('body').on('click', '.acceptBtn', function () {
        changeTopicRequestId = $(this).val();
        accepBtn = $('.acceptBtn');
        accepBtn.attr('disabled', 'disabled');
        accepBtn.css('cursor', 'not-allowed');
        accepBtn.css('opacity', '0.5');

        rejectBtn = $('.rejectBtn');
        rejectBtn.attr('disabled', 'disabled');
        rejectBtn.css('cursor', 'not-allowed');
        rejectBtn.css('opacity', '0.5');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('body').on('click', '.discardBtn', function () {
        accepBtn.removeAttr('disabled', 'disabled');
        accepBtn.css('cursor', 'pointer');
        accepBtn.css('opacity', '1');

        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showFormConfirm', function (e) {
        if (e.target === e.currentTarget) {
            accepBtn.removeAttr('disabled', 'disabled');
            accepBtn.css('cursor', 'pointer');
            accepBtn.css('opacity', '1');

            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');
            $('.showFormConfirm').toggle('hide-form');
        }
    });

    $('body').on('click', '.submitBtn', function () {
        AjaxCall('/ManageChangeTopic/AcceptChangeTopicRequest?changeTopicRequestId=' + changeTopicRequestId, 'POST')
            .done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Submited Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            if ($('#requestPendingBtn').hasClass('active')) {
                                GetListChangeTopicRequests(3, searchText);
                            }
                        })
                        accepBtn.removeAttr('disabled', 'disabled');
                        accepBtn.css('cursor', 'pointer');
                        accepBtn.css('opacity', '1');

                        rejectBtn.removeAttr('disabled', 'disabled');
                        rejectBtn.css('cursor', 'pointer');
                        rejectBtn.css('opacity', '1');
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Submit is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Submit is wrong! Try again later</p>"
                }).then(function () {
                    window.location = "/ManageChangeTopic/Index";
                });
            })
        $('.showFormConfirm').toggle('hide-form');
    });
})

$(document).ready(function () {
    var changeTopicRequestId;
    var rejectBtn;
    var accepBtn;
    $('body').on('click', '.rejectBtn', function () {
        changeTopicRequestId = $(this).val();
        rejectBtn = $('.rejectBtn');
        rejectBtn.attr('disabled', 'disabled');
        rejectBtn.css('cursor', 'not-allowed');
        rejectBtn.css('opacity', '0.5');

        accepBtn = $('.acceptBtn');
        accepBtn.attr('disabled', 'disabled');
        accepBtn.css('cursor', 'not-allowed');
        accepBtn.css('opacity', '0.5');

        $('.showFormCommentReject').toggle('hide-form');
    });
    $('body').on('click', '.closeBtn', function () {
        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');

        accepBtn.removeAttr('disabled', 'disabled');
        accepBtn.css('cursor', 'pointer');
        accepBtn.css('opacity', '1');
        $('.showFormCommentReject').toggle('hide-form');
    });
    $('body').on('click', '.showFormCommentReject', function (e) {
        if (e.target === e.currentTarget) {
            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');

            accepBtn.removeAttr('disabled', 'disabled');
            accepBtn.css('cursor', 'pointer');
            accepBtn.css('opacity', '1');
            $('.showFormCommentReject').toggle('hide-form');
        }
    });

    $('body').on('click', '.rejectFormBtn', function () {
        var commentReject = $('#textAreaComent').val();
        AjaxCall('/ManageChangeTopic/RejectChangeTopicRequest?changeTopicRequestId=' + changeTopicRequestId + '&&commentReject=' + commentReject, 'POST')
            .done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Submited Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            if ($('#requestPendingBtn').hasClass('active')) {
                                GetListChangeTopicRequests(3, searchText, 'none', 0);
                            }
                        })
                        rejectBtn.removeAttr('disabled', 'disabled');
                        rejectBtn.css('cursor', 'pointer');
                        rejectBtn.css('opacity', '1');

                        accepBtn.removeAttr('disabled', 'disabled');
                        accepBtn.css('cursor', 'pointer');
                        accepBtn.css('opacity', '1');
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Submit is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Submit is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });
            })
        $('.showFormCommentReject').toggle('hide-form');
    });
})

// show request pending or accepted detail;
$(document).ready(function () {
    $('body').on('click', '.showInformRequest', function () {
        var requestId = this.id;
        $('div').remove('.popup')
        AjaxCall('/ManageChangeTopic/GetDetailRequestChangeTopic?requestId=' + requestId, 'POST').done(function (response) {
            $('.showDetailRequest').append('<div class="popup">'
                + '<p>Change Topic Detail</p>'
                + '<div class= "changeTopic">'
                + '<div class="titleShowDetailRequest">'
                + '<p>Old Topic</p>'
                + '<p>New Topic</p>'
                + ' </div>'
                + '<div class="showEnglishNameRequest">'
                + ' <p><span> English Name:</span><b>' + response.oldTopicNameEnglish + '</b></p>'
                + ' <p><span> English Name:</span><b>' + response.newTopicNameEnglish + '</b></p>'
                + ' </div>'
                + ' <div class="showVietNameseNameRequest">'
                + ' <p><span> VietNamese Name:</span><b>' + response.oldTopicNameVietNamese + '</b></p>'
                + ' <p><span> VietNamese Name:</span><b>' + response.newTopicNameVietNamese + '</b></p>'
                + ' </div>'
                + ' <div class="showAbbreviationRequest">'
                + ' <p><span> Abbreviation:</span><b>' + response.oldAbbreviation + '</b></p>'
                + ' <p><span> Abbreviation:</span><b>' + response.newAbbreviation + '</b></p>'
                + ' </div>'
                + ' </div>'
                + ' <div class="emailSupervisorRequest">'
                + '  <p class="titleRequest">Email Supervisor: </p>'
                + ' <p>' + response.emailSuperVisor + '</p>'
                + '</div>'
                + ' <div class="reasonChangeRequest">'
                + ' <p class="titleRequest">Reason Change Topic: </p>'
                + ' <p>' + response.reasonChangeTopic + '</p>'
                + ' </div>'
                + '<div class="button">'
                + '<button class="closeFormDetailBtn">Close</button>'
                + '</div>'
                + '</div>')
        }).fail(function (error) {
            alert(error.statusText);
        });
        $('.showDetailRequest').toggle('hide-form');
    });
    $('body').on('click', '.informRequestAccepted', function () {
        $('.showDetailRequest').toggle('hide-form');
    });
    $('body').on('click', '.closeFormDetailBtn', function () {
        $('.showDetailRequest').toggle('hide-form');
    });
    $('body').on('click', '.showDetailRequest', function (e) {
        if (e.target === e.currentTarget) {
            $('.showDetailRequest').toggle('hide-form');
        }
    });
});

// show request rejected detail;
$(document).ready(function () {
    $('body').on('click', '.informRejectedRequest', function () {
        var requestId = this.id;
        $('div').remove('.popup')
        AjaxCall('/ManageChangeTopic/GetDetailRequestChangeTopic?requestId=' + requestId, 'POST').done(function (response) {
            $('.showDetailRejectedRequest').append('<div class="popup">'
                + '<p>Change Topic Detail</p>'
                + '<div class= "changeTopic">'
                + '<div class="titleShowDetailRequest">'
                + '<p>Old Topic</p>'
                + '<p>New Topic</p>'
                + ' </div>'
                + '<div class="showEnglishNameRequest">'
                + ' <p><span> English Name:</span><b>' + response.oldTopicNameEnglish + '</b></p>'
                + ' <p><span> English Name:</span><b>' + response.newTopicNameEnglish + '</b></p>'
                + ' </div>'
                + ' <div class="showVietNameseNameRequest">'
                + ' <p><span> VietNamese Name:</span><b>' + response.oldTopicNameVietNamese + '</b></p>'
                + ' <p><span> VietNamese Name:</span><b>' + response.newTopicNameVietNamese + '</b></p>'
                + ' </div>'
                + ' <div class="showAbbreviationRequest">'
                + ' <p><span> Abbreviation:</span><b>' + response.oldAbbreviation + '</b></p>'
                + ' <p><span> Abbreviation:</span><b>' + response.newAbbreviation + '</b></p>'
                + ' </div>'
                + ' </div>'
                + ' <div class="emailSupervisorRequest">'
                + '  <p class="titleRequest">Email Supervisor: </p>'
                + ' <p>' + response.emailSuperVisor + '</p>'
                + '</div>'
                + ' <div class="reasonChangeRequest">'
                + ' <p class="titleRequest">Reason Change Topic: </p>'
                + ' <p>' + response.reasonChangeTopic + '</p>'
                + '  </div>'
                + '<div class="reasonReject">'
                + ' <p class="titleRequest">Reason Reject: </p>'
                + '<p>' + response.staffComment + '</p>'
                + '</div>'
                + '<div class="button">'
                + '<button class="closeFormDetailRejectBtn">Close</button>'
                + '</div>'
                + '</div>');
        }).fail(function (error) {
            alert(error.statusText);
        })
        $('.showDetailRejectedRequest').toggle('hide-form');
    });
    $('body').on('click', '.closeFormDetailRejectBtn', function () {
        $('.showDetailRejectedRequest').toggle('hide-form');
    });
    $('body').on('click', '.showDetailRejectedRequest', function (e) {
        if (e.target === e.currentTarget) {
            $('.showDetailRejectedRequest').toggle('hide-form');
        }
    });
});


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
            $('.rejectFormBtn').attr('disabled', 'disabled');
            $('.rejectFormBtn').css('cursor', 'not-allowed');
            $('.rejectFormBtn').css('opacity', '0.5');
        } else if (commentReject.length > 500) {
            $('.rejectFormBtn').attr('disabled', 'disabled');
            $('.rejectFormBtn').css('cursor', 'not-allowed');
            $('.rejectFormBtn').css('opacity', '0.5');
        } else {
            $('.rejectFormBtn').removeAttr('disabled', 'disabled');
            $('.rejectFormBtn').css('cursor', 'pointer');
            $('.rejectFormBtn').css('opacity', '1');
        }
    })
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

