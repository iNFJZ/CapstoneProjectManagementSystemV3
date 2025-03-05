$(document).ready(function () {
    $('#btnLoginAs').click(function () {
        var studentEmail = $('#inputEmailOfOtherRole').val();
        AjaxCall('/AdminLoginAs/LoginAs', JSON.stringify(studentEmail), 'POST').done(function (response) {
            console.log("response", response);
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Login As successfully</p>'
                }).then(function () {
                    window.location.href = '/ListUser/Index'
                });
            }
            if (response == false) {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">Login As unsuccessfully</p>'
                }).then(function () {
                    $('#inputEmailOfStudent').val('');
                });
            }
        }).fail(function (error) {

        })
    })
})

// Call Ajax
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: type,
//        contentType: "application/json; charset=utf-8",
//    });
//}
