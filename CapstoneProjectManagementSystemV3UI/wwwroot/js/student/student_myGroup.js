$('.btnChangeTopic').click(function () {
    window.location.href = '/ChangeTopic/Index';
})

$('.btnChangeFinalGroup').click(function () {
    window.location.href = '/ChangeFinalGroup/Index';
})
const menuMember = document.querySelectorAll('.menuMember');
menuMember.forEach(dropdown => {
    const showMenuBtn = dropdown.querySelector('.showMenu');
    const dropdownMenu = dropdown.querySelector('.menuMember__dropdown');

    showMenuBtn.addEventListener('click', () => {
        dropdownMenu.classList.toggle('show--menuMember__dropdown');
    });

    document.addEventListener('click', function handleClickOutsideBox(event) {

        if (!showMenuBtn.contains(event.target)) {
            dropdownMenu.classList.remove('show--menuMember__dropdown');
        }
    });
})

/*confirm popup for accept request*/
$(document).ready(function () {
    var userId;
    $('.acceptButton').click(function () {
        userId = this.id;
        $('.showAcceptForm').toggle('hide-Acceptform');
    });
    $('.noBtnAccept').click(function () {
        $('.showAcceptForm').toggle('hide-Acceptform');
    });
    $('.showAcceptForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showAcceptForm').toggle('hide-Acceptform');
        }
    });
    $('.yesBtnAccept').click(function () {
        var form = document.getElementById("form1 " + userId);
        form.submit();
    });
});

/*confirm popup for reject request*/
$(document).ready(function () {
    var userId;
    $('.rejectButton').click(function () {
        userId = this.id;
        $('.showRejectForm').toggle('hide-Rejectform');
    });
    $('.noBtnReject').click(function () {
        $('.showRejectForm').toggle('hide-Rejectform');
    });
    $('.showRejectForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showRejectForm').toggle('hide-Rejectform');
        }
    });
    $('.yesBtnReject').click(function () {
        var form = document.getElementById("form2 " + userId);
        form.submit();
    });
});


/*confirm popup for change leader*/
$(document).ready(function () {
    var userId;
    $('.changeLead').click(function () {
        userId = this.id;
        $('.showChangeLeadForm').toggle('hide-ChangeLeadform');
    });
    $('.noBtnChangeLead').click(function () {
        $('.showChangeLeadForm').toggle('hide-ChangeLeadform');
    });
    $('.showChangeLeadForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showChangeLeadForm').toggle('hide-ChangeLeadform');
        }
    });
    $('.yesBtnChangeLead').click(function () {
        var form = document.getElementById("form3 " + userId);
        form.submit();
    });
});

/*confirm popup for remove member*/
$(document).ready(function () {
    var userId;
    $('.removeMem').click(function () {
        userId = this.id;
        $('.showRemoveMemForm').toggle('hide-RemoveMemform');
    });
    $('.noBtnRemoveMem').click(function () {
        $('.showRemoveMemForm').toggle('hide-RemoveMemform');
    });
    $('.showRemoveMemForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showRemoveMemForm').toggle('hide-RemoveMemform');
        }
    });
    $('.yesBtnRemoveMem').click(function () {
        var form = document.getElementById("form4 " + userId);
        form.submit();
    });
});

/*confirm popup for delete group*/
$(document).ready(function () {
    $('.deleteGroup').click(function () {
        $('.showDeleteGroupForm').toggle('hide-DeleteGroupform');
    });
    $('.noBtnDeleteGroup').click(function () {
        $('.showDeleteGroupForm').toggle('hide-DeleteGroupform');
    });
    $('.showDeleteGroupForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showDeleteGroupForm').toggle('hide-DeleteGroupform');
        }
    });
    $('.yesBtnDeleteGroup').click(function () {
        $('.yesBtnDeleteGroup').attr('disabled', 'disabled');
        $('.yesBtnDeleteGroup').css('cursor', 'not-allowed');
        $('.yesBtnDeleteGroup').css('opacity', '0.5');
        var form = document.getElementById("formDeleteGroup");
        form.submit();
    });
});
/*confirm popup for leave group*/
$(document).ready(function () {
    $('.leaveGroup').click(function () {
        $('.showLeaveGroupForm').toggle('hide-LeaveGroupform');
    });
    $('.noBtnLeaveGroup').click(function () {
        $('.showLeaveGroupForm').toggle('hide-LeaveGroupform');
    });
    $('.showLeaveGroupForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showLeaveGroupForm').toggle('hide-LeaveGroupform');
        }
    });
    $('.yesBtnLeaveGroup').click(function () {
        var form = document.getElementById("formLeaveGroup");
        form.submit();
    });
});
/*confirm popup for cancel registration request*/
$(document).ready(function () {
    $('.cancelRegistration').click(function () {
        $('.showCancelRegisForm').toggle('hide-CancelRegisform');
    });
    $('.noBtnCancelRegis').click(function () {
        $('.showCancelRegisForm').toggle('hide-CancelRegisform');
    });
    $('.showCancelRegisForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showCancelRegisForm').toggle('hide-CancelRegisform');
        }
    });
    $('.yesBtnCancelRegis').click(function () {
        var form = document.getElementById("formCancelRegis");
        form.submit();
    });
});


$('#btnCre').click(function () {
    $('#btnCre').attr('disabled', 'disabled');
    $('#btnCre').css('cursor', 'not-allowed');
    $('#btnCre').css('opacity', '0.5');
    var form = document.getElementById("submitFormRegis");
    form.submit();
})


/*warning because do not have enough members*/
$(document).ready(function () {
    var groupId;
    $('.submitFormRegis').click(function () {
        groupId = this.id;
        AjaxCall('/MyGroup/CheckIfGroupIsFull', JSON.stringify(groupId), "POST").done(function (response) {
            if (response == true) {
                //case full of member
                $('.showForm').toggle('hide-form');
            } else {
                //case not full
                $('.showWarningForm').toggle('hide-Warningform');
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
    $('.noBtnWarning').click(function () {
        $('.showWarningForm').toggle('hide-Warningform');
    });
    $('.showWarningForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showWarningForm').toggle('hide-Warningform');
        }
    });
    $('.yesBtnWarning').click(function () {
        $('.showWarningForm').toggle('hide-Warningform');
        $('.showForm').toggle('hide-form');
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