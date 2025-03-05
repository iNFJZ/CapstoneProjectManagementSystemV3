/*$(document).ready(function () {
    $('.changeBtn').click(function () {
        var id = this.id;
        AjaxCall('/ManageSupport/ChangeStatusToProcessed', JSON.stringify(id), "POST").done(function (response) {
            if (response != null) {
                $('#div_' + id + '').empty();
                $('#div_' + id + '').append('<p class="processedStatus">Processed</p>');
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Successfully!</p>'
                })
            } else {
                Swal.fire({
                    icon: 'Error',
                    title: '<p class="popupTitle">Something wrong! Please try again later</p>'
                })
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
});*/


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