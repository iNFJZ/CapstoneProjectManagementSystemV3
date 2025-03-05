$(document).ready(function () {
    $('.submitFormBtn').click(function () {
        var fileInput = document.getElementById("fileInput");
        var file = fileInput.files[0];

        var question = {
            Title: $('.inputTitle').val(),
            Content: tinymce.get('desSupportForm').getContent(),
            TypeSupport: $('input[name="type"]').filter(':checked').val() === "1",
            Staff: {
                StaffID: $('.staffId').val()
            }
        };

        var formData = new FormData();
        formData.append("file", file);
        formData.append("questionJsonString", JSON.stringify(question));
        console.log(question);
        $.ajax({
            url: '/ManageSupportQuestion/AddSupportQuestion',
            data: formData,
            type: "POST",
            processData: false,
            contentType: false
        }).done(function (response) {
            
            if (response == true) {
                $('.inputTitle').val("");
                $('.inputDescription').val("");
                
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Create Successfully!</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">Something wrong! Please try again later</p>',
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    location.reload(true);
                });
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
});


$(document).ready(function () {
   
    $('.submitFormBtn').attr('disabled', 'disabled');
    $('.submitFormBtn').css('cursor', 'not-allowed');
    $('.submitFormBtn').css('opacity', '0.5');
    $('#inputTitle').blur(function () {
        if ($('#inputTitle').val().replace(/\s/g, "").length <= 0) {
            $('#errorinputTitle').html('This field is required');
        } else if ($('#inputTitle').val().length > 500) {
            $('#errorinputTitle').html('Input less than 500 characters');
        } else {
            $('#errorinputTitle').html('');
        }
    })
    
    $('#desSupportForm').blur(function () {
        var content = tinymce.get('desSupportForm').getContent({ format: 'text' });

        if (content.replace(/\s/g, "").length <= 0) {
            $('#errordesSupportForm').html('This field is required');
        } else if ($('#desSupportForm').val().length > 2000) {
            $('#errordesSupportForm').html('Input less than 2000 characters');
        } else {
            $('#errordesSupportForm').html('');
        }
    })

    $('body').on('blur keyup', '#inputTitle, #desSupportForm', function () {
        var content = tinymce.get('desSupportForm').getContent({ format: 'text' });
        console.log(content.length);
        if ($('#inputTitle').val().replace(/\s/g, "").length <= 0 || $('#inputTitle').val().length > 300
            || content.replace(/\s/g, "").length <= 0) {
            $('#submitFormSupportBtn').attr('disabled', 'disabled');
            $('#submitFormSupportBtn').css('cursor', 'not-allowed');
            $('#submitFormSupportBtn').css('opacity', '0.5');
        } else {
            $('#submitFormSupportBtn').removeAttr('disabled', 'disabled');
            $('#submitFormSupportBtn').css('cursor', 'pointer');
            $('#submitFormSupportBtn').css('opacity', '1');
        }
    });
})


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