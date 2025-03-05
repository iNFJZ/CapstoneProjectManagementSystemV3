$(document).ready(function () {
    $('#btnLoginAs').click(function () {
        var studentEmail = $('#inputEmailOfStudent').val();
        AjaxCall('/LoginAs', JSON.stringify(studentEmail), 'POST').done(function (response) {
            if (response.isSuccessed == true) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Login As successfully</p>'
                }).then(function () {
                    window.location.href = '/StudentHome/Index'
                });
            }
            if (response.isSuccessed == false) {
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
//    var api = `https://localhost:7178${url}`;
//    return $.ajax({
//        url: api,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}
