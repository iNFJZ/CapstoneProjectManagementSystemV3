$('#role').change(function () {
    $('#btnSearch').click();
});

//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: type,
//        contentType: "application/json; charset=utf-8",
//    });
//}

function DeleteAfterCheckReference(userId) {
    AjaxCall('/ListUser/DeleteUser/?userId=' + userId, null, 'GET').done(function (result) {
        if (result == 1) {
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>Delete successfully</p>",
                timer: 1500,
                showConfirmButton: false
            }).then((result) => {
                location.reload();
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something went wrong</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
    })
}

$('.btn-delete').on('click', function () {
    var userId = $(this).val();

    AjaxCall('/ListUser/CheckReference?userId=' + userId, null, 'GET').done(function (respone) {
        if (respone == 0)
            Swal.fire({
                icon: 'warning',
                title: "<p class='popupTitle'>This account has reference data</p>",
                text: 'Are you sure to delete?',
                showDenyButton: true,
                confirmButtonText: 'Yes',
                denyButtonText: `No`
            }).then((result) => {
                if (result.isConfirmed) {
                    DeleteAfterCheckReference(userId);
                }
            });
        else if (respone == 1) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Can not delete this account because it has reference data</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (respone == 2) {
            Swal.fire({
                icon: 'warning',
                title: "<p class='popupTitle'>Are you sure to delete</p>",
                text: 'This account has not reference data yet',
                showDenyButton: true,
                confirmButtonText: 'Yes',
                denyButtonText: `No`
            }).then((result) => {
                if (result.isConfirmed) {
                    DeleteAfterCheckReference(userId);
                }
            });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something went wrong</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something went wrong</p>",
            timer: 1500,
            showConfirmButton: false
        })
    })
})

$('.btnloginAs').click(function () {
    var fptEmail = $(this).val();
    AjaxCall('/AdminLoginAs/LoginAs', JSON.stringify(fptEmail), 'POST').done(function (response) {
        if (response == 0) {
            Swal.fire({
                icon: 'error',
                title: '<p class="popupTitle">Something is wrong! Try again later</p>'
            }).then(function () {
                $('#inputEmailOfOtherRole').val('');
            });
        }
        if (response == 1) {
            Swal.fire({
                icon: 'success',
                title: '<p class="popupTitle">Login As successfully</p>'
            }).then(function () {
                window.location.href = '/StudentHome/Index'
            });
        }

        if (response == 2) {
            Swal.fire({
                icon: 'error',
                title: '<p class="popupTitle">Login As unsuccessfully</p>'
            }).then(function () {
                $('#inputEmailOfOtherRole').val('');
            });
        }
    }).fail(function (error) {

    })
})