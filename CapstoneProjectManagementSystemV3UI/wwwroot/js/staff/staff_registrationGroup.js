
const requestMain = document.querySelector('.requestMain');
const acceptedMain = document.querySelector('.acceptedMain');
const rejectedMain = document.querySelector('.rejectedMain');
const requestPendingBtn = document.querySelector('.requestPendingBtn');
const requestAcceptedBtn = document.querySelector('.requestAcceptedBtn');
const requestRejectedBtn = document.querySelector('.requestRejectedBtn');
var globalStatus = 0;

requestPendingBtn.addEventListener('click', () => {
    requestMain.classList.remove('hide--list');
    rejectedMain.classList.add('hide--list');
    acceptedMain.classList.add('hide--list');
    requestPendingBtn.classList.add('active');
    requestAcceptedBtn.classList.remove('active');
    requestRejectedBtn.classList.remove('active');
})
requestAcceptedBtn.addEventListener('click', () => {
    requestMain.classList.add('hide--list');
    rejectedMain.classList.add('hide--list');
    acceptedMain.classList.remove('hide--list');
    requestPendingBtn.classList.remove('active');
    requestAcceptedBtn.classList.add('active');
    requestRejectedBtn.classList.remove('active');
})
requestRejectedBtn.addEventListener('click', () => {
    requestMain.classList.add('hide--list');
    rejectedMain.classList.remove('hide--list');
    acceptedMain.classList.add('hide--list');
    requestPendingBtn.classList.remove('active');
    requestAcceptedBtn.classList.remove('active');
    requestRejectedBtn.classList.add('active');
})

var addedStudents = [];
var deletedStudents = [];
var registeredGroupId = -1;

$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/realTimeHub").build();
    connection.start().then(function () {
    }).catch(function (err) {

    });
    connection.on("ReceiveMessageWithAdditionalData", function (user, message, updatedRegisterGroupId) {
        if (message == "updateMembers" && updatedRegisterGroupId == registeredGroupId && user == userId) {
            Swal.fire({
                title: 'Warning!!!',
                text: 'Department leader have updated members of this group',
                icon: 'warning',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            });
            AjaxCall('/RegistrationGroup/GetDetailRegistrationOfGroupStudent?registeredGroupId=' + registeredGroupId, "POST").done(function (response) {
                generateGroupDetail(response);
                deletedStudents = [];
                addedStudents = [];
            }).fail(function (error) {
                deletedStudents = [];
                addedStudents = [];
                alert(error.statusText);
            });
        }

    });
})

document.addEventListener("change", function (event) {
    var checkbox = event.target;
    if (checkbox.type === "checkbox" && checkbox.name === "unGroupedStudents") {
        var studentId = checkbox.value;
        if (checkbox.checked) {
            // Thêm sinh viên vào danh sách sinh viên đã chọn
            addedStudents.push(studentId);
        } else {
            // Xóa sinh viên khỏi danh sách sinh viên đã chọn
            var index = addedStudents.indexOf(studentId);
            if (index !== -1) {
                addedStudents.splice(index, 1);
            }
        }
        if (addedStudents.length != 0) {
            $("#addStudentsButton").html(`<button onclick='addMember(${registeredGroupId})'>Add ${addedStudents.length} ${addedStudents.length > 2 ? 'Students' : 'Student'}</button>`);
        } else {
            $("#addStudentsButton").html("");
        }
    } else if (checkbox.type === "checkbox" && checkbox.name === "groupedStudents") {
        var studentId = checkbox.value;
        if (checkbox.checked) {
            // Thêm sinh viên vào danh sách sinh viên đã chọn
            deletedStudents.push(studentId);
        } else {
            // Xóa sinh viên khỏi danh sách sinh viên đã chọn
            var index = deletedStudents.indexOf(studentId);
            if (index !== -1) {
                deletedStudents.splice(index, 1);
            }
        }
        if (deletedStudents.length != 0) {
            $("#deleteStudentsButton").html(`<button onclick='deleteMember(${registeredGroupId})'>Delete ${deletedStudents.length} ${deletedStudents.length > 2 ? 'Students' : 'Student'}</button>`);
        } else {
            $("#deleteStudentsButton").html("");
        }
    };
});
var globalNumberMember = 0;

