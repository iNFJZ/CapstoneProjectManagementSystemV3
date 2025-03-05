
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}


$(document).ready(function () {
    AjaxCall('/SemesterManage/GetCurrentSemester', 'POST').done(function (response) {
        if (response != null) {
            $('#semesterName').html('Semester: ' + response.semesterCode);
        }
    }).fail(function (error) {
        console.log(error.statusText)
    })
})
