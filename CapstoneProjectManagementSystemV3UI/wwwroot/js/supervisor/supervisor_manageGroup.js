$("input[name='statuses-all']").change(function () {
    if (this.checked) {
        $("input[name='statuses']").prop("checked", true);
    } else {
        $("input[name='statuses-all']").prop("checked", false);
        $("input[name='statuses']").prop("checked", false);
    }
});

function toggle() {
    var element = document.getElementsByClassName("status-filter-menu")[0];
    if (element.style.display === "none") {
        element.style.display = "block";
    } else {
        element.style.display = "none";
    }
}

function UpdateStatusRegisterGroup(event, status, requestId) {
    event.stopPropagation();
    var message = '';
    if (status == 0) message = 'cancel';
    else if (status == 1) message = 'accept';
    else message = 'reject';

    Swal.fire({
        icon: 'question',
        title: `Are you sure you want to ${message} the request?`,
        showCancelButton: true,
        confirmButtonText: 'Confirm',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            var registeredGroup = {};
            registeredGroup.Status = status;
            registeredGroup.RegisteredGroupID = requestId;

            AjaxCall('/Group/UpdateStatusRegisterGroup', JSON.stringify(registeredGroup), 'POST').done(function (response) {
                if (response == 1) {
                    Swal.fire({
                        icon: 'success',
                        title: `<p class='popupTitle'>${message} successfully</p>`,
                        timer: 1000,
                        showConfirmButton: false
                    }).then(() => {
                        window.location.href = "/Group/Index";
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: `<p class='popupTitle'>${message} fail</p>`,
                        timer: 1500,
                        showConfirmButton: false
                    });
                }
            }).fail(function (error) {
                alert(error.StatusText);
            });
        }
    });
}


// registeredGroup detail
$(document).ready(function () {
    $('body').on('click', '.detailFormChange', function () {
        var groupId = this.id;
        $('div').remove('.popup')
        AjaxCall('/Group/GetDetailRegisterGroup?groupId=' + groupId, 'GET').done(function (response) {
            var projectAva = '';
            for (var i = 0; i < response.listInforStudentInGroupIdea.length; i++) {
                if (response.listInforStudentInGroupIdea[i].isLeader == true) {
                    projectAva = response.listInforStudentInGroupIdea[i].user.avatar;
                    break;
                }
            }

            var nameProject =
                ' <img src="' + projectAva + '" alt = "Avatar" />'
                + '<div class="inforProject">'
                + '<h3>' + response.groupInfo.groupIdea.projectEnglishName + '</h3>'
                + '<p>Create at: <span>' + moment(response.groupInfo.registeredGroup.createdAt).format('DD-MM-YYYY'); + '</span></p>'
                    + '</div >'
            $('.nameProject').html(nameProject);

            var vietnamTitle = ' <p class="title">Vietnamese Title</p>'
                + '<p class="content">' + response.groupInfo.groupIdea.projectVietNameseName + '</p> '
            $('.vietnamTitle').html(vietnamTitle);

            var professional = '<p class="title">Professional</p>'
                + '<p class="content">' + response.groupInfo.groupIdea.profession.professionFullName + '</p >'
            $('.professional').html(professional);

            var specialty = '<p class="title">Specialty</p>'
                + ' <p class="content">' + response.groupInfo.groupIdea.specialty.specialtyFullName + '</p>'
            $('.specialty').html(specialty);

            var abbreviations = '<p class="title">Abbreviations</p>'
                + '<p class="content">' + response.groupInfo.groupIdea.abrrevation + '</p>'
            $('.abbreviations').html(abbreviations);

            var description = ' <p class="title">Description</p>'
                + '<p class="content">' + response.groupInfo.groupIdea.description + '</p> '
            $('.description').html(description);

            var basicInformSupervisor = '<div class="inputInformSupervisor">'
                + '<p> Full Name</p>'
                + '<p>' + response.groupInfo.supervisor.user.fullName + '</p>'
                + '</div>'
            for (var idea of response.groupInfo.groupIdeaOfSupervisors) {
                basicInformSupervisor += '<div class="inputInformSupervisor">'
                    + '<p>Idea</p>'
                    + '<p>' + idea.projectEnglishName + '</p>'
                    + '</div>'
            }

            $('#basicInformSupervisor').html(basicInformSupervisor);

            if (response.groupInfo.studentComment) {
                var commentStudent = '<p class="title">Student Comment</p>'
                    + '<p>' + response.groupInfo.studentComment + '</p>'
                $('.commentStudent').html(commentStudent);
            } else {
                $('.commentStudent').html('');
            }
            var numMember = '<p>Total: <span>' + response.groupInfo.groupIdea.numberOfMember + ' members</span></p>'
            $('.numMember').html(numMember);

            var memberInfor = '';
            memberInfor += '<div class="showInfomember" id="memberinfor">'
                + '<div class="info" >'
                + '<img src = "' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).user.avatar + '" alt = "">'
                + '<div class="memInfo">'
                + '<a>' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).user.fptEmail + '</a>'
                + '<div class = "majorOfStu">'
                + '<p>Profession : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).profession.professionFullName + '</span></p>'
                + '<p>Specialty : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).specialty.specialtyFullName + '</span></p>'
                + '</div>'
                + '</div>'
                + '</div>'
                + '<div class="role">'
                + '<p>Leader</p >'
                + ' </div >'
                + '</div>';

            for (var i = 0; i < response.listInforStudentInGroupIdea.length; i++) {
                if (response.listInforStudentInGroupIdea[i].isLeader == false) {
                    memberInfor += '<div class="showInfomember" id="memberinfor">'
                        + '<div class="info" >'
                        + '<img src = "' + response.listInforStudentInGroupIdea[i].user.avatar + '" alt = "">'
                        + '<div class="memInfo">'
                        + '<a>' + response.listInforStudentInGroupIdea[i].user.fptEmail + '</a>'
                        + '<div class = "majorOfStu">'
                        + '<p>Profession : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].profession.professionFullName + '</span></p>'
                        + '<p>Specialty : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].specialty.specialtyFullName + '</span></p>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        + '<div class="role">'
                        + '<p>Member</p >'
                        + ' </div >'
                        + '</div>';
                }
            }
            $('.members').html(memberInfor);
        }).fail(function (error) {
            alert(error.statusText);
        });
        $('.showForm').toggle('hide-form');
    });

    $('body').on('click', '.closeFormBtn', function () {
        $('.showForm').toggle('hide-form');
    });
    $('body').on('click', '.showForm', function (e) {
        if (e.target === e.currentTarget) {
            $('.showForm').toggle('hide-form');
        }
    });
});