function addMember(registerGroupId) {
    var addedStudentIds = addedStudents.join(",");
    AjaxCall(`/RegistrationGroup/AddMember?addedStudentIds=${addedStudentIds}&registeredGroupId=${registerGroupId}`, "", "post").done(function (status) {
        Swal.fire({
            title: `The students have been added !`,
            text: `The ${addedStudents.length} students have been added !`,
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
        AjaxCall('/RegistrationGroup/GetDetailRegistrationOfGroupStudent?registeredGroupId=' + registeredGroupId, "POST").done(function (response) {
            generateGroupDetail(response);
            addedStudents = [];
        }).fail(function (error) {
            alert(error.statusText);
        });
    }).fail(function () {
        Swal.fire({
            title: 'Error!',
            text: 'Something went wrong.',
            icon: 'error',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
        addedStudents = [];
    });
}

function deleteGroup(registerGroupId) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Do you want to delete this group?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete group'
    }).then((result) => {
        if (result.isConfirmed) {
            AjaxCall(`/RegistrationGroup/DeleteRegisteredGroup?registeredGroupId=${registeredGroupId}`, "", "post").done(function (status) {
                $('.showForm').toggle('hide-form');
                if (status == 1) {
                    GetListRegisteredGroup(1, globalSearchText, globalPagingType, globalRecordNumber);
                    Swal.fire({
                        title: `Delete Group`,
                        text: `Delete Group Success !`,
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                } else {
                    Swal.fire({
                        title: `Delete Group`,
                        text: `Delete Group Fail !`,
                        icon: 'error',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                }
            });
        }
    });
}

function deleteMember(registerGroupId) {

    if (globalNumberMember == deletedStudents.length) {
        Swal.fire({
            title: 'Are you sure?',
            text: 'You can not delete all students of group.You would rather to delete group?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete group'
        }).then((result) => {
            if (result.isConfirmed) {
                AjaxCall(`/RegistrationGroup/DeleteRegisteredGroup?registeredGroupId=${registerGroupId}`, "", "post").done(function (status) {
                    if (status == 1) {
                        $('.showForm').toggle('hide-form');
                        GetListRegisteredGroup(1, globalSearchText, globalPagingType, globalRecordNumber);
                    }
                });
            }
        });
        return;
    }
    var deletedStudentIds = deletedStudents.join(",");
    AjaxCall(`/RegistrationGroup/DeleteMember?deletedStudentIds=${deletedStudentIds}&registeredGroupId=${registerGroupId}`, "", "post").done(function (status) {
        Swal.fire({
            title: `The students have been deleted !`,
            text: `The ${deletedStudents.length} students have been deleted !`,
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
        AjaxCall('/RegistrationGroup/GetDetailRegistrationOfGroupStudent?registeredGroupId=' + registeredGroupId, "POST").done(function (response) {
            generateGroupDetail(response);
            deletedStudents = [];
        }).fail(function (error) {
            deletedStudents = [];
            alert(error.statusText);
        });
    }).fail(function () {
        Swal.fire({
            title: 'Error!',
            text: 'Something went wrong.',
            icon: 'error',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
    });
}
function generateGroupDetail(response) {
    console.log("register response: ",response)
    $('.nameProject').html('');
    $('.professional').html('');
    $('.specialty').html('');
    $('.abbreviations').html('');
    $('.vietnamTitle').html('');
    $('#basicInformSupervisor1').html('');
    $('#basicInformSupervisor2').html('');
    $('.commentStudent').html('');
    $('.numMember').html('');
    $('#memberinfor').html('');
    $('.members').html('');
    $('.description').html('');
    $("div").remove("#memberinfor");
    var nameProject =
        '<div class="inforProject">'
        + '<h3>' + response.detailRegistration.groupIdea.projectEnglishName + '</h3>'
        + '<p>Create at: <span>' + moment(response.detailRegistration.createdAt).format('DD-MM-YYYY') + '</span></p>'
        + (response.detailRegistration.status == 1 ? `<button onclick="deleteGroup(${registeredGroupId})">Delete Group</button>` : '')
        + '</div>';

    $('.nameProject').append(nameProject);
    var professional = '<p class="title">Professional</p>'
        + '<p class="content">' + response.detailRegistration.groupIdea.profession.professionFullName + '</p >'
    $('.professional').append(professional);

    var specialty = '<p class="title">Specialty</p>'
        + ' <p class="content">' + response.detailRegistration.groupIdea.specialty.specialtyFullName + '</p>'
    $('.specialty').append(specialty);

    var abbreviations = '<p class="title">Abbreviations</p>'
        + '<p class="content">' + response.detailRegistration.groupIdea.abrrevation + '</p>';
    $('.abbreviations').append(abbreviations);

    var description = '<p class="title">Description</p>'
        + '<p class="content">' + response.detailRegistration.groupIdea.description + '</p>';
    $('.description').append(description);

    var vietnamTitle = ' <p class="title">Vietnamese Title</p>'
        + '<p class="content">' + response.detailRegistration.groupIdea.projectVietNameseName + '</p> '
    $('.vietnamTitle').append(vietnamTitle);
    for (var j = 0; j < response.detailRegistration.registerdGroupSupervisors.length; j++) {
        if (response.detailRegistration.registerdGroupSupervisors[j].supervisor.user.fullName != ""
            || response.detailRegistration.registerdGroupSupervisors[j].supervisor.phoneNumber != ""
            || response.detailRegistration.registerdGroupSupervisors[j].supervisor.user.fptEmail != "") {
            var basicInformSupervisor1 = '<div class="inputInformSupervisor">'
                + '<p> Full Name</p>'
                + '<p>' + response.detailRegistration.registerdGroupSupervisors[j].supervisor.user.fullName + '</p>'
                + '</div >'
                + '<div class="inputInformSupervisor">'
                + '<p>Phone Number</p>'
                + '<p>' + response.detailRegistration.registerdGroupSupervisors[j].supervisor.phoneNumber + '</p>'
                + '</div>'
                + '<div class="inputInformSupervisor">'
                + '<p>Email</p>'
                + '<p>' + response.detailRegistration.registerdGroupSupervisors[j].supervisor.user.fptEmail + '</p>'
                + '</div>'
            $('#basicInformSupervisor' + [j + 1]).append(basicInformSupervisor1);
        } else {
            $('#basicInformSupervisor1' + [j + 1]).html('');
        }
    }

    if (response.detailRegistration.studentComment != "") {
        var commentStudent = '<p class="title">Student Comment</p>'
            + '<p>' + response.detailRegistration.studentComment + '</p>'
        $('.commentStudent').append(commentStudent);
    } else {
        $('.commentStudent').html('');
    }
    var numMember = '<p>Total: <span>' + response.detailRegistration.groupIdea.numberOfMember + ' members</span></p>';
    globalNumberMember = response.detailRegistration.groupIdea.numberOfMember;
    $('.numMember').append(numMember);

    var memberInfor = '';


    $htmlInfoNotUpdate = '';
    if (response.listInforStudentInGroupIdea.length < response.detailRegistration.groupIdea.numberOfMember - 1) {
        $htmlInfoNotUpdate = '<p><i>Groups with members who do not update enough personal information ';
        try {
            if (response?.memberLackInfor) {
                response?.memberLackInfor.forEach((fptEmail) => {
                    $htmlInfoNotUpdate += `<b>${fptEmail} </b>`;
                });
            }

        } catch (e) {
            console.log(e);
        }


        $htmlInfoNotUpdate += '</i></p>';
    }

    memberInfor += $htmlInfoNotUpdate;


    var leader = response.leader;
    if (leader) {
        memberInfor += '<div class="showInfomember" id="memberinfor">'
            + '<div class="info" >'
            + '<img src = "' + leader?.user.avatar + '" alt = "">'
            + '<div class="memInfo">'
            + '<a>' + leader?.user.fptEmail + '</a>'
            + '<div class = "majorOfStu">'
            + '<p>Profession : <span style="color:black;font-weight: bold;">' + leader?.profession.professionFullName + '</span></p>'
            + '<p>Specialty : <span style="color:black;font-weight: bold;">' + leader?.specialty.specialtyFullName + '</span></p>'
            + '</div>'
            + '</div>'
            + '</div>'
            + '<div class="role">'
            + '<p>Leader</p >'
            + ' </div >'
            + `${response.detailRegistration.status == 1 ? `<input type='checkbox' style='margin-top: 20px;' name='groupedStudents' value='${leader?.studentID}' /> ` : ''} `
            + '</div>';
    } else {
        memberInfor += 'Leader : None';
    }
    
    for (var i = 0; i < response.listInforStudentInGroupIdea.length; i++) {
        memberInfor += '<div class="showInfomember" id="memberinfor">'
            + '<div class="info" >'
            + '<img src = "' + response.listInforStudentInGroupIdea[i]?.user?.avatar + '" alt = "">'
            + '<div class="memInfo">'
            + '<a>' + response.listInforStudentInGroupIdea[i]?.user.fptEmail + '</a>'
            + '<div class = "majorOfStu">'
            + '<p>Profession : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].profession.professionFullName + '</span></p>'
            + '<p>Specialty : <span style="color:black;font-weight: bold;">' + response.listInforStudentInGroupIdea[i].specialty.specialtyFullName + '</span></p>'
            + '</div>'
            + '</div>'
            + '</div>'
            + '<div class="role">'
            + '<p>member</p >'
            + ' </div >'
            + `${response.detailRegistration.status == 1 ? `<input type='checkbox' name='groupedStudents' value='${response.listInforStudentInGroupIdea[i].studentID}' /> ` : ''} `
            + '</div>';
    }
    $('.members').append(memberInfor);
    if (response.detailRegistration.status == 1) {
        $('.addDeleteButtons').html('<span id="deleteStudentsButton"></span> <span id = "addStudentsButton" ></span > ');
    } else {
        $('.addDeleteButtons').html("");
    }
    if (response.detailRegistration.status == 1) {
        // Clear previous content
        var studentList = '';
        // Generate student list items
        var students = response.ungroupedStudents;
        students?.forEach((student) => {
            var listItem = '<li>' +
                '<div class="listForm">' +
                '<div><strong>Fpt Email: </strong> ' + student.user.fptEmail + '</div>' +
                '<div><strong>Profession:</strong> ' + student.profession.professionFullName + '</div>' +
                '</div>' +
                '<div class="listForm">' +
                '<div><strong>Speciality:</strong> ' + student.specialty.specialtyFullName + '</div>' +
                '<div><strong>Want To Be Grouped: </strong>' + `${student.wantToBeGrouped ? '<span style="color : green;"> yes </span>' : '<span style="color : red;"> no </span>'}` + '</div>' +
                `<div> <input type='checkbox' name='unGroupedStudents' value='${student.studentID}' /> </div>` +
                '<div>' +
                '</li>';
            studentList += listItem;
        });
        $('.listUngroupedStudentsForAdd').html(studentList);
    }
}

$('body').on('click', '.displayForm,.displayAcceptedForm,.displayRejectedForm', function () {
    registeredGroupId = $(this).attr('id');
    AjaxCall('/RegistrationGroup/GetDetailRegistrationOfGroupStudent?registeredGroupId=' + registeredGroupId, "POST").done(function (response) {
        generateGroupDetail(response);
    }).fail(function (error) {
        alert(error.statusText);
    });
    $('.showForm').toggle('hide-form');
})


$('body').on('click', '.closeFormBtn', function () {
    $('.showForm').toggle('hide-form');
});
$('body').on('click', '.showForm', function (e) {
    if (e.target === e.currentTarget) {
        $('.showForm').toggle('hide-form');
    }
});



//////////////////////////////////////////////////////////////////////////////////


// load list request registration group in filter request pending 
$(document).ready(function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListRegisteredGroup(0, searchText, 'none', 0);
    }
})

$('#requestPendingBtn,#requestAcceptedBtn,#requestRejectedBtn,#searchNameRequestInput').on('click load change', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        GetListRegisteredGroup(0, searchText, 'none', 0);
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        GetListRegisteredGroup(1, searchText, 'none', 0);
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        GetListRegisteredGroup(2, searchText, 'none', 0);
    }
});


var startNum;
var numberOfRecordsPerPage;
var countResult;
var endNum;

/*previous page*/
$('body').on('click', '#previousBtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListRegisteredGroup(0, searchText, 'previous', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListRegisteredGroup(1, searchText, 'previous', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if (!(startNum == 1)) {
            GetListRegisteredGroup(2, searchText, 'previous', startNum);
        }
    }
});

/*next page*/
$('body').on('click', '#nextbtn', function () {
    var searchText = $('#searchNameRequestInput').val();
    if ($('#requestPendingBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListRegisteredGroup(0, searchText, 'next', startNum);
        }
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListRegisteredGroup(1, searchText, 'next', startNum);
        }
    }
    if ($('#requestRejectedBtn').hasClass('active')) {
        if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
            GetListRegisteredGroup(2, searchText, 'next', startNum);
        }
    }
});

