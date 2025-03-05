
$(document).ready(function () {
    // onclick of button "Save"
    $('.isActive').on('click', function () {
        var name = $(this).prop('name');
        var status = $("input[name='" + name + "']:checked").val();
        var supervisorid = document.getElementById(name).value; 
        // Send request from AJAX to Controller
        $.ajax({
            url: "/ViewSupervisorForDevHead/UpdateStatusForSupervisor",
            data: { status: status, supervisorid: supervisorid },
            type: "POST",
            success: function (result) {
                Swal.fire({
                    title: 'Success',
                    text: 'Status updated successfully.',
                    icon: 'success'
                });
                //window.location.reload();
                // success
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Error',
                    text: 'Failed to update status.',
                    icon: 'error'
                });

                // error
            }
        });
    });
});

//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}