// finalGroup detail
$(document).ready(function () {
    $('body').on('click', '.finalGroup-popup', function () {
        var groupId = this.id;

        var finalGroup = {};
        finalGroup.finalGroupId = groupId;
        var class_name = '.finalGroup-' + groupId;
        finalGroup.groupName = $(class_name).val();

        $('div').remove('.popup')
        AjaxCall('/Group/GetDetailFinalGroup', JSON.stringify(finalGroup), 'GET').done(function (response) {
            var projectAva = '';
            for (var i = 0; i < response.listInforStudentInGroupIdea.length; i++) {
                if (response.listInforStudentInGroupIdea[i].isLeader == true) {
                    projectAva = response.listInforStudentInGroupIdea[i].user.avatar;
                    break;
                }
            }

            var nameProject =
                ' <img src="' + projectAva + '" alt = "Avatar" />'
                + '<div class="inforProject">'
                + '<h3>' + response.groupInfo.projectEnglishName + '</h3>'
                + '<p>Create at: <span>' + moment(response.groupInfo.createdAt).format('DD-MM-YYYY'); + '</span></p>'
                    + '</div >'
            $('.nameProject').html(nameProject);

            var professional = '<p class="title">Professional</p>'
                + '<p class="content">' + response.groupInfo.profession.professionFullName + '</p >'
            $('.professional').html(professional);

            var specialty = '<p class="title">Specialty</p>'
                + ' <p class="content">' + response.groupInfo.specialty.specialtyFullName + '</p>'
            $('.specialty').html(specialty);

            var abbreviations = '<p class="title">Abbreviations</p>'
                + '<p class="content">' + response.groupInfo.abbreviation + '</p>'
            $('.abbreviations').html(abbreviations);

            var vietnamTitle = ' <p class="title">Vietnamese Title</p>'
                + '<p class="content">' + response.groupInfo.projectVietNameseName + '</p> '
            $('.vietnamTitle').html(vietnamTitle);

            var description = ' <p class="title">Description</p>'
                + '<p class="content">' + response.groupInfo.description + '</p> '
            $('.description').html(description);

            //var basicInformSupervisor = '<div class="inputInformSupervisor">'
            //    + '<p> Full Name</p>'
            //    + '<p>' + response.groupInfo.supervisor.user.fullName + '</p>'
            //    + '</div>'
            //for (var idea of response.groupInfo.groupIdeaOfSupervisors) {
            //    basicInformSupervisor += '<div class="inputInformSupervisor">'
            //        + '<p>Idea</p>'
            //        + '<p>' + idea.projectEnglishName + '</p>'
            //        + '</div>'
            //}

            $('.supervisorInspect').html('');
            //$('.commentStudent').html('');

            var numMember = '<p>Total: <span>' + response.groupInfo.numberOfMember + ' members</span></p>'
            $('.numMember').html(numMember);

            var memberInfor = '';
            memberInfor += '<div class="showInfomember" id="memberinfor">'
                + '<div class="info" >'
                + '<img src = "' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).user.avatar + '" alt = "">'
                + '<div class="memInfo">'
                + '<a>' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).user.fptEmail + '</a>'
                + '<div class = "majorOfStu">'
                + '<p>Profession : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).profession.professionFullName + '</span></p>'
                + '<p>Specialty : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea.find(x => x.isLeader == true).specialty.specialtyFullName + '</span></p>'
                + '</div>'
                + '</div>'
                + '</div>'
                + '<div class="role">'
                + '<p>Leader</p >'
                + ' </div >'
                + '</div>';

            for (var i = 0; i < response.listInforStudentInGroupIdea.length; i++) {
                if (response.listInforStudentInGroupIdea[i].isLeader == false) {
                    memberInfor += '<div class="showInfomember" id="memberinfor">'
                        + '<div class="info" >'
                        + '<img src = "' + response.listInforStudentInGroupIdea[i].user.avatar + '" alt = "">'
                        + '<div class="memInfo">'
                        + '<a>' + response.listInforStudentInGroupIdea[i].user.fptEmail + '</a>'
                        + '<div class = "majorOfStu">'
                        + '<p>Profession : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].profession.professionFullName + '</span></p>'
                        + '<p>Specialty : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].specialty.specialtyFullName + '</span></p>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        + '<div class="role">'
                        + '<p>Member</p >'
                        + ' </div >'
                        + '</div>';
                }
            }
            $('.members').html(memberInfor);
        }).fail(function (error) {
            alert(error.statusText);
        });
        $('.showForm').toggle('hide-form');
    });
});