var globalSearchText = "";
var globalPagingType;
var globalRecordNumber;
//get list registered group
function GetListRegisteredGroup(status, searchText, pagingType, recordNumber) {
    globalStatus = status;
    globalSearchText = searchText;
    globalPagingType = pagingType;
    globalRecordNumber = recordNumber;
    $('.informRequest').remove();
    AjaxCall('/RegistrationGroup/GetListRegisteredGroup?status=' + status + '&&searchText=' + searchText + '&&pagingType=' + pagingType + '&&recordNumber=' + recordNumber, "POST").done(function (response) {
        var informRequest = '';
        var count = 1;
        if (response.registeredGroups != null) {
            if (status == 0) {
                $('.requestMain').html('');
                $('.requestMain').append('<div class="titleRequest titleRequest2">'
                    + '<div class= "formTitle">'
                    + '<p>No.</p>'
                    + '<p>Name Capstone</p>'
                    + '<p>Major</p>'
                    + '<p>Supervisor</p>'
                    + '</div>'
                    + '<p>Action'
                    + '<svg width="14" height="10" viewBox="0 0 14 14" fill="none" xmlns="http://www.w3.org/2000/svg">'
                    + '<path d="M6.99967 0.333008C3.31967 0.333008 0.333008 3.31967 0.333008 6.99967C0.333008 10.6797 3.31967 13.6663 6.99967 13.6663C10.6797 13.6663 13.6663 10.6797 13.6663 6.99967C13.6663 3.31967 10.6797 0.333008 6.99967 0.333008ZM7.66634 10.333H6.33301V6.33301H7.66634V10.333ZM7.66634 4.99967H6.33301V3.66634H7.66634V4.99967Z"'
                    + 'fill="#1C1F27" fill-opacity="0.3" />'
                    + '</svg>'
                    + '</p>'
                    + '</div>');
                for (var i = 0; i < response.registeredGroups.length; i++) {
                    var supervisors = '';
                    for (var j = 0; j < response.registeredGroups[i].registerdGroupSupervisors.length; j++) {
                        supervisors += '<p>' + response.registeredGroups[i].registerdGroupSupervisors[j].supervisor.user.fullName + '</p>';
                    }

                    informRequest += '<div class="informRequest informRequest2">'
                        + '<div class="displayForm" id="' + response.registeredGroups[i].registeredGroupID + '">'
                        + '<p>' + count + ' </p>'
                        + '<p class="nameProjectRequest limitCharacter">' + response.registeredGroups[i].groupIdea.projectEnglishName + '</p>'
                        + '<p>' + response.registeredGroups[i].groupIdea.specialty.specialtyFullName + '</p>'
                        + '<div class="supervisor">'
                        + supervisors
                        + '</div>'
                        + '</div>'
                        + '<div class="buttonAccept">'
                        + '<button class="acceptBtn" value="' + response.registeredGroups[i].registeredGroupID + '">Accept</button>'
                        + '<button class="rejectBtn" value="' + response.registeredGroups[i].registeredGroupID + '">Reject</button>'
                        + '</div>'
                        + '</div>'
                    count++;
                }
                $('.requestMain').append(informRequest);
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.requestMain').append('<div class="requestMain">'
                    + '<div class= "pagination">'
                    + '<a href="#" id="previousBtn"><i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a href="#" id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>'
                    + '</div>')
            }
            if (status == 1) {
                $('.acceptedMain').html('');
                $('.acceptedMain').append('<div class="titleAcceptedRequest">'
                    + '<div class= "formAcceptedTitle">'
                    + '<p>No.</p>'
                    + '<p>Name Capstone</p>'
                    + '<p>Major</p>'
                    + '<p>Supervisor</p>'
                    + '</div>'
                    + '<p class="status">Status</p>'
                    + '<p>Action</p>'
                    + '</div>');
                for (var i = 0; i < response.registeredGroups.length; i++) {
                    var supervisors = '';
                    for (var j = 0; j < response.registeredGroups[i].registerdGroupSupervisors.length; j++) {
                        supervisors += '<p>' + response.registeredGroups[i].registerdGroupSupervisors[j].supervisor.user.fullName + '</p>';
                    }
                    informRequest += '<div class="informRequest">'
                        + '<div class="displayAcceptedForm" id="' + response.registeredGroups[i].registeredGroupID + '">'
                        + '<p>' + count + '</p>'
                        + '<p class="nameProjectRequest limitCharacter">' + response.registeredGroups[i].groupIdea.projectEnglishName + '</p>'
                        + '<p>' + response.registeredGroups[i].groupIdea.specialty.specialtyFullName + '</p>'
                        + '<div class="supervisor">'
                        + supervisors
                        + '</div>'
                        + '</div>'
                        + '<p class="acceptStatus">Accepted</p>'
                        + '<button class="reject" value="' + response.registeredGroups[i].registeredGroupID + '">Reject</button>'
                        + '</div>'
                    count++;
                }
                $('.acceptedMain').append(informRequest);
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.acceptedMain').append('<div class="acceptedMain">'
                    + '<div class= "pagination">'
                    + '<a href="#" id="previousBtn"><i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a href="#" id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>'
                    + '</div>')
            }
            if (status == 2) {
                $('.rejectedMain').html('');
                $('.rejectedMain').append('<div class="titleRejectedRequest">'
                    + '<div class= "formRejectedTitle">'
                    + '<p>No.</p>'
                    + '<p>Name Capstone</p>'
                    + '<p>Major</p>'
                    + '<p>Supervisor</p>'
                    + '</div >'
                    + '<p class="status">Status</p>'
                    + '<p>Reason</p>'
                    + '</div>');
                for (var i = 0; i < response.registeredGroups.length; i++) {
                    var supervisors = '';
                    for (var j = 0; j < response.registeredGroups[i].registerdGroupSupervisors.length; j++) {
                        supervisors += '<p>' + response.registeredGroups[i].registerdGroupSupervisors[j].supervisor.user.fullName + '</p>';
                    }
                    informRequest += '<div class="informRequest">'
                        + ' <div class="displayRejectedForm" id="' + response.registeredGroups[i].registeredGroupID + '">'
                        + '<p>' + count + '</p>'
                        + '<p class="nameProjectRequest limitCharacter">' + response.registeredGroups[i].groupIdea.projectEnglishName + '</p>'
                        + '<p>' + response.registeredGroups[i].groupIdea.specialty.specialtyFullName + '</p>'
                        + '<div class="supervisor">'
                        + supervisors
                        + '</div>'
                        + '</div>'
                        + '<p class="rejectStatus">Rejected</p>'
                        + '<p class="reason">' + response.registeredGroups[i].staffComment + '</p>'
                        + '</div>'
                    count++;
                }
                $('.rejectedMain').append(informRequest);
                startNum = response.startNum;
                numberOfRecordsPerPage = response.numberOfRecordsPerPage;
                countResult = response.countResult
                endNum = (startNum + numberOfRecordsPerPage - 1) > countResult ? countResult : startNum + numberOfRecordsPerPage - 1;
                $('.rejectedMain').append('<div class="rejectedMain">'
                    + '<div class= "pagination">'
                    + '<a href="#" id="previousBtn"><i class="fa-solid fa-angle-left"></i></a>'
                    + '<div class="numPage">'
                    + '<p class="number" id="pagingCount">' + startNum + '-' + endNum + ' in ' + countResult + ' results</p>'
                    + '</div>'
                    + '<a href="#" id="nextbtn"><i class="fa-solid fa-angle-right"></i></a>'
                    + '</div>'
                    + '</div>')
            }
        } else {
            if (status == 0) {
                $('.requestMain').html('');
                $('.requestMain').append("<h2 style='color:red'>You don't have any requests yet.</h2>");
            }
            if (status == 1) {
                $('.acceptedMain').html('');
                $('.acceptedMain').append("<h2 style='color:red'>You don't have any requests yet.</h2>");
            }
            if (status == 2) {
                $('.rejectedMain').html('');
                $('.rejectedMain').append("<h2 style='color:red'>You don't have any requests yet.</h2>");
            }
        }
    }).fail(function (error) {
        alert(error.statusText);
    });
}


