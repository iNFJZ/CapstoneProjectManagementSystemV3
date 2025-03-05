$(document).ready(function () {
    var userId;
    var groupDetail;
    $('#btnEditTopic').click(function () {
        var loadingIcon = $(".fa-spin");
        $('.inputTopic').toggle();
        $('.textTopic').toggle();
        var status = $(this).data("status");
        if (status == 1) {
            $(this).html('Save <i class="fas fa-spinner fa-spin" style="display: none;"> </i>');
            $("#engName-input").val(groupDetail.projectEnglishName);
            $("#vietName-input").val(groupDetail.projectVietNameseName);
            $("#abbreName-input").val(groupDetail.abbreviation);
            $("#description-input").val(groupDetail.description);

            $(this).data("status", 0);
        } else {
            $(this).html('Edit Topic <i class="fas fa-spinner fa-spin" style="display: none;"> </i>');
            $(this).data("status", 1);
            var engName = $("#engName-input").val();
            var vietName = $("#vietName-input").val();
            var abbreName = $("#abbreName-input").val();
            var description = $("description-input").val();
            AjaxCall(`/ManageGroup/UpdateNewTopic?engName=${engName}&abbreName=${abbreName}&vietName=${vietName}&finalGroupID=${groupDetail.finalGroupID}`, "", "post").done(function (request) {
                if (request) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Update topic successfully.',
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                    var newEngName = request.newTopicNameEnglish;
                    var newVietName = request.newTopicNameVietNamese;
                    var newAbbre = request.newAbbreviation;
                    var newDes = request.newDescription;
                    groupDetail.projectEnglishName = newEngName;
                    groupDetail.projectVietNameseName = newVietName;
                    groupDetail.abbreviation = newAbbre;
                    $('#engName').html(newEngName);
                    $('#abbreName').html(newAbbre);
                    $('#vietName').html(newVietName);
                    $('#description').html(newDes);
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Update topic unsuccessfully.',
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    title: 'Error!',
                    text: 'Something went wrong.',
                    icon: 'error',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                });
            }).always(function () {
                loadingIcon.toggle();
            });
        }
    });

    $('.nameProject').click(function () {
        var groupId = this.id.substring(6);

        AjaxCall('/ManageGroup/GetGroupInfo', JSON.stringify(groupId), "POST").done(function (response) {
            groupDetail = response;
            if (response != null) {
                $('.form').attr('id',response.finalGroupID);
                $('.member').empty();
                $('.member').append('<div class="TM">\
                                    <div class= "TM--left" > <p class="titleForm">Team Members</p></div>\
                                    <div class="TM--right"><p class= "titleForm" >Group Name</p></div></div>');
                /*add leader*/
                if (response.students.length > 1) {
                    $('.member').append('<div class="existMem" id="existMember_' + response.students[0].studentID + '">\
                                                <div class= "nameMem">\
                                                    <img id="groupLeadAvatar" src="'+ response.students[0].user.avatar + '" alt="Avatar">\
                                                    <p id="fptEmail">'+ response.students[0].user.fptEmail + ' (leader)</p>\
                                                </div>\
                                                <input class="groupNameOfStudent" id="groupName_'+ response.students[0].rollNumber + '" value="' + response.students[0].groupName + '">\
                                                <button class="btnChangeGNforStu" id="changeGN_'+ response.students[0].rollNumber + '">Change</button>\
                                                <button class="btnSaveGNforStu hide-btn" id="saveGN_'+ response.students[0].rollNumber + '">Save</button>\
                                                <button class="btnDelete" id="'+ response.students[0].studentID + '">Remove</button>\
                                            </div>');
                } else {
                    $('.member').append('<div class="existMem" id="existMember_' + response.students[0].studentID + '">\
                                                <div class= "nameMem">\
                                                    <img id="groupLeadAvatar" src="'+ response.students[0].user.avatar + '" alt="Avatar">\
                                                    <p id="fptEmail">'+ response.students[0].user.fptEmail + ' (leader)</p>\
                                                </div>\
                                                <input class="groupNameOfStudent" id="groupName_'+ response.students[0].rollNumber + '" value="' + response.students[0].groupName + '">\
                                                <button class="btnChangeGNforStu" id="changeGN_'+ response.students[0].rollNumber + '">Change</button>\
                                                <button class="btnSaveGNforStu hide-btn" id="saveGN_'+ response.students[0].rollNumber + '">Save</button>\
                                                <button class="btnDelete" id="'+ response.students[0].studentID + '" disabled >Remove</button>\
                                            </div>');
                }
                /*add other member*/
                for (var i = 1; i < response.students.length; i++) {
                    $('.member').append('<div class="existMem" id="existMember_' + response.students[i].studentID +'">\
                                                <div class= "nameMem">\
                                                    <img id="groupLeadAvatar" src="'+ response.students[i].user.avatar + '" alt="Avatar">\
                                                    <p id="fptEmail">'+ response.students[i].user.fptEmail + '</p>\
                                                </div>\
                                                <input class="groupNameOfStudent" id="groupName_'+ response.students[i].rollNumber +'" value="'+ response.students[i].groupName + '">\
                                                <button class="btnChangeGNforStu" id="changeGN_'+ response.students[i].rollNumber +'">Change</button>\
                                                <button class="btnSaveGNforStu hide-btn" id="saveGN_'+ response.students[i].rollNumber +'">Save</button>\
                                                <button class="btnDelete" id="'+ response.students[i].studentID +'">Remove</button>\
                                            </div>');
                }
                $('#groName').val(response.groupName);
                $('#proName').html(response.profession.professionFullName);
                $('#specName').html(response.specialty.specialtyFullName);
                $('#engName').html(response.projectEnglishName);
                $('#abbreName').html(response.abbreviation);
                $('#vietName').html(response.projectVietNameseName);
                $('#description').html(response.description);

                $('.showForm').toggle('hide-form');
                $('.existMem' + ' input').prop('disabled', true);

                /*event when click delete button*/
                $('.btnDelete').click(function () {
                    $('.yesBtnDelete').removeAttr('disabled', 'disabled');
                    $('.yesBtnDelete').css('cursor', 'pointer');
                    $('.yesBtnDelete').css('opacity', '1');
                    $('.showDeleteForm').toggle('hide-form');
                    userId = this.id;
                });
                $('.noBtnDelete').click(function () {
                    $('.showDeleteForm').toggle('hide-form');
                });
                $('.showDeleteForm').click(function (e) {
                    if (e.target === e.currentTarget) {
                        $('.showDeleteForm').toggle('hide-form');
                    }
                });
                $('.yesBtnDelete').click(function () {
                    $('.yesBtnDelete').attr('disabled', 'disabled');
                    $('.yesBtnDelete').css('cursor', 'not-allowed');
                    $('.yesBtnDelete').css('opacity', '0.5');
                    AjaxCall('/ManageGroup/DeleteMember', JSON.stringify(userId), "POST").done(function (response) {
                        if (response == true) {
                            userId = "existMember_" + userId;
                            document.getElementById(userId).remove();
                            Swal.fire({
                                icon: 'success',
                                title: '<p class="popupTitle">Delete Successfully!</p>'
                            })
                        }
                        $('.showDeleteForm').toggle('hide-form');
                    }).fail(function (error) {
                        alert(error.StatusText);
                    });                   
                });
                /*change group name of student*/
                $('.btnChangeGNforStu').click(function () {
                    var id = this.id.substring(9);
                    $('#groupName_'+ id + '').prop('disabled', false);
                    $('#changeGN_'+ id +'').toggle('hide-btn');
                    $('#saveGN_' + id + '').toggle('hide-btn');
                });
                /*save group name of student*/
                $('.btnSaveGNforStu').click(function () {
                    var id = this.id.substring(7);
                    var stuID = this.parentElement.id.substring(12);
                    var student = {};
                    student.StudentID = stuID;
                    student.GroupName = $('#groupName_' + id + '').val();
                    AjaxCall('/ManageGroup/UpdateGroupNameOfStudent', JSON.stringify(student), "POST").done(function (response) {
                        Swal.fire({
                            icon: 'success',
                            title: '<p class="popupTitle">Save Successfully!</p>'
                        })
                    }).fail(function (error) {
                        alert(error.StatusText);
                    });
                    $('#groupName_' + id + '').prop('disabled', true);
                    $('#changeGN_' + id + '').toggle('hide-btn');
                    $('#saveGN_' + id + '').toggle('hide-btn');
                });
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });  
    });
    $('#btnDiscard').click(function () {
        $('.showForm').toggle('hide-form');
        $('.searchBtn').click();
    });
    //$('.showForm').click(function (e) {
    //    if (e.target === e.currentTarget) {
    //        $('.showForm').toggle('hide-form');
    //    }
    //});

    /*save group name*/
    $('#btnSave').on('click', function () {
        var groupId = $('.form').attr('id');
        var groupName = $('#groName').val();
        var group = {};
        group.FinalGroupID = groupId;
        group.GroupName = groupName;
        AjaxCall('/ManageGroup/UpdateGroupName', JSON.stringify(group), "POST").done(function (response) {
            Swal.fire({
                icon: 'success',
                title: '<p class="popupTitle">Save Successfully!</p>'
            })
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });   

/*confirm popup for add button*/
    $('#btnAdd').click(function () {
        $('.yesBtnJoin').removeAttr('disabled', 'disabled');
        $('.yesBtnJoin').css('cursor', 'pointer');
        $('.yesBtnJoin').css('opacity', '1');
        $('.showJoinForm').toggle('hide-form');
    });
    $('.noBtnJoin').click(function () {
        $('.showJoinForm').toggle('hide-form');
    });
    $('.showJoinForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showJoinForm').toggle('hide-form');
        }
    });
    $('.yesBtnJoin').click(function () {
        $('.yesBtnJoin').attr('disabled', 'disabled');
        $('.yesBtnJoin').css('cursor', 'not-allowed');
        $('.yesBtnJoin').css('opacity', '0.5');
        var data = {};
        data.groupId = $('.form').attr('id');
        data.fptEmail = $('#txtMember').val();
        AjaxCall('/ManageGroup/AddMemberToGroup', JSON.stringify(data), "POST").done(function (response) {
            if (response != null) {
                //if (response == false) {
                //    $('#noti').append('This group has enough member.');
                //} else {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Add Successfully!</p>'
                    })
                    $('.member').append('<div class="existMem" id="existMember_' + response.studentID + '">\
                                                <div class= "nameMem">\
                                                    <img id="groupLeadAvatar" src="'+ response.user.avatar + '" alt="Avatar">\
                                                    <p id="fptEmail">'+ response.user.fptEmail + '</p>\
                                                </div>\
                                                <input class="groupNameOfStudent" id="groupName_'+ response.rollNumber + '" value="' + response.groupName + '">\
                                                <button class="btnChangeGNforStu" id="changeGN_'+ response.rollNumber + '">Change</button>\
                                                <button class="btnSaveGNforStu hide-btn" id="saveGN_'+ response.rollNumber +'">Save</button>\
                                                <button class="btnDelete" id="'+ response.studentID + '">Remove</button>\
                                            </div>');
                    $('.groupNameOfStudent').prop('disabled', true);
                    $('#txtMember').attr('value', '');
                    /*change group name of student*/
                    $('#changeGN_' + response.rollNumber + '').click(function () {
                        var id = this.id.substring(9);
                        $('#groupName_' + id + '').prop('disabled', false);
                        $('#changeGN_' + id + '').toggle('hide-btn');
                        $('#saveGN_' + id + '').toggle('hide-btn');
                    });
                    /*save group name of student*/
                    $('#saveGN_' + response.rollNumber + '').click(function () {
                        var id = this.id.substring(7);
                        var stuID = this.parentElement.id.substring(12);
                        var student = {};
                        student.StudentID = stuID;
                        student.GroupName = $('#groupName_' + id + '').val();
                        AjaxCall('/ManageGroup/UpdateGroupNameOfStudent', JSON.stringify(student), "POST").done(function (response) {
                            Swal.fire({
                                icon: 'success',
                                title: '<p class="popupTitle">Save Successfully!</p>'
                            })
                        }).fail(function (error) {
                            alert(error.StatusText);
                        });
                        $('#groupName_' + id + '').prop('disabled', true);
                        $('#changeGN_' + id + '').toggle('hide-btn');
                        $('#saveGN_' + id + '').toggle('hide-btn');
                    });
                    /*event when click delete button*/
                    $('.btnDelete').unbind();
                    $('.btnDelete').click(function () {
                        $('.yesBtnDelete').removeAttr('disabled', 'disabled');
                        $('.yesBtnDelete').css('cursor', 'pointer');
                        $('.yesBtnDelete').css('opacity', '1');
                        $('.showDeleteForm').toggle('hide-form');
                        userId = this.id;
                    });
                    
               /* }*/
            } else {
                $('#noti').append('This student was not found or already in group.');
            }
            $('.showJoinForm').toggle('hide-form');
        }).fail(function (error) {
            alert(error.StatusText);
        });     
    });

/*confirm popup for delete group button*/
    $('.btnDelGroup').click(function () {
        $('.showDeleteGroupForm').toggle('hide-form');
    });
    $('.noBtnDeleteGroup').click(function () {
        $('.showDeleteGroupForm').toggle('hide-form');
    });
    $('.showDeleteGroupForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showDeleteGroupForm').toggle('hide-form');
        }
    });
    $('.yesBtnDeleteGroup').click(function () {
        $('.yesBtnDeleteGroup').attr('disabled', 'disabled');
        $('.yesBtnDeleteGroup').css('cursor', 'not-allowed');
        $('.yesBtnDeleteGroup').css('opacity', '0.5');
        var groupId = $('.form').attr('id');
        AjaxCall('/ManageGroup/DeleteGroup', JSON.stringify(groupId), "POST").done(function (response) {                
            $('.searchBtn').click();
            $('.showDeleteGroupForm').toggle('hide-form');
            $('.showForm').toggle('hide-form');
        }).fail(function (error) {
            alert(error.StatusText);
        });        
    });

});

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
