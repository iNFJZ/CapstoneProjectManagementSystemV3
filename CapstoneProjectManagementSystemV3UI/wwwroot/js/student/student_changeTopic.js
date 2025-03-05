const formChangeTopicBtn = document.querySelector('.formChangeTopicBtn');
const requestChangeTopicBtn = document.querySelector('.requestChangeTopicBtn');
const formChange = document.querySelector('.formChange');
const requestChange = document.querySelector('.requestChange');

formChangeTopicBtn.addEventListener('click', () => {
    formChangeTopicBtn.classList.add('active');
    requestChangeTopicBtn.classList.remove('active');
    formChange.classList.remove('hide-form');
    requestChange.classList.add('hide-form');
})

requestChangeTopicBtn.addEventListener('click', () => {
    formChangeTopicBtn.classList.remove('active');
    requestChangeTopicBtn.classList.add('active');
    formChange.classList.add('hide-form');
    requestChange.classList.remove('hide-form');
})

$(document).ready(function () {
    $('body').on('click', '.detailFormChange', function () {
        var requestId = this.id;
        $('div').remove('.popup')
        AjaxCall('/ChangeTopic/GetDetailRequestChangeTopic?requestId=' + requestId, 'POST').done(function (response) {
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
                + '<button class="closeBtn">Close</button>'
                + '</div>'
                + '</div>')
        }).fail(function (error) {
            alert(error.statusText);
        });
        $('.showDetailRequest').toggle('hide-form');
    });
    $('body').on('click', '.closeBtn', function () {
        $('.showDetailRequest').toggle('hide-form');
    });
    $('body').on('click', '.showDetailRequest', function (e) {
        if (e.target === e.currentTarget) {
            $('.showDetailRequest').toggle('hide-form');
        }
    });
});

//load data when click requestchangetopicbtn
$('body').on('click', '.requestChangeTopicBtn', function () {
    AjaxCall('/ChangeTopic/GetListRequestChangeTopic', 'POST').done(function (response) {
        if (response != null) {
            $('.requestChange').html('');
            $('.requestChange').append('<div class="title">'
                + '<p> No.</p>'
                + '<p>Group Name</p>'
                + '<p>Old Topic</p>'
                + '<p>New Topic</p>'
                + '<p>Email Supervisor</p>'
                + '<p>Status</p>'
                + '<p>Comment Staff</p>'
                + '</div>');
            var count = 1;
            for (var i = 0; i < response.length; i++) {
                if (response[i].status == 0 || response[i].status == 2 || response[i].status == 3) {
                    $('.requestChange').append('<div class="detailFormChange" id="' + response[i].requestID + '">'
                        + '<p>' + count + '</p >'
                        + '<p class="groupName limitCharacter">' + response[i].finalGroup.groupName + '</p>'
                        + '<p class="limitCharacter">' + response[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].emailSuperVisor + '</p>'
                        + '<p class="pendingStatus">Pending</p>'
                        + '</div > ');
                }
                if (response[i].status == 4) {
                    $('.requestChange').append('<div class="detailFormChange" id="' + response[i].requestID + '">'
                        + '<p>' + count + '</p >'
                        + '<p class="groupName limitCharacter">' + response[i].finalGroup.groupName + '</p>'
                        + '<p class="limitCharacter">' + response[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].emailSuperVisor + '</p>'
                        + '<p class="acceptedStatus">Accepted</p>'
                        + '</div > ');
                }
                if (response[i].status == -1) {
                    $('.requestChange').append('<div class="detailFormChange" id="' + response[i].requestID + '">'
                        + '<p>' + count + '</p >'
                        + '<p class="groupName limitCharacter">' + response[i].finalGroup.groupName + '</p>'
                        + '<p class="limitCharacter">' + response[i].oldTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].newTopicNameEnglish + '</p>'
                        + '<p class="limitCharacter">' + response[i].emailSuperVisor + '</p>'
                        + '<p class="rejectedStatus">Rejected</p>'
                        + '<p>' + response[i].staffComment + '</p>'
                        + '</div>');
                }
                count++;
            }
        } else {
            $('.requestChange').html('');
            $('.requestChange').append("<h2 style='color:red'>You don't have any request change topic</h2>");
        }
    }).fail(function (error) {
        alert(error.statusText);
    })
})


