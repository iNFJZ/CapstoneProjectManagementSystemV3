$(document).ready(function () {
    //$('#btnSave').reattr('disabled', 'disabled');
    //$('#btnSave').css('cursor', 'not-allowed');
    //$('#btnSave').css('opacity', '0.5');
    $('.updateIdea').click(function () {
        $('.showUpdateForm').toggle('hide-UpdateForm');
    });
    $('#btnDiscard2').click(function () {
        $('.showUpdateForm').toggle('hide-UpdateForm');
    });
    $('.showUpdateForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showUpdateForm').toggle('hide-UpdateForm');
        }
    });
});

//keyword handle
const ul = document.querySelector(".addTag"),
    input = document.getElementById("inputTag");

let maxTags = 10, tags = [];
ul.querySelectorAll("li").forEach(li => tags.push(li.textContent));

function createTag() {
    ul.querySelectorAll("li").forEach(li => li.remove());
    tags.slice().reverse().forEach(tag => {
        let liTag = `<li>${tag} <i class="fa-solid fa-xmark" id="removeTag"></i></li>`;
        ul.insertAdjacentHTML("afterbegin", liTag);
    });
}

$('body').on('click', '#removeTag', function () {
    var x = $(this).parent('li').text();
    let index = tags.indexOf(x);
    tags = [...tags.slice(0, index), ...tags.slice(index + 1)]
    $(this).parent('li').remove();
})

//function remove(element, tag) {
//    let index = tags.indexOf(tag);
//    tags = [...tags.slice(0, index), ...tags.slice(index + 1)];
//    element.parentElement.remove();
//}

function addTag(e) {
    if (e.key == "Enter") {
        let tag = e.target.value.replace(/\s+/g, ' ');
        if (tag.length > 1 && !tags.includes(tag)) {
            if (tags.length < 10) {
                tag.split(',').forEach(tag => {
                    tags.push(tag);
                    createTag();
                });
            }
        }
        e.target.value = "";
    }
    tags = [];
    ul.querySelectorAll("li").forEach(li => tags.push(li.textContent));
}

input.addEventListener("keyup", addTag);

//Save
var listMember = [];

var currentNotification = '';
$(function () {
    $('#btnAdd').on("click", function () {
        var txtMember = $('#txtMember').val().trim();
        var checkConfig = false;
        for (var i = 0; i < listStudentConfig.length; i++) {
            if (txtMember.toLowerCase() == listStudentConfig[i].toLowerCase()) {
                checkConfig = true;
                break;
            }
        }
        if (!checkConfig) {
            updateNotification('The student you chose cannot be invited because his/her specialty is not allowed to work on the same thesis topic as yours!');
            return;
        }
        var memberAdded = '';
        updateNotification('');
        AjaxCall('/MyGroup/CheckStudentExistWhenBeforeAdded', JSON.stringify(txtMember), "POST").done(function (response) {
            if (response != null) {
                if (listMember.some(item => item.studentId === response.studentID)) {
                    updateNotification('This student has been added');
                }
                else {
                    listMember.push({
                        'studentId': response.studentID,
                        'avatar': response.user.avatar,
                        'fptEmail': response.user.fptEmail
                    });
                    memberAdded =
                        '<div class="existMem" id="existMember">'
                        + '<div class="nameMem">'
                        + '<p hidden >' + response.studentID + '</p >'
                    + '<img src = "' + response.user.avatar + '" alt = "Avatar">'
                        + '<p>' + response.user.fptEmail + '</p >'
                        + '</div>'
                        + '</div>';
                    $('.member').append(memberAdded);
                }
            } else {
                updateNotification('This student was not found or already in group or ineligible!');
            }
        })
    });

    function updateNotification(message) {
        if (message !== currentNotification) {
            currentNotification = message;
            $('#noti').html(message);
        }
    }

    $('#btnSave').click(function () {
        var strTag = '';
        for (var i = 0; i < tags.length; i++) {
            if (tags[tags.length - 1] == tags[i]) {
                strTag += tags[i]
            } else {
                strTag += tags[i] + ',';
            }
        }
        var profession = {};
        profession.professionID = $('.profesId').attr('id');
        var specialty = {};
        specialty.specialtyID = $('.specialId').attr('id');
        var groupIdea = {};
        groupIdea.projectEnglishName = $('#txtEnglishTitle').val();
        groupIdea.MaxMember = $('#maxMember').val();
        groupIdea.NumberOfMember = $('#numberOfMember').val();
        groupIdea.abrrevation = $('#txtShortTitle').val();
        groupIdea.projectVietNameseName = $('#txtVietNameseTitle').val();
        groupIdea.description = $('#desDetail').val();
        groupIdea.profession = profession;
        groupIdea.specialty = specialty;
        groupIdea.ProjectTags = strTag;
        groupIdea.Students = listMember;
        groupIdea.groupIdeaID = $('.updateIdea').attr('id');
        $('#notiWhenCreate').append($('#notiWhenCreate').html(''));
            AjaxCall('/MyGroup/UpdateIdea', JSON.stringify(groupIdea), "POST").done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: "<p class='popupTitle'>You updated an idea succesfully</p>",
                        timer: 1000,
                        showConfirmButton: false
                    }).then(function () {
                        window.location = "/MyGroup/Index";
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>You created an idea unsuccesfully</p>",
                        timer: 1000,
                        showConfirmButton: false
                    });
                }
            });
    });
});