function saveFilterByStatus() {
    var statuses = [];

    $('input[name="statuses"]:checked').each(function () {
        statuses.push($(this).val());
    });

    const urlObject = new URL(window.location.href);
    urlObject.searchParams.set("page", 1);
    urlObject.searchParams.set("rawStatuses", statuses.join(','));
    window.location.href = urlObject.toString();
}

//ajax call
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: type,
//        contentType: "application/json; charset=utf-8",
//    });
//}

// confirm final report
function ConfirmFinalReport(event, status, groupId) {
    event.stopPropagation();
    var message = '';
    if (status == 0) message = 'cancel';
    else if (status == 1) message = 'confirm';
    else message = 'reject';

    Swal.fire({
        title: 'Are you sure?',
        text: `Are you sure you want to ${message} the request?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            var isConfirm;
            if (status == 0) isConfirm = null;
            else if (status == 1) isConfirm = true;
            else isConfirm = false;

            var finalGroup = {};
            finalGroup.IsConfirmFinalReport = isConfirm;
            finalGroup.FinalGroupID = groupId;

            AjaxCall('/Group/ConfirmFinalReport', JSON.stringify(finalGroup), 'POST').done(function (respone) {
                if (respone == 1) {
                    Swal.fire({
                        icon: 'success',
                        html: "<p class='popupTitle'>" + message + " successfully</p>",
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {
                        window.location.href = "/Group/Index";
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        html: "<p class='popupTitle'>" + message + " fail</p>",
                        timer: 1500,
                        showConfirmButton: false
                    });
                }
            }).fail(function (error) {
                alert(error.StatusText);
            });
        }
    });
}