//submit form 
$(document).ready(function () {
    var submitBtn;
    $('.submitBtn').click(function () {
        submitBtn = $(this);
        submitBtn.attr('disabled', 'disabled');
        submitBtn.css('cursor', 'not-allowed');
        submitBtn.css('opacity', '0.5');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('.discardBtn').click(function () {
        submitBtn.removeAttr('disabled', 'disabled');
        submitBtn.css('cursor', 'pointer');
        submitBtn.css('opacity', '1');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('.showFormConfirm').click(function (e) {
        if (e.target === e.currentTarget) {
            submitBtn.removeAttr('disabled', 'disabled');
            submitBtn.css('cursor', 'pointer');
            submitBtn.css('opacity', '1');
            $('.showFormConfirm').toggle('hide-form');
        }
    });
    $('body').on('click', '.submitFormBtn', function () {
        var finalGroup = {};
        finalGroup.groupName = $('#groupName').html();
        finalGroup.finalGroupID = $('.submitBtn').val();
        var changeTopicRequests = {};
        changeTopicRequests.finalGroup = finalGroup;
        changeTopicRequests.newTopicNameEnglish = $('#topicNameEnglish').val();
        changeTopicRequests.newTopicNameVietNamese = $('#topicVietNameseEnglish').val();
        changeTopicRequests.newAbbreviation = $('#abbreviation').val();
        changeTopicRequests.reasonChangeTopic = $('#reasonChangeTopic').val();
        AjaxCall('/ChangeTopic/SubmitChangeTopic', JSON.stringify(changeTopicRequests), 'POST').done(function (response) {
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Submited Successfully</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    window.location = "/ChangeTopic/Index";
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Submit is wrong! Try again later</p>",
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    window.location = "/ChangeTopic/Index";
                });
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Submit is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                window.location = "/ChangeTopic/Index";
            });
        })
        $('.showFormConfirm').toggle('hide-form');
    });
})

$(document).ready(function () {
    $('.submitBtn').attr('disabled', 'disabled');
    $('.submitBtn').css('cursor', 'not-allowed');
    $('.submitBtn').css('opacity', '0.5');

    $('#topicNameEnglish').blur(function () {
        if ($('#topicNameEnglish').val().replace(/\s/g, "").length <= 0) {
            $('#errortopicNameEnglish').html('This field is required');
        } else if ($('#topicNameEnglish').val().length > 100) {
            $('#errortopicNameEnglish').html('Input less than 100 characters');
        } else {
            $('#errortopicNameEnglish').html('');
        }
    })

    $('#topicVietNameseEnglish').blur(function () {
        if ($('#topicVietNameseEnglish').val().replace(/\s/g, "").length <= 0) {
            $('#errortopicNameVN').html('This field is required');
        } else if ($('#topicVietNameseEnglish').val().length > 100) {
            $('#errortopicNameVN').html('Input less than 100 characters');
        } else {
            $('#errortopicNameVN').html('');
        }
    })

    $('#abbreviation').blur(function () {
        if ($('#abbreviation').val().replace(/\s/g, "").length <= 0) {
            $('#errorabbreviation').html('This field is required');
        } else if ($('#abbreviation').val().length > 20) {
            $('#errorabbreviation').html('Input less than 20 characters');
        } else {
            $('#errorabbreviation').html('');
        }
    })

    $('#reasonChangeTopic').blur(function () {
        if ($('#reasonChangeTopic').val().replace(/\s/g, "").length <= 0) {
            $('#errorreasonChangeTopic').html('This field is required');
        } else if ($('#reasonChangeTopic').val().length > 3000) {
            $('#errorreasonChangeTopic').html('Input less than 3000 characters');
        } else {
            $('#errorreasonChangeTopic').html('');
        }
    })

    $('body').on('blur keyup', '#topicNameEnglish, #topicVietNameseEnglish, #abbreviation, #reasonChangeTopic', function () {
        if ($('#topicNameEnglish').val().replace(/\s/g, "").length <= 0 || $('#topicNameEnglish').val().length > 100
            || $('#topicVietNameseEnglish').val().replace(/\s/g, "").length <= 0 || $('#topicVietNameseEnglish').val().length > 100
            || $('#abbreviation').val().replace(/\s/g, "").length <= 0 || $('#abbreviation').val().length > 20
            || $('#reasonChangeTopic').val().replace(/\s/g, "").length <= 0 || ($('#reasonChangeTopic').val().length > 3000)) {
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

//ajax call
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}