//accept in request pending
$(document).ready(function () {
    var registeredGroupId;
    var acceptBtn;
    var rejectBtn;
    $('body').on('click', '.acceptBtn', function (e) {
        $('.showFormConfirm').toggle('hide-form');
        acceptBtn = $('.acceptBtn');
        acceptBtn.attr('disabled', 'disabled');
        acceptBtn.css('cursor', 'not-allowed');
        acceptBtn.css('opacity', '0.5');

        rejectBtn = $('.rejectBtn');
        rejectBtn.attr('disabled', 'disabled');
        rejectBtn.css('cursor', 'not-allowed');
        rejectBtn.css('opacity', '0.5');

        registeredGroupId = $(this).val();
        $('body').on('click', '#submitBtn', function (p) {
            AjaxCall('/RegistrationGroup/AcceptRegisteredGroup?registeredGroupID=' + registeredGroupId, "POST").done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Accepted Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            if ($('#requestPendingBtn').hasClass('active')) {
                                GetListRegisteredGroup(0, searchText, 'none', 0);
                            }
                        })
                        location.reload(true);
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });;
            });
            $('.showFormConfirm').toggle('hide-form');
            e.stopPropogation();
        });
    });
    $('body').on('click', '.discardBtn', function () {
        acceptBtn.removeAttr('disabled', 'disabled');
        acceptBtn.css('cursor', 'pointer');
        acceptBtn.css('opacity', '1');

        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');
        $('.showFormConfirm').toggle('hide-form');
    });
    $('body').on('click', '.showFormConfirm', function (e) {
        if (e.target === e.currentTarget) {
            acceptBtn.removeAttr('disabled', 'disabled');
            acceptBtn.css('cursor', 'pointer');
            acceptBtn.css('opacity', '1');

            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');
            $('.showFormConfirm').toggle('hide-form');
        }
    });
})