//Validate Form Update Idea
$(document).ready(function () {
    /*
    $('#txtEnglishTitle').blur(function () {
        if ($('#txtEnglishTitle').val().replace(/\s/g, "").length <= 0) {
            $('#errortxtEnglishTitle').html('This field is required');
        } else if ($('#txtEnglishTitle').val().length > 100) {
            $('#errortxtEnglishTitle').html('Input less than 100 characters');
        } else {
            $('#errortxtEnglishTitle').html('');
        }
    })
    */
    $('#txtShortTitle').blur(function () {
        if ($('#txtShortTitle').val().replace(/\s/g, "").length <= 0) {
            $('#errortxtShortTitle').html('This field is required');
        } else if ($('#txtShortTitle').val().length > 20) {
            $('#errortxtShortTitle').html('Input less than 20 characters');
        } else {
            $('#errortxtShortTitle').html('');
        }
    })
    /*
    $('#txtVietNameseTitle').blur(function () {
        if ($('#txtVietNameseTitle').val().replace(/\s/g, "").length <= 0) {
            $('#errortxtVietNameseTitle').html('This field is required');
        } else if ($('#txtVietNameseTitle').val().length > 100) {
            $('#errortxtVietNameseTitle').html('Input less than 100 characters');
        } else {
            $('#errortxtVietNameseTitle').html('');
        }
    })
    $('#desDetail').blur(function () {
        if ($('#desDetail').val().replace(/\s/g, "").length <= 0) {
            $('#errordesDetail').html('This field is required');
        } else if ($('#desDetail').val().length > 2000) {
            $('#errordesDetail').html('Input less than 2000 characters');
        } else {
            $('#errordesDetail').html('');
        }
    })
    */
    $('#txtEnglishTitle, #txtShortTitle, #txtVietNameseTitle, #desDetail').keyup(function () {
        if ($('#txtEnglishTitle').val().replace(/\s/g, "").length <= 0
            || $('#txtShortTitle').val().replace(/\s/g, "").length <= 0 || $('#txtShortTitle').val().length > 20
            || $('#txtVietNameseTitle').val().replace(/\s/g, "").length <= 0
            || $('#desDetail').val().replace(/\s/g, "").length <= 0) {
            $('#btnSave').attr('disabled', 'disabled');
            $('#btnSave').css('cursor', 'not-allowed');
            $('#btnSave').css('opacity', '0.5');      
        } else {
            $('#btnSave').removeAttr('disabled', 'disabled');
            $('#btnSave').css('cursor', 'pointer');
            $('#btnSave').css('opacity', '1');    
        }
    });

})
$(".checkInput").on("keydown", function (event) {
    if (event.key === " " && event.target.selectionStart === 0) {
        event.preventDefault();
    }
});
var listStudentConfig;
$(document).ready(function () {
    // Gửi yêu cầu HTTP để lấy danh sách email dựa trên searchTerm
    $.ajax({
        url: `/MyGroup/GetEmailList?specialityIdOfGroupIdea=${specId}`,
        type: "POST",
        success: function (data) {
            listStudentConfig = data;;
            $("#txtMember").autocomplete({
                source: data,
            });
        }
    });
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

