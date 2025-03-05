$(document).ready(function () {
    $('body').on('click', '.detailFormChange', function () {
        var requestId = this.id;
        $('div').remove('.popup')
        AjaxCall('/SupervisorChangeTopicRequest/GetDetailRequestChangeTopic?requestId=' + requestId, 'POST').done(function (response) {
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