$('body').on('click', '.rejectBtn,.reject', function () {
    var rejectBtn = $('.rejectBtn');
    rejectBtn.attr('disabled', 'disabled');
    rejectBtn.css('cursor', 'not-allowed');
    rejectBtn.css('opacity', '0.5');

    var acceptBtn = $('.acceptBtn');
    acceptBtn.attr('disabled', 'disabled');
    acceptBtn.css('cursor', 'not-allowed');
    acceptBtn.css('opacity', '0.5');

    var rejectInAcceptedRequestBtn = $('.reject');
    rejectInAcceptedRequestBtn.attr('disabled', 'disabled');
    rejectInAcceptedRequestBtn.css('cursor', 'not-allowed');
    rejectInAcceptedRequestBtn.css('opacity', '0.5');
    $('.commentReject').toggle('hide-form');
})

$('body').on('click', '.closeBtn', function () {
    if ($('#requestPendingBtn').hasClass('active')) {
        var rejectBtn = $('.rejectBtn');
        rejectBtn.removeAttr('disabled', 'disabled');
        rejectBtn.css('cursor', 'pointer');
        rejectBtn.css('opacity', '1');

        var acceptBtn = $('.acceptBtn');
        acceptBtn.removeAttr('disabled', 'disabled');
        acceptBtn.css('cursor', 'pointer');
        acceptBtn.css('opacity', '1');

        var rejectInAcceptedRequestBtn = $('.reject');
        rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
        rejectInAcceptedRequestBtn.css('cursor', 'pointer');
        rejectInAcceptedRequestBtn.css('opacity', '1');
        $('.commentReject').toggle('hide-form');
    }
    if ($('#requestAcceptedBtn').hasClass('active')) {
        var rejectInAcceptedRequestBtn = $('.reject');
        rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
        rejectInAcceptedRequestBtn.css('cursor', 'pointer');
        rejectInAcceptedRequestBtn.css('opacity', '1');

        var acceptBtn = $('.acceptBtn');
        acceptBtn.removeAttr('disabled', 'disabled');
        acceptBtn.css('cursor', 'pointer');
        acceptBtn.css('opacity', '1');

        var rejectInAcceptedRequestBtn = $('.reject');
        rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
        rejectInAcceptedRequestBtn.css('cursor', 'pointer');
        rejectInAcceptedRequestBtn.css('opacity', '1');
        $('.commentReject').toggle('hide-form');
    }
});

$('body').on('click', '.commentReject', function (e) {
    if (e.target === e.currentTarget) {
        if ($('#requestPendingBtn').hasClass('active')) {
            var rejectBtn = $('.rejectBtn');
            rejectBtn.removeAttr('disabled', 'disabled');
            rejectBtn.css('cursor', 'pointer');
            rejectBtn.css('opacity', '1');

            var acceptBtn = $('.acceptBtn');
            acceptBtn.removeAttr('disabled', 'disabled');
            acceptBtn.css('cursor', 'pointer');
            acceptBtn.css('opacity', '1');

            var rejectInAcceptedRequestBtn = $('.reject');
            rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
            rejectInAcceptedRequestBtn.css('cursor', 'pointer');
            rejectInAcceptedRequestBtn.css('opacity', '1');
            $('.commentReject').toggle('hide-form');
        }
        if ($('#requestAcceptedBtn').hasClass('active')) {
            var rejectInAcceptedRequestBtn = $('.reject');
            rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
            rejectInAcceptedRequestBtn.css('cursor', 'pointer');
            rejectInAcceptedRequestBtn.css('opacity', '1');

            var acceptBtn = $('.acceptBtn');
            acceptBtn.removeAttr('disabled', 'disabled');
            acceptBtn.css('cursor', 'pointer');
            acceptBtn.css('opacity', '1');

            var rejectInAcceptedRequestBtn = $('.reject');
            rejectInAcceptedRequestBtn.removeAttr('disabled', 'disabled');
            rejectInAcceptedRequestBtn.css('cursor', 'pointer');
            rejectInAcceptedRequestBtn.css('opacity', '1');
            $('.commentReject').toggle('hide-form');
        }
    }
});


////reject in request pending
$(document).ready(function () {
    var registeredGroupId;
    $('body').on('click', '.rejectBtn', function (e) {
        registeredGroupId = $(this).val();
        $('.commentReject').html('');
        $('.commentReject').append('<div class="popup">'
            + '<p>Give to reasons for reject this request to change the topic?</p>'
            + '<textarea name="" id="textAreaComent" cols="60" rows="8" placeholder="Write your reason here..."></textarea>'
            + '<p class="showErrorMessage" id="showErrorMessageCommentReject"></p>'
            + '<div class="button">'
            + '<button class="submitBtn" id="reject1Btn" disabled>Submit</button>'
            + '<button class="closeBtn">Close</button>'
            + '</div>'
            + '</div>');
        $('body').on('click', '#reject1Btn', function (p) {
            var staffComment = $('#textAreaComent').val();
            AjaxCall('/RegistrationGroup/RejectRegisteredGrop?staffComment=' + staffComment + '&&registeredGroupId=' + registeredGroupId, "POST").done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Rejected Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            if ($('#requestPendingBtn').hasClass('active')) {
                                GetListRegisteredGroup(0, searchText, 'none', 0);
                            }
                        });
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });
            });
            $('.commentReject').toggle('hide-form');
            e.stopPropogation();
        })
    })
})

////reject in request accepted 
$(document).ready(function () {
    var registeredGroupId;
    registeredGroupId = $(this).val();

    $('body').on('click', '.reject', function (e) {
        $('.commentReject').html('');
        $('.commentReject').append('<div class="popup">'
            + '<p>Give to reasons for reject this request to change the topic?</p>'
            + '<textarea name="" id="textAreaComent" cols="60" rows="10" placeholder="Write your reason here..."></textarea>'
            + '<p class="showErrorMessage" id="showErrorMessageCommentReject"></p>'
            + '<div class="button">'
            + '<button class="submitBtn" id="reject2Btn" disabled>Submit</button>'
            + '<button class="closeBtn">Close</button>'
            + '</div>'
            + '</div>');
        registeredGroupId = $(this).val();
        $('body').on('click', '#reject2Btn', function (p) {
            var commentReject = $('#textAreaComent').val();
            AjaxCall('/RegistrationGroup/RejectWhenAccepted?registeredGroupID=' + registeredGroupId + '&&commentReject=' + commentReject, "POST").done(function (response) {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        title: '<p class="popupTitle">Rejected Successfully</p>'
                    }).then(function () {
                        $(document).ready(function () {
                            var searchText = $('#searchNameRequestInput').val();
                            $('#requestAcceptedBtn').addClass('active')
                            GetListRegisteredGroup(1, searchText, 'none', 0);
                        });
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                    }).then(function () {
                        location.reload(true);
                    });
                }
            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
                }).then(function () {
                    location.reload(true);
                });
            });
            $('.commentReject').toggle('hide-form');
            e.stopPropogation();
        });
    });
})


$(document).ready(function () {
    //validate commentReject
    $('body').on('blur', '#textAreaComent', function () {
        const commentReject = $('#textAreaComent').val();
        if (commentReject.length == 0) {
            $('#showErrorMessageCommentReject').html('This field is required')
        } else if (commentReject.length > 400) {
            $('#showErrorMessageCommentReject').html('Input less than 400 characters');
        } else {
            $('#showErrorMessageCommentReject').html('');
        }
    })
    // disable button submit
    $('body').on('blur change keyup', '#textAreaComent', function () {
        const commentReject = $('#textAreaComent').val();
        if (commentReject.length == 0) {
            $('#reject1Btn').attr('disabled', 'disabled');
            $('#reject1Btn').css('cursor', 'not-allowed');
            $('#reject1Btn').css('opacity', '0.5');

            $('#reject2Btn').attr('disabled', 'disabled');
            $('#reject2Btn').css('cursor', 'not-allowed');
            $('#reject2Btn').css('opacity', '0.5');
        } else if (commentReject.length > 400) {
            $('#reject1Btn').attr('disabled', 'disabled');
            $('#reject1Btn').css('cursor', 'not-allowed');
            $('#reject1Btn').css('opacity', '0.5');

            $('#reject2Btn').attr('disabled', 'disabled');
            $('#reject2Btn').css('cursor', 'not-allowed');
            $('#reject2Btn').css('opacity', '0.5');
        } else {
            $('#reject1Btn').removeAttr('disabled', 'disabled');
            $('#reject1Btn').css('cursor', 'pointer');
            $('#reject1Btn').css('opacity', '1');

            $('#reject2Btn').removeAttr('disabled', 'disabled');
            $('#reject2Btn').css('cursor', 'pointer');
            $('#reject2Btn').css('opacity', '1');
        }
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

// phase 2
$('.btnImport').click(function () {
    $('.showImportForm').toggle('hide-form');
});

$('.noBtnImport').click(function () {
    $('.showImportForm').toggle('hide-form');
});

$('.showImportForm').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.showImportForm').toggle('hide-form');
    }
});

$('.getTemplate').click(function () {
    $('#formExport').submit();
});

$('#btnImportCheckCondition').click(function () {
    if (document.getElementById("fileCheckCondition").files.length === 0) {
        Swal.fire({
            icon: 'error',
            title: '<p class="popupTitle">You have not selected any files yet.</p>'
        })
    } else {
        $('#btnImportCheckCondition').attr('disabled', 'disabled');
        $('#btnImportCheckCondition').css('cursor', 'not-allowed');
        $('#btnImportCheckCondition').css('opacity', '0.5');
        $('#formImportCheckCondition').submit();
    }
});